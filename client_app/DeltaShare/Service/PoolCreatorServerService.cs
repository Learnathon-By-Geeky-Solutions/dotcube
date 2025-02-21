using System.Diagnostics;
using System.Net;
using System.Text.Json;
using DeltaShare.Model;
using DeltaShare.Util;
using MimeKit;

namespace DeltaShare.Service
{
    public partial class PoolCreatorServerService : IDisposable
    {
        private CancellationTokenSource? cancellationTokenSource;
        private Task? listenTask;
        private readonly HashSet<string> poolUsersIpHashSet = [];
        private readonly HttpListener listener;

        public PoolCreatorServerService(HttpListener listener)
        {
            string username = Preferences.Get(Constants.UsernameKey, "");
            string fullname = Preferences.Get(Constants.FullNameKey, "");
            User currentUser = new(fullname, "", username, "", true);
            poolUsersIpHashSet.Add(currentUser.IpAddress);

            StateManager.PoolUsers.Add(currentUser);
            StateManager.PoolCreatorIpAddress = Constants.PoolCreatorIpAddress;
            StateManager.IpAddress = Constants.PoolCreatorIpAddress;
            StateManager.IsPoolCreator = true;
            StateManager.CurrentUser = currentUser;
            this.listener = listener;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopListening();
                cancellationTokenSource?.Dispose();
            }
        }

        public void StartListening()
        {
            cancellationTokenSource = new CancellationTokenSource();
            listenTask = Task.Run(() => Listen(cancellationTokenSource.Token));
        }

        public void StopListening()
        {
            cancellationTokenSource?.Cancel();
            listenTask?.Wait();
        }

        private async Task Listen(CancellationToken cancellationToken)
        {
            int maxConcurrentRequests = 10;
            try
            {
                Debug.WriteLine($"prefix: {listener.Prefixes}");
                listener.Start();
            }
            catch (HttpListenerException ex)
            {
                Debug.WriteLine($"Error starting listener: {ex.Message}");
                return;
            }

            HashSet<Task> requests = new();
            for (int requestIdx = 0; requestIdx < maxConcurrentRequests; requestIdx++)
            {
                requests.Add(listener.GetContextAsync());
            }
            while (!cancellationToken.IsCancellationRequested)
            {
                Task task = await Task.WhenAny(requests);
                requests.Remove(task);

                if (task is Task<HttpListenerContext> contextTask)
                {
                    HttpListenerContext ctx = await contextTask;
                    requests.Add(ProcessRequestAsync(ctx));
                    requests.Add(listener.GetContextAsync());
                }
            }

            listener.Stop();
            listener.Close();
        }

        private async Task ProcessRequestAsync(HttpListenerContext context)
        {
            if (context.Request.Url?.AbsolutePath == Constants.NewClientPath)
            {
                await ProcessNewUserRequest(context);
            }
            else if (context.Request.Url?.AbsolutePath == Constants.NewFileMetadataPath)
            {
                await ProcessNewFileRequest(context);
            }
        }

        private async Task ProcessNewFileRequest(HttpListenerContext context)
        {
            try
            {
                Dictionary<string, MimePart> formParts = await MultipartParser.Parse(context, Constants.NewFileMetadataPath);
                var fileJsonStream = formParts[Constants.UserFilesJsonField].Content.Stream;
                List<FileMetadata>? allFileMetadata = await JsonSerializer.DeserializeAsync<List<FileMetadata>>(fileJsonStream);
                string clientIpAddress = context.Request.RemoteEndPoint.Address.ToString();

                foreach (var fileMetadata in allFileMetadata ?? [])
                {
                    Stream thumbnailStream = formParts[fileMetadata.Uuid].Content.Stream;

                    using var memoryStream = new MemoryStream();
                    await thumbnailStream.CopyToAsync(memoryStream);
                    ByteArrayContent thumbnailContent = new(memoryStream.ToArray());

                    fileMetadata.ThumbnailContent = thumbnailContent;
                    fileMetadata.ThumbnailContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(fileMetadata.ContentType);
                    fileMetadata.ThumbnailSource = ImageSource.FromStream(() => thumbnailContent.ReadAsStream());
                    fileMetadata.OwnerIpAddress = clientIpAddress;
                    fileMetadata.Owner = StateManager.IpUserPair[clientIpAddress];
                    StateManager.PoolFiles.Add(fileMetadata);
                }
                MultipartParser.SendResponse(context, "success");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error processing request in server: {ex.Message}");
                MultipartParser.SendResponse(context, "error");
            }
        }

        private async Task ProcessNewUserRequest(HttpListenerContext context)
        {
            User? newUser = new("", "", "", "", false);
            try
            {
                Dictionary<string, MimePart> formParts = await MultipartParser.Parse(context, Constants.NewClientPath);
                var newUserJsonStream = formParts[Constants.UserJsonField].Content.Stream;
                newUser = await JsonSerializer.DeserializeAsync<User>(newUserJsonStream);
                newUser!.IpAddress = context.Request.RemoteEndPoint.Address.ToString();
                newUser!.IsAdmin = false;

                Debug.WriteLine($"Received new user: {newUser}");
                if (poolUsersIpHashSet.Contains(newUser.IpAddress))
                {
                    MultipartParser.SendResponse(context, "User already in pool");
                    return;
                }
                StateManager.PoolUsers.Add(newUser);
                poolUsersIpHashSet.Add(newUser.IpAddress);
                StateManager.IpUserPair[newUser.IpAddress] = newUser;
                MultipartParser.SendResponse(context, $"success {newUser?.IpAddress}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error processing request in server: {ex.Message}");
                MultipartParser.SendResponse(context, "error");
            }
        }
    }
}

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using DeltaShare.Model;
using DeltaShare.Util;
using MimeKit;

namespace DeltaShare.Service
{
    public sealed class PoolCreatorServerService : IDisposable
    {
        private CancellationTokenSource? cancellationTokenSource;
        private Task? listenTask;
        private readonly HashSet<string> poolUsersIpHashSet = new();
        public ObservableCollection<User> PoolUsers { get; set; } = new();

        public PoolCreatorServerService()
        {
            string username = Preferences.Get(Constants.UsernameKey, "");
            string fullname = Preferences.Get(Constants.FullNameKey, "");
            User currentUser = new(fullname, "", username, "", true);
            poolUsersIpHashSet.Add(currentUser.IpAddress);
            PoolUsers.Add(currentUser);
        }

        public void Dispose()
        {
            cancellationTokenSource?.Dispose();
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
            string prefix = $"http://{Constants.PoolCreatorIpAddress}:{Constants.Port}/";
            int maxConcurrentRequests = 10;
            HttpListener listener = new();
            listener.Prefixes.Add(prefix);
            listener.Start();

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
            try
            {
                Dictionary<string, MimePart> formParts = await MultipartParser.Parse(context, "/clients");

                User user = new(
                    await new StreamReader(formParts["Name"].Content.Stream).ReadToEndAsync(),
                    await new StreamReader(formParts["Email"].Content.Stream).ReadToEndAsync(),
                    await new StreamReader(formParts["Username"].Content.Stream).ReadToEndAsync(),
                    context.Request.RemoteEndPoint.Address.ToString(),
                    false
                );
                Debug.WriteLine($"Received new user: {user}");
                if (poolUsersIpHashSet.Contains(user.IpAddress))
                {
                    MultipartParser.SendResponse(context, "User already in pool");
                    return;
                }
                PoolUsers.Add(user);
                poolUsersIpHashSet.Add(user.IpAddress);
                MultipartParser.SendResponse(context, "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error processing request: {ex.Message}");
            }
        }

    }
}

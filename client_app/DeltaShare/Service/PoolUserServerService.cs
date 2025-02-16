using System.Diagnostics;
using System.Net;
using System.Text.Json;
using DeltaShare.Model;
using DeltaShare.Util;
using MimeKit;

namespace DeltaShare.Service
{
    public sealed class PoolUserServerService : IDisposable
    {
        private static string ClientSyncPath = "/clients-sync";
        private CancellationTokenSource? cancellationTokenSource;
        private Task? listenTask;
        private readonly HttpListener listener;

        public PoolUserServerService(HttpListener listener)
        {
            this.listener = listener;
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
            int maxConcurrentRequests = 10;
            try
            {
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
            if (context.Request.Url?.AbsolutePath == ClientSyncPath)
            {
                await ProcessUserSyncRequest(context);
            }
        }

        private async Task ProcessUserSyncRequest(HttpListenerContext context)
        {
            try
            {
                Dictionary<string, MimePart> formParts = await MultipartParser.Parse(context, ClientSyncPath);
                var userListJsonStream = formParts["AllUsersJson"].Content.Stream;
                List<User>? allUsers = await JsonSerializer.DeserializeAsync<List<User>>(userListJsonStream);
                StateManager.PoolUsers.Clear();
                foreach (User user in allUsers)
                {
                    StateManager.PoolUsers.Add(user);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error processing request in client: {ex.Message}");
            }
            finally
            {
                MultipartParser.SendResponse(context, "success");
            }
        }
    }
}

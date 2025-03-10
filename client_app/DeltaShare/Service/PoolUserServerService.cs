﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using DeltaShare.Model;
using DeltaShare.Util;
using MimeKit;

namespace DeltaShare.Service
{
    public partial class PoolUserServerService(HttpListener listener) : IDisposable
    {
        private CancellationTokenSource? cancellationTokenSource;
        private Task? listenTask;
        private readonly HttpListener listener = listener;

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
                listener.Start();
            }
            catch (HttpListenerException ex)
            {
                Debug.WriteLine($"Error starting listener: {ex.Message}");
                return;
            }

            HashSet<Task> requests = [];
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

        private static async Task ProcessRequestAsync(HttpListenerContext context)
        {
            if (context.Request.Url?.AbsolutePath == Constants.ClientsSyncPath)
            {
                await ProcessUserSyncRequest(context);
            }
            else if (context.Request.Url?.AbsolutePath == Constants.FilesSyncPath)
            {
                await ProcessFilesSyncRequest(context);
            }
            else if (context.Request.Url?.AbsolutePath == Constants.FileDownloadPath)
            {
                await FileHandler.ProcessFileDownloadRequest(context);
            }
        }

        private static async Task ProcessFilesSyncRequest(HttpListenerContext context)
        {
            try
            {
                Dictionary<string, MimePart> formParts = await MultipartParser.Parse(context, Constants.FilesSyncPath);
                var fileListJsonStream = formParts[Constants.AllFilesJsonField].Content.Stream;
                List<FileMetadata>? allFiles = await JsonSerializer.DeserializeAsync<List<FileMetadata>>(fileListJsonStream);
                ObservableCollection<FileMetadata> newFiles = new();
                foreach (FileMetadata fileMetadata in allFiles ?? [])
                {
                    Stream thumbnailStream = formParts[fileMetadata.Uuid].Content.Stream;

                    using var memoryStream = new MemoryStream();
                    await thumbnailStream.CopyToAsync(memoryStream);
                    ByteArrayContent thumbnailContent = new(memoryStream.ToArray());

                    fileMetadata.Owner = StateManager.IpToUserMap[fileMetadata.OwnerIpAddress];
                    fileMetadata.ThumbnailContent = thumbnailContent;
                    fileMetadata.ThumbnailContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(fileMetadata.ContentType);
                    newFiles.Add(fileMetadata);
                }

                StateManager.PoolFiles.Clear();
                foreach (FileMetadata file in newFiles)
                {
                    StateManager.PoolFiles.Add(file);
                }

                MultipartParser.SendResponse(context, "success");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error processing request in server: {ex.Message}");
            }
        }

        private static async Task ProcessUserSyncRequest(HttpListenerContext context)
        {
            try
            {
                Dictionary<string, MimePart> formParts = await MultipartParser.Parse(context, Constants.ClientsSyncPath);
                var userListJsonStream = formParts[Constants.AllUsersJsonField].Content.Stream;
                List<User>? allUsers = await JsonSerializer.DeserializeAsync<List<User>>(userListJsonStream);
                StateManager.PoolUsers.Clear();
                foreach (User user in allUsers ?? [])
                {
                    StateManager.PoolUsers.Add(user);
                    StateManager.IpToUserMap[user.IpAddress] = user;
                }
                MultipartParser.SendResponse(context, "success");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error processing request in client: {ex.Message}");
            }
        }
    }
}

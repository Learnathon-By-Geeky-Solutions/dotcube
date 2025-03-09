using System.Diagnostics;
using System.Text.Json;
using DeltaShare.Model;
using DeltaShare.Util;

namespace DeltaShare.Service
{
    public class PoolCreatorClientService(HttpClient client)
    {
        private readonly HttpClient client = client;

        public async Task SendAllFileInfoToAllUsers()
        {
            string allFilesJson = JsonSerializer.Serialize(StateManager.PoolFiles);
            foreach (var user in StateManager.PoolUsers)
            {
                if (user.IsAdmin)
                {
                    continue;
                }
                using var form = new MultipartFormDataContent
                {
                    { new StringContent(allFilesJson), Constants.AllFilesJsonField }
                };
                foreach (FileMetadata file in StateManager.PoolFiles)
                {
                    ByteArrayContent thumbnailContent = new(await file.ThumbnailContent!.ReadAsByteArrayAsync());
                    form.Add(thumbnailContent, file.Uuid, file.Filename);
                }
                try
                {
                    HttpResponseMessage response = await client.PostAsync(
                        $"http://{user.IpAddress}:{Constants.Port}{Constants.FilesSyncPath}",
                        //$"https://webhook.site/4d020842-8d4d-4b4f-be34-5a406022f6ac/files-sync",
                        form);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"response: {responseBody}");
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Error: {e.Message}");
                }
            }
        }
        public async Task SendAllUserInfoToAllUsers()
        {
            string allUsersJson = JsonSerializer.Serialize(StateManager.PoolUsers);
            foreach (var user in StateManager.PoolUsers)
            {
                if (user.IsAdmin)
                {
                    continue;
                }
                using var form = new MultipartFormDataContent
                {
                    { new StringContent(allUsersJson), Constants.AllUsersJsonField }
                };
                try
                {
                    HttpResponseMessage response = await client.PostAsync(
                        $"http://{user.IpAddress}:{Constants.Port}{Constants.ClientsSyncPath}",
                        //$"https://webhook.site/686e76ce-1f60-4924-b364-4ceb2e5823a1/{user.IpAddress}-{Constants.Port}/clients-sync",
                        form);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"response: {responseBody}");
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Error: {e.Message}");
                }
            }
        }
    }
}

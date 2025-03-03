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
                string url = $"http://{user.IpAddress}:{Constants.Port}{Constants.FilesSyncPath}";
                //string url = $"https://webhook.site/46b466db-bab6-4fd6-8e89-d687741832e3/{user.IpAddress}-{Constants.Port}/files-sync";
                using var form = new MultipartFormDataContent
                {
                    { new StringContent(allFilesJson), Constants.AllFilesJsonField }
                };
                foreach (FileMetadata file in StateManager.PoolFiles)
                {
                    form.Add(file.ThumbnailContent!, file.Uuid, file.Filename);
                }
                try
                {
                    HttpResponseMessage response = await client.PostAsync(url, form);
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
                string url = $"http://{user.IpAddress}:{Constants.Port}{Constants.ClientsSyncPath}";
                //string url = $"https://webhook.site/686e76ce-1f60-4924-b364-4ceb2e5823a1/{user.IpAddress}-{Constants.Port}/clients-sync";
                using var form = new MultipartFormDataContent
                {
                    { new StringContent(allUsersJson), Constants.AllUsersJsonField }
                };
                try
                {
                    HttpResponseMessage response = await client.PostAsync(url, form);
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

using System.Diagnostics;
using System.Text.Json;
using DeltaShare.Util;

namespace DeltaShare.Service
{
    public class PoolCreatorClientService
    {
        private readonly HttpClient client;
        public PoolCreatorClientService(HttpClient client)
        {
            this.client = client;
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
                string url = $"http://{user.IpAddress}:{Constants.Port}/clients-sync";
                //string url = $"https://webhook.site/686e76ce-1f60-4924-b364-4ceb2e5823a1/{user.IpAddress}-{Constants.Port}/clients-sync";
                using var form = new MultipartFormDataContent
                {
                    { new StringContent(allUsersJson), "AllUsersJson" }
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

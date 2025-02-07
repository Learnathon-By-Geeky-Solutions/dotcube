using System.Diagnostics;

namespace DeltaShare.Service
{
    public class PoolJoinService
    {

        private readonly HttpClient client;

        public PoolJoinService(HttpClient client)
        {
            this.client = client;
        }
        public async Task SendInfoToPoolCreator(string poolCode)
        {
            string url = $"http://{poolCode}:{Constants.Port}/clients";
            using var form = new MultipartFormDataContent
            {
                { new StringContent(Preferences.Get(Constants.FullNameKey, "")), "Name" },
                { new StringContent("null"), "Email" },
                { new StringContent(Preferences.Get(Constants.UsernameKey, "")), "Username" }
            };

            try
            {
                HttpResponseMessage response = await client.PostAsync(url, form);
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"response: {responseBody}");
            }
            catch
            (Exception e)
            {
                Debug.WriteLine($"Error: {e.Message}");
            }
        }
    }
}

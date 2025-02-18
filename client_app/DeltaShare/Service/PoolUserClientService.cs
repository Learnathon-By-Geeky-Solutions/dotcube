using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using DeltaShare.Model;
using DeltaShare.Util;

namespace DeltaShare.Service
{
    public class PoolUserClientService(HttpClient client)
    {
        private readonly HttpClient client = client;

        public async Task<bool> SendUserInfoToPoolCreator(string poolCode)
        {
            string url = $"http://{poolCode}:{Constants.Port}{Constants.NewClientPath}";
            User currentUser = new(
                Preferences.Get(Constants.FullNameKey, ""),
                "null",
                Preferences.Get(Constants.UsernameKey, ""),
                "null",
                false);
            string currentUserJson = JsonSerializer.Serialize(currentUser);
            using var form = new MultipartFormDataContent
            {
                { new StringContent(currentUserJson), Constants.UserJsonField },
            };

            try
            {
                HttpResponseMessage response = await client.PostAsync(url, form);
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"response: {responseBody}");

                StateManager.PoolCreatorIpAddress = Constants.PoolCreatorIpAddress;
                StateManager.IsPoolCreator = false;

                bool isSuccessful = responseBody.Contains("success");
                if (!isSuccessful)
                {
                    return false;
                }
                string ipAddress = responseBody.Split(" ")[1];
                StateManager.IpAddress = ipAddress;
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error: {e.Message}");
                return false;
            }
        }

        public void AddFilesToPool(ObservableCollection<FileMetadata> fileMetadata)
        {
            foreach (FileMetadata file in fileMetadata)
            {
                StateManager.PoolFiles.Add(file);
            }
        }

        public async Task SendFileInfoToPoolCreator(ObservableCollection<FileMetadata> fileMetadata)
        {

            // send to creator
            string filesJson = JsonSerializer.Serialize(fileMetadata);
            using var form = new MultipartFormDataContent
            {
                { new StringContent(filesJson), Constants.UserFilesJsonField },
            };
            try
            {
                HttpResponseMessage response = await client.PostAsync(
                $"http://{StateManager.PoolCreatorIpAddress}:{Constants.Port}{Constants.NewFileMetadataPath}",
                    //$"https://webhook.site/686e76ce-1f60-4924-b364-4ceb2e5823a1/newFiles",
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

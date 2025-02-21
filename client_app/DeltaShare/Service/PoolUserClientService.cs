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

        public async Task<bool> SendUserInfoToPoolCreator(string poolCreatorIpAddress)
        {
            string url = $"http://{poolCreatorIpAddress}:{Constants.Port}{Constants.NewClientPath}";
            Debug.WriteLine($"posting: {url}");
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

                StateManager.PoolCreatorIpAddress = poolCreatorIpAddress;
                StateManager.IsPoolCreator = false;

                bool isSuccessful = responseBody.Contains("success");
                if (!isSuccessful)
                {
                    return false;
                }
                string ipAddress = responseBody.Split(" ")[1];
                StateManager.IpAddress = ipAddress;
                StateManager.CurrentUser = currentUser;
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
            foreach (FileMetadata file in fileMetadata)
            {
                form.Add(file.ThumbnailContent, file.Uuid, file.Filename);
            }
            try
            {
                Debug.WriteLine($"posting: http://{StateManager.PoolCreatorIpAddress}:{Constants.Port}{Constants.NewFileMetadataPath}");
                HttpResponseMessage response = await client.PostAsync(
                    $"http://{StateManager.PoolCreatorIpAddress}:{Constants.Port}{Constants.NewFileMetadataPath}",
                    //$"https://webhook.site/02e7186e-d565-4711-b0d1-2708817b802f/newFiles",
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

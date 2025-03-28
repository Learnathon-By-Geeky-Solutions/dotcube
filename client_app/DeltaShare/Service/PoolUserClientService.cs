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
                { new StringContent(poolCreatorIpAddress), Constants.PoolCreatorIpField },
            };

            try
            {
                HttpResponseMessage response = await client.PostAsync(
                     $"http://{poolCreatorIpAddress}:{Constants.Port}{Constants.NewClientPath}",
                     form);
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"response: {responseBody}");
                bool isSuccessful = responseBody.Contains("success");
                if (!isSuccessful)
                {
                    return false;
                }

                StateManager.PoolCreatorIpAddress = poolCreatorIpAddress;
                StateManager.IsPoolCreator = false;

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

        public async Task SaveFilesFromPool(ObservableCollection<object> files)
        {
            ObservableCollection<object> selectedFiles = new(files);

            foreach (FileMetadata file in selectedFiles.Cast<FileMetadata>())
            {
                Debug.WriteLine($"Downloading {file.Filename}");
                HttpContent? content = await GetRemoteFileContent(file);
                if (content == null)
                {

                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await Alert.Show($"content null for {file.Filename}");
                    });
                    continue;
                }
                await FileHandler.SaveFileInLocalStorage(content, file);
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Alert.Show($"Downloaded {file.Filename}");
                });
            }
        }
        public async Task<HttpContent?> GetRemoteFileContent(FileMetadata fileMetadata)
        {
            MultipartFormDataContent form = new()
            {
                { new StringContent(fileMetadata.Uuid), Constants.FileUuidField  }
            };
            try
            {
                HttpResponseMessage response = await client.PostAsync(
                    $"http://{fileMetadata.OwnerIpAddress}:{Constants.Port}{Constants.FileDownloadPath}",
                    form);
                return response.Content;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error: {e.Message}");
                return null;
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
                ByteArrayContent thumbnailContent = new(await file.ThumbnailContent!.ReadAsByteArrayAsync());
                form.Add(thumbnailContent, file.Uuid, file.Filename);
            }
            try
            {
                Debug.WriteLine($"posting: http://{StateManager.PoolCreatorIpAddress}:{Constants.Port}{Constants.NewFileMetadataPath}");
                HttpResponseMessage response = await client.PostAsync(
                    $"http://{StateManager.PoolCreatorIpAddress}:{Constants.Port}{Constants.NewFileMetadataPath}",
                    //$"https://webhook.site/4d020842-8d4d-4b4f-be34-5a406022f6ac/newfiles",
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

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using DeltaShare.Model;
using MimeKit;
using SkiaSharp;

namespace DeltaShare.Util
{
    public static class FileHandler
    {
        public static async Task SaveFileInLocalStorage(Stream fileStream, string fileName)
        {
            string downloadFolderPath = String.Empty;
#if WINDOWS
            downloadFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
#endif
#if ANDROID
            downloadFolderPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)!.AbsolutePath;
#endif
            string destinationPath = Path.Combine(downloadFolderPath, "DeltaShare");
            Directory.CreateDirectory(destinationPath);
            using FileStream destinationStream = new(Path.Combine(destinationPath, fileName), FileMode.Create);
            await fileStream.CopyToAsync(destinationStream);
        }

        public static async Task ProcessFileDownloadRequest(HttpListenerContext context)
        {
            try
            {
                Dictionary<string, MimePart> formParts = await MultipartParser.Parse(context, Constants.FileDownloadPath);
                using StreamReader reader = new(formParts[Constants.FileUuidField].Content.Stream);
                string fileUuid = await reader.ReadToEndAsync();
                FileMetadata? fileMetadata = StateManager.PoolFiles.FirstOrDefault(file => file.Uuid == fileUuid);
                if (fileMetadata == null)
                {
                    MultipartParser.SendResponse(context, "error");
                    return;
                }
                Stream fileStream = await fileMetadata.FileRef!.OpenReadAsync();
                await MultipartParser.SendResponse(context, fileStream ?? Stream.Null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error processing request in server: {ex.Message}");
                MultipartParser.SendResponse(context, "error");
            }
        }

        public static async Task<IEnumerable<FileResult>> PickFiles()
        {
            try
            {
                PickOptions options = new()
                {
                    PickerTitle = "Please select a file",
                };
                IEnumerable<FileResult> result = await FilePicker.PickMultipleAsync(options);
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return [];
        }
        public static async Task<ObservableCollection<FileMetadata>> FileResultsToFileMetadata(IEnumerable<FileResult> fileResults)
        {
            ObservableCollection<FileMetadata> files = new();
            foreach (FileResult file in fileResults)
            {
                Stream fileStream = await file.OpenReadAsync();
                FileMetadata newFile = new(
                    uuid: Guid.NewGuid().ToString(),
                    thumbnailContent: await MakeThumbnailContent(file),
                    size: fileStream.Length,
                    filename: file.FileName,
                    ownerIpAddress: "null",
                    contentType: file.ContentType,
                    fileRef: file)
                {
                    Owner = StateManager.CurrentUser
                };
                files.Add(newFile);
            }

            return files;
        }


        private static async Task<ByteArrayContent> MakeThumbnailContent(FileResult fileResult)
        {
            SKBitmap originalBitmap;

            if (fileResult.ContentType.Contains("image"))
            {
                Stream fileStream = await fileResult.OpenReadAsync();
                originalBitmap = SKBitmap.Decode(SKCodec.Create(fileStream));
            }
            else
            {
                using var tempStream = await FileSystem.OpenAppPackageFileAsync("generic_file.png");
                originalBitmap = SKBitmap.Decode(SKCodec.Create(tempStream));
            }

            int newHeight = 100;
            int newWidth = (int)(originalBitmap.Width * (newHeight / (double)originalBitmap.Height));

            using var resizedBitmap = new SKBitmap(newWidth, newHeight);
            originalBitmap.ScalePixels(resizedBitmap, new SKSamplingOptions(SKFilterMode.Nearest, SKMipmapMode.Nearest));

            using var image = SKImage.FromBitmap(resizedBitmap);
            using var encodedData = image.Encode(SKEncodedImageFormat.Png, 100);

            byte[] imageBytes = encodedData.ToArray();
            var content = new ByteArrayContent(imageBytes);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

            return content;
        }

    }
}

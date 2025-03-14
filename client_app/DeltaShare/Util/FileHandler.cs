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
        public static async Task SaveFileInLocalStorage(HttpContent content, FileMetadata file)
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
            using FileStream destinationStream = new(Path.Combine(destinationPath, file.Filename), FileMode.Create);

            using Stream contentStream = await content.ReadAsStreamAsync();
            byte[] buffer = new byte[8192];
            long totalRead = 0;
            int read;
            Stopwatch stopwatch = Stopwatch.StartNew();

            file.IsDownloading = true;
            while ((read = await contentStream.ReadAsync(buffer)) > 0)
            {
                await destinationStream.WriteAsync(buffer.AsMemory(0, read));
                totalRead += read;
                file.DownloadedSize = totalRead;

                //double speedInKbps = totalRead / stopwatch.Elapsed.TotalSeconds / 1024;

                //Debug.WriteLine($"Downloaded: {totalRead / 1024} KB / {totalBytes / 1024} KB | {percentage:F2}% | {speedInKbps:F2} KB/s format:{file.FormattedDownloadedSize}");
            }
            file.IsDownloading = false;

            stopwatch.Stop();
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
                using FileStream fileStream = File.OpenRead(fileMetadata.FilePath);
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
            ObservableCollection<FileMetadata> files = [];
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
                    filePath: file.FullPath);
                files.Add(newFile);
            }

            return files;
        }

        public static async Task<ByteArrayContent> MakeThumbnailContent(FileResult fileResult)
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

using System.Collections.ObjectModel;
using System.Diagnostics;
using DeltaShare.Model;
using SkiaSharp;

namespace DeltaShare.Util
{
    public static class FileHandler
    {
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
                FileMetadata newFile = new FileMetadata(
                    uuid: Guid.NewGuid().ToString(),
                    thumbnailContent: await MakeThumbnailContent(fileStream, file.ContentType),
                    size: fileStream.Length,
                    filename: file.FileName,
                    ownerIpAddress: "null",
                    contentType: file.ContentType,
                    fileStream: fileStream);
                newFile.Owner = StateManager.CurrentUser;
                files.Add(newFile);
            }

            return files;
        }


        private static async Task<ByteArrayContent> MakeThumbnailContent(Stream fileStream, string contentType)
        {
            SKBitmap originalBitmap;

            if (contentType.Contains("image"))
            {
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

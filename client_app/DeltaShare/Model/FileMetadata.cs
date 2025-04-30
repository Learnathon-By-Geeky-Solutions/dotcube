using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using DeltaShare.Util;

namespace DeltaShare.Model
{
    public partial class FileMetadata : ObservableObject
    {
        [ObservableProperty]
        [JsonIgnore]
        private bool isDownloading = false;

        [ObservableProperty]
        [JsonIgnore]
        private bool isDownloaded = false;

        [ObservableProperty]
        [JsonIgnore]
        [NotifyPropertyChangedFor(nameof(FormattedDownloadedSize), nameof(DownloadProgress))]
        private long downloadedSize = 0;

        [JsonIgnore]
        public ByteArrayContent? ThumbnailContent { get; set; }

        [JsonIgnore]
        public User? Owner { get; set; }

        [JsonIgnore]
        public ImageSource ThumbnailSource
        {
            get
            {
                if (ThumbnailContent == null)
                {
                    FileResult fileResult = new("null")
                    {
                        ContentType = "generic"
                    };
                    ThumbnailContent = FileHandler.MakeThumbnailContent(fileResult).Result;
                }
                byte[] imageBytes = ThumbnailContent!.ReadAsByteArrayAsync().Result;
                return ImageSource.FromStream(() => new MemoryStream(imageBytes));
            }
        }

        [JsonIgnore]
        public string FormattedDownloadedSize
        {
            get
            {
                if (DownloadedSize >= 1024 * 1024)
                    return $"{DownloadedSize / (1024 * 1024)} MB";
                if (DownloadedSize >= 1024)
                    return $"{DownloadedSize / 1024} KB";
                return $"{DownloadedSize} bytes";
            }
        }

        [JsonIgnore]
        public double DownloadProgress
        {
            get
            {
                if (Size == 0)
                    return 0f;
                return (double)DownloadedSize / Size;
            }
        }

        [JsonIgnore]
        public string FormattedSize
        {
            get
            {
                if (Size >= 1024 * 1024)
                    return $"{Size / (1024 * 1024)} MB";
                if (Size >= 1024)
                    return $"{Size / 1024} KB";
                return $"{Size} bytes";
            }
        }

        public string FilePath { get; set; }
        public string Uuid { get; set; }
        public long Size { get; set; }
        public string Filename { get; set; }
        public string OwnerIpAddress { get; set; } // compute on server
        public string ContentType { get; set; }

        [JsonConstructor]
        public FileMetadata(string uuid, long size, string filename, string ownerIpAddress, string contentType, string filePath)
        {
            Uuid = uuid;
            Size = size;
            Filename = filename;
            OwnerIpAddress = ownerIpAddress;
            ContentType = contentType;
            FilePath = filePath;
        }

        public FileMetadata(string uuid, ByteArrayContent thumbnailContent, long size, string filename, string ownerIpAddress, string contentType, string filePath)
        {
            Uuid = uuid;
            ThumbnailContent = thumbnailContent;
            ThumbnailContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            Size = size;
            Filename = filename;
            OwnerIpAddress = ownerIpAddress;
            ContentType = contentType;
            FilePath = filePath;
        }

        public override string ToString()
        {
            return $"Uuid: {Uuid}, Size: {Size}, Filename: {Filename}, OwnerIpAddress: {OwnerIpAddress}, Filepath: {FilePath}";
        }
    }

}

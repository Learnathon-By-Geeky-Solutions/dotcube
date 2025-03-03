using System.Text.Json.Serialization;

namespace DeltaShare.Model
{
    public class FileMetadata
    {

        [JsonIgnore]
        public ByteArrayContent? ThumbnailContent { get; set; }

        [JsonIgnore]
        public FileResult? FileRef { get; set; }

        [JsonIgnore]
        public User? Owner { get; set; }

        [JsonIgnore]
        public ImageSource ThumbnailSource { get; set; }

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

        public string Uuid { get; set; }
        public long Size { get; set; }
        public string Filename { get; set; }
        public string OwnerIpAddress { get; set; } // compute on server
        public string ContentType { get; set; }

        [JsonConstructor]
        public FileMetadata(string uuid, long size, string filename, string ownerIpAddress, string contentType)
        {
            Uuid = uuid;
            Size = size;
            Filename = filename;
            OwnerIpAddress = ownerIpAddress;
            ContentType = contentType;
        }

        public FileMetadata(string uuid, ByteArrayContent thumbnailContent, long size, string filename, string ownerIpAddress, string contentType, FileResult fileRef)
        {
            Uuid = uuid;
            ThumbnailContent = thumbnailContent;
            ThumbnailContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            byte[] imageBytes = ThumbnailContent.ReadAsByteArrayAsync().Result;
            ThumbnailSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
            Size = size;
            Filename = filename;
            OwnerIpAddress = ownerIpAddress;
            ContentType = contentType;
            FileRef = fileRef;
        }

        public override string ToString()
        {
            return $"Uuid: {Uuid}, Size: {Size}, Filename: {Filename}, OwnerIpAddress: {OwnerIpAddress}";
        }
    }

}

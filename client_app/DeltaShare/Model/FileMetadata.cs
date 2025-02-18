using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace DeltaShare.Model
{
    public class FileMetadata(string uuid, string thumbnail, long size, string filename, string ownerIpAddress, Stream fileStream)
    {
        public string Uuid { get; set; } = uuid;
        public string Thumbnail { get; set; } = thumbnail;
        public long Size { get; set; } = size;
        public string Filename { get; set; } = filename;
        public string OwnerIpAddress { get; set; } = ownerIpAddress; // compute
        [JsonIgnore]
        public Stream? FileStream { get; set; } = fileStream; // compute

        public override string ToString()
        {
            return "Uuid: " + Uuid + ", Thumbnail (size): " + Thumbnail.Length + ", Size: " + Size + ", Filename: " + Filename + ", OwnerIpAddress: " + OwnerIpAddress;
        }

        public static async Task<ObservableCollection<FileMetadata>> FileResultsToFileMetadata(IEnumerable<FileResult> fileResults)
        {
            ObservableCollection<FileMetadata> files = new();
            foreach (FileResult file in fileResults)
            {
                Stream fileStream = await file.OpenReadAsync();
                files.Add(new FileMetadata(
                    uuid: Guid.NewGuid().ToString(),
                    thumbnail: "testbase64data",
                    size: fileStream.Length,
                    filename: file.FileName,
                    ownerIpAddress: "null",
                    fileStream: fileStream));
            }

            return files;
        }
    }
}

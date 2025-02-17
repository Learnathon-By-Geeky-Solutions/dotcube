using System.Numerics;

namespace DeltaShare.Model
{
    public class FileMetadata(string uuid, string thumbnail, BigInteger size, string filename, string ownerIpAddress, Stream fileStream)
    {
        public string Uuid { get; set; } = uuid;
        public string Thumbnail { get; set; } = thumbnail;
        public BigInteger Size { get; set; } = size;
        public string Filename { get; set; } = filename;
        public string OwnerIpAddress { get; set; } = ownerIpAddress; // compute
        public Stream? FileStream { get; set; } = fileStream; // compute

        public override string ToString()
        {
            return "Uuid: " + Uuid + ", Thumbnail (size): " + Thumbnail.Length + ", Size: " + Size + ", Filename: " + Filename + ", OwnerIpAddress: " + OwnerIpAddress;
        }
    }
}

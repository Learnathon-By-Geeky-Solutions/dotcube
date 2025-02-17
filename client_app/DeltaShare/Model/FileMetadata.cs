using System.Numerics;

namespace DeltaShare.Model
{
    public class FileMetadata
    {
        public string Uuid { get; set; }
        public string Thumbnail { get; set; }
        public BigInteger Size { get; set; }
        public string Filename { get; set; }
        public string OwnerIpAddress { get; set; }
        public Stream Stream { get; set; }


        public override string ToString()
        {
            return "Uuid: " + Uuid + ", Thumbnail (size): " + Thumbnail.Length + ", Size: " + Size + ", Filename: " + Filename + ", OwnerIpAddress: " + OwnerIpAddress;
        }
    }
}

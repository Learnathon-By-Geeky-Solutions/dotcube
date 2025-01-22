using ZXing;
using ZXing.QrCode;
using ZXing.Rendering;

namespace DeltaShare.Service
{
    public interface IQRCodeService
    {
        public ImageSource GenerateQRCode(string data, int height, int width, int margin);
    }
    public class QRCodeService : IQRCodeService
    {
        public ImageSource GenerateQRCode(string data, int height, int width, int margin)
        {
            BarcodeWriterPixelData writerPixelData = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = margin,
                    CharacterSet = "UTF-8"
                }
            };
            PixelData pixelData = writerPixelData.Write(data);
            ImageSource imageSource = ImageSource.FromStream(
                () => new MemoryStream(pixelData.Pixels));
            return imageSource;
        }

    }
}

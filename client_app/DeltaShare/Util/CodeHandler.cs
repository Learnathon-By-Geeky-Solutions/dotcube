namespace DeltaShare.Util
{
    public static class CodeHandler
    {
        public static string GenerateQrCodeData(string ipAddress)
        {
            return $"[deltashare] {ipAddress}";
        }

        public static string ExtractIpAddressFromQrCodeData(string qrCodeData)
        {
            return qrCodeData.Replace("[deltashare] ", "");
        }
    }
}

using System.Text;

namespace DeltaShare.Util
{
    public static class PoolCodeHandler
    {
        public static string GenerateQrCodeData()
        {
            IEnumerable<string> localIps = NetworkHandler.GetLocalIps();
            List<string> result = [];
            foreach (string ip in localIps)
            {
                if (ip.Equals("127.0.0.1") || ip.Equals("0.0.0.0"))
                {
                    continue;
                }
                result.Add(ip);
            }
            string base64PoolCode = Convert.ToBase64String(
                Encoding.UTF8.GetBytes(
                    string.Join(" ", result)
                )
            );

            return $"ds://{base64PoolCode}";
        }

        public static IEnumerable<string> DecodePoolCodeData(string qrCodeData)
        {
            if (!qrCodeData.StartsWith("ds://"))
                throw new ArgumentException("Invalid QR code data");

            string base64 = qrCodeData.Substring(5);
            string poolCode = Encoding.UTF8.GetString(Convert.FromBase64String(base64));

            return poolCode.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }
    }
}

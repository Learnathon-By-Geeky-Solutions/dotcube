using System.Net;

namespace DeltaShare.Util
{
    public static class PoolCodeHandler
    {
        private static uint IpToInt(string ip)
        {
            byte[] bytes = IPAddress.Parse(ip).GetAddressBytes();
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        private static string IntToIp(uint ip)
        {
            byte[] bytes = BitConverter.GetBytes(ip);
            Array.Reverse(bytes);
            return new IPAddress(bytes).ToString();
        }
        public static string GenerateQrCodeData(IEnumerable<string> localIps)
        {
            List<string> result = [];
            foreach (string ip in localIps)
            {
                if (ip.Equals("127.0.0.1") || ip.Equals("0.0.0.0"))
                {
                    continue;
                }
                result.Add(IpToInt(ip).ToString());
            }
            return string.Join(" ", result);
        }

        public static IEnumerable<string> DecodePoolCodeData(string qrCodeData)
        {

            List<string> result = [];
            foreach (string ip in qrCodeData.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                result.Add(IntToIp(uint.Parse(ip)));
            }
            return result;
        }
    }
}

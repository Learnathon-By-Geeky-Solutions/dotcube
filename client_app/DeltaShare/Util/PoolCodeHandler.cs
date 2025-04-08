using System.Net;
using System.Text;

namespace DeltaShare.Util
{
    public static class PoolCodeHandler
    {
        private static char[] Base24Chars = "zyxwvutsrqpnmkjihgfedcba".ToCharArray();
        //private static char[] Base24Chars = "abcdefghijkmnpqrstuvwxyz".ToCharArray();
        private static string IntToBase24(uint value)
        {
            if (value == 0)
            {
                return "0";
            }

            StringBuilder result = new StringBuilder();
            while (value > 0)
            {
                result.Append(Base24Chars[value % 24]);
                value /= 24;
            }
            return new string(result.ToString().Reverse().ToArray());
        }

        private static uint Base24ToInt(string value)
        {
            if (value == null)
            {
                return 0;
            }

            uint result = 0;
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                int index = Array.IndexOf(Base24Chars, c);
                if (index < 0)
                {
                    throw new ArgumentException($"Invalid character '{c}' in base24 string.");
                }
                result = result * 24 + (uint)index;
            }
            return result;
        }

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
                result.Add(IntToBase24(IpToInt(ip)));
            }
            return string.Join(" ", result);
        }

        public static IEnumerable<string> DecodePoolCodeData(string qrCodeData)
        {

            List<string> result = [];
            foreach (string ip in qrCodeData.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                result.Add(IntToIp(Base24ToInt(ip)));
            }
            return result;
        }
    }
}

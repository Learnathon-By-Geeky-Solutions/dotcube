using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net.Sockets;
namespace DeltaShare.Util
{
    public static class NetworkHandler
    {
        public static IEnumerable<string> GetLocalIps()
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Where(network => network.OperationalStatus == OperationalStatus.Up)
                .SelectMany(network => network.GetIPProperties().UnicastAddresses)
                .Where(ipInfo => ipInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                .Select(ipInfo => ipInfo.Address.ToString());
        }

        public static string? GetReachableIp(IEnumerable<string> ipAddresses, int port, int timeoutInSeconds)
        {
            foreach (string ip in ipAddresses)
            {
                using TcpClient client = new();
                try
                {
                    Debug.WriteLine($"Trying to connect to {ip}:{port}");
                    var task = client.ConnectAsync(ip, port);
                    if (task.Wait(TimeSpan.FromSeconds(timeoutInSeconds)))
                    {
                        return ip;
                    }
                }
                catch
                {
                    // Ignore
                }
            }
            return null;
        }
    }
}

using System.Net;
using System.Net.Sockets;
using DeltaShare.Util;

namespace DeltaShareTest.Util;

public class NetworkHandlerTest
{
    [Fact]
    public void GetLocalIps_ReturnsOnlyIPv4AndUpInterfaces()
    {
        // Act
        var result = NetworkHandler.GetLocalIps().ToList();

        // Assert
        Assert.All(result, ip =>
        {
            Assert.False(string.IsNullOrWhiteSpace(ip));
            Assert.True(IPAddress.TryParse(ip, out var parsed));
            Assert.Equal(AddressFamily.InterNetwork, parsed.AddressFamily);
        });
    }

    [Fact]
    public void GetReachableIp_ReturnsFirstReachable()
    {
        // Arrange
        // Use localhost with an open port for the test (e.g., HTTP port 80)
        var testListener = new TcpListener(IPAddress.Loopback, 0); // Bind to any free port
        testListener.Start();
        int openPort = ((IPEndPoint)testListener.LocalEndpoint).Port;

        List<string> ips = new() { "127.0.0.1", "192.0.2.1" }; // 192.0.2.1 is TEST-NET-1 and unreachable
        int timeout = 2;

        // Act
        string? reachable = NetworkHandler.GetReachableIp(ips, openPort, timeout);

        // Cleanup
        testListener.Stop();

        // Assert
        Assert.Equal("127.0.0.1", reachable);
    }

    [Fact]
    public void GetReachableIp_ReturnsNullWhenNoneReachable()
    {
        // Arrange
        List<string> unreachableIps = new() { "192.0.2.1", "198.51.100.1" }; // Reserved and unreachable
        int closedPort = 65000; // Assume no service running
        int timeout = 1;

        // Act
        string? reachable = NetworkHandler.GetReachableIp(unreachableIps, closedPort, timeout);

        // Assert
        Assert.Null(reachable);
    }
}


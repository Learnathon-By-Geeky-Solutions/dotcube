using System.Net;
using System.Net.Http.Headers;
using System.Text;
using DeltaShare;
using DeltaShare.Model;
using DeltaShare.Service;
using DeltaShare.Util;
using Moq;
using Moq.Protected;

namespace DeltaShareTest.Service;
public class PoolCreatorClientServiceTest
{
    private readonly User normalUser = new("Name", "email", "username", "127.0.0.1", false);
    private readonly User adminUser = new("Admin", "admin@email.com", "admin", "127.0.0.2", true);

    private FileMetadata CreateTestFile()
    {
        var content = new ByteArrayContent(Encoding.UTF8.GetBytes("fakeimage"));
        content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
        return new FileMetadata("uuid", content, 1000, "file.txt", "127.0.0.1", "text/plain", "/path/file.txt");
    }

    private HttpClient CreateMockHttpClient(out Mock<HttpMessageHandler> handlerMock)
    {
        handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
        .Protected()
            .Setup<Task<HttpResponseMessage>>(
        "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("Success")
            })
            .Verifiable();

        return new HttpClient(handlerMock.Object);
    }

    [Fact]
    public async Task SendAllFileInfoToAllUsers_SendsToNonAdmins()
    {
        // Arrange
        StateManager.PoolUsers = [normalUser, adminUser];
        StateManager.PoolFiles = [CreateTestFile()];
        var httpClient = CreateMockHttpClient(out var handlerMock);
        var service = new PoolCreatorClientService(httpClient);

        // Act
        await service.SendAllFileInfoToAllUsers();

        // Assert
        handlerMock.Protected().Verify(
        "SendAsync",
            Times.Once(), // Only to normalUser, not admin
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Post &&
                req.RequestUri!.ToString().Contains(Constants.FilesSyncPath)),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task SendAllUserInfoToAllUsers_SendsToNonAdmins()
    {
        // Arrange
        StateManager.PoolUsers = [normalUser, adminUser];
        var httpClient = CreateMockHttpClient(out var handlerMock);
        var service = new PoolCreatorClientService(httpClient);

        // Act
        await service.SendAllUserInfoToAllUsers();

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Post &&
                req.RequestUri!.ToString().Contains(Constants.ClientsSyncPath)),
            ItExpr.IsAny<CancellationToken>());
    }
}

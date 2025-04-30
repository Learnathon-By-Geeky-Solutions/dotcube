using DeltaShare.Util;

namespace DeltaShareTest.Util;
public class PoolCodeHandlerTest
{

    [Fact]
    public void DecodePoolCodeData_ThrowsArgumentExceptionForInvalidBase24()
    {
        // Arrange
        string qrCodeData = "invalid_base24";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => PoolCodeHandler.DecodePoolCodeData(qrCodeData));
        Assert.Contains("Invalid character", exception.Message);
    }
}

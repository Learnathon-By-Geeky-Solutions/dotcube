using DeltaShare.Util;

namespace DeltaShareTest.Util;

public class PoolCodeHandlerTest
{
    [Fact]
    public void IntToBase24_ValidData_ReturnsImageSource()
    {
        // Arrange
        uint value = 1234567890;

        // Act
        string result = PoolCodeHandler.IntToBase24(value);

        // Assert
        Assert.NotNull(result);
    }

}

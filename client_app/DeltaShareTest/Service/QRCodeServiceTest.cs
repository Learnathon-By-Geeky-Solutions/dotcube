using DeltaShare.Service;

namespace DeltaShareTest.Service
{
    public class QRCodeServiceTest
    {
        [Fact]
        public void GenerateQRCode_ValidData_ReturnsImageSource()
        {
            // Arrange
            string data = "this is an awesome string";
            QRCodeService QRCodeService = new();

            // Act
            ImageSource result = QRCodeService.GenerateQRCode(data, 100, 100, 0);

            // Assert
            Assert.NotNull(result);
        }
    }
}

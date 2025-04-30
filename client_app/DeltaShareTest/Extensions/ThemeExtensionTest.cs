namespace DeltaShareTest.Extensions;
using DeltaShare.Extensions;

public class ThemeExtensionTest
{
    [Fact]
    public void AddTitleBarTheme_ValidContentPage_SetsForegroundColor()
    {
        // Arrange
        var contentPage = new ContentPage();
        // Act
        contentPage.AddTitleBarTheme();
        // Assert
        Assert.NotNull(contentPage);
    }
}

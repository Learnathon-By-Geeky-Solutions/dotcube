using DeltaShare.Extensions;

namespace DeltaShareTest.Extensions;

public class ButtonExtensionTest
{
    [Fact]
    public void AddButtonTheme_ValidButton_SetsBackgroundAndGestureRecognizers()
    {
        // Arrange
        var button = new Button();
        // Act
        button.AddButtonTheme();
        // Assert
        Assert.NotNull(button.Background);
        Assert.Equal(1, button.GestureRecognizers.Count);
    }

}

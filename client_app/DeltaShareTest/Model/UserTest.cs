namespace DeltaShareTest.Model;
using DeltaShare.Model;
public class UserTest
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var user = new User("Alice", "alice@example.com", "alice123", "192.168.1.1", true);

        Assert.Equal("Alice", user.Name);
        Assert.Equal("alice@example.com", user.Email);
        Assert.Equal("alice123", user.Username);
        Assert.Equal("192.168.1.1", user.IpAddress);
        Assert.True(user.IsAdmin);
    }

    [Fact]
    public void ViewableUsername_ReturnsOriginal_IfUnder10Chars()
    {
        var user = new User("Bob", "bob@example.com", "bob123", "10.0.0.1", false);
        Assert.Equal("bob123", user.ViewableUsername);
    }

    [Fact]
    public void ViewableUsername_InsertsNewlines_Every10Chars()
    {
        var longUsername = "abcdefghijKLMNOPQRSTuvwxyz1234";
        var user = new User("Test", "test@example.com", longUsername, "127.0.0.1", false);

        var expected = "abcdefghij\nKLMNOPQRST\nuvwxyz1234\n";

        Assert.Equal(expected, user.ViewableUsername);
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        var user = new User("John", "john@doe.com", "johnny", "1.2.3.4", true);
        var expected = "Name: John, Email: john@doe.com, Username: johnny, IpAddress: 1.2.3.4, IsAdmin: True";

        Assert.Equal(expected, user.ToString());
    }
}

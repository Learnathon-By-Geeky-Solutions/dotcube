namespace DeltaShareTest.Model;
using DeltaShare.Model;

public class FileMetadataTest
{
    [Fact]
    public void FormattedDownloadedSize_ShouldReturnInBytes_KB_MB()
    {
        var file = new FileMetadata("uuid", 0, "file.txt", "127.0.0.1", "text/plain", "/some/path");

        file.DownloadedSize = 512;
        Assert.Equal("512 bytes", file.FormattedDownloadedSize);

        file.DownloadedSize = 2048;
        Assert.Equal("2 KB", file.FormattedDownloadedSize);

        file.DownloadedSize = 5 * 1024 * 1024;
        Assert.Equal("5 MB", file.FormattedDownloadedSize);
    }

    [Fact]
    public void DownloadProgress_ShouldBeZero_WhenSizeIsZero()
    {
        var file = new FileMetadata("uuid", 0, "file.txt", "127.0.0.1", "text/plain", "/some/path");
        file.DownloadedSize = 512;
        Assert.Equal(0f, file.DownloadProgress);
    }

    [Fact]
    public void DownloadProgress_ShouldCalculateProperly()
    {
        var file = new FileMetadata("uuid", 1000, "file.txt", "127.0.0.1", "text/plain", "/some/path");
        file.DownloadedSize = 250;
        Assert.Equal(0.25, file.DownloadProgress, precision: 2);
    }

    [Fact]
    public void FormattedSize_ShouldReturnCorrectUnits()
    {
        var file1 = new FileMetadata("uuid", 512, "file.txt", "127.0.0.1", "text/plain", "/some/path");
        Assert.Equal("512 bytes", file1.FormattedSize);

        var file2 = new FileMetadata("uuid", 2048, "file.txt", "127.0.0.1", "text/plain", "/some/path");
        Assert.Equal("2 KB", file2.FormattedSize);

        var file3 = new FileMetadata("uuid", 5 * 1024 * 1024, "file.txt", "127.0.0.1", "text/plain", "/some/path");
        Assert.Equal("5 MB", file3.FormattedSize);
    }

    [Fact]
    public void SettingDownloadedSize_ShouldRaisePropertyChangedEvents()
    {
        var file = new FileMetadata("uuid", 1024, "file.txt", "127.0.0.1", "text/plain", "/some/path");
        bool raisedFormattedSize = false;
        bool raisedProgress = false;

        file.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(FileMetadata.FormattedDownloadedSize))
                raisedFormattedSize = true;
            if (e.PropertyName == nameof(FileMetadata.DownloadProgress))
                raisedProgress = true;
        };

        file.DownloadedSize = 500;

        Assert.True(raisedFormattedSize);
        Assert.True(raisedProgress);
    }
}

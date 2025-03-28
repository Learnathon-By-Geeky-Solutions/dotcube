using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeltaShare.Model;
using DeltaShare.Service;
using DeltaShare.Util;

namespace DeltaShare.ViewModel
{
    public partial class DownloadFileViewModel(PoolUserClientService clientService) : BaseViewModel
    {
        private readonly PoolUserClientService clientService = clientService;
        public ObservableCollection<User> PoolUsers => StateManager.PoolUsers;
        public ObservableCollection<FileMetadata> PoolFiles => StateManager.PoolFiles;
        public ObservableCollection<object> SelectedFiles { get; set; } = [];

        [ObservableProperty]
        private bool isDownloadEnabled = false;

        [RelayCommand]
        private void ClickRefreshBtn()
        {
            Debug.WriteLine("Refresh button clicked");
            Debug.WriteLine(StateManager.PoolFiles);
            //User[] demoUsers =
            //[
            //    new("Alice", "", "alice", "1.1.1", true),
            //    new("Bob", "", "bob", "2.2.2", false),
            //    new("Charlie", "", "charlie", "3.3.3", false),
            //    new("David", "", "david", "4.4.4", false),
            //    new("Eve", "", "eve", "5.5.5", false),
            //    new("Frank", "", "frank", "6.6.6", false),
            //    new("Grace", "", "grace", "7.7.7", false),
            //    new("Heidi", "", "heidi", "8.8.8", false),
            //    new("Ivan", "", "ivan", "9.9.9", false),
            //];
            //FileMetadata[] metadata =
            //    [
            //    new("uuid1", 1000, "file1", "1.1.1.1", "video/mp4", "/some/path1"),
            //    new("uuid2", 2000, "file2", "1.1.1.2", "video/mp4", "/some/path2"),
            //    new("uuid3", 3000, "file3", "1.1.1.3", "video/mp4", "/some/path3") {
            //        IsDownloading = true,
            //        DownloadedSize = 1000,
            //        Size = 2000
            //    },
            //    new("uuid4", 4000, "file4", "1.1.1.4", "video/mp4", "/some/path4"),
            //    new("uuid5", 5000, "file5", "1.1.1.5", "video/mp4", "/some/path5") {
            //        IsDownloading = true,
            //        DownloadedSize = 2000,
            //        Size = 5000
            //    },
            //    new("uuid6", 6000, "file6", "1.1.1.6", "video/mp4", "/some/path6"),
            //    new("uuid7", 7000, "file7", "1.1.1.7", "video/mp4", "/some/path7"),
            //    new("uuid8", 8000, "file8", "1.1.1.8", "video/mp4", "/some/path8"),
            //    new("uuid9", 9000, "file9", "1.1.1.9", "video/mp4", "/some/path9"),
            //    new("uuid10", 10000, "file10", "1.1.1.10", "video/mp4", "/some/path10"),
            //    new("uuid11", 11000, "file11", "1.1.1.11", "video/mp4", "/some/path11"),
            //    new("uuid12", 12000, "file12", "1.1.1.12", "video/mp4", "/some/path12"),
            //    new("uuid13", 13000, "file13", "1.1.1.13", "video/mp4", "/some/path13"),
            //    ];
            //foreach (FileMetadata file in metadata)
            //{
            //    StateManager.PoolFiles.Add(file);
            //}
            //foreach (User user in demoUsers)
            //{
            //    StateManager.PoolUsers.Add(user);
            //}

        }

        [RelayCommand]
        private void FileSelectionChanged()
        {
            if (SelectedFiles.Count > 0)
            {
                IsDownloadEnabled = true;
            }
            else
            {
                IsDownloadEnabled = false;
            }
        }

        [RelayCommand]
        private void ClickDownloadBtn()
        {
            _ = Task.Run(() => clientService.SaveFilesFromPool(SelectedFiles));
        }

        [RelayCommand]
        private async Task ClickAddFilesBtn()
        {
            await UploadFiles();
        }

        private async Task UploadFiles()
        {
            IEnumerable<FileResult> files = await FileHandler.PickFiles();
            if (files == null)
            {
                return;
            }
            ObservableCollection<FileMetadata> fileMetadata = await FileHandler.FileResultsToFileMetadata(files);
            await clientService.SendFileInfoToPoolCreator(fileMetadata);
        }

    }
}

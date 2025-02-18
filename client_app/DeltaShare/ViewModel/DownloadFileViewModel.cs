using System.Collections.ObjectModel;
using System.Diagnostics;
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

        [RelayCommand]
        private static void ClickRefreshBtn()
        {
            Debug.WriteLine("Refresh button clicked");
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
            ObservableCollection<FileMetadata> fileMetadata = await FileMetadata.FileResultsToFileMetadata(files);
            clientService.AddFilesToPool(fileMetadata);
            await clientService.SendFileInfoToPoolCreator(fileMetadata);
        }

    }
}

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
            foreach (FileMetadata file in StateManager.PoolFiles)
            {
                Debug.WriteLine(file.Filename);
            }
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
        private async Task ClickDownloadBtn()
        {
            await clientService.SaveFilesFromPool(SelectedFiles);
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
            foreach (FileMetadata file in fileMetadata)
            {
                StateManager.LocalUuidFilePair[file.Uuid] = file;
            }
            clientService.AddFilesToPool(fileMetadata);
            await clientService.SendFileInfoToPoolCreator(fileMetadata);
        }

    }
}

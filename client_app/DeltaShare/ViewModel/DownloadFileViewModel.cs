using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using DeltaShare.Model;
using DeltaShare.Util;

namespace DeltaShare.ViewModel
{
    public partial class DownloadFileViewModel : BaseViewModel
    {
        public ObservableCollection<User> PoolUsers => StateManager.PoolUsers;
        public DownloadFileViewModel()
        {
        }

        [RelayCommand]
        async Task ClickRefreshBtn()
        {
            Debug.WriteLine("Refresh button clicked");
        }

        [RelayCommand]
        async Task ClickAddFilesBtn()
        {
            await UploadFiles();
        }
        public async Task UploadFiles()
        {
            IEnumerable<FileResult> files = await PickFiles();
            if (files == null)
            {
                return;
            }
            foreach (FileResult file in files)
            {
                if (file != null)
                {
                    Debug.WriteLine($"File name: {file.FileName}");
                }
            }
        }

        public async Task<IEnumerable<FileResult>> PickFiles()
        {
            try
            {
                PickOptions options = new()
                {
                    PickerTitle = "Please select a file",
                };
                IEnumerable<FileResult> result = await FilePicker.PickMultipleAsync(options);
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }
    }
}

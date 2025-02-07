using CommunityToolkit.Mvvm.Input;
using DeltaShare.View;

namespace DeltaShare.ViewModel
{
    public partial class DownloadFileViewModel : BaseViewModel
    {
        public DownloadFileViewModel()
        {

        }

        [RelayCommand]
        async Task ClickAddFilesBtn()
        {
            await Shell.Current.GoToAsync(nameof(UploadFileView));
        }
    }
}

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeltaShare.Model;
using DeltaShare.Service;
using DeltaShare.View;

namespace DeltaShare.ViewModel
{
    public partial class SharePoolViewModel : BaseViewModel
    {
        private PoolCreatorServerService serverService;
        public ObservableCollection<User> PoolUsers => serverService.PoolUsers;

        public SharePoolViewModel(PoolCreatorServerService serverService)
        {
            this.serverService = serverService;
        }

        [ObservableProperty]
        private ImageSource? poolQRImage;

        [RelayCommand]
        private async Task ClickViewSharedFilesBtn()
        {
            await Shell.Current.GoToAsync(nameof(DownloadFileView));
        }

        [RelayCommand]
        private void ClickGenerateQRBtn()
        {
            QRCodeService qRCodeService = new();
            PoolQRImage = qRCodeService.GenerateQRCode("test data 123", 200, 200, 0);
        }
    }
}

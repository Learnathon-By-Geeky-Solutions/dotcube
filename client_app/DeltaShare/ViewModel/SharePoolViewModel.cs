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
        [ObservableProperty]
        private string qrCodeData = String.Empty;
        private PoolCreatorServerService serverService;
        public ObservableCollection<User> PoolUsers => serverService.PoolUsers;

        public SharePoolViewModel(PoolCreatorServerService serverService)
        {
            this.serverService = serverService;
            QrCodeData = Constants.PoolCreatorIpAddress;
        }

        [RelayCommand]
        private async Task ClickViewSharedFilesBtn()
        {
            await Shell.Current.GoToAsync(nameof(DownloadFileView));
        }
    }
}

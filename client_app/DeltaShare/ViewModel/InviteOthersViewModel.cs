using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeltaShare.Util;

namespace DeltaShare.ViewModel
{
    public partial class InviteOthersViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string qrCodeData = String.Empty;

        public InviteOthersViewModel()
        {
            QrCodeData = PoolCodeHandler.GenerateQrCodeData([StateManager.PoolCreatorIpAddress]);
        }

        [RelayCommand]
        private async Task ClickViewSharedFilesBtn()
        {
            await Shell.Current.GoToAsync("../");
        }
    }
}

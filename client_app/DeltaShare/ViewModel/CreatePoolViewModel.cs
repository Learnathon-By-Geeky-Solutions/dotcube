using CommunityToolkit.Mvvm.Input;
using DeltaShare.Service;
using DeltaShare.View;

namespace DeltaShare.ViewModel
{
    public partial class CreatePoolViewModel : BaseViewModel
    {
        private PoolCreatorServerService serverService;
        private IWifiDirectService wifiDirectService;

        public CreatePoolViewModel(PoolCreatorServerService serverService, IWifiDirectService wifiDirectService)
        {
            this.serverService = serverService;
            this.wifiDirectService = wifiDirectService;

            wifiDirectService.RegisterReceiver();
        }

        [RelayCommand]
        private void ClickWifiDirectTurnOnBtn()
        {
            wifiDirectService.DiscoverPeers();
        }

        [RelayCommand]
        private async Task ClickSharePoolBtn()
        {
            serverService.StartListening();
            await Shell.Current.GoToAsync(nameof(SharePoolView));
        }
    }
}

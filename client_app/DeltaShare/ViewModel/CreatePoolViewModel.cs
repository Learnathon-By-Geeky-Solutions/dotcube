using CommunityToolkit.Mvvm.Input;
using DeltaShare.Service;
using DeltaShare.View;

namespace DeltaShare.ViewModel
{
    public partial class CreatePoolViewModel : BaseViewModel
    {
        private PoolCreatorServerService serverService;
        public CreatePoolViewModel(PoolCreatorServerService serverService)
        {
            this.serverService = serverService;
        }

        [RelayCommand]
        private void ClickBluetoothTurnOnBtn()
        {
            ShowDebugMsg("turn on bluetooth");
        }

        [RelayCommand]
        private void ClickHotspotTurnOnBtn()
        {
            ShowDebugMsg("turn on hotspot");
        }

        [RelayCommand]
        private void ClickLocationTurnOnBtn()
        {
            ShowDebugMsg("turn on location");
        }

        [RelayCommand]
        private async Task ClickSharePoolBtn()
        {
            serverService.StartListening();
            await Shell.Current.GoToAsync(nameof(SharePoolView));
        }
    }
}

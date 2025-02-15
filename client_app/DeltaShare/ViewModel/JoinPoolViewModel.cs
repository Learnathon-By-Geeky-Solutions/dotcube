using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeltaShare.Service;
using DeltaShare.View;
using ZXing.Net.Maui;

namespace DeltaShare.ViewModel
{
    public partial class JoinPoolViewModel : BaseViewModel
    {
        private readonly PoolUserServerService serverService;
        private readonly PoolUserClientService clientService;
        [ObservableProperty]
        private string poolCodeInputText = string.Empty;

        [ObservableProperty]
        private BarcodeReaderOptions readerOptions = new()
        {
            Formats = BarcodeFormat.QrCode,
            AutoRotate = true,
            Multiple = false
        };

        public JoinPoolViewModel(PoolUserClientService clientService, PoolUserServerService serverService)
        {
            this.clientService = clientService;
            this.serverService = serverService;
        }

        [RelayCommand]
        private async Task ClickJoinPoolBtn()
        {
            serverService.StartListening();

            bool status = await clientService.SendInfoToPoolCreator(PoolCodeInputText);
            if (!status)
            {
                return;
            }
            await Shell.Current.GoToAsync(nameof(DownloadFileView));
        }

        [RelayCommand]
        private async Task BarcodeDetected(List<string> barcodes)
        {
            if (barcodes.Any())
            {
                PoolCodeInputText = barcodes[0];
            }
        }
    }
}

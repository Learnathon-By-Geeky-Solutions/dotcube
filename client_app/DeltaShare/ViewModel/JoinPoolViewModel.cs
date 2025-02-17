using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeltaShare.Service;
using DeltaShare.View;
using ZXing.Net.Maui;

namespace DeltaShare.ViewModel
{
    public partial class JoinPoolViewModel(PoolUserClientService clientService, PoolUserServerService serverService) : BaseViewModel
    {
        private readonly PoolUserServerService serverService = serverService;
        private readonly PoolUserClientService clientService = clientService;
        [ObservableProperty]
        private string poolCodeInputText = string.Empty;

        [ObservableProperty]
        private BarcodeReaderOptions readerOptions = new()
        {
            Formats = BarcodeFormat.QrCode,
            AutoRotate = true,
            Multiple = false
        };

        [RelayCommand]
        private async Task ClickJoinPoolBtn()
        {
            serverService.StartListening();

            bool status = await clientService.SendInfoToPoolCreator(PoolCodeInputText);
            if (!status)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to join pool", "OK");
                return;
            }
            await Shell.Current.GoToAsync(nameof(DownloadFileView));
        }

        [RelayCommand]
        private void BarcodeDetected(List<string> barcodes)
        {
            if (barcodes.Count > 0)
            {
                PoolCodeInputText = barcodes[0];
            }
        }
    }
}

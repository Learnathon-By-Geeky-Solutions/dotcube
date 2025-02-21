using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeltaShare.Service;
using DeltaShare.Util;
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

        partial void OnPoolCodeInputTextChanged(string value)
        {
            _ = ProcessQrCode();
        }

        [RelayCommand]
        private async Task ClickJoinPoolBtn()
        {
            await ProcessQrCode();
        }

        private async Task ProcessQrCode()
        {
            serverService.StartListening();
            string poolCreatorIpAddress = CodeHandler.ExtractIpAddressFromQrCodeData(PoolCodeInputText);
            bool status = await clientService.SendUserInfoToPoolCreator(poolCreatorIpAddress);
            if (!status)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to join pool", "OK");
                });
                return;
            }
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.GoToAsync("../");
                await Shell.Current.GoToAsync(nameof(DownloadFileView));
            });
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

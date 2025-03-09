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

        private static async Task ShowFailedMsg(string msg)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.DisplayAlert("Error", msg, "OK");
            });
        }

        private async Task ProcessQrCode()
        {
            serverService.StartListening();
            IEnumerable<string> probableCreatorIps = PoolCodeHandler.DecodePoolCodeData(PoolCodeInputText);
            string? poolCreatorIp = NetworkHandler.GetReachableIp(probableCreatorIps, int.Parse(Constants.Port), 1);
            if (poolCreatorIp == null)
            {
                await ShowFailedMsg("Network error. Make sure you are on the same network");
                return;
            }

            bool connected = await clientService.SendUserInfoToPoolCreator(poolCreatorIp);
            if (!connected)
            {
                await ShowFailedMsg("Failed to connect to pool creator");
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

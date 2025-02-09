using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeltaShare.Service;
using ZXing.Net.Maui;

namespace DeltaShare.ViewModel
{
    public partial class JoinPoolViewModel : BaseViewModel
    {
        private readonly PoolJoinService joinService;
        [ObservableProperty]
        private string poolCodeInputText = string.Empty;

        [ObservableProperty]
        private BarcodeReaderOptions readerOptions = new()
        {
            Formats = BarcodeFormat.QrCode,
            AutoRotate = true,
            Multiple = false
        };

        public JoinPoolViewModel(PoolJoinService joinService)
        {
            this.joinService = joinService;
        }

        [RelayCommand]
        private async Task ClickJoinPoolBtn()
        {
            await joinService.SendInfoToPoolCreator(PoolCodeInputText);
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

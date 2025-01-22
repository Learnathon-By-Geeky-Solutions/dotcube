using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeltaShare.Service;

namespace DeltaShare.ViewModel
{
    public partial class SharePoolViewModel : BaseViewModel
    {
        private readonly IQRCodeService qrCodeService;
        public SharePoolViewModel(IQRCodeService qrCodeService)
        {
            this.qrCodeService = qrCodeService;
        }

        [ObservableProperty]
        private ImageSource poolQRImage;

        [RelayCommand]
        private void ClickGenerateQRBtn()
        {
            PoolQRImage = qrCodeService.GenerateQRCode("test data 123", 200, 200, 0);
        }
    }
}

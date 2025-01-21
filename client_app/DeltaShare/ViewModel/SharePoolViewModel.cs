using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeltaShare.Service;

namespace DeltaShare.ViewModel
{
    public partial class SharePoolViewModel : BaseViewModel
    {
        public SharePoolViewModel()
        {
        }

        [ObservableProperty]
        private ImageSource poolQRImage;

        [RelayCommand]
        private void ClickGenerateQRBtn()
        {
            QRCodeService qRCodeService = new();
            PoolQRImage = qRCodeService.GenerateQRCode("test data 123", 200, 200, 0);
        }
    }
}

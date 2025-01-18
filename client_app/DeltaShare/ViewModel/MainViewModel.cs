using CommunityToolkit.Mvvm.Input;

namespace DeltaShare.ViewModel
{
    public partial class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
        }

        [RelayCommand]
        void ClickJoinPoolBtn()
        {
            ShowDebugMsg("join pool clicked");
        }

        [RelayCommand]
        void ClickCreatePoolBtn()
        {
            ShowDebugMsg("create pool clicked");
        }

        [RelayCommand]
        void ClickPrevPoolLabel(string uid)
        {
            ShowDebugMsg($"pool clicked uid: {uid}");
        }

        [RelayCommand]
        void ClickCloudStorageBtn()
        {
            ShowDebugMsg("go to cloud storage");
        }

        [RelayCommand]
        void ClickLoginBtn()
        {
            ShowDebugMsg("login button clicked");
        }

        [RelayCommand]
        void ClickSignupBtn()
        {
            ShowDebugMsg("sign up button clicked");
        }
    }
}

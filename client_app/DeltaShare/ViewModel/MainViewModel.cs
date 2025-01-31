using CommunityToolkit.Mvvm.Input;
using DeltaShare.View;

namespace DeltaShare.ViewModel
{
    public partial class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
        }

        [RelayCommand]
        private async Task ClickJoinPoolBtn()
        {
        }

        [RelayCommand]
        private async Task ClickCreatePoolBtn()
        {
            await Shell.Current.GoToAsync(nameof(CreatePoolView));
        }

        [RelayCommand]
        private void ClickPrevPoolLabel(string uid)
        {
            ShowDebugMsg($"pool clicked uid: {uid}");
        }

        [RelayCommand]
        private void ClickCloudStorageBtn()
        {
            ShowDebugMsg("go to cloud storage");
        }

        [RelayCommand]
        private async Task ClickLoginBtn()
        {
            await Shell.Current.GoToAsync(nameof(LoginView));
        }

        [RelayCommand]
        private void ClickSignupBtn()
        {
            ShowDebugMsg("sign up button clicked");
        }
    }
}

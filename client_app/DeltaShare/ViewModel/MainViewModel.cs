using CommunityToolkit.Mvvm.Input;
using DeltaShare.Service;
using DeltaShare.View;

namespace DeltaShare.ViewModel
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly IPermissionService permissionService;
        public MainViewModel(IPermissionService permissionService)
        {
            this.permissionService = permissionService;
            bool settingsShowed = Preferences.Get(Constants.SettingsShowedKey, false);
            if (!settingsShowed)
            {
                Shell.Current.GoToAsync(nameof(SettingsView));
            }
        }

        [RelayCommand]
        private async Task ClickSettingsBtn()
        {
            await Shell.Current.GoToAsync(nameof(SettingsView));
        }

        private async Task<bool> RequestPermissions()
        {
            PermissionStatus permissionStatus = await permissionService.RequestPermissions();

            while (permissionStatus != PermissionStatus.Granted)
            {
                bool shouldRetry = await Shell.Current.DisplayAlert("Permissions", "Please grant all permissions to use the app", "Retry", "Cancel");
                if (!shouldRetry)
                {
                    return false;
                }
                permissionStatus = await permissionService.RequestPermissions();
            }
            return true;
        }

        [RelayCommand]
        private async Task ClickJoinPoolBtn()
        {
            await RequestPermissions();
            await Shell.Current.GoToAsync(nameof(JoinPoolView));
        }

        [RelayCommand]
        private async Task ClickCreatePoolBtn()
        {
            if (!await RequestPermissions())
            {
                return;
            }
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

        [RelayCommand]
        private void ClearPreferencesBtn()
        {
            Preferences.Clear();
            ShowDebugMsg("Preferences cleared");
        }
    }
}

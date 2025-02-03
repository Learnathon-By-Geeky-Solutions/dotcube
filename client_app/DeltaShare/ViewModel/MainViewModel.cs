using CommunityToolkit.Mvvm.Input;
using DeltaShare.View;

namespace DeltaShare.ViewModel
{
    public partial class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
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

        private async Task RequestPermissions()
        {
            await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            await Permissions.RequestAsync<Permissions.NetworkState>();
            await Permissions.RequestAsync<Permissions.NearbyWifiDevices>();

            var locationPermissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            var networkPermissionStatus = await Permissions.CheckStatusAsync<Permissions.NetworkState>();
            var nearbyPermissionStatus = await Permissions.CheckStatusAsync<Permissions.NearbyWifiDevices>();
            if (locationPermissionStatus != PermissionStatus.Granted ||
                networkPermissionStatus != PermissionStatus.Granted ||
                nearbyPermissionStatus != PermissionStatus.Granted)
            {
                await Shell.Current.DisplayAlert("Permissions", "Please grant all permissions to use the app", "OK");
            }

        }

        [RelayCommand]
        private async Task ClickJoinPoolBtn()
        {
            await RequestPermissions();
        }

        [RelayCommand]
        private async Task ClickCreatePoolBtn()
        {
            await RequestPermissions();
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

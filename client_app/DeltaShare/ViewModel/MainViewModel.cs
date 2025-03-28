using CommunityToolkit.Mvvm.Input;
using DeltaShare.Service;
using DeltaShare.View;

namespace DeltaShare.ViewModel
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly PoolCreatorServerService serverService;
        private readonly IPermissionService permissionService;
        public MainViewModel(IPermissionService permissionService, PoolCreatorServerService serverService)
        {
            this.permissionService = permissionService;
            bool settingsShowed = Preferences.Get(Constants.SettingsShowedKey, false);
            if (!settingsShowed)
            {
                Shell.Current.GoToAsync(nameof(SettingsView));
            }
            this.serverService = serverService;
        }

        [RelayCommand]
        private async Task ClickSettingsBtn()
        {
            //var listener = new HttpListener();
            //listener.Prefixes.Add($"http://+:{Constants.Port}/");
            //PoolCreatorServerService serverService = new(listener);
            //serverService.StartListening();
            //await Shell.Current.GoToAsync(nameof(DownloadFileView));

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
            serverService.StartListening();
            //await Shell.Current.GoToAsync(nameof(CreatePoolView));
            await Shell.Current.GoToAsync(nameof(SharePoolView));
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
        private async Task ClickSignupBtn()
        {
            await Shell.Current.GoToAsync(nameof(SignupView));
        }

        [RelayCommand]
        private void ClearPreferencesBtn()
        {
            Preferences.Clear();
            ShowDebugMsg("Preferences cleared");
        }
    }
}

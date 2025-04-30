using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using DeltaShare.Service;
using DeltaShare.Util;
using DeltaShare.View;

namespace DeltaShare.ViewModel
{
    public partial class MainViewModel : BaseViewModel
    {
        public ObservableCollection<string> Claims;
        public string Email = string.Empty;

        private readonly PoolCreatorServerService serverService;
        private readonly IPermissionService permissionService;
        private IWebAuthenticator authenticator;
        private ISecureStorage storage;
        private IUserProfileService profileService;
        public MainViewModel(IPermissionService permissionService, PoolCreatorServerService serverService, IWebAuthenticator authenticator, ISecureStorage storage, IUserProfileService profileService)
        {
            this.permissionService = permissionService;
            bool settingsShowed = Preferences.Get(Constants.SettingsShowedKey, false);
            if (!settingsShowed)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    Shell.Current.GoToAsync(nameof(SettingsView));
                });
            }
            this.serverService = serverService;
            this.authenticator = authenticator;
            this.storage = storage;
            this.profileService = profileService;

            Claims = new ObservableCollection<string>();
        }

        [RelayCommand]
        private async Task ClickSettingsBtn()
        {
            //var listener = new HttpListener();
            //listener.Prefixes.Add($"http://+:{Constants.Port}/");
            //PoolCreatorServerService serverService = new(listener);
            //serverService.StartListening();
            //testFunc();
            //await Shell.Current.GoToAsync(nameof(DownloadFileView));
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync(nameof(SettingsView));
            });
        }

        //private void testFunc()
        //{
        //    string[] test = { "192.168.1.101", "172.27.27.84", "196.27.8.1" };
        //    string result = PoolCodeHandler.GenerateQrCodeData(test);
        //    Debug.WriteLine($"enc: {result}");

        //    IEnumerable<string> ips = PoolCodeHandler.DecodePoolCodeData(result);
        //    Debug.WriteLine($"dec: {string.Join(", ", ips)}");
        //}

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
            MainThread.BeginInvokeOnMainThread(async () => { await Shell.Current.GoToAsync(nameof(JoinPoolView)); });
        }

        [RelayCommand]
        private async Task ClickCreatePoolBtn()
        {
            //if (!await RequestPermissions())
            //{
            //    return;
            //}
            serverService.StartListening();
            //await Shell.Current.GoToAsync(nameof(CreatePoolView));
            MainThread.BeginInvokeOnMainThread(async () => { await Shell.Current.GoToAsync(nameof(SharePoolView)); });
        }

        [RelayCommand]
        private void ClickPrevPoolLabel(string uid)
        {
        }

        [RelayCommand]
        private async Task ClickCloudStorageBtn()
        {
#if WINDOWS
            await Alert.Show("Choosing MAUI was a big mistake. https://github.com/dotnet/maui/issues/2702 . Cloud storage would be available in future releases.");
#else
            await Alert.Show("Cloud storage would be available in future releases.");
#endif
            return;
            //try
            //{
            //    var result = await authenticator.AuthenticateAsync(new WebAuthenticatorOptions
            //    {
            //        CallbackUrl = new Uri($"deltashare://"),
            //        Url = new Uri(new Uri(Constants.BackendBaseUrl), "/auth/login")
            //    });
            //    await storage.SetAsync("access_token", result.AccessToken);

            //    using var response = await profileService.GetProfileClaims();
            //    if (response.IsSuccessStatusCode)
            //    {
            //        Claims.Clear();
            //        var claims = response.Content!.Select(x => $"{x.Key}: {x.Value}");
            //        foreach (var claim in claims)
            //        {
            //            Claims.Add(claim);
            //        }
            //        if (response.Content!.TryGetValue("email", out string email))
            //        {
            //            Email = email;
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    Debug.WriteLine($"Error: {e.Message}");
            //}
        }

        [RelayCommand]
        private void ClearPreferencesBtn()
        {
            Preferences.Clear();
        }
    }
}

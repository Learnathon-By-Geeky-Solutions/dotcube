using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DeltaShare.ViewModel;

public partial class SettingsViewModel : BaseViewModel
{
    [ObservableProperty]
    private string fullName = string.Empty;

    [ObservableProperty]
    private string username = string.Empty;

    [RelayCommand]
    private async Task ClickSaveBtn()
    {
        SaveSettings();
#if WINDOWS
        await Shell.Current.DisplayAlert("DeltaShare", "Settings saved", "OK");
#endif
#if ANDROID
        var toast = Toast.Make("Saved");
        await toast.Show();
#endif
        await Shell.Current.GoToAsync("..");
    }

    public SettingsViewModel()
    {
        Task.Run(LoadSettings);
    }

    private async Task LoadSettings()
    {
        FullName = Preferences.Get(Constants.FullNameKey, Constants.DefaultUsername);
        Username = Preferences.Get(Constants.UsernameKey, await Util.UsernameGenerator.GetRandomUsername());

        SaveSettings();
    }

    private void SaveSettings()
    {
        Preferences.Set(Constants.FullNameKey, FullName);
        Preferences.Set(Constants.UsernameKey, Username);
        Preferences.Set(Constants.SettingsShowedKey, true);
    }
}
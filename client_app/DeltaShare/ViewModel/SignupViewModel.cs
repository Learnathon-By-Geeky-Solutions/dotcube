using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DeltaShare.ViewModel;

public partial class SignupViewModel : BaseViewModel
{
    [ObservableProperty]
    private string email = string.Empty;
    [ObservableProperty]
    private string password = string.Empty;
    [ObservableProperty]
    private string fullName = string.Empty;
    [ObservableProperty]
    private string confirmPassword = string.Empty;
    [ObservableProperty]
    private string username = string.Empty;

    [RelayCommand]
    private async Task ClickSignUpBtn()
    {
        ShowDebugMsg("Sign up button clicked");
    }
}

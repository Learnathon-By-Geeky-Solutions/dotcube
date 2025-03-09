using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DeltaShare.ViewModel
{
    public partial class LoginViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string emailInputText = string.Empty;

        [ObservableProperty]
        private string passwordInputText = string.Empty;

        public LoginViewModel()
        {
        }

        [RelayCommand]
        private static void ClickForgotPasswordBtn()
        {
            ShowDebugMsg("Forgot Password Clicked");
        }

        [RelayCommand]
        private static void ClickGoogleSignInBtn()
        {
            ShowDebugMsg("Google Sign In Clicked");
        }

        [RelayCommand]
        private static void ClickFacebookSignInBtn()
        {
            ShowDebugMsg("Facebook Sign In Clicked");
        }

        [RelayCommand]
        private static void ClickSignUpBtn()
        {
            ShowDebugMsg("Sign Up Clicked");
        }

        [RelayCommand]
        private void ClickLoginBtn()
        {
            ShowDebugMsg($"login with\nemail: {EmailInputText}\npassword: {PasswordInputText}");
        }
    }
}

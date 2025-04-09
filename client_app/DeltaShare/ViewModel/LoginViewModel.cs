using System.Diagnostics;
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
        }

        [RelayCommand]
        private static void ClickGoogleSignInBtn()
        {
        }

        [RelayCommand]
        private static void ClickFacebookSignInBtn()
        {
        }

        [RelayCommand]
        private static void ClickSignUpBtn()
        {
        }

        [RelayCommand]
        private void ClickLoginBtn()
        {
            Debug.WriteLine($"login with\nemail: {EmailInputText}\npassword: {PasswordInputText}");
        }
    }
}

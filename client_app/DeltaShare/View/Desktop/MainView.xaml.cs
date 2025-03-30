using DeltaShare.Extensions;
using DeltaShare.ViewModel;

namespace DeltaShare.View.Desktop;

public partial class MainView : ContentPage
{

    public MainView(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        settingsBtn.AddButtonTheme();
        joinPoolBtn.AddButtonTheme();
        createPoolBtn.AddButtonTheme();
        cloudStorageBtn.AddButtonTheme();
        loginBtn.AddButtonTheme();
        signupBtn.AddButtonTheme();
    }

}


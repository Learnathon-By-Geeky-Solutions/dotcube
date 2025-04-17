using DeltaShare.Extensions;
using DeltaShare.ViewModel;

namespace DeltaShare.View;

public partial class MainView : ContentPage
{
    public MainView(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

#if ANDROID || IOS
        logoImg.HeightRequest = 35;
        animationView.HeightRequest = 180;
        animationView.WidthRequest = 350;
#else
        logoImg.HeightRequest = 40;
        animationView.HeightRequest = 260;
        animationView.WidthRequest = 630;
#endif
        settingsBtn.AddButtonTheme();
        joinPoolBtn.AddButtonTheme();
        createPoolBtn.AddButtonTheme();
        cloudStorageBtn.AddButtonTheme();
        loginBtn.AddButtonTheme();
        signupBtn.AddButtonTheme();
    }

}


using CommunityToolkit.Maui;
using DeltaShare.Service;
using DeltaShare.View;
using DeltaShare.ViewModel;
using Microsoft.Extensions.Logging;

namespace DeltaShare
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Font-Awesome-6-Free-Solid.otf", "FASolid");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            // Dependency Injection - Views
            builder.Services.AddSingleton<MainView>();
            builder.Services.AddSingleton<LoginView>();
            builder.Services.AddSingleton<SharePoolView>();
            builder.Services.AddSingleton<CreatePoolView>();
            builder.Services.AddSingleton<SettingsView>();

            // Dependency Injection - ViewModels
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<SharePoolViewModel>();
            builder.Services.AddSingleton<CreatePoolViewModel>();
            builder.Services.AddSingleton<SettingsViewModel>();

            // Dependency Injection - Services
            builder.Services.AddSingleton<QRCodeService>();
            builder.Services.AddSingleton<PoolCreatorServerService>();

            // Register - Routes
            AppRoutes.RegisterRoutes();

            return builder.Build();
        }
    }
}

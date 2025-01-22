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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            // DI - Views
            builder.Services.AddSingleton<MainView>();
            builder.Services.AddSingleton<LoginView>();
            builder.Services.AddSingleton<SharePoolView>();

            // DI - ViewModels
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<SharePoolViewModel>();

            // ID - Services
            builder.Services.AddSingleton<IQRCodeService, QRCodeService>();

            // Register - Routes
            AppRoutes.RegisterRoutes();

            return builder.Build();
        }
    }
}

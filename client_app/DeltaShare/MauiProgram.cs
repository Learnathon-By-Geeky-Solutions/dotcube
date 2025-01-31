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
            builder.Services.AddSingleton<CreatePoolView>();

            // DI - ViewModels
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<SharePoolViewModel>();
            builder.Services.AddSingleton<CreatePoolViewModel>();

            // DI - Services
            builder.Services.AddSingleton<QRCodeService>();
            builder.Services.AddSingleton<PoolCreatorServerService>();

            // Register - Routes
            AppRoutes.RegisterRoutes();

            return builder.Build();
        }
    }
}

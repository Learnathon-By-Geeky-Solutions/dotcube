using System.Net;
using CommunityToolkit.Maui;

#if ANDROID
using DeltaShare.Platforms.Android.Service;
#endif
#if WINDOWS
using DeltaShare.Platforms.Windows.Service;
#endif
using DeltaShare.Service;
using DeltaShare.View;
using DeltaShare.ViewModel;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using ZXing.Net.Maui.Controls;

namespace DeltaShare
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .UseMauiCommunityToolkit()
                .UseBarcodeReader()
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
            builder.Services.AddSingleton<JoinPoolView>();
            builder.Services.AddSingleton<DownloadFileView>();

            // Dependency Injection - ViewModels
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<SharePoolViewModel>();
            builder.Services.AddSingleton<CreatePoolViewModel>();
            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddSingleton<JoinPoolViewModel>();
            builder.Services.AddSingleton<DownloadFileViewModel>();

            // Dependency Injection - Services
            builder.Services.AddSingleton<PoolCreatorServerService>();
            builder.Services.AddSingleton<PoolCreatorClientService>();
            builder.Services.AddSingleton<PoolUserServerService>();
            builder.Services.AddSingleton<PoolUserClientService>();
            HttpClient client = new()
            {
                Timeout = Timeout.InfiniteTimeSpan
            };
            builder.Services.AddSingleton(client);
            HttpListener listener = new();
            listener.Prefixes.Add($"http://+:{Constants.Port}/");
            builder.Services.AddSingleton(listener);

            // Dependency Injection - Platform specific components
#if ANDROID
            // WifiDIrect service needs the Android context
            builder.Services.AddSingleton(_ => Android.App.Application.Context);
            builder.Services.AddSingleton<IWifiDirectService, WifiDirectService>(sp =>
            {
                var context = Platform.CurrentActivity ?? throw new InvalidOperationException("Android context is not available.");
                return new WifiDirectService(context);
            });
            builder.Services.AddSingleton<IPermissionService, PermissionService>();
#endif

#if WINDOWS
            builder.Services.AddSingleton<IWifiDirectService, WifiDirectService>();
            builder.Services.AddSingleton<IPermissionService, PermissionService>();
#endif

            // Register - Routes
            AppRoutes.RegisterRoutes();

            return builder.Build();
        }
    }
}

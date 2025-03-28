using DeltaShare.View;

namespace DeltaShare
{
    public static class AppRoutes
    {
        public static void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
            Routing.RegisterRoute(nameof(SignupView), typeof(SignupView));
            Routing.RegisterRoute(nameof(CreatePoolView), typeof(CreatePoolView));
            Routing.RegisterRoute(nameof(SharePoolView), typeof(SharePoolView));
            Routing.RegisterRoute(nameof(SettingsView), typeof(SettingsView));
            Routing.RegisterRoute(nameof(JoinPoolView), typeof(JoinPoolView));
            Routing.RegisterRoute(nameof(DownloadFileView), typeof(DownloadFileView));
        }
    }
}

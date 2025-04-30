using DeltaShare.View;

namespace DeltaShare
{
    public static class AppRoutes
    {
        public static void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(SharePoolView), typeof(SharePoolView));
            Routing.RegisterRoute(nameof(SettingsView), typeof(SettingsView));
            Routing.RegisterRoute(nameof(JoinPoolView), typeof(JoinPoolView));
            Routing.RegisterRoute(nameof(DownloadFileView), typeof(DownloadFileView));
            Routing.RegisterRoute(nameof(InviteOthersView), typeof(InviteOthersView));
        }
    }
}

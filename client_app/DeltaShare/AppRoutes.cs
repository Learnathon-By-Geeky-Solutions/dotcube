using DeltaShare.View;

namespace DeltaShare
{
    public static class AppRoutes
    {
        public static void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
            Routing.RegisterRoute(nameof(MainView), typeof(MainView));
            Routing.RegisterRoute(nameof(SharePoolView), typeof(SharePoolView));
        }
    }
}

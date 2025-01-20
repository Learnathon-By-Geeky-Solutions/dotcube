using DeltaShare.View;

namespace DeltaShare
{
    public static class AppRoutes
    {
        public static void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        }
    }
}

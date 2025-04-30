namespace DeltaShare.Extensions;

public static class ThemeExtension
{
    public static void AddTitleBarTheme(this ContentPage contentPage)
    {
#if ANDROID || IOS
        Shell.SetForegroundColor(contentPage, Color.FromRgba("#1a434e"));
#else
        if (Application.Current == null)
            return;
        var currentTheme = Application.Current!.RequestedTheme;
        if (currentTheme == AppTheme.Dark)
        {
            Shell.SetForegroundColor(contentPage, Color.FromRgba("#1a434e"));
        }
        else
        {
            Shell.SetForegroundColor(contentPage, Colors.White);
        }
#endif

    }
}

namespace DeltaShare.Util
{
    public static class Alert
    {
        public static async Task Show(string message)
        {
#if WINDOWS
            await Shell.Current.DisplayAlert("DeltaShare", message, "OK");
#endif

#if ANDROID
            var toast = CommunityToolkit.Maui.Alerts.Toast.Make(message);
            await toast.Show();
#endif
        }
    }
}

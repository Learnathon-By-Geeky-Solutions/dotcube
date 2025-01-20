using CommunityToolkit.Mvvm.ComponentModel;

namespace DeltaShare.ViewModel
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;

        [ObservableProperty]
        private string title = string.Empty;

        public bool IsNotBusy => !IsBusy;

#if DEBUG
        public static void ShowDebugMsg(string msg)
        {
            Shell.Current.DisplayAlert("DEBUG", msg, "OK");
        }
#endif

    }
}

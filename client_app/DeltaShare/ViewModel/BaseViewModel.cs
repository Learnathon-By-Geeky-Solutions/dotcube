using CommunityToolkit.Mvvm.ComponentModel;

namespace DeltaShare.ViewModel
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        [ObservableProperty]
        string title = string.Empty;

        public bool IsNotBusy => !IsBusy;

#if DEBUG
        public static void ShowDebugMsg(string msg)
        {
            Shell.Current.DisplayAlert("DEBUG", msg, "OK");
        }
#endif

    }
}

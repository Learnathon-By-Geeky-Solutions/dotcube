using DeltaShare.Extensions;
using DeltaShare.ViewModel;

namespace DeltaShare.View;

public partial class DownloadFileView : ContentPage
{
    public DownloadFileView(DownloadFileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        inviteBtn.AddButtonTheme();
        refreshBtn.AddButtonTheme();
        addFilesBtn.AddButtonTheme();
        saveToCloudBtn.AddButtonTheme();
        downloadBtn.AddButtonTheme();

        if (fileCollection.ItemsLayout is GridItemsLayout gridLayout)
        {
#if ANDROID || IOS
            gridLayout.Span = 2;
#else
            gridLayout.Span = 4;
#endif
        }
    }
    protected override bool OnBackButtonPressed()
    {
        Dispatcher.Dispatch(async () =>
        {
            bool result = await DisplayAlert(
                "Confirm Exit",
                "Are you sure you want to go back? This pool will be closed.",
                "Yes",
                "No"
            );

            if (result)
                await Navigation.PopAsync();
        });

        return true;
    }
}
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
}
using DeltaShare.ViewModel;

namespace DeltaShare.View;

public partial class UploadFileView : ContentPage
{
    public UploadFileView(UploadFileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
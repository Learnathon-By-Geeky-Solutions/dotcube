using DeltaShare.ViewModel;

namespace DeltaShare.View;

public partial class SharePoolView : ContentPage
{
    public SharePoolView(SharePoolViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
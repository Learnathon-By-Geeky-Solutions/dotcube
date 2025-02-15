using DeltaShare.ViewModel;

namespace DeltaShare.View;

public partial class CreatePoolView : ContentPage
{
    public CreatePoolView(CreatePoolViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
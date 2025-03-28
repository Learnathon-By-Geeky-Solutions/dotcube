using DeltaShare.ViewModel;

namespace DeltaShare.View;

public partial class SignupView : ContentPage
{
    public SignupView(SignupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
using DeltaShare.ViewModel;

namespace DeltaShare.View.Phone;

public partial class MainView : ContentPage
{

    public MainView(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

}


using DeltaShare.ViewModel;

namespace DeltaShare.View
{
    public partial class MainPage : ContentPage
    {

        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

    }

}

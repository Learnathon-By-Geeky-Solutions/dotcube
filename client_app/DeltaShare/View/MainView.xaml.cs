using DeltaShare.ViewModel;

namespace DeltaShare.View
{
    public partial class MainView : ContentPage
    {

        public MainView(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

    }

}

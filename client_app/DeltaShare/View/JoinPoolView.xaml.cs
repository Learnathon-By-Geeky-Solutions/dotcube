using DeltaShare.ViewModel;

namespace DeltaShare.View;

public partial class JoinPoolView : ContentPage
{
	public JoinPoolView(JoinPoolViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
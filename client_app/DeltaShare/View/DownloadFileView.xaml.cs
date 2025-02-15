using DeltaShare.ViewModel;

namespace DeltaShare.View;

public partial class DownloadFileView : ContentPage
{
	public DownloadFileView(DownloadFileViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
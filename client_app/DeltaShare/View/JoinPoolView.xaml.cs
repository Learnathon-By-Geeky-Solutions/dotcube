using DeltaShare.ViewModel;

namespace DeltaShare.View;

public partial class JoinPoolView : ContentPage
{
    public JoinPoolView(JoinPoolViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        cameraBarcodeReaderView.BarcodesDetected += (_, eventArgs) =>
        {
            var barcodes = eventArgs.Results.Select(result => result.Value).ToList();
            viewModel.BarcodeDetectedCommand.Execute(barcodes);
        };
    }

}
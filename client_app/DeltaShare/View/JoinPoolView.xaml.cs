using DeltaShare.Extensions;
using DeltaShare.ViewModel;

namespace DeltaShare.View;

public partial class JoinPoolView : ContentPage
{
    public JoinPoolView(JoinPoolViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        joinPoolBtn.AddButtonTheme();

        cameraBarcodeReaderView.BarcodesDetected += (_, eventArgs) =>
        {
            var barcodes = eventArgs.Results.Select(result => result.Value).ToList();
            viewModel.BarcodeDetectedCommand.Execute(barcodes);
        };
    }

}
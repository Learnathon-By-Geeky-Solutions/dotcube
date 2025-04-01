using DeltaShare.Extensions;
using DeltaShare.ViewModel;

namespace DeltaShare.View;

public partial class SettingsView : ContentPage
{
    public SettingsView(SettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        saveBtn.AddButtonTheme();
    }
}
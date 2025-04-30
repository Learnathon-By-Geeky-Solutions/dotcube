using DeltaShare.Extensions;
using DeltaShare.ViewModel;

namespace DeltaShare.View;

public partial class InviteOthersView : ContentPage
{
    public InviteOthersView(InviteOthersViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        viewSharedFilesBtn.AddButtonTheme();

        this.AddTitleBarTheme();
    }
}
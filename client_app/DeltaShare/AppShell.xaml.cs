using DeltaShare.View;

namespace DeltaShare;

public partial class AppShell : Shell
{
    public AppShell()
    {
        Items.Add(new ShellContent
        {
            Title = "DeltaShare",
            ContentTemplate = new DataTemplate(typeof(MainView)),
            Route = nameof(MainView)
        });

        InitializeComponent();
    }
}

#if ANDROID || IOS
using DeltaShare.View.Phone;
#else
using DeltaShare.View.Desktop;
#endif

namespace DeltaShare
{
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
}

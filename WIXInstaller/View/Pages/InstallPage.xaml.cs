using System.Windows.Controls;

namespace WIXInstaller.VIew.Pages
{
    public partial class InstallPage : Page
    {
        public InstallPage()
        {
            InitializeComponent();
            DataContext = AppInstaller.viewModel;
        }
    }
}

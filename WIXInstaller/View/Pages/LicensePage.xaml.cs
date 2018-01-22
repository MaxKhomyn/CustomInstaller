using System.Windows.Controls;

namespace WIXInstaller.VIew.Pages
{
    public partial class LicensePage : Page
    {
        public LicensePage()
        {
            InitializeComponent();
            DataContext = AppInstaller.viewModel;
        }
    }
}

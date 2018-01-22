using System.Windows.Controls;

namespace WIXInstaller.VIew.Pages
{
    public partial class ActivationPage : Page
    {
        public ActivationPage()
        {
            InitializeComponent();
            DataContext = AppInstaller.viewModel;
        }
    }
}

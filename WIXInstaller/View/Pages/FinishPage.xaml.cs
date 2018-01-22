using System.Windows.Controls;

namespace WIXInstaller.VIew.Pages
{
    public partial class FinishPage : Page
    {
        public FinishPage()
        {
            InitializeComponent();
            DataContext = AppInstaller.viewModel;
        }
    }
}

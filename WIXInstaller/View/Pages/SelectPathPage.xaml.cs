using System.Windows.Controls;

namespace WIXInstaller.VIew.Pages
{
    public partial class SelectPathPage : Page
    {
        public SelectPathPage()
        {
            InitializeComponent();
            DataContext = AppInstaller.viewModel;
        }
    }
}

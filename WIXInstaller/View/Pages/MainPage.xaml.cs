using System.Windows.Controls;

namespace WIXInstaller.VIew.Pages
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = AppInstaller.viewModel;
        }
    }
}

using System;
using System.Globalization;
using System.Windows.Controls;

namespace WIXInstaller.VIew.Pages
{
    public partial class LanguagePage : Page
    {
        public LanguagePage()
        {
            InitializeComponent();
            DataContext = AppInstaller.viewModel;

            AppInstaller.LanguageChanged += LanguageChanged;

            CultureInfo currLang = AppInstaller.Language;
            
            LanguagesBox.Items.Clear();
            foreach (var lang in AppInstaller.Languages)
            {
                CheckBox CB = new CheckBox()
                {
                    Content = lang.DisplayName,
                    IsEnabled = false,
                    IsChecked = lang.Equals(currLang)
                };

                ComboBoxItem Lang = new ComboBoxItem()
                {
                    Content = CB,
                    Tag = lang
                };
                LanguagesBox.SelectionChanged += ChangeLanguageClick;
                LanguagesBox.Items.Add(Lang);

                if(CB.IsChecked == true)
                LanguagesBox.SelectedItem = Lang;
            }
        }

        #region Language
        private void LanguageChanged(Object sender, EventArgs e)
        {
            CultureInfo currLang = AppInstaller.Language;

            foreach (ComboBoxItem i in LanguagesBox.Items)
            {
                CultureInfo ci = i.Tag as CultureInfo;

                CheckBox CB = (CheckBox)i.Content;
                CB.IsChecked = ci != null && ci.Equals(currLang);
                if (CB.IsChecked == true)
                    LanguagesBox.SelectedItem = i;
            }
        }

        private void ChangeLanguageClick(Object sender, EventArgs e)
        {
            ComboBox CB = sender as ComboBox;
            ComboBoxItem CBI = CB.SelectedItem as ComboBoxItem;
            if (CBI != null)
            {
                CultureInfo lang = CBI.Tag as CultureInfo;
                if (lang != null)
                {
                    AppInstaller.Language = lang;
                }
            }
        }
        #endregion
    }
}
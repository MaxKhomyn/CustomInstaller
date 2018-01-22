using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;
using System.Diagnostics;
using System.Windows.Threading;
using WIXInstaller.Model;
using WIXInstaller.ViewModel;
using System.Threading;
using System.Windows;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace WIXInstaller
{
    public class AppInstaller : BootstrapperApplication
    {
        #region Properties

        public static Dispatcher BootstrapperDispatcher { get; private set; }
        public static MainViewModel viewModel;
        public static MainWindow view;

        public static string Args = string.Empty;

        private static List<CultureInfo> m_Languages = new List<CultureInfo>();
        public static List<CultureInfo> Languages
        {
            get
            {
                return m_Languages;
            }
        }

        public static event EventHandler LanguageChanged;

        #endregion

        protected override void Run()
        {
            if (!InstanceCheck())
            {
                Thread t = new Thread
                (
                    () =>
                    {
                        MessageBox.Show("Неможливо запустити кілька копій програми!");
                        Thread.Sleep(10000);
                    }
                );
                t.Start();

                Engine.Quit(0);
            }
            else
            {
                //Debugger.Launch();

                if (Command.GetCommandLineArgs().Length > 0)
                    Args = Command.GetCommandLineArgs()[0];

                var envArgs = Environment.GetCommandLineArgs();
                if (envArgs.Length == 5)
                    Args = envArgs[envArgs.Length - 1];

                Engine.Log(LogLevel.Verbose, "Launching");

                BootstrapperDispatcher = Dispatcher.CurrentDispatcher;

                if (!Launcher.IsRunAsAdmin())
                    Launcher.RunAsAdministrator();
                
                viewModel = new MainViewModel(this);
                viewModel.Bootstrapper.Engine.Detect();
                view = new MainWindow { DataContext = viewModel };
                view.Closed += (sender, e) => BootstrapperDispatcher.InvokeShutdown();

                m_Languages.Clear();
                m_Languages.Add(new CultureInfo("uk-UA"));
                m_Languages.Add(new CultureInfo("en-US"));

                view.Show();

                Language = Properties.Settings.Default.DefaultLanguage;
                Dispatcher.Run();
                
                Engine.Quit(0);
            }
        }

        static Mutex InstanceCheckMutex;
        static bool InstanceCheck()
        {
            bool isNew;
            InstanceCheckMutex = new Mutex(true, "ProjectSolutionInstallerLaucher", out isNew);
            return isNew;
        }

        public static CultureInfo Language
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (value == Thread.CurrentThread.CurrentUICulture) return;

                Thread.CurrentThread.CurrentUICulture = value;

                ResourceDictionary dict = new ResourceDictionary();
                switch (value.Name)
                {
                    case "en-US":
                        dict.Source = new Uri(String.Format("/WIXInstaller;component/UIStyles/Lang/lang.{0}.xaml", value.Name), UriKind.Relative);
                        break;
                    default:
                        dict.Source = new Uri(String.Format("/WIXInstaller;component/UIStyles/Lang/lang.xaml"), UriKind.Relative);
                        break;
                }

                ResourceDictionary oldDict = null;
                try
                {
                    oldDict =
                    (
                        from d in Application.Current.Resources.MergedDictionaries
                        where
                            d.Source != null &&
                            d.Source.OriginalString.StartsWith
                            (
                                "pack://application:,,,/WIXInstaller;component/UIStyles/Lang/lang."
                            )
                        select d
                    ).First();
                }
                catch (Exception e) { Debug.WriteLine(e.Message); }
                
                if (oldDict != null)
                {
                    ResourceService.ReplaceWindowResource(view, oldDict, dict);

                    ResourceService.ReplaceAllPageResource(viewModel.PagesIterator.GetAll(), oldDict, dict);
                }
                else
                {
                    ResourceService.AddWindowResource(view, dict);

                    ResourceService.AddAllPageResource(viewModel.PagesIterator.GetAll(), dict);
                }

                LanguageChanged(Application.Current, new EventArgs());
            }
        }

        private void App_LanguageChanged(Object sender, EventArgs e)
        {
            Properties.Settings.Default.DefaultLanguage = Language;
            Properties.Settings.Default.Save();
        }
    }
}
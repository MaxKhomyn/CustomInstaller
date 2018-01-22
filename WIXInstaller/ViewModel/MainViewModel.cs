using System;
using System.IO;
using System.Xml;
using System.Windows;
using System.Threading;
using GalaSoft.MvvmLight;
using InstallerWIX.Model;
using System.Diagnostics;
using WIXInstaller.Model;
using System.Windows.Media;
using System.Windows.Markup;
using WIXInstaller.Services;
using WIXInstaller.VIew.Pages;
using System.Windows.Controls;
using WIXInstaller.Model.UpDate;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using WIXInstaller.UIStyles.Controls;
using WIXInstaller.Model.Installation;
using WIXInstaller.BLL.Model.Iterator;
using static WIXInstaller.AppInstaller;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace WIXInstaller.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties

        public BootstrapperApplication Bootstrapper { get; private set; }

        private Frame MainFrame => view?.MainFrame;
        public IteratorBrowser PagesIterator;

        #region object

        //private object currentPage = new LanguagePage();
        //public object CurrentPage
        //{
        //    get { return currentPage; }
        //    set
        //    {
        //        currentPage = value;
        //        RaisePropertyChanged("CurrentPage");
        //    }
        //}

        #endregion // object

        #region bool

        private bool createStartShortcut = true;
        public bool CreateStartShortcut
        {
            get { return createStartShortcut; }
            set
            {
                createStartShortcut = value;
                RaisePropertyChanged("CreateStartShortcut");
            }
        }

        private bool createDesktopShortcut = true;
        public bool CreateDesktopShortcut
        {
            get { return createDesktopShortcut; }
            set
            {
                createDesktopShortcut = value;
                RaisePropertyChanged("CreateDesktopShortcut");
            }
        }

        private bool createStartMenuFolder = true;
        public bool CreateStartMenuFolder
        {
            get { return createStartMenuFolder; }
            set
            {
                createStartMenuFolder = value;
                RaisePropertyChanged("CreateStartMenuFolder");
            }
        }

        private bool installEnabled;
        public bool InstallEnabled
        {
            get { return installEnabled; }
            set
            {
                installEnabled = value;
                RaisePropertyChanged("InstallEnabled");
            }
        }

        private bool uninstallEnabled;
        public bool UninstallEnabled
        {
            get { return uninstallEnabled; }
            set
            {
                uninstallEnabled = value;
                RaisePropertyChanged("UninstallEnabled");
            }
        }

        private bool isLaunchApplication = true;
        public bool IsLaunchApplication
        {
            get { return isLaunchApplication; }
            set
            {
                isLaunchApplication = value;
                RaisePropertyChanged("IsLaunchApplication");
            }
        }

        private bool isThinking;
        public bool IsThinking
        {
            get { return isThinking; }
            set
            {
                isThinking = value;
                RaisePropertyChanged("IsThinking");
            }
        }

        #endregion // bool

        #region string

        public string RequiredSize
        {
            get { return Settings.RequiredSize; }
        }

        private string installCompleted;
        public string InstallCompleted
        {
            get
            {
                return installCompleted;
            }
            set
            {
                installCompleted = value;
                RaisePropertyChanged("InstallCompleted");
            }
        }

        private string installFolderPath = "";
        public string InstallFolderPath
        {
            get
            {
                if (string.IsNullOrEmpty(installFolderPath))
                    return $@"{Settings.ProgramFilesPath}\{Settings.CurrentPackage.Manufacturer}";
                else
                    return installFolderPath;
            }
            set
            {
                installFolderPath = value;
                Settings.ExecutableFile = value + $@"\{Settings.CurrentPackage.ApplicationName}\{Settings.CurrentPackage.ExeName}";

                Settings.IconLocation = value + $@"\{Settings.CurrentPackage.ApplicationName}\{Settings.CurrentPackage.ExeName}";

                RaisePropertyChanged("InstallFolderPath");
            }
        }

        private string freeDiscSpace;
        public string FreeDiscSpace
        {
            get { return Settings.GetFreeDiscSpace(InstallFolderPath).ToString(); }
            set
            {
                freeDiscSpace = value;
                RaisePropertyChanged("FreeDiscSpace");
            }
        }

        private string installButtonText;
        public string InstallButtonText
        {
            get { return installButtonText; }
            set
            {
                installButtonText = value;
                RaisePropertyChanged("InstallButtonText");
            }
        }

        private string upDateButtonText;
        public string UpDateButtonText
        {
            get { return upDateButtonText; }
            set
            {
                upDateButtonText = value;
                RaisePropertyChanged("UpDateButtonText");
            }
        }

        private string unInstallButtonText;
        public string UnInstallButtonText
        {
            get { return unInstallButtonText; }
            set
            {
                unInstallButtonText = value;
                RaisePropertyChanged("UnInstallButtonText");
            }
        }

        private string installationText;
        public string InstallationText
        {
            get { return installationText; }
            set
            {
                installationText = value;
                RaisePropertyChanged("InstallationText");
            }
        }

        private string userKeyText;
        public string UserKeyText
        {
            get { return userKeyText; }
            set
            {
                userKeyText = value;
                RaisePropertyChanged("UserKeyText");
            }
        }

        private string userEMailText;
        public string UserEMailText
        {
            get { return userEMailText; }
            set
            {
                userEMailText = value;
                RaisePropertyChanged("UserEMailText");
            }
        }

        private string userKeyTitleText;
        public string UserKeyTitleText
        {
            get { return userKeyTitleText; }
            set { userKeyTitleText = value; }
        }

        private string userEMailTitleText;
        public string UserEMailTitleText
        {
            get { return userEMailTitleText; }
            set { userEMailTitleText = value; }
        }

        #endregion // string

        #region Quantitative

        private long installProgressValue;
        public long InstallProgressValue
        {
            get { return installProgressValue; }
            set
            {
                installProgressValue = value;
                RaisePropertyChanged("InstallProgressValue");
            }
        }

        private long installProgressValuePercent;
        public long InstallProgressValuePercent
        {
            get { return installProgressValuePercent; }
            set
            {
                installProgressValuePercent = value;
                RaisePropertyChanged("InstallProgressValuePercent");
            }
        }

        private long maxValAppPartsSize = long.MaxValue;
        public long MaxValAppPartsSize
        {
            get { return maxValAppPartsSize; }
            set
            {
                maxValAppPartsSize = value;
                RaisePropertyChanged("MaxValAppPartsSize");
            }
        }

        #endregion // Quantitative

        #region Enums

        private Visibility launchAppVisibility = Visibility.Visible;
        public Visibility LaunchAppVisibility
        {
            get { return launchAppVisibility; }
            set
            {
                launchAppVisibility = value;
                RaisePropertyChanged("LaunchAppVisibility");
            }
        }

        private Visibility installVisibility;
        public Visibility InstallVisibility
        {
            get { return installVisibility; }
            set
            {
                installVisibility = value;
                RaisePropertyChanged("InstallVisibility");
            }
        }

        private Visibility upDateVisibility;
        public Visibility UpDateVisibility
        {
            get { return upDateVisibility; }
            set
            {
                upDateVisibility = value;
                RaisePropertyChanged("UpDateVisibility");
            }
        }

        private Visibility installProgressPercentVisibility = Visibility.Visible;
        public Visibility InstallProgressPercentVisibility
        {
            get { return installProgressPercentVisibility; }
            set
            {
                installProgressPercentVisibility = value;
                RaisePropertyChanged("InstallProgressPercentVisibility");
            }
        }

        #endregion

        #endregion //Properties
        
        public MainViewModel(AppInstaller bootstrapperInstaller)
        {
            IsThinking = false;
            Bootstrapper = bootstrapperInstaller;
            Bootstrapper.PlanComplete += OnPlanComplete;

            //Downloader.OnDownloadComponentsComplite += Installer.Configurate;
            Downloader.OnSizeComponentsUpdate += DownloaderOnSizeAppPartsUpdate;

            Installer.OnComplite += InstallerOnComplite;
        }
        
        #region RelayCommands

        private RelayCommand nextCommand;
        public RelayCommand NextCommand
        {
            get
            {
                if (nextCommand == null)
                    nextCommand = new RelayCommand
                    (
                        () =>
                        {
                            CheckConnecting(InternetConnection.Check());
                            NextPageAnimation();
                        }
                    );
                return nextCommand;
            }
        }

        private RelayCommand activateCommand;
        public RelayCommand ActivateCommand
        {
            get
            {
                if (activateCommand == null)
                    activateCommand = new RelayCommand
                    (
                        () =>
                        {
                            if (Activation.Activate(UserEMailText, UserKeyText))
                            {
                                NextPageAnimation();
                            }
                            else
                            {
                                MessageBox.Show(view?.FindResource("ActivationError").ToString());
                            }
                        }
                    );
                return activateCommand;
            }
        }

        private RelayCommand installCommand;
        public RelayCommand InstallCommand
        {
            get
            {
                if (installCommand == null)
                    installCommand = new RelayCommand
                    (
                        () =>
                        {
                            InstallProgressPercentVisibility = Visibility.Hidden;
                            InstallationText = view?.FindResource("PleaseWaitWhileInstallationComplete").ToString();
                            NextPageAnimation();
                        },
                        () => InstallEnabled == true
                        //() => InstallVisibility == Visibility.Visible
                    );
                return installCommand;
            }
        }

        private RelayCommand upDateCommand;
        public RelayCommand UpDateCommand
        {
            get
            {
                if (upDateCommand == null)
                    upDateCommand = new RelayCommand
                    (
                        () =>
                        {
                            UpDateMenuItems();
                            InstallProgressPercentVisibility = Visibility.Hidden;
                            InstallationText = view?.FindResource("PleaseWaitWhileUpDateCompleted").ToString();
                            NextPageAnimation();

                            Updater.StartUpDate();
                        },
                        () => UpDateVisibility == Visibility.Visible
                    );
                return upDateCommand;
            }
        }

        private RelayCommand uninstallCommand;
        public RelayCommand UninstallCommand
        {
            get
            {
                if (uninstallCommand == null)

                    uninstallCommand = new RelayCommand
                    (
                        () =>
                        {
                            UnInstallMenuItems();
                            InstallProgressValuePercent = 0;
                            InstallProgressPercentVisibility = Visibility.Hidden;
                            InstallationText = view?.FindResource("PleaseWaitWhileUnInstallCompleted").ToString();
                            NextPageAnimation();

                            Thread t = new Thread
                            (
                                () =>
                                {
                                    Installer.StartUninstallation();
                                    UnistallCacheAndRegistry();
                                    //InstallCommand.RaiseCanExecuteChanged();
                                }
                            );
                            t.Start();
                        }, 
                        () => UninstallEnabled == true
                    );
                return uninstallCommand;
            }
        }

        private RelayCommand selectPathCommand;
        public RelayCommand SelectPathCommand
        {
            get
            {
                if (selectPathCommand == null)
                    selectPathCommand = new RelayCommand(() =>
                    {
                        var dialog = new CommonOpenFileDialog()
                        {
                            IsFolderPicker = true
                        };
                        try
                        {
                            //Thread t = new Thread(
                            //()=> 
                            //{
                                CommonFileDialogResult result = dialog.ShowDialog(); 

                                if (dialog.FileName.Length > 3)
                                {
                                    InstallFolderPath = dialog.FileName + $@"\{Settings.CurrentPackage.Manufacturer}";
                                }
                                else
                                {
                                    InstallFolderPath = dialog.FileName + $@"{Settings.CurrentPackage.Manufacturer}";
                                }
                                RaisePropertyChanged("FreeDiscSpace");
                            //});
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                            // The dialog has closed. To do nothing 
                        }
                    });
                return selectPathCommand;
            }
        }

        private RelayCommand settingsToNextPageCommand;
        public RelayCommand SettingsToNextPageCommand
        {
            get
            {
                if (settingsToNextPageCommand == null)
                    settingsToNextPageCommand = new RelayCommand(() =>
                    {


                    }, () =>
                    {
                        if (Convert.ToInt32(RequiredSize) > Convert.ToInt32(FreeDiscSpace))
                            return false;
                        else
                            return true;
                    });

                return settingsToNextPageCommand;
            }
        }

        private RelayCommand navigateBackCommand;
        public RelayCommand NavigateBackCommand
        {
            get
            {
                if (navigateBackCommand == null)
                    navigateBackCommand = new RelayCommand(() =>
                    {
                        BackPageAnimation();
                    });
                return navigateBackCommand;
            }
        }

        private RelayCommand pasteMailCommand;
        public RelayCommand PasteMailCommand
        {
            get
            {
                if (pasteMailCommand == null)
                    pasteMailCommand = new RelayCommand(() =>
                    {
                        UserEMailText = Clipboard.GetText();
                    });
                return pasteMailCommand;
            }
        }

        private RelayCommand pasteKeyCommand;
        public RelayCommand PasteKeyCommand
        {
            get
            {
                if (pasteKeyCommand == null)
                    pasteKeyCommand = new RelayCommand(() =>
                    {
                        UserKeyText = Clipboard.GetText();
                    });
                return pasteKeyCommand;
            }
        }

        private RelayCommand startInstallCommand;
        public RelayCommand StartInstallCommand
        {
            get
            {
                if (startInstallCommand == null)
                    startInstallCommand = new RelayCommand(() =>
                    {
                        NextPageAnimation();

                        StartInstall();
                    });

                return startInstallCommand;
            }
        }

        private RelayCommand finishCommand;
        public RelayCommand FinishCommand
        {
            get
            {
                if (finishCommand == null)
                    finishCommand = new RelayCommand(() =>
                    {
                        if (IsLaunchApplication)
                            Launcher.LaunchExecutableFile(Settings.ExecutableFile, string.Empty);
                        
                        BootstrapperDispatcher.InvokeShutdown();
                    });

                return finishCommand;
            }
        }

        #region Socials

        private RelayCommand openFaceBookCommand;
        public RelayCommand OpenFaceBookCommand
        {
            get
            {
                if (openFaceBookCommand == null)
                    openFaceBookCommand = new RelayCommand(() =>
                    {
                        Animation.ButtonDownAnimation(view?.FacebookButton);
                        Process.Start("https://www.facebook.com/prsolution.com.ua");
                    });

                return openFaceBookCommand;
            }
        }

        private RelayCommand openBitBucketCommand;
        public RelayCommand OpenBitBucketCommand
        {
            get
            {
                if (openBitBucketCommand == null)
                    openBitBucketCommand = new RelayCommand(() =>
                    {
                        Animation.ButtonDownAnimation(view?.BucketkButton);
                        Process.Start("https://bitbucket.org/PR_solution/");
                    });

                return openBitBucketCommand;
            }
        }

        private RelayCommand openGitHubCommand;
        public RelayCommand OpenGitHubCommand
        {
            get
            {
                if (openGitHubCommand == null)
                    openGitHubCommand = new RelayCommand(() =>
                    {
                        Animation.ButtonDownAnimation(view?.GitButton);
                        Process.Start("https://github.com/PRSolution");
                    });

                return openGitHubCommand;
            }
        }

        private RelayCommand openMailCommand;
        public RelayCommand OpenMailCommand
        {
            get
            {
                if (openMailCommand == null)
                    openMailCommand = new RelayCommand(() =>
                    {
                        Animation.ButtonDownAnimation(view?.MailButton);
                        Process.Start("http://prsolution.com.ua/info@prsolution.com.ua");
                    });

                return openMailCommand;
            }
        }

        #endregion
        #endregion //RelayCommands

        #region Methods

        private void WrongEMail()
        {            
            UserEMailTitleText = view?.FindResource("Wrong").ToString() + view?.FindResource("EMail").ToString();
        }
        private void WrongActivationKey()
        {
            UserKeyTitleText = view?.FindResource("TimeOut").ToString() + view?.FindResource("ActivationKey").ToString();
        }

        private void NextPageAnimation()
        {
            Animation PageAnimation0 = new Animation();
            PageAnimation0.FrameOutNextAnimation(view?.MainFrame, MoveNextWithAnimation_Completed);
        }
        private void BackPageAnimation()
        {
            Animation PageAnimation0 = new Animation();
            PageAnimation0.FrameOutBackAnimation(view?.MainFrame, MoveBackWithAnimation_Completed);
        }
        private void LastPageAnimation()
        {
            Animation PageAnimation0 = new Animation();
            PageAnimation0.FrameOutNextAnimation(view?.MainFrame, MoveEndWithAnimation_Completed);
        }

        private void MoveNextWithAnimation_Completed()
        {
            MainFrame.Navigate(PagesIterator.GetNext());

            Animation PageAnimation1 = new Animation();
            PageAnimation1.IndicatorAnimation(PagesIterator.GetPosition(), view?.MenuIndicator);
        }
        private void MoveBackWithAnimation_Completed()
        {
            MainFrame.Navigate(PagesIterator.GetPrev());
            Animation PageAnimation1 = new Animation();
            PageAnimation1.IndicatorAnimation(PagesIterator.GetPosition(), view?.MenuIndicator);
        }
        private void MoveEndWithAnimation_Completed()
        {
            MainFrame.Navigate(PagesIterator.GetLast());
            Animation PageAnimation1 = new Animation();
            PageAnimation1.IndicatorAnimation(PagesIterator.GetPosition(), view?.MenuIndicator);
        }

        private void InstallerOnComplite()
        {
            InstallCacheAndRegistry();
        }
        private void DownloaderOnSizeAppPartsUpdate(long size)
        {
            MaxValAppPartsSize = size;
        }
        private void DownloaderOnDownloadDatabasesComplite()
        {
            InstallProgressValuePercent = 100;
            InstallProgressValue = MaxValAppPartsSize;

            NextPageAnimation();
        }
        
        private void StartInstall()
        {
            Installer.StartInstallation();
        }

        //private void InstallMSIExecute()
        //{
        //    IsThinking = true;
        //    Bootstrapper.Engine.Plan(LaunchAction.Install);

        //}

        //private void UninstallMSIExecute()
        //{
        //    IsThinking = true;
        //    Bootstrapper.Engine.Plan(LaunchAction.Uninstall);
        //}
        
        private void InstallCacheAndRegistry()
        {
            Configurator.CreateCacheFile();
            Configurator.ConfigurateRegistry();
            OnInstallComplete();
        }
        private void UnistallCacheAndRegistry()
        {
            Configurator.DeleteCacheFile();
            Configurator.ClearRegistry(Settings.CurrentPackage.ApplicationName);
            OnUnistallComplete();
        }
        
        private void OnInstallComplete()
        {
            InstallCompleted = view?.FindResource("InstallComplete").ToString();

            FinishInstallation();
        }
        private void OnUpDateComplete()
        {
            InstallCompleted = view?.FindResource("UpDateComplete").ToString();

            FinishInstallation();
        }
        private void FinishInstallation()
        {
            view.Dispatcher.Invoke
            (
                () =>
                {
                    NextPageAnimation();
                    Installer.DeleteTempDirectory();

                    JSONSerialization<PackageInformation>.Serialization(Settings.CurrentPackage, Settings.FullPath, "Settings");
                    //JSONSerialization.Serialization(Settings.CurrentPackage, Settings.FullPath, "Settings");
                }
            );
        }
        private void OnUnistallComplete()
        {
            InstallCompleted = view?.FindResource("UnInstallComplete").ToString();
            //"Uninstalling completed!";
            LaunchAppVisibility = Visibility.Collapsed;
            IsLaunchApplication = false;
            view.Dispatcher.Invoke
            (
                () =>
                {
                    LastPageAnimation();
                }
            );
        }
               
        private void OnPlanComplete(object sender, PlanCompleteEventArgs e)
        {
            if (e.Status >= 0)
                Bootstrapper.Engine.Apply(IntPtr.Zero);
        }
                
        private void DownloadSettingsFile()
        {
            Configurator.CreateTempDirectory();

            Downloader.DownloadSettingsFile
            (
                Settings.ServerVersionFileName,
                Settings.ServerVersionFileLink
            );
            DetectComponents();
        }
        private void DetectComponents()
        {
            //49-052-047-049-050-047-049-056-124-109

            //JSONSerialization<PackageInformation>.DeSerialization
            //(
            //    Settings.CurrentPackage,
            //    Settings.FullPath +
            //    Settings.ServerVersionFileName
            //);
            //PackageInformation ServerPI = new PackageInformation();
            //JSONSerialization<PackageInformation>.DeSerialization
            //(
            //    ServerPI,
            //    Settings.TempPath +
            //    Settings.ServerVersionFileName
            //);
            try
            {
                Settings.CurrentPackage = new PackageInformation
                (
                    JSONSerialization<PackageInformation>.DeSerialization
                    (
                        Settings.FullPath +
                        Settings.ServerVersionFileName
                    )
                );
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
            PackageInformation ServerPI = new PackageInformation();
            try
            {
                ServerPI = new PackageInformation
                (
                    JSONSerialization<PackageInformation>.DeSerialization
                    (
                        Settings.TempPath +
                        Settings.ServerVersionFileName
                    )
                );
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }

            bool UpDate = CheckVersion.Check(Settings.CurrentPackage.Version, ServerPI.Version);
            if (UpDate || Settings.CurrentPackage.Components.Count < 1)
            {
                Settings.CurrentPackage = ServerPI;
            }
            
            UpDateVisibility = Visibility.Hidden;
            InstallVisibility = Visibility.Visible;

            UninstallEnabled = false;
            InstallEnabled = false;

            if (Configurator.IsApplicationInstalled(Settings.CurrentPackage.ApplicationName))
            {
                //If already install
                UninstallEnabled = true;

                UnInstallButtonText = view?.FindResource("UNINSTALL").ToString();

                if (UpDate)
                {
                    //If update is ready
                    Downloader.OnDownloadComponentsComplite += OnUpDateComplete;

                    UpDateButtonText = view?.FindResource("UPDATE").ToString() + " " + Settings.CurrentPackage.Version;

                    UpDateVisibility = Visibility.Visible;
                    InstallVisibility = Visibility.Hidden;

                    InitializeUpDateIterator();
                }
                else
                {
                    //Without update
                    InstallButtonText = view?.FindResource("ISALREADYINSTALL").ToString();

                    view.Dispatcher.Invoke(() =>
                    {
                        UninstallCommand?.RaiseCanExecuteChanged();
                    });

                    InitializeUnInstallIterator();
                }
            }
            else
            {
                //If not install
                Downloader.OnDownloadComponentsComplite += Installer.Configurate;

                InstallEnabled = true;
                InstallButtonText = view?.FindResource("INSTALL").ToString();
                UnInstallButtonText = view?.FindResource("ISNOTUNINSTALL").ToString();

                view.Dispatcher.Invoke(() =>
                {
                    InstallCommand?.RaiseCanExecuteChanged();
                });

                InitializeInstallIterator();
            }
        }
        private void CheckConnecting(bool Connection)
        {
            if (!Connection)
            {
                MessageBox.Show(view?.FindResource("YouAreNotConnectedToTheInternet").ToString());
                BootstrapperDispatcher.InvokeShutdown();
            }
        }

        private void LanguageChanged(Object sender, EventArgs e)
        {
            if (Configurator.IsApplicationInstalled(Settings.CurrentPackage.ApplicationName))
            {
                InstallButtonText = view?.FindResource("ISALREADYINSTALL").ToString();
                UnInstallButtonText = view?.FindResource("UNINSTALL").ToString();
            }
            else
            {
                InstallButtonText = view?.FindResource("INSTALL").ToString();
                UnInstallButtonText = view?.FindResource("ISNOTUNINSTALL").ToString();
            }

            UpDateButtonText = view?.FindResource("UPDATE").ToString() + " " + Settings.CurrentPackage.Version;
            UserKeyTitleText = view?.FindResource("EMail").ToString();
            UserEMailTitleText = view?.FindResource("ActivationKey").ToString();
        }
        
        private void InitializeInstallIterator()
        {
            List<object> PageCollection = new List<object>()
            {
                new LanguagePage(),
                new MainPage(),
                new LicensePage(),
                new ActivationPage(),
                new SelectPathPage(),
                new InstallPage(),
                new FinishPage()
            };
            PagesIterator = new IteratorBrowser(new Iterator(PageCollection));

            view?.MenuIterator.Children.Clear();

            TextBlock MenuText2 = (TextBlock)ControlClone(InitializeTextBlock());
            MenuText2.SetResourceReference(TextBlock.TextProperty, "License");
            view?.MenuIterator.Children.Add(MenuText2);

            TextBlock MenuText3 = (TextBlock)ControlClone(MenuText2);
            MenuText3.SetResourceReference(TextBlock.TextProperty, "Activation");
            view?.MenuIterator.Children.Add(MenuText3);

            TextBlock MenuText4 = (TextBlock)ControlClone(MenuText2);
            MenuText4.SetResourceReference(TextBlock.TextProperty, "Options");
            view?.MenuIterator.Children.Add(MenuText4);

            TextBlock MenuText5 = (TextBlock)ControlClone(MenuText2);
            MenuText5.SetResourceReference(TextBlock.TextProperty, "Install");
            view?.MenuIterator.Children.Add(MenuText5);

            TextBlock MenuText6 = (TextBlock)ControlClone(MenuText2);
            MenuText6.SetResourceReference(TextBlock.TextProperty, "Finish");
            view?.MenuIterator.Children.Add(MenuText6);

        }
        private void InitializeUnInstallIterator()
        {            
            InitializeUpDateIterator();

            view?.MenuIterator.Children.Clear();

            TextBlock MenuText3 = (TextBlock)ControlClone(InitializeTextBlock());
            MenuText3.SetResourceReference(TextBlock.TextProperty, "UnInstall");
            view?.MenuIterator.Children.Add(MenuText3);

            TextBlock MenuText4 = (TextBlock)ControlClone(MenuText3);
            MenuText4.SetResourceReference(TextBlock.TextProperty, "Finish");
            view?.MenuIterator.Children.Add(MenuText4);
        }
        private void InitializeUpDateIterator()
        {
            List<object> PageCollection = new List<object>()
            {
                new LanguagePage(),
                new MainPage(),
                new InstallPage(),
                new FinishPage()
            };
            PagesIterator = new IteratorBrowser(new Iterator(PageCollection));            
        }

        private void UnInstallMenuItems()
        {
            view?.MenuIterator.Children.Clear();

            TextBlock MenuText3 = (TextBlock)ControlClone(InitializeTextBlock());
            MenuText3.SetResourceReference(TextBlock.TextProperty, "UnInstall");
            view?.MenuIterator.Children.Add(MenuText3);

            TextBlock MenuText4 = (TextBlock)ControlClone(MenuText3);
            MenuText4.SetResourceReference(TextBlock.TextProperty, "Finish");
            view?.MenuIterator.Children.Add(MenuText4);
        }
        private void UpDateMenuItems()
        {
            view?.MenuIterator.Children.Clear();

            TextBlock MenuText3 = (TextBlock)ControlClone(InitializeTextBlock());
            MenuText3.SetResourceReference(TextBlock.TextProperty, "UpDate");
            view?.MenuIterator.Children.Add(MenuText3);

            TextBlock MenuText4 = (TextBlock)ControlClone(MenuText3);
            MenuText4.SetResourceReference(TextBlock.TextProperty, "Finish");
            view?.MenuIterator.Children.Add(MenuText4);
        }

        private TextBlock InitializeTextBlock()
        {
            TextBlock MenuText0 = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                FontSize = 25,
                Margin = new Thickness(0, 7, 0, 8),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Source Sans Pro"),
                Foreground = new SolidColorBrush(Color.FromArgb(180, 0, 0, 0))
            };
            MenuText0.SetResourceReference(TextBlock.TextProperty, "Language");
            view?.MenuIterator.Children.Add(MenuText0);

            TextBlock MenuText1 = (TextBlock)ControlClone(MenuText0);
            MenuText1.SetResourceReference(TextBlock.TextProperty, "Welcome");
            view?.MenuIterator.Children.Add(MenuText1);

            return MenuText0;
        }
        private object ControlClone(object item)
        {
            string itemXaml = XamlWriter.Save(item);
            StringReader stringReader = new StringReader(itemXaml);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return XamlReader.Load(xmlReader);
        }

        #endregion // Methods

        #region Events

        public void WindowLoaded()
        {
            Installer.DeleteTempDirectory();

            AppInstaller.LanguageChanged += LanguageChanged;
            DownloadSettingsFile();
            
            MainFrame.Navigate(PagesIterator.GetNext());
            
            Animation animation = new Animation();
            animation.ImageAnimation(view?.MainBorder.ActualWidth, view?.BackGroundImage);
            
            UserEMailTitleText = view?.FindResource("EMail").ToString();
            UserKeyTitleText = view?.FindResource("ActivationKey").ToString();
        }

        public void DragWindow()
        {
            view.DragMove();
        }

        #endregion // Events
    }
}
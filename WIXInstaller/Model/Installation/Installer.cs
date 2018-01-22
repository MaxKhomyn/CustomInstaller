using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using InstallerWIX.Model;
using static WIXInstaller.AppInstaller;
using static InstallerWIX.Model.Settings;
using WIXInstaller.Model.GoogleDrive;
using System.Diagnostics;

namespace WIXInstaller.Model
{
    public static class Installer
    {
        public delegate void OnInstallComplite();

        public static event OnInstallComplite OnComplite;

        public static void StartInstallation()
        {
            Task.Factory.StartNew(() =>
            {
                if (!Configurator.CreateTempDirectory())
                {
                    MessageBox.Show("Error Installation. Could not create temporary installer folder.");
                    return;
                }

            }).ContinueWith((t) =>
            {
                try
                {
                    CreateApplicationDirectories();
                }
                catch (Exception) { MessageBox.Show("Error Installation. Could not create application directory."); }

            }).ContinueWith((t) =>
            {
                try
                {
                    Downloader.DownloadComponents();
                }
                catch (Exception)
                { }
            });
            //DriveDownloader.DownloadAppParts();
        }

        private static void CreateApplicationDirectories()
        {
            string subdirectoryPath = viewModel.InstallFolderPath + $@"\{Settings.CurrentPackage.ApplicationName}";

            if (Directory.Exists(subdirectoryPath))
                return;

            if (Directory.Exists(viewModel.InstallFolderPath) && !Directory.Exists(subdirectoryPath))
            {
                Directory.CreateDirectory(subdirectoryPath);
            }
            else
            {
                DirectoryInfo mainDirectory = Directory.CreateDirectory(viewModel.InstallFolderPath);
                mainDirectory.CreateSubdirectory(Settings.CurrentPackage.ApplicationName);
            }
        }
        
        public static void Configurate()
        {
            Configurator.CreateStartMenuDirectory();
            Configurator.CreateShortcuts();

            view.Dispatcher.Invoke(() => { OnComplite?.Invoke(); });
        }

        public static void StartUninstallation()
        {
            DeleteApplicationDirectory();
            DeleteShorcuts();
        }

        private static void DeleteApplicationDirectory()
        {
            string applicationPath = Configurator.GetRegistryValue(CurrentPackage.ApplicationName, "ApplicationPath");
            if (Directory.Exists(applicationPath))
                Directory.Delete(applicationPath, true);
        }
        public static void DeleteTempDirectory()
        {
            try
            {
                if (Directory.Exists(TempPath))
                {
                    Directory.Delete(TempPath, true);
                }
            }
            catch(Exception exc) { Debug.WriteLine(exc); }
        }

        private static void DeleteShorcuts()
        {
            Configurator.DeleteStartMenuDirectory();
            Configurator.DeleteDesktopShortcuts();
        }
    }
}
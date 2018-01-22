using System.IO;
using System.Threading.Tasks;
using static InstallerWIX.Model.Settings;

namespace WIXInstaller.Model.Installation
{
    class Updater
    {
        public static void StartUpDate()
        {
            Task.Factory.StartNew(() =>
            {
                if (Directory.Exists(FullPath))
                    Directory.Delete(FullPath, true);
            }).ContinueWith((t) =>
            {
                Directory.CreateDirectory(FullPath);
                Downloader.DownloadComponents();
            });
        }
    }
}
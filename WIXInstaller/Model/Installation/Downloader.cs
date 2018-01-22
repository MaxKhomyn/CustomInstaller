using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using WIXInstaller.Services;
using static InstallerWIX.Model.Settings;
using static WIXInstaller.AppInstaller;

namespace WIXInstaller.Model
{
    public static class Downloader
    {
        #region Delegates

        public delegate void OnDownloadComplited();
        public delegate void OnSizeUpdate(long size);

        #endregion

        #region Events

        public static event OnDownloadComplited OnDownloadComponentsComplite;
        public static event OnSizeUpdate OnSizeComponentsUpdate;

        #endregion

        #region Properties

        private static WebClient _client;
        private static long ComponentsSize;
        private static long bytesIn;

        #endregion

        #region Methods

        public static async void DownloadComponents()
        {
            try
            {
                SetSizeProgressBar
                (
                    CurrentPackage.Components.Sum
                    (
                        Component => GetFileSizeAsync(Component.Link).Result
                    )
                );
                
                _client = new WebClient();

                _client.DownloadProgressChanged += ClientOnDownloadProgressChanged;
                
                foreach (var Component in CurrentPackage.Components)
                {
                    string TmpPath = TempPath + Component.PackageName;
                    bytesIn = 0;
                    try
                    {
                        if (File.Exists(TempPath)) File.Delete(TmpPath);
                        
                        await _client.DownloadFileTaskAsync(new Uri(Component.Link), TmpPath);
                        System.Threading.Thread.Sleep(10);
                        RARService.ExtractFiles(TmpPath, viewModel.InstallFolderPath + $@"\{CurrentPackage.ApplicationName}");
                    }
                    catch (WebException e)
                    {
                        MessageBox.Show(e.Message);
                        Debug.WriteLine(e.Message);
                    }
                }
                OnDownloadComponentsComplite?.Invoke();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Debug.WriteLine(e.Message);
            }
        }
        public static void DownloadSettingsFile(string FileName, string FileLink)
        {
            _client = new WebClient();

            string TmpPath = TempPath + FileName;
            try
            {
                if (File.Exists(TmpPath)) { File.Delete(TmpPath); MessageBox.Show(File.Exists(TmpPath).ToString()); };
                _client.DownloadFile(new Uri(FileLink), TmpPath);
            }
            catch (WebException e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private static async Task<long> GetFileSizeAsync(string url)
        {
            long size = 0;
            try
            {
                WebRequest req = WebRequest.Create(url);
                req.Timeout = 8000;
                req.Method = "HEAD";

                using (WebResponse resp = await req.GetResponseAsync().ConfigureAwait(false))
                {
                    if (long.TryParse(resp.Headers.Get("Content-Length"), out long contentLength))
                    {
                        size += contentLength;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return size;
        }
        
        private static void ClientOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs downloadProgressChangedEventArgs)
        {
            try
            {
                long downloadBytes = downloadProgressChangedEventArgs.BytesReceived - bytesIn;
                bytesIn = downloadProgressChangedEventArgs.BytesReceived;

                viewModel.InstallProgressValue += downloadBytes;
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
            //viewModel.InstallProgressValue += downloadBytes;
            //viewModel.InstallProgressValuePercent += downloadBytes / viewModel.MaxValAppPartsSize * 100;
            //viewModel.InstallProgressValuePercent = (downloadBytes / ComponentsSize) * 100;
        }
        
        private static void SetSizeProgressBar(long size)
        {
            try
            {
                ComponentsSize = size;
                OnSizeComponentsUpdate?.Invoke(size);
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        #endregion
    }
}
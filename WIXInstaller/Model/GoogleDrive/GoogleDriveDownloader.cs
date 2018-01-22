using System;
using System.IO;
using System.Threading;
using Google.Apis.Services;
using Google.Apis;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using Google.Apis.Auth.OAuth2;
using System.Collections.Generic;

namespace GoogleDrive.Model
{
    public class GoogleDriveDownloader
    {
        #region Delegates

        public delegate void OnDownloadComplite();

        public delegate void OnSizeUpdate(long size);

        #endregion

        #region Events

        public static event OnDownloadComplite OnDownloadAppPartsComplite;

        public static event OnSizeUpdate OnSizeAppPartsUpdate;

        #endregion

        #region Propeties

        private static string ApplicationName;

        public static long? size = 0;
        private static string[] Scopes;
        private static Google.Apis.Drive.v3.Data.File FileToDownload { get; set; }

        private static UserCredential US { get; set; }
        private static DriveService service { get; set; }
        private static FilesResource.GetRequest request { get; set; }

        #endregion

        #region Methods

        public GoogleDriveDownloader()
        {
            ApplicationName = "ProjectSolutionInstaller";
            //Scopes = new string[]{ DriveService.Scope.Drive };
        }

        private static UserCredential GetUserCredentials(string TokensFileName)
        {
            using (var stream = new FileStream(TokensFileName, FileMode.Open, FileAccess.Read))
            {
                string creadPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                creadPath = Path.Combine(creadPath, "driveApiCredentials", "drive-credentials.json");

                return GoogleWebAuthorizationBroker.AuthorizeAsync
                (
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "User",
                    CancellationToken.None,
                    new FileDataStore(creadPath, true)
                ).Result;
            }
        }
        private static DriveService GetDriveService()
        {
            return new DriveService
            (
                new BaseClientService.Initializer()
                {
                    HttpClientInitializer = US,
                    ApplicationName = ApplicationName
                }
            );
        }

        private static string GetFile(string FileName)
        {
            IList<Google.Apis.Drive.v3.Data.File> files = service.Files.List().Execute().Files;
            foreach (var file in files)
            {
                if (file.Name == FileName) { FileToDownload = file; break; }
            }
            size = FileToDownload.Size;
            return FileToDownload.Id;
        }

        public static async void DownloadAppParts(string FileName, string DiskPath, string TokensFileName)
        {
            US = GetUserCredentials(TokensFileName);
            service = GetDriveService();

            request = service.Files.Get(GetFile(FileName));

            //SetSizeProgressBar(size.Value);

            string path = DiskPath + FileName;

            using (var stream = new MemoryStream())
            {
                //request.MediaDownloader.ProgressChanged += ClientOnDownloadProgressChanged;

                //await request.DownloadAsync(stream);

                using (var filestream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    filestream.Write(stream.GetBuffer(), 0, stream.GetBuffer().Length);
                }
            }
            OnDownloadAppPartsComplite?.Invoke();
        }

        public static void DownloadFile(string FileName, string PathToSave)
        {
            //try
            //{
            //JsonSerialize.Serialize(new Tokens());

            //US = GetUserCredentials();
            //service = GetDriveService();

            ////"MailClient.1.0.0.0.7z"
            //var request = service.Files.Get(GetFile(FileName));

            //using (var stream = new MemoryStream())
            //{
            //    request.MediaDownloader.ProgressChanged += (IDownloadProgress Progress) =>
            //    {
            //        switch (Progress.Status)
            //        {
            //            case DownloadStatus.Downloading:
            //                MessageBox.Show(Progress.BytesDownloaded.ToString());
            //                break;
            //            case DownloadStatus.Completed:
            //                MessageBox.Show("Complete!");
            //                break;
            //            case DownloadStatus.Failed:
            //                MessageBox.Show("Failed");
            //                break;
            //        }
            //    };
            //    request.DownloadAsync(stream);

            //    using (var filestream = new FileStream(PathToSave + "Settings", FileMode.Create, FileAccess.Write))
            //    {
            //        filestream.WriteAsync(stream.GetBuffer(), 0, stream.GetBuffer().Length);
            //    }
            //}
            //}
            //catch { }
        }

        private static void ClientOnDownloadProgressChanged(IDownloadProgress ProgressChangedEventArgs)
        {
            long downloadBytesPercent = ProgressChangedEventArgs.BytesDownloaded / (size.Value / 100);

            //viewModel.InstallProgressValue = ProgressChangedEventArgs.BytesDownloaded;
            //viewModel.InstallProgressValuePercent = downloadBytesPercent;
        }

        private static void SetSizeProgressBar(long size)
        {
            OnSizeAppPartsUpdate?.Invoke(size);
        }

        #endregion
    }
}
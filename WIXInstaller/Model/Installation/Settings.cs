using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using WIXInstaller.Model.Installation;
using static WIXInstaller.AppInstaller;

namespace InstallerWIX.Model
{
    public static class Settings
    {
        #region Settings

        public static PackageInformation CurrentPackage = new PackageInformation();
        public static string ServerVersionFileName = "Settings";
        public static string ServerVersionFileLink = "https:/" + "/github.com/JacobyShaddix/MailClient/raw/master/Settings";

        #endregion
        #region Pathes

        internal static string FullPath =
            viewModel.InstallFolderPath + $@"\{CurrentPackage.ApplicationName}\";

        internal static string ExecutableFile =
            viewModel.InstallFolderPath + $@"\{CurrentPackage.ApplicationName}\{CurrentPackage.ExeName}";

        internal static string IconLocation = 
            viewModel.InstallFolderPath + $@"\{CurrentPackage.ApplicationName}\{CurrentPackage.ExeName}";

        internal static string TempPath => Path.GetTempPath() + $@"{CurrentPackage.ApplicationName}\";

        internal static string ProgramFilesPath
            => Environment.GetFolderPath(Environment.Is64BitOperatingSystem
            ? Environment.SpecialFolder.ProgramFilesX86
            : Environment.SpecialFolder.ProgramFiles);

        internal const int EstimatedSize = 29912;
        internal static string RequiredSize = "2";

        #endregion

        public static long GetFreeDiscSpace(string discRoot)
        {
            string driveName = Path.GetPathRoot(discRoot);
            DriveInfo drive = new DriveInfo(driveName);
            long size = drive.TotalFreeSpace;
            return (size / 1048000);
        }
    }
}
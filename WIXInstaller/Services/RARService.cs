using System;
using System.Diagnostics;
using System.IO.Compression;
using System.Windows;

namespace WIXInstaller.Services
{
    public class RARService
    {
        public static void ExtractFiles(string ZipPath, string ExtractPath)
        {
            try
            {
                ZipFile.ExtractToDirectory(ZipPath, ExtractPath);

                //using (ZipArchive archive = ZipFile.OpenRead(ZipPath))
                //{
                //    foreach (ZipArchiveEntry entry in archive.Entries)
                //    {
                //        entry.ExtractToFile(ExtractPath);
                //    }
                //}
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Debug.WriteLine(e.Message);
            }
        }
    }
}
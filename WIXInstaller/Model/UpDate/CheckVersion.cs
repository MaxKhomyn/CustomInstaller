using System;

namespace WIXInstaller.Model.UpDate
{
    class CheckVersion
    {
        public static bool Check(string CurrentVersion, string VersionFromServer) => DecodeVersion(CurrentVersion) < DecodeVersion(VersionFromServer);

        private static double DecodeVersion(string Version)
        {
            double result = 0; int index = 1;
            string[] spliter = Version.Split('.');

            for (int i = spliter.Length - 1; i >= 0; i--)
            {
                result += Convert.ToInt32(spliter[i]) * Math.Pow(10, index);
                index++;
            }

            return result;
        }
    }
}

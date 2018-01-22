using System.Collections.Generic;

namespace WIXInstaller.Model.Installation
{
    [System.Serializable]
    public class PackageInformation
    {
        #region Company

        public string Manufacturer { get; set; } = "P.R. Solution";
        public string SupportUrl { get; set; } = "http:/" + "/prsolution.com.ua/";

        #endregion

        #region App

        public string ApplicationName { get; set; } = "Mail Client";
        public string ExeName { get; set; } = "MailClient.exe";
        public string Version { get; set; } = "0.0.0.0";

        #endregion

        #region Server

        public List<PackageComponent> Components { get; set; } = new List<PackageComponent>
        {
            //new PackageComponent
            //{
                //Link = "https:/"+"/github.com/JacobyShaddix/MailClient/raw/master/Package.1.0.0.0.zip",
                //PackageName = "Package.1.0.0.0.zip"
            //}
        };

        #endregion

        public PackageInformation() { }
        public PackageInformation(PackageInformation packageInformation)
        {
            Manufacturer = packageInformation.Manufacturer;
            SupportUrl = packageInformation.SupportUrl;

            ApplicationName = packageInformation.ApplicationName;
            ExeName = packageInformation.ExeName;
            Version = packageInformation.Version;

            Components.Clear();
            foreach (var Component in packageInformation.Components)
            {
                Components.Add(new PackageComponent(Component));
            }
        }
    }
}

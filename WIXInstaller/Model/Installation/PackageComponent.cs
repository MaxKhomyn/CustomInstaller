namespace WIXInstaller.Model.Installation
{
    [System.Serializable]
    public class PackageComponent
    {
        public string Link { get; set; }
        public string PackageName { get; set; }
        public string PackageSize { get; set; }

        public PackageComponent() { }
        public PackageComponent(PackageComponent Component)
        {
            Link = Component.Link;
            PackageName = Component.PackageName;
            PackageSize = Component.PackageSize;
        }
    }
}

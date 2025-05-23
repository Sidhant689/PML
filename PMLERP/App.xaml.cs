namespace PMLERP
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzg3NTk2OUAzMjM5MmUzMDJlMzAzYjMyMzkzYmhsOVZncGxyRmR2VnB2UituS2VjdFJWMm1mdVFWaGRQcnN2UHEreEdaems9;Mzg3NTk3MEAzMjM5MmUzMDJlMzAzYjMyMzkzYkp5aG1nc2FQNWhRWVkvc3UzeHVVNzJIZlBackgrdU5nRnYwY2N4VXpWUWM9");
        }
    }
}

using MyRecipeBookMaker.Models;
using MyRecipeBookMaker.Common;
using AsyncAwaitBestPractices;
using PlatformIntegrationDemo.Models;
namespace MyRecipeBookMaker
{
    public partial class App : Application
    {
        public static UserSessionData currentSessionData;
        public static string ImageServerPath { get; } = "https://cdn.syncfusion.com/essential-ui-kit-for-.net-maui/common/uikitimages/";

        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                window.Width = 650; // iPhone width in points
            }
            return window;
        }
        protected override void OnStart()
        {
            // Handle when your app is activated

            currentSessionData = new UserSessionData();
            currentSessionData.LoadData().SafeFireAndForget();
            GetSecretsHelper.GetSecrets().SafeFireAndForget();
#if ANDROID

            List<PermissionItem> permissions = new List<PermissionItem>();
            permissions.Add(new PermissionItem("Storage (Read)", new Permissions.StorageRead()));
            permissions.Add(new PermissionItem("Storage (Write)", new Permissions.StorageWrite()));
            permissions[0].CheckStatusCommand.Execute(null);
#endif



        }



    }
}
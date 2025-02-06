using MyRecipeBookMaker.Models;
using MyRecipeBookMaker.Common;
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
            GetSecretsHelper.GetSecrets();
        }
        
 
     
    }
}
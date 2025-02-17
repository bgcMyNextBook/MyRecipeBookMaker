using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;

using DevExpress.Maui;
using DevExpress.Maui.Core;

using Microsoft.Extensions.Logging;

using MyRecipeBookMaker.ViewModels;
using MyRecipeBookMaker.Views;

using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.Toolkit.Hosting;
namespace MyRecipeBookMaker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            ThemeManager.UseAndroidSystemColor = false;
            ThemeManager.Theme = new Theme(ThemeSeedColor.TealGreen);
            var builder = MauiApp.CreateBuilder();
            builder
                .ConfigureSyncfusionCore()
                .ConfigureSyncfusionToolkit()
                .UseMauiApp<App>()
                .UseDevExpressControls()
                .UseDevExpress()
                .UseDevExpressCollectionView()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitCamera()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Roboto-Medium.ttf", "Roboto-Medium");
                    fonts.AddFont("Roboto-Regular.ttf", "Roboto-Regular");
                    fonts.AddFont("materialdesignicons-webfont.ttf", "IMD");
                    fonts.AddFont("Segoe UI.ttf", "Segoe UI");
                });
            builder.Services.AddTransientPopup<RecipeCollectionItemPopup, RecipeCollectionItemPopupViewModel>();
            builder.Services.AddSingleton<RecipeCollection>();
            builder.Services.AddSingleton<RecipeCollectionViewModel>();
            builder.Services.AddTransient<IPopupService, PopupService>();

            //Register Syncfusion license https://help.syncfusion.com/common/essential-studio/licensing/how-to-generate
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWX5cdXVVR2FZUEx2WEE=");

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

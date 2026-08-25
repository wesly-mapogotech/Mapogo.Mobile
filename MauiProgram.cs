using Mapogo.Mobile.Services;
using Microsoft.Extensions.Logging;

namespace Mapogo.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<ConfigurationService>();
            builder.Services.AddSingleton<AndroidThemeService>();
            builder.Services.AddSingleton<SplashService>();
            return builder.Build();
        }
    }
}

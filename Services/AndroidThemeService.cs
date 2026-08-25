using Mapogo.Mobile.Models;

#if ANDROID
using AndroidColor = Android.Graphics.Color;
using Microsoft.Maui.ApplicationModel;
#endif

namespace Mapogo.Mobile.Services;

public class AndroidThemeService
{
    public void Apply(AppConfig config)
    {
#if ANDROID
        var activity = Platform.CurrentActivity;

        if (activity?.Window == null)
            return;

        var window = activity.Window;

        // Status bar
        window.SetStatusBarColor(
            AndroidColor.ParseColor(
                config.Theme.StatusBarColor));

        // Navigation bar
        window.SetNavigationBarColor(
            AndroidColor.ParseColor(
                config.Theme.NavigationBarColor));

        // Status bar icons
        window.DecorView.SystemUiVisibility = 0;
#endif
    }
}
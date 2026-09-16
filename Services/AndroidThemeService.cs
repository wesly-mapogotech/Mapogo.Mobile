using Mapogo.Mobile.Models;

#if ANDROID
using AndroidColor = Android.Graphics.Color;
using AndroidWindow = Android.Views.Window;
using AndroidView = Android.Views.View;
using AndroidX.Core.View;
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

        AndroidWindow window = activity.Window;

        if (OperatingSystem.IsAndroidVersionAtLeast(35))
        {
            ApplyAndroid35Plus(window);
        }
        else
        {
            ApplyAndroidPre35(window, config);
        }
#endif
    }

#if ANDROID

    private static void ApplyAndroidPre35(
        AndroidWindow window,
        AppConfig config)
    {
        var statusBarColor =
            AndroidColor.ParseColor(
                config.Theme.StatusBarColor);

        var navigationBarColor =
            AndroidColor.ParseColor(
                config.Theme.NavigationBarColor);

        window.SetStatusBarColor(statusBarColor);
        window.SetNavigationBarColor(navigationBarColor);

        var controller =
            WindowCompat.GetInsetsController(
                window,
                window.DecorView);

        if (controller != null)
        {
            controller.AppearanceLightStatusBars = false;
            controller.AppearanceLightNavigationBars = false;
        }

        WindowCompat.SetDecorFitsSystemWindows(
            window,
            true);
    }

    private static void ApplyAndroid35Plus(
        AndroidWindow window)
    {
        WindowCompat.SetDecorFitsSystemWindows(
            window,
            false);

        var controller =
            WindowCompat.GetInsetsController(
                window,
                window.DecorView);

        if (controller != null)
        {
            // false = white system bar icons
            controller.AppearanceLightStatusBars = false;
            controller.AppearanceLightNavigationBars = false;
        }

        var contentView =
            window.DecorView.FindViewById(
                Android.Resource.Id.Content);

        if (contentView == null)
            return;

        ViewCompat.SetOnApplyWindowInsetsListener(
            contentView,
            new SystemBarInsetsListener());

        ViewCompat.RequestApplyInsets(contentView);
    }

    private sealed class SystemBarInsetsListener
        : Java.Lang.Object,
          IOnApplyWindowInsetsListener
    {
        public WindowInsetsCompat OnApplyWindowInsets(
            AndroidView view,
            WindowInsetsCompat insets)
        {
            var systemBars =
                insets.GetInsets(
                    WindowInsetsCompat.Type.SystemBars());

            view.SetPadding(
                systemBars.Left,
                systemBars.Top,
                systemBars.Right,
                systemBars.Bottom);

            return insets;
        }
    }

#endif
}
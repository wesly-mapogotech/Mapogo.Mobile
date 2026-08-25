namespace Mapogo.Mobile.Models
{
    public class AppConfig
    {
        public string AppName { get; set; } = "Mapogo";

        public string SiteUrl { get; set; } =
            "https://mapogo.chat";

        public string LogoUrl { get; set; } =
            "https://mapogo.chat/assets/logo.png";

        public string SplashUrl { get; set; } = string.Empty;
        public int SplashDurationMilliseconds { get; set; } = 3000;
        public ThemeConfig Theme { get; set; } = new();
    }
    public class ThemeConfig
    {
        public string PrimaryColor { get; set; } = "#000000";
        public string StatusBarColor { get; set; } = "#000000";
        public string NavigationBarColor { get; set; } = "#000000";
        public string LoadingIndicatorColor { get; set; } = "#000000";
    }
}

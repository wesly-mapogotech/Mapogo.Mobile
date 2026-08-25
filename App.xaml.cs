using Mapogo.Mobile.Services;

namespace Mapogo.Mobile
{
    public partial class App : Application
    {
        private readonly ConfigurationService _configurationService;
        private readonly AndroidThemeService _themeService;
        private readonly SplashService _splashService;
        public App(ConfigurationService configurationService, AndroidThemeService themeService, SplashService splashService)
        {
            InitializeComponent();

            _configurationService = configurationService;
            _themeService = themeService;
            _splashService = splashService;

            MainPage = new ContentPage
            {
                Content = new ActivityIndicator
                {
                    IsRunning = true,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };

            InitializeAsync();
        }
        private async void InitializeAsync()
        {
            var config =
                await _configurationService.GetConfigAsync();
            // 2. Apply Android theme
            _themeService.Apply(config);
            MainPage = new MainPage(config, _splashService);
        }
    }
}
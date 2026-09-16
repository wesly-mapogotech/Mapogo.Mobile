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
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var loadingPage = new ContentPage
            {
                Content = new ActivityIndicator
                {
                    IsRunning = true,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };

            var window = new Window(loadingPage);

            _ = InitializeAsync(window);

            return window;
        }

        private async Task InitializeAsync(Window window)
        {
            var config = await _configurationService.GetConfigAsync();
            // 2. Apply Android theme
            _themeService.Apply(config);
            window.Page = new MainPage(config, _splashService);
        }
    }
}
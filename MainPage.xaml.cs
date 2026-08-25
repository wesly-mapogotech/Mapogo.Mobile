using Mapogo.Mobile.Models;
using Mapogo.Mobile.Services;

namespace Mapogo.Mobile
{
    public partial class MainPage : ContentPage
    {
        private readonly AppConfig _config;
        private readonly SplashService _splashService;

        private bool _splashReady;
        private bool _webViewLoaded;
        private bool _splashHidden;


        private DateTime _splashStartTime;

        public MainPage(AppConfig config, SplashService splashService)
        {
            InitializeComponent();

            _config = config;
            _splashService = splashService;
            ApplyTheme(config);
            MapogoWebView.Navigating += OnNavigating;
            MapogoWebView.Navigated += OnNavigated;

            MapogoWebView.Source = _config.SiteUrl;
        }

        private async void OnNavigating(
            object? sender,
            WebNavigatingEventArgs e)
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            // WebView sudah selesai loading
            _webViewLoaded = true;

            // Coba sembunyikan splash
            await TryHideSplashAsync();
        }

        private void OnNavigated(
            object? sender,
            WebNavigatedEventArgs e)
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }

        protected override bool OnBackButtonPressed()
        {
            if (MapogoWebView.CanGoBack)
            {
                MapogoWebView.GoBack();

                return true;
            }

            return base.OnBackButtonPressed();
        }

        private void ApplyTheme(AppConfig config)
        {
            var primaryColor =
                Color.FromArgb(config.Theme.PrimaryColor);
            BackgroundColor = primaryColor;


            var loadingIndicatorColor = Color.FromArgb(config.Theme.LoadingIndicatorColor);
            LoadingIndicator.Color = loadingIndicatorColor;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            _splashStartTime = DateTime.UtcNow;
            await InitializeSplashAsync();
        }

        private async Task InitializeSplashAsync()
        {
            try
            {
                var cachedSplash =
                    _splashService.GetCachedSplash();

                if (!string.IsNullOrWhiteSpace(cachedSplash))
                {
                    // Cache langsung ditampilkan.
                    SplashImage.Source =
                        ImageSource.FromFile(
                            cachedSplash);

                    _splashReady = true;

                    await TryHideSplashAsync();

                    // Update splash di background.
                    _ = UpdateSplashInBackgroundAsync();

                    return;
                }

                // First launch:
                // download splash terlebih dahulu.
                var downloadedSplash =
                    await _splashService.DownloadSplashAsync(
                        _config.SplashUrl);

                if (!string.IsNullOrWhiteSpace(downloadedSplash))
                {
                    SplashImage.Source =
                        ImageSource.FromFile(
                            downloadedSplash);
                }

                _splashReady = true;

                await TryHideSplashAsync();
            }
            catch
            {
                // Jangan sampai splash merusak startup aplikasi.

                _splashReady = true;

                await TryHideSplashAsync();
            }
        }

        private async Task UpdateSplashInBackgroundAsync()
        {
            try
            {
                await _splashService.DownloadSplashAsync(
                    _config.SplashUrl);
            }
            catch
            {
                // Ignore.
            }
        }

        private async Task TryHideSplashAsync()
        {
            if (_splashHidden)
                return;

            if (!_splashReady)
                return;

            if (!_webViewLoaded)
                return;

            var elapsed =
        DateTime.UtcNow - _splashStartTime;

            var remaining =
                TimeSpan.FromMilliseconds(_config.SplashDurationMilliseconds)
                - elapsed;

            if (remaining > TimeSpan.Zero)
            {
                await Task.Delay(remaining);
            }
            await HideSplashAsync();
        }

        private async Task HideSplashAsync()
        {
            if (_splashHidden)
                return;

            _splashHidden = true;

            await SplashOverlay.FadeTo(
                0,
                300,
                Easing.CubicOut);

            SplashOverlay.IsVisible = false;
            SplashOverlay.InputTransparent = true;
        }
    }
}

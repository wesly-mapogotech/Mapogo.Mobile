namespace Mapogo.Mobile.Services
{
    public class SplashService
    {
        private const string CacheFileName =
            "mapogo_splash.png";

        private readonly HttpClient _httpClient;

        public SplashService()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }

        public string CachePath =>
            Path.Combine(
                FileSystem.CacheDirectory,
                CacheFileName);

        public bool HasCachedSplash =>
            File.Exists(CachePath);

        public string? GetCachedSplash()
        {
            if (!File.Exists(CachePath))
                return null;

            return CachePath;
        }

        public async Task<string?> DownloadSplashAsync(
            string url,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(url))
                return GetCachedSplash();

            var tempPath = CachePath + ".tmp";

            try
            {
                using var response =
                    await _httpClient.GetAsync(
                        url,
                        HttpCompletionOption.ResponseHeadersRead,
                        cancellationToken);

                response.EnsureSuccessStatusCode();

                await using var networkStream =
                    await response.Content.ReadAsStreamAsync(
                        cancellationToken);

                await using var fileStream =
                    new FileStream(
                        tempPath,
                        FileMode.Create,
                        FileAccess.Write,
                        FileShare.None);

                await networkStream.CopyToAsync(
                    fileStream,
                    cancellationToken);

                fileStream.Close();

                // Hanya replace cache setelah
                // download selesai dengan sukses.
                File.Move(
                    tempPath,
                    CachePath,
                    true);

                return CachePath;
            }
            catch
            {
                // Bersihkan file sementara.
                try
                {
                    if (File.Exists(tempPath))
                        File.Delete(tempPath);
                }
                catch
                {
                    // Ignore cleanup error.
                }

                // Gunakan cache lama jika ada.
                return GetCachedSplash ();
            }
        }
    }
}
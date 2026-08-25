using Mapogo.Mobile.Models;
using System.Text.Json;

namespace Mapogo.Mobile.Services
{
    public class ConfigurationService
    {
        private const string ConfigUrl = "https://wesly-mapogotech.github.io/Pages/mapogo.json";

        public async Task<AppConfig> GetConfigAsync()
        {
            try
            {
                using var client = new HttpClient();

                var json = await client.GetStringAsync(ConfigUrl);

                var config = JsonSerializer.Deserialize<AppConfig>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                return config ?? new AppConfig();
            }
            catch
            {
                // Fallback configuration
                return new AppConfig();
            }
        }
    }
}

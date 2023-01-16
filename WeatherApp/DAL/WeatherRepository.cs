using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DAL.Models.OpenWeather;

namespace DAL
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient = new HttpClient();

        public WeatherRepository(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<double> GetTemperatureByCityNameAsync(string cityName)
        {
            using var response = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={cityName}&units=metric&APPID={_apiKey}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var weather = JsonSerializer.Deserialize<WeatherResponse>(responseBody, options);

            return weather.Main.Temp;
        }
    }
}

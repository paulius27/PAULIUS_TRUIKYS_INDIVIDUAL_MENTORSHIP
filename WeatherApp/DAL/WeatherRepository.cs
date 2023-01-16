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
            using HttpResponseMessage response = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={cityName}&units=metric&APPID={_apiKey}");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            WeatherResponse weather = JsonSerializer.Deserialize<WeatherResponse>(responseBody);

            return weather.Main.Temp;
        }
    }
}

using DAL.Models;
using DAL.Models.OpenWeather;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL
{
    public class GeocodingRepository : IGeocodingRepository
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient = new HttpClient();

        public GeocodingRepository(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<Coordinates> GetCoordinatesByCityNameAsync(string cityName)
        {
            using var response = await _httpClient.GetAsync($"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&limit=1&APPID={_apiKey}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var geocoding = JsonSerializer.Deserialize<List<GeocodingResponse>>(responseBody, options).First();

            return new Coordinates(geocoding.Lat, geocoding.Lon);
        }
    }
}

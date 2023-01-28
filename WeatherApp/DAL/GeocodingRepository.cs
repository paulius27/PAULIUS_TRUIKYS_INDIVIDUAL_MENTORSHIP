﻿using DAL.Models;
using DAL.Models.OpenWeather;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL
{
    public class GeocodingRepository : IGeocodingRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;

        public GeocodingRepository(IHttpClientFactory httpClientFactory, string apiKey)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = apiKey;
        }

        public async Task<Coordinates> GetCoordinatesByCityNameAsync(string cityName)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            using var response = await httpClient.GetAsync($"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&limit=1&APPID={_apiKey}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var geocoding = JsonSerializer.Deserialize<List<GeocodingResponse>>(responseBody, options).FirstOrDefault();

            if (geocoding == null)
                throw new KeyNotFoundException("city coordinates not found");
            
            return new Coordinates(geocoding.Lat, geocoding.Lon);
        }
    }
}

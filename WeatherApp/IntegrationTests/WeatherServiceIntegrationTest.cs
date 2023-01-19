using Microsoft.Extensions.Configuration;

namespace IntegrationTests
{
    public class WeatherServiceIntegrationTest
    {
        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<WeatherServiceIntegrationTest>()
                .Build();

            string apiKey = config["weather_api_key"] ?? throw new KeyNotFoundException("Weather API Key not found.");
        }

        [Test]
        public void Test1()
        {   
            Assert.Pass();
        }
    }
}
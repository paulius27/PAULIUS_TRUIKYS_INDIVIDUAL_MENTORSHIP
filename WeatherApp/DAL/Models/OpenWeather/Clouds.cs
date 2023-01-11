using System.Text.Json.Serialization;

namespace DAL.Models.OpenWeather
{
    public class Clouds
    {
        [JsonPropertyName("all")]
        public int All { get; set; }
    }
}

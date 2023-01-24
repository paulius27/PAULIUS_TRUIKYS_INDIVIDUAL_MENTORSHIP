using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DAL.Models.OpenMeteo
{
    public class Daily
    {
        public List<string> Time { get; set; }

        [JsonPropertyName("temperature_2m_max")]
        public List<double> Temperature2mMax { get; set; }

        [JsonPropertyName("temperature_2m_min")]
        public List<double> Temperature2mMin { get; set; }
    }
}

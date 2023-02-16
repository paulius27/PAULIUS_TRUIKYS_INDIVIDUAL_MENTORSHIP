using System;

namespace DAL.Models
{
    public class WeatherHistoryEntry
    {
        public int Id { get; set; }

        public string CityName { get; set; }

        public double Temperature { get; set; }

        public DateTime Time { get; set; }
    }
}

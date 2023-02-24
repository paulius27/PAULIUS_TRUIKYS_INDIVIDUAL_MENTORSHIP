using System;

namespace DAL.Models
{
    public class WeatherHistoryEntry
    {
        public int Id { get; set; }

        public double Temperature { get; set; }

        public DateTime Time { get; set; }

        public int CityId { get; set; }

        public City City { get; set; }
    }
}

using System;

namespace BL.Models
{
    public class WeatherHistoryDataEntry
    {
        public WeatherHistoryDataEntry(double temperature, DateTime time) 
        {
            Temperature = temperature;
            Time = time;
        }

        public double Temperature { get; set; }

        public DateTime Time { get; set; }
    }
}

using System.Collections.Generic;

namespace BL.Models
{
    public class WeatherHistory
    {
        public WeatherHistory(string cityName, List<WeatherHistoryDataEntry> weatherHistory)
        {
            CityName = cityName;
            Data = weatherHistory;
        }

        public string CityName { get; set; }

        public List<WeatherHistoryDataEntry> Data { get; set; }
    }
}

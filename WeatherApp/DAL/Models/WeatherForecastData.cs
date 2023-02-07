using System;

namespace DAL.Models
{
    public class WeatherForecastData
    {
        public WeatherForecastData(DateTime date, double minTemperature, double maxTemperature)
        {
            Date = date;
            MinTemperature = minTemperature;
            MaxTemperature = maxTemperature;
        }

        public DateTime Date { get; set; }
        public double MinTemperature { get; set; }
        public double MaxTemperature { get; set; }
    }
}

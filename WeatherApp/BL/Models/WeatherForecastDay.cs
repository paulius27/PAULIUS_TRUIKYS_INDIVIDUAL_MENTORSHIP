using System;

namespace BL.Models
{
    public class WeatherForecastDay
    {
        public WeatherForecastDay(DateTime date, double temperature, string comment)
        {
            Date = date;
            Temperature = temperature;
            Comment = comment;
        }

        public DateTime Date { get; set; }

        public double Temperature { get; set; }

        public string Comment { get; set; }
    }
}

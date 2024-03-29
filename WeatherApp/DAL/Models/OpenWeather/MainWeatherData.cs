﻿namespace DAL.Models.OpenWeather
{
    public class MainWeatherData
    {
        public double Temp { get; set; }

        public double FeelsLike { get; set; }

        public double TempMin { get; set; }

        public double TempMax { get; set; }

        public int Pressure { get; set; }

        public int Humidity { get; set; }
    }
}

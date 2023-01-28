namespace DAL.Models.OpenMeteo
{
    public class WeatherForecastResponse
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double GenerationtimeMs { get; set; }

        public int UtcOffsetSeconds { get; set; }

        public string Timezone { get; set; }

        public string TimezoneAbbreviation { get; set; }

        public double Elevation { get; set; }

        public DailyUnits DailyUnits { get; set; }

        public Daily Daily { get; set; }
    }
}

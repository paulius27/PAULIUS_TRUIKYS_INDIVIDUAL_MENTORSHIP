namespace BL.Models
{
    public class Weather
    {
        public Weather(string cityName, double temperature, string comment) 
        {
            CityName = cityName;
            Temperature = temperature;
            Comment = comment;
        }

        public string CityName { get; set; }

        public double Temperature { get; set; }

        public string Comment { get; set; }
    }
}

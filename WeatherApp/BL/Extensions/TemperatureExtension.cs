namespace BL.Extensions
{
    public static class TemperatureExtension
    {
        public static string GetTemperatureComment(this double temperature)
        {
            if (temperature < 0)
                return "Dress warmly";
            else if (temperature < 20)
                return "It's fresh";
            else if (temperature < 30)
                return "Good weather";
            else
                return "It's time to go to the beach";
        }
    }
}

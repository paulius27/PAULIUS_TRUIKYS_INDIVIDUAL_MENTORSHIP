namespace BL.Validation
{
    public interface IValidation
    {
        public bool IsCityNameValid(string cityName);

        public bool AreForecastDaysValid(int days);
    }
}

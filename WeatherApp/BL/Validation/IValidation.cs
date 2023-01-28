namespace BL.Validation
{
    public interface IValidation
    {
        bool IsCityNameValid(string cityName);

        bool AreForecastDaysValid(int days);
    }
}

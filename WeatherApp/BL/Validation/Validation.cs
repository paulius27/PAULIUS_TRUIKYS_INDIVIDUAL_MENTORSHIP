namespace BL.Validation
{
    public class Validation : IValidation
    {
        public bool IsCityNameValid(string cityName) => !string.IsNullOrEmpty(cityName.Trim());
    }
}

namespace BL.Validation
{
    public class CityNameValidator : IValidator<string>
    {
        public bool Validate(string cityName) => !string.IsNullOrEmpty(cityName?.Trim());
    }
}

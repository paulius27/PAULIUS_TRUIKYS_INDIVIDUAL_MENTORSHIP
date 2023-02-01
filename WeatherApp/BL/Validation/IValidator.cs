namespace BL.Validation
{
    public interface IValidator<T>
    {
        bool Validate(T value);
    }
}

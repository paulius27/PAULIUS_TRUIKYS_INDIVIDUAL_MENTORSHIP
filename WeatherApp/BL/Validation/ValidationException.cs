using System;

namespace BL.Validation
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
}

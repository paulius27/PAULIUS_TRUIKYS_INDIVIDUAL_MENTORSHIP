using System;

namespace BL.Validation
{
    public class ValidationException : Exception
    {
        public ValidationException(string message, string paramName) : base(message) 
        {
            ParamName = paramName;
        }

        public string ParamName { get; }
    }
}

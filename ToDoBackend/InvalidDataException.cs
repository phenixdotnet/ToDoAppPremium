using System;
namespace ToDoBackend
{
    public class InvalidDataException : Exception
    {
        private readonly string? paramName;
        private readonly string? correlationId;

        public InvalidDataException(string? paramName)
        {
            this.paramName = paramName;
        }

        public InvalidDataException(string? paramName, string correlationId)
            : this(paramName)
        {
        }

        public InvalidDataException(string? paramName, string message, string correlationId)
            : base(message)
        {
            this.paramName = paramName;
            this.correlationId = correlationId;
        }

        public override string ToString()
        {
            return $"{this.correlationId}|{this.paramName} is invalid: {this.Message}";
        }
    }
}


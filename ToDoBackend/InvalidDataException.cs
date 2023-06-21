<<<<<<< HEAD
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
=======
﻿using System;
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
>>>>>>> 1-2
        {
        }

        public override string ToString()
        {
<<<<<<< HEAD
            return $"{this.correlationId}|{this.paramName} is invalid";
=======
			return $"{this.correlationId}|{this.paramName} is invalid";
>>>>>>> 1-2
        }
    }
}


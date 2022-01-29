using System;
using System.Collections.Generic;

namespace NHGeneral
{
    public class InvalidUserInputException : Exception
    {
        public List<string> ErrorMessages { get; set; }

        public InvalidUserInputException()
            : base()
        {
        }

        public InvalidUserInputException(string message, Exception innerException, List<string> errorMessages)
            : base(message, innerException)
        {
            ErrorMessages = errorMessages;
        }

        public InvalidUserInputException(string message, List<string> errorMessages)
            : base(message)
        {
            ErrorMessages = errorMessages;
        }

        public InvalidUserInputException(List<string> errorMessages)
            : base()
        {
            ErrorMessages = errorMessages;
        }

    }
}
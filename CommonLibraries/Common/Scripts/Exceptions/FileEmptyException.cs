using System;
using System.IO;

namespace Uheaa.Common.Scripts.Exceptions
{
    class FileEmptyException : FileNotFoundException
    {
        public FileEmptyException()
            : base()
        {
        }

        public FileEmptyException(string message)
            : base(message)
        {
        }

        public FileEmptyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public FileEmptyException(string message, string fileName)
            : base(message, fileName)
        {
        }

        public FileEmptyException(string message, string fileName, Exception innerException)
            : base(message, fileName, innerException)
        {
        }
    }
}

using System;

namespace NHGeneral
{
    public class DCRSubjectOptionAlreadyExistsInListTableException : Exception
    {
        public DCRSubjectOptionAlreadyExistsInListTableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DCRSubjectOptionAlreadyExistsInListTableException(string message)
            : base(message)
        {
        }

        public DCRSubjectOptionAlreadyExistsInListTableException()
            : base()
        {
        }

    }
}
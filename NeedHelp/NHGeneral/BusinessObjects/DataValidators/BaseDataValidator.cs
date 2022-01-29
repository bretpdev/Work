using System.Collections.Generic;

namespace NHGeneral
{
    abstract class BaseDataValidator
    {
        protected List<string> _errorMessageList = new List<string>();

        public abstract List<string> ValidateData(TicketData data);
    }
}
using System;
using System.Collections.Generic;

namespace NHGeneral
{
    class DV1 : BaseDataValidator
    {
        /// <summary>
        /// Validate data
        /// </summary>
        /// <param name="data"></param>
        public override List<string> ValidateData(TicketData data)
        {
            if (string.IsNullOrEmpty(data.TicketCode))
            {
                _errorMessageList.Add("A ticket type must be provided.");
            }
            return _errorMessageList;
        }
    }
}
using System;
using System.Collections.Generic;

namespace NHGeneral
{
    class DV3 : BaseDataValidator
    {
        /// <summary>
        /// Validate data
        /// </summary>
        /// <param name="data"></param>
        public override List<string> ValidateData(TicketData data)
        {
            if (string.IsNullOrEmpty(data.IssueUpdate))
            {
                _errorMessageList.Add("A update must be provided.");
            }
            return _errorMessageList;
        }
    }
}
using System;
using System.Collections.Generic;

namespace NHGeneral
{
    class DV6 : DV5
    {
        /// <summary>
        /// Validate data
        /// </summary>
        /// <param name="data"></param>
        public override List<string> ValidateData(TicketData data)
        {
            _errorMessageList = base.ValidateData(data); //do DV5's validation steps
            if (string.IsNullOrEmpty(data.AssignedTo.ID.ToString()))
            {
                _errorMessageList.Add("A staff member to assign the ticket to must be provided.");
            }
            return _errorMessageList;
        }
    }
}
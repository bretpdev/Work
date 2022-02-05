using System;
using System.Collections.Generic;

namespace NHGeneral
{
    class DV5 : DV2
    {
        /// <summary>
        /// Validate data
        /// </summary>
        /// <param name="data"></param>
        public override List<string> ValidateData(TicketData data)
        {
            _errorMessageList = base.ValidateData(data); //do DV2's validation steps
            if (data.Systems.Count == 0)
            {
                _errorMessageList.Add("A system must be provided.");
            }
            return _errorMessageList;
        }
    }
}
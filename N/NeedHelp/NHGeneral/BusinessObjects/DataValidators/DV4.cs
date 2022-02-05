using System;
using System.Collections.Generic;

namespace NHGeneral
{
    class DV4 : DV2
    {
        /// <summary>
        /// Validate data
        /// </summary>
        /// <param name="data"></param>
        public override List<string> ValidateData(TicketData data)
        {
            _errorMessageList = base.ValidateData(data); //do DV2's validation steps
            if (string.IsNullOrEmpty(data.ResolutionCause))
            {
                _errorMessageList.Add("A resolution cause must be provided.");
            }
            if (string.IsNullOrEmpty(data.ResolutionFix))
            {
                _errorMessageList.Add("A resolution fix must be provided.");
            }
            if (string.IsNullOrEmpty(data.ResolutionPrevention))
            {
                _errorMessageList.Add("A resolution prevention must be provided.");
            }
            return _errorMessageList;
        }
    }
}
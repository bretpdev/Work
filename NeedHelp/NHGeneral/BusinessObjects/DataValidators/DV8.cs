using System;
using System.Collections.Generic;
using Uheaa.Common;

namespace NHGeneral
{
    class DV8 : BaseDataValidator
    {
        /// <summary>
        /// Validate data
        /// </summary>
        /// <param name="data"></param>
        public override List<string> ValidateData(TicketData data)
        {
            if (string.IsNullOrEmpty(data.Subject))
            {
                _errorMessageList.Add("A subject must be provided.");
            }
            if (string.IsNullOrEmpty(data.Requester.ID.ToString()))
            {
                _errorMessageList.Add("A requester must be provided.");
            }
            if (string.IsNullOrEmpty(data.Requested.ToShortDateString()) || !IsDate(data.Requested.ToShortDateString()))
            {
                _errorMessageList.Add("A valid requested date must be provided.");
            }
            if (data.Unit != null && data.Unit.Name.IsNullOrEmpty())
            {
                _errorMessageList.Add("A business unit must be provided.");
            }
            if (data.Priority <= 0)
            {
                _errorMessageList.Add("Enough information for a priority to be calculated must be provided.");
            }
            if (string.IsNullOrEmpty(data.Required.ToShortDateString()) || !IsDate(data.Required.ToShortDateString()))
            {
                _errorMessageList.Add("A valid required date must be provided.");
            }
            if (string.IsNullOrEmpty(data.Issue))
            {
                _errorMessageList.Add("An issue description must be provided.");
            }
            return _errorMessageList;
        }

        //checks if a string is a valid date
        private Boolean IsDate(string dateString)
        {
            try
            {
                DateTime dummy = DateTime.Parse(dateString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

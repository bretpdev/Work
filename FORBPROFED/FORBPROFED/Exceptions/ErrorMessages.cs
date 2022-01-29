using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;

namespace FORBPROFED
{
    static class ErrorMessages
    {
        public static string GetTSX30(ForbProcessingRecord record, ReflectionInterface ri)
        {
            return "Error encountered screen TSX30 for ForbearanceProcessingId: " + record.ForbearanceProcessingId + " Start Date: " + record.StartDate.Date.ToString() + " End Date: " + record.EndDate.Date.ToString() + " Forbearance Type: " + record.ForbearanceType.ToString() + " Error: " + ri.Message;
        }
        public static string GetTSX31(ForbProcessingRecord record, ReflectionInterface ri)
        {
            return "Error encountered screen TSX31 for ForbearanceProcessingId: " + record.ForbearanceProcessingId + " Start Date: " + record.StartDate.Date.ToString() + " End Date: " + record.EndDate.Date.ToString() + " Forbearance Type: " + record.ForbearanceType.ToString() + " Error: " + ri.Message;
        }

        public static string GetTSX32(ForbProcessingRecord record, ReflectionInterface ri)
        {
            return "Error, not on TSX32 screen for ForbearanceProcessingId: " + record.ForbearanceProcessingId + " Start Date: " + record.StartDate.Date.ToString() + " End Date: " + record.EndDate.Date.ToString() + " Forbearance Type: " + record.ForbearanceType.ToString() + " Error: " + ri.Message;
        }

        public static string GetTSXA5(ForbProcessingRecord record, ReflectionInterface ri)
        {
            return "Error, not on TSXA5 screen for ForbearanceProcessingId: " + record.ForbearanceProcessingId + " Start Date: " + record.StartDate.Date.ToString() + " End Date: " + record.EndDate.Date.ToString() + " Forbearance Type: " + record.ForbearanceType.ToString() + " Error: " + ri.Message;
        }

        public static string GetTSX7E(ForbProcessingRecord record, ReflectionInterface ri)
        {
            return "Error, not on TSX7E screen for ForbearanceProcessingId: " + record.ForbearanceProcessingId + " Start Date: " + record.StartDate.Date.ToString() + " End Date: " + record.EndDate.Date.ToString() + " Forbearance Type: " + record.ForbearanceType.ToString() + " Error: " + ri.Message;
        }

        public static string GetTSX7F(ForbProcessingRecord record, ReflectionInterface ri)
        {
            return "Error, not on TSX7F screen for ForbearanceProcessingId: " + record.ForbearanceProcessingId + " Start Date: " + record.StartDate.Date.ToString() + " End Date: " + record.EndDate.Date.ToString() + " Forbearance Type: " + record.ForbearanceType.ToString() + " Error: " + ri.Message;
        }

        public static string Get01004(ForbProcessingRecord record, ReflectionInterface ri)
        {
            return "Error, Message was not 01004 for ForbearanceProcessingId: " + record.ForbearanceProcessingId + " Start Date: " + record.StartDate.Date.ToString() + " End Date: " + record.EndDate.Date.ToString() + " Forbearance Type: " + record.ForbearanceType.ToString() + " Error: " + ri.Message;
        }

        public static string BadForbType(ForbProcessingRecord record, ReflectionInterface ri)
        {
            return "Unable to locate forbearance type: " + record.ForbearanceType + " on ATS0H ForbearanceProcessingId: " + record.ForbearanceProcessingId + " Start Date: " + record.StartDate.Date.ToString() + " End Date: " + record.EndDate.Date.ToString() + " Error: " + ri.Message;
        }

        public static string BorrowerHasNoLoans(ForbProcessingRecord record, ReflectionInterface ri)
        {
            return "Borrower Has No Loans " + record.AccountNumber + " ForbearanceProcessingId: " + record.ForbearanceProcessingId + " Start Date: " + record.StartDate.Date.ToString() + " End Date: " + record.EndDate.Date.ToString() + " Forbearance Type: " + record.ForbearanceType.ToString() + " Error: " + ri.Message;
        }

        public static string NoLoansSelected(ForbProcessingRecord record, ReflectionInterface ri)
        {
            return "No loans were selected to be processed for ForbearanceProcessingId: " + record.ForbearanceProcessingId + " Start Date: " + record.StartDate.Date.ToString() + " End Date: " + record.EndDate.Date.ToString() + " Forbearance Type: " + record.ForbearanceType.ToString() + " Error: " + ri.Message; 
        }
    }
}

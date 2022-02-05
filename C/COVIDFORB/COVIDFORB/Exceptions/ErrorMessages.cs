using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;

namespace COVIDFORB
{
    static class ErrorMessages
    {
        public static string GetTSX30(ForbProcessingRecord record, ReflectionInterface ri)
        {
            return ri.Message + ", Forbearance Type: " + record.ForbearanceType.ToString() +  ", Please see SSRS report." + " Start Date: " + record.StartDate.Date.ToShortDateString() + ", End Date: " + record.EndDate.Date.ToShortDateString();
        }
        public static string GetTSX31(ForbProcessingRecord record, ReflectionInterface ri)
        {
            return ri.GetText(21,2,80) +", Forbearance Type: " + record.ForbearanceType.ToString() + ", Please see SSRS report." + " Start Date: " + record.StartDate.Date.ToShortDateString() + ", End Date: " + record.EndDate.Date.ToShortDateString();
        }

        public static string GetTSX32(ForbProcessingRecord record, ReflectionInterface ri)
        {
            return ri.Message + ", Forbearance Type: " + record.ForbearanceType.ToString() +  ", Please see SSRS report." + " Start Date: " + record.StartDate.Date.ToShortDateString() + ", End Date: " + record.EndDate.Date.ToShortDateString();
        }

        public static string GetTSXA5(ForbProcessingRecord record, ReflectionInterface ri)
        {
            return ri.Message + ", Forbearance Type: " + record.ForbearanceType.ToString() +  ", Please see SSRS report." + " Start Date: " + record.StartDate.Date.ToShortDateString() + ", End Date: " + record.EndDate.Date.ToShortDateString();
        }

        public static string GetTSX7E(ForbProcessingRecord record, ReflectionInterface ri)
        {
            return ri.Message + ", Forbearance Type: " + record.ForbearanceType.ToString() +  ", Please see SSRS report." + " Start Date: " + record.StartDate.Date.ToShortDateString() + ", End Date: " + record.EndDate.Date.ToShortDateString();
        }

        public static string GetTSX7F(ForbProcessingRecord record, ReflectionInterface ri)
        {
            return ri.Message +  ", Forbearance Type: " + record.ForbearanceType.ToString() +  ", Please see SSRS report." + " Start Date: " + record.StartDate.Date.ToShortDateString() + ", End Date: " + record.EndDate.Date.ToShortDateString();
        }

        public static string Get01004(ForbProcessingRecord record, ReflectionInterface ri)
        {
            return ri.Message +  ", Start Date: " + ", Forbearance Type: " + record.ForbearanceType.ToString() +  ", Please see SSRS report." + " Start Date: " + record.StartDate.Date.ToShortDateString() + ", End Date: " + record.EndDate.Date.ToShortDateString();
        }

        public static string BadForbType(ForbProcessingRecord record, ReflectionInterface ri)
        {
            return "Unable to locate forbearance type: " + record.ForbearanceType + " on ATS0H ForbearanceProcessingId: " + record.ForbearanceProcessingId +  ", Please see SSRS report." + ", Start Date: " + record.StartDate.Date.ToShortDateString() + ", End Date: " + record.EndDate.Date.ToShortDateString()  ;
        }

        public static string BorrowerHasNoLoans(ForbProcessingRecord record, ReflectionInterface ri)
        {
            return "Borrower Has No Loans " + record.AccountNumber + " ForbearanceProcessingId: " + record.ForbearanceProcessingId + ", Forbearance Type: " + record.ForbearanceType.ToString() +  ", Please see SSRS report." + " Start Date: " + record.StartDate.Date.ToShortDateString() + ", End Date: " + record.EndDate.Date.ToShortDateString()  ;
        }

        public static string NoLoansSelected(ForbProcessingRecord record, ReflectionInterface ri)
        {
            return ri.Message + " No loans selected for ForbearanceProcessingId: " + record.ForbearanceProcessingId + " Forbearance Type: " + record.ForbearanceType.ToString() +  ", Please see SSRS report." + " Start Date: " + record.StartDate.Date.ToShortDateString() + ", End Date: " + record.EndDate.Date.ToShortDateString()  ; 
        }
    }
}

using System.Linq;
using System.Collections.Generic;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace ACHSETUPFD
{
    public static class LetterDataFormatter
    {
        public static string GenerateApprovedLetterData(SystemBorrowerDemographics demos, string borrowerName, string autoPayBeginDate, string additionalAmount, string ssn, string accountNumber)
        {
            //File Header Definition
            //"KeyLine, AccountNumber, Name, Address1, Address2, City, State, Zip, Country, BorrowerName, AutoPayBeginDate, AdditionalAmount"
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}",
                            DocumentProcessing.ACSKeyLine(ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal),
                                     accountNumber, ((demos is SystemBorrowerDemographics) ? string.Format("{0} {1}", demos.FirstName, demos.LastName) : demos.FirstName),
                                     demos.Address1, demos.Address2, demos.City, demos.State, demos.ZipCode, demos.Country, borrowerName, autoPayBeginDate, additionalAmount);
        }

        public static string GenerateApprovedLetterDataCoborrower(CoborrowerDemographics demos, string borrowerName, string autoPayBeginDate, string additionalAmount)
        {
            //File Header Definition
            //"KeyLine, AccountNumber, Name, Address1, Address2, City, State, Zip, Country, BorrowerName, AutoPayBeginDate, AdditionalAmount"
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}",
                            DocumentProcessing.ACSKeyLine(demos.CoBorrowerSSN.Trim(new char[] { ' ' }), DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal),
                                     demos.AccountNumber.Trim(new char[] { ' ' }), (string.Format("{0} {1}", demos.FirstName.Trim(new char[] { ' ' }), demos.LastName.Trim(new char[] { ' ' }))),
                                     demos.Address1.Trim(new char[] { ' ' }), demos.Address2.Trim(new char[] { ' ' }), demos.City.Trim(new char[] { ' ' }), demos.State.Trim(new char[] { ' ' }), demos.Zip.Trim(new char[] { ' ' }), demos.ForeignCountry.Trim(new char[] { ' ' }), borrowerName.Trim(new char[] { ' ' }), autoPayBeginDate, additionalAmount);
        }

        public static string GenerateDeniedLetterData(SystemBorrowerDemographics demos, List<string> denials)
        {
            string formatted = GenerateDenialString(denials);
            //File Header
            //"KeyLine, AccountNumber, Name, Address1, Address2, City, State, Zip, Country, DenialReason1, DenialReason2, DenialReason3, DenialReason4, DenialReason5"
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
                                     DocumentProcessing.ACSKeyLine(demos.Ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal),
                                     demos.AccountNumber, string.Format("{0} {1}", demos.FirstName, demos.LastName), demos.Address1, demos.Address2,
                                     demos.City, demos.State, demos.ZipCode, demos.Country, formatted);
        }

        public static string GenerateDeniedLetterDataCoborrower(CoborrowerDemographics demos, List<string> denials)
        {
            string formatted = GenerateDenialString(denials);
            //File Header
            //"KeyLine, AccountNumber, Name, Address1, Address2, City, State, Zip, Country, DenialReason1, DenialReason2, DenialReason3, DenialReason4, DenialReason5"
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
                                     DocumentProcessing.ACSKeyLine(demos.CoBorrowerSSN.Trim(new char[] { ' ' }), DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal),
                                     demos.BorrowerAccountNumber.Trim(new char[] { ' ' }), string.Format("{0} {1}", demos.FirstName.Trim(new char[] { ' ' }), demos.LastName.Trim(new char[] { ' ' })), demos.Address1.Trim(new char[] { ' ' }), demos.Address2.Trim(new char[] { ' ' }),
                                     demos.City.Trim(new char[] { ' ' }), demos.State.Trim(new char[] { ' ' }), demos.Zip.Trim(new char[] { ' ' }), demos.ForeignCountry.Trim(new char[] { ' ' }), formatted);
        }

        public static string GenerateDenialString(List<string> denials)
        {
            int max = 5;
            int min = max - denials.Count();
            string formatted = "";
            for (int n = 0; n < denials.Count(); n++)
            {
                formatted += "\"- " + denials[n] + "\",";
            }
            if (min == 0)
                formatted = formatted.SafeSubString(0, formatted.Length - 1);
            else
                for (int n = 0; n < min - 1; n++)
                    formatted += ",";

            return formatted;
        }
    }
}

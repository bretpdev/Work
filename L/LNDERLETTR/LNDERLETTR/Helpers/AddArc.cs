using System;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common;


namespace LNDERLETTR.Helpers
{
    public class AddArc
    {
        public Populations pops { get; set; }
        public DataAccess da { get; set; }
        public ProcessLogRun plr { get; set; }

        public AddArc(DataAccess DA, ProcessLogRun PLR, Populations POPS)
        {
            pops = POPS;
            plr = PLR;
            da = DA;
        }


        /// <summary>
        /// Adds LCNCM ARC to borrower account
        /// </summary>
        /// <param name="lender">Lender object with borrower data</param>
        public bool AddArcForLettersId(string account, int id, LetterData letter)
        {
            if (letter.ArcAddProcessingId != null)
                return false;

            string comment = "";
            bool isValid = (letter.ValidLenderAddress == "Y");

            Console.WriteLine("Adding ARC for borrower: {0}, LettersId: {1}", account, letter.LettersId);
            if (pops.InUheaa.ContainsKey(letter.LenderId))
                comment = $"{letter.LenderId}: We are the original lender, closing task.";
            else if (pops.InBana.ContainsKey(letter.LenderId) || (!pops.InOpen.ContainsKey(letter.LenderId) && isValid))
                comment = $"{letter.LenderId}: Sent Original Lender Letter.";
            else if (!pops.InClosed.ContainsKey(letter.LenderId) && letter.ValidLenderAddress == "N" && letter.Address.Contains("X-CLOSED-"))
                comment = $"{letter.LenderId}: Original lender is now closed, unable to send skip letter for borrower demographics.";
            else
            {
#if !DEBUG              // JR is the only queue this script deals with, thus hardcoded.
                        plr.AddNotification($"JR: The population for borrower {letter.Ssn} could not be determined.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
#endif
                return false;
            }


            ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = account,
                Arc = "LCNCM",
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans, 
                Comment = comment,
                IsEndorser = false,
                IsReference = false,
                ScriptId = "LNDERLETTR"
            };
            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                string message = string.Format("Error adding LCNCM ARC for borrwer: {0}, LettersId: {1}", letter.Ssn, letter.LettersId, result.Ex);
                plr.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return false;
            }

            if (!da.SetArcAddId(id, result.ArcAddProcessingId))
            {
                Console.WriteLine(string.Format("Error setting ArcId for letter {0} for Id {1}.", id, result.ArcAddProcessingId));
                return true;
            }
            return result.ArcAdded;
        }

    }
}

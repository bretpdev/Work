using System;
using System.Collections.Generic;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common.ProcessLogger;

namespace PEPSFED
{
    class ProgramUpdater
    {
        private List<LoanProgram> LoanPrograms { get; set; }
        private DataAccess DA { get; set; }
        public ProgramUpdater(DataAccess da)
        {
            DA = da;
        }

        private void LoadLoanPgms(ProgramData data)
        {
            List<LoanProgram> loanPrograms = new List<LoanProgram>();
            loanPrograms.Add(new LoanProgram("STFFRD", data.FfelStafApprovInd, data.FfelStaffordStartDate.Value));
            loanPrograms.Add(new LoanProgram("UNSTFD", data.FfelStafUnsubApprovInd, data.FfelUnsubsidizedStaffordStartDate.Value));
            loanPrograms.Add(new LoanProgram("PLUS", data.FfelPlusApprovInd, data.FfelPlusStartDate.Value));
            loanPrograms.Add(new LoanProgram("PLUSGB", data.FfelPlusApprovInd, data.FfelPlusStartDate.Value));
            loanPrograms.Add(new LoanProgram("SLS", data.FfelSlsApprovInd, data.FfelSlsStartDate.Value));
            loanPrograms.Add(new LoanProgram("DLSTFD", data.FdslpStafApprovInd, data.DlStaffordStartDate.Value));
            loanPrograms.Add(new LoanProgram("DLUNST", data.FdslpStafUnsubApprovInd, data.DlUnsibsidizedStaffordStartDate.Value));
            loanPrograms.Add(new LoanProgram("DLPLGB", data.FdslpPlusApprovInd, data.DlPlusStartDate.Value));
            loanPrograms.Add(new LoanProgram("DLPLUS", data.FdslpPlusApprovInd, data.DlPlusStartDate.Value));
            loanPrograms.Add(new LoanProgram("PERKNS", data.FperkinsApprovInd, data.FederalPerkinsStartDate.Value));
            LoanPrograms = loanPrograms;
        }

        public void UpdateSystem(ReflectionInterface ri, ProgramData data)
        {
            Console.WriteLine("About to process PROGRAM_ID: {0}", data.RecordId);
            data.FormatProperties();
            LoadLoanPgms(data);
            ri.FastPath(string.Format("TX3Z/CTX10{0}", data.OpeId));
            if (ri.CheckForText(23, 2, "50510"))
            {
                //SCHOOL NOT FOUND
                ri.FastPath(string.Format("TX3Z/ITX0Y{0}", data.OpeId));
                if (ri.CheckForText(23, 2, "30021 INSTITUTION NOT DEFINED ON INSTITUTION DEMOGRAPHICS"))
                {
                    Program.PLR.AddNotification(string.Format("Unable to update School Information for the following peps line: {0}; Session Message: {1}", data.ToString(), ri.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    DA.UpdateProgramProcessed(data.RecordId);
                    return;
                }
            }
            foreach (LoanProgram lp in LoanPrograms)
            {
                ri.FastPath(string.Format("TX3Z/CTX10{0};{1}", data.OpeId, lp.Abbreviation));
                if (!ri.CheckForText(1, 73, "TXX12"))
                {
                    if (lp.StartDate != default(DateTime))
                        ri.PutText(1, 4, "A", Key.Enter);
                    else
                        continue;
                }
                if (lp.StartDate == default(DateTime) && ri.CheckForText(10, 17, lp.ApprovalStatus)) //AGY APV STA
                    continue;
                UpdateLoan(ri, lp);
                //note that an update failed but continue processing additional loan program records
                if (!ri.CheckForText(23, 2, "01005", "01003", "01004"))
                    Program.PLR.AddNotification(string.Format("Unable to update School Information for the following peps line: {0}; Session Message: {1}", data.ToString(), ri.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }

            DA.UpdateProgramProcessed(data.RecordId);
        }

        private void UpdateLoan(ReflectionInterface ri, LoanProgram lp)
        {
            ri.PutText(10, 17, lp.ApprovalStatus); //AGY APV STA
            if (lp.StartDate != default(DateTime))
                ri.PutText(11, 17, lp.StartDate.ToString("MMddyy")); //APV STA DATE
            else
                ri.PutText(11, 17, DateTime.Today.ToString("MMddyy"));

            if (ri.CheckForText(14, 24, "N")) //SERIAL MPN ELIGIBLE
            {
                ri.PutText(15, 17, DateTime.Today.ToString("MMddyy")); //MPN EFF DATE
                ri.PutText(15, 41, "1"); //REASON
            }

            if (ri.CheckForText(14, 24, "Y")) //SERIAL MPN ELIGIBLE
            {
                //ri.PutText(15, 17, DateTime.Today.ToString("MMddyy")); //MPN EFF DATE
                ri.PutText(15, 41, "", true); //REASON
            }

            if (!ri.CheckForText(15, 17, "_") && Convert.ToDateTime(ri.GetText(15, 17, 8).Replace(" ", "/")) < DateTime.Today.AddYears(-5)) //MPN EFF DATE
                ri.PutText(15, 17, DateTime.Today.ToString("MMddyy"));//MPN EFF DATE

            ri.Hit(Key.Enter);
            if (ri.CheckForText(23, 2, "01329"))
            {
                DateTime falseDate = lp.StartDate.AddDays(1);
                ri.PutText(11, 17, falseDate.ToString("MMddyy")); //APV STA DATE
                ri.Hit(Key.Enter);
                ri.PutText(11, 17, lp.StartDate.ToString("MMddyy")); //APV STA DATE
                ri.Hit(Key.Enter);
            }
        }

        private class LoanProgram
        {
            public string Abbreviation { get; private set; }
            public string ApprovalStatus { get; private set; }
            public DateTime StartDate { get; private set; }

            public LoanProgram(string abbreviation, string approvalIndicator, DateTime startDate)
            {
                Abbreviation = abbreviation;
                ApprovalStatus = (approvalIndicator == "Y" ? "B" : "K");
                StartDate = startDate;
            }
        }

    }
}

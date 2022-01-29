using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using static System.Console;

namespace RHBRWINPC
{
    public class RehabBorrowerInPreClaim
    {
        public ProcessLogRun LogRun { get; set; }
        public DataAccess DA { get; set; }

        public RehabBorrowerInPreClaim(ProcessLogRun logRun)
        {
            LogRun = logRun;
            DA = new DataAccess(logRun);
        }

        public int Main()
        {
            int returnCount = 0;

            //Load the data from the warehouse to the local tables
            WriteLine("");
            WriteLine("Gathering borrower data from warehouse to load into local database.");
            DA.LoadNewLetters();

            List<LetterData> letters = DA.GetLetterData();

            if (letters.Count > 0)
            {
                WriteLine("");
                WriteLine("Ready to process {0} letters", letters.Count);
                returnCount = PrintLetters(letters);
                returnCount += AddArcs(letters);
            }

            return returnCount >= 1 ? 1 : 0;
        }

        /// <summary>
        /// Generates a data file and uses CostCenterPrinting to print the letter
        /// </summary>
        private int PrintLetters(List<LetterData> letters)
        {
            int returnCount = 0;
            WriteLine("");
            //"DF_SPE_ACC_ID,DM_PRS_1,DM_PRS_LST,DX_STR_ADR_1 ,DX_STR_ADR_2 ,DM_CT,DC_DOM_ST,DF_ZIP,DM_FGN_CNY,COST_CENTER_CODE,ACSKEY" the header row
            foreach (LetterData data in letters)
            {
                WriteLine($"Adding letter to the Print Processing table for borrower {data.AccountNumber}");
                if (data.PrintedAt.IsNullOrEmpty())
                {
                    string keyLine = DocumentProcessing.ACSKeyLine(data.AccountNumber.Trim(), DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
                    string letterData = $"{data.AccountNumber.Trim()},{data.FirstName.Trim()},{data.LastName.Trim()},{data.Address1.Trim()},{data.Address2.Trim()},{data.City.Trim()},{data.State.Trim()},{data.Zip.Trim()},{data.Country.Trim()},MA2329,{keyLine}";
                    if (EcorrProcessing.AddOneLinkRecordToPrintProcessing(Program.ScriptId, "DATREDEF", letterData, data.AccountNumber, "MA2329").HasValue)
                        DA.SetPrintedAt(data.LettersId);
                    else
                    {
                        string message = $"There was an error adding the DATREDEF letter to Print Processing for borrower: {data.AccountNumber}";
                        LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        WriteLine(message);
                        returnCount = 1;
                    }
                }
            }
            return returnCount;
        }

        /// <summary>
        /// Adds an LP50 ARC to each account to the ArcAddProcessing table
        /// </summary>
        private int AddArcs(List<LetterData> letters)
        {
            WriteLine("");
            int returnCount = 0;
            foreach (LetterData data in letters)
            {
                WriteLine($"Adding ALSBR ARC for borrower {data.AccountNumber} to Arc Add Processing");
                if (data.ArcAddProcessingId == 0)
                {
                    ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
                    {
                        AccountNumber = data.AccountNumber,
                        ActivityContact = "03",
                        ActivityType = "LT",
                        Arc = "ALSBR",
                        ArcTypeSelected = ArcData.ArcType.OneLINK,
                        Comment = "Default Aversion letter sent to borrower",
                        ScriptId = Program.ScriptId
                    };
                    ArcAddResults result = arc.AddArc();
                    if (!result.ArcAdded)
                    {
                        string message = string.Format("Error adding ALSBR comment in LP50 for borrower: {0}", data.AccountNumber);
                        LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        WriteLine(message);
                        returnCount = 1;
                    }
                    else
                        DA.SetArcAddId(data.LettersId, result.ArcAddProcessingId);
                }
            }

            return returnCount;
        }

    }
}
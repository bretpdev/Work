using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace BILLING
{
    public class Comment
    {
        private ProcessLogRun PLR { get; set; }
        private DataAccess DA { get; set; }

        /// <summary>
        /// Creates and ArcData for each borrower and adds the ARC to the ArcProcessing table
        /// </summary>
        /// <param name="borrowers">All the borrowers that need an ARC</param>
        /// <param name="logData">ProcessLogData object</param>
        public void Start(List<Borrower> borrowers, ProcessLogRun plr, DataAccess da)
        {
            DA = da;
            PLR = plr;
            Console.WriteLine("Adding ARCs");
            Parallel.ForEach(borrowers, new ParallelOptions { MaxDegreeOfParallelism = int.MaxValue }, bor =>
                {
                    if (bor.ArcAddedAt == null)
                    {
                        //Process the Borrower ARCs
                        BorrowerArcs(bor);
                        //Add Endorser ARC if borrower has endorser
                        EndorserArcs(bor);
                        DA.SetCommentComplete(bor.PrintProcessingId);
                    }
                });
            if (borrowers.Count > 0)
                Console.WriteLine("Finished Adding ARC's");
        }

        /// <summary>
        /// Adds BILLS and any delinquent ARC to borrower account
        /// </summary>
        /// <param name="logData">ProcessLogData object</param>
        /// <param name="bor">BorrowerData object</param>
        private void BorrowerArcs(Borrower bor)
        {
            CommentsAndArc ca = new CommentsAndArc(bor, bor.LineData.SplitAndRemoveQuotes(","), PLR, DA);
            if (ca.IsEndorser)
                return;
            ArcData arc = GetBILLSArcData(bor, ca); //Add BILLS ARC to Every Borrower
                AddArc(bor, arc); //Add the ARC
            if (bor.ReportNumber.IsIn(25, 26))
            {
                arc.Arc = "COPBL";
                AddArc(bor, arc);
            }
            else if ((ca.DaysDelinquent != null && ca.DaysDelinquent.Value > 0) || bor.ReportNumber.IsIn(12, 13, 15, 16, 17, 18, 19, 20, 23, 24))
            {
                arc = GetDelinquentArc(bor, ca);
                if (!bor.ReportNumber.IsIn(10, 11) && !ca.IsEndorser) //Don't leave a delinquent ARC for R10 & R11
                    AddArc(bor, arc);
            }
        }

        /// <summary>
        /// Adds Endorser ARCs
        /// </summary>
        /// <param name="logData">ProcessLogData object</param>
        /// <param name="bor">BorrowerData object</param>
        private void EndorserArcs(Borrower bor)
        {
            CommentsAndArc ca = new CommentsAndArc(bor, bor.LineData.SplitAndRemoveQuotes(","), PLR, DA);
            if (((!ca.IsEndorser && !ca.EndorserSameAddress) && ca.EndorserSsn.IsPopulated()) || !ca.IsEndorser && ca.EndorserSameAddress)
            {
                ArcData arc = GetEndorserArcData(bor, ca);
                if (!arc.Comment.ToUpper().Contains("ENDORSER"))
                    arc.Comment = arc.Comment.Replace("Borrower", "Endorser");
                AddArc(bor, arc);
                if (bor.ReportNumber.IsIn(25, 26))
                {
                    arc.Arc = "COPBE";
                    AddArc(bor, arc);
                    return;
                }
            }
            //Add Endorser Delinquent ARC
            if ((((!ca.IsEndorser && !ca.EndorserSameAddress) && ca.EndorserSsn.IsPopulated()) || !ca.IsEndorser && ca.EndorserSameAddress)
                && ((ca.DaysDelinquent != null && ca.DaysDelinquent.Value > 0) || bor.ReportNumber.IsIn(12, 13, 15, 16, 17, 18, 19, 20, 23, 24)))
            {
                ArcData arc = GetDelinqEndArcData(bor, ca);
                if (!arc.Comment.Contains("Endorser"))
                    arc.Comment = arc.Comment.Replace("Borrower", "Endorser");
                if (!bor.ReportNumber.IsIn(10, 11))
                    AddArc(bor, arc);
            }
        }

        /// <summary>
        /// Adds the ARC to the ArcAddProcessing table
        /// </summary>
        /// <param name="logData">ProcessLogData object</param>
        /// <param name="bor">BorrowerData object</param>
        /// <returns>ArcAddResults object</returns>
        private void AddArc(Borrower bor, ArcData arc)
        {
            if (arc.Arc.IsPopulated())
            {
                ArcAddResults result = arc.AddArc();
                if (!result.ArcAdded)
                {
                    string message = string.Format("There was an error adding an ARC for borrower {0}, Errors: {1}", bor.AccountNumber, string.Join(",", result.Errors));
                    PLR.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
            }
        }

        /// <summary>
        /// Every borrower gets a BILLS ARC
        /// </summary>
        /// <param name="bor">Borrower object</param>
        /// <param name="CA">CommentsAndArc object</param>
        /// <returns>A new ArcData object with the parameters needed to add an ARC</returns>
        private ArcData GetBILLSArcData(Borrower bor, CommentsAndArc ca)
        {
            List<string> fields = bor.LineData.SplitAndRemoveQuotes(",");
            return new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = bor.AccountNumber,
                Arc = "BILLS", //Default to BILLS, every borrower gets a BILLS
                ArcTypeSelected = ArcData.ArcType.Atd22ByLoan,
                Comment = ca.GetComment(bor, false, false),
                IsEndorser = false,
                IsReference = false,
                LoanPrograms = null,
                LoanSequences = GetSequences(fields),
                NeedBy = null,
                ProcessFrom = null,
                ProcessOn = null,
                ProcessTo = null,
                RecipientId = "",
                RegardsCode = null,
                RegardsTo = null,
                ResponseCode = null,
                ScriptId = Program.ScriptId
            };
        }

        /// <summary>
        /// Gets the ARC Data for borrowers in delinquency
        /// </summary>
        /// <param name="bor">BorrowerData object</param>
        /// <param name="ca">CommentsAndArc object</param>
        /// <returns>ArcData object</returns>
        private ArcData GetDelinquentArc(Borrower bor, CommentsAndArc ca)
        {
            List<string> fields = bor.LineData.SplitAndRemoveQuotes(",");
            return new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = bor.AccountNumber,
                Arc = ca.GetArc(bor, false, false),
                ArcTypeSelected = ArcData.ArcType.Atd22ByLoan,
                Comment = ca.GetComment(bor, false, false),
                IsEndorser = false,
                IsReference = false,
                LoanPrograms = null,
                LoanSequences = GetSequences(fields),
                NeedBy = null,
                ProcessFrom = null,
                ProcessOn = null,
                ProcessTo = null,
                RecipientId = "",
                RegardsCode = null,
                RegardsTo = null,
                ResponseCode = null,
                ScriptId = Program.ScriptId
            };
        }

        /// <summary>
        /// Gets the ARC data that will be added to the database
        /// </summary>
        /// <param name="bor">The borrower being processed</param>
        /// <param name="ca">CommentsAndArc object</param>
        /// <returns>ArcData object</returns>
        private ArcData GetEndorserArcData(Borrower bor, CommentsAndArc ca)
        {
            List<string> fields = bor.LineData.SplitAndRemoveQuotes(",");
            return new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = bor.AccountNumber,
                Arc = "BILLC",
                ArcTypeSelected = ArcData.ArcType.Atd22ByLoan,
                Comment = ca.GetComment(bor, ca.IsEndorser, ca.EndorserSameAddress),
                IsEndorser = true,
                IsReference = false,
                LoanPrograms = null,
                LoanSequences = GetSequences(fields),
                NeedBy = null,
                ProcessFrom = null,
                ProcessOn = null,
                ProcessTo = null,
                RecipientId = ca.EndorserSsn,
                RegardsCode = null,
                RegardsTo = null,
                ResponseCode = null,
                ScriptId = Program.ScriptId
            };
        }

        /// <summary>
        /// Get the ArcData for Endorsers in delinquency
        /// </summary>
        /// <param name="bor">BorrowerData object of the borrower being processed</param>
        /// <param name="ca">CommentsAndArc object</param>
        /// <returns>ArcData object</returns>
        private ArcData GetDelinqEndArcData(Borrower bor, CommentsAndArc ca)
        {
            List<string> fields = bor.LineData.SplitAndRemoveQuotes(",");
            return new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = bor.AccountNumber,
                Arc = ca.GetArc(bor, ca.EndorserSsn.IsPopulated(), ca.EndorserSameAddress),
                ArcTypeSelected = ArcData.ArcType.Atd22ByLoan,
                Comment = ca.GetComment(bor, ca.IsEndorser, ca.EndorserSameAddress),
                IsEndorser = true,
                IsReference = false,
                LoanPrograms = null,
                LoanSequences = GetSequences(fields),
                NeedBy = null,
                ProcessFrom = null,
                ProcessOn = null,
                ProcessTo = null,
                RecipientId = ca.EndorserSsn,
                RegardsCode = null,
                RegardsTo = null,
                ResponseCode = null,
                ScriptId = Program.ScriptId
            };
        }

        /// <summary>
        /// Gets all the loan sequences from the line data
        /// </summary>
        /// <param name="fields">list of all the fields in the line data</param>
        /// <returns>list of int's representing the loan sequences</returns>
        private List<int> GetSequences(List<string> fields)
        {
            List<int> loanSeq = new List<int>();
            for (int i = 14; i < 365; i += 13)
            {
                if (fields[i].ToInt() > 0)
                    loanSeq.Add(fields[i].ToInt());
            }
            return loanSeq;
        }

    }
}
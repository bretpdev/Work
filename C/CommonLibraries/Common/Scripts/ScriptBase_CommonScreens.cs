using System;
using System.Collections.Generic;

namespace Uheaa.Common.Scripts
{
    public partial class ScriptBase
    {
        
        protected bool Atd22AllLoans(string ssnOrAccountNum, string arc, string comment, string recipientId, string scriptId, bool pauseForManualComments)
        {
            return RI.Atd22AllLoans(ssnOrAccountNum, arc, comment, recipientId, scriptId, pauseForManualComments);
        }

        protected bool Atd22AllLoans(string ssnOrAccountNum, string arc, string comment, string recipientId, string scriptId, bool pauseForManualComments, bool reference = false)
        {
            return RI.Atd22AllLoans(ssnOrAccountNum, arc, comment, recipientId, scriptId, pauseForManualComments);
        }

        protected bool Atd22ByBalance(string ssnOrAccountNum, string arc, string comment, string recipientId, string scriptId, bool pauseForManualComments)
        {
            return RI.Atd22ByBalance(ssnOrAccountNum, arc, comment, recipientId, scriptId, pauseForManualComments, false);
        }

        protected bool Atd22ByBalance(string ssnOrAccountNum, string arc, string comment, string recipientId, string scriptId, bool pauseForManualComments, bool reference)
        {
            return RI.Atd22ByBalance(ssnOrAccountNum, arc, comment, recipientId, scriptId, pauseForManualComments, reference);
        }

        protected bool Atd22ByLoan(string ssnOrAccountNum, string arc, string comment, string recipientId, List<int> loanSequenceNumbers, string scriptId, bool pauseForManualComments)
        {
            return RI.Atd22ByLoan(ssnOrAccountNum, arc, comment, recipientId, loanSequenceNumbers, scriptId, pauseForManualComments);
        }

        protected bool Atd37(string ssnOrAccountNum, string arc, string comment, string recipientId, List<int> loanSequenceNumbers, string scriptId, bool pauseForManualComments)
        {
            RI.FastPath("TX3ZATD37" + ssnOrAccountNum);
            if (!RI.CheckForText(1, 72, "TDX38"))
                return false;
            //Find the ARC
            Coordinate arcLocation = RI.FindText(arc);
            while (arcLocation == null)
            {
                RI.Hit(ReflectionInterface.Key.F8);
                if (RI.CheckForText(23, 2, "90007"))
                    return false;
                arcLocation = RI.FindText(arc);
            }

            //Select the ARC
            RI.PutText(arcLocation.Row, arcLocation.Column - 5, "01", ReflectionInterface.Key.Enter);
            //Exit the function if the selection screen is not displayed
            if (!RI.CheckForText(1, 72, "TDX39"))
                return false;

            int row = 11;

            //Mark appropriate loans
            while (!RI.CheckForText(23, 2, "900007"))
            {
                foreach (int seq in loanSequenceNumbers)
                {
                    if (int.Parse(RI.GetText(row, 37, 4)) == seq)
                        RI.PutText(row, 18, "X");
                }
                row++;
            }
            RI.Hit(ReflectionInterface.Key.Enter);

            //Check to see if there was a selection made
            if (RI.CheckForText(23, 2, "01490") || RI.CheckForText(23, 2, "01764"))
                return false;

            RI.Hit(ReflectionInterface.Key.F4);
            RI.PutText(8, 5, string.Format("{0}  {1}{2}{3} /{4}", comment, "{", scriptId, "}", userId));
            RI.Hit(ReflectionInterface.Key.Enter);

            //Check to make sure the comment was successful
            if (RI.CheckForText(23, 2, "02114"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Get demographics from LP22
        /// </summary>
        /// <param name="accountIdentifier">The SSN (9 chars) or Account Number (10 chars)</param>
        /// <returns>A populated SystemBorrowerDemographics object</returns>
        protected SystemBorrowerDemographics GetDemographicsFromLP22(string accountIdentifier)
        {
            return ri.GetDemographicsFromLP22(accountIdentifier);
        }

        /// <summary>
        /// Add a queue task to LP9O
        /// </summary>
        /// <param name="ssn">Borrower SSN</param>
        /// <param name="workGroup">Workgroup ID</param>
        /// <param name="dueDate">Date due</param>
        /// <param name="comment1">First comment line</param>
        /// <param name="comment2">Second comment line</param>
        /// <param name="comment3">Third comment line</param>
        /// <param name="comment4">Fourth comment line</param>
        /// <returns></returns>
        protected bool AddQueueTaskInLP9O(string ssn, string workGroup, DateTime? dueDate, string comment1, string comment2, string comment3, string comment4)
        {
            return ri.AddQueueTaskInLP9O(ssn, workGroup, dueDate, comment1, comment2, comment3, comment4);
        }

        /// <summary>
        /// Adds a comment to LP50
        /// </summary>
        /// <param name="ssn">SSN for borrower/Reference Id for reference</param>
        /// <param name="activityType">Activity Type</param>
        /// <param name="activityContact">Contact Type</param>
        /// <param name="actionCode">Action code to use</param>
        /// <param name="comment">Comment must be less than 585 characters</param>
        /// <param name="scriptId">Script Id</param>
        /// <returns></returns>
        protected bool AddCommentInLP50(string ssn, string activityType, string activityContact, string actionCode, string comment, string scriptId)
        {
            return ri.AddCommentInLP50(ssn, activityType, activityContact, actionCode, comment, scriptId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endorserSsn"></param>
        /// <param name="borrowerSsn"></param>
        /// <returns></returns>
        public List<int> GetEndorsersLoanSeqences(string endorserSsn, string borrowerSsn)
        {
            List<int> loanSeqs = new List<int>();
            RI.FastPath("TX3Z/ITS26" + borrowerSsn);

            if (RI.CheckForText(1, 72, "T1X07"))
                return loanSeqs;

            if (RI.CheckForText(1, 72, "TSX28"))
                RI.PutText(21, 12, "01", ReflectionInterface.Key.Enter);

            RI.Hit(ReflectionInterface.Key.F4);

            for (int row = 10; !RI.CheckForText(23, 2, "90007"); row++)
            {
                if (row > 21)
                {
                    row = 9;
                    RI.Hit(ReflectionInterface.Key.F8);
                    continue;
                }

                if (RI.CheckForText(row, 52, "M"))
                {
                    if (endorserSsn.Contains(RI.GetText(row, 13, 9)))
                    {
                        loanSeqs.Add(int.Parse(RI.GetText(row, 9, 3)));
                    }
                }
            }

            return loanSeqs;
        }
    }
}

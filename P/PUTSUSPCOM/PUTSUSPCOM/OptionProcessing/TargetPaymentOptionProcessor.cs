using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;
using System.Windows.Forms;

namespace PUTSUSPCOM
{
    public class TargetPaymentOptionProcessor : OptionProcessorBase
    {

        public string LoanSequenceNumbers { get; set; }
        public string Amount { get; set; }

        public TargetPaymentOptionProcessor(ReflectionInterface ri, Suspense suspenseData)
            : base(ri, suspenseData)
        {
            LoanSequenceNumbers = string.Empty;
        }

        public override void Process()
        {
            //Target Suspense
            TargetSuspense();
            //Create comment string
            string assignedTo = (_systemSuspenseData.AssignedTo.StartsWith("U") ? "" : _systemSuspenseData.AssignedTo);
            string comment = string.Format("Target Suspense Payment {0} - Effective {1} for {2} batch # {3} for {4} {5}",
                                            assignedTo, _systemSuspenseData.EffectiveDate, _systemSuspenseData.Amount, _systemSuspenseData.BatchNumber, LoanSequenceNumbers,Comments);
            //Add comments
            AddComments(comment);
        }

        private void TargetSuspense()
        {
            FastPath(string.Format("TX3ZCTS3I{0}", _systemSuspenseData.FastPathText));
            while (Check4Text(1, 72, "TSX3G") == false)
            {
                MessageBox.Show("The script wasn't able to return to the suspense detail record to be targeted.  Please navigate to it again and hit <Insert> when you are done.", "Detail Screen Needed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PauseForInsert();
            }
            int row = 12;
            Hit(ReflectionInterface.Key.F6);
            while (Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
            {
                List<string> loanSeqNum = CreateLoanSequenceNumberList();
                if ((from l in loanSeqNum
                     where int.Parse(l) == int.Parse(GetText(row, 50, 2))
                     select l).Count() > 0)
                {
                    PutText(row, 3, "X");
                }
                row++;
                if (Check4Text(row, 3, "_") == false)
                {
                    row = 12;
                    Hit(ReflectionInterface.Key.F8);
                }
            }
            Hit(ReflectionInterface.Key.Enter);
            Hit(ReflectionInterface.Key.F12);
            PutText(11, 55, "N");
            Hit(ReflectionInterface.Key.Enter);
        }

        /// <summary>
        /// Add Comments
        /// </summary>
        /// <param name="comments"></param>
        protected override void AddComments(string comments)
        {
            FastPath(string.Format("TX3ZATC00{0}",_systemSuspenseData.BorrowerDemos.SSN));
            PutText(19,38,"4",ReflectionInterface.Key.Enter);
            PutText(12,10,comments, ReflectionInterface.Key.Enter);
            if (Check4Text(23,2,"01004 RECORD SUCCESSFULLY ADDED") == false)
            {
                if (ATD37FirstLoan(_systemSuspenseData.BorrowerDemos.SSN,"SUSDE",comments,SCRIPT_ID,false) != Common.CompassCommentScreenResults.CommentAddedSuccessfully)
                {
                    AddCommentInLP50(_systemSuspenseData.BorrowerDemos.SSN, "SUSDE", SCRIPT_ID, "MS", "16", comments);
                }
            }
        }

        /// <summary>
        /// Checks if loan sequence string is valid and returns a list of the sequence numbers.
        /// </summary>
        /// <returns></returns>
        public List<string> CreateLoanSequenceNumberList()
        {
            if (LoanSequenceNumbers == string.Empty)
            {
                throw new Exception("Empty loan sequence number entry.  Please try again.");
            }
            string tempLoanSequenceNumbers = LoanSequenceNumbers.Replace(" ", string.Empty);
            List<string> loanSequenceNumberList = tempLoanSequenceNumbers.Split(",".ToCharArray()[0]).ToList<string>();
            //check if any of the list entries aren't numeric
            if ((from lnSeq in loanSequenceNumberList
                 where lnSeq.IsNumeric() == false
                 select lnSeq).ToList<string>().Count > 0)
            {
                throw new Exception("Non-numeric entry found in loan sequence number list.  Please try again.");
            }
            return loanSequenceNumberList;
        }
    }
}

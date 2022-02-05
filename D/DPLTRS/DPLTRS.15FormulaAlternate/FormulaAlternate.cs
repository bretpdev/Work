using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.Scripts;

namespace DPLTRS._15FormulaAlternate
{
    public class LoanManagementLetters_15FormulaAlternate : ScriptBase
    {
        private LetterHelper lh;
        public LoanManagementLetters_15FormulaAlternate(ReflectionInterface ri)
            : base(ri, "DPLTRS")
        {
            lh = new LetterHelper(ri, ScriptId);
        }

        public override void Main()
        {
            var demos = lh.PromptForBorrower();
            if (demos != null && lh.LC05Check(demos) && lh.DefaultAndEDCheck(demos))
            {
                if (!demos.IsValidAddress)
                {
                    Dialog.Error.Ok("Borrower does not have a valid address. Please correct the issue before sending a letter.");
                    return;
                }

                if (!Dialog.Info.YesNo("Has the borrower filed a tax return in either of the last 2 completed tax years?"))
                {
                    List<string> addheader = new List<string>() { "DueDate" };
                    List<string> addVal = new List<string>() { "\"" + DateTime.Now.AddDays(14).ToShortDateString() + "\""};
                    lh.CreateLetter(demos, "ALT15", addheader, addVal);
                    AddCommentInLP50(demos.Ssn, "LT", "03", "DDALT", "Alternate Documentation of Income for 15% formula sent to borrower.", ScriptId);
                }
                else
                    Dialog.Info.Ok("Notify the borrower to send UHEAA their most recently filed 1040 tax filing form.");
            }
        }
    }
}

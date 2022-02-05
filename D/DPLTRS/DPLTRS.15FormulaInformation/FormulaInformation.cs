using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common.WinForms;

namespace DPLTRS._15FormulaInformation
{
    public class LoanManagementLetters_15FormulaInformation : ScriptBase
    {
        LetterHelper lh;
        public LoanManagementLetters_15FormulaInformation(ReflectionInterface ri)
            : base(ri, "DPLTRS")
        {
            lh = new LetterHelper(ri, ScriptId);
        }

        public override void Main()
        {
            var demos = lh.PromptForBorrower();
            if (demos != null && lh.LC05Check(demos) && lh.DefaultAndEDCheck(demos))
            {
                if(!demos.IsValidAddress)
                {
                    Dialog.Error.Ok("Borrower does not have a valid address. Please correct the issue before sending a letter.");
                    return;
                }

                List<string> addheader = new List<string>() { "DueDate" };
                List<string> addVal = new List<string>() { "\"" + DateTime.Now.AddDays(14).ToShortDateString() + "\"" };
                lh.CreateLetter(demos, "15INFO", addheader, addVal);
                AddCommentInLP50(demos.Ssn, "LT", "03", "D15IN", "15% Formula Information Letter sent to borrower.", ScriptId);
            }
        }


    }
}

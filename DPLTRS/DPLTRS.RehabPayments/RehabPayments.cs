using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.Scripts;

namespace DPLTRS.RehabPayments
{
    public class LoanManagementLetters_RehabPayments : ScriptBase
    {
        private LetterHelper lh;
        public LoanManagementLetters_RehabPayments(ReflectionInterface ri)
            : base(ri, "DPLTRS")
        {
            lh = new LetterHelper(ri, ScriptId);
        }

        public override void Main()
        {
            if (InitialPrompt())
            {
                var demos = lh.PromptForBorrower();
                if (demos != null && lh.LC05Check(demos) && lh.DefaultAndEDCheck(demos))
                {
                    if (!demos.IsValidAddress)
                    {
                        Dialog.Error.Ok("Borrower does not have a valid address. Please correct the issue before sending a letter.");
                        return;
                    }

                    lh.CreateLetter(demos, "FDRARHB");
                    AddCommentInLP50(demos.Ssn, "LT", "03", "DFDRA", "Financial Disclosure for Reasonable and Affordable Rehabilitation Payments form sent to borrower.", ScriptId);
                }
            }
        }

        private bool InitialPrompt()
        {
            if (Dialog.Info.YesNo("Has the borrower already applied for payments for the 15% formula?", ScriptId))
                return true;
            else
                Dialog.Info.Ok("Notify the borrower to send UHEAA their most recently filed 1040 tax filing form.");
            return false;
        }
    }
}

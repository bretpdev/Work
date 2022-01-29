using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common.WinForms;

namespace DPLTRS.MissingRAPInfo
{
    public class LoanManagementLetters_MissingRAPInfo : ScriptBase
    {
        LetterHelper lh;
        public LoanManagementLetters_MissingRAPInfo(ReflectionInterface ri)
            : base(ri, "DPLTRS")
        {
            lh = new LetterHelper(ri, ScriptId);
        }

        public override void Main()
        {
            SystemBorrowerDemographics demos = null;
            RapInfo info = null;
            using (var form = new MissingRapForm())
            {
                bool prompt = true;
                while (prompt)
                {
                    prompt = false;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        info = form.GetInfo();
                        demos = lh.ResolveBorrower(info.AccountIdentifier);
                        if (demos == null)
                            prompt = true;
                    }
                }
            }
            if (demos != null && lh.LC05Check(demos) && lh.DefaultAndEDCheck(demos))
            {
                if (!demos.IsValidAddress)
                {
                    Dialog.Error.Ok("Borrower does not have a valid address. Please correct the issue before sending a letter.");
                    return;
                }

                //HACK in .net 3.5 when the union method is called it removed dup and empty values so we have to padd the values with something they will be removed in the CreateLetter
                lh.CreateLetter(demos, "RAPDOC",
                    new string[] { "DueDate", "Item1", "Item2", "Item3", "Item4", "Item5", "Item6", "Item7", "Item8" },
                    new string[] { "\"" + info.DueDate + "\"", info.Item1.IsNullOrEmpty() ? "removeValue1" : info.Item1, info.Item2.IsNullOrEmpty() ? "removeValue2" : info.Item2, 
                        info.Item3.IsNullOrEmpty() ? "removeValue3" : info.Item3, info.Item4.IsNullOrEmpty() ? "removeValue4" : info.Item4, info.Item5.IsNullOrEmpty() ? "removeValue5" : info.Item5,
                       info.Item6.IsNullOrEmpty() ? "removeValue6" : info.Item6, info.Item7.IsNullOrEmpty() ? "removeValue7" : info.Item7,info.Item8.IsNullOrEmpty() ? "removeValue8" : info.Item8 });

                List<string> items = new List<string>() { info.DueDate, info.Item1, info.Item2, info.Item3, info.Item4, info.Item5, info.Item6, info.Item7, info.Item8 };

                AddCommentInLP50(demos.Ssn, "LT", "03", "DRAPM", string.Format("Incomplete RAP form received.  Sent request for additional information.  Due {0}", string.Join("\n", items.Select(e => ", " + e).ToArray())), ScriptId);
            }
        }
    }
}

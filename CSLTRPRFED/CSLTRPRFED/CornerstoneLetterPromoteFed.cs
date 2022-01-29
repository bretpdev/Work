using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace CSLTRPRFED
{
    public class CornerstoneLetterPromoteFed : FedScript
    {
        private ErrorReport Err;
        private bool HasErrors;

        public CornerstoneLetterPromoteFed(ReflectionInterface ri)
            : base(ri, "CSLTRPRFED")
        {
            Err = new ErrorReport("CornerStone Incomplete Promotion Letters", "ERR_BU35");
            HasErrors = false;
        }

        public override void Main()
        {
            //This method will check to ensure the user has access to the correct screens.
            CheckScreenAccess();

            string startupMessage = "This script will promote letters in CornerStone. Click OK to continue, or Cancel to quit.";
            if (MessageBox.Show(startupMessage, ScriptId, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
                EndDllScript();

            List<string> letterIdsToPromote = new List<string>();

            using (LetterInput letterInput = new LetterInput(letterIdsToPromote))
            {
                if (letterInput.ShowDialog() == DialogResult.Cancel)
                    EndDllScript();
            }

            PromoteTheLetters(letterIdsToPromote);
            Err.Publish();
            string message = HasErrors ? string.Format("Processing is complete. However, some Letter IDs may be invalid or in an incomplete status. Please refer to {0} titled CornerStone Incomplete Promotion Letters.", EnterpriseFileSystem.GetPath("ERR_BU35")) : "Processing Complete.";
            MessageBox.Show(message);
            EndDllScript();
        }//end main

        /// <summary>
        /// Checks to see if the user has access to CTX7G and CTX8U
        /// </summary>
        private void CheckScreenAccess()
        {
            FastPath("TX3Z/CTX7G");

            if (!CheckForText(1, 72, "TXX7H"))
            {
                MessageBox.Show("You do not have access to CTX7G.  Please Contact Systems Support.", "No Access", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EndDllScript();
            }

            FastPath("TX3Z/CTX8U");

            if (!CheckForText(1, 72, "TXX7H"))
            {
                MessageBox.Show("You do not have access to CTX7G.  Please Contact Systems Support.", "No Access", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EndDllScript();
            }
        }//end CheckScreen

        /// <summary>
        /// Will go though TX7G and TX8U and promote each letter.
        /// </summary>
        /// <param name="letterIdsToPromote">List of all the letters to promote.</param>
        private void PromoteTheLetters(List<string> letterIdsToPromote)
        {
            foreach (string letter in letterIdsToPromote)
            {
                FastPath(string.Format("TX3Z/CTX7G{0}", letter));

                if (!CheckForText(1, 72, "TXX7I"))
                {
                    Err.AddRecord("The follwing letter id was not promoted: ", new { Letter = letter, ErrorMessage = GetText(23,2,77) });
                    HasErrors = true;
                    continue;
                }

                PutText(8, 19, "Y", ReflectionInterface.Key.Enter);

                if (!CheckForText(23, 2, "01005 RECORD SUCCESSFULLY CHANGED"))
                {
                    Err.AddRecord("The follwing letter id was not promoted: ", new { Letter = letter, ErrorMessage = GetText(23, 2, 77) });
                    HasErrors = true;
                    continue;
                }

                FastPath(string.Format("TX3Z/CTX8U{0}", letter));

                PutText(8, 19, "C", ReflectionInterface.Key.Enter);

                if (!CheckForText(23, 2, "01005 RECORD SUCCESSFULLY CHANGED"))
                {
                    Err.AddRecord("The follwing letter id was not promoted: ", new { Letter = letter, ErrorMessage = GetText(23, 2, 77) });
                    HasErrors = true;
                    continue;
                }

                FastPath(string.Format("TX3Z/CTX7G{0}", letter));
                PutText(8, 19, "P", ReflectionInterface.Key.Enter);

                if (!CheckForText(23, 2, "01005 RECORD SUCCESSFULLY CHANGED"))
                {
                    Err.AddRecord("The follwing letter id was not promoted: ", new { Letter = letter, ErrorMessage = GetText(23, 2, 77) });
                    HasErrors = true;
                    continue;
                }
            }//end foreach
        }//end PromoteTheLetter
    }
}

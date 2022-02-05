using System;
using Uheaa.Common;
using Match = System.Text.RegularExpressions.Match;

namespace NHGeneral
{
    public class SsnHelper
    {
        private string NineDigitPattern
        {
            get
            {
                return @"(\d{3}-?\d{2}-?\d{4}|XXX-XX-XXXX|\d{9}|\d{3} \d{2} \d{4}|XXX XX XXXX)";
            }
        }
        private string TenDigitPattern
        {
            get
            {
                return @"(\d{4}-?\d{2}-?\d{4}|\d{3}-?\d{2}-?\d{5}|XXX-XX-XXXXX|XXXX-XX-XXXX|\d{10}|\d{4} \d{2} \d{4}|\d{3} \d{2} \d{5}|XXX XX XXXXX|XXXX XX XXXX|\d{9}\w|\w\d{9})";
            }
        }
        private bool SsnFound { get; set; }
        private string OriginalText { get; set; }
        private string UpdatedText { get; set; }
        public bool FormNotCanceled { get; set; }
        public HandleSsn UpdateSsn { get; set; }
        public NeedHelpTickets TicketForm { get; set; }

        public SsnHelper(NeedHelpTickets ticketForm)
        {
            UpdateSsn = new HandleSsn();
            FormNotCanceled = true;
            TicketForm = ticketForm;
        }

        public string MaskSsnIfExists(string text)
        {
            if (!FormNotCanceled) //If user click cancel on mask ssn form, do not continue processing
                return "";
            OriginalText = text;
            UpdatedText = text;
            SsnFound = false;
            while (PossibleSsn(text))
                text = MaskAllNumbers(text);

            if (CheckIfUpdateNeeded() && FormNotCanceled && UpdateSsn.ShouldMask.Value)
                return UpdatedText;
            else
                return OriginalText;
        }

        /// <summary>
        /// Checks to see if the text being passed in has a possible SSN to check for
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool PossibleSsn(string text)
        {
            return Regex.Match(text, NineDigitPattern).Success;
        }

        private string MaskAllNumbers(string text)
        {
            Match m = Regex.Match(text, NineDigitPattern);
            string textToCheckAfter = IncrementFoundValue(text, m);
            string texttocheckBefore = m.Index > 0 ? text.Substring(m.Index - 1, 10) : m.Value;
            if (Regex.Match(textToCheckAfter, TenDigitPattern).Success)
                text = string.Format("{0}{1}{2}",
                    UpdatedText.Substring(0, m.Index),
                    ReplaceChars(textToCheckAfter),
                    text.Substring(m.Index + (HasSeparator(m.Value) ? 12 : 10)));
            else if (Regex.Match(texttocheckBefore, TenDigitPattern).Success)
                text = string.Format("{0}{1}{2}",
                    UpdatedText.Substring(0, m.Index - 1),
                    ReplaceChars(texttocheckBefore),
                    text.Substring((m.Index - 1) + (HasSeparator(m.Value) ? 12 : 10)));
            else if (m.Value.IsPopulated())
            {
                //only update the UpdatedText if the regex did not find a match for 10 and the text is a length of 9
                UpdatedText = string.Format("{0}{1}{2}",
                    UpdatedText.Substring(0, m.Index),
                    m.Value.MaskSsn(),
                    UpdatedText.Substring(m.Index + (HasSeparator(m.Value) ? 11 : 9)));
                text = string.Format("{0}{1}{2}",
                    text.Substring(0, m.Index),
                    ReplaceChars(m.Value.MaskSsn()),
                    text.Substring(m.Index + (HasSeparator(m.Value) ? 11 : 9)));
                SsnFound = true;
            }
            return text;
        }

        private string ReplaceChars(string value)
        {
            string updated = "";
            for (int i = 0; i < value.Length; i++)
                updated += "X";
            return updated;
        }

        private string IncrementFoundValue(string text, Match m)
        {
            string textToCheck;
            if (HasSeparator(m.Value))
            {
                if (text.Length > m.Index + 11)
                    textToCheck = m.Value + text.Substring(m.Index + 11, 1).Trim();
                else
                    textToCheck = m.Value;
            }
            else
            {
                if (text.Length > m.Index + 9)
                    textToCheck = m.Value + text.Substring(m.Index + 9, 1).Trim();
                else
                    textToCheck = m.Value;
            }

            return textToCheck;
        }

        /// <summary>
        /// Checks to see if the text being passed in has a separator value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool HasSeparator(string text)
        {
            if (text.Contains(" "))
                return true;
            if (text.Contains("-"))
                return true;
            return false;
        }

        private bool CheckIfUpdateNeeded()
        {
            if (SsnFound && UpdateSsn.ShouldMask == null)
            {
                if (TicketForm != null)
                    TicketForm.Hide();
                UpdateSsn.ShowDialog();
                if (TicketForm != null)
                    TicketForm.Show();
                return FormNotCanceled = UpdateSsn.CloseForm;
            }
            return UpdateSsn.ShouldMask ?? false;
        }
    }
}
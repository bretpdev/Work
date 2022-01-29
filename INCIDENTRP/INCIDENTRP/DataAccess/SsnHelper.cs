using System.Windows.Forms;
using Uheaa.Common;
using Match = System.Text.RegularExpressions.Match;

namespace INCIDENTRP
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
                return @"(\d{4}-?\d{2}-?\d{4}|\d{3}-?\d{2}-?\d{5}|XXX-XX-XXXXX|XXXX-XX-XXXX|\d{10}|\d{4} \d{2} \d{4}|\d{3} \d{2} \d{5}|XXX XX XXXXX|XXXX XX XXXX)";
            }
        }
        private bool SsnFound { get; set; }
        private string OriginalText { get; set; }
        private string UpdatedText { get; set; }
        public bool FormNotCanceled { get; set; }
        public HandleSsn UpdateSsn { get; set; }
        private Form PassedForm { get; set; }

        public SsnHelper(Form form)
        {
            UpdateSsn = new HandleSsn();
            FormNotCanceled = true;
            PassedForm = form;
        }

        public string MaskSsnIfExists(string text)
        {
            if (!FormNotCanceled) //If user click cancel on mask ssn form, do not continue processing
                return "";
            OriginalText = text;
            UpdatedText = text;
            SsnFound = false;
            if (text == "")
                return "";
            if (text == null)
                return null;
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
                text = $"{UpdatedText.Substring(0, m.Index)}"
                    + $"{ReplaceChars(textToCheckAfter)}"
                    + $"{text.Substring(m.Index + (HasSeparator(m.Value) ? 12 : 10))}";
            else if (Regex.Match(texttocheckBefore, TenDigitPattern).Success)
                text = $"{UpdatedText.Substring(0, m.Index - 1)}"
                    + $"{ReplaceChars(texttocheckBefore)}"
                    + $"{text.Substring((m.Index - 1) + (HasSeparator(m.Value) ? 12 : 10))}";
            else if (m.Value.IsPopulated())
            {
                //only update the UpdatedText if the regex did not find a match for 10 and the text is a length of 9
                UpdatedText = $"{UpdatedText.Substring(0, m.Index)}"
                    + $"{m.Value.MaskSsn()}"
                    + $"{UpdatedText.Substring(m.Index + (HasSeparator(m.Value) ? 11 : 9))}";
                text = $"{text.Substring(0, m.Index)}"
                    + $"{ReplaceChars(m.Value.MaskSsn())}"
                    + $"{text.Substring(m.Index + (HasSeparator(m.Value) ? 11 : 9))}";
                SsnFound = true;
            }
            return text;
        }

        private string ReplaceChars(string value)
        {
            string updated = "";
            for (int i = 0; i < value.Length; i++)
            {
                updated += "X";
            }
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
                if (PassedForm != null)
                    PassedForm.Hide();
                UpdateSsn.ShowDialog();
                UpdateSsn.BringToFront();
                if (PassedForm != null)
                    PassedForm.Show();
                return FormNotCanceled = UpdateSsn.CloseForm;
            }
            return UpdateSsn.ShouldMask ?? false;
        }

        /// <summary>
        /// Checks all the threats for SSN
        /// </summary>
        public void ThreatCheck(Ticket ticket)
        {
            if (ticket.Threat.BombInfo != null)
            {
                ticket.Threat.BombInfo.Appearance = MaskSsnIfExists(ticket.Threat.BombInfo.Appearance);
                ticket.Threat.BombInfo.CallerName = MaskSsnIfExists(ticket.Threat.BombInfo.CallerName);
                ticket.Threat.BombInfo.DetonationTime = MaskSsnIfExists(ticket.Threat.BombInfo.DetonationTime);
                ticket.Threat.BombInfo.Location = MaskSsnIfExists(ticket.Threat.BombInfo.Location);
                ticket.Threat.BombInfo.WhoPlacedAndWhy = MaskSsnIfExists(ticket.Threat.BombInfo.WhoPlacedAndWhy);
            }
            if (ticket.Threat.Caller != null)
            {
                ticket.Threat.Caller.AccountNumber = MaskSsnIfExists(ticket.Threat.Caller.AccountNumber);
                ticket.Threat.Caller.Address = MaskSsnIfExists(ticket.Threat.Caller.Address);
                ticket.Threat.Caller.Age = MaskSsnIfExists(ticket.Threat.Caller.Age);
                ticket.Threat.Caller.BackgroundNoise.OtherDescription = MaskSsnIfExists(ticket.Threat.Caller.BackgroundNoise.OtherDescription);
                ticket.Threat.Caller.CallDuration = MaskSsnIfExists(ticket.Threat.Caller.CallDuration);
                ticket.Threat.Caller.Dialect.ForeignAccentDescription = MaskSsnIfExists(ticket.Threat.Caller.Dialect.ForeignAccentDescription);
                ticket.Threat.Caller.Dialect.RegionalAmericanDescription = MaskSsnIfExists(ticket.Threat.Caller.Dialect.RegionalAmericanDescription);
                ticket.Threat.Caller.FamiliaritySpecifics = MaskSsnIfExists(ticket.Threat.Caller.FamiliaritySpecifics);
                ticket.Threat.Caller.Language.OtherDescription = MaskSsnIfExists(ticket.Threat.Caller.Language.OtherDescription);
                ticket.Threat.Caller.Manner.OtherDescription = MaskSsnIfExists(ticket.Threat.Caller.Manner.OtherDescription);
                ticket.Threat.Caller.Name = MaskSsnIfExists(ticket.Threat.Caller.Name);
                ticket.Threat.Caller.PhoneNumber = MaskSsnIfExists(ticket.Threat.Caller.PhoneNumber);
                ticket.Threat.Caller.Voice.OtherDescription = MaskSsnIfExists(ticket.Threat.Caller.Voice.OtherDescription);
            }
            if (ticket.Threat.Info != null)
            {
                ticket.Threat.Info.AdditionalRemarks = MaskSsnIfExists(ticket.Threat.Info.AdditionalRemarks);
                ticket.Threat.Info.NatureOfCall = MaskSsnIfExists(ticket.Threat.Info.NatureOfCall);
                ticket.Threat.Info.WordingOfThreat = MaskSsnIfExists(ticket.Threat.Info.WordingOfThreat);
            }
            if (ticket.Threat.InformationTechnologyAction != null)
                ticket.Threat.InformationTechnologyAction.PersonContacted = MaskSsnIfExists(ticket.Threat.InformationTechnologyAction.PersonContacted);
            if (ticket.Threat.LawEnforcementAction != null)
                ticket.Threat.LawEnforcementAction.PersonContacted = MaskSsnIfExists(ticket.Threat.LawEnforcementAction.PersonContacted);
            if (ticket.Threat.Notifier != null)
            {
                ticket.Threat.Notifier.EmailAddress = MaskSsnIfExists(ticket.Threat.Notifier.EmailAddress);
                ticket.Threat.Notifier.Name = MaskSsnIfExists(ticket.Threat.Notifier.Name);
                ticket.Threat.Notifier.OtherMethod = MaskSsnIfExists(ticket.Threat.Notifier.OtherMethod);
                ticket.Threat.Notifier.OtherRelationship = MaskSsnIfExists(ticket.Threat.Notifier.OtherRelationship);
                ticket.Threat.Notifier.OtherType = MaskSsnIfExists(ticket.Threat.Notifier.OtherType);
                ticket.Threat.Notifier.PhoneNumber = MaskSsnIfExists(ticket.Threat.Notifier.PhoneNumber);
            }
            if (ticket.Threat.Reporter != null)
                ticket.Threat.Reporter.PhoneNumber = MaskSsnIfExists(ticket.Threat.Reporter.PhoneNumber);
            foreach (HistoryRecord history in ticket.History)
                history.UpdateText = MaskSsnIfExists(history.UpdateText);
        }

        /// <summary>
        /// Checks all the incidents for SSN
        /// </summary>
        public void IncidentCheck(Ticket ticket)
        {
            if (ticket.Incident.ActionsTaken.Count > 0)
            {
                foreach (ActionTaken action in ticket.Incident.ActionsTaken)
                {
                    if (action.PersonContacted != null)
                        action.PersonContacted = MaskSsnIfExists(action.PersonContacted);
                }
            }
            if (ticket.Incident.AgencyDataInvolved != null)
                ticket.Incident.AgencyDataInvolved.OtherInformation = MaskSsnIfExists(ticket.Incident.AgencyDataInvolved.OtherInformation);
            if (ticket.Incident.AgencyEmployeeDataInvolved != null)
            {
                ticket.Incident.AgencyEmployeeDataInvolved.Name = MaskSsnIfExists(ticket.Incident.AgencyEmployeeDataInvolved.Name);
                ticket.Incident.AgencyEmployeeDataInvolved.NotifierRelationshipToEmployee = MaskSsnIfExists(ticket.Incident.AgencyEmployeeDataInvolved.NotifierRelationshipToEmployee);
            }
            if (ticket.Incident.BorrowerDataInvolved != null)
            {
                ticket.Incident.BorrowerDataInvolved.AccountNumber = MaskSsnIfExists(ticket.Incident.BorrowerDataInvolved.AccountNumber);
                ticket.Incident.BorrowerDataInvolved.Name = MaskSsnIfExists(ticket.Incident.BorrowerDataInvolved.Name);
                ticket.Incident.BorrowerDataInvolved.NotifierRelationshipToPiiOwner = MaskSsnIfExists(ticket.Incident.BorrowerDataInvolved.NotifierRelationshipToPiiOwner);
            }
            if (ticket.Incident.Notifier != null)
            {
                ticket.Incident.Notifier.EmailAddress = MaskSsnIfExists(ticket.Incident.Notifier.EmailAddress);
                ticket.Incident.Notifier.Name = MaskSsnIfExists(ticket.Incident.Notifier.Name);
                ticket.Incident.Notifier.OtherMethod = MaskSsnIfExists(ticket.Incident.Notifier.OtherMethod);
                ticket.Incident.Notifier.OtherType = MaskSsnIfExists(ticket.Incident.Notifier.OtherType);
                ticket.Incident.Notifier.PhoneNumber = MaskSsnIfExists(ticket.Incident.Notifier.PhoneNumber);
                ticket.Incident.Notifier.Relationship = MaskSsnIfExists(ticket.Incident.Notifier.Relationship);
            }
            if (ticket.Incident.Reporter != null)
            {
                ticket.Incident.Reporter.Location = MaskSsnIfExists(ticket.Incident.Reporter.Location);
                ticket.Incident.Reporter.PhoneNumber = MaskSsnIfExists(ticket.Incident.Reporter.PhoneNumber);
            }
            if (ticket.Incident.ThirdPartyDataInvolved != null)
            {
                ticket.Incident.ThirdPartyDataInvolved.AccountNumber = MaskSsnIfExists(ticket.Incident.ThirdPartyDataInvolved.AccountNumber);
                ticket.Incident.ThirdPartyDataInvolved.Name = MaskSsnIfExists(ticket.Incident.ThirdPartyDataInvolved.Name);
                ticket.Incident.ThirdPartyDataInvolved.NotifierRelationshipToPiiOwner = MaskSsnIfExists(ticket.Incident.ThirdPartyDataInvolved.NotifierRelationshipToPiiOwner);
            }
            ticket.Incident.Cause = MaskSsnIfExists(ticket.Incident.Cause);
            ticket.Incident.Location = MaskSsnIfExists(ticket.Incident.Location);
            ticket.Incident.Narrative = MaskSsnIfExists(ticket.Incident.Narrative);
            foreach (HistoryRecord history in ticket.History)
                history.UpdateText = MaskSsnIfExists(history.UpdateText);
        }

    }
}

using LSLETTERSU.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace LSLETTERSU.Processing
{
    public class LoanServicingLetters
    {
        public InputData UserInput { get; set; }
        public DataAccess DA { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public ErrorMessage Error { get; set; }
        public string ScriptId { get; set; }

        public LoanServicingLetters(InputData userInput, DataAccess da, ProcessLogRun logRun, string scriptId)
        {
            UserInput = userInput;
            DA = da;
            LogRun = logRun;
            ScriptId = scriptId;
            Error = new ErrorMessage();
        }

        public ErrorMessage Process()
        {
            try
            {
                UserInput.Demos = DA.GetDemos(UserInput.AccountNumber);
                if (UserInput.Demos == null || (UserInput.Demos.AccountNumber.IsNullOrEmpty() && UserInput.Demos.Ssn.IsNullOrEmpty()))
                    Error.Message = "Unable to locate the borrower in the local warehouse. Please try again";
                else
                {
                    List<Endorsers> endorsers = DA.GetCoborrowers(UserInput.Demos.Ssn);
                    List<EndorserData> eData = LoadEndorser(endorsers);
                    return DetermineAndGenerateLetter(UserInput.Demos, eData);
                }
            }
            catch (Exception ex)
            {
                LogRun.AddNotification(ex.Message, NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                Error.Message = $"There was an error in processing Please contact System Support and reference Process Log ID: {LogRun.ProcessLogId}";
            }
            return Error;
        }

        private List<EndorserData> LoadEndorser(List<Endorsers> endorsers)
        {
            List<EndorserData> eData = new List<EndorserData>();
            if (endorsers.Count > 0)
            {
                foreach (Endorsers end in endorsers.DistinctBy(p => p.LF_EDS))
                {
                    SystemBorrowerDemographics eDemos = DA.GetDemos(end.LF_EDS);
                    eData.Add(new EndorserData()
                    {
                        Ssn = end.LF_EDS,
                        SequenceNumbers = endorsers.Where(p => p.LF_EDS == end.LF_EDS).Select(p => p.LN_SEQ).ToList(),
                        EndorserDemo = eDemos,
                        EndorserEcorr = EcorrProcessing.CheckEcorr(eDemos.AccountNumber)
                    });
                }
            }
            return eData;
        }

        /// <summary>
        /// Determine Ecorr status, call method to generate letter then call method to generate ARC
        /// </summary>
        private ErrorMessage DetermineAndGenerateLetter(SystemBorrowerDemographics bDemos, List<EndorserData> eData)
        {
            UpdateMergeFields();
            EndorserData originalEndorser = eData.FirstOrDefault(); //Keep the original list endorser for comments
            ErrorMessage em = new LetterGenerator(DA, LogRun, "LSLETTERSU", UserInput.Letters).GenerateTheLetter(UserInput, bDemos, eData);
            em = AddComment(originalEndorser, bDemos, em);
            if (em.Message.IsPopulated())
                em.Message = $"{em.Message} {em.EndorserMessage}";
            return em;
        }

        /// <summary>
        /// Gets a list of merge fields replace text in the denial reason
        /// </summary>
        private void UpdateMergeFields()
        {
            foreach (string denial in UserInput.DenialReasons)
            {
                LetterData letter = UserInput.Letters.Where(p => p.LetterChoices == denial).FirstOrDefault();
                int? id = letter?.LoanServicingLettersId;
                if (id.HasValue)
                {
                    List<MergeFields> fields = DA.GetMergeFields(id.Value);
                    if (fields != null)
                    {
                        foreach (MergeFields field in fields)
                            denial.Replace(field.MergeField, field.FormField);
                    }
                }
            }
        }

        /// <summary>
        /// Adds ARC to the ArcAddProcessing table if the letter was generated
        /// </summary>
        private ErrorMessage AddComment(EndorserData eData, SystemBorrowerDemographics demos, ErrorMessage emLetter)
        {
            ErrorMessage em = new ErrorMessage();
            EcorrData borrowerEcorr = EcorrProcessing.CheckEcorr(demos.AccountNumber);
            LetterData selectedLetter = UserInput.Letters.Where(p => p.LoanServicingLettersId == UserInput.LoanServicingLettersId).FirstOrDefault();
            string borAndCoBor = DetermineDeliveryMessage(eData, demos.IsValidAddress, ((borrowerEcorr?.LetterIndicator ?? false) && (borrowerEcorr?.ValidEmail ?? false)));
            string comment = DetermineDispositionMessage(borAndCoBor, selectedLetter);
            if (comment.IsNullOrEmpty())
            {
                em.Message = $"There was an error adding the ARC: {selectedLetter.Arc} for borrower account: {UserInput.AccountNumber} because the comment was missing";
                LogRun.AddNotification(em.Message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return em;
            }

            ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = UserInput.AccountNumber,
                Arc = selectedLetter.Arc,
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Comment = comment,
                ScriptId = ScriptId,
                RunBy = ActiveDirectoryUsers.UserName
            };

            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"There was an error adding the ARC: {selectedLetter.Arc}, Comment: {comment} for borrower account: {UserInput.AccountNumber}.";
                em.Message = message;
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, result.Ex);
            }

            if (emLetter.Message.IsPopulated() || emLetter.EndorserMessage.IsPopulated() || em.Message.IsPopulated())
                emLetter.Message = $"{borAndCoBor} {emLetter.Message + emLetter.EndorserMessage} {em.Message}".Trim();
            return emLetter;
        }

        /// <summary>
        /// Determine the comment to add to the ARC
        /// </summary>
        private string DetermineDeliveryMessage(EndorserData eData, bool hadValidMailingAddress, bool borrowerEcorr)
        {
            string message = "";
            bool notifyBorrower = (hadValidMailingAddress || borrowerEcorr);
            bool notifyEndorser = false;
            if (eData != null)
                notifyEndorser = (eData.EndorserDemo.IsValidAddress || (eData.EndorserEcorr.LetterIndicator && eData.EndorserEcorr.ValidEmail));


            if (eData == null && notifyBorrower)
                message = "Sent letter to borrower.";
            else if (eData == null && !notifyBorrower)
                message = "No letter sent to borrower (invalid mailing address).";
            else if (notifyBorrower && notifyEndorser)
                message = "Sent letter to borrower and co-borrower.";
            else if (!notifyBorrower && notifyEndorser)
                message = "Sent letter to co-borrower.  No letter sent to borrower (invalid mailing address).";
            else if (notifyBorrower && !notifyEndorser)
                message = "Sent letter to borrower.  No letter sent to co-borrower (invalid mailing address).";
            else if (!notifyBorrower && !notifyEndorser)
                message = "No letter sent to borrower or co-borrower (invalid mailing address).";
            return message;
        }

        /// <summary>
        /// Create comment from denial reason and replace you/your with they/their
        /// </summary>
        private string DetermineDispositionMessage(string borrowerAndCoborrower, LetterData letter)
        {
            string comment = "";
            string denialReason = string.Empty;
            foreach (string item in UserInput.DenialReasons)
                denialReason += item + ",";
            if (denialReason.IsPopulated())
            {
                denialReason = denialReason.Remove(denialReason.LastIndexOf(","), 1);
                denialReason = denialReason.ToLower().Replace("your", "their");
                denialReason = denialReason.ToLower().Replace("you", "they");
            }
            else
                return null;
            comment = $"Borrower is denied for {letter.LetterOptions} {letter.LetterType} because {denialReason} {borrowerAndCoborrower}";

            return comment;
        }
    }
}
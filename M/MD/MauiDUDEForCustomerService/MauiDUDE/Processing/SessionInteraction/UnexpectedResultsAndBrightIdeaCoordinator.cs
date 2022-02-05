using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace MauiDUDE
{
    public class UnexpectedResultsAndBrightIdeaCoordinator
    {
        public enum EmailCommentProcessingOptions
        {
            BrightIdea,
            UnexpectedResults
        }

        /// <summary>
        /// point of entry for bright idea and unexpected results functionality
        /// </summary>
        public static void ShowBrightIdeaOrUnexpectedResults(EmailCommentProcessingOptions option)
        {
            if(option == EmailCommentProcessingOptions.BrightIdea)
            {
                BrightIdea.ShowBrightIdea();
            }
            else
            {
                UnexpectedResults.ShowUnexpectedResults();
            }
        }

        /// <summary>
        /// Does processing for the bright idea functionality
        /// </summary>
        public void BrightIdeaProcessing(string userComments)
        {
            string subject = "Maui DUDE - Good Idea";
            if(DataAccessHelper.TestMode)
            {
                subject = $"TEST EMAIL PLEASE IGNORE == {subject}";
            }
            EmailHelper.SendMail(DataAccessHelper.TestMode, SessionInteractionComponents.MAUI_DUDE_EMAIL_ADDRESS, $"{Environment.UserName}@utahsbr.edu", subject, userComments, "", EmailHelper.EmailImportance.Normal, false);
        }

        public void UnexpectedResultsProcessing(string userComments)
        {
            SessionInteractionComponents.RI.ReflectionSession.PrintFileName = $"{EnterpriseFileSystem.TempFolder}MD Reflection Unexpected.txt";
            SessionInteractionComponents.RI.ReflectionSession.PrintFileExistsAction = (int)Reflection.Constants.rcOverwrite;
             SessionInteractionComponents.RI.ReflectionSession.PrintToFile = 1; //true;
             SessionInteractionComponents.RI.ReflectionSession.PrintScreen((int)Reflection.Constants.rcPrintScreen, 1); //create print screen in file
            SessionInteractionComponents.RI.ReflectionSession.PrintToFile = 0; //false
            string subject = "Maui DUDE - Unexpected Result";
            if(DataAccessHelper.TestMode)
            {
                subject = $"TEST EMAIL PLEASE IGNORE -- {subject}";
            }
            EmailHelper.SendMail(DataAccessHelper.TestMode, SessionInteractionComponents.MAUI_DUDE_EMAIL_ADDRESS, $"{Environment.UserName}@utahsbr.edu", subject, userComments, "", EmailHelper.EmailImportance.Normal, false);
        }
    }
}

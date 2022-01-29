using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace ACURINTR.DemographicsParsers
{
    class OneLinkCommaParser : DemographicsParserBase
    {
        public OneLinkCommaParser(ReflectionInterface ri, DataAccess da) : base(ri, da) { }

        /// <summary>
        /// Create a QueueTask object from the session demographic info
        /// </summary>
        /// <returns>QueueTask Object</returns>
        public override QueueTask Parse()
        {
            string commentText = (RI.GetText(12, 11, 58) + RI.GetText(13, 11, 58) + RI.GetText(14, 11, 58) + RI.GetText(15, 11, 26)).Trim('_').Trim(); //Parse the address from the comment text.
            if (commentText == "") { throw new ParseException("", "", "", RejectAction.Sources.DUDE, ""); }
            string tempOriginalDemographicsText = commentText.Substring(0, General.FindEndOfCommentText(commentText) + 1);
            string[] elements = tempOriginalDemographicsText.Split(',');

            if (elements.Length != 10 && elements.Length != 11) { throw new ParseException("", "", "", RejectAction.Sources.DUDE, tempOriginalDemographicsText); }

            QueueTask task;
            switch (elements[9])
            {
                case "EMAIL":
                    task = new QueueTask(RejectAction.Sources.EMAIL, SystemCode.Sources.ONELINK_EMAIL, DA);
                    break;
                case "AUTOPAY":
                    task = new QueueTask(RejectAction.Sources.AUTOPAY, SystemCode.Sources.AUTOPAY, DA);
                    break;
                default:
                    task = new QueueTask(RejectAction.Sources.DUDE, SystemCode.Sources.DUDE, DA);
                    task.AdditionalInfo = elements[9];
                    break;
            }

            task.OriginalDemographicsText = tempOriginalDemographicsText;
            AccurintRDemographics taskDemographics = new AccurintRDemographics();
            taskDemographics.Address1 = elements[0].Trim();
            taskDemographics.Address2 = elements[1].Trim();
            taskDemographics.City = elements[2].Trim();
            taskDemographics.State = elements[3].Trim();
            taskDemographics.ZipCode = elements[4].Trim();
            taskDemographics.Country = elements[5].Trim();
            taskDemographics.PrimaryPhone = elements[6].Trim();
            taskDemographics.AlternatePhone = elements[7].Trim();
            taskDemographics.EmailAddress = elements[8].Trim();
            if (elements.Length == 11)
                taskDemographics.ConsentIndicator = elements[10].Trim();
            if (RI.CheckForText(1, 71, "QUEUE"))
            {
                //Use the borrower's SSN.
                taskDemographics.Ssn = RI.GetText(17, 70, 9);
                taskDemographics.AccountNumber = RI.GetText(17, 52, 12).Replace(" ", "");
            }
            else //1, 67, "WORKGROUP"
            {
                //Use the target SSN.
                taskDemographics.Ssn = RI.GetText(5, 70, 9);
                taskDemographics.AccountNumber = "";
            }
            task.Demographics = taskDemographics;

            return task;
        }
    }
}

using System;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace ACURINTR.DemographicsParsers
{
	class OneLinkPostOfficeCommaParser : DemographicsParserBase
	{
		public OneLinkPostOfficeCommaParser(ReflectionInterface ri, DataAccess da) : base(ri, da) { }

        /// <summary>
        /// Creates a AccurintRDemographics object and gathers other associated details from a OneLINK post office queue task.
        /// The screen MUST be on LP5FC for an open PDEM task before calling this method.
        /// </summary>
        public override QueueTask Parse()
		{
			QueueTask task = new QueueTask(RejectAction.Sources.POST_OFFICE, SystemCode.Sources.POST_OFFICE, DA);
			string accountNumber = "";
			string ssn = "";
			if (RI.CheckForText(1, 71, "QUEUE"))
			{
				ssn = RI.GetText(17, 70, 9); //Use the borrower's SSN.
				accountNumber = RI.GetText(17, 52, 12).Replace(" ", "");
			}
			else //1, 67, "WORKGROUP"
				ssn = RI.GetText(5, 70, 9); //Use the target SSN.

			string commentText = (RI.GetText(12, 11, 58) + RI.GetText(13, 11, 58) + RI.GetText(14, 11, 58) + RI.GetText(15, 11, 26)).Trim('_').Trim(); //Parse the address from the comment text.
			task.OriginalDemographicsText = commentText;
            if (task.OriginalDemographicsText.Contains("TEMPORARILY AWAY"))
            {
                string message = RejectAction.Sources.POST_OFFICE + " Return invalid address.";
                throw new QueueTaskException(message, ssn, RejectAction.Sources.POST_OFFICE, SystemCode.Sources.POST_OFFICE, task.OriginalDemographicsText);
            }
			if (commentText.Contains("RF@")) 
			{ 
				throw new ParseException(commentText, ssn, RejectAction.Sources.POST_OFFICE, SystemCode.Sources.POST_OFFICE, task.OriginalDemographicsText); 
			}
			task.OriginalDemographicsText = commentText.Substring(0, General.FindEndOfCommentText(commentText) + 1);
			string[] elements = task.OriginalDemographicsText.Split(',');
			if (elements.Length != 5) 
			{ 
				throw new ParseException(commentText, ssn, RejectAction.Sources.POST_OFFICE, SystemCode.Sources.POST_OFFICE, task.OriginalDemographicsText); 
			}
			AccurintRDemographics taskDemographics = new AccurintRDemographics();
			taskDemographics.Address1 = elements[0].Trim();
			taskDemographics.Address2 = elements[1].Trim();
			taskDemographics.City = elements[2].Trim();
			taskDemographics.State = elements[3].Trim();
			taskDemographics.ZipCode = elements[4].Trim();
			taskDemographics.AccountNumber = accountNumber;
			taskDemographics.Ssn = ssn;
			task.Demographics = taskDemographics;

			return task;
		}
	}
}

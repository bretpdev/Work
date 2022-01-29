using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace ACURINTR.DemographicsParsers
{
    class CompassCommaParser : DemographicsParserBase
    {
        public CompassCommaParser(ReflectionInterface ri, DataAccess da) : base(ri, da) { }
		/// <summary>
		/// Parse the address from the comment text.
		/// </summary>
		/// <returns>QueueTask Object including the demographic info </returns>
        public override QueueTask Parse()
		{
            string commentText = (RI.GetText(16, 2, 77) + RI.GetText(20, 2, 77)).Trim('_').Trim();
            string[] elements = commentText.Split(',');

            if (elements.Length != 10)
			{
				string message = "Incomplete demographics provided.";
				string ssn = RI.GetText(12, 2, 9);
				throw new ParseException(message, ssn, RejectAction.Sources.EMAIL, SystemCode.Sources.COMPASS, commentText);
			}

            QueueTask task;
			if (elements[9] == "EMAIL")
				task = new QueueTask(RejectAction.Sources.EMAIL, SystemCode.Sources.COMPASS_EMAIL, DA);
            else if (elements[9].Contains("ACCURINT"))
                task = new QueueTask(RejectAction.Sources.ACCURINT, SystemCode.Sources.ACCURINT, DA);
            else
                task = new QueueTask(RejectAction.Sources.AUTOPAY, SystemCode.Sources.AUTOPAY, DA);

            task.OriginalDemographicsText = commentText;
			AccurintRDemographics taskDemographics = new AccurintRDemographics();
            taskDemographics.Address1 = elements[0].Trim();
            taskDemographics.Address2 = elements[1].Trim();
            taskDemographics.City = elements[2].Trim();
            taskDemographics.State = elements[3].Trim();
            taskDemographics.ZipCode = elements[4].Trim();
            taskDemographics.Country = elements[5].Trim();
            taskDemographics.PrimaryPhone= elements[6].Trim();
            taskDemographics.AlternatePhone = elements[7].Trim();
            taskDemographics.EmailAddress = elements[8].Trim();
            taskDemographics.Ssn = RI.GetText(12, 2, 9);
			task.Demographics = taskDemographics;

            return task;
		}
	}
}


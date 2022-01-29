using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace ACURINTR.DemographicsParsers
{
    class CompassCommaParser : IDemographicsParser
    {
		private ReflectionInterface RI { get; set; }
		public CompassCommaParser(ReflectionInterface ri)
		{
			RI = ri;
		}

		/// <summary>
		/// Parse the address from the comment text.
		/// </summary>
		/// <returns>QueueTask Object including the demographic info </returns>
        public QueueTask Parse()
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
				task = new QueueTask(RejectAction.Sources.EMAIL, SystemCode.Sources.COMPASS_EMAIL);
            else if (elements[9].Contains("ACCURINT"))
                task = new QueueTask(RejectAction.Sources.ACCURINT, SystemCode.Sources.ACCURINT);
            else
                task = new QueueTask(RejectAction.Sources.AUTOPAY, SystemCode.Sources.AUTOPAY);

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


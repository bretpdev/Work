using System;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace ACURINTR.DemographicsParsers
{
	class CompassPdemParser : DemographicsParserBase
	{
		public CompassPdemParser(ReflectionInterface ri, DataAccess da) : base(ri, da) { }

		/// <summary>
		/// Creates a AccurintRDemographics object and gathers other associated details from a COMPASS Pending PDEM queue task.
		/// The screen MUST be on TXX1T for an open COMPASS Pending PDEM task before calling this method.
		/// </summary>
		public override QueueTask Parse()
		{
			QueueTask pdem = new QueueTask(RejectAction.Sources.COMPASS_PENDING_PDEM, SystemCode.Sources.COMPASS, DA);
			if (!RI.CheckForText(6, 12, "     ")) //The assumption is that if the PND ADR is filled in, this task is an address; otherwise, it's a phone number.
			{
				pdem.Demographics = ParseAddress();
				pdem.PdemSource = RI.GetText(11, 11, 2);
				if (!RI.CheckForText(6, 72, "  "))
					pdem.PdemVerificationDate = DateTime.Parse(RI.GetText(6, 72, 8));
				pdem.OriginalDemographicsText = string.Format("{0} {1}, {2}, {3} {4} {5}", pdem.Demographics.Address1, pdem.Demographics.Address2, pdem.Demographics.City, pdem.Demographics.State, pdem.Demographics.ZipCode, pdem.Demographics.Country);
			}
			else
			{
				pdem.Demographics = ParsePhone();
				pdem.PdemSource = RI.GetText(9, 67, 2);
				if (!RI.CheckForText(10, 68, "  "))
					pdem.PdemVerificationDate = DateTime.Parse(RI.GetText(10, 68, 8));
				pdem.OriginalDemographicsText = pdem.Demographics.PrimaryPhone;
			}
			pdem.Demographics.Ssn = RI.GetText(3, 10, 11).Replace("-", "");

			return pdem;
		}

		/// <summary>
		/// Grab the address components from the session
		/// </summary>
		/// <returns>AccurintRDemographics with address info populated</returns>
		private AccurintRDemographics ParseAddress()
		{
			AccurintRDemographics taskDemographics = new AccurintRDemographics();
			taskDemographics.Address1 = RI.GetText(6, 12, 30);
			taskDemographics.Address2 = RI.GetText(7, 2, 20);
			taskDemographics.City = RI.GetText(8, 38, 20);
			taskDemographics.State = RI.GetText(7, 38, 2);
			taskDemographics.ZipCode = RI.GetText(8, 63, 10);
			return taskDemographics;
		}

		/// <summary>
		/// Grab the phone components from the session
		/// </summary>
		/// <returns>AccurintRDemographics with the phone pieces populated</returns>
		private AccurintRDemographics ParsePhone()
		{
			AccurintRDemographics taskDemographics = new AccurintRDemographics();
			taskDemographics.PrimaryPhone = RI.GetText(9, 24, 3) + RI.GetText(9, 34, 3) + RI.GetText(9, 45, 4);
			taskDemographics.MblIndicator = RI.GetText(16, 20, 1);
			taskDemographics.ConsentIndicator = RI.GetText(16, 30, 1);
			return taskDemographics;
		}
	}
}

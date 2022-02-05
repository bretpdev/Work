using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace ACURINTR.DemographicsParsers
{
    class OneLinkPdemParser : DemographicsParserBase
    {
        public OneLinkPdemParser(ReflectionInterface ri, DataAccess da) : base(ri, da) { }

        /// <summary>
        /// Creates a AccurintRDemographics object and gathers other associated details from a OneLINK Pending PDEM queue task.
        /// The screen MUST be on LP5FC for an open PDEM task before calling this method.
        /// </summary>
        public override QueueTask Parse()
        {
            QueueTask pdem;
            if (RI.CheckForText(13, 34, "CREDIT BUREAU"))
                pdem = new QueueTask(RejectAction.Sources.ONELINK_PENDING_PDEM, SystemCode.Sources.CREDIT_BUREAU, DA);
            else
                pdem = new QueueTask(RejectAction.Sources.ONELINK_PENDING_PDEM, SystemCode.Sources.NON_CREDIT_BUREAU, DA);

            pdem.Demographics = new AccurintRDemographics();
            //Determine whether it's an address, home phone, or alternate phone.
            switch (RI.GetText(22, 3, 5))
            {
                case "49103":
                case "49104":
                case "49105":
                case "49106":
                case "49109":
                    pdem.Demographics = ParseAddress();
                    DateTime? addrDate = RI.GetText(14, 59, 8).ToDateNullable();
                    if (addrDate.HasValue)
                    {
                        pdem.PdemVerificationDate = addrDate.Value;
                    }

                    pdem.OriginalDemographicsText = string.Format("{0}, {1}, {2}, {3}, {4}, {5}", pdem.Demographics.Address1, pdem.Demographics.Address2, pdem.Demographics.City, pdem.Demographics.State, pdem.Demographics.ZipCode, pdem.Demographics.Country);
                    break;
                case "49125":
                    string received = RI.GetText(13, 34, 20);
                    string ssn = RI.GetText(2, 13, 9);
                    throw new QueueTaskException(received, ssn, "", "", "");
                case "49113":
                case "49114":
                    pdem.Demographics = ParseHomePhone();
                    pdem.PdemVerificationDate = DateTime.Parse(RI.GetText(17, 55, 8).ToDateFormat());
                    pdem.OriginalDemographicsText = pdem.Demographics.PrimaryPhone;
                    break;
                case "49117":
                case "49118":
                    pdem.Demographics = ParseAltPhone();
                    if (RI.CheckForText(18, 55, "MMDDYYYY"))
                        pdem.PdemVerificationDate = DateTime.Parse(RI.GetText(17, 55, 8).ToDateFormat());
                    else
                        pdem.PdemVerificationDate = DateTime.Parse(RI.GetText(18, 55, 8).ToDateFormat());

                    pdem.OriginalDemographicsText = pdem.Demographics.AlternatePhone;
                    break;
            }

            pdem.Demographics.Ssn = RI.GetText(2, 13, 9);
            pdem.PageNumber = int.Parse(RI.GetText(2, 73, 2));
            return pdem;
        }

        /// <summary>
        /// Parse valid addresses into AccurintRDemographics object
        /// </summary>
        /// <returns>AccurintRDemographics</returns>
        private AccurintRDemographics ParseAddress()
        {
            //Don't process if it's marked invalid.
            if (RI.CheckForText(14, 80, "N"))
            {
                throw new ParseException("", "", RejectAction.Sources.ONELINK_PENDING_PDEM, "", "");
            }

            AccurintRDemographics taskDemographics = new AccurintRDemographics();
            taskDemographics.Address1 = RI.GetText(14, 10, 35).Trim('_').Trim();
            taskDemographics.Address2 = RI.GetText(15, 10, 35).Trim('_').Trim();
            taskDemographics.City = RI.GetText(16, 8, 30).Trim('_').Trim();
            taskDemographics.State = RI.GetText(16, 45, 2).Trim('_').Trim();
            taskDemographics.ZipCode = RI.GetText(16, 52, 5).Trim('_').Trim();
            taskDemographics.Country = RI.GetText(15, 56, 25).Trim('_').Trim();
            return taskDemographics;
        }

        /// <summary>
        /// Parse valid phone into AccurintRDemographics object
        /// </summary>
        /// <returns>AccurintRDemographics</returns>
        private AccurintRDemographics ParseHomePhone()
        {
            //Don't process if it's marked invalid.
            if (RI.CheckForText(17, 46, "N"))
            {
                throw new ParseException("", "", RejectAction.Sources.ONELINK_PENDING_PDEM, "", "");
            }

            AccurintRDemographics taskDemographics = new AccurintRDemographics();
            taskDemographics.PrimaryPhone = RI.GetText(17, 13, 17);
            return taskDemographics;
        }

        private AccurintRDemographics ParseAltPhone()
        {
            //Don't process if it's marked invalid.
            if (RI.CheckForText(18, 46, "N"))
            {
                throw new ParseException("", "", RejectAction.Sources.ONELINK_PENDING_PDEM, "", "");
            }

            AccurintRDemographics taskDemographics = new AccurintRDemographics();
            taskDemographics.AlternatePhone = RI.GetText(18, 13, 17);
            return taskDemographics;
        }
    }
}

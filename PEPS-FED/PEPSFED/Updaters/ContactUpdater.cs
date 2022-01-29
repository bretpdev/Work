using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using System;

namespace PEPSFED
{
    class ContactUpdater 
    {
        private DataAccess DA { get; set; }
        public ContactUpdater(DataAccess da)
        {
            DA = da;
        }

        public void UpdateSystem(ReflectionInterface ri , ContactData data)
        {
            Console.WriteLine("About to process CONTACT_ID: {0}", data.RecordId);
            data.FormatProperties();
            ri.FastPath(string.Format("TX3Z/CTX0Y{0};000", data.OpeId));
            if (ri.CheckForText(23, 2, "30021 INSTITUTION NOT DEFINED ON INSTITUTION DEMOGRAPHICS"))
            {
                Program.PLR.AddNotification(string.Format("Unable to update Contact Info for the following peps line: {0}; Session Message:{1}", data.ToString(), ri.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                DA.UpdateContactProcessed(data.RecordId);
                return;

            }
            ri.PutText(16, 31, data.ContFirstName.SafeSubString(0, 10), true);
            ri.PutText(16, 56, data.ContLastName.SafeSubString(0, 20), true);
            string title = GetTitle(data.ContType, data);
            if(title.IsNullOrEmpty())
            {
                DA.UpdateContactProcessed(data.RecordId);
                return;
            }
            ri.PutText(17, 31,title, true);
            if (string.IsNullOrEmpty(data.ContForeignPhone))
                UpdateDomesticPhone(ri, data);
            else
                UpdateForeignPhone(ri, data);
            ri.Hit(Key.Enter);
            if (!ri.CheckForText(23, 2, "01005", "01004", "01003"))
                Program.PLR.AddNotification(string.Format("Unable to update Contact Info for the following peps line: {0}; Session Message:{1}", data.ToString(), ri.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);

            DA.UpdateContactProcessed(data.RecordId);
        }

        private static void UpdateDomesticPhone(ReflectionInterface ri, ContactData data)
        {
            //enter domestic phone
            ri.PutText(18, 20, data.ContAreaCode, true);
            ri.PutText(18, 26, data.ContExchange, true);
            ri.PutText(18, 30, data.ContExt, true);
            ri.PutText(18, 43, data.ContExt2, true);
            //enter domestic fax
            if (data.ContFax.Length > 0)
            {
                ri.PutText(19, 20, data.ContFax.SafeSubString(0, 3), true);
                ri.PutText(19, 26, data.ContFax.SafeSubString(3, 3), true);
                ri.PutText(19, 30, data.ContFax.SafeSubString(6, 4), true);
            }

            //blank out foreign phone and fax fields
            ri.PutText(20, 21, "", true);
            ri.PutText(20, 34, "", true);
            ri.PutText(20, 44, "", true);
            ri.PutText(20, 56, "", true);
            ri.PutText(21, 21, "", true);
            ri.PutText(21, 34, "", true);
            ri.PutText(21, 44, "", true);
            ri.PutText(21, 56, "", true);
        }

        private static void UpdateForeignPhone(ReflectionInterface ri, ContactData data)
        {
            //determine international code
            string country = ri.GetText(15, 49, 15);
            string internationalCode = (country == "CANADA" || country == "CARIBBEAN ISLAN" ? "809" : "011");
            //enter foreign phone
            ri.PutText(20, 21, internationalCode);
            ri.PutText(20, 34, data.ContForeignPhone.SafeSubString(0, 3), true);
            ri.PutText(20, 44, data.ContForeignPhone.SafeSubString(3, 4), true);
            ri.PutText(20, 56, data.ContForeignPhone.SafeSubString(7, 7), true);
            //enter foreign fax
            if (data.ContFax.Length > 0)
            {
                ri.PutText(21, 21, internationalCode);
                ri.PutText(21, 34, data.ContFax.SafeSubString(0, 3), true);
                ri.PutText(21, 44, data.ContFax.SafeSubString(3, 4), true);
                ri.PutText(21, 56, data.ContFax.SafeSubString(7, 7), true);
            }

            //blank out foreign phone and fax fields
            ri.PutText(18, 20, "", true);
            ri.PutText(18, 26, "", true);
            ri.PutText(18, 30, "", true);
            ri.PutText(18, 43, "", true);
            ri.PutText(19, 20, "", true);
            ri.PutText(19, 26, "", true);
            ri.PutText(19, 30, "", true);
        }

        private string GetTitle(string contactType, ContactData data)
        {
            switch (contactType)
            {
                case "03":
                    return "Financial Aid Admin";
                case "04":
                    return "CFO";
                case "34":
                    return "Pres/CEO/Chancellor";
                default:
                    Program.PLR.AddNotification(string.Format("Unable to parse the title: {1} for the following peps line: {0};", data.ToString(),contactType), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return string.Empty;
            }
        }
    }
}

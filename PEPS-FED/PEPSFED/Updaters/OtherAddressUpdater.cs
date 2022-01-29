using System;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace PEPSFED
{
    class OtherAddressUpdater
    {
        private DataAccess DA { get; set; }
        public OtherAddressUpdater(DataAccess da)
        {
            DA = da;
        }

        public void UpdateSystem(ReflectionInterface ri, OtherAddressData data)
        {
            Console.WriteLine("About to process OTHERADD_ID: {0}", data.RecordId);
            data.FormatProperties();
            if (data.Line1Adr.Length > 29)
            {
                Program.PLR.AddNotification(string.Format("Unable to update school address, Address 1 is > 29 charcters.  peps line: {0}", data.ToString()), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                DA.UpdateOtherAddressProcessed(data.RecordId);
                return;
            }
            if (data.Line2Adr.Length > 29)
            {
                Program.PLR.AddNotification(string.Format("Unable to update school address, Address 2 is > 29 charcters.  peps line: {0}", data.ToString()), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                DA.UpdateOtherAddressProcessed(data.RecordId);
                return;
            }
            string departmentCode;
            switch (data.AddressType)
            {
                case "01":
                    departmentCode = "000"; //GENERAL INFORMATION
                    break;
                case "03":
                    departmentCode = "004"; //FINALCIAL AID
                    break;
                default:
                    //Skip any other address types.
                    DA.UpdateOtherAddressProcessed(data.RecordId);
                    return;
            }
            ri.FastPath(string.Format("TX3Z/CTX0Y{0};{1}", data.OpeId, departmentCode));
            if (ri.CheckForText(23, 2, "30021 INSTITUTION NOT DEFINED ON INSTITUTION DEMOGRAPHICS"))
            {
                Program.PLR.AddNotification(string.Format("Unable to update School Information for the following peps line: {0}; Session Message: {1}", data.ToString(), ri.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                DA.UpdateOtherAddressProcessed(data.RecordId);
                return;
            }
            UpdateAddress(ri, data);
            if (string.IsNullOrEmpty(data.OtherForeignPhone))
                UpdateDomesticPhone(ri, data);
            else
                UpdateForeignPhone(ri, data);
            ri.PutText(22, 10, "Y");
            ri.PutText(22, 31, DateTime.Today.ToString("MMddyy"));
            ri.Hit(Key.Enter);
            if (!ri.CheckForText(23, 2, "01005", "01004", "01003"))
            {
                Program.PLR.AddNotification(string.Format("Unable to update School Information for the following peps line: {0}; Session Message: {1}", data.ToString(), ri.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                DA.UpdateOtherAddressProcessed(data.RecordId);
                return;
            }

            DA.UpdateOtherAddressProcessed(data.RecordId);
        }

        private void UpdateAddress(ReflectionInterface ri, OtherAddressData data)
        {
            if (ri.CheckForText(23, 2, "01019 ENTERED KEY NOT FOUND"))
                ri.PutText(1, 4, "A", Key.Enter);
            ri.PutText(11, 23, data.Line1Adr, true);
            ri.PutText(12, 23, data.Line2Adr, true);
            ri.PutText(14, 13, data.City.SafeSubString(0, 19), true);
            ri.PutText(14, 53, data.State, true);
            ri.PutText(14, 69, data.Zip, true);
            ri.PutText(15, 21, data.ForeignProvinceName.SafeSubString(0, 15), true);
            if (data.Country != "USA" && data.Country != "U.S.A." && data.Country != "United States" && data.Country != "United States of America")
                ri.PutText(15, 49, data.Country, true);
            if (!string.IsNullOrEmpty(data.FscContFirstName))
                ri.PutText(16, 31, data.FscContFirstName.SafeSubString(0, 10), true);
            if (!string.IsNullOrEmpty(data.FscContLastName))
                ri.PutText(16, 56, data.FscContLastName.SafeSubString(0, 20), true);
        }

        private void UpdateForeignPhone(ReflectionInterface ri, OtherAddressData data)
        {
            //determine international code
            string country = ri.GetText(15, 49, 15);
            string internationalCode = (country == "CANADA" || country == "CARIBBEAN ISLAN" ? "809" : "011");
            //enter foreign phone
            if (!string.IsNullOrEmpty(data.OtherAreaCode))
                ri.PutText(20, 21, internationalCode);
            if (data.OtherForeignPhone.Length > 0)
            {
                ri.PutText(20, 34, data.OtherForeignPhone.SafeSubString(0, 3), true);
                ri.PutText(20, 44, data.OtherForeignPhone.SafeSubString(3, 4), true);
                ri.PutText(20, 56, data.OtherForeignPhone.SafeSubString(7, 7), true);
            }
            //enter foreign fax
            if (data.OtherFax.Length > 0)
            {
                ri.PutText(21, 21, internationalCode);
                ri.PutText(21, 34, data.OtherFax.SafeSubString(0, 3), true);
                ri.PutText(21, 44, data.OtherFax.SafeSubString(3, 4), true);
                ri.PutText(21, 56, data.OtherFax.SafeSubString(7, 7), true);
            }

            //blank out domestic phone and fax fields
            ri.PutText(18, 20, "", true);
            ri.PutText(18, 26, "", true);
            ri.PutText(18, 30, "", true);
            ri.PutText(18, 43, "", true);
            ri.PutText(19, 20, "", true);
            ri.PutText(19, 26, "", true);
            ri.PutText(19, 30, "", true);
        }

        private void UpdateDomesticPhone(ReflectionInterface ri, OtherAddressData data)
        {
            //enter domestic phone
            if (!string.IsNullOrEmpty(data.OtherAreaCode))
                ri.PutText(18, 20, data.OtherAreaCode, true);
            if (!string.IsNullOrEmpty(data.OtherExchange))
                ri.PutText(18, 26, data.OtherExchange, true);
            if (!string.IsNullOrEmpty(data.OtherExt))
                ri.PutText(18, 30, data.OtherExt, true);
            if (!string.IsNullOrEmpty(data.OtherExt2))
                ri.PutText(18, 43, data.OtherExt2, true);
            //enter domestic fax
            if (data.OtherFax.Length > 0)
            {
                ri.PutText(19, 20, data.OtherFax.SafeSubString(0, 3), true);
                ri.PutText(19, 26, data.OtherFax.SafeSubString(3, 3), true);
                ri.PutText(19, 30, data.OtherFax.SafeSubString(6, 4), true);
            }

            //blank out foreign phone and fax fields
            ri.PutText(20, 21, "", true);
            ri.PutText(20, 34, "", true);
            ri.PutText(20, 44, "", true);
            ri.PutText(20, 56, "", true);
            ri.PutText(20, 75, "", true);
            ri.PutText(21, 21, "", true);
            ri.PutText(21, 34, "", true);
            ri.PutText(21, 44, "", true);
            ri.PutText(21, 56, "", true);
        }
    }
}

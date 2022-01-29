using System;
using System.Collections.Generic;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace PEPSFED
{
    class DetailUpdater
    {

        private DataAccess DA { get; set; }
        public DetailUpdater(DataAccess da)
        {
            DA = da;
        }

        public void UpdateSystem(ReflectionInterface ri, DetailData data)
        {
            Console.WriteLine("About to process DETAIL_ID: {0}", data.RecordId);
            data.FormatProperties();
            if (data.Line1Adr.Length > 29)
            {
                Program.PLR.AddNotification(string.Format("Unable to update school address, Address 1 is > 29 charcters.  peps line: {0}", data.ToString()), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                DA.UpdateDetailProcessed(data.RecordId);
                return;
            }
            if (data.Line2Adr.Length > 29)
            {
                Program.PLR.AddNotification(string.Format("Unable to update school address, Address 2 is > 29 charcters.  peps line: {0}", data.ToString()), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                DA.UpdateDetailProcessed(data.RecordId);
                return;
            }
            UpdateSchoolInformation(ri, data);
            if (data.EligibilityActionDate != default(DateTime))
                UpdateLoanPrograms(ri, data);

            DA.UpdateDetailProcessed(data.RecordId);
        }

        private string GetApprovalStatus(string actionCode, DetailData data)
        {
            switch (actionCode)
            {
                case "01":
                case "02":
                case "03":
                case "23":
                case "53":
                    return "D";
                case "04":
                case "05":
                case "06":
                case "50":
                case "51":
                case "52":
                case "56":
                    return "E";
                case "07":
                case "08":
                case "54":
                case "55":
                    return "I";
                default:
                    Program.PLR.AddNotification(string.Format("Invalid Action Code received in the following peps line: {0}", data.ToString()), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    DA.UpdateDetailProcessed(data.RecordId);
                    return string.Empty;
            }
        }

        private string GetSchoolType(string programLength, string schoolType, DetailData data)
        {
            switch (programLength)
            {
                case "00":
                case "02":
                case "03":
                case "11":
                    return "07";
                case "01":
                case "07":
                case "08":
                case "09":
                case "10":
                    return "12";
                case "04":
                case "05":
                case "12":
                    switch (schoolType)
                    {
                        case "1":
                        case "6":
                            return "05";
                        case "2":
                        case "5":
                            return "04";
                        case "3":
                        case "7":
                            return "AP";
                        default:
                            Program.PLR.AddNotification(string.Format("Invalid Program Length received in the following peps line: {0}", data.ToString()), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                            DA.UpdateDetailProcessed(data.RecordId);
                            return string.Empty;
                    }
                case "06":
                    switch (schoolType)
                    {
                        case "1":
                        case "5":
                            return "02";
                        case "2":
                        case "6":
                            return "01";
                        case "3":
                        case "7":
                            return "AP";
                        default:
                            Program.PLR.AddNotification(string.Format("Invalid Program Length received in the following peps line: {0}", data.ToString()), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                            DA.UpdateDetailProcessed(data.RecordId);
                            return string.Empty;
                    }
                default:
                    Program.PLR.AddNotification(string.Format("Invalid Program Length received in the following peps line: {0}", data.ToString()), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    DA.UpdateDetailProcessed(data.RecordId);
                    return string.Empty;
            }
        }

        private string GetApprovalReason(string reasonCode, DetailData data)
        {
            switch (reasonCode)
            {
                case "16":
                case "17":
                case "18":
                case "19":
                case "20":
                case "28":
                case "29":
                case "31":
                case "32":
                case "35":
                case "37":
                    return "B";
                case "01":
                case "02":
                case "03":
                case "06":
                case "08":
                case "09":
                case "10":
                case "11":
                case "13":
                case "15":
                case "23":
                case "36":
                case "38":
                case "39":
                case "50":
                case "51":
                case "52":
                case "61":
                    return "C";
                case "04":
                    return "E";
                case "05":
                    return "F";
                default:
                    Program.PLR.AddNotification(string.Format("Invalid Approval Reason received in the following peps line: {0}", data.ToString()), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    DA.UpdateDetailProcessed(data.RecordId);
                    return string.Empty;
            }
        }

        private void UpdateLoanPrograms(ReflectionInterface ri, DetailData data)
        {
            ri.FastPath(string.Format("TX3Z/CTX13{0}", data.OpeId));
            if (ri.CheckForText(23, 2, "01443 NO GUARANTORS ASSOCIATED WITH THIS SCHOOL"))
            {
                List<string> loanPrograms = new List<string>();
                loanPrograms.Add("DLPLGB");
                loanPrograms.Add("DLPLUS");
                loanPrograms.Add("DLSTFD");
                loanPrograms.Add("DLUNST");
                loanPrograms.Add("PLUS");
                loanPrograms.Add("PLUSGB");
                loanPrograms.Add("STFFRD");
                loanPrograms.Add("UNSTFD");

                foreach (string lp in loanPrograms)
                {
                    ri.FastPath(string.Format("TX3Z/ATX13{0};{1};000502", data.OpeId, lp));
                    ri.PutText(8, 26, data.OpeId);
                    string approvalStatus = GetApprovalStatus(data.ActnCd, data);
                    if (approvalStatus.IsNullOrEmpty())
                        return;
                    ri.PutText(10, 12, approvalStatus);
                    ri.PutText(11, 15, data.EligibilityActionDate.Value.ToString("MMddyy"));
                    if (ri.CheckForText(10, 12, "I"))
                    {
                        string approvalReason = GetApprovalReason(data.ActnReasonCd, data);
                        if (approvalReason.IsNullOrEmpty())
                            return;
                        ri.PutText(12, 16, GetApprovalReason(data.ActnReasonCd, data));
                    }
                    ri.Hit(Key.Enter);
                }
            }
            else
            {
                for (int row = 8; !ri.CheckForText(row, 2, "  "); row++)
                {
                    ri.PutText(21, 15, ri.GetText(row, 2, 2), Key.Enter, true);
                    string approvalReason = GetApprovalReason(data.ActnReasonCd, data);
                    if (approvalReason.IsNullOrEmpty())
                        return;
                    if (ri.CheckForText(10, 12, "I") && !ri.CheckForText(12, 16, "_") && approvalReason != "I")
                        ri.PutText(12, 16, "", true);
                    ri.PutText(10, 12, GetApprovalStatus(data.ActnCd, data));
                    if (data.EligibilityActionDate != default(DateTime))
                        ri.PutText(11, 15, data.EligibilityActionDate.Value.ToString("MMddyy"));
                    if (ri.CheckForText(10, 12, "I"))
                        ri.PutText(12, 16, GetApprovalReason(data.ActnReasonCd, data), true);
                    ri.Hit(Key.Enter);
                    if (!ri.CheckForText(23, 2, "01005", "01004", "01003"))
                        ri.Hit(Key.F12);
                }
            }
        }

        private void UpdateSchoolInformation(ReflectionInterface ri, DetailData data)
        {
            ri.FastPath(string.Format("TX3Z/CTX0Y{0};000", data.OpeId));
            if (ri.CheckForText(23, 2, "30021 INSTITUTION NOT DEFINED ON INSTITUTION DEMOGRAPHICS"))
            {
                if (data.CloseDate == default(DateTime))
                {
                    Program.PLR.AddNotification(string.Format("Unable to update School Information for the following peps line: {0}; Session Message: {1}", data.ToString(), ri.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    DA.UpdateDetailProcessed(data.RecordId);
                    return;
                }
                else
                    return;
            }

            ri.PutText(6, 19, data.SchoolName.SafeSubString(0, 40), true);
            ri.PutText(7, 19, data.SchoolName.SafeSubString(0, 20), true);
            ri.PutText(11, 23, data.Line1Adr, true);
            ri.PutText(12, 23, data.Line2Adr, true);
            ri.PutText(14, 13, data.City.SafeSubString(0, 19), true);
            ri.PutText(14, 53, data.State, true);
            ri.PutText(14, 69, data.Zip, true);
            ri.PutText(15, 21, data.ForeignProvinceName.SafeSubString(0, 15), true);
            if (data.Country != "USA" && data.Country != "U.S.A." && data.Country != "United States" && data.Country != "United States of America")
            {
                ri.PutText(15, 49, data.Country, true);
                ri.PutText(18, 20, "", true);
                ri.PutText(18, 26, "", true);
                ri.PutText(18, 30, "", true);
                ri.PutText(18, 43, "", true);
                ri.PutText(19, 20, "", true);
                ri.PutText(19, 26, "", true);
                ri.PutText(19, 30, "", true);
            }
            ri.PutText(22, 10, "Y"); //VALID
            ri.PutText(22, 31, DateTime.Now.ToString("MMddyy")); //DATE VEriFIED
            ri.Hit(Key.Enter);
            if (!ri.CheckForText(23, 2, "01005", "01003"))
            {
                Program.PLR.AddNotification(string.Format("Unable to update School Information for the following peps line: {0}; Session Message: {1}", data.ToString(), ri.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                DA.UpdateDetailProcessed(data.RecordId);
                return;
            }

            if (data.CloseDate.HasValue && data.CloseDate != default(DateTime))
            {
                ri.Hit(Key.F10);
                string schoolType = GetSchoolType(data.PgmLength, data.SchType, data);
                if (schoolType.IsNullOrEmpty())
                    return;
                ri.PutText(6, 16, schoolType);
                ri.PutText(13, 19, "C"); //CURRENT STATUS
                ri.PutText(14, 27, data.CloseDate.Value.ToString("MMddyy")); //DATE OF CURRENT STATUS
                ri.Hit(Key.Enter);
                if (!ri.CheckForText(23, 2, "01005", "01003"))
                {
                    Program.PLR.AddNotification(string.Format("Unable to update School Information for the following peps line: {0}; Session Message: {1}", data.ToString(), ri.Message), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    DA.UpdateDetailProcessed(data.RecordId);
                    return;
                }
            }
        }
    }
}

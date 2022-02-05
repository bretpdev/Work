using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static System.Console;
using static Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace RTRNMAILOL
{
    public class DemographicsProcessor
    {
        public enum DemographicsUpdateStatus
        {
            None,
            Failed,
            Skipped,
            Updated
        }
        private ReflectionInterface RI { get; set; }
        private DataAccess DA { get; set; }

        public DemographicsProcessor(ReflectionInterface ri, DataAccess da)
        {
            RI = ri;
            DA = da;
        }

        public List<string> GetPersonTypes(BarcodeData data)
        {
            List<string> personTypes = new List<string>();
            if (data.IsReference)
                return personTypes;

            RI.FastPath($"LP22I{(data.AccountIdentifier.Length == 9 ? "" : ";;;;;;")}{data.AccountIdentifier}");
            if (!RI.CheckForText(1, 62, "PERSON DEMOGRAPHICS"))
                return personTypes;

            RI.Hit(F10);
            if (RI.AltMessageCode == "44024")
                return personTypes;

            string borrowerName = RI.GetText(3, 40, 41);
            while (RI.AltMessageCode != "46004")
            {
                for (int row = 7; row < 19; row++)
                {
                    string personType = RI.GetText(row, 7, 1);
                    if (!personTypes.Any(p => p.ToString() == personType))
                    {
                        if (RI.CheckForText(row, 11, borrowerName))
                            personTypes.Add(personType);
                    }
                }
                RI.Hit(F8);
            }
            return personTypes;
        }

        public bool AddComment(BarcodeData data, string comment, List<string> personTypes)
        {
            if (personTypes.Contains("B") || personTypes.Contains("R") || personTypes.Count() == 0)
                return AddBorrowerComment(data, comment);
            if (personTypes.Contains("E"))
                return AddEndorserComment(data, comment);
            return false;
        }

        /// <summary>
        /// Adds S4LRM comment to borrowers account
        /// </summary>
        private bool AddBorrowerComment(BarcodeData data, string comment)
        {
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = data.Ssn,
                ActivityContact = "90",
                ActivityType = "LT",
                Arc = "S4LRM",
                ArcTypeSelected = ArcData.ArcType.OneLINK,
                Comment = comment,
                IsEndorser = false,
                IsReference = false,
                ScriptId = Program.ScriptId
            };
            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"Unable to add OneLINK S4LRM ARC for borrower {data.AccountIdentifier}";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                WriteLine(message);
            }
            WriteLine($"Arc: {arc.Arc} was added to barcode data id: {data.BarcodeDataId}");
            DA.InsertArcAddId(data.BarcodeDataId, result.ArcAddProcessingId);
            return result.ArcAdded;
        }

        /// <summary>
        /// Adds SELRM comment to reference account
        /// </summary>
        private bool AddEndorserComment(BarcodeData data, string comment)
        {
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = data.Ssn,
                ActivityContact = "90",
                ActivityType = "LT",
                Arc = "SELRM",
                ArcTypeSelected = ArcData.ArcType.OneLINK,
                Comment = comment,
                IsReference = false,
                IsEndorser = true,
                ScriptId = Program.ScriptId
            };
            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"Unable to add OneLINK SELRM ARC for endorser {data.AccountIdentifier} to borrower: {data.Ssn}";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                WriteLine(message);
            }
            WriteLine($"Arc: {arc.Arc} was added to barcode data id: {data.BarcodeDataId}");
            DA.InsertArcAddId(data.BarcodeDataId, result.ArcAddProcessingId);
            return result.ArcAdded;
        }

        /// <summary>
        /// Invalidates borrower address
        /// </summary>
        public DemographicsUpdateStatus InvalidateBorrower(BarcodeData data)
        {
            RI.FastPath("LP22C" + data.Ssn);
            DateTime? addressEffectiveDate;
            try
            {
                addressEffectiveDate = RI.GetText(10, 72, 8).ToDateNullable();
            }
            catch (Exception)
            {
                string message = $"Invalid address effective date in LP22C for borrower: {data.AccountIdentifier}";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return DemographicsUpdateStatus.Failed;
            }
            if (addressEffectiveDate.HasValue && addressEffectiveDate.Value >= data.CreateDate)
                return DemographicsUpdateStatus.Skipped;
            else
            {
                RI.PutText(3, 9, "2");
                RI.PutText(10, 57, "N");
                RI.Hit(F6);
                if (RI.AltMessageCode.IsIn("49000", "40639"))
                {
                    WriteLine($"Invalidated address for barcode data id: {data.BarcodeDataId}");
                    return DemographicsUpdateStatus.Updated;
                }
                else
                {
                    string message = $"There was an error invalidating the address for borrower: {data.AccountIdentifier}; Session Message: {RI.AltMessage}";
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    WriteLine(message);
                    return DemographicsUpdateStatus.Failed;
                }
            }
        }

        /// <summary>
        /// Updates the borrower address in LP22
        /// </summary>
        public int UpdateBorrowerAddress(BarcodeData data)
        {
            RI.FastPath("LP22C" + data.Ssn);
            if (!RI.CheckForText(1, 60, "PERSON"))
            {
                RI.PutText(3, 9, "2");
                RI.PutText(10, 9, data.Address1, true);
                RI.PutText(11, 9, data.Address2, true);
                RI.PutText(12, 9, data.City, true);
                RI.PutText(12, 60, data.Zip, true);
                RI.PutText(12, 52, data.State);
                RI.PutText(10, 57, "Y", F6);

                if (!RI.AltMessageCode.IsIn("49000", "40639"))
                {
                    string message = $"There was an error updating the borrowers address for Account: {data.AccountIdentifier}. Please review the account and update the address manually.";
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    WriteLine(message);
                    return Process.ERROR;
                }
                WriteLine($"Forwarding address updated for barcode data id: {data.BarcodeDataId}");
                return Process.SUCCESS;
            }
            return Process.ERROR;
        }

        /// <summary>
        /// Invalidates reference address
        /// </summary>
        public DemographicsUpdateStatus InvalidateReference(BarcodeData data)
        {
            RI.FastPath("LP2CC;" + data.AccountIdentifier);
            DateTime? addressEffectiveDate;
            try
            {
                addressEffectiveDate = RI.GetText(8, 55, 8).ToDateNullable();
            }
            catch (Exception)
            {
                string message = $"Invalid address effective date in LP22C for reference: {data.AccountIdentifier}";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return DemographicsUpdateStatus.Failed;
            }
            if (addressEffectiveDate.HasValue && addressEffectiveDate.Value >= data.CreateDate)
                return DemographicsUpdateStatus.Skipped;
            else
            {
                RI.PutText(8, 53, "N");
                RI.PutText(8, 55, DateTime.Now.ToString("MMddyyyy"));
                RI.Hit(Enter);
                RI.Hit(F6);
                if (RI.AltMessageCode.IsIn("49000", "40639"))
                {
                    WriteLine($"Invalidated address for barcode data id: {data.BarcodeDataId}");
                    return DemographicsUpdateStatus.Updated;
                }
                else
                {
                    string message = $"There was an error invalidating the address for reference: {data.AccountIdentifier}; Session Message: {RI.AltMessage}";
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    WriteLine(message);
                    return DemographicsUpdateStatus.Failed;
                }
            }
        }

        /// <summary>
        /// Updates the reference address in LP2C
        /// </summary>
        public int UpdateReferenceAddress(BarcodeData data)
        {
            RI.FastPath($"LP2CC;{data.AccountIdentifier}");
            if (RI.CheckForText(1, 59, "REFERENCE DEMOGRAPHICS"))
            {
                RI.PutText(8, 9, data.Address1, true);
                RI.PutText(9, 9, data.Address2, true);
                RI.PutText(10, 9, data.City, true);
                RI.PutText(10, 52, data.State);
                RI.PutText(10, 60, data.Zip, true);
                RI.PutText(8, 53, "Y");
                RI.PutText(8, 55, DateTime.Now.ToString("MMddyyyy"), Enter);
                RI.Hit(F6);

                if (!RI.AltMessageCode.IsIn("49000", "40639"))
                {
                    string message = $"There was an error updating the references address for Reference: {data.AccountIdentifier}. Please review the account and update the address manually.";
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    WriteLine(message);
                    return Process.ERROR;
                }
                WriteLine($"Forwarding address updated for barcode data id: {data.BarcodeDataId}");
                return Process.SUCCESS;
            }
            return Process.ERROR;
        }

        public void MarkDefaultInvalid(string ssn)
        {
            RI.FastPath("LC05C;;;;" + ssn);
            if (RI.CheckForText(1, 76, "RECAP"))
                RI.PutText(21, 13, "1", Enter);
            if (RI.CheckForText(1, 74, "DISPLAY"))
            {
                //Target screen. Mark all records.
                while (RI.AltMessageCode != "46004")
                {
                    //Go to page 3.
                    RI.Hit(F10, 2);
                    //Mark the DFLT1 INVALID field.
                    RI.PutText(7, 17, "Y", Enter);
                    //Go to page 1 and move to the next record.
                    RI.Hit(F10);
                    RI.Hit(F8);
                }
            }
        }
    }
}
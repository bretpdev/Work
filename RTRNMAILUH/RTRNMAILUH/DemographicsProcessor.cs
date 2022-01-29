using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static System.Console;
using static Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace RTRNMAILUH
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
        public DataAccess DA { get; set; }

        public DemographicsProcessor(ReflectionInterface ri, DataAccess da)
        {
            RI = ri;
            DA = da;
        }

        public bool AddComment(BarcodeData data, string comment, IEnumerable<string> personTypes)
        {
            //Add the borrower arc last so the last update to the database is the borrower ARC
            bool added = false;
            if (data.IsReference)
                added = AddReferenceArc(data, comment);
            if (personTypes.Contains("E") && data.BorrowerSsn.IsPopulated())
                added = AddEndorserArc(data, comment);
            if (personTypes.Contains("B"))
                added = AddBorrowerArc(data, comment);
            return added;
        }

        /// <summary>
        /// Adds arc for reference account
        /// </summary>
        private bool AddReferenceArc(BarcodeData data, string comment)
        {
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = data.BorrowerSsn,
                Arc = "S4LRM",
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Comment = comment += $" Reference: {data.AccountIdentifier}",
                IsEndorser = false,
                IsReference = true,
                RegardsCode = "R",
                RegardsTo = data.AccountIdentifier,
                ScriptId = Program.ScriptId
            };
            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"Unable to add compass S4LRM arc for borrower {data.BorrowerSsn} for Reference: {data.AccountIdentifier}";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                WriteLine(message);
            }
            WriteLine($"Arc: {arc.Arc} was added to barcode data id: {data.BarcodeDataId}");
            DA.InsertArcAddId(data.BarcodeDataId, result.ArcAddProcessingId);
            return result.ArcAdded;
        }

        /// <summary>
        /// Adds an ARC to the borrowers account
        /// </summary>
        private bool AddBorrowerArc(BarcodeData data, string comment)
        {
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = data.Ssn,
                Arc = "S4LRM",
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Comment = comment,
                IsEndorser = false,
                IsReference = false,
                ScriptId = Program.ScriptId
            };
            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"Unable to add compass S4LRM ARC for borrower {data.Ssn}";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                WriteLine(message);
            }
            WriteLine($"Arc: {arc.Arc} was added to barcode data id: {data.BarcodeDataId}");
            DA.InsertArcAddId(data.BarcodeDataId, result.ArcAddProcessingId);
            return result.ArcAdded;
        }

        /// <summary>
        /// Adds an ARC to the endorsers account
        /// </summary>
        private bool AddEndorserArc(BarcodeData data, string comment)
        {
            string endSsn = DA.GetDemos(data.AccountIdentifier).Ssn;
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = data.BorrowerSsn,
                Arc = "SELRM",
                ArcTypeSelected = ArcData.ArcType.Atd22ByLoan,
                Comment = comment,
                IsEndorser = true,
                IsReference = false,
                LoanSequences = data.LoanSequences,
                RecipientId = endSsn,
                RegardsTo = endSsn,
                RegardsCode = "E",
                ScriptId = Program.ScriptId
            };
            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                string message = $"Unable to add compass SELRM ARC for endorser {data.AccountIdentifier}";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                WriteLine(message);
            }
            WriteLine($"Arc: {arc.Arc} was added to barcode data id: {data.BarcodeDataId}");
            DA.InsertArcAddId(data.BarcodeDataId, result.ArcAddProcessingId);
            return result.ArcAdded;
        }

        public List<string> GetPersonTypes(BarcodeData data)
        {
            data.LoanSequences = new List<int>();
            List<string> personType = new List<string>();
            foreach (string type in new string[] { "B", "E" })
            {
                RI.FastPath($"TX3Z/ITX1J{type}{data.Ssn}");
                if (RI.CheckForText(1, 71, "TXX1R"))
                {
                    if (!personType.Any(p => p == type))
                        personType.Add(type);
                    if (type == "E")
                    {
                        data.BorrowerSsn = RI.GetText(7, 11, 11).Replace(" ", "");
                        RI.Hit(F2);
                        RI.Hit(F4);
                        for (int row = 10; RI.MessageCode != "90007"; row++)
                        {
                            if (row > 21 || RI.CheckForText(row, 2, "  "))
                            {
                                row = 9;
                                RI.Hit(F8);
                                continue;
                            }

                            if (RI.CheckForText(row, 5, "E") && RI.CheckForText(row, 13, data.Ssn))
                                data.LoanSequences.Add(RI.GetText(row, 9, 3).ToInt());
                        }

                    }
                }
            }
            return personType;
        }

        public DemographicsUpdateStatus InvalidateBorrower(BarcodeData data)
        {
            RI.FastPath($"TX3Z/CTX1J;{data.Ssn}");
            if (!RI.CheckForText(1, 71, "TXX1R"))
                return DemographicsUpdateStatus.None;
            DateTime? addressLastVerified;
            try
            {
                addressLastVerified = RI.GetText(10, 32, 8).ToDateNullable();
            }
            catch (Exception)
            {
                string message = $"Invalid address last verified in CTX1J for borrower: {data.AccountIdentifier}";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                WriteLine(message);
                return DemographicsUpdateStatus.Failed;
            }
            if (addressLastVerified.HasValue && addressLastVerified.Value >= data.CreateDate)
                return DemographicsUpdateStatus.Skipped;
            else
            {
                RI.Hit(F6);
                RI.Hit(F6);
                if (RI.CheckForText(8, 5, "SOURCE: CODE"))
                    RI.PutText(8, 18, "25");
                if (!RI.CheckForText(9, 18, "_") && !RI.CheckForText(1, 9, "E"))
                    RI.PutText(9, 18, "", true);
                RI.PutText(11, 55, "N");
                RI.PutText(10, 32, $"{DateTime.Now:MMddyy}", Enter);
                if (RI.CheckForText(23, 2, "01096", "01097", "04323", "01022", "01299", "01003", "03417", "01005", "01100", "01102"))
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

        public DemographicsUpdateStatus InvalidateReference(BarcodeData data)
        {
            RI.FastPath($"TX3Z/ITX1J;{data.AccountIdentifier}");
            if (!RI.CheckForText(1, 71, "TXX1R"))
                return DemographicsUpdateStatus.None;

            //Get the reference's name as it would appear on the borrower relations screen.
            bool invalidatedAnAddress = false;

            DateTime? addressEffectiveDate;
            try
            {
                //Address Last Verified
                addressEffectiveDate = RI.GetText(10, 32, 8).ToDateNullable();
            }
            catch (Exception ex)
            {
                string message = $"There was an error getting the address effective date for reference: {data.AccountIdentifier}";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                WriteLine(message);
                return DemographicsUpdateStatus.Failed;
            }
            if (addressEffectiveDate.HasValue && addressEffectiveDate.Value < data.CreateDate)
            {
                //Switch to change mode and invalidate the address.
                RI.PutText(1, 4, "C", Enter);
                RI.Hit(F6, 2);
                if (RI.CheckForText(8, 5, "SOURCE: CODE"))
                    RI.PutText(8, 18, "25");
                RI.PutText(11, 55, "N");
                RI.PutText(10, 32, $"{DateTime.Now:MMddyy}", Enter);
                if (RI.CheckForText(23, 2, "01096", "01097", "04323", "01022", "01299", "01003", "03417", "01005", "01100", "01102"))
                {
                    WriteLine($"Invalidated address for barcode data id: {data.BarcodeDataId}");
                    return DemographicsUpdateStatus.Updated;
                }
                else
                {
                    string message = $"There was an error invalidating the address for reference: {data.AccountIdentifier}; Session Message: {RI.Message}";
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    WriteLine(message);
                    return DemographicsUpdateStatus.Failed;
                }
            }
            return (invalidatedAnAddress ? DemographicsUpdateStatus.Updated : DemographicsUpdateStatus.Skipped);
        }
    }
}
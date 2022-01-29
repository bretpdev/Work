using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using static System.Console;

namespace FINALREV
{
    public class References
    {
        public ReflectionInterface RI { get; set; }
        public BorrowerRecord Borrower { get; set; }
        public DataAccess DA { get; set; }
        public List<RefLetterData> LettersData { get; set; }
        public bool LetterSent { get; set; }
        public string RefAccount { get; set; }


        public References(ReflectionInterface ri, BorrowerRecord bor, DataAccess da)
        {
            RI = ri;
            Borrower = bor;
            DA = da;
            LettersData = new List<RefLetterData>();
        }

        public void ReviewReferences()
        {
            int row = 7;
            RI.FastPath($"LP2CI{Borrower.Demos.Ssn}");
            if (RI.CheckForText(1, 65, "REFERENCE SELECT"))
            {
                while (RI.AltMessageCode != "46004")
                {
                    if (RI.CheckForText(row, 27, "A") && !RI.CheckForText(row, 37, "04"))
                    {
                        RI.PutText(21, 13, RI.GetText(row - 1, 2, 2), ReflectionInterface.Key.Enter);
                        ReferenceCheck();
                        RI.Hit(ReflectionInterface.Key.F12);
                    }
                    row += 3;
                    if (row > 19 || RI.GetText(row - 1, 3, 1).IsNullOrEmpty())
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 7;
                    }
                }
            }
            else if (RI.CheckForText(1, 59, "REFERENCE DEMOGRAPHICS"))
            {
                if (RI.CheckForText(6, 67, "A") && !RI.CheckForText(5, 24, "04"))
                    ReferenceCheck();
            }

            if (Borrower.SkipSystem == FinalReview.SkipSystem.Compass || Borrower.SkipSystem == FinalReview.SkipSystem.Both)
                AddRefercalsQueue();

            if (LetterSent)
                if (!RI.AddCommentInLP50(RefAccount, "LT", "27", "KRREF", "", "FINALREV"))
                    RI.LogRun.AddNotification($"Error adding a KRREF activity record to Account: {RefAccount}", NotificationType.ErrorReport, NotificationSeverityType.Warning);

            Borrower.UpdateStep(DA, FinalReview.RecoveryStep.REF_REV);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReferenceCheck()
        {
            bool refrcal = false;
            string referenceId = RI.GetText(3, 14, 9);
            bool Ksbup = RI.CheckForText(6, 67, "A") && !RI.CheckForText(13, 77, "04") && RI.CheckForText(13 ,36, "Y");
            DateTime defDate = new DateTime(1900, 1, 1);
            DateTime Dla = RI.GetText(13, 67, 8).ToDateNullable() ?? defDate;
            DateTime Dlc = RI.GetText(13, 58, 8).ToDateNullable() ?? defDate;
            DateTime Ped = RI.GetText(15, 71, 8).ToDateNullable() ?? defDate;
            DateTime Lsd = RI.GetText(8, 73, 8).ToDateNullable() ?? defDate;
            //task is complete if address skip type date criteria is met
            if (((Dla >= Borrower.StartDate && RI.GetText(13, 51, 2).ToInt() > 2)
                && (((Borrower.SkipType == "A" || Borrower.SkipType == "B") && Dla >= Ped) || Borrower.SkipType != "A"))
                || (Dlc >= Borrower.StartDate && Dlc >= Ped) || Lsd > Borrower.StartDate)
            { } //Do nothing
            else
            { //if the phone is valid, send a letter and add a call task
                if (RI.CheckForText(13, 36, "Y")) //If the phone is valid, send a letter and add a call task
                {
                    if (RI.CheckForText(8, 53, "Y")) //Send a letter if the address is valid
                        AddLetterData();
                    //Add REFERCAL queue task
                    AddReferenceData(referenceId, "", Ksbup, Borrower.StartDate, "REFRCALS");
                    refrcal = true;
                }
                else
                { //Send a letter if the address is valid or add a DA ref call task if the address is not valid
                    if (RI.CheckForText(8, 53, "Y"))
                        AddLetterData();
                    else
                    {
                        AddReferenceData(referenceId, "DA call, reference phone invalid", Ksbup, Borrower.StartDate, "REFRCALR");
                        refrcal = true;
                    }
                }
            }
            if (Ksbup && !refrcal) //Add a record to look for a KSUBP activity record if a REFRCAL queue task was not added
                AddReferenceData(referenceId, "", Ksbup, Borrower.StartDate, "REFRCALS");
        }

        /// <summary>
        /// Add a reference to the list so the LP50 process and run later
        /// </summary>
        private void AddReferenceData(string referenceId, string comment, bool ksbup, DateTime startDate, string queue)
        {
            //Check if the reference is in the list of reference data and update the record
            for (int i = 0; i < Borrower.ReferenceIds.Count; i++)
            {
                if (Borrower.ReferenceIds[i].ReferenceId == referenceId)
                {
                    if (!Borrower.ReferenceIds[i].IsKsbup && ksbup)
                        Borrower.ReferenceIds[i].IsKsbup = true;
                    if (Borrower.ReferenceIds[i].Comment.IsNullOrEmpty() && comment.IsPopulated())
                        Borrower.ReferenceIds[i].Comment = comment;
                    return; //if the record exists, no need to add a new one
                }
            }

            //Add a record if one does not exist
            ReferenceData rData = new ReferenceData()
            {
                ReferenceId = referenceId,
                Comment = comment,
                IsKsbup = ksbup,
                RefDate = startDate,
                Queue = queue
            };
            Borrower.ReferenceIds.Add(rData);
        }

        /// <summary>
        /// Adds a REFRCALS queue to borrower account
        /// </summary>
        private void AddRefercalsQueue()
        {
            for (int i = 0; i < Borrower.ReferenceIds.Count; i++)
            {
                ReferenceData rd = Borrower.ReferenceIds[i];
                if (rd.IsKsbup)
                {
                    RI.FastPath($"LP50I{rd.ReferenceId}");
                    RI.PutText(9, 20, "KSBUP");
                    RI.PutText(18, 29, rd.RefDate.ToString("MMddyyyy"));
                    RI.PutText(18, 41, DateTime.Now.Date.ToString("MMddyyyy"));
                    if (RI.CheckForText(1, 58, "ACTIVITY SUMMARY SELECT"))
                        RI.PutText(3, 13, "X", ReflectionInterface.Key.Enter);
                    if (RI.CheckForText(1, 58, "ACTIVITY DETAIL DISPLAY"))
                    {
                        for (int row = 13; row < 20; row++)
                            rd.Comment += RI.GetText(row, 2, 75);
                        rd.Comment = rd.Comment.Substring(0, 180); //Only allow the first 180 characters
                    }
                }
                Borrower.TaskAdded = true;
                RI.FastPath($"LP8YISKP;{rd.Queue};;{Borrower.Demos.Ssn};{rd.ReferenceId}");
                if (!RI.CheckForText(1, 64, "QUEUE TASK DETAIL"))
                    RI.AddQueueTaskInLP9O(rd.ReferenceId, rd.Queue);
            }
        }

        /// <summary>
        /// Add record to the data file to be printed later
        /// </summary>
        private void AddLetterData()
        {
            RefAccount = RI.GetText(3, 14, 9);
            if (LettersData.Any(p => p.FirstName == Borrower.Demos.FirstName && p.LastName == Borrower.Demos.LastName && p.ReferenceId == RefAccount))
                return; // The record already exists

            string refName = $"{RI.GetText(4, 44, 11)}{RI.GetText(4, 60, 1).Insert(0, " ")} {RI.GetText(4, 5, 34)}";
            string refZip = RI.GetText(10, 60, 9).Length == 9 ? RI.GetText(10, 60, 9).Insert(5, "-") : RI.GetText(10, 60, 9);
            string refState = RI.GetText(9, 55, 1).IsNullOrEmpty() ? RI.GetText(10, 52, 2) : "";
            string keyLine = DocumentProcessing.ACSKeyLine(RefAccount, DocumentProcessing.LetterRecipient.Reference, DocumentProcessing.ACSKeyLineAddressType.Legal);
            string letterData = $"{keyLine},{Borrower.Demos.AccountNumber},{refName},{RI.GetText(8, 9, 34)},{RI.GetText(9, 9, 34)},{RI.GetText(10, 9, 29)},{refState},{refZip},{RI.GetText(9, 55, 25)}" +
                    $",{Borrower.Demos.FirstName},{Borrower.Demos.LastName},{Borrower.Demos.Address1},{Borrower.Demos.Address2},{Borrower.Demos.City},{Borrower.Demos.State}" +
                    $",{Borrower.Demos.ZipCode},{Borrower.Demos.Country},{RefAccount},{RI.GetText(10, 52, 2)},MA2330";
            if (!EcorrProcessing.AddRecordToPrintProcessing(FinalReview.ScriptId, "REFRCAL", letterData, Borrower.Demos.AccountNumber, "MA2330", DataAccessHelper.CurrentRegion).HasValue)
            {
                string message = $"There was an error adding a Print Processing record for Account: {RefAccount}, Letter ID: REFRCAL";
                RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                WriteLine(message);
            }
            else
            {
                LetterSent = true;
                WriteLine($"Added record to Print Processing to send REFRCAL letter to reference: {RefAccount}");
            }
        }
    }
}
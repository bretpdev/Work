using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace COVIDFORB
{
    class ForbearanceProcessor
    {
        public ProcessLogRun logRun { get; set; }
        public DataAccess DA { get; set; }

        public ForbearanceProcessor(ProcessLogRun logRun)
        {
            this.logRun = logRun;
            DA = new DataAccess(logRun);
        }

        public void Process()
        {
            //Get Unprocessed Records
            List<ForbProcessingRecord> records = DA.GetUnprocessedForb();

            //Access ATS0H
            //Parallel.ForEach(records, new ParallelOptions() { MaxDegreeOfParallelism = Program.SessionCount },
            //    record =>
            //    {
                    WorkRecord(records);
                //});

        }

        public void WorkRecord(List<ForbProcessingRecord> records)
        {
            ReflectionInterface ri = null;
            try
            {
                ri = GetSingleRI();
                if (ri.IsLoggedIn)
                {
                    foreach (var record in records)
                        HandleATS0H(ri, record);
                }
                else
                {
                    throw new Exception("Session not logged in.");
                }
            }
            finally
            {
                if (ri != null)
                {
                    ri.CloseSession();
                }
            }

        }

        public ReflectionInterface GetSingleRI()
        {
            ReflectionInterface ri = new ReflectionInterface();
            BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(logRun, ri, Program.ScriptId, "BatchUheaa");
            return ri;
        }

        public void HandleATS0H(ReflectionInterface ri, ForbProcessingRecord record)
        {
            //Get loan informtion for record, null if all loans
            List<int> loans = null;
            if (!record.SelectAllLoans)
            {
                loans = DA.GetSelectedLoans(record.ForbearanceProcessingId);
                if (loans != null && loans.Count == 0)
                {
                    //Error occured selecting loans
                    logRun.AddNotification(ErrorMessages.NoLoansSelected(record, ri), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    ArcProcessor.SetProcessedFailure(record, DA, logRun, ErrorMessages.NoLoansSelected(record, ri));
                    return;
                }
            }
            ri.FastPath("TX3Z/ATS0H");
            if (CheckScreenCodeError(ri, "TSX7F", record, ErrorMessages.GetTSX7F(record, ri), loans))
            {
                //An error occured accessing ATS0H
                return;
            }
            //Enter Search Information On ATS0H
            ri.PutText(7, 33, record.AccountNumber); //Account Number
            ri.PutText(9, 33, record.ForbCode);
            ri.PutText(11, 33, record.DateRequested.ToString("MMddyy"));
            ri.Hit(ReflectionInterface.Key.Enter);
            //Check Screen Code For Page After ATS0H
            if (CheckScreenCodeError(ri, "TSX7E", record, ErrorMessages.GetTSX7E(record, ri), loans))
            {
                //An error occured after the record informtion was entered on ATS0H
                return;
            }
            //Select Forbearance Type
            string selection = IterateForbearanceType(ri, record, loans);
            if (selection == null)
            {
                //An error occured selecting the forbearance type
                return;
            }
            ri.PutText(21, 13, selection);
            ri.Hit(ReflectionInterface.Key.Enter);
            //Check No Loans Found For The Borrower
            if (ri.CheckForText(23, 2, "50108"))
            {
                //Borrower has no loans
                logRun.AddNotification(ErrorMessages.BorrowerHasNoLoans(record, ri), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                ArcProcessor.SetProcessedFailure(record, DA, logRun, ErrorMessages.BorrowerHasNoLoans(record, ri));
                return;
            }

            //If TSX32 is reached from this point there is only 1 loan and loans do no need to be selected
            if (!ri.CheckForText(1, 72, "TSX32"))
            {
                //Select Loans
                if (CheckScreenCodeError(ri, "TSXA5", record, ErrorMessages.GetTSXA5(record, ri), loans))
                {
                    return;
                }
                //DA.GetSelectedLoans(record.ForbearanceProcessingId);
                bool success = SelectLoans(ri, record, loans);
                if (!success)
                {
                    //Error occured selecting loans
                    logRun.AddNotification(ErrorMessages.NoLoansSelected(record, ri), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    ArcProcessor.SetProcessedFailure(record, DA, logRun, ErrorMessages.NoLoansSelected(record, ri));
                    return;
                }
            }

            //Put forb info into TSX32
            if (EnterForbRecord(ri, record, loans))
            {
                //forb successfully added
                ArcProcessor.SetProcessedSuccess(record, DA, logRun);
            }
        }

        public bool EnterForbRecord(ReflectionInterface ri, ForbProcessingRecord record, List<int> loans)
        {
            //Return no success if errored
            if (CheckScreenCodeError(ri, "TSX32", record, ErrorMessages.GetTSX32(record, ri), loans))
            {
                return false;
            }
            //Enter Forb Data
            ri.PutText(7, 18, record.StartDate.ToString("MMddyy")); //enter begin date
            ri.PutText(8, 18, record.EndDate.ToString("MMddyy")); //enter end date
            ri.PutText(9, 18, record.DateCertified.ToString("MMddyy")); //enter date certified
            if (record.SubType != null && record.SubType != "" && ri.CheckForText(9, 37, "_"))
            {
                ri.PutText(9, 37, record.SubType); //enter subtype if it exists
            }
            if (record.SchoolCode != null && record.SchoolCode != "" && ri.CheckForText(10, 15, "_"))
            {
                ri.PutText(10, 15, record.SchoolCode); //enter schoolCode if it exists
            }
            if (record.MedicalInternship != null && record.MedicalInternship != "" && ri.CheckForText(11, 34, "_"))
            {
                ri.PutText(11, 34, record.MedicalInternship); //enter the medical internship flag if it exists
            }
            if (record.StateLicensingCertificationProvided != null && record.StateLicensingCertificationProvided != "" && ri.CheckForText(12, 42, "_"))
            {
                ri.PutText(12, 42, record.StateLicensingCertificationProvided); //enter the StateLicensingCertificationProvided flag if it exists
            }
            if (record.SchoolEnrollment != null && record.SchoolEnrollment != "" && ri.CheckForText(13, 73, "_"))
            {
                ri.PutText(13, 73, record.SchoolEnrollment); //enter schoolEnrollment if it exists
            }
            if (record.ReservistNationalGuard != null && record.ReservistNationalGuard != "" && ri.CheckForText(14, 34, "_"))
            {
                ri.PutText(14, 34, record.ReservistNationalGuard); //enter ReservistNationalGuard if it exists
            }
            if (record.DodForm != null && record.DodForm != "" && ri.CheckForText(15, 73, "_"))
            {
                ri.PutText(15, 73, record.DodForm); //enter DodForm if it exists
            }
            if (record.SignatureOfOfficial != null && record.SignatureOfOfficial != "" && ri.CheckForText(18, 73, "_"))
            {
                ri.PutText(18, 73, record.SignatureOfOfficial); //enter SignatureOfOfficial if it exists
            }
            if (record.PhysiciansCertification != null && record.PhysiciansCertification != "" && ri.CheckForText(18, 73, "_"))
            {
                ri.PutText(18, 73, record.PhysiciansCertification); //enter PhysiciansCertification if it exists
            }
            if (record.CoMakerEligibility != null && record.CoMakerEligibility != "" && ri.CheckForText(14, 73, "_"))
            {
                ri.PutText(14, 73, record.CoMakerEligibility); //enter comaker elligibility if it exists
            }
            if (record.AuthorizedToExceedMax != null && record.AuthorizedToExceedMax != "" && ri.CheckForText(15, 34, "_"))
            {
                ri.PutText(15, 34, record.AuthorizedToExceedMax); //enter authorized to exceed max if it exists
            }
            if (record.PaymentAmount != null && record.PaymentAmount != "" && ri.CheckForText(17, 66, "_"))
            {
                ri.PutText(17, 66, record.PaymentAmount);
            }
            if (record.SignatureOfBorrower != null && record.SignatureOfBorrower != "" && ri.CheckForText(18, 34, "_"))
            {
                ri.PutText(18, 34, record.SignatureOfBorrower);
            }
            if (record.ForbToClearDelq != null && record.ForbToClearDelq != "" && ri.CheckForText(16, 37, "_"))
            {
                ri.PutText(16, 37, record.ForbToClearDelq); //Enter Forbearance to clear delinquency
            }
            if (record.CapitalizeInterest != null && record.CapitalizeInterest != "" && ri.CheckForText(17, 34, "_"))
            {
                ri.PutText(17, 34, record.CapitalizeInterest);//Enter Capitalize Interest If It Is Not Null
            }
            ri.PutText(20, 17, "", ReflectionInterface.Key.EndKey); //Clear manual review field
            ri.Hit(ReflectionInterface.Key.Enter);
            //Validate Successful Addition Of Forb
            if (ri.CheckForText(1, 72, "TSX30"))
            {
                ri.PutText(21, 15, "1", ReflectionInterface.Key.Enter);
            }

            if (IfScreenThenError(ri, "TSX30", record, ErrorMessages.GetTSX30(record, ri), loans, logRun.ProcessLogId))
            {
                return false;
            }
            if (IfScreenThenError(ri, "TSX31", record, ErrorMessages.GetTSX31(record, ri), loans, logRun.ProcessLogId))
            {
                return false;
            }
            if (CheckScreenCodeError(ri, "TSX32", record, ErrorMessages.GetTSX32(record, ri), loans))
            {
                return false;
            }
            string message = ri.GetText(23, 2, 5);
            if (!ri.CheckForText(23, 2, "01004"))
            {
                //Error if message code was not success
                logRun.AddNotification(ErrorMessages.Get01004(record, ri), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                ArcProcessor.SetProcessedFailure(record, DA, logRun, ErrorMessages.Get01004(record, ri));
                return false;
            }
            //Add Successful, Continue To Success Arc
            return true;
        }

        public bool SelectLoans(ReflectionInterface ri, ForbProcessingRecord record, List<int> loans)
        {
            if (loans == null)
            {
                if (ri.CheckForText(1, 72, "TSXA5"))
                {
                    for (int row = 12; !ri.CheckForText(23, 2, "90007"); row++)
                    {
                        if (row > 21)
                        {
                            row = 11;
                            ri.Hit(ReflectionInterface.Key.F8);
                            continue;
                        }

                        if (ri.CheckForText(row, 2, "_") && !ri.CheckForText(row, 17, "TILP"))
                            ri.PutText(row, 2, "X");
                    }
                    ri.PutText(8, 14, "", true);

                    ri.Hit(ReflectionInterface.Key.Enter);
                }

                //ri.PutText(8, 14, "X");
                //ri.Hit(ReflectionInterface.Key.Enter);
                ri.CheckScreenAbend();
                return true;
            }
            else
            {
                bool rowSelected = false;
                int beginRow = 12;
                int endRow = 22;
                for (int row = beginRow; row <= endRow; row++)
                {
                    int loan;
                    bool success = int.TryParse(ri.GetText(row, 13, 3), out loan);
                    if (success && loans.Contains(loan) && ri.CheckForText(row, 2, "_"))
                    {
                        ri.PutText(row, 2, "X");
                        rowSelected = true;
                    }

                    if (row == endRow)
                    {
                        ri.Hit(ReflectionInterface.Key.F8);
                        row = beginRow;
                        if (ri.CheckForText(23, 2, "90007"))
                        {
                            if (!rowSelected)
                            {
                                logRun.AddNotification(ErrorMessages.NoLoansSelected(record, ri), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                                ArcProcessor.SetProcessedFailure(record, DA, logRun, ErrorMessages.NoLoansSelected(record, ri));
                            }
                            ri.Hit(ReflectionInterface.Key.Enter);
                            ri.CheckScreenAbend();
                            //Screen code should be TSXA5
                            //Finish
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public string IterateForbearanceType(ReflectionInterface ri, ForbProcessingRecord record, List<int> loans)
        {
            string selection = "";
            int beginRow = 7;
            int endRow = 19;
            int row = beginRow;
            while (row <= endRow)
            {
                //Check first col
                if (ri.CheckForText(row, 3, record.ForbearanceType))
                {
                    selection = ri.GetText(row, 3, 2);
                    return selection;
                }
                //Check second col
                if (ri.CheckForText(row, 42, record.ForbearanceType))
                {
                    selection = ri.GetText(row, 42, 2);
                    return selection;
                }
                //Tab to the next page
                if (row == endRow)
                {
                    ri.Hit(ReflectionInterface.Key.F8);
                    row = beginRow;
                    //Check if there are no more records, error if there are no more
                    if (ri.CheckForText(23, 2, "90007"))
                    {
                        logRun.AddNotification(ErrorMessages.BadForbType(record, ri), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        ArcProcessor.SetProcessedFailure(record, DA, logRun, ErrorMessages.BadForbType(record, ri));
                        return null;
                    }
                    continue;
                }
                row++;
            }
            logRun.AddNotification(ErrorMessages.BadForbType(record, ri), NotificationType.ErrorReport, NotificationSeverityType.Critical);
            ArcProcessor.SetProcessedFailure(record, DA, logRun, ErrorMessages.BadForbType(record, ri));
            return null;
        }

        /// <summary>
        /// Validates the screen code in the top right
        /// </summary>
        /// <param name="screenCode">Code to validate against(5 characters)</param>
        public bool CheckScreenCodeError(ReflectionInterface ri, string screenCode, ForbProcessingRecord record, string comment, List<int> loans)
        {
            if (!ri.CheckForText(1, 72, screenCode))
            {
                logRun.AddNotification("Unexpected Screen Encountered, Expecting: " + screenCode + " Received: " + ri.GetText(1, 72, 5), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                ArcProcessor.SetProcessedFailure(record, DA, logRun, comment);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Validates the screen code in the top right
        /// </summary>
        /// <param name="screenCode">Code to validate against(5 characters)</param>
        public bool IfScreenThenError(ReflectionInterface ri, string screenCode, ForbProcessingRecord record, string comment, List<int> loans, int plId)
        {
            if (ri.CheckForText(1, 72, screenCode))
            {
                logRun.AddNotification("Unexpected Screen Encountered, Received: " + ri.GetText(1, 72, 5) + " " + comment, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                ArcProcessor.SetProcessedFailure(record, DA, logRun, comment + $" PL: {plId}");
                return true;
            }
            return false;
        }

    }
}

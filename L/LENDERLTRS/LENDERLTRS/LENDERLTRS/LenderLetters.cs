using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace LENDERLTRS
{
    public class LenderLetters : ScriptBase
    {
        private bool? JR_Queue { get; set; }
        private bool ScriptEnd { get; set; } = false;
        private bool ClosedJRtask { get; set; } = false;
        private string Queue { get; set; } = "LD";
        private string Subq { get; set; } = "01";
        private ProcessLogRun LogRun { get; set; }


        public LenderLetters(ReflectionInterface ri)
            : base(ri, "LENDERLTRS", DataAccessHelper.Region.Uheaa)
        {
            LogRun = new ProcessLogRun((RI.LogRun?.ProcessLogId ?? ProcessLogData.ProcessLogId), "LENDERLTRS", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, true);
            RI.LogRun = LogRun;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }

        public override void Main()
        {
            ProcessTask();
            LogRun.LogEnd();
        }

        private void ProcessTask()
        {
            JR_Queue = Dialog.Info.YesNoCancel("Would you like to process the JR queue? \n Selecting \"No\" will process the LD queue.");
            if (JR_Queue == null)
                return;

            do
            {
                if (JR_Queue == true)
                {
                    RI.FastPath("TX3ZITX6XJR;01");
                    if (!RI.CheckForText(1, 74, "TXX71"))
                    {
                        Dialog.Info.Ok("There are no tasks to process in the JR queue.");
                        return;
                    }
                }
                else
                {
                    RI.FastPath("TX3ZITX6XLD;01");
                    if (RI.CheckForText(23, 2, "01020"))
                    {
                        Dialog.Info.Ok("There are no tasks to process in the LD queue.");
                        return;
                    }
                    ProcessLDQueue();
                    return;
                }

                RI.PutText(21, 18, "01", ReflectionInterface.Key.Enter);

                if (ProcessBorrower() > 0) //ITX1J
                    return;

            } while (Dialog.Info.YesNo("Would you like to process another task?"));
        }

        private bool ProcessLDQueue()
        {
            int currRow = 8;
            int startRow = 8;
            int endRow = 17;
            int skipRows = 3;
            bool keepGoing = true;
            int properPage = 1;
            while (!RI.CheckForText(23, 2, "90007") && !RI.CheckForText(23, 2, "46004") && keepGoing)
            {
                string selection = string.Empty;
                selection = RI.GetText(currRow, 4, 2);
                if (!selection.IsNullOrEmpty())
                {
                    RI.PutText(21, 18, selection.ToString().PadLeft(2, '0'), ReflectionInterface.Key.Enter);
                    if (ProcessBorrower() > 0)
                        keepGoing = false;

                    currRow += skipRows;
                    RI.FastPath("TX3ZITX6XLD;01");
                    int currentPage = RI.GetText(2, 71, 2).ToInt();
                    if (currentPage < properPage)
                        for (int n = currentPage; n < properPage; n++)
                            RI.Hit(ReflectionInterface.Key.F8);

                    if (currRow > endRow)
                    {
                        currRow = startRow;
                        properPage++;
                        RI.Hit(ReflectionInterface.Key.F8);
                    }

                    if (!keepGoing)
                        return keepGoing;

                    if (!Dialog.Info.YesNo("Would you like to process another task?"))
                        return keepGoing;
                }

            }
            return true;
        }

        private int ProcessBorrower()
        {
            BorrowerData data = new BorrowerData();
            data.Ssn = RI.GetText(1, 19, 9);
            PopulateBorrower(data);

            GetTS26Data(data);
            Lenders lenderList = new Lenders();

            foreach (LoanData ld in data.LoanInfo)
                if (lenderList.InBana.ContainsKey(ld.OrigLender))
                    ld.OrigLender = "814817";

            List<string> lenders = data.LoanInfo.Select(p => p.OrigLender).Distinct().ToList();
            int lenderCount = 0;
            bool canceled = false;
            bool inUheaa = false;
            bool inBana = false;
            int queueToClose = 0;

            foreach (string lender in lenders)
            {
                if (ClosedJRtask)
                    continue;

                canceled = false;
                RI.FastPath("TX3Z/ITX0H" + lender);

                LenderData lenderInfo = new LenderData(RI);

                bool nextLender = false;

                using (LenderForm ldr = new LenderForm(lenderInfo))
                {
                    bool reshowForm = true;
                    do
                    {
                        DialogResult diRes = ldr.ShowDialog();

                        if (diRes == DialogResult.No) // The Unlocated button returns NO.
                        {
                            if (JR_Queue == true)
                            {
                                if (Dialog.Info.YesNo("This will close the JR and create an LD for locate assistance. Would you like to continue?"))
                                {
                                    CloseTask(false, data.AccountNumber);
                                    AddArc(true, data.AccountNumber, lender, true);
                                    ClosedJRtask = true; // Do not processs any more lenders for this borrower
                                    nextLender = true;
                                    break;
                                }
                                else
                                    continue;
                            }
                            else
                            {
                                RI.ReAssignQueueTask(Queue, Subq, UserId, "");
                                return 0;
                            }
                        }
                        else if (diRes == DialogResult.Cancel)
                        {
                            UnassignQueue(data.Ssn, JR_Queue == true ? "JR" : "LD");
                            canceled = true;
                            ScriptEnd = true;
                            return 1;
                        }

                        if (!ldr.UpdatedData.Valid)
                        {
                            if (ldr.UpdatedData.Address1.Contains("X-CLOSED"))
                                ldr.UpdatedData.Valid = false;
                            else
                                ldr.UpdatedData.Valid = true;
                            DataAccess da = new DataAccess(LogRun);
                            da.AddToDb(ldr.UpdatedData);
                        }

                        if (!ldr.UpdatedData.Valid && ldr.UpdatedData.Address1.Contains("X-CLOSED"))
                        {
                            AddArc(false, data.AccountNumber, ldr.UpdatedData.LenderId, false);
                            nextLender = true;
                            break;
                        }

                        inUheaa = inUheaa ? true : lenderList.InUheaa.ContainsKey(lenderInfo.LenderId); // We only want to set this once.
                        inBana = inBana ? true : lenderList.InBana.ContainsKey(lenderInfo.LenderId); // We only want to set this once.
                        ++lenderCount;

                        if ((ldr.UpdatedData.Valid && !lenderList.InUheaa.ContainsKey(lenderInfo.LenderId)))
                        {
                            queueToClose += CreateLetter(data, ldr, lender);
                        }
                        reshowForm = false;

                    } while (reshowForm);
                }

                if (nextLender)
                    continue;
            }

            if (ClosedJRtask)
            {
                ClosedJRtask = false;
                return 0;
            }

            if ((!canceled && queueToClose > 0) || inBana || inUheaa)
                CloseTask(true, data.AccountNumber);
            else
                CloseTask(false, data.AccountNumber);

            if (inUheaa)
            {
                AddArc(true, data.AccountNumber, "834529", false);
            }

            return 0;
        }

        private int CreateLetter(BorrowerData _data, LenderForm _lf, string _lender)
        {
            int success = 0;
            if (_data.ForState.IsPopulated())
                _data.ForCountry = _data.ForState + ", " + _data.ForCountry;

            if (_data.ForeignPhone.IsPopulated())
                _data.Phone = _data.ForeignPhone;
            if (_data.ForeignAlt1.IsPopulated())
                _data.AltPhone1 = _data.ForeignAlt1;
            if (_data.ForeignAlt2.IsPopulated())
                _data.AltPhone2 = _data.ForeignAlt2;
            //File Header: LenderId,LenderName,LenderAddress1,LenderAddress2,City,State,ZIP,Country,BorrowerName,PrevName,Last4,BwrAddress1,BwrAddress2,BwrCity,BwrState,BwrZip,BwrCountry,BwrPhone,BwrAltPhone1,BwrAltPhone2,Email
            string letterData = $"\"{_lender}\", \"{_lf.UpdatedData.FullName}\", \"{_lf.UpdatedData.Address1}\", \"{_lf.UpdatedData.Address2}\", \"{_lf.UpdatedData.City}\", \"{_lf.UpdatedData.State}\","
            + $"\"{ _lf.UpdatedData.Zip}\", , \"{_data.Name}\", \"{_data.Prev}\", \"{_data.Ssn.Substring(5, 4)}\", \"{_data.Address1}\", \"{_data.Address2}\", \"{_data.City}\", \"{_data.State}\", \"{_data.Zip}\","
            + $"\"{_data.ForCountry}\", \"{_data.Phone}\", \"{_data.AltPhone1}\", \"{_data.AltPhone2}\", \"{_data.Email}\"";

            if (!EcorrProcessing.AddRecordToPrintProcessing("LENDERLTRS", "US06BLCNTM", letterData, _lender, "", DataAccessHelper.CurrentRegion, null, true).HasValue)
            {
                string message = $"There was an error adding the US06BLCNTM letter to lender: {_lender} for borrower: {_data.AccountNumber} to the Print Processing table.";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Dialog.Error.Ok(message, "Error");
            }
            if (_lf.UpdatedData.Valid)
                success++;
            AddArc(_lf.UpdatedData.Valid, _data.AccountNumber, _lender, false);
            return success;
        }
        private bool PopulateBorrower(BorrowerData _data)
        {
            RI.FastPath("TX3Z/ITS24" + _data.Ssn); // ITX01

            _data.Ssn = RI.GetText(1, 9, 9);
            _data.AccountNumber = RI.GetText(6, 10, 12).Replace(" ", "");
            _data.Name = RI.GetText(4, 37, 43);
            _data.Prev = RI.GetText(5, 37, 43);
            _data.Dob = RI.GetText(16, 68, 10);
            _data.Address1 = RI.GetText(8, 13, 32);
            _data.Address2 = RI.GetText(9, 13, 32).Replace("_", "");
            _data.City = RI.GetText(11, 13, 22);
            _data.State = RI.GetText(11, 36, 2);
            _data.ForState = RI.GetText(8, 61, 15);
            _data.ForCountry = RI.GetText(9, 54, 23);
            _data.Zip = RI.GetText(11, 40, 17);
            _data.Phone = RI.GetText(13, 21, 12);
            _data.AltPhone1 = RI.GetText(14, 21, 12);
            _data.AltPhone2 = RI.GetText(15, 21, 12);
            _data.Email = RI.GetText(17, 10, 69);
            _data.ForeignPhone = RI.GetText(13, 35, 25);
            _data.ForeignAlt1 = RI.GetText(14, 35, 25);
            _data.ForeignAlt2 = RI.GetText(15, 35, 25);
            return true;
        }


        private void CloseTask(bool wasValid, string accountNumber)
        {
            string queue = JR_Queue == true ? "JR" : "LD";

            if (!RI.CloseCompassQueue(queue, wasValid ? "C" : "X", wasValid ? "COMPL" : "CANCL").Contains("01005"))
            {
                string message = wasValid ? "Letter generated but queue not closed" : "Queue could not be closed, please review.";
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Informational);
                Dialog.Error.Ok(message, "Please Review");
            }
        }


        public void AddArc(bool wasValid, string accountNumber, string lenderId, bool isNoLocate)
        {
            string message = wasValid ? $"Sent original lender {lenderId} letter." : $"Original Lender {lenderId} is now closed, unable to send skip letter for borrower demographics.";
            if (lenderId == "834529")
                message = "834529: We are the original lender.";

            ArcData arc = new ArcData(DataAccessHelper.CurrentRegion)
            {
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                AccountNumber = accountNumber,
                Arc = "LCNCM",
                ScriptId = ScriptId,
                Comment = message
            };

            ArcAddResults result = arc.AddArc();
            if (!result.ArcAdded)
            {
                string errorMessage = $"Unable to add the following ARC to ArcAddProcessing, please review.  Arc:{arc.Arc} ArcType:{arc.ArcTypeSelected} AccountNumber:{arc.AccountNumber} Comment:{arc.Comment}";
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Dialog.Error.Ok(errorMessage, "Error Adding Arc");
            }

            if ((JR_Queue == true) && isNoLocate)
            {
                message = $"Unable to locate lender: {lenderId}, creating task for locate assistance.";
                arc = new ArcData(DataAccessHelper.CurrentRegion)
                {
                    ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                    AccountNumber = accountNumber,
                    Arc = "QUELD",
                    ScriptId = ScriptId,
                    Comment = message
                };

                result = arc.AddArc();
                if (!result.ArcAdded)
                {
                    string errorMessage = $"Unable to add the following ARC to ArcAddProcessing, please review.  Arc:{arc.Arc} ArcType:{arc.ArcTypeSelected} AccountNumber:{arc.AccountNumber} Comment:{arc.Comment}";
                    ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, errorMessage, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    Dialog.Error.Ok(errorMessage, "Error Adding Arc");
                }
            }
        }

        private void GetTS26Data(BorrowerData data)
        {
            RI.FastPath("TX3Z/ITS26" + data.Ssn);
            if (RI.ScreenCode == "TSX29")//Only one loan
            {
                GetLoanInfo(data);
                if (RI.CheckForText(23, 2, "40040"))
                    RI.Hit(ReflectionInterface.Key.F12);
            }
            else if (RI.ScreenCode == "TSX28")//Selection screen
            {
                for (int row = 8; RI.MessageCode != "90007"; row++)
                {
                    if (row > 19 || RI.CheckForText(row, 3, " "))
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        row = 7;
                        continue;
                    }


                    RI.PutText(21, 12, RI.GetText(row, 2, 2), ReflectionInterface.Key.Enter, true);
                    GetLoanInfo(data);
                    if (RI.CheckForText(23, 2, "40040"))
                        RI.Hit(ReflectionInterface.Key.F12);
                    else
                        RI.Hit(ReflectionInterface.Key.F12, 2);
                }
            }
            else
            {
                string message = $"An unexpected screen was encountered.  Expected to be on TSX28 or TSX29, current screen code {RI.ScreenCode}.  PLease review and try again.";
                ProcessLogger.AddNotification(ProcessLogData.ProcessLogId, message, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                Dialog.Error.Ok(message);
                EndDllScript();
            }
        }

        private void GetLoanInfo(BorrowerData data)
        {
            int loanSeq = RI.GetText(7, 35, 4).ToInt();
            RI.Hit(ReflectionInterface.Key.Enter);
            if (RI.CheckForText(23, 2, "40040"))
                return;
            data.LoanInfo.Add(new LoanData() { LoanSeq = loanSeq, OrigLender = RI.GetText(10, 40, 6) });
        }

        private void UnassignQueue(string ssn, string queue)
        {
            RI.FastPath("TX3Z/CTX6J");
            RI.PutText(7, 42, queue);
            RI.PutText(8, 42, "01");
            string wildcard = ssn + "*";
            RI.PutText(9, 42, wildcard);
            RI.PutText(12, 42, "W");
            RI.PutText(13, 42, UserId);
            RI.PutText(9, 76, queue == "JR" ? "D" : "A", ReflectionInterface.Key.Enter);
            if (!RI.CheckForText(1, 72, "TXX6O"))
            {
                LogRun.AddNotification("Unable to un-assign task, manual un-assign required.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                RI.PutText(23, 2, "Unable to un-assign task, manual un-assign required.");
            }
            RI.PutText(8, 15, "", ReflectionInterface.Key.EndKey);
            RI.Hit(ReflectionInterface.Key.Enter);
            if (!RI.CheckForText(23, 2, "01005"))
            {
                LogRun.AddNotification("Unable to un-assign task, manual un-assign required.", NotificationType.ErrorReport, NotificationSeverityType.Informational);
                RI.PutText(23, 2, "Unable to un-assign task, manual un-assign required.");
            }
        }

        private void AddLdTask()
        {
            RI.FastPath("PROF");
            RI.Hit(ReflectionInterface.Key.Enter);
            string userId = RI.GetText(2, 49, 8);
            RI.Hit(ReflectionInterface.Key.F3);

            RI.FastPath("TX3Z/CTX6J");
            RI.PutText(7, 42, "LD");
            RI.PutText(8, 42, "01");
            RI.PutText(12, 42, userId);
            RI.Hit(ReflectionInterface.Key.Enter);
        }

    }
}

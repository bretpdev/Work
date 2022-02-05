using MDIntermediary;
using MDLetters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Scripts.ReflectionInterface;

namespace MauiDUDE
{
    public class HomePageSessionInteractionCoordinator //: ReflectionInterfaceForDUDE
    {
        //TODO static concerns me
        private Borrower _borrower;
        private DataAccessHelper.Region _region;
        private bool _sessionInUse = false;
        public ReflectionInterface RI;
        //this constructor is only used by internal shared functionality
        private HomePageSessionInteractionCoordinator()
        {
            RI = SessionInteractionComponents.RI;
        }

        public HomePageSessionInteractionCoordinator(Borrower borrower, DataAccessHelper.Region region)
        {
            _borrower = borrower;
            _region = region;
            RI = SessionInteractionComponents.RI;
        }

        public static string DoPreBorrowerCreationProcessing(AcpQueueInfo queueInfoContainer)
        {
            HomePageSessionInteractionCoordinator sessionInteractor = new HomePageSessionInteractionCoordinator();
            return sessionInteractor.ACPDialerQueueAndFastPathCheck(queueInfoContainer);
        }

        public static bool AddArcForAllLoans(string ssn, string arc, string comment, string scriptId, bool pause, DataAccessHelper.Region region)
        {
            HomePageSessionInteractionCoordinator sessionInteractor = new HomePageSessionInteractionCoordinator();
            sessionInteractor.RI.Stup(region, DataAccessHelper.CurrentMode);
            return sessionInteractor.RI.Atd22AllLoans(ssn, arc, comment, "", scriptId, pause);
        }

        /// <summary>
        /// Checks if the user started in ACP and collect queue information for queue completion later if working a queue task
        /// </summary>
        private string ACPDialerQueueAndFastPathCheck(AcpQueueInfo queueInfoContainer)
        {
            string SSNOrAccountNumber = "";

            if (RI.CheckForText(1, 72, "TCX05")) //address verification screen for ACP
            {
                SSNOrAccountNumber = RI.GetText(1, 9, 9);
                RI.Hit(Key.F12);
                if (RI.CheckForText(1, 72, "J0X02")) //user is using a queue or the dialer
                {
                    RI.Hit(Key.Home);
                    RI.EnterText("ITX6X");
                    RI.Hit(Key.EndKey);
                    RI.Hit(Key.Enter);
                    if (!RI.CheckForText(6, 37, "__")) //user working a queue task
                    {
                        //populate queue and sub queue information if applicable
                        queueInfoContainer.Queue = RI.GetText(6, 37, 2);
                        queueInfoContainer.SubQueue = RI.GetText(8, 37, 2);

                        //go to TX1J to determine the region the user is in
                        RI.FastPath("TX3ZITX1JB");
                        queueInfoContainer.QueueRegion = DataAccessHelper.Region.Uheaa;
                    }
                }
            }
            else
            {
                //Get account#/ssn from fast path if possible
                if (!RI.CheckForText(1, 2, "")) //OneLINK/LO TODO this seems like it wouldn't work
                {
                    return RI.GetText(1, 9, 9);
                }
                else if (RI.CheckForText(1, 5, "TX1J")) //Compass TX1J
                {
                    return RI.GetText(1, 11, 9);
                }
                else if (!RI.CheckForText(1, 4, " ") && !RI.CheckForText(1, 4, "_"))//Compass other screens
                {
                    return RI.GetText(1, 9, 9);
                }
                else
                {
                    return "";
                }
            }
            //return SSN
            return SSNOrAccountNumber;
        }

        /// <summary>
        /// plugs in passed in pay off date to system and returns payoff amount
        /// </summary>
        public string GetPayoffInformation(DateTime payOffDate)
        {
            RI.Stup(_region, DataAccessHelper.CurrentMode);
            RI.FastPath($"TX3Z/ITS2O{_borrower.SSN}");
            if (RI.CheckForText(1, 72, "T1X01") || RI.CheckForText(23, 2, "50108"))
            {
                return "NA"; //means that ITS2O didn't display for the borrower
            }
            else
            {
                RI.PutText(7, 26, payOffDate.ToString("MM"));
                RI.PutText(7, 29, payOffDate.ToString("dd"));
                RI.PutText(7, 32, payOffDate.ToString("yy"));
                RI.PutText(9, 16, "X");
                RI.PutText(9, 54, "Y", Key.Enter);
                RI.Hit(Key.Enter);
                return RI.GetText(12, 29, 10);
            }
        }

        /// <summary>
        /// Gets months left on a deferment or forbearance
        /// </summary>
        public string GetDefermentOrFobearanceMonthsLeft(DefermentForbearance df)
        {
            if (_sessionInUse)
            {
                return "";
            }

            _sessionInUse = true;
            int row;

            RI.Stup(_region, DataAccessHelper.CurrentMode);
            RI.FastPath($"TX3ZITS26{_borrower.SSN}");
            if (RI.CheckForText(1, 72, "TSX28"))
            {
                //selection screen
                row = 8;
                //find the correct loan sequence number
                while (RI.GetText(row, 14, 4).ToInt() != df.OneofTheLoanSequenceNumbersAssociatedWithDeferOrForb)
                {
                    row++;
                    if (RI.CheckForText(row, 3, " "))
                    {
                        RI.Hit(Key.F8);
                        row = 8;
                    }
                }
                //select the row
                RI.PutText(21, 12, RI.GetText(row, 2, 2), Key.Enter);
            }

            //on target screen
            RI.Hit(Key.F2);
            RI.Hit(Key.F7);

            row = 12;
            while (RI.GetText(row, 21, 8).ToDate() != df.BeginDate.ToDate()
                || RI.GetText(row, 30, 8).ToDate() != df.EndDate.ToDate()
                || (df.CertDate.Length != 0 && RI.GetText(row, 72, 8).ToDate() != df.CertDate.ToDate()))
            {
                row++;

                if(RI.CheckForText(row, 6, " "))
                {
                    RI.Hit(Key.F8);
                    if(RI.CheckForText(23,2,"90007"))
                    {
                        _sessionInUse = false;
                        return "";
                    }
                    row = 12;
                }
            }

            if(!RI.GetText(row, 58, 7).IsNumeric())
            {
                _sessionInUse = false;
                return RI.GetText(row, 58, 7);
            }
            else
            {
                //turn into month value
                _sessionInUse = false;
                return ((RI.GetText(row, 58, 7).ToLong() / 365) * 12).ToString();
            }
        }

        public bool ReprintBill()
        {
            RI.Stup(_region, DataAccessHelper.CurrentMode);
            //access ITS26
            RI.FastPath($"TX3ZITS26{_borrower.SSN}");
            if(RI.CheckForText(1,72,"TSX28"))
            {
                //TS26 selection screen
                int row = 8;
                while(!RI.CheckForText(23,2, "90007 NO MORE DATA TO DISPLAY"))
                {
                    if(!RI.CheckForText(row, 64, " 0.00") && !RI.CheckForText(row, 69, "CR"))
                    {
                        RI.PutText(21, 12, RI.GetText(row, 2, 3), Key.Enter); //select loan
                        RI.Hit(Key.Enter);
                        RI.Hit(Key.Enter);
                        RI.Hit(Key.F6);
                        if(RI.CheckForText(1,72,"T1X01"))
                        {
                            //billing screen not displayed
                            WhoaDUDE.ShowWhoaDUDE("Maui DUDE wasn't able to find an active bill to re-print.", "Active Bill Not Found");
                            return false;
                        }
                        else if(RI.CheckForText(1,72,"TSX15"))
                        {
                            //target screen
                            if(RI.CheckForText(6,54,"A"))
                            {
                                RI.Hit(Key.F2);
                                KnarlyDUDE.ShowKnarlyDude("Processing Complete", "Processing Complete");
                                return true;
                            }
                            else
                            {
                                //if an active bill wasn't found
                                WhoaDUDE.ShowWhoaDUDE("Maui DUDE wasn't able to find an active bill to re-print.", "Active Bill Not Found");
                                return false;
                            }
                        }
                        else if(RI.CheckForText(1,72,"TSX14"))
                        {
                            //selection screen
                            //search for an active bill
                            int subRow = 8;
                            while(!RI.CheckForText(23,2, "90007 NO MORE DATA TO DISPLAY"))
                            {
                                if(RI.CheckForText(subRow, 24, "A"))
                                {
                                    RI.PutText(21, 12, RI.GetText(subRow, 2, 3), Key.Enter); //Select loan
                                    RI.Hit(Key.F2);
                                    KnarlyDUDE.ShowKnarlyDude("Processing Complete", "Processing Complete");
                                    return true;
                                }
                                subRow++;
                                if(subRow == 21)
                                {
                                    subRow = 8;
                                    RI.Hit(Key.F8);
                                }
                            }
                            //if an active bill wasn't found
                            WhoaDUDE.ShowWhoaDUDE("Maui DUDE wasn't able to find an active bill to re-print.", "Active Bill Not Found");
                            return false;
                        }
                    }
                    //Check for page forward
                    row++;
                    if(row == 21)
                    {
                        row = 8;
                        RI.Hit(Key.F8);
                    }
                }
                WhoaDUDE.ShowWhoaDUDE("Maui DUDE couldn't find a COMPASS loan with a balance greater than zero.", "Active Bill Not Found");
                return false;
            }
            else
            {
                //TS26 Target screen
                if(!RI.CheckForText(11,17," 0.00") && !RI.CheckForText(11,22,"CR"))
                {
                    RI.Hit(Key.Enter);
                    RI.Hit(Key.Enter);
                    RI.Hit(Key.F6);
                    if(RI.CheckForText(1,72,"T1X01"))
                    {
                        //billing screen not displayed
                        //if an active bill wasn't found
                        WhoaDUDE.ShowWhoaDUDE("Maui DUDE wasn't able to find an active bill to re-print.", "Active Bill Not Found");
                        return false;
                    }
                    else if(RI.CheckForText(1,72,"TSX15"))
                    {
                        //target screen 
                        if(RI.CheckForText(6,54,"A"))
                        {
                            RI.Hit(Key.F2);
                            KnarlyDUDE.ShowKnarlyDude("Processing Complete", "Processing Complete");
                            return true;
                        }
                        else
                        {
                            //if an active bill wasn't found
                            WhoaDUDE.ShowWhoaDUDE("Maui DUDE wasn't able to find an active bill to re-print.", "Active Bill Not Found");
                            return false;
                        }
                    }
                    else if(RI.CheckForText(1,72,"TSX14"))
                    {
                        //selection screen
                        //search for active bill
                        int subRow = 8;
                        while(!RI.CheckForText(23,2, "90007 NO MORE DATA TO DISPLAY"))
                        {
                            if(RI.CheckForText(subRow, 24, "A"))
                            {
                                RI.PutText(21, 12, RI.GetText(subRow, 2, 3), Key.Enter);       //select loan
                                RI.Hit(Key.F2);
                                KnarlyDUDE.ShowKnarlyDude("Processing Complete", "Processing Complete");
                                return true;
                            }
                        }
                        //if an active bill wasn't found
                        WhoaDUDE.ShowWhoaDUDE("Maui DUDE wasn't able to find an active bill to re-print.", "Active Bill Not Found");
                        return false;
                    }
                }
                else
                {
                    WhoaDUDE.ShowWhoaDUDE("Maui DUDE couldn't find a COMPASS loan with a balance greater than zero.", "Active Bill Not Found");
                    return false;
                }
            }
            return false;
        }

        public void CompleteProcessingThroughACP(string activityComment)
        {
            RI.Stup(_region, DataAccessHelper.CurrentMode);
            AcpSelectionResult selection = _borrower.AcpResponses.Selection;
            //TESTIT: does md only go back through queue for correct region?
            //calculate need values

            if (!selection.DescriptionValue.IsNullOrEmpty())
            {
                _borrower.AcpResponses.EntryInfo = DataAccess.DA.GetACPSelection(selection.ActivityCodeValue, selection.ContactCodeValue, selection.DescriptionValue);
            }
            else
            {
                _borrower.AcpResponses.EntryInfo = DataAccess.DA.GetACPSelection(selection.ActivityCodeValue, selection.ContactCodeValue);
            }

            AcpProcessor processor = new AcpProcessor(RI.ReflectionSession, SessionInteractionComponents.ProcessLogId);
            //if(!_borrower.CompassDemographics.UpdatedAlternateFormat.IsNullOrEmpty()) //This Implicitly only hits FED
            //{
            //    MdLettersHelper mdLettersHelper = new MdLettersHelper(Enumerable.Empty<Formats>());
            //    Formats format = new Formats();
            //    format.CorrespondenceFormatId = _borrower.CompassDemographics.UpdatedAlternateFormatId;
            //    mdLettersHelper.SetFormat(_borrower.CompassDemographics.AccountNumber, format);
            //    activityComment += " Alternate Format set to " + _borrower.CompassDemographics.UpdatedAlternateFormat + ".";
            //}
            processor.Process(activityComment, _borrower, _region); ;
        }

        /// <summary>
        /// Adds comment for ACH modification request
        /// </summary>
        public void AddComments(List<ActivityCommentToBeAdded> comments)
        {
            RI.Stup(_region, DataAccessHelper.CurrentMode);

            foreach(var comment in comments)
            {
                var results = RI.Atd22AllLoans(_borrower.SSN, comment.ARC, comment.Comment, "", SessionInteractionComponents.MAUI_DUDE_SCRIPT_ID, false);
                if(results == false)
                {
                    WhoaDUDE.ShowWhoaDUDE(string.Format("For some reason an activity comment using the {0} ARC could not be added.  Please notifiy the System Support Help Desk.", comment.ARC), "Activity Comment Add Error");
                }
            }
        }

        /// <summary>
        /// Returns the sum of all late fees from TD01
        /// </summary>
        public List<PaymentInfo> GetPaymentInformationFromSession()
        {
            List<PaymentInfo> paymentInfo = new List<PaymentInfo>();

            RI.Stup(_region, DataAccessHelper.CurrentMode);
            RI.FastPath($"TX3Z/ITD0L{_borrower.SSN}");
            if(!RI.CheckForText(1,72,"TDX0M"))
            {
                paymentInfo.Add(new PaymentInfo(0, 0, 0, 0));
            }
            else
            {
                RI.Hit(Key.F8);
                while(!RI.CheckForText(23,2,"90007"))
                {
                    decimal amtPastDue = RI.GetText(15, 33, 10).ToDecimal();
                    decimal totAmtDue = RI.GetText(15, 67, 10).ToDecimal();
                    decimal curAmtDue = totAmtDue - amtPastDue;
                    decimal totAmtDuePlusFees = totAmtDue + RI.GetText(16, 67, 10).ToDecimal();
                    paymentInfo.Add(new PaymentInfo(curAmtDue, amtPastDue, totAmtDue, totAmtDuePlusFees));
                    RI.Hit(Key.F8);
                }
            }

            return paymentInfo;
        }

        #region Repayment Options Calculator
        
        //Gather their current repayment plan
        public List<RepaymentDetail> GatherCurrentOption()
        {
            List<RepaymentDetail> details = new List<RepaymentDetail>();

            RI.Stup(_region, DataAccessHelper.CurrentMode);
            RI.FastPath($"TX3Z/ITS0N{_borrower.SSN}");
            if(RI.CheckForText(1,72,"TSX0Q"))
            {
                details = CheckTSX0Q(details, "");
            }
            else if(RI.CheckForText(1,72,"TSX0S"))
            {
                details = CheckTSX0S(details, "");
            }
            else if(RI.CheckForText(1,72,"TSX0P"))
            {
                details = CheckTSX0P(details, "");
            }
            else
            {
                return null;
            }

            if(RI.CheckForText(23,2,"20310") || details.Count == 0)
            {
                MessageBox.Show("There is no repayment schedule for this borrower", "No Repayment Schedule", MessageBoxButtons.OK);
                return null;
            }

            return details;
        }

        private List<RepaymentDetail> CheckTSX0Q(List<RepaymentDetail> details, string code)
        {
            if(RI.CheckForText(1,72,"TSX0Q"))
            {
                int row = 8;
                bool secondTime = false;
                while(!RI.CheckForText(row, 19, " ") && row < 20)
                {
                    RI.PutText(21, 13, RI.GetText(row, 19, 2), Key.Enter);
                    if(RI.CheckForText(1,72,"TSX0R"))
                    {
                        details = CheckTSX0R(details, code, secondTime);
                        secondTime = true;
                        RI.Hit(Key.F12);
                    }
                    else if(RI.CheckForText(1,72,"TSX0S"))
                    {
                        details = CheckTSX0S(details, code);
                    }
                    else
                    {
                        return null;
                    }
                    row++;
                }
            }
            return details;
        }

        private List<RepaymentDetail> CheckTSX0P(List<RepaymentDetail> details, string code)
        {
            if(RI.CheckForText(1,72,"TSX0P"))
            {
                int row = 8;
                bool secondTime = false;
                while(!RI.CheckForText(row, 5, " "))
                {
                    RI.PutText(21, 13, RI.GetText(row, 5, 1), Key.Enter);
                    if(RI.CheckForText(1,72,"TSX0Q"))
                    {
                        details = CheckTSX0Q(details, code);
                        secondTime = true;
                        RI.Hit(Key.F12);
                    }
                    else if(RI.CheckForText(1,72,"TSX0R"))
                    {
                        details = CheckTSX0R(details, code, secondTime);
                        secondTime = true;
                        RI.Hit(Key.F12);
                    }
                    else if(RI.CheckForText(23,2,"20310"))
                    {
                        RI.FastPath($"TX3Z/ITS0N{_borrower.SSN}");
                    }
                    row++;
                }
            }
            return details;
        }

        private List<RepaymentDetail> CheckTSX0R(List<RepaymentDetail> details, string code, bool secondTime)
        {
            List<string> typeList = new List<string>();
            int row = 10;
            string type = "";

            while(!RI.CheckForText(row, 3, " "))
            {
                type = RI.GetText(row, 39, 2);
                if(!typeList.Contains(type))
                {
                    typeList.Add(type);
                }
                row++;
                if(row == 22)
                {
                    RI.Hit(Key.F8);
                    row = 10;
                }
            }
            row = 10;

            for(int i = 1; i < typeList.Count; i++)
            {
                while(!RI.CheckForText(row,3," "))
                {
                    if(typeList.Count == 1)
                    {
                        RI.PutText(row, 3, "X");
                    }
                    else
                    {
                        if(RI.CheckForText(row,39,typeList[i - 1]))
                        {
                            RI.PutText(row, 3, "X");
                        }
                        else
                        {
                            RI.PutText(row, 3, " ");
                        }
                    }
                    row++;
                }

                RI.Hit(Key.Enter);
                details = GetPaymentFromSystem(secondTime, details, code);
                RI.Hit(Key.F12);
                row = 10;
            }
            return details;
        }

        private List<RepaymentDetail> CheckTSX0S(List<RepaymentDetail> details, string code)
        {
            if(RI.CheckForText(1,72,"TSX0S"))
            {
                int row = 11;
                while(!RI.CheckForText(row,3," "))
                {
                    RI.PutText(row, 3, " ");
                    RI.Hit(Key.Enter);
                    if(row == 11)
                    {
                        details = GetPaymentFromSystem(false, details, code);
                    }
                    else
                    {
                        details = GetPaymentFromSystem(true, details, code);
                    }
                    RI.Hit(Key.F12);
                    RI.Hit(Key.F12);
                    RI.PutText(row, 3, " ");
                    row++;
                }
            }
            else
            {
                return null;
            }
            RI.Hit(Key.F12);
            return details;
        }

        public List<RepaymentDetail> GatherData(string code)
        {
            List<RepaymentDetail> details = new List<RepaymentDetail>();

            RI.Stup(_region, DataAccessHelper.CurrentMode);
            RI.FastPath($"TX3Z/ATS0N{_borrower.SSN}");
            string loanProgram = RI.GetText(5, 15, 7);
            if(code == "G" && (loanProgram == "S/UNCNS" || loanProgram == "S/UNSPC" || loanProgram == "CNSLDN"))
            {
                return null;
            }
            //TODO verify
            //previously the loan program equality used ors but that didn't make sense so I changed it
            else if((code == "S2" || code == "S5") && (loanProgram != "S/UNCNS" && loanProgram != "CNSLDN" &&  loanProgram != "S/UNSPC"))
            {
                return null;
            }
            else
            {
                if(RI.CheckForText(1,72,"TSX0Q"))
                {
                    details = CheckTSX0Q(details, code);
                }
                else if(RI.CheckForText(1,72,"TSX0S"))
                {
                    details = CheckTSX0S(details, code);
                }
                else if(RI.CheckForText(1,72,"TSX0P"))
                {
                    details = CheckTSX0P(details, code);
                }
            }

            return details;
        }

        private List<RepaymentDetail> GetPaymentFromSystem(bool isDone, List<RepaymentDetail> details, string code)
        {
            int row = 14;
            int counter = 0;
            bool isDoneCreatingList = isDone;

            if(code != "")
            {
                RI.PutText(8, 14, code, Key.Enter);
                if(RI.CheckForText(23,2,"04341"))
                {
                    return details;
                }
            }
            do
            {
                if (!RI.CheckForText(row, 3, "   "))
                {
                    if (isDoneCreatingList)
                    {
                        details[counter].Amount += RI.GetText(row, 8, 10).ToDecimal(); 
                    }
                    else
                    {
                        details.Add(new RepaymentDetail(RI.GetText(row, 3, 3).TrimStart('0'), RI.GetText(row, 8, 10).ToDecimal(), RI.GetText(row, 29, 8)));
                    }
                }
                row++;
                counter++;
                if (row == 19 && RI.CheckForText(11, 78, "+"))
                {
                    RI.Hit(Key.F6);
                    RI.Hit(Key.F8);
                    if (!RI.CheckForText(11, 76, "-"))
                    {
                        RI.Hit(Key.F8);
                    }
                    row = 14;
                }
                else if (row == 19 && RI.CheckForText(3, 78, "+"))
                {
                    RI.Hit(Key.F6);
                    RI.Hit(Key.F8);
                    if (!RI.CheckForText(3, 76, "-"))
                    {
                        RI.Hit(Key.F6);
                        RI.Hit(Key.F8);
                    }
                    row = 14;
                    counter = 0;
                    isDoneCreatingList = true;
                }
            } while (row != 19 && (!RI.CheckForText(3, 76, "-") && !RI.CheckForText(3, 70, "MORE")));
            return details;
        }

        #endregion
    }
}

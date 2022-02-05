using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;
using System.Windows.Forms;
using Reflection;
using Microsoft.VisualBasic;
using System.IO;


namespace Payments
{
    public class Payments : ScriptBase
    {

        private TestModeResults _tModeResults;
        BrwInfo _brw;
        PossiblePaymentDate _selectedPaymentDate;
        MiscCommentAndQueueStuff _mcqs = new MiscCommentAndQueueStuff();

        /// <summary>
        /// Constructor For Borg Call
        /// </summary>
        /// <param name="ri"></param>
        public Payments(ReflectionInterface ri)
            : base(ri, "PMTARGMT")
        {
        }

        /// <summary>
        /// Constructor For DUDE Call
        /// </summary>
        /// <param name="ri"></param>
        public Payments(ReflectionInterface ri, MDBorrower mdBor, int runNum)
            : base(ri, "PMTARGMT", mdBor, runNum)
        {
        }

        /// <summary>
        /// main functionality starting point
        /// </summary>
        public override void Main()
        {
            _brw = new BrwInfo();
            //set directory vars
            _tModeResults = TestMode(@"X:\PADD\Collections\");
            //get needed information from MD if called by MD
            if (CalledByMauiDUDE)
            {
                _brw.BrwDemos = MauiDUDEBorrower.UserProvidedDemos;
                _mcqs.ActivityType = MauiDUDEBorrower.ActivityCode;
                _mcqs.ContactType = MauiDUDEBorrower.ContactCode;
            }


            //prompt the user for an SSN
            if (CalledByMauiDUDE == false)
            {
                bool tryAgain = true;
                while (tryAgain)
                {

                    InputBoxResults ssnRe = InputBox.ShowDialog("Enter SSN/Account#", "Payment Arrangements");
                    //check what the user selected
                    if (ssnRe.DialogRe != DialogResult.OK)
                    {
                        //if user clicked cancel then end the script
                        EndDLLScript();
                    }
                    try
                    {
                        _brw.BrwDemos = GetDemographicsFromLP22(ssnRe.UserProvidedText);
                        _brw.SSN = _brw.BrwDemos.SSN;
                        tryAgain = false;
                    }
                    catch (DemographicRetrievalException)
                    {
                        //try again won't be switched to false so it will lopp again for another SSN or account #
                    }
                }
            }
            else
            {
                _brw.BrwDemos = MauiDUDEBorrower.UserProvidedDemos;
                _brw.SSN = MauiDUDEBorrower.SSN;
            }
            //prompt the user for the payment amount
            InputBoxResults pmtipRe = InputBox.ShowDialog("Enter the proposed payment amount", "Enter Payment Amount");
            //end if nothing is entered or the dialog box is canceled
            if (pmtipRe.DialogRe != DialogResult.OK || string.IsNullOrEmpty(pmtipRe.UserProvidedText))
            {
                //end script if user didn't select OK
                EndDLLScript();
            }
            //set the variable based on the amount input
            _brw.APmt = double.Parse(pmtipRe.UserProvidedText);
            //go to the ChkElig subroutine to check for eligibility
            ChkElig();

            //go to LC18 to determine the reinstatement status
            FastPath("LC18I" + _brw.SSN);
            _brw.RehabPmts = int.Parse(GetText(18, 51, 3));
            _brw.MinPmtOvrd = "N";
            Analyze();
            CalcN();
        }

        private void ChkExtendedPaymentElig()
        {
            string earlyDsbDt = string.Empty;
            string cluid1 = string.Empty;
            List<string> aCLUID = new List<string>();
            FastPath(string.Format("LG0HI;{0}", _brw.SSN));
            if (Check4Text(1, 53, "DISBURSEMENT ACTIVITY SELECT"))
            {
                PutText(21, 11, "01", ReflectionInterface.Key.Enter);
            }
            while (Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") == false)
            {
                //get the first disbursement date for the loan if it is not blank
                if (Check4Text(12, 20, "MM") == false)
                {
                    earlyDsbDt = string.Format("{0}/{1}/{2}", GetText(12, 20, 2), GetText(12, 22, 2), GetText(12, 24, 4));
                    if (DateTime.Parse(earlyDsbDt) < DateTime.Parse("10/07/1998"))
                    {
                        aCLUID.Add(GetText(5, 25, 19));
                    }
                }
                Hit(ReflectionInterface.Key.F8);
            }

            //if no disbursement date was found, warn the user and end the script
            if (earlyDsbDt == string.Empty)
            {
                MessageBox.Show("The borrower is not eligible for extended repayment.", "Borrower not Eligible", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                EndDLLScript();
            }

            //check LG10 for loan status
            if (aCLUID.Count > 0)
            {
                FastPath("LG10I" + _brw.SSN);
                if (Check4Text(1, 53, "LOAN BWR STATUS RECAP SELECT"))
                {
                    while (Check4Text(20, 3, "46004 NO MORE DATA TO DISPLAY") == false)
                    {
                        for (int x = 7; x < 18; x++)
                        {
                            if (GetText(x, 5, 2) != "")
                            {
                                PutText(19, 15, GetText(x, 5, 2).PadLeft(2, "0".ToCharArray()[0]), ReflectionInterface.Key.Enter);
                                while (Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") == false)
                                {
                                    for (int x2 = 11; x2 < 20; x2++)
                                    {
                                        if (GetText(x2, 4, 19) != string.Empty)
                                        {
                                            for (int x3 = 0; x3 < aCLUID.Count; x3++)
                                            {
                                                if (Check4Text(x2, 4, aCLUID[x3]) && Check4Text(x2, 59, "PN") == false && Check4Text(x2, 59, "PF") == false && Check4Text(x2, 59, "PM") == false)
                                                {
                                                    _brw.ExtendedPaymentElig = false;
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                    Hit(ReflectionInterface.Key.F8);
                                }
                                Hit(ReflectionInterface.Key.F12);
                            }
                        }
                        Hit(ReflectionInterface.Key.F8);
                    }
                }
                else
                {
                    while (Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") == false)
                    {
                        for (int x2 = 11; x2 < 20; x2++)
                        {
                            for (int x3 = 0; x2 < aCLUID.Count - 1; x2++)
                            {
                                if (Check4Text(x2, 4, aCLUID[x3]) && Check4Text(x2, 59, "PN") == false && Check4Text(x2, 59, "PF") == false && Check4Text(x2, 59, "PM") == false)
                                {
                                    _brw.ExtendedPaymentElig = false;
                                    return;
                                }
                            }
                        }
                        Hit(ReflectionInterface.Key.F8);
                    }
                }

            }
        }


        //analyse the amount entered and determine the appropriate type of arrangement and call the appropriate subroutines to establish the arrangement
        private void Analyze()
        {
            //access LC05
            FastPath("LC05I" + _brw.SSN);
            //get the account balance and monthly interest
            _brw.ABal = GetText(4, 69, 12);
            _brw.MoInt = GetText(4, 43, 10);
            _brw.BilledAmt = GetText(3, 16, 10);
            _brw.Balance = GetText(4, 69, 12);
            _brw.NextDue = "10021968";    //10/02/68 used to denote a blank date
            _brw.MinEnd = "10/02/1968";   //10/02/68 used to denote a blank date as a blank date is considered in the past and the variable needs a date value to perform date calcs
            _brw.UpdateNextDue = "N";
            _brw.AmtLastPay = 0;
            RS.TransmitANSI("01");
            Hit(ReflectionInterface.Key.Enter);
            while (Check4Text(22, 3, "46004") == false)
            {
                if (Check4Text(4, 10, "03") && Check4Text(4, 26, "05") && Check4Text(19, 73, "MMDDCCYY"))
                {
                    Hit(ReflectionInterface.Key.F10);
                    string defStatus = GetText(17, 5, 20);
                    MessageBox.Show(string.Format("The borrower has an open loan in a deferred status of {0}. Payment arrangements cannot be made.", defStatus), "Invalid Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (MessageBox.Show("Would you like to create a DSUPCALL queue task?", "Create Queue Task", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        AddQueueTaskInLP9O(_brw.SSN, "DSUPCALL", DateTime.Today.ToShortDateString(), string.Format("borrower requested payment amount of ${0} but one or more loans are deferred", _brw.APmt.ToString()), "", "", "");
                    }
                    else
                    {
                        MessageBox.Show("A queue task will not be created. Before running the payment arrangement script again the deferment must be resolved.", "Resolve Deferment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    EndDLLScript();
                }
                else if (Check4Text(4, 10, "03") && Check4Text(4, 26, "  ") == false && Check4Text(19, 73, "MMDDCCYY"))
                {
                    MessageBox.Show(string.Format("The borrower has an open loan in a {0} status so payment arrangements cannot be made.", GetText(4, 29, 10)), "Payment Arrangements Cannot Be Made", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    EndDLLScript();
                }
                else if (Check4Text(4, 10, "03") && Check4Text(19, 73, "MMDDCCYY"))
                {
                    _brw.ClaimID.Add(GetText(21, 11, 4));
                    _brw.WT.Add(double.Parse(GetText(20, 32, 12)) / double.Parse(_brw.ABal));
                    _brw.ARate = _brw.ARate + (double.Parse(GetText(6, 8, 6)) * _brw.WT.Last<double>());
                    //if the next due is blank, the update next due needs to be updated
                    if (Check4Text(10, 73, "MMDDCCYY"))
                    {
                        _brw.UpdateNextDue = "Y";
                        _brw.NextDue = "10021968";
                        //if the next due for the loan does not equal the next due for the previous loan, the next due needs to be updated so all loans have the same due date
                    }
                    else if (Check4Text(10, 73, _brw.NextDue) == false && _brw.NextDue != "10021968")
                    {
                        _brw.UpdateNextDue = "Y";
                        //if the next due is not blank and it equals the next due of the previous loan, get the value
                    }
                    else
                    {
                        _brw.NextDue = GetText(10, 73, 8);
                    }
                    if (Check4Text(14, 73, "MMDDCCYY"))
                    {
                        _brw.LastPayDate = string.Empty;
                    }
                    else
                    {
                        _brw.LastPayDate = GetText(14, 73, 8).ToDateFormat();
                    }
                    _brw.AmtLastPay = _brw.AmtLastPay + double.Parse(GetText(15, 73, 8));
                    //get the most recent minimum payment arrangement end date
                    //go to page two
                    Hit(ReflectionInterface.Key.F10);
                    //get the date and format it as a date if it is not blank and if it is more recent than the current most recent end date
                    if (Check4Text(21, 11, "MMDDCCYY") == false)
                    {
                        if (DateTime.Parse(GetText(21, 11, 8).ToDateFormat()) > DateTime.Parse(_brw.MinEnd))
                        {
                            _brw.MinEnd = GetText(21, 11, 8).ToDateFormat();
                        }
                    }
                }
                Hit(ReflectionInterface.Key.F8);
            }

            if (_brw.ClaimID.Count == 0)
            {
                MessageBox.Show("The borrower does not have any loans eligible for payment arrangements.", "No Eligible Loans", MessageBoxButtons.OK, MessageBoxIcon.Information);
                EndDLLScript();
            }
            _brw.NextDue = _brw.NextDue.ToDateFormat();
            _brw.CurrExpPmt = ExpPmt();

            //Check eligibility for extended payments
            ChkExtendedPaymentElig();

        }


        private void CalcN()
        {
            bool continueProc = true;
            long nopmts = 0;
            int yrs = 0;
            long mos = 0;
            InputBoxResults pmtip;
            //calculate the number of payments
            while (true)
            {
                try
                {
                    //calculate term
                    nopmts = long.Parse(Math.Round(Financial.NPer((_brw.ARate / 100 / 12), -_brw.APmt, double.Parse(_brw.ABal), 0, DueDate.EndOfPeriod), 0).ToString());
                    yrs = (int)(nopmts / 12);
                    mos = (nopmts - (yrs * 12));
                    if (nopmts > 300)
                    {
                        //number of payments to pay off should be less than 300 if possible
                        throw new System.Exception("Number of payments must be less than 300.");
                    }
                    continueProc = true;
                }
                catch (Exception)
                {
                    //warn the user if the payment amount is too low to calculate the payoff information
                    pmtip = InputBox.ShowDialog("The proposed payment amount is too low to calculate the number of payments required to pay off the loan.  (1) Click OK to continue with the current payment amount, OR (2) update the proposed payment amount below and click OK to review another amount, OR (3) click Cancel to quit.", "Payment Approved for Income Sensitive Repayment ", _brw.APmt.ToString());
                    if (pmtip.DialogRe == DialogResult.Cancel)
                    {
                        EndDLLScript();
                    }
                    else if (double.Parse(pmtip.UserProvidedText) == _brw.APmt)
                    {
                        nopmts = 999;
                        continueProc = true;
                    }
                    else
                    {
                        _brw.APmt = double.Parse(pmtip.UserProvidedText);
                        continueProc = false;
                    }
                }

                if (continueProc)
                {
                    if (nopmts > 120)
                    {
                        if (double.Parse(_brw.ABal) < 31000 || (double.Parse(_brw.ABal) > 30000 && nopmts < 240 && openLoansPrior()))
                        {
                            pmtip = InputBox.ShowDialog("The proposed payment amount may be accepted on a Income Sensitive Repayment arrangement.  (1) Click OK to enter the payment arrangement and print a letter for the borrower, OR (2) update the proposed payment amount below and click OK to review another amount, OR (3) click Cancel to quit.", "Payment Approved for Income Sensitive Repayment ", _brw.APmt.ToString());
                            if (pmtip.DialogRe == DialogResult.Cancel)
                            {
                                EndDLLScript();
                            }
                            else if (double.Parse(pmtip.UserProvidedText) == _brw.APmt)
                            {
                                _brw.ArngTyp = "P";
                                PreProc();
                                ISR();
                                EndDLLScript();
                            }
                            else
                            {
                                _brw.APmt = double.Parse(pmtip.UserProvidedText);
                            }
                        }
                        else
                        {
                            if (_brw.ExtendedPaymentElig == false)
                            {
                                MessageBox.Show("This borrower is not eligible for extended payments. The Balance Sensitive Repayment is not an option.", "Not Eligible", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            pmtip = InputBox.ShowDialog("The proposed amount may be accepted on a Balance Sensitive Repayment arrangement.  (1) Click OK to enter the payment arrangement and print a letter for the borrower, OR (2) update the proposed payment amount below and click OK to review another amount, OR (3) click Cancel to quit.", "Payment Approved for Balance Sensitive Repayment", _brw.APmt.ToString());
                            if (pmtip.UserProvidedText == string.Empty)
                            {
                                EndDLLScript();
                            }
                            else if (double.Parse(pmtip.UserProvidedText) == _brw.APmt)
                            {
                                _brw.ArngTyp = "P";
                                PreProc();
                                BSR();
                                EndDLLScript();
                            }
                            else
                            {
                                _brw.APmt = double.Parse(pmtip.UserProvidedText);
                            }
                        }
                    }
                    else
                    {
                        pmtip = InputBox.ShowDialog(string.Format("The proposed payment amount of ${0} will pay the outstanding balance in {1} years and {2} months so the proposed amount may be accepted on a permanent basis.  (1) Click OK to enter the payment arrangement and print a letter for the borrower, OR (2) update the proposed payment amount below and click OK to review another amount, OR (3) click Cancel to quit.", _brw.APmt.ToString("###,##0.00"), yrs, mos), "Payment Approved for Permanent Arrangement", _brw.APmt.ToString("###,##0.00"));
                        if (pmtip.DialogRe == DialogResult.Cancel)
                        {
                            EndDLLScript();
                        }
                        else if (double.Parse(pmtip.UserProvidedText) == _brw.APmt)
                        {
                            _brw.ArngTyp = "P";
                            PreProc();
                            Perm();
                            EndDLLScript();
                        }
                        else
                        {
                            _brw.APmt = double.Parse(pmtip.UserProvidedText);
                        }
                    }
                }
            }
        }

        private bool openLoansPrior()
        {
            FastPath("LG0HI" + _brw.SSN);
            if (Check4Text(1, 53, "DISBURSEMENT ACTIVITY SELECT"))
            {
                PutText(21, 11, "01", ReflectionInterface.Key.Enter);
            }
            while (Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") == false)
            {
                if (GetText(12, 20, 2) != "MM")
                {
                    if (DateTime.Parse(string.Format("{0}/{1}/{2}", GetText(12, 20, 2), GetText(12, 22, 2), GetText(12, 24, 4))) < DateTime.Parse("10/07/1998"))
                    {
                        return true;
                    }
                }
                Hit(ReflectionInterface.Key.F8);
            }
            return false;
        }

        //pre arrangement processing processing
        private void PreProc()
        {
            Paymnts paymntFrm = new Paymnts();
            while (_selectedPaymentDate == null)
            {
                //prompt the user to enter the due date the borrower will make the first payment
                if (paymntFrm.ShowDialog() == DialogResult.Cancel)
                {
                    EndDLLScript();
                }
                else
                {
                    _selectedPaymentDate = paymntFrm.SelectedPaymentDue;
                    if (_selectedPaymentDate == null)
                    {
                        MessageBox.Show("You must select a month.", "Month Selection Needed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

        }

        //control flow of script through subroutines necessary to produce permanent payment arrangement
        private void Perm()
        {
            _brw.BillCd = "01";
            LC34();
            _brw.HoldDate = _selectedPaymentDate.ActualDate.AddDays(7);
            LC18();
            LC05();
            _mcqs.Comment = string.Format("perm arrangement made with borrower for ${0} beginning {1}, entered type 11 hold on LC18 which expires {2}, mailed arrangement letter to borrower; ", _brw.APmt.ToString("###,##0.00"), _selectedPaymentDate.ActualDate.ToString("MM/dd/yyyy"), _brw.HoldDate.ToString("MM/dd/yyyy"));
            _mcqs.ActCd = "DD136";
            LP50();
            VbaStyleFileOpen(DataAccess.PersonalDataDirectory + "cldat.txt", 1, Common.MSOpenMode.Output);
            VbaStyleFileWriteLine(1, "SSN", "AccountNumber", "KeyLine", "FirstName", "LastName", "Address1", "Address2", "City", "State", "Country", "ZIP", "BilledAmt", "DueDay", "DueDate", "StartDate", "ExpDate", "RedAmt", "FirstRegDue", "ResponseDate", "LastPayDate", "LastPayAmt", "Balance");
            VbaStyleFileWriteLine(1, _brw.SSN, _brw.BrwDemos.AccountNumber, ACSKeyLine(_brw.SSN, Common.ACSKeyLinePersonType.Borrower, Common.ACSKeyLineAddressType.Legal), _brw.BrwDemos.FName, _brw.BrwDemos.LName, _brw.BrwDemos.Addr1, _brw.BrwDemos.Addr2, _brw.BrwDemos.City, _brw.BrwDemos.State, (_brw.BrwDemos.Country == null ? string.Empty : _brw.BrwDemos.Country), _brw.BrwDemos.Zip, double.Parse(_brw.BilledAmt).ToString("###,##0.00"), _selectedPaymentDate.CalculatedDueDay(), _selectedPaymentDate.ActualDate.ToString("MM/dd/yyyy"), "", "", _brw.APmt.ToString("###,##0.00"), _selectedPaymentDate.ActualDate.AddMonths(6).ToString("MM/dd/yyyy"), DateTime.Today.AddDays(15).ToString("dddd, MMMM dd, yyyy"), (_brw.LastPayDate == string.Empty ? string.Empty : DateTime.Parse(_brw.LastPayDate).ToString("MM/dd/yyyy")), _brw.AmtLastPay.ToString("###,##0.00"), double.Parse(_brw.Balance).ToString("###,##0.00"));
            VbaStyleFileClose(1);
            GiveMeItAll_LtrEmlFax_BarCd_StaCurDt("PMTS", DocumentHandling.CentralizedPrintingSystemToAddComments.stacOneLINK, _brw.SSN, DataAccess.PersonalDataDirectory + "cldat.txt", "AccountNumber", "PMTS", _tModeResults.DocFolder, "State", DocumentHandling.CentralizedPrintingDeploymentMethod.dmUserPrompt, "03", DocumentHandling.Barcode2DLetterRecipient.lrBorrower);
            File.Delete(DataAccess.PersonalDataDirectory + "cldat.txt");
        }

        //control flow of script through subroutines necessary to produce temp reduced payment arrangement
        private void ISR()
        {
            _brw.BillCd = "11";
            LC34();
            _brw.HoldDate = _selectedPaymentDate.ActualDate.AddDays(7);
            _brw.StartDate = DateTime.Today;
            _brw.ExpDate = _brw.StartDate.AddMonths(6);
            LC18();
            _mcqs.Queue = "DPYMTRVW";
            _mcqs.DateDue = DateTime.Today.AddDays(15).ToString("MM/dd/yyyy");
            _mcqs.LP9OComment = "Borrower setup on ISR payment arrangement, following up to verify that a Family Budget Statement was returned.";
            LP9O();
            _mcqs.Comment = string.Format("pmt arrgmnt made w/borr; for ${0} task in DPYMTRVW for {1}; LC18 hld 11 exp {2}; LC34: bill sta to 11, bill amt to ${0}, date due to {3}; letter to borr", _brw.APmt, DateTime.Today.AddDays(30).ToString("MM/dd/yyyy"), _brw.HoldDate, _selectedPaymentDate.ActualDate);
            _mcqs.ActCd = "DD136";
            LP50();
            SendIBSRPMTS(false);
        }

        private void BSR()
        {
            _brw.BillCd = "11";
            LC34();
            _brw.HoldDate = _selectedPaymentDate.ActualDate.AddDays(7);
            _brw.StartDate = DateTime.Today;
            _brw.ExpDate = _brw.StartDate.AddMonths(6);
            LC18();
            _mcqs.Comment = string.Format("pmt arrgmnt made w/borr; for ${0} task in DPYMTRVW for {1}; LC18 hld 11 exp {2}; LC34: bill sta to 11 bill amt to ${0}, date due to {3}; letter to borr", _brw.APmt, _selectedPaymentDate.ActualDate.AddDays(30).ToString("MM/dd/yyyy"), _brw.HoldDate, _selectedPaymentDate.ActualDate.AddDays(30).ToString("MM/dd/yyyy"));
            _mcqs.ActCd = "DD136";
            LP50();
            SendIBSRPMTS(true);
        }


        private void SendIBSRPMTS(bool billOnly)
        {
            _brw.ArngTyp = "F";
            _brw.UpdateBillAmt = "N";
            _brw.UpdateNextDue = "N";
            _brw.HoldDate = DateTime.Today.AddDays(30);
            _brw.BillCd = "01";
            _mcqs.Comment = string.Format("mailed FBS and bill to borrower, entered type '11' hold on LC18 which expires {0}, updated billing status to '01' on LC34", _brw.HoldDate);
            LC05();
            //Per SR 2986 I think the next two lines are suppose to be deleted???
            //LC18();
            //LC34();
            _mcqs.ActCd = "DD014";
            AddCommentInLP50(_brw.SSN, _mcqs.ActCd, _mcqs.ActivityType, _mcqs.ContactType, _mcqs.Comment, true, false);
            VbaStyleFileOpen(DataAccess.PersonalDataDirectory + "cldat.txt", 1, Common.MSOpenMode.Output);
            VbaStyleFileWriteLine(1, "SSN", "AccountNumber", "KeyLine", "FirstName", "LastName", "Address1", "Address2", "City", "State", "Country", "ZIP", "BilledAmt", "DueDay", "DueDate", "StartDate", "ExpDate", "RedAmt", "FirstRegDue", "ResponseDate", "LastPayDate", "LastPayAmt", "Balance", "StaticCurrentDate");
            VbaStyleFileWriteLine(1, _brw.SSN, _brw.BrwDemos.AccountNumber, ACSKeyLine(_brw.BrwDemos.SSN, Common.ACSKeyLinePersonType.Borrower, Common.ACSKeyLineAddressType.Legal), _brw.BrwDemos.FName, _brw.BrwDemos.LName, _brw.BrwDemos.Addr1, _brw.BrwDemos.Addr2, _brw.BrwDemos.City, _brw.BrwDemos.State, (_brw.BrwDemos.Country == null ? string.Empty : _brw.BrwDemos.Country), _brw.BrwDemos.Zip, double.Parse(_brw.BilledAmt).ToString("###,##0.00"), _selectedPaymentDate.CalculatedDueDay(), _selectedPaymentDate.ActualDate.ToString("MM/dd/yyyy"), _brw.StartDate.ToString("MM/dd/yyyy"), _brw.ExpDate.ToString("MM/dd/yyyy"), _brw.APmt.ToString("###,##0.00"), _selectedPaymentDate.ActualDate.AddMonths(6).ToString("MM/dd/yyyy"), DateTime.Today.AddDays(15).ToString("dddd, MMMM dd, yyyy"), _brw.LastPayDate, _brw.AmtLastPay.ToString("###,##0.00"), double.Parse(_brw.Balance).ToString("###,##0.00"), DateTime.Today.ToString("MMMM dd, yyyy"));
            VbaStyleFileClose(1);
            if (billOnly)
            {
                GiveMeItAll_LtrEmlFax_BarCd_StaCurDt("PMTS", DocumentHandling.CentralizedPrintingSystemToAddComments.stacOneLINK, _brw.SSN, DataAccess.PersonalDataDirectory + "cldat.txt", "AccountNumber", "IBSRPMTS", _tModeResults.DocFolder, "State", DocumentHandling.CentralizedPrintingDeploymentMethod.dmUserPrompt, "03", DocumentHandling.Barcode2DLetterRecipient.lrBorrower);
            }
            else
            {
                GiveMeItAll_LtrEmlFax_BarCd_StaCurDt("IBSRPMTS", DocumentHandling.CentralizedPrintingSystemToAddComments.stacOneLINK, _brw.SSN, DataAccess.PersonalDataDirectory + "cldat.txt", "AccountNumber", "IBSRPMTS", _tModeResults.DocFolder, "State", DocumentHandling.CentralizedPrintingDeploymentMethod.dmUserPrompt, "03", DocumentHandling.Barcode2DLetterRecipient.lrBorrower);
            }
            File.Delete(DataAccess.PersonalDataDirectory + "cldat.txt");
        }

        //update the promised to pay date and minimum pmt info in LC05 for each loan
        private void LC05()
        {
            //access LC05
            FastPath("LC05C" + _brw.SSN);
            PutText(21, 13, "01", ReflectionInterface.Key.Enter);
            while (Check4Text(22, 3, "46004") == false)
            {
                //check if claim is in the list
                if (_brw.ClaimID.Where(p => p == GetText(21, 11, 4)).Select(p => p) != null)
                {
                    //go to page 2
                    Hit(ReflectionInterface.Key.F10);
                    //go to page 3 and check the coll cost flag to see if the date prom to pay needs to be updated
                    Hit(ReflectionInterface.Key.F10);
                    PutText(9, 26, _selectedPaymentDate.ActualDate.ToString("MMddyyyy"), ReflectionInterface.Key.Enter);
                }
                //go to the next loan
                Hit(ReflectionInterface.Key.F8);
            }
        }

        //enter a hold on LC18
        private void LC18()
        {
            //access LC18
            FastPath(string.Format("LC18C{0}", _brw.SSN));
            //update REPAYMENT PLAN indicator 
            PutText(14, 72, "Y");
            //enter the current employer if there is one
            if (_brw.EmployerID != string.Empty)
            {
                PutText(14, 29, _brw.EmployerID);
            }
            //enter the hold expiration date
            PutText(15, 12, _brw.HoldDate.ToString("MMddyyyy"));
            //enter hold reason 11
            RS.TransmitANSI("11");
            Hit(ReflectionInterface.Key.Enter);
        }


        //enter the payment arrangement info in LC34
        private void LC34()
        {
            //access LC34
            FastPath(string.Format("LC34C{0}", _brw.SSN));
            RS.TransmitANSI("01");
            Hit(ReflectionInterface.Key.Enter);
            //enter the billing status code
            PutText(4, 10, _brw.BillCd);

            //update the billed amount if it needs to be updated and the min pmt end date is a past date
            PutText(4, 21, _brw.APmt.ToString("###,##0.00"), ReflectionInterface.Key.EndKey);
            _mcqs.Comment2 = string.Format("{0}, bill amt to ${1}", _mcqs.Comment2, _brw.APmt);
            //update the due date
            if (_brw.NextDue == string.Empty) _brw.NextDue = _selectedPaymentDate.ActualDate.ToString("MM/dd/yyyy");
            if (_brw.UpdateNextDue == "Y" || DateTime.Parse(_brw.NextDue).Day != _selectedPaymentDate.ActualDate.Day)
            {
                //add a month if the due date is within the next 14 days
                if (DateTime.Today.AddDays(15) >= _selectedPaymentDate.ActualDate)
                {
                    _brw.LC34DueDate = _selectedPaymentDate.ActualDate.AddMonths(1).ToString("MMddyyyy");
                }
                else
                {
                    _brw.LC34DueDate = _selectedPaymentDate.ActualDate.ToString("MMddyyyy");
                }
                PutText(4, 42, _brw.LC34DueDate);
                _mcqs.Comment2 = string.Format("{0}, date due to {1}", _mcqs.Comment2, _brw.LC34DueDate.ToDateFormat());
            }
            int row = 9;
            int counter;
            while (true)
            {
                counter = 0;
                while (counter < _brw.WT.Count)
                {
                    if (Check4Text(row, 5, _brw.ClaimID[counter]))
                    {
                        PutText(row, 3, "X");
                        break;
                    }
                    counter++;
                }
                row++;
                //if the row is blank, forward to the next page, if no more pages are displayed, stop processing
                if (Check4Text(row, 3, " "))
                {
                    Hit(ReflectionInterface.Key.F8);
                    if (Check4Text(22, 3, "46004"))
                    {
                        break;
                    }
                    else
                    {
                        row = 9;
                    }
                }
            }
            Hit(ReflectionInterface.Key.Enter);
        }

        //enter a follow-up tasks
        private void LP9O()
        {
            FastPath(string.Format("LP9OA;;{0}", _mcqs.Queue));
            //prompt user if unable to add task
            if (Check4Text(22, 3, "44000") == false)
            {
                MessageBox.Show("Unable to add task.  Wait for the script to finish and then enter the task manually.", "Add Task Manually Later", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //enter info
            else
            {
                PutText(11, 25, DateTime.Parse(_mcqs.DateDue).ToString("MMddyyyy"));
                PutText(16, 12, _mcqs.LP9OComment);
                if (MessageBox.Show("Do you want to enter additional comments?  Click Yes to pause the script, add the additional comments, and then hit <Insert> to resume the script.  Click No to continue processing without adding additional comments.", "Add Additional Comments", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    PauseForInsert();
                }
                Hit(ReflectionInterface.Key.Enter);
                Hit(ReflectionInterface.Key.F6);
                RS.Wait("2"); //go figure why this has to be here but it wouldn't work otherwise
            }
        }


        //enter an activity record in LP50
        private void LP50()
        {
            //access LP50
            FastPath("LP50A" + _brw.SSN);
            RS.TransmitANSI(_mcqs.ActCd);
            Hit(ReflectionInterface.Key.Enter);
            //pause for the user to enter the contact and activity type
            if (_mcqs.ActivityType == string.Empty)
            {
                PutText(7, 2, InputBox.ShowDialog("Enter Contact Type:", "Contact Type").UserProvidedText);
                PutText(7, 5, InputBox.ShowDialog("Enter Activity Code:", "Activity Code").UserProvidedText);
            }
            else
            {
                PutText(7, 5, _mcqs.ContactType);
                PutText(7, 2, _mcqs.ActivityType);
            }
            //enter the comment
            PutText(13, 2, string.Format("{0}{1}{2}", _mcqs.FBSComment, _mcqs.Comment, _mcqs.Comment2));
            if (MessageBox.Show("Do you want to enter additional comments?  Click Yes to pause the script, add the additional comments, and then hit <Insert> to resume the script.  Click No to continue processing without adding additional comments.", "Add Additional Comments", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                PauseForInsert();
            }
            int row = 13;
            int col = 2;
            while (row != 21)
            {
                if (Check4Text(row, col, "____________"))
                {
                    PutText(row, col, ScriptID);
                    break;
                }
                else if (col == 62)
                {
                    row++;
                    col = 2;
                }
                else
                {
                    col++;
                }
            }
            Hit(ReflectionInterface.Key.F6);
        }

        //review the account for eligibility to have a payment arrangement established
        private void ChkElig()
        {
            _mcqs.ActCd = "DSBNP";
            LoclEmplVer();
            //AWG record
            FastPath(string.Format("LC67I{0}GG", _brw.SSN));
            if (Check4Text(22, 3, "47004"))
            {
            }
            else
            {
                if (Check4Text(21, 3, "SEL"))
                {
                    RS.TransmitANSI("01");
                    Hit(ReflectionInterface.Key.Enter);
                }
                while (Check4Text(22, 3, "46004") == false)
                {
                    if (Check4Text(8, 19, "  ") == false || Check4Text(16, 63, "  ") == false)
                    {
                        Hit(ReflectionInterface.Key.F8);
                    }
                    else
                    {
                        if (MessageBox.Show("The borrower has an open AWG record. Advise the borrower that the garnishment will not be released. The payment arrangement will be established in addition to the garnishment.", "Open AWG Record", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                        {
                            //Cancel
                            EndDLLScript();
                        }
                        else
                        {
                            //OK
                            return;
                        }
                    }
                }
            }
            //EX record
            FastPath(string.Format("LC67I{0}EX", _brw.SSN));
            if (Check4Text(22, 3, "47004") == false)
            {
                if (MessageBox.Show("The borrower has an open judicial garnishment.  Advise the borrower that the garnishment will not be released. The payment arrangement will be established in addition to the garnishment.", "Open Garnishment", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                {
                    //Cancel
                    EndDLLScript();
                }
                else
                {
                    //OK
                    return;
                }
            }
            if (_brw.EmployerID != string.Empty)
            {
                FastPath("LC41I" + _brw.SSN);
                PutText(7, 36, "X", ReflectionInterface.Key.Enter);
                if (Check4Text(21, 3, "SEL"))
                {
                    int row = 7;
                    while (((DateTime.Parse(GetText(row, 5, 8).ToDateFormat())) >= (DateTime.Today.AddDays(-45))) && (Check4Text(22, 3, "46004") == false))
                    {
                        if (Check4Text(row, 34, "BR") || Check4Text(row, 34, "CS") || Check4Text(row, 34, "EP"))
                        {
                            return;
                        }
                        else
                        {
                            row++;
                            if (Check4Text(row, 34, "  "))
                            {
                                row = 7;
                                Hit(ReflectionInterface.Key.F8);
                            }
                        }
                    }
                    while (((DateTime.Parse(GetText(row, 5, 8).ToDateFormat())) >= (DateTime.Today.AddDays(-45))) && (Check4Text(22, 3, "46004") == false))
                    {
                        if (Check4Text(row, 34, "BR") || Check4Text(row, 34, "CS") || Check4Text(row, 34, "EP"))
                        {
                            return;
                        }
                        else
                        {
                            row++;
                            if (Check4Text(row, 34, "  "))
                            {
                                row = 7;
                                Hit(ReflectionInterface.Key.F8);
                            }
                        }
                    }
                }
                FastPath("LP50I" + _brw.SSN);
                PutText(9, 20, "DD136");
                PutText(18, 29, DateTime.Today.AddDays(-180).ToString("MMddyyyy") + DateTime.Today.ToString("MMddyyyy"), ReflectionInterface.Key.Enter);
                if (Check4Text(3, 2, "SEL"))
                {
                    string owedate = string.Empty;
                    FastPath("LC18I" + _brw.SSN);
                    if (Check4Text(14, 9, "MMDDCCYY"))
                    {
                        owedate = DateTime.Today.AddDays(30).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        owedate = DateTime.Parse(GetText(14, 9, 8).ToDateFormat()).AddDays(30).ToString("MM/dd/yyyy");
                    }
                    MessageBox.Show(string.Format("The borrower has made more than one payment arrangement in the last 180 days, has a current employer, and has not kept the payment arrangements made.  Therefore, another payment arrangement cannot be made until the borrower makes a payment.  Inform the borrower that an order to withhold wages will be sent to his or her employer on or about {0} and he or she will need to make a payment before then to avoid wage withholding.  Once the payment has been made, the borrower may call back to make payment arrangements.", owedate), "Payment Arrangements not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (MessageBox.Show("Would you like to run the Check by Phone Script?", "Check By Phone Script", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // run onelink check by phone script
                        OneLinkCheckByPhn.OneLinkCheckByPhn olCkByPhnScrp = new OneLinkCheckByPhn.OneLinkCheckByPhn(RI);
                        olCkByPhnScrp.Main();
                    }
                    EndDLLScript();
                }
            }
        }

        private void LoclEmplVer()
        {
            //access LC20
            FastPath("LC20I" + _brw.SSN);
            //get the current employer if there is one or prompt the user to select one
            if (Check4Text(1, 59, "EMPLOYMENT INFORMATION") && Check4Text(10, 5, " ") == false && Check4Text(10, 5, "2000") == false)
            {
                _brw.EmployerID = GetText(10, 12, 8);
                //access LPEM to get the employer name
                FastPath("LPEMI" + _brw.EmployerID);
                //prompt the user to verify that the correct employer is displayed
                DialogResult re = MessageBox.Show("Is this the correct employer?  Click Yes to continue, No to select another employer, or Cancel to add a task to the DEMPA queue and add an activity record indicating you attempted to obtain current employer information but the borrower is not employed.", "Employer", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (re == DialogResult.No)
                {
                    LoclEmplResearch();
                }
                else if (re == DialogResult.Cancel)
                {
                    EmplNLE();
                }
            }
            else
            {
                DialogResult re = MessageBox.Show("No current employer was found.  Is the borrower employed?  Click Yes to go to LPEM and search for an employer or to add a task to the DEMPA queue if the employer is not listed).  Click No add an activity record indicating you attempted to obtain current employer information but the borrower is not employed.", "Employer", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (re == DialogResult.No)
                {
                    AddCommentInLP50(_brw.SSN, "DD011", _mcqs.ActivityType, _mcqs.ContactType, "attempted to obtain current employer information but the borrower is not employed", false, false);
                }
                else
                {
                    LoclEmplResearch();
                }
            }
        }

        //adds comments and creates queue task for no employer problem
        private void EmplNLE()
        {
            AddCommentInLP50(_brw.SSN, "DD011", _mcqs.ActivityType, _mcqs.ContactType, "attempted to obtain current employer information but the borrower is not employed", false, false);
            AddQueueTaskInLP9O(_brw.SSN, "DEMPA", DateTime.Today.ToString("MM/dd/yyyy"), "borrower said he/she is no longer employed", "", "", "");
            _brw.EmployerID = string.Empty;
        }


        private void LoclEmplResearch()
        {
            while (true)
            {
                ExtendedEmployerDemographics demos = new ExtendedEmployerDemographics();
                FindEmployer feFrm = new FindEmployer(demos);
                FastPath("LPEMI");
                if (feFrm.ShowDialog() == DialogResult.Cancel)
                {
                    //end script if cancel is clicked
                    EndDLLScript();
                }
                if (feFrm.GetButtonClickedResults == FindEmployer.ButtonClicked.Search)
                {
                    //search
                    PutText(7, 33, demos.ID);
                    PutText(11, 33, demos.Name);
                    PutText(15, 33, demos.City);
                    PutText(17, 33, demos.State, ReflectionInterface.Key.Enter);
                    List<ExtendedEmployerDemographicsWithListView> demoRecsFromLPEM = new List<ExtendedEmployerDemographicsWithListView>();
                    while (Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") == false)
                    {
                        for (int x = 7; x < 20; x++)
                        {
                            if (GetText(x, 2, 2) != string.Empty)
                            {
                                //TODO: This really needs to be adjusted to handle multiple departments
                                PutText(21, 13, GetText(x, 2, 2).PadLeft(2, "0".ToCharArray()[0]), ReflectionInterface.Key.Enter);
                                demoRecsFromLPEM.Add(new ExtendedEmployerDemographicsWithListView(GetText(4, 21, 8), GetText(5, 21, 40), GetText(8, 21, 40), GetText(11, 21, 30), GetText(11, 59, 2), GetText(11, 66, 14)));
                                Hit(ReflectionInterface.Key.F12);
                            }
                        }
                        Hit(ReflectionInterface.Key.F8);
                    }
                    EmployerSelection esFrm = new EmployerSelection(demoRecsFromLPEM);
                    if (esFrm.ShowDialog() == DialogResult.Cancel)
                    {
                        EndDLLScript(); //end script
                    }
                    ExtendedEmployerDemographicsWithListView TheChosenOne = esFrm.TheChosenOne;
                    if (MessageBox.Show(string.Format("Is this the correct Employer?\n\nID:  {0}\nName:  {1}\nAddress:  {2}\nCity:  {3}\nState:  {4}\nZip:  {5}", TheChosenOne.EmployerDemos.ID, TheChosenOne.EmployerDemos.Name, TheChosenOne.EmployerDemos.Addr1, TheChosenOne.EmployerDemos.City, TheChosenOne.EmployerDemos.State, TheChosenOne.EmployerDemos.Zip), "Is This Correct?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string qtr = string.Empty;
                        DateTime currentDt = DateTime.Today;
                        if (currentDt.Month < 4)
                        {
                            qtr = "01";
                        }
                        else if (currentDt.Month < 7)
                        {
                            qtr = "02";
                        }
                        else if (currentDt.Month < 10)
                        {
                            qtr = "03";
                        }
                        else
                        {
                            qtr = "04";
                        }
                        //get employer ID
                        FastPath("LC20C" + _brw.SSN);
                        if (Check4Text(22, 3, "48012 ENTERED KEY NOT FOUND"))
                        {
                            PutText(1, 7, "A", ReflectionInterface.Key.Enter);
                            PutText(10, 2, qtr);
                            PutText(10, 5, currentDt.ToString("yyyy"));
                            PutText(10, 12, TheChosenOne.EmployerDemos.ID);
                        }
                        else
                        {
                            PutText(9, 2, qtr);
                            PutText(9, 5, currentDt.ToString("yyyy"));
                            PutText(9, 12, TheChosenOne.EmployerDemos.ID);
                        }
                        PutText(10, 78, "Y", ReflectionInterface.Key.Enter);
                        AddCommentInLP50(_brw.SSN, "DD014", _mcqs.ActivityType, _mcqs.ContactType, string.Format("Updated LC20 with new current employer information: {0},{1}", TheChosenOne.EmployerDemos.Name, TheChosenOne.EmployerDemos.ID), true, false);
                        return;
                    }
                }
                else
                {
                    //add
                    AddQueueTaskInLP9O(_brw.SSN, "DEMPA", "", string.Format("{0},{1},{2},{3},{4}", demos.Name, demos.Addr1, demos.City, demos.State, demos.Zip), "", "", "");
                    AddCommentInLP50(_brw.SSN, "DD014", _mcqs.ActivityType, _mcqs.ContactType, string.Format("added task to DEMPA with new current employer information:  {0},{1},{2},{3},{4}", demos.Name, demos.Addr1, demos.City, demos.State, demos.Zip), true, false);
                    return;
                }
            }
        }

        private double ExpPmt()
        {
            //access LC34
            FastPath("LC34I" + _brw.SSN);
            RS.TransmitANSI("01");
            Hit(ReflectionInterface.Key.Enter);
            int row = 9;
            double tempExpPmt = 0;
            while (true)
            {
                tempExpPmt = tempExpPmt + double.Parse(GetText(row, 44, 10));
                row++;
                if (Check4Text(row, 3, "_") == false)
                {
                    Hit(ReflectionInterface.Key.F8);
                    if (Check4Text(22, 3, "46004"))
                    {
                        break;
                    }
                    row = 9;
                }
            }
            return tempExpPmt;
        }


    }
}

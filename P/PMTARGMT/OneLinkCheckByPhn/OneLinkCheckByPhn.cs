using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;
using System.Windows.Forms;

namespace OneLinkCheckByPhn
{
    public class OneLinkCheckByPhn : ScriptBase
    {

        private CheckByPhoneData _data;
        UserInputOneLinkCheckByPhn _ckByPhnFrm;

        //For Nexus
        public OneLinkCheckByPhn(ReflectionInterface ri)
            : base(ri,"OLCHKBYPHN")
        {
        }

        //For DUDE
        public OneLinkCheckByPhn(ReflectionInterface ri, MDBorrower mdBor, int tRunNum)
            : base(ri, "OLCHKBYPHN", mdBor, tRunNum)
        {
        }

        /// <summary>
        /// Main function to start the script
        /// </summary>
        public override void Main()
        {
            if (CalledByMauiDUDE)
            {
                _data = new CheckByPhoneData(MauiDUDEBorrower.SSN);
            }
            else
            {
                _data = new CheckByPhoneData("");
            }
            _ckByPhnFrm = new UserInputOneLinkCheckByPhn(_data);
            if (_ckByPhnFrm.ShowDialog(UserInputOneLinkCheckByPhn.ControlToHaveFocus.SSNOrAcctNum) == DialogResult.Cancel)
            {
                //for cancelled form
                EndDLLScript();
            }
            //validate data
            UserInputOneLinkCheckByPhn.ControlToHaveFocus validResults = Validator();
            while (validResults != UserInputOneLinkCheckByPhn.ControlToHaveFocus.AllDataValid)
            {
                //show form again to gather information again if something wasn't valid
                if (_ckByPhnFrm.ShowDialog(validResults) == DialogResult.Cancel)
                {
                    //for cancelled form
                    EndDLLScript();
                }
                validResults = Validator();
            }
            //do system updates
            //add leading zeros to the payment amount
            _data.PaymentAmount = _data.PaymentAmount.PadLeft(8, '0');
            //add trailing spaces to the account number
            _data.VerCheckingAcctNum = _data.VerCheckingAcctNum.PadRight(20, ' ');
            string accountType = (_data.Savings ? "S" : "C");
            //figure contact code
            string contactCode;
            if (_data.Inbound)
            {
                contactCode = "04";
            }
            else
            {
                contactCode = "03";
            }
            //add comments
            AddCommentInLP50(_data.SSN, "DPNCK", "AM", contactCode, string.Format("${0} Check by phone payment received from borrower.  Payment will post on {1}.", _data.PaymentAmount, _data.PaymentEffectiveDate));
            //calculate what to enter into the Due Date field of the queue task
            string calculatedPaymentPostingDate = (DateTime.Parse(_data.PaymentEffectiveDate) == DateTime.Today.Date?string.Empty:_data.PaymentEffectiveDate);
            //add queue task
            AddQueueTaskInLP9O(_data.SSN, "CKBYPHON", calculatedPaymentPostingDate, string.Format("${0} {1} {2}{3}{4}", _data.PaymentAmount, _data.VerRouting, _data.VerCheckingAcctNum, accountType, _data.PaymentEffectiveDate));
            //add record to database table
            DataAccess.AddRecordToOneLINKCheckByPhoneData(TestModeProperty, contactCode, decimal.Parse(_data.PaymentAmount));
            MessageBox.Show("Processing Complete","Processing Complete",MessageBoxButtons.OK,MessageBoxIcon.Information);
            EndDLLScript();
        }

        private UserInputOneLinkCheckByPhn.ControlToHaveFocus Validator()
        {            
            //do SSN translation if account number is provided and check that the SSN or acct number are on the system
            try
            {
                _data.SSN = GetDemographicsFromLP22(_data.SSNOrAcctNum).SSN;
            }
            catch (DemographicRetrievalException)
            {
                MessageBox.Show("This account does not have a default record, you cannot process a check by phone payment.  Please check your information and try again if you believe you have received this message in error.", "No Default Record",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return UserInputOneLinkCheckByPhn.ControlToHaveFocus.SSNOrAcctNum;
            }

            //access LC05
            FastPath(string.Format("LC05I{0}",_data.SSN));
            if (Check4Text(22, 3, "47004 NO RECORDS FOUND FOR ENTERED SEARCH CRITERIA"))
            {
                MessageBox.Show("This account does not have a default record, you cannot process a check by phone payment.  Please check your information and try again if you believe you have received this message in error.", "No Default Record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return UserInputOneLinkCheckByPhn.ControlToHaveFocus.SSNOrAcctNum;
            }
            double outside;
            if (GetText(4, 72, 9).Replace(",","").Replace(".","").IsNumeric())
            {
                outside = double.Parse(GetText(4, 72, 9));
            }
            else
	        {
                outside = 0;
	        }
            if (outside <= 0)
            {
                FastPath(string.Format("TX3Z/ITS26{0}",_data.SSN));
                if (Check4Text(1, 72, "TSX28"))
                {
                    //SELECTION SCREEN
                    while (Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
                    {
                        for (int x = 8; x < 20; x++)
                        {
                            if (GetText(x, 49, 9).Replace(",","").Replace(".","").IsNumeric())
                            {
                                if (double.Parse(GetText(x, 49, 9).Replace(",","")) > 0)
                                {
                                    MessageBox.Show("This borrower does not have any open default loans.  Use the COMPASS check by phone script.","Borrower Not Defaulted",MessageBoxButtons.OK,MessageBoxIcon.Information);
                                    EndDLLScript();
                                }
                            }
                        }
                        Hit(ReflectionInterface.Key.F8);
                    }
                    MessageBox.Show("This borrower does not have open loans on OneLink or Compass.","No Open Loans",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    EndDLLScript();
                }
                else if (Check4Text(1, 72, "TSX29"))
                {
                    //TARGET SCREEN
                    if (double.Parse(GetText(11, 12, 10).Replace(",","")) > 0)
                    {
                        MessageBox.Show("This borrower does not have any open default loans.  Use the COMPASS check by phone script.","Borrower Not Defaulted",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        EndDLLScript();
                    }
                    else
                    {
                        MessageBox.Show("This borrower does not have open loans on OneLink or Compass.","No Open Loans",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        EndDLLScript();
                    }
                }
                else
                {
                    MessageBox.Show("This borrower does not have open loans on OneLink or Compass.","No Open Loans",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    EndDLLScript();
                }
                
            }
                
            if (_data.SSN.Length == 9 && _data.Routing.Length == 9 && _data.CheckingAcctNum.Length != 0 &&
               _data.SSN.IsNumeric() && _data.Routing.IsNumeric() && _data.CheckingAcctNum.IsNumeric() &&
               _data.PaymentAmount.IsNumeric() && (_data.Checking || _data.Savings))
            {
                //remove any "," if found
                _data.PaymentAmount = _data.PaymentAmount.Replace(",","");
                if (double.Parse(_data.PaymentAmount) <= 99999.99)
                {
                    //check if payment posting date is valid
                    if (_data.PaymentEffectiveDate.IsValidDate() == false || DateTime.Parse(_data.PaymentEffectiveDate) < DateTime.Today.Date || DateTime.Parse(_data.PaymentEffectiveDate) > DateTime.Today.AddDays(14).Date)
                    {
                        MessageBox.Show("The Payment Posting Date must be the current or a future date, and if a future date it must be 14 days or less in the future.", "Invalid Payment Posting Date", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return UserInputOneLinkCheckByPhn.ControlToHaveFocus.PaymentPostingDate;
                    }
                    //check if a "." is in the 3rd from the last place in the string
                    if (_data.PaymentAmount.SafeSubstring(_data.PaymentAmount.Length - 3, 1) != ".")
                    {
                        MessageBox.Show("The payment amount must be in the following format: \"XXXXXX.XX\".","Bad Payment Amount Format",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                        return UserInputOneLinkCheckByPhn.ControlToHaveFocus.PaymentAmount;
                    }
                    else
                    {
                        if (_data.Routing != _data.VerRouting) //check if double entry routing numbers =
                        {
                            _data.Routing = string.Empty;
                            _data.VerRouting = string.Empty;
                            MessageBox.Show("The routing numbers don't match, please try again.","Given Entries Don't Match",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                            return UserInputOneLinkCheckByPhn.ControlToHaveFocus.RoutingNum;
                        }
                        else if (_data.CheckingAcctNum != _data.VerCheckingAcctNum) //check if double entry acct # =
                        {
                            _data.CheckingAcctNum = string.Empty;
                            _data.VerCheckingAcctNum = string.Empty;
                            MessageBox.Show("The bank account numbers don't match, please try again.", "Given Entries Don't Match", MessageBoxButtons.OK,MessageBoxIcon.Stop);
                            return UserInputOneLinkCheckByPhn.ControlToHaveFocus.CheckingAcctNum;
                        }
                        else //if every thing is accurate proceed
                        {
                            DialogResult userResponse = MessageBox.Show(string.Format("Borrower SSN: {1}{0}Routing Number: {2}{0}Checking Account Number: {3}{0}Payment Amount: {4}{0}Account Type: {5}{0}Payment Effective Date: {6}{0}{0}Click \"Yes\" to process, \"No\" to change entered data elements, or \"Cancel\" to end the script.", Environment.NewLine, _data.SSN, _data.VerRouting, _data.VerCheckingAcctNum, double.Parse(_data.PaymentAmount).ToString("$###,##0.00"), _data.CalculatedAccountType(),_data.PaymentEffectiveDate),"Information Confirmation",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question);

                            if (userResponse == DialogResult.Yes)
                            {
                                return UserInputOneLinkCheckByPhn.ControlToHaveFocus.AllDataValid;
                            }
                            else if (userResponse == DialogResult.No)
                            {
                                return UserInputOneLinkCheckByPhn.ControlToHaveFocus.SSNOrAcctNum;
                            }
                            else //if the close or cancel buttons are clicked
                            {
                                EndDLLScript();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("The \"Payment Amount\" field must be less than $100,000.00.", "Too Much Money",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    return UserInputOneLinkCheckByPhn.ControlToHaveFocus.PaymentAmount;
                }
            }
            else
            {
                if (_data.Routing.Length != 9)
                {
                    MessageBox.Show("The routing number must be nine digits.", "Nine Digits Needed",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    return UserInputOneLinkCheckByPhn.ControlToHaveFocus.RoutingNum;
                }
                else
                {
                    MessageBox.Show("In order to continue all fields must be populated and numeric.","Information Needed",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    return UserInputOneLinkCheckByPhn.ControlToHaveFocus.SSNOrAcctNum;
                }
            }
            throw new System.Exception("Something really bad happened.  Please contact Systems Support.");
        }

    }
}

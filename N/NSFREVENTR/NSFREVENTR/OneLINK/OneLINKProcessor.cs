//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Q;
//using System.Windows.Forms;

//namespace NSFREVENTR
//{
//    class OneLINKProcessor: SystemProcessor
//    {
//        //TODO: This whole class needs to be rewritten.  I was tempted to, but was short on time and had a hard time justifying doing so when there where no changes needed to this functionaity. AA

//        /// <summary>
//        /// Constructor
//        /// </summary>
//        public OneLINKProcessor(ReversalEntry userEntry, ReflectionInterface ri)
//            : base(ri)
//        { 
//            _userEntry = userEntry;
//            _testModeResults = TestMode(NSFReversalEntry.ONELINK_DIR);
//        }

//        /// <summary>
//        /// Main starting point for processing.
//        /// </summary>
//        public override void ProcessEntry()
//        {
//            BorrowerDemographics demos;
//            try
//            {
//                demos = GetDemographicsFromLP22(_userEntry.SSN);
//                _userEntry.SSN = demos.SSN;
//            }
//            catch (DemographicRetrievalException)
//            {
//                System.Windows.Forms.MessageBox.Show("The borrower wasn't found on OneLINK.  Please try again.");
//                return;
//            }
//            bool found = false;
//            int fcount = 0;
//            //count matching loans
//            FastPath(string.Format("LC41I{0}",_userEntry.SSN));
//            PutText(7,36,"X",ReflectionInterface.Key.Enter);
//            if (Check4Text(1, 60, "PAYMENT RECORD SELECT"))
//            {
//                while (Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") == false)
//                {
//                    for (int x = 7;x < 19;x++)
//                    {
//                        if (Check4Text(x, 34, "BR") && Check4Text(x, 5, DateTime.Parse(_userEntry.EffectiveDate).ToString("MMddyyyy")) && GetText(x, 39, 3) == "")
//                        {
//                            fcount++;
//                        }
//                    }
//                    Hit(ReflectionInterface.Key.F8);
//                }
//            }
//            else if (Check4Text(1, 59, "PAYMENT RECORD DISPLAY"))
//            {
//                if (Check4Text(4, 28, "BRWR PAYMT") && Check4Text(4, 72, DateTime.Parse(_userEntry.EffectiveDate).ToString("MMddyyyy")) && GetText(6, 28, 9) == "")
//                {
//                    fcount++;
//                }
//            }
            
//            DateTime PostedDt = DateTime.Today; //today isn't used any where I just had to init the var with something
//            string TransType = string.Empty;

//            if (fcount == 1)
//            {
//                FastPath(string.Format("LC41I{0}", _userEntry.SSN));
//                PutText(7, 36, "X",ReflectionInterface.Key.Enter);
//                if (Check4Text(1, 60, "PAYMENT RECORD SELECT"))
//                {
//                    while (Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") == false)
//                    {
//                        for (int x = 7;x < 19;x++)
//                        {
//                            if (Check4Text(x, 34, "BR") && Check4Text(x, 5, DateTime.Parse(_userEntry.EffectiveDate).ToString("MMddyyyy")) && GetText(x, 39, 3) == "")
//                            {
//                                found = true;
//                                TransType = GetText(x, 34, 2);
//                                PostedDt = DateTime.Parse(string.Format("{0}/{1}/{2}",GetText(x, 25, 2), GetText(x, 27, 2), GetText(x, 29, 4)));
//                                break;
//                            }
//                        }
//                        if (found) break; //break out of loop if transaction is found
//                        Hit(ReflectionInterface.Key.F8);
//                    }
//                }
//                else if (Check4Text(1, 59, "PAYMENT RECORD DISPLAY"))
//                {
//                    if (Check4Text(4, 28, "BRWR PAYMT ") && Check4Text(4, 72, DateTime.Parse(_userEntry.EffectiveDate).ToString("MMddyyyy")) && GetText(6, 28, 9) == "")
//                    {
//                        found = true;
//                        TransType = GetText(4, 28, 2);
//                        PostedDt = DateTime.Parse(string.Format("{0}/{1}/{2}",GetText(5, 72, 2),GetText(5, 74, 2),GetText(5, 76, 4)));
//                    }
//                }
//            }
//            else if (fcount > 1)
//            {
//                FastPath(string.Format("LC41I{0}",_userEntry.SSN));
//                PutText(7, 36, "X",ReflectionInterface.Key.Enter);
//                MessageBox.Show("More than one transaction was found that matches the criteria. Please select one and press <Insert>.","More Than One",MessageBoxButtons.OK,MessageBoxIcon.Information);
//                PauseForInsert();
//                while (!Check4Text(1, 59, "PAYMENT RECORD DISPLAY"))
//                {
//                    MessageBox.Show("You must be on the PAYMENT RECORD DISPLAY screen to proceed.  Select the desired payment and hit <Insert> to proceed.", "Payment not Selected", MessageBoxButtons.OK,MessageBoxIcon.Error);
//                    PauseForInsert();
//                }
//                TransType = GetText(4, 28, 2);
//                PostedDt = DateTime.Parse(string.Format("{0}/{1}/{2}",GetText(5, 72, 2),GetText(5, 74, 2),GetText(5, 76, 4)));
//                found = true;
//            }
//            else if (fcount == 0)
//            {
//                found = false;
//            }
            
//            if (found)
//            {
//                string NSF;
//                string QTask = "N";
//                if (_userEntry.BatchType == NSFReversalEntry.BatchType.Cash)
//                {
//                    NSF = "C";
//                }
//                else //wired
//                {
//                    NSF = "W";
//                }
//                FastPath(string.Format("LC41I{0}",_userEntry.SSN));
//                PutText(7, 36, "X",ReflectionInterface.Key.Enter);
//                if (Check4Text(1, 60, "PAYMENT RECORD SELECT"))
//                {
//                    bool foundTran = false;
//                    while (Check4Text(22, 3, "46004 NO MORE DATA TO DISPLAY") == false)
//                    {
//                        for (int x = 7;x < 19;x++)
//                        {
//                            if (GetText(x, 5, 8) != "")
//                            {
//                                if (Check4Text(x, 34, "BR") && DateTime.Parse(string.Format("{0}/{1}/{2}",GetText(x, 5, 2),GetText(x, 7, 2),GetText(x, 9, 4))) > DateTime.Today.AddMonths(-6) && Check4Text(x, 39, "BC"))
//                                {
//                                    QTask = "Y";
//                                    foundTran = true;
//                                    break;
//                                }
//                            }
//                        }
//                        if (foundTran) break;
//                        Hit(ReflectionInterface.Key.F8);
//                    }
//                }
//                else if (Check4Text(1, 59, "PAYMENT RECORD DISPLAY"))
//                {
//                    if (Check4Text(4, 28, "BRWR PAYMT") && DateTime.Parse(string.Format("{0}/{1}/{2}",GetText(4, 72, 2),GetText(4, 74, 2),GetText(4, 76, 4))) > DateTime.Today.AddMonths(-6) && Check4Text(6, 29, "BAD CHECK"))
//                    {
//                        QTask = "Y";
//                    }
//                }
//                //update batch text file
//                VbaStyleFileOpen(_testModeResults.DocFolder + NSFReversalEntry.ONELINK_NSF_ENTRY_FILE,1,Common.MSOpenMode.Append);
//                VbaStyleFileWriteLine(1, _userEntry.SSN, demos.LName, _userEntry.EffectiveDate, PostedDt.ToString("MM/dd/yyyy"), _userEntry.PaymentAmount, NSF, TransType, string.Format("{0} - {1}", _userEntry.NSFRe.Description, _userEntry.NSFRe.Code), QTask);
//                VbaStyleFileClose(1);
//            }
//            else
//            {
//                MessageBox.Show("There are no matching payment transactions. Please review the payment transactions and press <Insert> when done.", "No Transactions Found",MessageBoxButtons.OK,MessageBoxIcon.Warning);
//                PauseForInsert();
//                return;
//            }
            
//        }
//    }
//}

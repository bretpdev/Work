using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace LegalAWGAnswer
{
    public class LegalAWGAnswer : ScriptBase
    {
        public LegalAWGAnswer(ReflectionInterface ri)
            : base(ri, "LGLAWGANS")   
        {
        }

        public LegalAWGAnswer(ReflectionInterface ri, MDBorrower mdBorr, int runNum)
            : base(ri, "LGLAWGANS", mdBorr, runNum)
        {
        }

        string employerID = string.Empty;
        string employerName = string.Empty;
        string WillDo = string.Empty;
        string CancAct = string.Empty;
        string Comment = string.Empty;
        string GarnElig = string.Empty;
        string ActTyp = string.Empty;
        string ActCd = string.Empty;
        string ConTyp = string.Empty;
        string holdrea = string.Empty;
        string SSN = string.Empty;
        int option = 0;

        private GarnishmentEligibilityCheck LC67Check;

        public override void Main()
        {
            ActCd = "LGANS";
            ActTyp = "CD";
            ConTyp = "82";
            GarnElig = "Y"; // Set to yes by default, made N if not eligible.
            SSN = "";

            if (CalledByMauiDUDE)
            {
                SSN = MauiDUDEBorrower.SSN;
            }

            frmLglAWGAns lglAWGAnsForm;
            lglAWGAnsForm = new frmLglAWGAns(SSN);

                lglAWGAnsForm.ShowDialog();

                SSN = lglAWGAnsForm.acctOrSSN;
                option = lglAWGAnsForm.selection;

                if (option == -1)
                {
                    return;
                }

            if (SSN.Length < 9)
            {
                MessageBox.Show("SSN or Account Number is not a valid length.");
                return; //length = 0 if user pushed cancel. 
            }

            else if ((SSN.Length == 10) || (SSN.Length == 9))
            {
                SystemBorrowerDemographics Borr = GetDemographicsFromLP22(SSN);
                SSN = Borr.SSN;
            }

            if (option != 6)
            {
                VerEmp();
                VerElig();

            }

            else
            {
                GarnElig = "N";
                VerEmp();
                FastPath(string.Format("LC20C"));

                if (RI.GetText(1, 66, 7) == "ENT INF")
                {
                    RI.PutText(10, 78, "N");

                    int loanCntr = 10;
                    bool pass;
                    pass = (RI.GetText(loanCntr, 12, 8) == employerID) || (RI.GetText(loanCntr, 12, 1) == " ");
                    if (!pass)
                    {
                        do
                        {
                            loanCntr++;
                        } while ((RI.GetText(loanCntr, 12, 8) == employerID) || (RI.GetText(loanCntr, 12, 1) == " "));
                    }

                    if (RI.GetText(loanCntr, 12, 1) != " ")
                    {
                        RI.PutText(loanCntr, 2, "012000", Key.Enter);
                    }
                }

                FastPath(string.Format("LC67I" + SSN + ";EX"));

                if (RI.GetText(22, 3, 5) != "47004")
                {
                }
                else if (RI.GetText(21, 3, 3) != "SEL")
                {
                    RI.PutText(21, 13, "01", Key.Enter);
                    while ((RI.GetText(2,21,2) != "EX") || (RI.GetText(6,20,2) != "05"))
                    {
                        Hit(Key.F8);
                        if (RI.GetText(22, 3, 5) == "46004")
                        {
                            break;
                        }
                    } 
                }
                if ((RI.GetText(22, 3, 5) != "46004") && (RI.GetText(22, 3, 5) != "47004"))
                {
                    RI.PutText(1, 7, "D", Key.Enter);
                    Hit(Key.Enter); //2nd required?
                }
                ActCd = "L030N";
                Comment = "LEGAL AWG:  received answer from " + employerName + " (" + employerID + "), borrower is NLE";
            }

            if (GarnElig == "Y")
            {
                if (option == 1)
                {
                    decimal garnAmount = 0;

                    frmGarn garnishment;
                    garnishment = new frmGarn();

                    garnishment.ShowDialog();

                    if (garnishment.selection == 1)
                    {
                        return;
                    }

                    garnAmount = garnishment.garnValue;


                    Comment = "Legal AWG:  " + employerID + " received answer in the amount of " + garnAmount.ToString("C") + " from " + employerName + " (" + employerID + ")";
                }
                else if (option == 2)
                {
                    holdrea = "23";
                    HoldLC67();
                    Comment = "Legal AWG:  " + employerID + " received answer from " + employerName + " (" + employerID + ")" + ", borrower on temporary leave";
                }
                else if (option == 3)
                {
                    holdrea = "14";
                    HoldLC67();
                    Comment = "Legal AWG:  " + employerID + " received answer from " + employerName + " (" + employerID + ")" + ", borrower currently being garnished by another agency";
                }
                else if (option == 4)
                {
                    holdrea = "13";
                    HoldLC67();
                    Comment = "Legal AWG:  " + employerID + " received answer from " + employerName + " (" + employerID + ")" + ", borrower does not make enough wages to garnish";
                }
                else
                {
                    holdrea = "15";
                    HoldLC67();
                    Comment = "Legal AWG:  " + employerID + " received answer from " + employerName + " (" + employerID + ")" + ", borrower currently being garnished by recovery services";
                }
            }
            if ((option == 1) || (option == 6) || (GarnElig != "Y"))
            {
                //Queue = "LEGALAWG"; Set inside CancelTask();
                LC67Check = new GarnishmentEligibilityCheck(RI, SSN, WillDo, CancAct);
                LC67Check.CancelTask("LEGALAWG");
            }
            AddCommentInLP50(SSN, ActCd, ActTyp, ConTyp, Comment);

            MessageBox.Show("Processing Complete", "Processing Complete", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        //verify the current employer
        void VerEmp()
        {
            FastPath(string.Format("LC20I" + SSN));
            //if ((Check4Text(10, 78, "Y") == false) && (Check4Text(10, 2, "  ") == false))

            if (Check4Text(10, 2, " ") == false)
            {
                employerID = GetText(10, 12, 8);
                employerID = employerID.Trim();

                do
                {
                    FastPath(string.Format("LPEMI" + employerID));

                    DialogResult result = MessageBox.Show("Is this the correct employer?  Click OK to continue or Cancel to pause the script so you can select another employer (hit Insert to continue once the employer you select is displayed).", "Critical Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Cancel)
                    {
                        PauseForInsert();
                    }

                    if (Check4Text(1, 57, "INSTITUTION DEMOGRAPHICS") == false)
                    {
                        result = MessageBox.Show("An employer record must be displayed on the LPEM INSTITUTION DEMOGRAPHICS screen to proceed.  Click OK to pause the script so you can select an employer (hit Insert to continue once the employer you select is displayed) or click Cancel to end the script.", "Critical Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (result == DialogResult.Cancel)
                        {
                            EndDLLScript();
                        }
                        else if (result == DialogResult.OK)
                        {
                            PauseForInsert();
                        }
                    }
                    else
                    {
                        employerName = GetText(5, 21, 40).Trim();
                        employerID = GetText(4, 21, 8).Trim();
                        break;
                    }
                }
                while (true);
            }

            else
            {
                DialogResult result = MessageBox.Show("No current employer was found.  Access the record for the desired employer on the LPEM screen and hit Insert to continue.", "Critical Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                if (result == DialogResult.OK)
                {
                    do
                    {
                        FastPath(string.Format("LPEMI"));
                        PauseForInsert();

                        if (Check4Text(1, 57, "INSTITUTION DEMOGRAPHICS") == false)
                        {
                            result = MessageBox.Show("An employer record must be displayed on the LPEM INSTITUTION DEMOGRAPHICS screen to proceed.  Click OK to pause the script so you can select an employer (hit Insert to continue once the employer you select is displayed) or click Cancel to end the script.", "Critical Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (result == DialogResult.Cancel)
                            {
                                EndDLLScript();
                            }
                            else if (result == DialogResult.OK)
                            {
                                PauseForInsert();
                            }
                        }
                        else
                        {
                            employerName = GetText(5, 21, 40).Trim();
                            employerID = GetText(4, 21, 8).Trim();
                            break;
                        }
                    }
                    while (true);
                }

            }
        }

        void VerElig()
        {
            FastPath(string.Format("LC67I" + SSN + ";EX"));

            if (RI.GetText(22, 3, 5) == "47004")
            {
                NotElig(); 
                return;
            }
            else if (RI.GetText(21, 3, 3) == "SEL")
            {
                RI.PutText(21, 13, "01", Key.Enter);
            }

            while ((RI.GetText(2, 21, 2) != "EX") || (RI.GetText(6, 20, 2) != "05"))
            {
                Hit(Key.F8);
                if (RI.GetText(22, 3, 5) == "46004")
                {
                    NotElig();
                    return;
                }
            }

            FastPath(string.Format("LC18I" + SSN));


            if (RI.GetText(15, 12, 8) != "MMDDCCYY")
            {
                DateTime tempDate;

                tempDate = Convert.ToDateTime((RI.GetText(15, 12, 2)) + "/" + (RI.GetText(15, 14, 2)) + "/" + (RI.GetText(15, 16, 4)));
                //tempDate = (RI.GetText(15, 12, 8)).ToString();

                if (tempDate >= DateTime.Now)
                {
                    NotElig();
                    return;
                }
            }

            WillDo = "The script will complete the LEGALAWG task and add an activity record.";
            CancAct = "LEGAL AWG:  answer received from " + employerName + " (" + employerID + ")";
            Comment = ", LEGAL AWG answer received from " + employerName + " (" + employerID + ")" + ", borrower still employed but no longer eligible for garnishment";
            LC67Check = new GarnishmentEligibilityCheck(RI, SSN, WillDo, CancAct);
            LC67Check.LC67GarnEligible(); //CommonGarn.LC67
            
            
            if (GarnElig != "Y")
            {
                ActTyp = "AM";
                ConTyp = "10";
            }
        }

        void NotElig()
        {
            GarnElig = "N";
            ActTyp = "AM";
            ConTyp = "10";
            Comment = "LEGAL AWG:  answer received from " + employerName + " (" + employerID + ")" + ", borrower still employed but no longer eligible for garnishment";
        }

        void HoldLC67()
        {
            string holdDate = string.Empty;

            while (true)
            {
                
                holdDate = InputBox.ShowDialog("Enter the date the borrower will return to work OR the date you wish to attempt another garnishment.  Do not enter a date more than six months from today (" + DateTime.Now.AddMonths(6).ToShortDateString() + ").  Enter the date in MMDDCCYY format.", "Hold Date").UserProvidedText;
                //InputBox("Enter the date the borrower will return to work OR the date you wish to attempt another garnishment.  Do not enter a date more than six months from today (" & Format(DateAdd("m", 6, Date), "MM/DD/YYYY") & ").  Enter the date in MMDDCCYY format.", "Hold Date");
                
                if (holdDate == "")
                {
                    return;
                }
                else if (Convert.ToDateTime(holdDate) <= Convert.ToDateTime(DateTime.Now.AddMonths(6).ToShortDateString())) //** Modify
                {
                    break;
                }
                else
                {
                    DialogResult result = MessageBox.Show("The date entered is more than six months in the future.  Click OK to enter another date.", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
            }

            FastPath(string.Format("LC67C" + SSN + "SC"));

            if (RI.GetText(1, 54, 7) != "SUMMONS")
            {
                RI.PutText(21, 13, "01", Key.Enter);
            }

            do
            {
                if ((RI.GetText(8, 19, 2) == "__") || (RI.GetText(15, 63, 2) == "__"))
                {
                    RI.PutText(16, 71,  holdDate.Replace("/",""));
                    RI.PutText(17, 57, holdrea, Key.Enter);
                }
                Hit(Key.F8);
            } while (RI.GetText(21, 3, 5) != "46004");
        }
    }
}
    

 

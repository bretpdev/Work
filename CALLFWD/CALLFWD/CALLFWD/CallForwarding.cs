using System.Windows.Forms;
using Q;
using Key = Q.ReflectionInterface.Key;

namespace CALLFWD
{
    public class CallForwarding : ScriptBase
    {
        public CallForwarding(ReflectionInterface ri)
            : base(ri, "CALLFWD")
        {
        }

        public override void Main()
        {
            BusinessUnitDialog businessUnitDialog = new BusinessUnitDialog();
            DialogResult dialogResult = DialogResult.OK;
            while (dialogResult == DialogResult.OK)
            {
                //Get some sort of borrower ID from the user.
                InputBoxResults userInput = InputBox.ShowDialog("Please enter a valid SSN, Account Number, or ACS Keyline.", "Borrower Info");
                if (userInput.DialogRe != DialogResult.OK || string.IsNullOrEmpty(userInput.UserProvidedText)) { EndDLLScript(); }

                //Get an SSN from the borrower ID and check it against the system.
                string ssn = GetVerifiedSsn(userInput.UserProvidedText);
                //Determine from the system which business unit can best server the borrower.
                BusinessUnitDialog.BusinessUnit businessUnit = GetBusinessUnit(ssn);
                //Show the user the results and see if they want to check more users.
                dialogResult = businessUnitDialog.ShowDialog(businessUnit);
            }
        }//Main()

        private string GetVerifiedSsn(string userInput)
        {
            if (userInput.IsNumeric())
            {
                if (userInput.Length == 10)
                {
                    //Account number.
                    FastPath("LP22I;;;;;;" + userInput);
                    if (Check4Text(1, 62, "PERSON DEMOGRAPHICS"))
                    {
                        return GetText(3, 23, 9);
                    }
                    else
                    {
                        MessageBox.Show("That wasn't a valid account number.");
                        EndDLLScript();
                    }
                }
                else if (userInput.Length == 9)
                {
                    //SSN.
                    FastPath("LP22I" + userInput);
                    if (Check4Text(1, 62, "PERSON DEMOGRAPHICS"))
                    {
                        return GetText(3, 23, 9);
                    }
                    else
                    {
                        MessageBox.Show("That wasn't a valid SSN.");
                        EndDLLScript();
                    }
                }
                else
                {
                    MessageBox.Show("You didn't provide valid data.");
                    EndDLLScript();
                }
            }
            else
            {
                if (userInput.Length == 9)
                {
                    //ACS Keyline.
                    FastPath("LP22I" + Common.GetSsnFromKeyline(userInput));
                    if (Check4Text(1, 62, "PERSON DEMOGRAPHICS"))
                    {
                        return GetText(3, 23, 9);
                    }
                    else
                    {
                        MessageBox.Show("That ACS Keyline didn't translate into a valid SSN.");
                        EndDLLScript();
                    }
                }
                else
                {
                    MessageBox.Show("You didn't provide valid data.");
                    EndDLLScript();
                }
            }
            //The code can't actually reach this point, but the compiler doesn't know that,
            //so here's a return statement to keep the compiler happy.
            return string.Empty;
        }//GetVerifiedSsn()

        //Determine which business unit should help a borrower.
        private BusinessUnitDialog.BusinessUnit GetBusinessUnit(string ssn)
        {
            string[] sallieMaeIds = { "700079", "700004", "700789", "700191", "700190", "700121" };
            string[] nonBorrowerServiceStatuses = { "CA", "PC", "PM", "PF", "PN", "RF" };

            BusinessUnitDialog.BusinessUnit businessUnit = BusinessUnitDialog.BusinessUnit.None;
            bool contingency = false;

            FastPath("LG02I;" + ssn);
            if (Check4Text(1, 58, "LOAN APPLICATION SELECT"))
            {
                //Selection screen.
                for (int row = 10; !Check4Text(22, 3, "46004"); row++)
                {
                    if (Check4Text(row, 3, " ", "SELECTION"))
                    {
                        //No more records on this page; go to the next page.
                        Hit(Key.F8);
                        row = 9; //This will be incremented to 10 by the "row++" in the "for" statement.
                        continue;
                    }

                    //Check various statuses to see which business units are applicable.
                    if (Check4Text(row, 75, "CP", "DN"))
                    {
                        if (Check4Text(row, 78, "BC", "BO", "BH"))
                        {
                            if (Check4Text(row, 75, "CP")) { businessUnit |= BusinessUnitDialog.BusinessUnit.AuxServices; }
                        }
                        else
                        {
                            businessUnit |= BusinessUnitDialog.BusinessUnit.PostClaim;
                        }
                    }
                    if (Check4Text(row, 75, "CR"))
                    {
                        if (Check4Text(row, 78, "BC", "BO", "BH"))
                        {
                            businessUnit |= BusinessUnitDialog.BusinessUnit.AuxServices;
                        }
                        else
                        {
                            businessUnit |= BusinessUnitDialog.BusinessUnit.LoanManagement;
                        }
                    }
                    if (Check4Text(row, 46, "700126") && !Check4Text(row, 75, nonBorrowerServiceStatuses))
                    {
                        businessUnit |= BusinessUnitDialog.BusinessUnit.BorrowerServices;
                    }
                    //Select this loan to check whether it's been put to ED.
                    PutText(21, 13, GetText(row, 2, 2), Key.Enter);
                    if (Check4Text(2, 2, "GUARANTY TRANSFERRED"))
                    {
                        businessUnit |= BusinessUnitDialog.BusinessUnit.FedLoanServicing;
                    }
                    Hit(Key.F12);
                    if (Check4Text(row, 46, sallieMaeIds) && !Check4Text(row, 75, nonBorrowerServiceStatuses))
                    {
                        contingency = true;
                    }
                }//for
            }
            else
            {
                //Target screen.
                FastPath("LG10I" + ssn);
                if (Check4Text(11, 59, "CP", "DN"))
                {
                    if (Check4Text(11, 46, "BC", "BO", "BH"))
                    {
                        if (Check4Text(11, 59, "CP")) { businessUnit |= BusinessUnitDialog.BusinessUnit.AuxServices; }
                    }
                    else
                    {
                        businessUnit |= BusinessUnitDialog.BusinessUnit.PostClaim;
                    }
                }
                if (Check4Text(11, 59, "CR"))
                {
                    if (Check4Text(11, 46, "BC", "BO", "BH"))
                    {
                        businessUnit |= BusinessUnitDialog.BusinessUnit.AuxServices;
                    }
                    else
                    {
                        businessUnit |= BusinessUnitDialog.BusinessUnit.LoanManagement;
                    }
                }
                if (Check4Text(5, 27, "700126") && !Check4Text(11, 59, nonBorrowerServiceStatuses))
                {
                    businessUnit |= BusinessUnitDialog.BusinessUnit.BorrowerServices;
                }
                if (Check4Text(2, 2, "GUARANTY TRANSFERRED"))
                {
                    businessUnit |= BusinessUnitDialog.BusinessUnit.FedLoanServicing;
                }
                if (Check4Text(5, 27, sallieMaeIds) && !Check4Text(11, 59, nonBorrowerServiceStatuses))
                {
                    contingency = true;
                }
            }

            //If no business unit has been selected, do some additional checks.
            if (businessUnit == BusinessUnitDialog.BusinessUnit.None)
            {
                if (base.TestModeProperty)
                {
                    MessageBox.Show("Please take a moment and log into LCO test. Then press <Insert>.");
                    PauseForInsert();
                }
                //See if the borrower is on LCO.
                FastPath("TPDD" + ssn);
                if (Check4Text(1, 19, "LCO PERSONAL INFORMATION DISPLAY"))
                {
                    businessUnit |= BusinessUnitDialog.BusinessUnit.BorrowerServices;
                }
                if (base.TestModeProperty)
                {
                    MessageBox.Show("Please take a moment and log into regular test. Then press <Insert>.");
                    PauseForInsert();
                }
                //If the contingency variable is set, select the Loan Management business unit.
                if (contingency) { businessUnit |= BusinessUnitDialog.BusinessUnit.LoanManagement; }
            }

            //As a final fallback if no business units are selected, select Borrower Services.
            if (businessUnit == BusinessUnitDialog.BusinessUnit.None)
            {
                businessUnit |= BusinessUnitDialog.BusinessUnit.BorrowerServices;
            }

            return businessUnit;
        }//GetBusinessUnit()
    }//class
}//namespace

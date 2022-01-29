using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace SOPRTPROC
{
    public partial class StateOffsetForm : Form
    {
        StateOffset Offset;
        BorrowerDemographic Demo;
        private List<string> States;

        public StateOffsetForm(StateOffset offset, BorrowerDemographic demo)
        {
            InitializeComponent();
            this.ActiveControl = ListDateLabel;
            Offset = offset;
            Demo = demo;
            GetStates();
        }

        [UsesSproc(DataAccessHelper.Database.Csys, "spGENR_GetStateCodes")]
        private void GetStates()
        {
            States = DataAccessHelper.ExecuteList<string>("spGENR_GetStateCodes", DataAccessHelper.Database.Csys);
            States.Insert(0, "");
            State.Items.AddRange(States.ToArray());
        }

        private void ProcessOk_Click(object sender, EventArgs e)
        {
            if (!ValidateStateOffset()) //If data is missing, stop processing
                return;

            if (!Offset.CheckIfBatchExists(Demo)) //Check if the batch exists for this borrower
                return;

            if (!Offset.CheckLC05(Demo.SSN)) //Check LC05 to make sure this borrow is eligible
                return;

            if (Joint.Checked)
                Offset.AddDDSOFComment();
            else
                Offset.ProcessOffset();

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Validates the fields in the State Offset Processing group
        /// </summary>
        /// <returns>True: All data is available and valid; False: Data is missing or incorrect</returns>
        private bool ValidateStateOffset()
        {
            string errorMessage = "There are errors with the following field(s):\n\n";

            if (!GetSSN()) errorMessage += "SSN/Account Number invalid\n";
            if (!GetGarnishmentAmount()) errorMessage += "Garnishment Amount must be a number and under $99,999.99\n";
            if (GetListDate()) errorMessage += "List Date is not in a valid date format\n";
            if (!GetWarrantNumber()) errorMessage += "Warrant Number: Must be 10 digits and a number\n";
            if (!Single.Checked && !Joint.Checked) errorMessage += "Select either Single or Joint\n";
            else Demo.IsSingle = Single.Checked ? true : false;
            if (!GetAddress1()) errorMessage += "Address 1\n";
            if (!GetCity()) errorMessage += "City\n";
            if (!GetState()) errorMessage += "State\n";
            if (!GetZip()) errorMessage += "Zip Code";

            if (errorMessage.Length > 50)
            {
                MessageBox.Show(errorMessage, "Errors Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Gets the SSN and Account number for the number input
        /// </summary>
        /// <returns>True: When SSN and Account Number are valid; False: Invalid SSN/Account number</returns>
        private bool GetSSN()
        {
            if (!SSN.Text.IsNullOrEmpty() && SSN.Text.Length >= 9 && SSN.Text.Length <= 10)
            {
                try
                {
                    int num = int.Parse(SSN.Text);
                    if (SSN.Text.Length == 9)
                    {
                        Demo.SSN = SSN.Text;
                        Offset.GetAccountNumber();
                    }
                    else if (SSN.Text.Length == 10)
                    {
                        Offset.GetSSN();
                        Demo.AccountNumber = SSN.Text;
                    }
                    if (!Demo.SSN.IsNullOrEmpty() && !Demo.AccountNumber.IsNullOrEmpty())
                        return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Validates that the garnishment amount is a double and less than $99,999.99
        /// </summary>
        /// <returns></returns>
        private bool GetGarnishmentAmount()
        {
            if (!GarnishAmount.Text.IsNullOrEmpty())
            {
                try
                {
                    double num = double.Parse(GarnishAmount.Text.Replace("$", ""));
                    if (num > 99999.99)
                        throw new Exception();
                    Demo.GarnishmentAmount = GarnishAmount.Text;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Validate the List Date is in a valid date format
        /// </summary>
        /// <returns>True: Valid date format; False: Invalid date format</returns>
        private bool GetListDate()
        {
            if (!ListDate.Text.IsNullOrEmpty() && ListDate.Text.Length == 10)
            {
                try
                {
                    Demo.ListDate = DateTime.Parse(ListDate.Text).Date;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Make sure the Warrant Number is 10 digits and is a number
        /// </summary>
        /// <returns>True: 10 digits and numberic only; False: less than 10 digits or not all numeric</returns>
        private bool GetWarrantNumber()
        {
            if (!WarrantNumber.Text.IsNullOrEmpty() && WarrantNumber.Text.Length == 10)
            {
                try
                {
                    long num = long.Parse(WarrantNumber.Text);
                    Demo.WarrantNumber = WarrantNumber.Text;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// sets Address 1 and 2
        /// </summary>
        /// <returns>True: Address 1 was supplied; False: Address 1 was not supplied</returns>
        private bool GetAddress1()
        {
            if (!Address1.Text.IsNullOrEmpty())
            {
                Demo.Address1 = Address1.Text;
                Demo.Address2 = Address2.Text;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets the borrower city
        /// </summary>
        /// <returns>True: City was supplied; False: City was not supplied</returns>
        private bool GetCity()
        {
            if (!City.Text.IsNullOrEmpty())
            {
                Demo.City = City.Text;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets the borrower state
        /// </summary>
        /// <returns>True: State was selected; False: state was not selected</returns>
        private bool GetState()
        {
            if (!State.Text.IsNullOrEmpty())
            {
                Demo.State = State.Text;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set the borrower zip code
        /// </summary>
        /// <returns>True: Zip code was supplied and was all numbers; False: Zip code was missing or had invalid characters</returns>
        private bool GetZip()
        {
            if (!ZipCode.Text.IsNullOrEmpty())
            {
                try
                {
                    int num = int.Parse(ZipCode.Text);
                    Demo.Zip = ZipCode.Text;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        private void Update_Click(object sender, EventArgs e)
        {
            Address1.Enabled = true;
            Address2.Enabled = true;
            City.Enabled = true;
            State.Enabled = true;
            ZipCode.Enabled = true;
            Demo.UpdateAddress = true;
        }

        /// <summary>
        /// Loads the LP22 demo info into the controls, clears the controls if no demo info.
        /// </summary>
        private void LoadDemoControls()
        {
            if (Demo.Address1.IsNullOrEmpty())
            {
                Address1.Text = "";
                Address2.Text = "";
                City.Text = "";
                State.Text = "";
                ZipCode.Text = "";
            }
            else
            {
                Address1.Text = Demo.Address1;
                Address2.Text = Demo.Address2;
                City.Text = Demo.City;
                State.Text = Demo.State;
                ZipCode.Text = Demo.Zip;
            }
        }

        private void SSN_TextChanged(object sender, EventArgs e)
        {
            ProcessOk.Enabled = false;
            if (SSN.Text.Length > 7)
            {
                Offset.LoadBorrowerDemo(SSN.Text);
                LoadDemoControls();
                if (!Address1.Text.IsNullOrEmpty())
                    ProcessOk.Enabled = true;
            }
        }

        private void GarnishAmount_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!GarnishAmount.Text.IsNullOrEmpty())
                    GarnishAmount.Text = string.Format("{0:C}", (double.Parse(GarnishAmount.Text.Replace("$", ""))));
            }
            catch (Exception)
            {
                MessageBox.Show("The Amount must be in a dollar format", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GarnishAmount.Text = "";
            }
        }
    }
}

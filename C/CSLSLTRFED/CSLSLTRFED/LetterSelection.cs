using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;

namespace CSLSLTRFED
{
    public partial class LetterSelection : Form
    {
        public LetterData SelectedChoice { get; set; }
        public LetterData LastSelectedChoice { get; set; }
        public int DefForbIndex { get; set; }
        public DateTime DefForbDate { get; set; }
        public List<LetterData> AdditionDenialReasons { get; set; }
        public List<LetterData> OriginalReasons { get; set; }
        private bool HasAdditionDenialReasons { get; set; }
        private InputData UserData { get; set; }
        private List<LetterData> Letters { get; set; }

        public LetterSelection(InputData data, List<LetterData> letters)
        {
            InitializeComponent();
            Letters = letters;
            cboChoices.DrawMode = DrawMode.OwnerDrawFixed;
            cboLetterType.DataSource = Letters.OrderBy(p => p.LetterType).Select(p => p.LetterType).Distinct().ToList();
            cboLetterType.SelectedIndex = -1;
            cboChoices.SelectedIndex = -1;
            AdditionDenialReasons = new List<LetterData>();
            UserData = data;
            dtpDefForbDate.MinDate = dtpLastDate.MaxDate = dtpLoanTermEndDate.MinDate = dtpSchoolClosure.MaxDate = DateTime.Now.Date;

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            CurrentVersion.Text = $"Version: {version.Major}.{version.Build}.{version.Revision}";
            Disable();
        }

        private void cboLetterType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cboLetterOptions.DataSource = null;
            cboLetterOptions.SelectedIndex = -1;
            cboChoices.DataSource = null;
            cboChoices.SelectedIndex = -1;
            Disable();
            RemoveAdditionalReasons();
            cboLetterOptions.DataSource = Letters.Where(p => p.LetterType == cboLetterType.SelectedItem.ToString()).OrderBy(p => p.LetterOptions).OrderBy(p => p.LetterType).Select(p => p.LetterOptions).Distinct().ToList();
            cboLetterOptions.SelectedIndex = -1;
        }

        private void cboLetterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckContinue();
        }

        private void cboLetterOptions_MouseClick(object sender, MouseEventArgs e)
        {
            if (cboLetterType.SelectedIndex > -1)
            {
                cboChoices.DataSource = null;
                cboChoices.SelectedIndex = -1;
                RemoveAdditionalReasons();
                cboManualLetters.DataSource = null;
                cboManualLetters.SelectedIndex = -1;
            }
            CheckContinue();
        }

        private void cboLetterOptions_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Disable();

            SetChoices(sender);
            if (cboChoices.Items.Count > 1)
                SetManualLetters(sender);
        }

        /// <summary>
        /// Sets up the choices drop down items
        /// </summary>
        private void SetChoices(object sender)
        {
            cboChoices.DataSource = Letters.Where(p => p.LetterType == cboLetterType.SelectedItem.ToString() && p.LetterOptions == cboLetterOptions.SelectedItem.ToString()).OrderBy(p => p.LetterChoices).Select(p => p.LetterChoices).ToList();
            cboChoices.SelectedIndex = -1;

            ComboBox senderComboBox = (ComboBox)sender;
            Graphics g = senderComboBox.CreateGraphics();
            Font font = senderComboBox.Font;
            int vertScrollBarWidth = (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems) ? SystemInformation.VerticalScrollBarWidth : 0;

            int maxWidth = 350, temp = 0;
            foreach (var obj in cboChoices.Items)
            {
                temp = TextRenderer.MeasureText(obj.ToString(), cboChoices.Font).Width;
                if (temp > maxWidth)
                    maxWidth = temp;
            }

            cboChoices.DropDownWidth = maxWidth + vertScrollBarWidth;
        }

        /// <summary>
        /// Sets up the manual letter drop down items
        /// </summary>
        private void SetManualLetters(object sender)
        {
            cboManualLetters.DataSource = Letters.Where(p => p.LetterType == cboLetterType.SelectedItem.ToString() && p.LetterOptions == cboLetterOptions.SelectedItem.ToString() && p.AdditionalReason).Select(p => p.LetterName).Distinct().ToList();
            cboManualLetters.SelectedIndex = -1;

            ComboBox senderComboBox = (ComboBox)sender;
            Graphics g = senderComboBox.CreateGraphics();
            Font font = senderComboBox.Font;
            int vertScrollBarWidth = (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems) ? SystemInformation.VerticalScrollBarWidth : 0;

            int maxWidth = 350, temp = 0;
            foreach (var obj in cboManualLetters.Items)
            {
                temp = TextRenderer.MeasureText(obj.ToString(), cboManualLetters.Font).Width;
                if (temp > maxWidth)
                    maxWidth = temp;
            }
            cboManualLetters.DropDownWidth = maxWidth + vertScrollBarWidth;
        }

        private void cboChoices_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Disable();
            SelectedChoice = cboChoices.SelectedItem == null ? null : Letters.Where(p => p.LetterType == cboLetterType.SelectedItem.ToString() && p.LetterOptions == cboLetterOptions.SelectedItem.ToString() && p.LetterChoices == cboChoices.SelectedItem.ToString()).FirstOrDefault();

            if (SelectedChoice == null)
                return;

            CheckTheChoices(SelectedChoice);
            OriginalReasons = Letters.Where(p => p.LetterType == cboLetterType.SelectedItem.ToString() && p.LetterOptions == cboLetterOptions.SelectedItem.ToString()).ToList();
            if (SelectedChoice.AdditionalReason)
            {
                AdditionDenialReasons = new List<LetterData>();
                if (!SelectedChoice.DischargeAmount)
                    AdditionDenialReasons.AddRange(OriginalReasons.Where(p => p.LetterChoices != SelectedChoice.LetterChoices).OrderBy(p => p.LetterChoices).ToList());

                if (AdditionDenialReasons.Count == 0)
                    ckbAdditionDenial.Enabled = false;
                else
                {
                    cboAdditionalDenial.DataSource = AdditionDenialReasons.OrderBy(p => p.LetterChoices).Select(p => p.LetterChoices).ToList();
                    ckbAdditionDenial.Enabled = true;
                    lblManualDenial.Enabled = true;
                    txtManualDenial.Enabled = true;
                }
            }

            txtAccountNumber.Enabled = true;
            cboManualLetters.DataSource = null;

            CheckContinue();
        }

        private void cboManualLetters_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Disable();
            SelectedChoice = null;
            lblManualDenial.Enabled = true;
            txtManualDenial.Enabled = true;
            cboChoices.DataSource = null;

            SelectedChoice = Letters.Where(p => p.LetterType == cboLetterType.SelectedItem.ToString() && p.LetterOptions == cboLetterOptions.SelectedItem.ToString() && p.LetterName == cboManualLetters.SelectedItem.ToString()).FirstOrDefault();
            SelectedChoice.LetterChoices = "";
            CheckTheChoices(SelectedChoice);

            if (SelectedChoice == null)
                return;

            txtAccountNumber.Enabled = true;
            CheckContinue();
        }

        private void cboChoices_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index > -1)
            {
                string text = this.cboChoices.GetItemText(cboChoices.Items[e.Index]);
                e.DrawBackground();
                using (SolidBrush br = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(text, e.Font, br, e.Bounds);
                }

                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    this.toolTip1.ShowAlways = true;//(text, cboChoices, e.Bounds.Right, e.Bounds.Bottom);
                    this.toolTip1.SetToolTip(cboChoices, text);
                }
                else
                    this.toolTip1.Hide(cboChoices);

                e.DrawFocusRectangle();
            }
        }

        private void cboChoices_DropDownClosed(object sender, EventArgs e)
        {
            this.toolTip1.Hide(cboChoices);
        }

        private void ckbAdditionDenial_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbAdditionDenial.Checked)
            {
                if (!ValidateChoice())
                {
                    ckbAdditionDenial.Checked = false;
                    return;
                }
                lblAdditionDenialResons.Visible = true;
                cboAdditionalDenial.Visible = true;
                lsvAdditionalReasons.Visible = true;
                cboChoices.Enabled = false;

                int maxWidth = 350, temp = 0;
                foreach (var obj in cboAdditionalDenial.Items)
                {
                    temp = TextRenderer.MeasureText(obj.ToString(), cboAdditionalDenial.Font).Width;
                    if (temp > maxWidth)
                        maxWidth = temp;
                }
                cboAdditionalDenial.DropDownWidth = maxWidth;
                cboAdditionalDenial.SelectedIndex = -1;
            }
            else
            {
                lblAdditionDenialResons.Visible = false;
                cboAdditionalDenial.Visible = false;
                lsvAdditionalReasons.Visible = false;
                cboChoices.Enabled = true;
                RemoveAdditionalReasons();
            }
        }

        private void cboAdditionalDenial_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!ValidateLastChoice())
            {
                cboAdditionalDenial.SelectedIndex = -1;
                return;
            }
            int manual = 0;
            if (txtManualDenial.Enabled && txtManualDenial.Text.IsPopulated())
                manual = 1;
            CheckTheChoices(AdditionDenialReasons.Where(p => p.LetterChoices == cboAdditionalDenial.Text).FirstOrDefault());
            if ((lsvAdditionalReasons.Items.Count + manual) < 4)
            {
                txtManualDenial.Enabled = true;
                cboAdditionalDenial.Enabled = true;
                AdditionDenialReasons.Remove(AdditionDenialReasons.Where(p => p.LetterChoices == cboAdditionalDenial.Text).FirstOrDefault());
                lsvAdditionalReasons.Items.Add(cboAdditionalDenial.SelectedItem.ToString());
                LastSelectedChoice = Letters.Where(p => p.LetterChoices == cboAdditionalDenial.SelectedItem.ToString()).FirstOrDefault();
                cboAdditionalDenial.DataSource = null;
                cboAdditionalDenial.DataSource = AdditionDenialReasons.OrderBy(p => p.LetterChoices).Select(p => p.LetterChoices).ToList();
            }
            if (lsvAdditionalReasons.Items.Count == 3 && manual == 1)
                cboAdditionalDenial.Enabled = false;
            else if (lsvAdditionalReasons.Items.Count == 4)
            {
                lblManualDenial.Enabled = false;
                txtManualDenial.Enabled = false;
            }

            cboAdditionalDenial.SelectedIndex = -1;

        }

        private void cboAdditionalDenial_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index > -1)
            {
                string text = this.cboChoices.GetItemText(cboAdditionalDenial.Items[e.Index]);
                e.DrawBackground();
                using (SolidBrush br = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(text, e.Font, br, e.Bounds);
                }

                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    this.toolTip1.ShowAlways = true;
                    this.toolTip1.SetToolTip(cboAdditionalDenial, text);
                }
                else
                    this.toolTip1.Hide(cboAdditionalDenial);
                e.DrawFocusRectangle();
            }
        }

        private void cboAdditionalDenial_DropDownClosed(object sender, EventArgs e)
        {
            this.toolTip1.Hide(cboAdditionalDenial);
        }

        private void lsvAdditionalReasons_DoubleClick(object sender, EventArgs e)
        {
            if (lsvAdditionalReasons.SelectedItem != null)
            {
                LetterData removeChoice = OriginalReasons.Where(p => p.LetterChoices == lsvAdditionalReasons.SelectedItem.ToString()).FirstOrDefault();
                AdditionDenialReasons.Add(removeChoice);
                cboAdditionalDenial.DataSource = null;
                cboAdditionalDenial.DataSource = AdditionDenialReasons.OrderBy(p => p.LetterChoices).Select(p => p.LetterChoices).ToList();
                lsvAdditionalReasons.Items.RemoveAt(lsvAdditionalReasons.SelectedIndex);
                cboAdditionalDenial.SelectedIndex = -1;
                if (lsvAdditionalReasons.Items.Count < 4)
                {
                    cboAdditionalDenial.Enabled = true;
                    lblManualDenial.Enabled = true;
                    txtManualDenial.Enabled = true;
                }
                LastSelectedChoice = lsvAdditionalReasons.Items.Count > 0 ? Letters.Where(p => p.LetterChoices == lsvAdditionalReasons.Items[lsvAdditionalReasons.Items.Count - 1].ToString()).FirstOrDefault() : new LetterData();
                CheckTheChoices(LastSelectedChoice);
                if (removeChoice.DefForbType)
                {
                    DefForbIndex = cboDefForb.SelectedIndex;
                    cboDefForb.SelectedIndex = -1;
                    DefForbDate = dtpDefForbDate.Value;
                    dtpDefForbDate.Value = DateTime.Now;
                }
            }
            else
                Dialog.Error.Ok("Please choose a listed item", "No items selected");
        }

        private void txtDischargeAmt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Double.Parse(txtDischargeAmt.Text);
            }
            catch (Exception)
            {
                if (!string.IsNullOrEmpty(txtDischargeAmt.Text))
                {
                    Dialog.Info.Ok("You did not enter a number please try again.");
                    txtDischargeAmt.Text = txtDischargeAmt.Text.Remove((txtDischargeAmt.Text.Length - 1));
                }
            }
        }

        private void txtDischargeAmt_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            decimal d;
            if (decimal.TryParse(txtDischargeAmt.Text, out d))
                txtDischargeAmt.Text = d.ToString("0.##");
        }

        private void txtAccountNumber_TextChanged(object sender, EventArgs e)
        {
            CheckContinue();
        }

        private void txtAccountNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                btnContinue_Click(sender, new EventArgs());
        }

        private void txtManualDenial_TextChanged(object sender, EventArgs e)
        {
            if (txtManualDenial.Text.IsPopulated() && lsvAdditionalReasons.Items.Count == 3)
                cboAdditionalDenial.Enabled = false;
            else if (txtManualDenial.Text.IsNullOrEmpty() && lsvAdditionalReasons.Items.Count < 4)
                cboAdditionalDenial.Enabled = true;

            CheckContinue();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            UserData.DenialReasons = new List<string>();
            if (SelectedChoice.DischargeAmount)
            {
                if (txtDischargeAmt.Text.IsNullOrEmpty())
                {
                    Dialog.Error.Ok("You must enter the approved discharge amount.");
                    return;
                }
                UserData.AmountForDischarge = txtDischargeAmt.Text;
            }

            if (SelectedChoice.SchoolName)
            {
                if (txtSchoolName.Text.IsNullOrEmpty())
                {
                    Dialog.Error.Ok("You must enter a school name.");
                    return;
                }
                UserData.SchoolName = txtSchoolName.Text;
            }

            if (SelectedChoice.LastDateAttendance)
            {
                if (dtpLastDate.Value.Date == DateTime.Now.Date)
                {
                    if (!Dialog.Info.YesNo($"Are you sure the last date of attendance is {DateTime.Now.ToString("MM/dd/yyyy")}?", "Confirm"))
                        return;
                }
                UserData.LastDateOfAttendance = dtpLastDate.Value;
            }

            if (SelectedChoice.SchoolClosureDate)
            {
                if (dtpSchoolClosure.Value.Date == DateTime.Now.Date)
                {
                    if (!Dialog.Info.YesNo($"Are you sure the school closure date is {DateTime.Now.ToString("MM/dd/yyyy")}?", "Confirm"))
                        return;
                }
                UserData.SchoolClosureDate = dtpSchoolClosure.Value;
            }

            if (SelectedChoice.DefForbType)
            {
                if (cboDefForb.SelectedIndex < 0)
                {
                    Dialog.Info.Ok("You must enter a deferment/forbearance type.");
                    return;
                }
                UserData.DefermentForbearanceType = cboDefForb.SelectedValue.ToString();
            }

            if (SelectedChoice.DefForbEndDate)
            {
                if (dtpDefForbDate.Value.Date == DateTime.Now.Date)
                {
                    if (!Dialog.Info.YesNo($"Are you sure the deferment/forbearance end date is {DateTime.Now.ToString("MM/dd/yyyy")}?", "Confirm"))
                        return;
                }

                UserData.DefForbEndDate = dtpDefForbDate.Value;
            }

            if (SelectedChoice.LoanTermEndDate)
            {
                if (dtpLoanTermEndDate.Value.Date == DateTime.Now.Date)
                {
                    if (!Dialog.Info.YesNo($"Are you sure the loan term end date is {DateTime.Now.ToString("MM/dd/yyyy")}?", "Confirm"))
                        return;
                }

                UserData.LoanTermEndDate = dtpLoanTermEndDate.Value;
                SelectedChoice.LetterChoices = SelectedChoice.LetterChoices.Replace("MM/DD/CCY2", dtpLoanTermEndDate.Value.ToString("MM/dd/yyyy"));
                UserData.DenialReasons.Add(SelectedChoice.LetterChoices.Replace("MM/DD/CCY2", dtpLoanTermEndDate.Value.ToString("MM/dd/yyyy")));
            }

            if (SelectedChoice.SchoolYear)
            {
                if (txtBeginYear.Text.IsNullOrEmpty() || txtBeginYear.Text.Length != 4)
                {
                    Dialog.Info.Ok("You must enter the first 4 digit year the school appeared on the Low Income Directory.");
                    return;
                }
                if (txtEndYear.Text.IsNullOrEmpty() || txtBeginYear.Text.Length != 4)
                {
                    Dialog.Info.Ok("You must enter the last 4 digit year the school appeared on the Low Income Directory.");
                    return;
                }

                UserData.LowDirectoryBegin = txtBeginYear.Text;
                UserData.LowDirectoryEnd = txtEndYear.Text;
                SelectedChoice.LetterChoices = SelectedChoice.LetterChoices.Replace("CCYY-", $"{txtBeginYear.Text}-");
                SelectedChoice.LetterChoices = SelectedChoice.LetterChoices.Replace("-CCYY", $"-{txtEndYear.Text}");
            }

            UserData.AccountNumber = txtAccountNumber.Text;
            UserData.LoanServicingLettersId = SelectedChoice.LoanServicingLettersId;
            UserData.LetterType = cboLetterType.SelectedValue.ToString();
            UserData.LetterOption = cboLetterOptions.SelectedValue.ToString();

            if (Letters.Where(p => p.LetterChoices == SelectedChoice.LetterChoices).Select(p => p.DefForbType).FirstOrDefault() && Letters.Where(p => p.LetterChoices == SelectedChoice.LetterChoices).Select(p => p.DefForbEndDate).FirstOrDefault())
                UserData.DenialReasons.Add(SelectedChoice.LetterChoices.Replace("MM/DD/CCY1", dtpDefForbDate.Value.ToString("MM/dd/yyyy")).Replace("[deferment/forbearance]", UserData.DefermentForbearanceType));
            else if (SelectedChoice.LetterChoices.IsPopulated())
                UserData.DenialReasons.Add(SelectedChoice.LetterChoices);

            foreach (string item in lsvAdditionalReasons.Items)
            {
                if (!ValidateReason(Letters.Where(p => p.LetterChoices == item).FirstOrDefault()))
                    return;
                if (Letters.Where(p => p.LetterChoices == item).Select(p => p.DefForbType).FirstOrDefault() && Letters.Where(p => p.LetterChoices == item).Select(p => p.DefForbEndDate).FirstOrDefault())
                    UserData.DenialReasons.Add(item.Replace("MM/DD/CCY1", dtpDefForbDate.Value.ToString("MM/dd/yyyy")).Replace("[deferment/forbearance]", UserData.DefermentForbearanceType));
                else
                    UserData.DenialReasons.Add(item);
            }

            if (txtManualDenial.Enabled && txtManualDenial.Text.IsPopulated())
                UserData.DenialReasons.Add(txtManualDenial.Text);


            DialogResult = DialogResult.OK;
        }

        private List<string> GetDefermentForbearanceTypes()
        {
            List<string> combinedDefForb = new List<string>();

            List<string> deferments = Letters.Where(p => p.LetterType.ToLower().IsIn("deferment")).Select(p => p.LetterOptions).Distinct().ToList();
            List<string> forbearances = Letters.Where(p => p.LetterType.ToLower().IsIn("forbearance")).Select(p => p.LetterOptions).Distinct().ToList();
            foreach (string item in deferments)
                combinedDefForb.Add($"{item} Deferment");
            foreach (string item in forbearances)
                combinedDefForb.Add($"{item} Forbearance");

            combinedDefForb.Insert(0, "");
            return combinedDefForb;
        }

        private void CheckTheChoices(LetterData choice)
        {
            lblAmtAprv.Enabled = choice.DischargeAmount;
            txtDischargeAmt.Enabled = choice.DischargeAmount;
            lblSchoolName.Enabled = choice.SchoolName;
            txtSchoolName.Enabled = choice.SchoolName;
            lblLastdate.Enabled = choice.LastDateAttendance;
            dtpLastDate.Enabled = choice.LastDateAttendance;
            lblDateOfClosure.Enabled = choice.SchoolClosureDate;
            dtpSchoolClosure.Enabled = choice.SchoolClosureDate;
            lblDefForb.Enabled = choice.DefForbType;
            cboDefForb.Enabled = choice.DefForbType;
            if (choice.DefForbType && cboDefForb.DataSource == null)
                cboDefForb.DataSource = GetDefermentForbearanceTypes();
            lblDefForbDate.Enabled = choice.DefForbEndDate;
            dtpDefForbDate.Enabled = choice.DefForbEndDate;
            lblLoanTermEndDate.Enabled = choice.LoanTermEndDate;
            dtpLoanTermEndDate.Enabled = choice.LoanTermEndDate;
            lblLowIncome.Enabled = choice.SchoolYear;
            lblDash.Enabled = choice.SchoolYear;
            txtBeginYear.Enabled = choice.SchoolYear;
            txtEndYear.Enabled = choice.SchoolYear;
            ckbAdditionDenial.Enabled = choice.AdditionalReason && !cboManualLetters.Enabled;
        }

        private void Disable()
        {
            txtDischargeAmt.Enabled = false;
            txtDischargeAmt.Text = "";
            txtSchoolName.Enabled = false;
            txtSchoolName.Text = "";
            dtpLastDate.Enabled = false;
            dtpSchoolClosure.Enabled = false;
            cboDefForb.Enabled = false;
            cboDefForb.SelectedIndex = -1;
            dtpDefForbDate.MinDate = DateTime.Now;
            dtpDefForbDate.Enabled = false;
            dtpLoanTermEndDate.MinDate = DateTime.Now;
            dtpLoanTermEndDate.Enabled = false;
            txtBeginYear.Enabled = false;
            txtBeginYear.Text = "";
            txtEndYear.Enabled = false;
            txtEndYear.Text = "";
            lblAdditionDenialResons.Visible = false;
            cboAdditionalDenial.Visible = false;
            lsvAdditionalReasons.Visible = false;
            cboChoices.Enabled = true;
            if (txtAccountNumber.Text.Length < 9)
                btnContinue.Enabled = false;
            txtAccountNumber.Enabled = false;
            ckbAdditionDenial.Checked = false;
            ckbAdditionDenial.Enabled = false;
            lblManualDenial.Enabled = false;
            txtManualDenial.Enabled = false;
            txtManualDenial.Text = "";
        }

        private void RemoveAdditionalReasons()
        {
            while (lsvAdditionalReasons.Items.Count > 0)
                lsvAdditionalReasons.Items.RemoveAt(0);

            if (cboLetterOptions.SelectedItem != null && cboLetterOptions.SelectedItem.ToString().IsPopulated())
            {
                OriginalReasons = Letters.Where(p => p.LetterType == cboLetterType.SelectedItem.ToString() && p.LetterOptions == cboLetterOptions.SelectedItem.ToString()).ToList();
                AdditionDenialReasons.AddRange(OriginalReasons);
            }

            cboAdditionalDenial.DataSource = AdditionDenialReasons.OrderBy(p => p.LetterChoices).Select(p => p.LetterChoices).ToList();
        }

        private void CheckContinue()
        {
            if ((txtAccountNumber.Text.Length == 10 || txtAccountNumber.Text.Length == 9) && cboLetterType.Text.ToString().IsPopulated() && cboLetterOptions.Text.ToString().IsPopulated() && (cboChoices.Text.ToString().IsPopulated() || (cboManualLetters.Enabled && txtManualDenial.Text.IsPopulated())))
                btnContinue.Enabled = true;
            else
                btnContinue.Enabled = false;
        }

        private bool ValidateChoice()
        {
            if (SelectedChoice.DefForbType && cboDefForb.Text.IsNullOrEmpty())
            {
                Dialog.Error.Ok("You must provide a deferment or forbearance type first.");
                return false;
            }
            if (SelectedChoice.DefForbType && dtpDefForbDate.Value.Date == DateTime.Now.Date)
            {
                Dialog.Error.Ok("You must provide a future deferment or forbearance end date first.");
                return false;
            }
            if (SelectedChoice.LoanTermEndDate && dtpLoanTermEndDate.Value.Date == DateTime.Now.Date)
            {
                Dialog.Error.Ok("You must provide a future loan term end date first.");
                return false;
            }
            return true;
        }

        private bool ValidateLastChoice()
        {
            if (LastSelectedChoice != null)
            {
                if (LastSelectedChoice.DefForbType && cboDefForb.Text.IsNullOrEmpty())
                {
                    Dialog.Error.Ok("You must provide a deferment or forbearance type first.");
                    return false;
                }
                if (LastSelectedChoice.DefForbType && dtpDefForbDate.Value.Date == DateTime.Now.Date)
                {
                    Dialog.Error.Ok("You must provide a future deferment or forbearance end date first.");
                    return false;
                }
                if (LastSelectedChoice.LoanTermEndDate && dtpLoanTermEndDate.Value.Date == DateTime.Now.Date)
                {
                    Dialog.Error.Ok("You must provide a future loan term end date first.");
                    return false;
                }
            }
            return true;
        }

        private bool ValidateReason(LetterData selectedLetter)
        {
            if (selectedLetter.DefForbType && cboDefForb.Text.IsNullOrEmpty())
            {
                Dialog.Error.Ok("You must provide a deferment or forbearance type first.");
                return false;
            }
            if (selectedLetter.DefForbType && dtpDefForbDate.Value.Date == DateTime.Now.Date)
            {
                Dialog.Error.Ok("You must provide a future deferment or forbearance end date first.");
                return false;
            }
            if (selectedLetter.LoanTermEndDate && dtpLoanTermEndDate.Value.Date == DateTime.Now.Date)
            {
                Dialog.Error.Ok("You must provide a future loan term end date first.");
                return false;
            }
            return true;
        }
    }
}
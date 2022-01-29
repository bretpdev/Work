using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.WinForms;

namespace IDRUSERPRO
{
    public partial class PaystubsForm : Form
    {
        List<PayStubs> Paystubs;
        ErrorAttacher ea = new ErrorAttacher();
        public PaystubsForm(List<PayStubs> stubs)
        {
            InitializeComponent();
            Paystubs = stubs;
            foreach (var stub in stubs)
                stub.EmployerName = stub.EmployerName ?? "Employer 1";  //don't support null names
            if (!stubs.Any())
                AddEmployer("Employer 1");
            LoadEmployers();
        }

        private void AddPaystubButton_Click(object sender, EventArgs e)
        {
            foreach (PaystubControl stubControl in PaystubsPanel.Controls)
                stubControl.HasHadFocus = true;
            var stub = new PayStubs();
            stub.EmployerName = (string)EmployersBox.SelectedItem;
            stub.PayFrequency = GetFrequency() ?? stub.PayFrequency;
            Paystubs.Add(stub);
            AddPaystubToForm(stub);
        }

        private void AddEmployer(string name)
        {
            var namedStub = new PayStubs() { EmployerName = name };
            Paystubs.Add(namedStub);
            LoadEmployers();
            EmployersBox.SelectedItem = name;
        }
        private void LoadEmployers(string selectionToMaintain = null)
        {
            var newSource = Paystubs.Select(o => o.EmployerName ?? "").Distinct().ToList();
            var oldSource = EmployersBox.DataSource as List<string> ?? new List<string>();
            var bothSource = newSource.Intersect(oldSource).ToList();
            if (bothSource.Count != newSource.Count || bothSource.Count != oldSource.Count)
            {
                if (!newSource.Any())
                    newSource.Add("Employer 1");
                using (ch.TemporarilyDisableEvent(EmployersBox, EmployersBox_SelectedIndexChanged))
                {
                    EmployersBox.DataSource = newSource;
                    if (selectionToMaintain == null)
                        LoadCurrentEmployer();
                    else
                        EmployersBox.SelectedItem = selectionToMaintain;
                }
            }
        }

        ControlHelper ch = new ControlHelper();
        private void LoadCurrentEmployer()
        {
            using (ch.TemporarilyDisableEvent(EmployerNameBox, EmployerNameBox_TextChanged))
            {
                string employerName = (string)EmployersBox.SelectedItem;
                var stubs = Paystubs.Where(o => o.EmployerName == employerName);
                if (EmployerNameBox.Text == employerName && PaystubsPanel.Controls.Count == stubs.Count())
                    return;  //nothing changed
                PaystubsPanel.Controls.Clear();
                PerformValidation();

                if (EmployersBox.SelectedIndex == -1)
                {
                    EmployerGroup.Enabled = false;
                    EmployerNameBox.Text = "";
                    PaystubsGroup.Enabled = false;
                    AddPaystubButton.Enabled = false;
                    return;
                }
                EmployerGroup.Enabled = true;
                EmployerNameBox.Text = employerName;

                PaystubsGroup.Enabled = true;
                foreach (var stub in stubs)
                    AddPaystubToForm(stub);

                var firstStub = stubs.FirstOrDefault();
                if (firstStub != null)
                {
                    PayFrequency.SelectedItem = firstStub.PayFrequency.ToString();
                    if (firstStub.PayFrequency == PayStubs.Frequency.BiWeekly)
                        PayFrequency.SelectedItem = "Bi-Weekly";
                    if (firstStub.PayFrequency == PayStubs.Frequency.SemiMonthly)
                        PayFrequency.SelectedItem = "Semi-Monthly";
                    if (firstStub.PayFrequency == 0)
                        PayFrequency.SelectedIndex = -1;
                }
                else
                    PayFrequency.SelectedIndex = -1;

                PerformValidation();
                employerHasHadFocus = false;
                EmployerNameBox.Focus();
            }
        }

        private bool employerHasHadFocus = false;
        public void PerformValidation()
        {
            ea.ClearAllErrors();
            var regex = new Regex(@"[^a-zA-Z0-9\s]");
            if (EmployerNameBox.Text.IsNullOrEmpty())
                ea.SetError("Employer Name required.", EmployerNameBox, EmployerNameLabel);
            else if (regex.IsMatch(EmployerNameBox.Text))
                ea.SetError("Employer Name cannot contain special characters.", EmployerNameBox, EmployerNameLabel);

            foreach (PaystubControl stub in PaystubsPanel.Controls)
                stub.PerformValidation(ea, PaystubsPanel.Controls.Count);
            if (GetFrequency() == null || GetFrequency() == 0)
            {
                if (employerHasHadFocus)
                    ea.SetError("Frequency required.", PayFrequency, FrequencyLabel);
                else
                    ea.AdditionalErrorCount++;
            }


            var noErrors = ea.ErrorCount == 0;
            bool onlyOneUnfilledStub = Paystubs.Count == 1 && Paystubs.Single().EmployerBorrowerAltIncome == 0;
            OkButton.Enabled = noErrors || onlyOneUnfilledStub;
            EmployersBox.Enabled = AddEmployerButton.Enabled = noErrors && Paystubs.Any();

            var employerName = (string)EmployersBox.SelectedItem;
            var employerIncome = AdoiCalculator.AverageEmployerPaystubs(Paystubs, employerName);
            var altIncome = AdoiCalculator.SumPaystubs(Paystubs);
            TotalsLabel.Text = string.Format("Total Alt Income: {0:0.00}    Total {1} Income: {2:0.00}", altIncome, employerName, employerIncome);
        }

        public void AddPaystubToForm(PayStubs stub)
        {
            var stubControl = new PaystubControl();
            stubControl.LoadStub(stub, RemoveStub, PerformValidation);
            stubControl.Dock = DockStyle.Top;
            PaystubsPanel.Controls.Add(stubControl);
            stubControl.BringToFront(); //move to bottom of panel
            stubControl.Focus();
            PerformValidation();
        }

        public void RemoveStub(PaystubControl control, PayStubs stub)
        {
            Paystubs.Remove(stub);
            control.Parent.Controls.Remove(control);
            LoadEmployers();
            PerformValidation();
        }

        private void EmployersBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCurrentEmployer();
        }

        private void AddEmployerButton_Click(object sender, EventArgs e)
        {
            var newTempName = "Employer ";
            var newTempIndex = 1;
            while (Paystubs.Any(o => o.EmployerName == newTempName + newTempIndex))
                newTempIndex++;
            AddEmployer(newTempName + newTempIndex);
        }

        private void RemoveEmployerButton_Click(object sender, EventArgs e)
        {
            var employerName = EmployerNameBox.Text;
            var matches = Paystubs.Where(o => o.EmployerName == employerName).ToArray();
            if (Dialog.Def.YesNo(string.Format("Really remove the paystubs corresponding to employer \"{0}\"?", employerName)))
            {
                foreach (var match in matches)
                    Paystubs.Remove(match);
                LoadEmployers();
            }
        }

        private PayStubs.Frequency? GetFrequency()
        {
            var frequency = EnumParser.Parse<PayStubs.Frequency>(PayFrequency.Text.Replace("-", ""));
            return frequency;
        }
        private void PayFrequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            var frequency = GetFrequency();
            foreach (PaystubControl stub in PaystubsPanel.Controls)
                stub.Frequency = frequency ?? stub.Frequency;
        }

        private void EmployerGroup_Leave(object sender, EventArgs e)
        {
            employerHasHadFocus = true;
        }

        private void EmployerNameBox_TextChanged(object sender, EventArgs e)
        {
            PerformValidation();
            var oldName = EmployersBox.SelectedItem as string;
            var newName = EmployerNameBox.Text;
            if (oldName != newName)
            {
                var matches = Paystubs.Where(o => o.EmployerName == oldName);
                foreach (var match in matches)
                    match.EmployerName = newName;
                LoadEmployers(newName);
            }
        }
    }
}

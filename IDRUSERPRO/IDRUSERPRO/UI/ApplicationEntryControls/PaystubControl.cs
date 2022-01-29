using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.WinForms;

namespace IDRUSERPRO
{
    public partial class PaystubControl : UserControl
    {
        public PaystubControl()
        {
            InitializeComponent();
        }

        PayStubs stub;
        Action<PaystubControl, PayStubs> onRemove;
        Action onValueChange;
        const string TwoDecimals = "0.00";
        public void LoadStub(PayStubs stub, Action<PaystubControl, PayStubs> onRemove, Action onValueChange)
        {
            this.onRemove = onRemove;
            this.onValueChange = onValueChange;
            FtwBox.Text = stub.Ftw.ToString(TwoDecimals);
            GrossBox.Text = stub.Gross.ToString(TwoDecimals);
            DeductionsBox.Text = stub.TotalPreTax.ToString(TwoDecimals);
            BonusBox.Text = stub.Bonus.ToString(TwoDecimals);
            OvertimeBox.Text = stub.Overtime.ToString(TwoDecimals);
            this.stub = stub;
            SavePaystub();
        }

        public PayStubs.Frequency Frequency
        {
            get { return stub.PayFrequency; }
            set
            {
                stub.PayFrequency = value;
                SavePaystub();
            }
        }

        private void SavePaystub()
        {
            if (stub == null)
                return;
            stub.Ftw = FtwBox.Text.ToDecimal();
            stub.Gross = GrossBox.Text.ToDecimal();
            if (stub.Ftw > 0)
                GrossBox.Enabled = false;
            if (stub.Gross > 0)
                FtwBox.Enabled = false;
            if ((stub.Ftw > 0 && stub.Gross > 0) || (stub.Ftw == 0 && stub.Gross == 0))
                FtwBox.Enabled = GrossBox.Enabled = true;  //invalid situations the user must be able to fix
            DeductionsBox.Enabled = GrossBox.Enabled;
            if (!GrossBox.Enabled)
                GrossBox.Text = TwoDecimals;
            if (!DeductionsBox.Enabled)
                DeductionsBox.Text = TwoDecimals;
            if (!FtwBox.Enabled)
                FtwBox.Text = TwoDecimals;
            stub.TotalPreTax = DeductionsBox.Text.ToDecimal();
            stub.Bonus = BonusBox.Text.ToDecimal();
            stub.Overtime = OvertimeBox.Text.ToDecimal();
            AgiBox.Text = stub.AnnualTaxableGross.ToString(TwoDecimals);
            onValueChange();
        }

        public void PerformValidation(ErrorAttacher ea, int stubCount)
        {
            if (stub == null)
                return;
            var setError = new Action<string, Control, Label>((s, c, l) =>
            {
                if (HasHadFocus)
                    ea.SetError(s, c, l);
                else
                    ea.AdditionalErrorCount++;
            });
            if (stub.Ftw == 0 && stub.Gross == 0 && stubCount != 1)
            {
                setError("FTW or Gross required.", FtwBox, FtwLabel);
                setError("Gross of FTW required.", GrossBox, GrossLabel);
            }
            foreach (var box in AmountControls)
            {
                if (box.Item1.Text.ToDecimalNullable() == null)
                    setError("Please enter a valid amount.", box.Item1, box.Item2);
                else if (box.Item1.Text.ToDecimalNullable() > 9999999)
                    setError("Please enter an amount no more than 7 digits prior to the decimal.", box.Item1, box.Item2);
                else
                {
                    string afterDecimal = box.Item1.Text.Split('.').Skip(1).SingleOrDefault();
                    if (afterDecimal != null && afterDecimal.Length > 2)
                        setError("Please enter no more than two digits after the decimal place", box.Item1, box.Item2);
                }
            }
        }

        private IEnumerable<Tuple<NumericDecimalTextBox, Label>> AmountControls
        {
            get
            {
                return new Tuple<NumericDecimalTextBox, Label>[]
                {
                    Tuple.Create(FtwBox, FtwLabel),
                    Tuple.Create(GrossBox, GrossLabel),
                    Tuple.Create(DeductionsBox, DeductionsLabel),
                    Tuple.Create(BonusBox, BonusLabel),
                    Tuple.Create(OvertimeBox, OvertimeLabel)
                };
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            onRemove(this, stub);
        }

        private void Control_TextChanged(object sender, EventArgs e)
        {
            SavePaystub();
            HasHadFocus = true;
        }

        private void Control_Enter(object sender, EventArgs e)
        {
            var control = sender as NumericDecimalTextBox;
            control.Select(0, control.Text.Length);
        }

        public bool HasHadFocus { get; set; }
        private void PaystubControl_Leave(object sender, EventArgs e)
        {
            HasHadFocus = true;
        }

        private void Control_Leave(object sender, EventArgs e)
        {
            //SavePaystub();
        }
    }
}

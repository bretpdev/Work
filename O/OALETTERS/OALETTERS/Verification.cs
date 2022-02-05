using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OALETTERS
{
    public partial class Verification : Form
    {

        public Verification(LetterSelection ls)
        {
            InitializeComponent();

            BorrowerData(ls);
            if (ls.IsCompany)
                CompanyData(ls);
        }

        private void BorrowerData(LetterSelection ls)
        {
            AccountNumber.Text = ls.Bor.AccountNumber;
            LetterId.Text = ls.Letter.Letter;
            BorrowerName.Text = string.Format("{0} {1}", ls.Bor.FirstName, ls.Bor.LastName);
            Address1.Text = ls.Bor.Address1;
            Address2.Text = ls.Bor.Address2;
            City.Text = ls.Bor.City;
            State.Text = ls.Bor.State;
            Zip.Text = ls.Bor.Zip;
            Country.Text = ls.Bor.Country;
            RefundAmount.Text = string.Format("${0}", ls.Bor.RefundAmount);
            EffectiveDate.Text = ls.Bor.EffectiveDate;
            PaymentSource.Text = ls.Bor.PaymentSource;
        }

        private void CompanyData(LetterSelection ls)
        {
            AccountNumber.Text = ls.Bor.AccountNumber;
            LetterId.Text = ls.Letter.Letter;
            CompanyName.Text = ls.CompanyData.FirstName;
            CompanyAddress1.Text = ls.CompanyData.Address1;
            CompanyAddress2.Text = ls.CompanyData.Address2;
            CompanyCity.Text = ls.CompanyData.City;
            CompanyState.Text = ls.CompanyData.State;
            CompanyZip.Text = ls.CompanyData.Zip;
            CompanyCountry.Text = ls.CompanyData.Country;
        }

        private void Verified_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MauiDUDE
{
    public partial class BorrowerBenefitsDisplay : UserControl
    {
        public BorrowerBenefitsDisplay()
        {
            InitializeComponent();
        }

        public BorrowerBenefitsDisplay(ServicingLoanDetail detail)
        {
            InitializeComponent();

            servicingLoanDetailBindingSource.DataSource = detail;
        }
    }
}

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
    public partial class ServicingLoanDisplay : UserControl
    {
        public ServicingLoanDisplay()
        {
            InitializeComponent();
        }

        public ServicingLoanDisplay(ServicingLoanDetail detail)
        {
            InitializeComponent();

            servicingLoanDetailBindingSource.DataSource = detail;
        }
    }
}

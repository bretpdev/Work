using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BLKADDFED.UserControls
{
    partial class PhoneRecord : UserControl
    {
        public PhoneRecord(BorrowerPhone brwPhone)
        {
            InitializeComponent();
            borrowerPhoneBindingSource.DataSource = brwPhone;

        }

    }
}

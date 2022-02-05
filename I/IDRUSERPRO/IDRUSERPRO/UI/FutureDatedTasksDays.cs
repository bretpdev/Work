using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;

namespace IDRUSERPRO
{
    public partial class FutureDatedTasksDays : Form
    {
        public decimal Days { get; set; }
        public FutureDatedTasksDays()
        {
            InitializeComponent();

            FutureDays.Value = 60;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            Days = FutureDays.Value;
        }
    }
}

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
    public partial class EmployerDemoDisplay : UserControl
    {
        /// <summary>
        /// Just give it the employer's demographics and it will databind for you
        /// </summary>
        public EmployerDemographics Demos 
        {
            set
            {
                employerDemographicsBindingSource.DataSource = value;
            } 
        }

        public EmployerDemoDisplay()
        {
            InitializeComponent();
        }
    }
}

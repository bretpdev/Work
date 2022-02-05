using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.WinForms;

namespace MauiDUDE
{

    public partial class DefermentAndForbearanceDispaly : UserControl
    {
        private DefermentForbearance Data { get; set; }
        public bool DisableGetMonths { get; set; } = false;
        public DefermentAndForbearanceDispaly()
        {
            InitializeComponent();
            if(DisableGetMonths)
            {
                buttonGetMonthsLeft.Visible = false;
            }
        }

        public DefermentAndForbearanceDispaly(DefermentForbearance defermentForbearance)
        {
            InitializeComponent();

            buttonGetMonthsLeft.Visible = true;
            Data = defermentForbearance;
            defermentForbearanceBindingSource.DataSource = defermentForbearance;
            labelMonthsLeft.Text = "";
            if (DisableGetMonths)
            {
                buttonGetMonthsLeft.Visible = false;
            }
        }

        private void buttonGetMonthsLeft_Click(object sender, EventArgs e)
        {
            Processing.MakeVisible();
            labelMonthsLeft.Text = ((UheaaHomePage)ParentForm).GetMonthsLeftForDefermentOrForbearance(Data);

            Processing.MakeInvisible();
            ParentForm.Activate();
        }

        private void DefermentAndForbearanceDispaly_Load(object sender, EventArgs e)
        {
            if (DisableGetMonths)
            {
                buttonGetMonthsLeft.Visible = false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using System.Net.Http.Headers;

namespace MauiDUDE
{
    public partial class RepaymentOptionsControl : UserControl
    {
        private HomePageSessionInteractionCoordinator HomePage;
        private List<RepaymentDetail> Current = null;
        private List<RepaymentDetail> Level = null;
        private List<RepaymentDetail> Graduated = null;
        private List<RepaymentDetail> S2 = null;
        private List<RepaymentDetail> S5 = null;

        public RepaymentOptionsControl()
        {
            InitializeComponent();
        }

        public RepaymentOptionsControl(Borrower borrower, DataAccessHelper.Region region)
        {
            InitializeComponent();

            HomePage = new HomePageSessionInteractionCoordinator(borrower, region);
            Current = HomePage.GatherCurrentOption();
            if (Current != null)
            {
                foreach (var item in Current)
                {
                    flowLayoutPanelCurrentOptions.Controls.Add(new RepaymentOptionsDisplay(item.Term, item.Amount.ToString(), item.BeginDate));
                }
            }
            Level = HomePage.GatherData("L");
            Graduated = HomePage.GatherData("G");
            S2 = HomePage.GatherData("S2");
            S5 = HomePage.GatherData("S5");
            if (Level != null || Graduated != null)
            {
                buttonStafford_PLUS_TILP.Enabled = true;
            }
            if (Level != null && (S2 != null || S5 != null))
            {
                buttonConsolidation.Enabled = true;
            }
        }

        private void buttonStafford_PLUS_TILP_Click(object sender, EventArgs e)
        {
            flowLayoutPanelDisplayOptions.Controls.Clear();
            if (Level != null)
            {
                flowLayoutPanelDisplayOptions.Controls.Add(new Label() { Text = "Level" });
                foreach (var item in Level)
                {
                    flowLayoutPanelDisplayOptions.Controls.Add(new RepaymentOptionsDisplay(item.Term, item.Amount.ToString(), item.BeginDate)); 
                }
            }
            if (Graduated != null)
            {
                flowLayoutPanelDisplayOptions.Controls.Add(new Label() { Text = "" });
                flowLayoutPanelDisplayOptions.Controls.Add(new Label() { Text = "Graduated" });
                foreach (var item in Graduated)
                {
                    flowLayoutPanelDisplayOptions.Controls.Add(new RepaymentOptionsDisplay(item.Term, item.Amount.ToString(), item.BeginDate)); 

                }
            }
        }

        private void buttonConsolidation_Click(object sender, EventArgs e)
        {

        }

        private void buttonPrivateLoans_Click(object sender, EventArgs e)
        {

        }
    }
}

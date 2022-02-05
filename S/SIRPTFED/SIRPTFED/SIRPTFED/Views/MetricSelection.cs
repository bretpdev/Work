using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using SIRPTFED.Models;
using Uheaa.Common;


namespace SIRPTFED
{
    public partial class MetricSelection : Form
    {
        private List<MetricSummaryData> Data { get; set; }
        private CurrentUser User { get; set; }

        private DataAccess da { get; set; }

        public MetricSelection(DataAccess DA)
        {
            InitializeComponent();
            da = DA;
            LoadData();
            
            
        }

        private void LoadData()
        {
            User = da.GetCurrentUser();
            CheckAccess(User);
            Data = da.Populate();
            int endMonth = DateTime.Now.Month;
            int beginMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
            Data = Data.Where(p => p.MetricMonth.BetweenInc(beginMonth, endMonth)).ToList();
            Metrics.DataSource = Data;
        }

        private void CheckAccess(CurrentUser user)
        {
            if (user == null)
            {
                MessageBox.Show("You do not have access to run this application.  Please contact Systems Support.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        private void Metrics_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            MetricSummaryData item = Data[Metrics.CurrentRow.Index];
            using (ServicerMetrics sm = new ServicerMetrics(User.AllowedUserId, da, item))
            {
                sm.ShowDialog();
                LoadData();
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            using (ServicerMetrics sm = new ServicerMetrics(User.AllowedUserId, da))
            {
                sm.ShowDialog();
                LoadData();
            }
        }
    }
}

using System;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using SIRPTFED.Models;


namespace SIRPTFED.Views
{
    public partial class MetricAssign : Form
    {
        private List<ActiveDirectoryUser> ADUserList { get; set; }
        private List<MasterData> MasterList { get; set; }
        private string TheGoal { get; set; }

        public MetricAssign()
        {
            InitializeComponent();
            ADUserList = new List<ActiveDirectoryUser>();
            MasterList = new List<MasterData>();
            ADUserList = new Controllers.AssembleGroups().Assembly();
            ADUsers.DataSource = ADUserList.Select(o => o.AccountName).ToList();
            MasterList = Program.DA.PopulateMaster();
            Metric.DataSource = MasterList.OrderByDescending(o => o.Category).Select(o => o.Category + " - " + o.Metric).ToList();
            
            FillList();
        }

        private void assignButton_Click(object sender, EventArgs e)
        {
            RoleInfo rInfo = Program.DA.GetGroup(ADUsers.SelectedItem.ToString());
            if (rInfo == null)
            {
                AssignRole(ADUsers.SelectedItem.ToString());
                rInfo = Program.DA.GetGroup(ADUsers.SelectedItem.ToString());
            }


            string[] CatMet = Metric.SelectedItem.ToString().Split('-');
            TheGoal = Program.DA.GetGoal(CatMet[0], CatMet[1]);
            var sqlParams = new SqlParameter[]
               {
                    SqlParams.Single("ADUser", ADUsers.SelectedItem.ToString()),
                    SqlParams.Single("ADRole", rInfo.ADRole),
                    SqlParams.Single("ServicerMetric",   CatMet[1]), //SvcData.Where(p => p.Metrics == Metric.Text).First()),
                    SqlParams.Single("ServicerCategory", CatMet[0]), // SvcData.Where(p => p.Metrics == Category.Text).First()),
                    SqlParams.Single("MetricGoal", TheGoal),
                    SqlParams.Single("MetricMonth", DateTime.Now.Month.ToString()),
                    SqlParams.Single("MetricYear", DateTime.Now.Year.ToString()),
                    SqlParams.Single("ComplaintRecords", 0),
                    SqlParams.Single("TotalRecords", 0),
                    SqlParams.Single("AvgBacklog", 0),
                    SqlParams.Single("ADRoleId", rInfo.ADRoleId)
               };

            Program.DA.InsertData(sqlParams);
            DialogResult = DialogResult.OK;

        }

        private bool AssignRole(string name)
        {
            using (RoleAssign ra = new RoleAssign(name))
            {
                ra.ShowDialog();
            }
            return true;
        }

        private void FillList()
        {
            

            foreach (var all in ADUserList)
            {
                AllUsers auser = new AllUsers();

                int index = all.DisplayName.LastIndexOf(' ');
                auser.FirstName = all.DisplayName.SafeSubString(0, index);
                auser.LastName = all.DisplayName.SafeSubString(index, all.DisplayName.Length - index);
                auser.WindowsUserName = all.AccountName;
                //ADUserList.Add(auser);
            }
        }

        private void Metric_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void Metric_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }
    }
}

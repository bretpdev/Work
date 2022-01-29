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

namespace BatchLoginDatabase
{
    public partial class UserNameSelection : Form
    {
        List<BatchPasswordData> UserInfo { get; set; }
        public UserNameSelection()
        {
            InitializeComponent();
            GetUserIds();
        }

        [UsesSproc(DataAccessHelper.Database.BatchProcessing, "GetAllUsers")]
        private void GetUserIds()
        {
            UserIds.DataSource = UserInfo = DataAccessHelper.ExecuteList<BatchPasswordData>("GetAllUsers", DataAccessHelper.Database.BatchProcessing);
        }

        private void Add_Click(object sender, EventArgs e)
        {
            using (AddChangeRecord frm = new AddChangeRecord())
            {
                this.Hide();
                frm.ShowDialog();
                this.Show();
                GetUserIds();
            }
        }

        private void UserIds_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            using (AddChangeRecord frm = new AddChangeRecord(UserInfo[UserIds.CurrentRow.Index]))
            {
                this.Hide();
                frm.ShowDialog();
                this.Show();
                GetUserIds();
            }
        }

        private void ChangeScriptInfo_Click(object sender, EventArgs e)
        {
            using (SackerScriptInfo script = new SackerScriptInfo())
            {
                script.ShowDialog();
            }
        }

        private void Login_Click(object sender, EventArgs e)
        {
            using (ChangeLoginTypes lTypes = new ChangeLoginTypes())
            {
                lTypes.ShowDialog();
            }
        }
    }
}

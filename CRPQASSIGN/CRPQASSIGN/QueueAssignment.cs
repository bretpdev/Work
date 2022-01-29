using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.CheckedListBox;

namespace CRPQASSIGN
{
    public partial class QueueAssignment : Form
    {
        public bool UnassignTask { get; set; }
        private List<Queues> AllQueues { get; set; }
        private List<Users> AllUsers { get; set; }
        public Queues SelectedQueue { get; set; }
        public Queue<string> SelectedUsers { get; set; }
        public QueueAssignment(DataAccess da)
        {
            InitializeComponent();
            AllQueues = da.GetAllQueues();
            AllUsers = da.GetAllUsers();
            Qs.DataSource = AllQueues.Select(p => string.Format("{0}:{1}:{2}", p.Queue, p.SubQueue, p.Arc)).ToList();
            Us.Items.AddRange(AllUsers.Select(p => string.Format("{0}:{1}", p.UserName, p.AgentName)).ToArray());
            Qs.SelectedIndex = -1;
            SelectedUsers = new Queue<string>();
            for (int index = 0; index < Us.Items.Count; index++)
                Us.SetItemChecked(index, true);
        }

        private void Assign_Click(object sender, EventArgs e)
        {
            if(!ValidateInput())
                return;

            DialogResult = DialogResult.OK;
        }

        private bool ValidateInput()
        {
            if (Qs.SelectedIndex == -1)
                Qs.BackColor = Color.LightPink;
            else
                Qs.BackColor = SystemColors.Window;

            if (Us.CheckedItems.Count == 0)
                Us.BackColor = Color.LightPink;
            else
                Us.BackColor = SystemColors.Window;

            if (Qs.BackColor == Color.LightPink || Us.BackColor == Color.LightPink)
                return false;

            SelectedQueue = AllQueues[Qs.SelectedIndex];
            foreach (var user in Us.CheckedItems)
                SelectedUsers.Enqueue(user.ToString().Substring(0,user.ToString().IndexOf(":")));

            return true;
        }

        private void Unassign_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            UnassignTask = true;
            DialogResult = DialogResult.OK;
        }

        private void Sel_CheckedChanged(object sender, EventArgs e)
        {
            if(Sel.Checked)
            {
                for (int index = 0; index < Us.Items.Count; index++)
                    Us.SetItemChecked(index, false);
            }
            else
            {
                for (int index = 0; index < Us.Items.Count; index++)
                    Us.SetItemChecked(index, true);
            }
        }
    }
}

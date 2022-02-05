using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;

namespace CommentAuditTracker
{
    public partial class ManageUtIdsForm : Form
    {
        public Agent Agent { get; private set; }
        public ManageUtIdsForm(Agent agent)
        {
            InitializeComponent();
            this.Agent = agent;
            LoadGrid();
            SetAgentName(this.Agent.FullName);
        }

        public void SetAgentName(string name)
        {
            AgentNameBox.Text = name;
        }

        /// <summary>
        /// Load the grid with the UT IDs from the agent.  This should only be performed once to avoid overwriting changes.
        /// </summary>
        public void LoadGrid()
        {
            IdsGrid.Rows.Clear();
            foreach (string id in this.Agent.UtIds)
            {
                IdsGrid.Rows.Add("UT" + id);
            }
            this.Text = "UT IDs - " + Agent.FullName;
        }

        /// <summary>
        /// Compare the current IDs with the agent IDs to determine if any changes were made.
        /// </summary>
        public bool HasChanges
        {
            get
            {
                var ids = GetFormattedIds();
                if (ids.Count != Agent.UtIds.Count)
                    return true;
                if (ids.Union(Agent.UtIds).Distinct().Count() != ids.Count)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Strips the leading UT from all IDs and returns them in a list.
        /// </summary>
        public List<string> GetFormattedIds()
        {
            List<string> ids = new List<string>();
            foreach (DataGridViewRow row in IdsGrid.Rows)
            {
                string value = (string)row.Cells[0].Value;
                if (value == null)
                    continue;
                ids.Add(value.Replace("UT", ""));
            }
            return ids;
        }

        /// <summary>
        /// After edit, attempt to convert to a properly formatted UT id.
        /// </summary>
        private void IdsGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var cell = IdsGrid[e.ColumnIndex, e.RowIndex];
            string value = (cell.Value ?? "").ToString();
            value = new string(value.Where(o => char.IsDigit(o)).ToArray());
            if (value.ToIntNullable().HasValue)
            {
                if (value.Length < 5)
                    value = value.PadLeft(5, '0');
                else if (value.Length > 5)
                    value = value.Substring(0, 5);
                var ids = GetFormattedIds();
                ids.RemoveAt(e.RowIndex);
                if (ids.Contains(value)) //duplicated
                    cell.Value = null;
                else
                    cell.Value = "UT" + value;
            }
            else
                cell.Value = null;

            if (cell.Value == null)
            {
                this.BeginInvoke(new Action(() =>
                {
                    //can't delete during this event, so delete in the next message loop.
                    IdsGrid.Rows.RemoveAt(e.RowIndex);
                }));
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            LoadGrid();
        }
    }
}

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

namespace LOCATE
{
    public partial class LocateInfo : Form
    {
        private List<LocateTypes> Types { get; set; }
        public LocateTypes SelectedType { get; set; }
        public LocateInfo()
        {
            InitializeComponent();
            Types = LocateTypes.GetLocateTypes();
            Tasks.DataSource = Types.Select(p => p.ToString()).ToList();
            Tasks.SelectedIndex = -1;
        }

        private void Tasks_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Description.Text = Types.Where(p => p.LocateType == Tasks.SelectedItem.ToString().Substring(0, 3).Trim()).Select(q => q.LongDescription).SingleOrDefault();
        }

        private void Continue_Click(object sender, EventArgs e)
        {
            if (Description.Text.IsNullOrEmpty())
            {
                Dialog.Info.Ok("You must select a Task Type or Task Worked.");
                return;
            }

            SelectedType = Types.Where(p => p.LocateType == Tasks.Text.Substring(0, 3).Trim()).SingleOrDefault();
            DialogResult = DialogResult.OK;
        }
    }
}

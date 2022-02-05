using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace TPERM
{
    public partial class CoMakerInfo : Form
    {
        public CoMakerData SelectedCoMaker { get; set; }
        private List<CoMakerData> CoMakers { get; set; }
        public CoMakerInfo(List<CoMakerData> coMakers)
        {
            InitializeComponent();
            CoMakers = coMakers;
            coData.DataSource = coMakers;
            coData.Columns[0].HeaderText = "Account Number";
            coData.Columns[1].HeaderText = "SSN";
            coData.Columns[2].HeaderText = "First Name";
            coData.Columns[3].HeaderText = "Last Name";
            coData.Columns[4].HeaderText = "Comaker Signed Form";
        }

        private void Cont_Click(object sender, EventArgs e)
        {
            int selectedCount = 0;
            int selectedIndex = -1;
            foreach (DataGridViewRow row in coData.Rows)
            {
                if ((bool)row.Cells[4].Value)
                {
                    selectedCount++;
                    selectedIndex = row.Index;
                }
            }

            if (selectedCount > 1)
            {
                Dialog.Error.Ok("You can only select 1 comaker that signed the form.");
                return;
            }
            if (selectedIndex != -1)
                SelectedCoMaker = CoMakers[selectedIndex];
            DialogResult = DialogResult.OK;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BARCODEFED
{
    public partial class SelectPersonType : Form
    {
        public string SelectedPersonType { get; set; }
        public SelectPersonType(List<string> personTypes)
        {
            InitializeComponent();
            List<string> displayPersonTypes = GetDisplayPersonTypes(personTypes);
            comboBox1.Items.AddRange(displayPersonTypes.ToArray());
        }

        public List<string> GetDisplayPersonTypes(List<string> personTypes)
        {
            List<string> displayPersonTypes = new List<string>();
            if(personTypes.Contains("B"))
            {
                displayPersonTypes.Add("Borrower");
            }
            if(personTypes.Contains("E"))
            {
                displayPersonTypes.Add("Endorser");
            }
            if (personTypes.Contains("R"))
            {
                displayPersonTypes.Add("Reference");
            }
            return displayPersonTypes;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedItem != null)
            {
                SelectedPersonType = (comboBox1.SelectedItem.ToString()[0]).ToString();
            }
            else
            {
                MessageBox.Show("Please select a person type from the drop down", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}

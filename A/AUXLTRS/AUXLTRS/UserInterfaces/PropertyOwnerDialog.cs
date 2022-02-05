using System.Windows.Forms;

namespace AUXLTRS
{
    public partial class PropertyOwnerDialog : Form
    {
        public PropertyOwnerDialog(PropertyOwner propertyOwner, DataAccess da)
        {
            InitializeComponent();

            this.propertyOwnerBindingSource.DataSource = propertyOwner;
            cmbState.DataSource = da.GetStateAbbreviations();
            propertyOwner.State = "UT"; //Default the state to Utah.
        }

        private void button1_Click(object sender, System.EventArgs e)
        {

        }

        private void button1_Click_1(object sender, System.EventArgs e)
        {
            if ((textBox1.Text.Length == 9 || textBox1.Text.Length == 10) && textBox2.Text != "" && textBox3.Text != "" && textBox5.Text != "" && textBox6.Text != "")
                this.DialogResult = DialogResult.OK;
        }
    }
}
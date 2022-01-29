using System.Windows.Forms;


namespace ACHSETUPFD
{
    partial class ChangeInfoDialog : Form
    {
        ChangeData Data { get; set; }

        public ChangeInfoDialog(ChangeData changeData)
        {
            InitializeComponent();
            Data = changeData;
            changeDataBindingSource.DataSource = changeData;
        }

        private void txtAccountType_KeyDown(object sender, KeyEventArgs e)
        {
            //Ignore ReflectionInterface.Key presses that aren't the C or S ReflectionInterface.Key.
            if (e.KeyCode != Keys.C && e.KeyCode != Keys.S) { e.Handled = true; }
        }

        private void OK_Click(object sender, System.EventArgs e)
        {
            Data.EFT = ChangeData.EFTSource.Paper;
            this.DialogResult = DialogResult.OK;
        }
    }
}

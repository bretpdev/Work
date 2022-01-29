using System.Windows.Forms;

namespace NHGeneral
{
    public partial class HandleSsn : Form
    {
        public bool? ShouldMask { get; set; }
        public string UpdatedText { get; set; }
        public bool CloseForm { get; set; }

        public HandleSsn()
        {
            InitializeComponent();
            ShouldMask = null;
        }

        private void MaskSsn_Click(object sender, System.EventArgs e)
        {
            ShouldMask = true;
            CloseForm = true;
            this.Hide();
        }

        private void NotSsn_Click(object sender, System.EventArgs e)
        {
            CloseForm = true;
            ShouldMask = false;
            this.Hide();
        }

        private void HandleSsn_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CloseForm)
            {
                ShouldMask = false;
                this.Hide();
            }
        }
    }
}
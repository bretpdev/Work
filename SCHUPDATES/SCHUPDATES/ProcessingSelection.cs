using System;
using System.Windows.Forms;

namespace SCHUPDATES
{
    public partial class ProcessingSelection : Form
    {
        public bool FileProcessing { get; set; }
        public ProcessingSelection()
        {
            InitializeComponent();
        }

        private void File_Click(object sender, EventArgs e)
        {
            FileProcessing = true;
        }
    }
}
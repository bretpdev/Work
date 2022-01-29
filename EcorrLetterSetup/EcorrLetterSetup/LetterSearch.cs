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
using Uheaa.Common;

namespace EcorrLetterSetup
{
    public partial class LetterSearch : Form
    {
        private List<LetterSearchData> LetterData { get; set; }
        public LetterSearch()
        {
            InitializeComponent();
        }

        private void Search_Click(object sender, EventArgs e)
        {
            Letters.DataSource = null;
            Letters.DataSource = LetterData = DataAccessHelper.ExecuteList<LetterSearchData>("LTDB_SearchLetter", DataAccessHelper.Database.Bsys, LetterId.Text.ToSqlParameter("LetterId"));
        }

        private void Letters_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            LetterSearchData selectedLetter = LetterData[Letters.CurrentRow.Index];
            if (selectedLetter.DocTyp != "Script")
                ProcessSystemLetter(selectedLetter);
            else
                ProcessScriptLetter(selectedLetter);
        }

        /// <summary>
        /// Creates a ScriptLetter Object and launches the Script Letter Form.
        /// </summary>
        /// <param name="selectedLetter">Letter That was selected.</param>
        private void ProcessScriptLetter(LetterSearchData selectedLetter)
        {
            ScriptLetter ltr = new ScriptLetter(selectedLetter);
            using (LetterSetup scriptLtr = new LetterSetup(ltr))
            {
                this.Hide();
                scriptLtr.ShowDialog();
                this.Show();
            }
        }

        /// <summary>
        /// Creates a SystemLetter Object and launches the System Letter Form.
        /// </summary>
        /// <param name="selectedLetter">Letter That was selected.</param>
        private void ProcessSystemLetter(LetterSearchData selectedLetter)
        {
            SystemLetter ltr = new SystemLetter(selectedLetter);
            using (LetterSetup scriptLtr = new LetterSetup(ltr))
            {
                this.Hide();
                scriptLtr.ShowDialog();
                this.Show();
            }
        }
    }
}

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
    public partial class AddChangeSystemLetter : Form
    {
        public SystemSprocs Sproc { get; set; }
        private List<SystemLetterReturnTypes> ReturnTypes { get; set; }

        public AddChangeSystemLetter(SystemSprocs sproc = null)
        {
            InitializeComponent();

            //Check to see if this is a change or Add.
            if (sproc == null)
                Sproc = new SystemSprocs();//Add
            else
            {
                Sproc = sproc; //Change
                Active.SelectedValue = Sproc.Active;
            }

            ReturnTypes = DataAccessHelper.ExecuteList<SystemLetterReturnTypes>("GetAllSystemLetterReturnTypes", DataAccessHelper.Database.Bsys);
            ReturnTypeSelection.DataSource = ReturnTypes.Select( p => p.ReturnType).ToList();
            
            SprocName.Text = Sproc.StoredProcedureName;
            ReturnTypeSelection.Text = Sproc.ReturnType;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            Sproc.ReturnType = ReturnTypeSelection.Text;
            Sproc.StoredProcedureName = SprocName.Text;
            Sproc.Active = Active.SelectedValue;
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Checks for Errors on the form.
        /// </summary>
        /// <returns></returns>
        private bool CheckErrors()
        {
            List<string> errors = new List<string>();
            if (SprocName.Text.IsNullOrEmpty())
                errors.Add("- You must enter a Stored Procedure Name.");
            if(ReturnTypeSelection.Text.IsNullOrEmpty())
                errors.Add("- You must enter a Return Type.");

            if (errors.Any())
            {
                string message = string.Format("Please review the following errors: \n\n{0}", string.Join("\n", errors.Select(p => p)));
                MessageBox.Show(message);
                return true;
            }

            return false;
        }
    }
}

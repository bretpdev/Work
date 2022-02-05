using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WinForms;

namespace BatchLettersUI
{
    public partial class BatchLettersUI : Form
    {
        private List<DatabaseData> DatabaseDataList { get; set; }

        /// <summary>
        /// Default Constructor will initialize the form and set the data grid view's data source
        /// </summary>
        public BatchLettersUI()
        {
            InitializeComponent();
            Letters.DataSource = this.DatabaseDataList = DataAccess.SetDataSource();
            //We do not want to show the Identity Id to the user.
            Letters.Columns[0].Visible = false;
        }

        private void Letters_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Letters.DataSource != null)
            {
                DatabaseData dbData = CreateObjectFromDataRow();
                FormBuilder form = new FormBuilder("Update Letter");
                form.InputWidth = 400;

                form.FormAccepted += fb =>
                {
                    dbData.UpdatedBy = Environment.UserName;
                    return true;
                };

                if (FormBuilder.Generate(dbData, form).ShowPopoverDialog(this) == true)
                {
                    SqlParameter[]  test = SqlParams.Update(dbData);
                    DataAccess.UpdateBatchLetters(test);
                    Letters.DataSource = null;
                    Letters.DataSource = this.DatabaseDataList = DataAccess.SetDataSource();
                }
            }
        }

        /// <summary>
        /// Takes all the data in the selected datarow and creates a DatabaseData object
        /// </summary>
        /// <returns>A DatabaseData object with all of the properties filled out from the data in the datarow</returns>
        private DatabaseData CreateObjectFromDataRow()
        {
            return this.DatabaseDataList[Letters.CurrentRow.Index];
        }

        private void AddRecord_Click(object sender, EventArgs e)
        {
            DatabaseData dbData = new DatabaseData();
            FormBuilder form = new FormBuilder("Add Record");
            form.InputWidth = 400;
            if (FormBuilder.Generate(dbData, form).ShowPopoverDialog(this) == true)
            {
                SqlParameter[] sqlParms = new { LetterId = dbData.LetterId, SasFilePattern = dbData.SasFilePattern, StateFieldCodeName = dbData.StateFieldCodeName,AccountNUmberFieldIndex = dbData.AccountNumberFieldIndex, AccountNumberFieldName = dbData.AccountNumberFieldName, CostCenterFieldCodeName = dbData.CostCenterFieldCodeName, OkIfMissing = dbData.OkIfMissing, ProcessAllFiles = dbData.ProcessAllFiles, Arc = dbData.Arc, Comment = dbData.Comment, CreatedBy = Environment.UserName }.SqlParameters();
                DataAccess.AddBatchLetter(sqlParms);
                //Reset the data source with the new inserted data
                Letters.DataSource = this.DatabaseDataList = DataAccess.SetDataSource();
            }
        }
    }
}

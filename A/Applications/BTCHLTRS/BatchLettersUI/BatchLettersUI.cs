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
using Uheaa.Common.DataAccess;

namespace BatchLettersUI
{
    public partial class BatchLettersUI : Form
    {
        private DataTable DbData = new DataTable();

        public BatchLettersUI()
        {
            InitializeComponent();
            SetData();
        }

        private void SetData()
        {
            Letters.DataSource = DbData = DataAccessHelper.ExecuteDataTable("GetAllBatchLettersRecords", DataAccessHelper.Database.Uls);
            Letters.Columns["BatchLettersId"].ReadOnly = true;
            Letters.Columns["CreatedAt"].ReadOnly = true;
            Letters.Columns["CreatedBy"].ReadOnly = true;
            Letters.Columns["UpdatedAt"].ReadOnly = true;
            Letters.Columns["UpdatedBy"].ReadOnly = true;
            ((DataGridViewTextBoxColumn)Letters.Columns["LetterId"]).MaxInputLength = 10;
            ((DataGridViewTextBoxColumn)Letters.Columns["SasFilePattern"]).MaxInputLength = 50;
            ((DataGridViewTextBoxColumn)Letters.Columns["StateFieldCodeName"]).MaxInputLength = 25;
            ((DataGridViewTextBoxColumn)Letters.Columns["AccountNumberFieldName"]).MaxInputLength = 25;
            ((DataGridViewTextBoxColumn)Letters.Columns["CostCenterFieldCodeName"]).MaxInputLength = 25;
            ((DataGridViewTextBoxColumn)Letters.Columns["Arc"]).MaxInputLength = 5;
            ((DataGridViewTextBoxColumn)Letters.Columns["Comment"]).MaxInputLength = 1200;

        }

        private void Save_Click(object sender, EventArgs e)
        {
            DbData = (DataTable)Letters.DataSource;
            int id = Convert.ToInt32(DbData.Compute("MAX(BatchLettersId)", string.Empty));
            foreach (DataRow row in DbData.Rows)
            {
                if (row.RowState == DataRowState.Modified)
                {
                    row["UpdatedAt"] = DateTime.Now.ToString();
                    row["UpdatedBy"] = Environment.UserName;
                }
                else if (row.RowState == DataRowState.Added)
                {
                    row["BatchLettersId"] = ++id;
                    row["CreatedAt"] = DateTime.Now.ToString();
                    row["CreatedBy"] = Environment.UserName;
                }

                List<string> errors = ValidateRow(row);
                if (errors.Any())
                {
                    Dialog.Error.Ok("Please review the following errors: \n" + string.Join("\n", errors.Select(p => " - " + p).ToArray()));
                    return;
                }
            }

            DataAccessHelper.Execute("BatchLettersUICommitChanges", DataAccessHelper.Database.Uls, SqlParams.Single("Data", DbData));
            SetData();

        }

        private List<string> ValidateRow(DataRow row)
        {
            List<string> errors = new List<string>();
            if (row["LetterId"].ToString().IsNullOrEmpty())
                errors.Add("One or more records are missing the LetterId");
            if (row["SasFilePattern"].ToString().IsNullOrEmpty())
                errors.Add("One or more records are missing the SasFilePattern");
            if (row["StateFieldCodeName"].ToString().IsNullOrEmpty())
                errors.Add("One or more records are missing the StateFieldCodeName");
            if (row["CostCenterFieldCodeName"].ToString().IsNullOrEmpty())
                errors.Add("One or more records are missing the CostCenterFieldCodeName");

            return errors;
        }
    }
}

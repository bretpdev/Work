using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;

namespace EcorrLetterSetup
{
    public partial class LetterSetup : Form
    {
        private Letter LetterInfo { get; set; }
        private Forms Form { get; set; }
        private bool HasExistingForm { get; set; }

        public LetterSetup(Letter letterInfo)
        {
            InitializeComponent(); ;
            LetterInfo = letterInfo;
            LetterIdLabel.Text += LetterInfo.LetterData.LetterId;
            SetDataGrid();
            GetFormInfo();
        }

        /// <summary>
        /// Checks to see if the current letter has a form.
        /// </summary>
        private void GetFormInfo()
        {
            try
            {
                Form = DataAccessHelper.ExecuteSingle<Forms>("GetFormAndPathForLetterId", DataAccessHelper.Database.Csys, LetterInfo.LetterData.LetterId.ToSqlParameter("LetterId"));
            }
            catch (InvalidOperationException ex)//No result where returned
            {
                Form = new Forms();
            }

            if (!Form.LetterId.IsNullOrEmpty())
            {
                FormPath.Text = Form.PathAndForm;
                HasExistingForm = true;
            }
            else
            {
                FormPath.Text = string.Empty;
                HasExistingForm = false;
            }
        }

        /// <summary>
        /// Sets the data grid view.
        /// </summary>
        private void SetDataGrid()
        {
            LetterDataGrid.DataSource = null;
            LetterDataGrid.DataSource = LetterInfo.GetData();
        }

        private void SelectForm_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "|*.pdf";
            if (file.ShowDialog() == DialogResult.OK)
                FormPath.Text = Form.PathAndForm = file.FileName;
        }

        private void PreviewForm_Click(object sender, EventArgs e)
        {
            if (!FormPath.Text.IsNullOrEmpty())
                Process.Start(FormPath.Text);
        }

        private void Add_Click(object sender, EventArgs e)
        {
            this.Hide();
            LetterInfo.AddRecord(LetterInfo.LetterData.LetterId);
            SetDataGrid();
            this.Show();
        }

        private void LetterDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dynamic selectedData = LetterInfo.GetData()[LetterDataGrid.CurrentRow.Index];
            LetterInfo.ChangeRecord(LetterInfo.LetterData.LetterId, selectedData);
            SetDataGrid();
        }

        private void SaveForm_Click(object sender, EventArgs e)
        {
			if (FormPath.Text.IsNullOrEmpty())
			{
				DataAccessHelper.Execute("DeleteFormFromLetter", DataAccessHelper.Database.Csys, LetterInfo.LetterData.LetterId.ToSqlParameter("LetterId"));
				GetFormInfo();
			}
			else
			{
				string path = Path.GetDirectoryName(FormPath.Text) + @"\";
				string form = Path.GetFileName(FormPath.Text);
				DataAccessHelper.Execute("InsertFormInformation", DataAccessHelper.Database.Csys, LetterInfo.LetterData.LetterId.ToSqlParameter("LetterId"), path.ToSqlParameter("Path"), form.ToSqlParameter("Form"));
			}
        }

        private void DeleteForm_Click(object sender, EventArgs e)
        {
            if (Dialog.Warning.YesNo("Are you sure you want to delete this form?"))
            {
                DataAccessHelper.Execute("DeleteFormFromLetter", DataAccessHelper.Database.Csys, LetterInfo.LetterData.LetterId.ToSqlParameter("LetterId"));
                GetFormInfo();
            }
        }

        private SqlConnection GetConnection(bool testMode)
        {
            SqlConnection conn = null;
            if (testMode) //Test update NOCHOUSETest
                conn = new SqlConnection(DataAccessHelper.GetConnectionString(DataAccessHelper.Database.Bsys, DataAccessHelper.Mode.Test));
            else //Live update NOCHOUSE
                conn = new SqlConnection(DataAccessHelper.GetConnectionString(DataAccessHelper.Database.Bsys, DataAccessHelper.Mode.Live));

            return conn;
        }

        private void PromoteScriptDbData(string letter, List<ScriptLetterData> data, bool testMode)
        {
            SqlConnection conn = GetConnection(testMode);
            conn.Open();
            DataAccessHelper.Execute("PromoteScriptLetter", conn, SqlParams.Single("Letter", letter), SqlParams.Single("Data", data.Select(p => new { p.Header, p.HeaderType, p.Order, p.Active }).ToList().ToDataTable()), SqlParams.Single("User", Environment.UserName));
            conn.Dispose();
        }

        private void PromoteSystemLetter(string letter, List<SystemSprocs> data, bool testMode)
        {
            SqlConnection conn = GetConnection(testMode);
            conn.Open();
            DataAccessHelper.Execute("PromoteSystemLetter", conn, SqlParams.Single("Letter", letter), SqlParams.Single("Data", data.Select(p => new { p.StoredProcedureName, p.ReturnType, }).ToList().ToDataTable()), SqlParams.Single("User", Environment.UserName));
            conn.Dispose();
        }

        private void Promote_Click(object sender, EventArgs e)
        {
            if (Dialog.Def.YesNo("Are you sure you want to promote this letter to Live?"))
            {
                using (ModeSelector mode = new ModeSelector())
                {
                    if (mode.ShowDialog() != DialogResult.OK)
                        return;

                    string promoteDir = Path.GetDirectoryName(DataAccessHelper.ExecuteSingle<string>("spLTDBGetPathAndName", DataAccessHelper.Database.Bsys, SqlParams.Single("LetterId", LetterInfo.LetterData.LetterId)));
                    string testPath = string.Empty;

                    if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone)
                        testPath = promoteDir.Replace("Z:", "Y:");
                    else
                    {
                        //TODO figure out UHEAA later
                    }

                    var selectedData = LetterInfo.GetDataForPromotion();

                    if (LetterInfo.LetterData.DocTyp == "Compass")
                    {
                        PromoteSystemLetter(LetterInfo.LetterData.LetterId, (List<SystemSprocs>)selectedData, mode.TestMode);
                    }
                    else if (LetterInfo.LetterData.DocTyp == "Script")
                        PromoteScriptDbData(LetterInfo.LetterData.LetterId, (List<ScriptLetterData>)selectedData, mode.TestMode);
                    else
                    {
                        Dialog.Error.Ok(string.Format("Unable to handle DocType {0}  The letter cannot be promoted using this application.  Please contact App Dev.", LetterInfo.LetterData.DocTyp));
                        return;
                    }

                    List<string> filesToPromote = Directory.GetFiles(testPath, LetterInfo.LetterData.LetterId + ".*").ToList();
                    filesToPromote.AddRange(Directory.GetFiles(testPath, LetterInfo.LetterData.LetterId + "Ecorr.*").ToList());
                    if (mode.TestMode)
                        promoteDir = @"T:\";

                    foreach (string file in filesToPromote)
                        File.Move(file, promoteDir);
                }
            }
        }
    }
}

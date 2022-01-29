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

namespace EcorrLetterSetup
{
    public partial class AddChangeScriptLetter : Form
    {
        private List<FileHeaders> HeadersList { get; set; }
        private List<HeaderTypes> HeaderTypesList { get; set; }
        private string LetterId { get; set; }
        private bool Add { get; set; }
        private dynamic SelectedData { get; set; }

        public AddChangeScriptLetter(string letterId, dynamic selectedData)
        {
            InitializeComponent();
            SelectedData = selectedData;
            Add = SelectedData == null;
            LetterIdLabel.Text += letterId;
            LetterId = letterId;
            PopulateLists();
            SetComboBoxes();
            Active.SelectedValue = Add ? Add : selectedData.Active;
        }

        /// <summary>
        /// Populates the lists that the combo boxes will use as there data source.
        /// </summary>
        private void PopulateLists()
        {
            HeaderTypesList = DataAccessHelper.ExecuteList<HeaderTypes>("LTDB_GetAllActiveHeaderTypes", DataAccessHelper.Database.Bsys);
            HeadersList = DataAccessHelper.ExecuteList<FileHeaders>("LTDB_GetAllActiveHeaders", DataAccessHelper.Database.Bsys);
        }

        /// <summary>
        /// Sets the combo data sources.
        /// </summary>
        private void SetComboBoxes()
        {
            Headers.DataSource = HeadersList.OrderBy(p => p.Header).Select(p => p.Header).ToList();
            HeaderTypes.DataSource = HeaderTypesList.OrderBy(p => p.HeaderType).Select(p => p.HeaderType).ToList();

            if (Add)
            {
                Headers.SelectedIndex = -1;
                HeaderTypes.SelectedIndex = -1;
            }
            else
            {
                Headers.Text = SelectedData.Header;
                HeaderTypes.Text = SelectedData.HeaderType;
                Order.Value = SelectedData.Order;
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (CheckErrors())
                return;

            FileHeaders header = GetHeaderValue();
            HeaderTypes headerTypes = GetHeaderTypeValue();

            if (Add)
            {
                DataAccessHelper.Execute("LTDB_InsertLetterMappingTable", DataAccessHelper.Database.Bsys, LetterId.ToSqlParameter("LetterId"), new SqlParameter("HeaderTypeId", headerTypes.HeaderTypeId),
                    new SqlParameter("HeaderId", header.HeaderId), new SqlParameter("Order", (int)Order.Value), Environment.UserName.ToSqlParameter("User"));
            }
            else
            {
                DataAccessHelper.Execute("LTDB_UpdateLetterMapping", DataAccessHelper.Database.Bsys, new SqlParameter("MappingId", SelectedData.LetterHeaderMappingId), 
                    new SqlParameter("HeaderTypeId", headerTypes.HeaderTypeId), new SqlParameter("HeaderId", header.HeaderId), new SqlParameter("Order", (int)Order.Value),
                        Environment.UserName.ToSqlParameter("User"), new SqlParameter("Active", Active.SelectedValue));
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Gets the selected header type values, if the header does not exist it will insert into the DB.
        /// </summary>
        /// <returns>HeaderType object</returns>
        private HeaderTypes GetHeaderTypeValue()
        {
            HeaderTypes headerTypes = new HeaderTypes();
            if (!HeaderTypesList.Any(p => p.HeaderType == HeaderTypes.Text))
            {
                headerTypes = DataAccessHelper.ExecuteSingle<HeaderTypes>("LTDB_InsertNewHeaderType", DataAccessHelper.Database.Bsys,
                     HeaderTypes.Text.ToUpper().ToSqlParameter("HeaderType"), Environment.UserName.ToSqlParameter("User"));

                HeaderTypesList.Add(headerTypes);
            }
            else
                headerTypes = HeaderTypesList.Where(p => p.HeaderType == HeaderTypes.Text).ToList().SingleOrDefault();
            return headerTypes;
        }

        /// <summary>
        /// Gets the selected header values, if the header does not exist it will insert into the DB.
        /// </summary>
        /// <returns>FileHeader Object</returns>
        private FileHeaders GetHeaderValue()
        {
            FileHeaders header = new FileHeaders();
            if (!HeadersList.Any(p => p.Header == Headers.Text))
            {
                header = DataAccessHelper.ExecuteSingle<FileHeaders>("LTDB_InsertNewHeader", DataAccessHelper.Database.Bsys,
                      Headers.Text.ToUpper().ToSqlParameter("Header"), Environment.UserName.ToSqlParameter("User"));

                HeadersList.Add(header);
            }
            else
                header = HeadersList.Where(p => p.Header == Headers.Text).ToList().SingleOrDefault();
            return header;
        }

        /// <summary>
        /// Checks for Errors on the form.
        /// </summary>
        /// <returns>True if errors are found False if the data is good.</returns>
        private bool CheckErrors()
        {
            List<string> errors = new List<string>();
            if (Headers.Text == null || Headers.Text.IsNullOrEmpty())
                errors.Add("- You must Select a Header from the List or Type in a new Header.");

            if (HeaderTypes.Text == null || HeaderTypes.Text.IsNullOrEmpty())
                errors.Add("- You must Select a Header Type from the List or Type in a new Header Type.");

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

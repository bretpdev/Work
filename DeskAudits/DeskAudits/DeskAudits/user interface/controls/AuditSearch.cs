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
using Uheaa.Common.ProcessLogger;

namespace DeskAudits
{
    public partial class AuditSearch : UserControl
    {
        DataAccess DA { get; set; }
        ProcessLogRun LogRun { get; set; }
        private string UserName { get; set; }
        private DataTable AuditData { get; set; }

        public AuditSearch()
        {
            InitializeComponent();
        }

        public void InitializeValues(DataAccess da, string userName, ProcessLogRun logRun)
        {
            DA = da;
            LogRun = logRun;
            UserName = userName;
            BeginDateField.Value = DateTime.Now;
            EndDateField.Value = DateTime.Now;
            BeginDateField.MaxDate = DateTime.Today;
            EndDateField.MaxDate = DateTime.Today;
            ExportButton.Enabled = false;
            SharedFieldsControl.InitializeValues(DA, true, userName);
        }

        private Audit GetSelections()
        {
            Audit audit = new Audit();
            audit.Auditee = SharedFieldsControl.GetAuditee();
            audit.Passed = SharedFieldsControl.GetAuditResult();
            audit.CommonFailReasonId = SharedFieldsControl.GetFailReason()?.FailReasonId;
            audit.Auditor = SharedFieldsControl.GetAuditor();

            return audit;
        }

        private void SearchAuditsButton_Click(object sender, EventArgs e)
        {
            Audit audit = GetSelections();
            DateTime? beginDate = BeginDateField.Value;
            DateTime? endDate = EndDateField.Value;
            if (IsValid(audit, beginDate, endDate))
            {
                List<Audit> audits = DA.GetAudits(audit, beginDate, endDate);
                DataTable dt = ConvertToDatatable<Audit>(audits);
                AuditsView.DataSource = dt;
                AuditData = dt;
                foreach (DataGridViewColumn column in AuditsView.Columns)
                    column.SortMode = DataGridViewColumnSortMode.Programmatic;

                if (audits?.Count() > 0)
                    ExportButton.Enabled = true;
                else
                    ExportButton.Enabled = false;
            }
        }

        private bool IsValid(Audit audit, DateTime? beginDate, DateTime? endDate)
        {
            if (beginDate.HasValue && endDate.HasValue && beginDate.Value > endDate.Value)
            {
                Dialog.Warning.Ok($"Audit not submitted. The {BeginDateLabel.Text} cannot come after the {EndDateLabel.Text}.");
                return false;
            }
            else if (audit.Auditor != null && !audit.Auditor.IsAlpha())
            {
                Dialog.Warning.Ok("Audit not submitted. The Auditor field must be composed of alphabetical characters only.");
                return false;
            }
            else if (audit.Auditor != null && !audit.Auditor.IsIn(DA.GetLoggedAuditors().ToArray()))
            {
                Dialog.Warning.Ok("Audit not submitted. The selected Auditor does not exist. Please use the auditor's WindowsUserName.");
                return false;
            }
            return true;
        }

        private void ClearFormButton_Click(object sender, EventArgs e)
        {
            ResetFields();
        }

        private void ResetFields()
        {
            ResetText();
            SharedFieldsControl.PopulateFields(true, UserName);
            BeginDateField.Value = DateTime.Today;
            EndDateField.Value = DateTime.Today;
            AuditsView.DataSource = null;
            ExportButton.Enabled = false;
            AuditData = null;
        }

        private void AuditsView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn selectedColumn = AuditsView.Columns[e.ColumnIndex];
            DataGridViewColumn previouslySortedColumn = AuditsView.SortedColumn;
            ListSortDirection sortDirection;

            if (previouslySortedColumn != null)
            {
                if (previouslySortedColumn == selectedColumn && previouslySortedColumn.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    sortDirection = ListSortDirection.Descending;
                    selectedColumn.HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
                else
                {
                    sortDirection = ListSortDirection.Ascending;
                    previouslySortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                    selectedColumn.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
            }
            else
            {
                sortDirection = ListSortDirection.Ascending;
                selectedColumn.HeaderCell.SortGlyphDirection = SortOrder.Ascending;
            }

            AuditsView.Sort(selectedColumn, sortDirection);
        }

        /// <summary>
        /// Used to convert a list to a datatable, which allows for automatic sorting
        /// of the DataGridView.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private static DataTable ConvertToDatatable<T>(List<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            List<int> propertyIndexes = new List<int>();

            for (int i = 0; i < props.Count; i++) // Add field name and type to table
            {
                AttributeCollection attributes = props[i].Attributes;
                if (attributes[typeof(BrowsableAttribute)].Equals(BrowsableAttribute.Yes)) // We only want browsable properties
                {
                    PropertyDescriptor prop = props[i];
                    if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) // Since there are nullable types
                        table.Columns.Add(prop.DisplayName, prop.PropertyType.GetGenericArguments()[0]);
                    else
                        table.Columns.Add(prop.DisplayName, prop.PropertyType);
                    propertyIndexes.Add(i);
                }
            }

            object[] values = new object[propertyIndexes.Count];
            foreach (T item in data) // Add field value to table
            {
                int valuesIndex = 0;
                for (int i = 0; i < props.Count; i++)
                {
                    if (i.IsIn(propertyIndexes.ToArray()))
                        values[valuesIndex++] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        /// <summary>
        /// When user selects the Export Data button, they select what to name the file
        /// and where to save it. An Excel file with the current data is written out.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.RestoreDirectory = false;
            saveFile.FileName = $"Desk Audits - {UserName} - {DateTime.Today:MMMM - dd - yyyy}";
            saveFile.Filter = "Excel Files(.xlsx)|*.xlsx| Excel Files(*.xlsm)| *.xlsm | Excel Files(.xls)|*.xls";
            string filePath = "";

            if (saveFile.ShowDialog() == DialogResult.OK)
                filePath = saveFile.FileName;

            bool? result = new ExportHelper(LogRun).ExportData(filePath, AuditData);
        }
    }
}

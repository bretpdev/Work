using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;

namespace Uheaa.Common.WinForms
{
    public partial class BorrowerResultsControl : UserControl
    {
        public BorrowerResultsControl()
        {
            InitializeComponent();
        }
        private ApplicationSettingsBase Settings { get; set; }
        private PropertyInfo LayoutSetting { get; set; }
        public void RegisterSetting<T>(T settings, PropertyInfo layoutSetting) where T : ApplicationSettingsBase
        {
            Settings = settings;
            LayoutSetting = layoutSetting;
            var layout = (ColumnLayoutInfo)LayoutSetting.GetValue(Settings, null);
            if (layout == null || layout.NotSet)
                SaveResultsLayout();
            else
                layout.OrderGrid(SearchResultsGrid);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool PhoneColumnsVisible
        {
            get
            {
                return SearchResultsGrid.Columns["HomePhoneColumn"].Visible;
            }
            set
            {
                SearchResultsGrid.Columns["HomePhoneColumn"].Visible =
                    SearchResultsGrid.Columns["WorkPhoneColumn"].Visible =
                    SearchResultsGrid.Columns["AltPhoneColumn"].Visible = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool EmailsColumnVisible
        {
            get
            {
                return SearchResultsGrid.Columns["EmailsColumn"].Visible;
            }
            set
            {
                SearchResultsGrid.Columns["EmailsColumn"].Visible = value;
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ChooseBorrowerOnSelection { get; set; } = false;

        public delegate void SelectedBorrowerDelegate(object sender, QuickBorrower selected);
        public event SelectedBorrowerDelegate OnSelectionChanged;
        public event SelectedBorrowerDelegate OnBorrowerChosen;
        private List<QuickBorrower> results;
        public List<QuickBorrower> GetResults()
        {
            return results;
        }
        public void SetResults(List<QuickBorrower> results)
        {
            this.results = results;
            codeInitiated = true;
            SearchResultsGrid.AutoGenerateColumns = false;
            SearchResultsGrid.DataSource = results;
            SearchResultsGrid.ClearSelection();
            codeInitiated = false;
            SearchResultsBox.Text = "Search Results";
            if (results != null)
                SearchResultsBox.Text += " (" + results.Count + ")";
        }

        private QuickBorrower SelectedBorrower
        {
            get
            {
                if (SearchResultsGrid.SelectedRows.Count > 0)
                    return this.results[SearchResultsGrid.SelectedRows[0].Index];
                return null;
            }
        }

        bool codeInitiated = false;
        private void SearchResultsGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (!codeInitiated)
            {
                OnSelectionChanged?.Invoke(this, SelectedBorrower);
                if (ChooseBorrowerOnSelection)
                    OnBorrowerChosen?.Invoke(this, SelectedBorrower);
            }
        }

        private void SearchResultsGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (SelectedBorrower != null)
                OnBorrowerChosen?.Invoke(this, SelectedBorrower);
        }

        public void ClearSelection()
        {
            codeInitiated = true;
            SearchResultsGrid.ClearSelection();
            codeInitiated = false;
        }

        private void SearchResultsGrid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            SaveResultsLayout();
        }

        private void SearchResultsGrid_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            SaveResultsLayout();
        }

        private void SaveResultsLayout()
        {
            if (Settings != null)
            {
                LayoutSetting.SetValue(Settings, new ColumnLayoutInfo(SearchResultsGrid), null);
                Settings.Save();
            }
        }

        private void SearchResultsGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (results == null) return;
            var column = SearchResultsGrid.Columns[e.ColumnIndex];
            var header = column.HeaderCell;
            var direction = header.SortGlyphDirection;
            var prop = typeof(QuickBorrower).GetProperty(column.DataPropertyName);
            if (direction == SortOrder.None || direction == SortOrder.Ascending)
            {
                direction = SortOrder.Descending;
                results = results.OrderByDescending(o => prop.GetValue(o, null)).ToList();
            }
            else
            {
                direction = SortOrder.Ascending;
                results = results.OrderBy(o => prop.GetValue(o, null)).ToList();
            }
            SetResults(results);
            header.SortGlyphDirection = direction;
        }

        private void SearchResultsGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var column = SearchResultsGrid.Columns[e.ColumnIndex];
            if (column.DataPropertyName.Contains("Phone"))
            {
                var borrower = results[e.RowIndex];
                bool consent = false;
                if (column.DataPropertyName == "WorkPhone" && borrower.WorkPhoneConsent)
                    consent = true;
                else if (column.DataPropertyName == "HomePhone" && borrower.HomePhoneConsent)
                    consent = true;
                else if (column.DataPropertyName == "AlternatePhone" && borrower.AlternatePhoneConsent)
                    consent = true;
                Color c = consent ? Color.LightGreen : Color.LightPink;
                if (SearchResultsGrid[e.ColumnIndex, e.RowIndex].Value == null)
                    return;
                e.CellStyle.BackColor = c;
            }
        }

        private void SearchResultsGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (SelectedBorrower != null)
                    OnBorrowerChosen?.Invoke(this, SelectedBorrower);
            }
        }
    }

}

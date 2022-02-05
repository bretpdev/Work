using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;

namespace CCCLOSURES
{
    public partial class SelectionScreen : Form
    {
        private enum SortingOptions
        {
            StartAsc,
            StartDesc,
            EndAsc,
            EndDesc,
            None
        }

        private List<ScheduleSelctionData> Data { get; set; }
        public DataAccess DA { get; set; }
        public List<Regions> Regions { get; set; }

        public SelectionScreen(DataAccess da)
        {
            InitializeComponent();
            DA = da;
            SetAccess(DA.CheckUserAccess());
            SetDataGrid();
            Regions = DA.PopulateRegions();
        }

        /// <summary>
        /// Retrieves schedule data and determines what to display.
        /// </summary>
        /// <param name="opt"></param>
        private void SetDataGrid(SortingOptions opt = SortingOptions.None)
        {
            Data = DA.PopulateScheduleData();
            Data = Data.OrderBy(p => p.StartAt).ThenBy(p => p.RegionName).ToList();
            ScheduleSelection.DataSource = new SortableBindingList<ScheduleSelctionData>(Data);

            if (Data.Any())
            {
                ScheduleSelection.Columns[5].DefaultCellStyle.Format = "MM/dd/yyyy hh:mm:ss tt";
                ScheduleSelection.Columns[6].DefaultCellStyle.Format = "MM/dd/yyyy hh:mm:ss tt";
                foreach (PropertyInfo pi in Data[0].GetType().GetProperties())//Cycle through attributes to determine which ones to show on the data grid view.
                {
                    DataGridViewHidden hidden = pi.GetCustomAttribute<DataGridViewHidden>();
                    DataGridViewLabel labal = pi.GetCustomAttribute<DataGridViewLabel>();
                    if (hidden != null)
                        ScheduleSelection.Columns[hidden.Index].Visible = false;
                    if (labal != null)
                        ScheduleSelection.Columns[labal.Index].HeaderText = labal.Label;
                }

            }
        }

        /// <summary>
        /// Enables form buttons based on user access.
        /// </summary>
        /// <param name="hasChangeMode"></param>
        private void SetAccess(bool hasChangeMode)
        {
            if (!hasChangeMode)//Everyone can use the app but only some users can update
            {
                Add.Enabled = false;
                Update.Enabled = false;
                Delete.Enabled = false;
            }
        }

        /// <summary>
        /// Opens form to update selected schedule.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_Click(object sender, EventArgs e)
        {
            if (ScheduleSelection.RowCount > 0)
            {
                ScheduleSelctionData selectedItem = Data.Where(p => p.StatusScheduleId == (int)ScheduleSelection.CurrentRow.Cells[0].Value).Single();
                ShowUserForm(selectedItem);
            }
        }

        /// <summary>
        /// Provides user with form to update or add a schedule.
        /// </summary>
        /// <param name="data"></param>
        private void ShowUserForm(ScheduleSelctionData data = null)
        {
            using (UserInput input = new UserInput(DA, Regions, data))
            {
                this.Hide();
                if (input.ShowDialog() == DialogResult.OK)
                    SetDataGrid();
                this.Show();
            }
        }

        /// <summary>
        /// Opens form to add a new schedule.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, EventArgs e)
        {
            ShowUserForm(null);
        }

        /// <summary>
        /// Removes the selected schedule.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Click(object sender, EventArgs e)
        {
            if (ScheduleSelection.RowCount > 0)
            {
                if (Dialog.Warning.YesNo("Are you sure you want to delete this record?"))
                {
                    ScheduleSelctionData selectedItem = Data.Where(p => p.StatusScheduleId == (int)ScheduleSelection.CurrentRow.Cells[0].Value).Single();
                    DA.DeleteSchedule(selectedItem);
                    SetDataGrid();
                }
            }
        }

        /// <summary>
        /// Handles the user selecting a different schedule. 
        /// Determines whether the Update and Delete buttons
        /// should be enabled based on the selected row and
        /// their access.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScheduleSelection_SelectionChanged(object sender, EventArgs e)
        {
            if (ScheduleSelection.CurrentRow != null)
            {
                Update.Enabled = true;
                Delete.Enabled = true;
                bool hasAccess = ScheduleSelection.CurrentRow.Cells[2].Value.ToString().ToLower().IsIn(Regions.Select(p => p.RegionName.ToLower()).ToArray());
                if (!hasAccess)
                {
                    Update.Enabled = false;
                    Delete.Enabled = false;
                }
            }
        }
    }
}
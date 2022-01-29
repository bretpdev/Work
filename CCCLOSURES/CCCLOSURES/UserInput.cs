using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;

namespace CCCLOSURES
{
    public partial class UserInput : Form
    {
        List<Regions> AllRegions { get; set; }
        List<StatusCodes> AllStatusCodes { get; set; }
        ScheduleSelctionData Data { get; set; }
        public DataAccess DA { get; set; }

        public UserInput(DataAccess da, List<Regions> regions, ScheduleSelctionData data = null)
        {
            InitializeComponent();
            DA = da;
            AllRegions = regions;
            SetDataSources();
            Data = data;
            
            if (Data != null)//This is an update load the form with the data
            {
                Region.SelectedIndex = AllRegions.IndexOf(AllRegions.Where(p => p.RegionsId == Data.RegionId).SingleOrDefault());
                Status.SelectedIndex = AllStatusCodes.IndexOf(AllStatusCodes.Where(p => p.StatusCodeId == Data.StatusCodeId).SingleOrDefault());
                StartDate.MinDate = Data.StartAt;
                StartDate.Value = Data.StartAt;
                StartTime.Value = Data.StartAt;
                EndDate.Value = Data.EndAt;
                EndTime.Value = Data.EndAt;
            }
            else
                StartDate.MinDate = DateTime.Now;
        }

        private void SetDataSources()
        {
            AllStatusCodes = DA.PopulateStatusCodes();
            Status.DataSource = AllStatusCodes.Select(p => p.StatusCodeName).ToList();
            Region.DataSource = AllRegions.Select(p => p.RegionName).ToList();
            Status.SelectedIndex = -1;
            Region.SelectedIndex = -1;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            DateTime newStart = new DateTime(StartDate.Value.Year, StartDate.Value.Month, StartDate.Value.Day, StartTime.Value.Hour, StartTime.Value.Minute, StartTime.Value.Second);
            DateTime newEnd = new DateTime(EndDate.Value.Year, EndDate.Value.Month, EndDate.Value.Day, EndTime.Value.Hour, EndTime.Value.Minute, EndTime.Value.Second);
            if (!ValidateInput(newStart, newEnd))
                return;

            AddUpdateRecord(newStart, newEnd);
            DialogResult = DialogResult.OK;
        }

        private void AddUpdateRecord(DateTime start, DateTime end)
        {
            int statucCodeId = AllStatusCodes[Status.SelectedIndex].StatusCodeId;
            int regionId = AllRegions[Region.SelectedIndex].RegionsId;
            if (Data == null)//Add
                DA.AddSchedule(start, end, statucCodeId, regionId);
            else//Update
                DA.UpdateSchedule(start, end, statucCodeId, regionId, Data);
        }

        private bool ValidateInput(DateTime newStart, DateTime newEnd)
        {

            List<string> errors = new List<string>();

            if (Region.SelectedIndex == -1)
                errors.Add("You must select a Region.");
            if (Status.SelectedIndex == -1)
                errors.Add("You must select a Status.");

            if (newStart == newEnd)
                errors.Add("The End Date and Time must be different than the Start Date and Time.");
            else if (newStart > newEnd)
                errors.Add("The End Date and Time cannot be earlier than the Start Date and Time.");

            if (Region.SelectedIndex != -1)
            {
                int region = AllRegions.Where(p => p.RegionName == Region.Text).SingleOrDefault().RegionsId;

                if (DA.CheckOverlappintDates(newStart, newEnd, region, Data))
                    errors.Add("Overlap Exists with another exemption row.  Unable to Add/Update row.");
            }

            if (errors.Any())
            {
                Dialog.Error.Ok(string.Format("Please review the following errors: \r\n\r\n{0}", string.Join(Environment.NewLine, errors)));
                return false;
            }

            return true;
        }

        private void StartDate_MouseDown(object sender, MouseEventArgs e)
        {
            StartDate.MinDate = DateTime.Now;
        }

        private void StartDate_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            StartDate.MinDate = DateTime.Now;
        }

        private void StartDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            StartDate.MinDate = DateTime.Now;
        }
    }
}
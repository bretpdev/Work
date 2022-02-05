using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using System.Globalization;

namespace TIMETRAKUP
{
    public partial class UpdateTime : Form
    {
        List<User> AllUsers = new List<User>();
        private bool IsDescending = false;
        private int SelectedColumn = 1;
        private DateTime Start;
        private DateTime End;
        private DateTime StartingDate;
        private DateTime EndingDate;
        private int TimeTrackingId;
        private Func<UserTime, object> predicate = null;
        private DataGridViewRow SelectedRow;

        User SelectedUser
        {
            get
            {
                try
                {
                    return (User)AllUsers.Where(p => p.WindowsUserName == Environment.UserName).SingleOrDefault();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        UserTime record;

        public UpdateTime(DataAccessHelper.Mode mode)
        {
            InitializeComponent();
            DataAccessHelper.CurrentMode = mode;

            //Get list of all active users
            AllUsers = DataAccess.AllUsers().Where(train => !train.FullName.Contains("train")).OrderBy(p => p.FullName).ToList();

            End = To.Value.Date;

            //Set the date to todays date and reset the grid view so that it displays no data
            To.Value = From.MaxDate = DateTime.Now;
            UserTimes.DataSource = null;
            SetStartTime();
            LoadTimes();

            SetUserNameText();
            SetVersionNumber();

            this.Text = this.Text + " - " + mode.ToString();
        }

        private void SetUserNameText()
        {
            try
            {
                UserName.Text = (AllUsers.Where(p => p.WindowsUserName == Environment.UserName).SingleOrDefault()).FullName;
            }
            catch (Exception)
            {
                UserName.Text = "User Not Found";
            }
        }

        private void SetVersionNumber()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            VersionNumber.Text = string.Format("Version {0}.{1}.{2}", version.Major, version.Minor, version.Build);
        }

        /// <summary>
        /// Queries the table and sets the datasource for the grid control
        /// </summary>
        /// <param name="index">The index of the selected header row for the sort order</param>
        private void LoadTimes()
        {
            try
            {
                //Make sure that a user has been selected
                if (SelectedUser != null && !SelectedUser.FullName.IsNullOrEmpty())
                {
                    UserTimes.DataSource = null;
                    List<UserTime> times = DataAccess.GetTimesForUser(SelectedUser, UnstoppedOnly.Checked);
                    List<UserTime> startEnd = times.Where(d => (d.StartTime >= Start && d.EndTime >= Start) && (d.StartTime <= End && d.EndTime <= End)).ToList();
                    List<UserTime> startNull = times.Where(d => (d.StartTime >= Start && d.EndTime == null) && (d.StartTime <= End && d.EndTime == null)).ToList();
                    List<UserTime> combined = startEnd.Union(startNull).ToList();
                    switch (SelectedColumn)
                    {
                        case 2:
                            predicate = p => p.TicketID;
                            break;
                        case 3:
                            predicate = p => p.StartTime;
                            break;
                        case 4:
                            predicate = p => p.EndTime;
                            break;
                        case 5:
                            predicate = p => p.Elapsed;
                            break;
                        default:
                            predicate = p => p.Region;
                            break;
                    }
                    if (IsDescending)
                        combined = combined.OrderBy(predicate).ToList();
                    else
                        combined = combined.OrderByDescending(predicate).ToList();
                    UserTimes.DataSource = combined.ToList();

                    //Removing the TimeTrackingId column because it's not necessary to see
                    if (UserTimes.Columns.Count == 6 && UserTimes.Columns[0].HeaderText.ToUpper().Contains("TIMETRACKINGID"))
                        UserTimes.Columns[0].Visible = false;
                    if (TimeTrackingId > 0)
                    {
                        DataGridViewRow row = UserTimes.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[0].Value.ToString().Equals(TimeTrackingId.ToString())).FirstOrDefault();
                        if (row != null)
                            UserTimes.Rows[row.Index].Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n\r\n" + ex.StackTrace);
            }
        }

        /// <summary>
        /// Gets the current month and sets the starting month for the quarter we are currently in
        /// </summary>
        private void SetStartTime()
        {
            //convert to first day of first month of quarter
            int curMonth = (int)((DateTime.Now.Month - 1) / 3) * 3 + 1;
            From.Value = Start = new DateTime(DateTime.Now.Year, curMonth, 1);
        }

        private void UnstoppedOnly_CheckedChanged(object sender, EventArgs e)
        {
            LoadTimes();
        }

        private void UnlockStart_Click(object sender, EventArgs e)
        {
            if (UnlockStart.Text.Contains("Unlock Start"))
            {
                StartDate.Enabled = true;
                StartTime.Enabled = true;
                UnlockStart.Text = "Lock Start";
            }
            else if (UnlockStart.Text.Contains("Lock Start"))
            {
                StartDate.Enabled = false;
                StartTime.Enabled = false;
                UnlockStart.Text = "Unlock Start";
            }
        }

        private void UserTime_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedRow = UserTimes.SelectedRows[0];
            StartDate.MinDate = StartTime.MinDate = EndTime.MinDate = EndDate.MinDate = new DateTime(1900, 1, 1);
            StartDate.MaxDate = StartTime.MaxDate = EndTime.MaxDate = EndDate.MaxDate = new DateTime(9000, 1, 1);
            StartDate.Value = StartTime.Value = new DateTime(1900, 1, 1);
            EndDate.Value = EndTime.Value = new DateTime(9000, 1, 1);
            Func<string, string> getCellValue = new Func<string, string>((s) => (UserTimes.SelectedRows[0].Cells[s].Value ?? "").ToString());
            Func<string, DateTime> getCellDate = new Func<string, DateTime>((s) => getCellValue(s).ToDateNullable() ?? DateTime.Now.Date);
            TicketID.Text = getCellValue("TicketID");
            ElapsedTime.Text = getCellValue("Elapsed");
            DateTime ds = getCellDate("StartTime");
            if (ds > DateTime.Now)
                StartDate.Value = StartTime.Value = getCellDate("EndTime");
            else
                StartDate.Value = StartTime.Value = ds;
            if (EndDate.Value < StartDate.Value)
                EndDate.Value = EndTime.Value = StartDate.Value;

            if (getCellDate("EndTime").TimeOfDay == new TimeSpan(0, 0, 0))
            {
                EndDate.Value = DateTime.Now.Date;
                EndTime.Value = DateTime.Now;
            }
            else
                EndDate.Value = EndTime.Value = getCellDate("EndTime");
            StartingDate = StartDate.Value;
            if (ElapsedTime.Text != "")
                if (TimeSpan.Parse(ElapsedTime.Text).TotalSeconds < 0)
                    EndingDate = StartDate.Value;
                else
                    EndingDate = EndDate.Value;

            record = new UserTime();
            record.TimeTrackingId = int.Parse(getCellValue("TimeTrackingId"));
            record.StartTime = StartTime.Value;
            record.EndTime = EndTime.Value;
            EndDate.Enabled = true;
            EndTime.Enabled = true;
            UnlockStart.Enabled = true;
            Update.Enabled = true;
            EndDate.MinDate = StartDate.Value;
        }

        /// <summary>
        /// Validates and updates the selected record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TicketID.Text))
            {
                UserTime UpdatedRecord = new UserTime();

                UpdatedRecord.TimeTrackingId = record.TimeTrackingId;
                UpdatedRecord.StartTime = DateTime.Parse(StartDate.Text + " " + StartTime.Text);
                if (UpdatedRecord.StartTime.TimeOfDay == DateTime.Today.TimeOfDay)
                    UpdatedRecord.StartTime = UpdatedRecord.StartTime.AddSeconds(1);
                UpdatedRecord.EndTime = DateTime.Parse(EndDate.Text + " " + EndTime.Text);
                if (UpdatedRecord.EndTime.Value.TimeOfDay == DateTime.Today.TimeOfDay)
                    UpdatedRecord.EndTime = UpdatedRecord.EndTime.Value.AddSeconds(1);

                if ((record.StartTime == UpdatedRecord.StartTime) && (record.EndTime == UpdatedRecord.EndTime))
                    MessageBox.Show("There were no changes made to the start and end times.", "No Changes Made", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (UpdatedRecord.StartTime > DateTime.Parse(EndDate.Text + " " + EndTime.Text))
                    MessageBox.Show("The ending date and time is before the starting date and time.", "No Changes Made", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (DateTime.Parse(EndDate.Text + " " + EndTime.Text) > DateTime.Now)
                    MessageBox.Show("The ending date and time is in the future. Please fix and try again.", "Future Date/Time", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (DataAccess.UpdateRecord(UpdatedRecord))
                {
                    MessageBox.Show("Record Successfully Updated", "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Func<string, string> getCellValue = new Func<string, string>((s) => (UserTimes.SelectedRows[0].Cells[s].Value ?? "").ToString());
                    TimeTrackingId = int.Parse(getCellValue("TimeTrackingId"));
                    IsDescending = !IsDescending; //It will change in load times so this sets it back to the original setting.
                    UpdatedRecord.Region = getCellValue("Region");
                    UpdatedRecord.TicketID = int.Parse(getCellValue("TicketId"));
                    SetSelectedRow(UpdatedRecord);
                    TicketID.Text = string.Empty;
                    ElapsedTime.Text = string.Empty;
                    StartDate.Enabled = false;
                    StartTime.Enabled = false;
                    EndDate.Enabled = false;
                    EndTime.Enabled = false;
                    UnlockStart.Enabled = false;
                    Update.Enabled = false;
                    UnlockStart.Text = "Unlock Start";
                    UserTimes.AutoResizeColumns();
                }
                else
                    MessageBox.Show("Record not updated. Verify all data and try again.", "Record Not Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("You must choose a record first", "No Record Chosen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Sorts the data by the selected header item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserTime_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            List<UserTime> times = DataAccess.GetTimesForUser(SelectedUser, UnstoppedOnly.Checked);
            UserTimes.DataSource = null;

            SelectedColumn = e.ColumnIndex;
            IsDescending = !IsDescending;
            LoadTimes();
        }

        private void From_ValueChanged(object sender, EventArgs e)
        {
            Start = From.Value.Date;
            LoadTimes();
        }

        private void To_ValueChanged(object sender, EventArgs e)
        {
            End = new DateTime(To.Value.Year, To.Value.Month, To.Value.Day, 23, 59, 59);
            LoadTimes();
        }

        private void StartDate_ValueChanged(object sender, EventArgs e)
        {
            if (StartDate.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Please choose a date in the past. Future dates are not allowed", "Future Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                StartDate.Value = StartingDate;
                EndDate.Value = EndingDate;
            }
            else if (StartDate.Value.Date > EndDate.Value.Date)
            {
                EndDate.Value = StartDate.Value;
                EndTime.MinDate = StartTime.Value;
            }
            else
                EndDate.MinDate = StartDate.Value.Date;
            UpdateElapsedTime(sender, e);
        }

        private void UpdateElapsedTime(object sender, EventArgs e)
        {
            DateTime endedDate = new DateTime(EndDate.Value.Year, EndDate.Value.Month, EndDate.Value.Day, EndTime.Value.Hour, EndTime.Value.Minute, EndTime.Value.Second, EndTime.Value.Millisecond);
            DateTime startedDate = new DateTime(StartDate.Value.Year, StartDate.Value.Month, StartDate.Value.Day, StartTime.Value.Hour, StartTime.Value.Minute, StartTime.Value.Second, StartTime.Value.Millisecond);
            TimeSpan span = endedDate - startedDate;
            ElapsedTime.Text = span.Days.ToString() + "." +
                            span.Hours.ToString("00") + ":" +
                            span.Minutes.ToString("00") + ":" +
                            span.Seconds.ToString("00");
            if ((endedDate - startedDate).TotalSeconds < 0)
            {
                EndDate.Value = StartDate.Value;
                EndTime.Value = StartTime.Value;
            }
        }

        private void SetSelectedRow(UserTime updatedRecord)
        {
            int row = UserTimes.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[0].Value.ToString().Equals(updatedRecord.TimeTrackingId.ToString())).FirstOrDefault().Index;
            UserTimes.Rows[row].Cells["StartTime"].Value = updatedRecord.StartTime;
            UserTimes.Rows[row].Cells["EndTime"].Value = updatedRecord.EndTime;
            UserTimes.Rows[row].Cells["Region"].Value = updatedRecord.Region;
            UserTimes.Rows[row].Cells["Elapsed"].Value = updatedRecord.Elapsed;
            UserTimes.Rows[row].Cells["TicketId"].Value = updatedRecord.TicketID;
            UserTimes.Refresh();
        }

    }//Class
}//NameSpace

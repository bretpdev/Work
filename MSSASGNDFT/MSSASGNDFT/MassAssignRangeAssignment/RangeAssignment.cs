using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace MassAssignRangeAssignment
{
    public partial class RangeAssignment : Form
    {
        public List<SqlUser> Users { get; set; }
        public List<UserRange> CurrentRanges { get; set; }
        public SqlUser CurrentUser { get; set; }
        public List<QueueNames> Queues { get; set; }
        public List<Queues> DeleteQueues { get; set; }
        public List<Queues> UpdateQueues { get; set; }
        public DataAccess DA { get; set; }
        public List<UserRange> Ranges { get; set; }

        public RangeAssignment(List<SqlUser> users, ProcessLogRun logRun)
        {
            InitializeComponent();
            DA = new DataAccess(logRun);
            Users = users;
            LoadRanges(DA.GetCurrentUsers());
            LoadQueues();
            CurrentUser = Users.Where(p => p.WindowsUserName == Environment.UserName).FirstOrDefault();
            Text = string.Format("Collections Mass Assign Batch :: Version {0}", FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
            DeleteQueues = new List<Queues>();
            UpdateQueues = new List<Queues>();
        }

        /// <summary>
        /// Adds a new blank UserRange control with the Begin/End ranges set.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int mid = 0;
            if (CurrentRanges.Count == 0)
                CurrentRanges.Insert(CurrentRanges.Count, new UserRange() { BeginRange = 0, EndRange = 99, CountrolNumber = 1 });
            else if (CurrentRanges[CurrentRanges.Count - 1].BeginRange != null && (CurrentRanges.Count == 1 || CurrentRanges[CurrentRanges.Count - 2].EndRange < 96))
            {
                mid = 99 - ((99 - CurrentRanges[CurrentRanges.Count - 1].BeginRange.Value) / 2);
                CurrentRanges.Insert(CurrentRanges.Count, new UserRange() { EndRange = 99, CountrolNumber = CurrentRanges.Count + 1, BeginRange = mid < 98 ? mid + 1 : mid });
            }
            else
            {
                MessageBox.Show("There are not enough ranges left. Please adjust the number to allow for more ranges.", "No Ranges Left", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            RangePanel.Controls.Add(new Ranges(this, Users, CurrentRanges, DA, CurrentRanges.Count - 1));
            ResetRanges(mid);
            RangePanel.Refresh();
            RangePanel.AutoScrollPosition = new Point(0, RangePanel.Height);
        }

        private void ResetRanges(int mid)
        {
            if (CurrentRanges.Count == 1)
            {
                ((Ranges)RangePanel.Controls[CurrentRanges.Count - 1]).End.Value = 99;
                ((Ranges)RangePanel.Controls[CurrentRanges.Count - 1]).End.Enabled = true;
            }
            else if (CurrentRanges.Count > 1)
            {
                ((Ranges)RangePanel.Controls[CurrentRanges.Count - 2]).IsLoading = true;
                ((Ranges)RangePanel.Controls[CurrentRanges.Count - 2]).End.Value = mid;
                ((Ranges)RangePanel.Controls[CurrentRanges.Count - 2]).End.Enabled = true;
            }
        }

        /// <summary>
        /// Moves the current data to the history table then saves the new data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, EventArgs e)
        {
            List<UserRange> ranges = CreateAndSaveRanges(true);
            if (ranges != null && ValidateRanges(ranges))
            {
                if (DA.InsertRangeAssignment(ranges))
                    MessageBox.Show("Range Assignment Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Clears all changes and pulls what is in the database to start over.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartOver_Click(object sender, EventArgs e)
        {
            LoadRanges(DA.GetCurrentUsers());
        }

        /// <summary>
        /// Adds a new queue to the list of queues to be processed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddQueue_Click(object sender, EventArgs e)
        {
            QueueNames q = new QueueNames();
            q.QueueName = QueueName.Text.ToUpper();
            q.FutureDated = FutureDated.Checked;
            DA.InsertQueue(q);
            LoadQueues();
            QueueName.Text = "";
        }

        /// <summary>
        /// Removes all selected queues from the queues to be processed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Click(object sender, EventArgs e)
        {
            foreach (Queues q in DeleteQueues)
            {
                DA.RemoveQueue(q.Queue.QueueId);
            }
            LoadQueues();
            Delete.Enabled = false;
        }

        private void Update_Click(object sender, EventArgs e)
        {
            foreach (Queues q in UpdateQueues)
            {
                DA.UpdateQueue(q.Queue);
            }
            LoadQueues();
            Update.Enabled = false;
        }

        /// <summary>
        /// Checks the QueueName textbox to see if there is a queue name provided and unlocks the AddQueue button if there is.
        /// Checks the new queue name provided to make sure it is not already in the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QueueName_TextChanged(object sender, EventArgs e)
        {
            if (QueueName.Text != "")
                AddQueue.Enabled = true;
            else
                AddQueue.Enabled = false;

            if (Queues.Any(p => p.QueueName.Contains(QueueName.Text.ToUpper())))
                AddQueue.Enabled = false;
        }

        /// <summary>
        /// Checks if the enter key is pressed and called AddQueue_Click if it is.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QueueName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                AddQueue_Click(sender, new EventArgs());
        }

        public void RefreshPanel(List<UserRange> ranges)
        {
            RangePanel.Controls.Clear();
            CurrentRanges = ranges;
            int i = 0;
            foreach (UserRange range in ranges)
            {
                RangePanel.Controls.Add(new Ranges(this, Users, CurrentRanges, DA, i));
                i++;
            }
            RangePanel.Refresh();
            RangePanel.AutoScrollPosition = new Point(0, RangePanel.Height);
        }
        /// <summary>
        /// Gets the data from the database and creates a new UserRange control for each database instance.
        /// </summary>
        /// <param name="userRanges"></param>
        private void LoadRanges(List<UserRange> userRanges)
        {
            CurrentRanges = userRanges;
            RangePanel.Controls.Clear();
            if (RangeAssignGroup.Enabled)
            {
                int num = 1;
                userRanges = userRanges?.OrderBy(p => p.BeginRange).ToList();
                foreach (UserRange user in userRanges)
                {
                    user.CountrolNumber = num;
                    if (num == 0 && user.BeginRange != 0)
                        user.BeginRange = 0;
                    Ranges r = new Ranges(this, Users, userRanges, DA, num - 1);
                    RangePanel.Controls.Add(r);
                    num++;
                }
            }
        }

        /// <summary>
        /// Loads each queue into the panel
        /// </summary>
        private void LoadQueues()
        {
            QueuePanel.Controls.Clear();
            Queues = DA.GetQueueList().OrderBy(p => p.QueueName).ToList();
            foreach (QueueNames queue in Queues)
            {
                Queues q = new Queues(queue, this);
                QueuePanel.Controls.Add(q);
            }
        }

        /// <summary>
        /// Updates the UserRange controls with any data changes
        /// </summary>
        /// <param name="controlNumber"></param>
        /// <param name="begin"></param>
        ///public void UpdateRange(int controlNumber, int begin, int? end)
        public void UpdateRange(UserRange range, bool begin)
        {
            UpdatePrevious(range, begin);
            UpdateCurrent(range, begin);
            UpdateNext(range);

            foreach (UserControl cntl in RangePanel.Controls)
            {
                cntl.Refresh();
            }
        }

        private void UpdatePrevious(UserRange range, bool begin)
        {
            if (range.CountrolNumber == 1)
                return;

            if (begin)
            {
                ((Ranges)RangePanel.Controls[range.CountrolNumber - 2]).End.Minimum = CurrentRanges[range.CountrolNumber - 2].BeginRange.Value + 1;
                ((Ranges)RangePanel.Controls[range.CountrolNumber - 2]).End.Maximum = CurrentRanges[range.CountrolNumber - 1].BeginRange.Value - 1;
                ((Ranges)RangePanel.Controls[range.CountrolNumber - 2]).End.Value = ((Ranges)RangePanel.Controls[range.CountrolNumber - 1]).Begin.Value - 1;
                ((Ranges)RangePanel.Controls[range.CountrolNumber - 2]).Begin.Maximum = ((Ranges)RangePanel.Controls[range.CountrolNumber - 1]).Begin.Value - 2;
            }
        }

        private void UpdateCurrent(UserRange range, bool begin)
        {
            if (begin)
            {
                ((Ranges)RangePanel.Controls[range.CountrolNumber - 1]).Begin.Minimum = CurrentRanges[range.CountrolNumber - 2].BeginRange.Value + 2;
                ((Ranges)RangePanel.Controls[range.CountrolNumber - 1]).Begin.Maximum = range.EndRange.Value - 1;
                ((Ranges)RangePanel.Controls[range.CountrolNumber - 1]).Begin.Value = range.BeginRange.Value;
            }
            else
            {
                ((Ranges)RangePanel.Controls[(CurrentRanges.Count > 1 && range.CountrolNumber > 1) ? range.CountrolNumber - 1 : 0]).End.Minimum = range.BeginRange.Value + 1;
                ((Ranges)RangePanel.Controls[(CurrentRanges.Count > 1 && range.CountrolNumber > 1) ? range.CountrolNumber - 1 : 0]).End.Maximum = CurrentRanges[range.CountrolNumber].EndRange.Value - 2;
                ((Ranges)RangePanel.Controls[(CurrentRanges.Count > 1 && range.CountrolNumber > 1) ? range.CountrolNumber - 1 : 0]).End.Value = range.EndRange.Value;
            }
        }

        private void UpdateNext(UserRange range)
        {
            if (range.CountrolNumber == RangePanel.Controls.Count)
                return;

            ((Ranges)RangePanel.Controls[range.CountrolNumber]).Begin.Minimum = CurrentRanges[range.CountrolNumber - 1].EndRange.Value + 1;
            ((Ranges)RangePanel.Controls[range.CountrolNumber]).Begin.Maximum = CurrentRanges[range.CountrolNumber].EndRange.Value - 1;
            ((Ranges)RangePanel.Controls[range.CountrolNumber]).Begin.Value = CurrentRanges[range.CountrolNumber].BeginRange.Value;
            ((Ranges)RangePanel.Controls[range.CountrolNumber]).End.Minimum = CurrentRanges[range.CountrolNumber].BeginRange.Value + 1;
        }

        /// <summary>
        /// Verifies that all controls have all data fille out.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        private bool ValidateRange(Ranges range)
        {
            string message = "";
            if (range.UserId.SelectedIndex == 0)
                message = "A user must be selected\r\n";
            if (range.AesId.Text.IsNullOrEmpty() || range.AesId.Text.Length < 7 || !range.AesId.Text.Contains("UT"))
                message += "A valid UT ID is required\r\n";
            if (range.Begin.Value == 0 && range.ControlNumber != 1)
                message += "Invalid start range\r\n";
            if (range.End.Value == 0)
                message += "Invalid end range";
            if (message.IsPopulated())
            {
                Uheaa.Common.Dialog.Error.Ok(message, "Missing Data");
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Checks each range and verifies the next range start and end time are after the previous.
        /// </summary>
        private bool ValidateRanges(List<UserRange> ranges)
        {
            int start = ranges[0].BeginRange.Value;
            int end = ranges[0].EndRange.Value;
            for (int i = 1; i < ranges.Count; i++)
            {
                if (end > ranges[i].BeginRange.Value)
                {
                    Uheaa.Common.Dialog.Error.Ok(string.Format("The start range of row {0} is less than the end range of row {1} which is not possible.", ranges[i].CountrolNumber + 1, ranges[i].CountrolNumber));
                    return false;
                }
                if (start > end)
                {
                    Uheaa.Common.Dialog.Error.Ok(string.Format("The start range of row {0} is greater than the end range of row {1} which is not possible.", ranges[i].CountrolNumber, ranges[i].CountrolNumber));
                    return false;
                }
                start = ranges[i].BeginRange.Value;
                end = ranges[i].EndRange.Value;
            }
            return true;
        }

        /// <summary>
        /// Removed the bottom UserRange control and resets the begin/end ranges for the previous control.
        /// </summary>
        public void Remove(int controlNumber)
        {
            if (CurrentRanges.Count == 2 || controlNumber == CurrentRanges.Count)
            {
                CurrentRanges.Remove(CurrentRanges[CurrentRanges.Count - 1]);
                CurrentRanges[CurrentRanges.Count - 1].EndRange = 99;
            }
            else
            {
                UserRange remove = CurrentRanges.Where(p => p.CountrolNumber == controlNumber).First();
                CurrentRanges.Remove(remove);
                int? mid = (remove.EndRange - remove.BeginRange) / 2;
                CurrentRanges[remove.CountrolNumber - 2].EndRange = remove.BeginRange + mid;
                CurrentRanges[remove.CountrolNumber - 1].BeginRange = remove.BeginRange + mid + 1;
                foreach (UserRange range in CurrentRanges.Skip(remove.CountrolNumber - 1))
                {
                    range.CountrolNumber = --range.CountrolNumber;
                }
            }
            LoadRanges(CurrentRanges);
        }

        private List<UserRange> CreateAndSaveRanges(bool saving)
        {
            List<UserRange> saveRanges = new List<UserRange>();
            foreach (Ranges range in RangePanel.Controls)
            {
                if (ValidateRange(range))
                {
                    UserRange r = new UserRange();
                    r.UserId = range.Range.UserId;
                    r.WindowsUserName = range.Range.WindowsUserName;
                    r.AesId = range.Range.AesId;
                    r.BeginRange = range.Range.BeginRange;
                    r.EndRange = range.Range.EndRange;
                    r.CountrolNumber = range.Range.CountrolNumber;
                    r.AddedBy = range.Range.AddedBy;
                    r.AddedOn = range.Range.AddedOn;
                    saveRanges.Add(r);
                }
                else
                {
                    if (saving)
                        return null;
                }
            }
            return saveRanges;
        }
    }
}
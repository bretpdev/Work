using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;

namespace MassAssignRangeAssignment
{
    public partial class Ranges : UserControl
    {
        private RangeAssignment RA;
        List<SqlUser> Users;
        public UserRange Range { get; set; }
        public List<UserRange> UserRanges { get; set; }
        public DataAccess DA { get; set; }
        public bool IsLoading { get; set; }
        public int ControlNumber { get; set; }

        public Ranges(RangeAssignment ra, List<SqlUser> users, List<UserRange> userRanges, DataAccess da, int count)
        {
            InitializeComponent();
            IsLoading = true;
            RA = ra;
            Users = users;
            Range = userRanges[count];
            UserRanges = userRanges;
            ControlNumber = Range.CountrolNumber;
            DA = da;
            SetData(userRanges);
            Count.Text = (++count).ToString();
            Begin.Enabled = true;
            End.Enabled = true;
            if (userRanges.Count > 1)
                userRanges[userRanges.Count - 2].EndRange = userRanges[userRanges.Count - 1].BeginRange - 1;
            if (count == UserRanges.Count)
            {
                End.Enabled = false;
                End.Value = 99;
            }
            if (count == 1)
            {
                Begin.Value = 0;
                Begin.Enabled = false;
            }
            IsLoading = false;
            if (Range.CountrolNumber == 1)
                Remove.Enabled = false;
            else
                Remove.Enabled = true;
        }

        /// <summary>
        /// Sets all the data for the new control being created
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        private void SetData(List<UserRange> userRanges)
        {
            if (Users[0].ID != 0)
                Users.Insert(0, new SqlUser() { BusinessUnit = 19 });
            UserId.DataSource = Users.Where(p => p.BusinessUnit == 19).ToList();
            UserId.DisplayMember = "WindowsUserName";
            UserId.ValueMember = "ID";
            SqlUser user = Users.Where(p => p.ID == Range.UserId).SingleOrDefault();
            int? index = (Range.UserId == 0 || user == null) ? 0 : UserId.Items.IndexOf(user);
            UserId.SelectedIndex = index.Value;
            Begin.Minimum = ControlNumber.IsIn(0, 1) ? 0 : userRanges[ControlNumber - (ControlNumber > 2 ? 3 : 2)].BeginRange.Value + 2;
            Begin.Maximum = ControlNumber == 1 ? 99 : ControlNumber == userRanges.Count ? 98 : userRanges[ControlNumber].EndRange.Value - 2;
            End.Minimum = Range.BeginRange.HasValue ? Range.BeginRange.Value + 1 : userRanges[ControlNumber - 3].EndRange.Value + 2;
            End.Maximum = ControlNumber.IsIn(0, userRanges.Count) ? 99 : ControlNumber == userRanges.Count - 1 ? 99 : userRanges[ControlNumber].EndRange.Value - 2;
            Begin.Value = Range.BeginRange.HasValue ? Range.BeginRange.Value : Begin.Minimum;
            End.Value = Range.EndRange.HasValue ? Range.EndRange.Value : Range.BeginRange.Value + 2;
        }

        /// <summary>
        /// Removes the UserRange from the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove_Click(object sender, EventArgs e)
        {
            if (Range.CountrolNumber > 1)
                RA.Remove(Range.CountrolNumber);
        }

        private void Begin_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoading)
            {
                if (Begin.Value > 98)
                {
                    Begin.Value = Range.BeginRange.Value;
                    return;
                }
                if (Begin.Value > Begin.Maximum || Begin.Value < Begin.Minimum)
                {
                    Begin.Value = Range.BeginRange.Value;
                    return;
                }
                BeginChange();
            }
        }

        private void BeginChange()
        {
            if (Begin.Text != "" && End.Text != "")
            {
                Range.BeginRange = (int)Begin.Value;
                UserRanges[Range.CountrolNumber - 2].EndRange = Range.BeginRange.Value - 1;
                if (UserRanges.Count > 2)
                    RA.UpdateRange(Range, true);
            }
            this.Refresh();
            RA.RefreshPanel(UserRanges);
        }

        private void End_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoading)
            {
                if (End.Value < 1)
                {
                    End.Value = Range.EndRange.Value;
                    return;
                }
                if (End.Value < Begin.Value)
                    return;
                EndChange();
            }
        }

        private void EndChange()
        {
            if (Begin.Text.IsPopulated() && End.Text.IsPopulated())
            {
                Range.EndRange = (int)End.Value;
                UserRanges[Range.CountrolNumber].BeginRange = Range.EndRange.Value + 1;
                RA.UpdateRange(Range, false);
            }
            this.Refresh();
        }

        /// <summary>
        /// Set the AesId when a user is selected from the UserId drop down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UserId.SelectedIndex > 0)
            {
                SqlUser user = Users.Where(p => p.ID == ((SqlUser)UserId.SelectedItem).ID).FirstOrDefault();
                AesId.Text = user.AesUserId;
                UserRange r = RA.CurrentRanges.Where(p => p.CountrolNumber == Range.CountrolNumber).FirstOrDefault();
                r.AesId = AesId.Text;
                r.UserId = ((SqlUser)UserId.SelectedItem).ID;
                r.WindowsUserName = Users.Where(p => p.ID == ((SqlUser)UserId.SelectedItem).ID).FirstOrDefault().WindowsUserName;
            }
        }
    }
}
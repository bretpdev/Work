using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using dah = Uheaa.Common.DataAccess.DataAccessHelper;

namespace CMPLNTRACK
{
    public partial class MainForm : Form
    {
        public ComplaintDataAccess DA { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public MainForm(Args args)
        {
            InitializeComponent();
            GroupsFilterBox.SelectedIndex = FlagsFilterBox.SelectedIndex = TypesFilterBox.SelectedIndex = PartiesFilterBox.SelectedIndex = 0;
            OpenClosedFilterBox.SelectedIndex = 1;
            SetRegion(dah.Region.Uheaa);
            if (!AccessHelper.IsAdmin)
            {
                FlagsMenu.Visible = TypesMenu.Visible = PartiesMenu.Visible = GroupsMenu.Visible = false;
                AccountNumberBox.Text = args.AccountNumber;
                AccountNumberBox.Enabled = args.AccountNumber.IsPopulated() ? false : true;
            }
        }

        private void SetRegion(dah.Region region)
        {
            dah.CurrentRegion = region;
            this.Text = string.Format("{0} Complaints", dah.CurrentRegion.ToString());
            FlagsMenu.Text = string.Format("Manage {0} Flags", dah.CurrentRegion.ToString());
            TypesMenu.Text = string.Format("Manage {0} Types", dah.CurrentRegion.ToString());
            PartiesMenu.Text = string.Format("Manage {0} Parties", dah.CurrentRegion.ToString());
            LogRun = new ProcessLogRun("CMPLNTRACK", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), dah.CurrentRegion, dah.CurrentMode);
            DA = new ComplaintDataAccess(LogRun);
            this.BackColor = Color.FromArgb(123, 193, 252);
            ReloadFilters();
            Search();
        }

        private void FlagsMenu_Click(object sender, EventArgs e)
        {
            var form = new ListTableEditor("Flag", "Flags", "FlagName", "FlagId", () => DA.FlagsGetAll(), s => DA.FlagAdd(s), i => DA.FlagDelete(i),
                (id, menu) =>
                {
                    var existingFlag = DA.FlagsGetAll().Single(o => o.FlagId == id);
                    var flagItem = new MenuItem() { Text = "Flag Enables Control Mail Fields", Checked = existingFlag.EnablesControlMailFields };
                    flagItem.Click += (o, ea) =>
                    {
                        DA.FlagSave(id, !existingFlag.EnablesControlMailFields);
                    };
                    menu.MenuItems.Add(flagItem);
                });
            form.ShowDialog(this);
            ReloadFilters();
        }

        private void PartiesMenu_Click(object sender, EventArgs e)
        {
            var form = new ListTableEditor("Party", "Parties", "PartyName", "ComplaintPartyId", () => DA.ComplaintPartiesGetAll(), n => DA.ComplaintPartyAdd(n), d =>
            {
                if (DA.OpenComplaintsByPartyId(d).Any())
                    Dialog.Warning.Ok("This party cannot be retired until open complaints referencing it are resolved.");
                else
                    DA.ComplaintPartyDelete(d);
            });
            form.ShowDialog(this);
            ReloadFilters();
        }

        private void TypesMenu_Click(object sender, EventArgs e)
        {
            var form = new ListTableEditor("Type", "Types", "TypeName", "ComplaintTypeId", () => DA.ComplaintTypesGetAll(), n => DA.ComplaintTypeAdd(n), d =>
            {
                if (DA.OpenComplaintsByTypeId(d).Any())
                    Dialog.Warning.Ok("This type cannot be retired until open complaints referencing it are resolved.");
                else
                    DA.ComplaintTypeDelete(d);
            });
            form.ShowDialog(this);
            ReloadFilters();
        }

        private void GroupsMenu_Click(object sender, EventArgs e)
        {
            var form = new ListTableEditor("Group", "Groups", "GroupName", "ComplaintGroupId", () => DA.GetComplaintGroupsGetAll(), n => DA.ComplaintGroupAdd(n), d =>
            {
                if (DA.OpenComplaintsByGroupId(d).Any())
                    Dialog.Warning.Ok("This group cannot be retired until open complaints referencing it are resolved.");
                else
                    DA.ComplaintGroupDelete(d);
            });
            form.ShowDialog(this);
            ReloadFilters();
        }


        private void ReloadFilters()
        {
            ControlDrawingHelper.SuspendDrawing(this);
            LoadFlags();
            LoadParties();
            LoadTypes();
            LoadGroups();
            ControlDrawingHelper.ResumeDrawing(this);
        }

        private void LoadFlags()
        {
            LoadComboBox<Flag>(FlagsFilterBox, DA.FlagsGetAll(), o => o.FlagName, o => o.FlagId);
        }
        private void LoadParties()
        {
            LoadComboBox<ComplaintParty>(PartiesFilterBox, DA.ComplaintPartiesGetAll(), o => o.PartyName, o => o.ComplaintPartyId);
        }

        private void LoadTypes()
        {
            LoadComboBox<ComplaintType>(TypesFilterBox, DA.ComplaintTypesGetAll(), o => o.TypeName, o => o.ComplaintTypeId);
        }

        private void LoadGroups()
        {
            LoadComboBox<ComplaintGroup>(GroupsFilterBox, DA.GetComplaintGroupsGetAll(), o => o.GroupName, o => o.ComplaintGroupId);
        }

        private void LoadComboBox<T>(ComboBox box, List<T> freshDatabaseItems, Func<T, string> getText, Func<T, int> getId)
        {
            int selectedIndex = box.SelectedIndex;
            while (box.Items.Count > 1)
                box.Items.RemoveAt(1); //clear all but first default item
            foreach (var item in freshDatabaseItems)
                box.Items.Add(new { Text = getText(item), Id = getId(item) });
            box.DisplayMember = "Text";
            box.ValueMember = "Id";
            box.SelectedIndex = selectedIndex;
        }

        private void Search()
        {
            if (DA != null)
            {
                bool openComplaints = OpenClosedFilterBox.SelectedIndex == 0 || OpenClosedFilterBox.SelectedIndex == 1;
                bool closedComplaints = OpenClosedFilterBox.SelectedIndex == 0 || OpenClosedFilterBox.SelectedIndex == 2;
                var getId = new Func<ComboBox, int?>(c =>
                {
                    if (c.SelectedIndex > 0)
                        return (int)c.SelectedItem.GetType().GetProperty("Id").GetValue(c.SelectedItem);
                    return null;
                });
                int? flagId = getId(FlagsFilterBox);
                int? partyId = getId(PartiesFilterBox);
                int? groupId = getId(GroupsFilterBox);
                int? typeId = getId(TypesFilterBox);
                ResultsGrid.AutoGenerateColumns = false;
                ResultsGrid.DataSource = DA.Search(AccountNumberBox.Text, openComplaints, closedComplaints, flagId, partyId, typeId, groupId);
            }
        }

        private void NewComplaintButton_Click(object sender, EventArgs e)
        {
            new ComplaintForm(DA, AccountNumberBox.Text).ShowDialog();
            Search();
        }

        private void ResultsGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var item = ResultsGrid.Rows[e.RowIndex].DataBoundItem as Complaint;
            new ComplaintForm(DA, item).ShowDialog();
            Search();
        }

        private void AccountNumberBox_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (AccountNumberBox.TextLength == 10)
                this.BeginInvoke(new Action(() =>
                {
                    if (!DA.Search(AccountNumberBox.Text, true, false, null, null, null, null).Any())
                        NewComplaintButton.PerformClick();
                }));
        }

    }
}
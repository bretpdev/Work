using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.WinForms;

namespace SftpCoordinator
{
    public partial class PathTypesForm : Form
    {
        public PathTypesForm()
        {
            InitializeComponent();
            NameText.InstalledBehaviors.Add(new TrackChangedValueBehavior<BehaviorTextBox>(NameText));
            NameText.InstalledBehaviors.Install();
            RootPathText.InstalledBehaviors.Add(new TrackChangedValueBehavior<BehaviorTextBox>(RootPathText));
            RootPathText.InstalledBehaviors.Install();
            LoadGrid();
            LoadSelectedPathType();
        }

        List<PathType> pathTypes;
        public PathType SelectedPathType
        {
            get
            {
                if (PathTypesList.SelectedIndex != -1)
                    return pathTypes[PathTypesList.SelectedIndex];
                return null;
            }
        }

        private void LoadGrid()
        {
            codeInitiated = true;
            PathTypesGroup.Enabled = true;
            int selectedIndex = PathTypesList.SelectedIndex;
            PathTypesList.DataSource = pathTypes = PathType.GetAll();
            codeInitiated = false;
            PathTypesList.SelectedIndex = Math.Min(Math.Max(selectedIndex, 0), PathTypesList.Items.Count - 1);
        }

        bool codeInitiated = false;
        private void PathTypesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (codeInitiated) return;
            if (SelectedPathType != null)
                LoadPathType(SelectedPathType);
            else
                DisableEdit();
        }

        private void LoadSelectedPathType()
        {
            if (SelectedPathType != null)
                LoadPathType(SelectedPathType, false);
        }

        private void LoadPathType(PathType pt, bool isNew = false)
        {
            EditGroup.Enabled = true;
            if (isNew)
            {
                EditGroup.Text = "Add";
                SaveButton.Text = "Add/Save";
                PathTypesGroup.Enabled = false;
                ViewLabel.Visible = false;
            }
            else
            {
                EditGroup.Text = "Edit";
                SaveButton.Text = "Save";
                PathTypesGroup.Enabled = true;
                ViewLabel.Visible = false;
                if (pt.AffectedFiles > 0)
                {
                    ViewLabel.Visible = true;
                    ViewLabel.Text = $"{pt.AffectedFiles} project files use this path";
                }
                DeleteButton.Enabled = pt.AffectedFiles == 0;
            }
            NameText.Value = pt.Description;
            RootPathText.Value = pt.RootPath.UpdatePath();
            SyncButtons();
        }

        private void DisableEdit()
        {
            EditGroup.Enabled = false;
            NameText.Value = RootPathText.Value = EditGroup.Text = "";
            SaveButton.Text = "Save";
            DeleteButton.Enabled = false;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (SelectedPathType == null)
                PathTypesList.SelectedIndex = 0;
            LoadSelectedPathType();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            PathType pt = new PathType()
            {
                Description = NameText.Text,
                RootPath = RootPathText.Text.UpdatePath()
            };
            if (pt.RootPath.IsNullOrEmpty())
            {
                Dialog.Error.Ok("Invalid file path");
                return;
            }
            if (SelectedPathType != null && pt.RootPath.IsPopulated())
            {
                pt.PathTypeId = SelectedPathType.PathTypeId;
                PathType.Save(pt);
            }
            else if (pt.RootPath.IsPopulated())
                PathType.Insert(pt);

            int selectedIndex = PathTypesList.SelectedIndex;
            LoadGrid();
            if (selectedIndex == -1)    
                PathTypesList.SelectedIndex = pathTypes.Select((obj, index) => new {Id = obj.PathTypeId, Index = index}).Where(o => o.Id == pt.PathTypeId).Single().Index;
            LoadSelectedPathType();
            SaveButton.Enabled = false;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            PathTypesList.ClearSelected();
            LoadPathType(new PathType() { }, true);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (Dialog.Warning.YesNo($"Are you sure you want to delete PathType ({SelectedPathType.Description})?"))
            {
                PathType.Delete(SelectedPathType);
                LoadGrid();
            }
        }

        private void PathTypesList_DoubleClick(object sender, EventArgs e)
        {
            NameText.Focus();
        }

        private void ViewLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new ProjectsForm(SelectedPathType).ShowDialog();
            LoadGrid();
            LoadSelectedPathType();
        }

        private void TextBox_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SyncButtons();
        }

        private void SyncButtons()
        {
            bool changeMade = NameText.InstalledBehaviors.Get<TrackChangedValueBehavior<BehaviorTextBox>>().HasChanged
                           || RootPathText.InstalledBehaviors.Get<TrackChangedValueBehavior<BehaviorTextBox>>().HasChanged;
            SaveButton.Enabled = changeMade;
            CancelButton.Enabled = changeMade || !PathTypesGroup.Enabled; //can always cancel a new path type
        }
    }
}

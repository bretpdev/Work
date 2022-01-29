using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.WinForms;

namespace SftpCoordinator
{
    public partial class ProjectsForm : Form
    {
        PathType affectedBy;
        Color affectedColor = Color.LightGreen;
        Color selectedColor = Color.FromArgb(255, 105, 173, 105);
        public ProjectsForm(PathType affectedBy = null)
        {
            InitializeComponent();
            if (affectedBy != null)
            {
                this.affectedBy = affectedBy;
                this.Text = $"Projects affected by Path Type ({affectedBy.Description})";
                NewProjectButton.Enabled = false;
                NewProjectFileButton.Enabled = false;
            }
            LoadProjects();
            ProjectsList.SelectedIndex = Math.Min(ProjectsList.Items.Count - 1, 0);
        }

        List<Project> affectedProjects;
        List<Project> currentProjects;
        Project currentProject;
        List<ProjectFile> currentProjectFiles;
        private void LoadProjects()
        {
            int selectedIndex = ProjectsList.SelectedIndex;
            //grab all projects
            var allProjects = Project.GetAllProjects();
            ProjectsList.DataSource = currentProjects = new List<Project>(allProjects);
            affectedProjects = new List<Project>();
            if (affectedBy != null)
            {
                var projects = allProjects.Where(o => ProjectFile.GetProjectFilesByProject(o).Any(pf =>
                    pf.SourcePathTypeId == affectedBy.PathTypeId || pf.DestinationPathTypeId == affectedBy.PathTypeId));
                affectedProjects.AddRange(projects);
            }
            ProjectsList.ValueMember = "ProjectId";
            ProjectsList.DisplayMember = "Name";
            DeleteProjectButton.Enabled = allProjects.Any();
            selectedIndex = Math.Max(selectedIndex, 0); //don't go under 0
            selectedIndex = Math.Min(selectedIndex, ProjectsList.Items.Count - 1); //don't go over list's count
            if (selectedIndex != -1)
                ProjectsList.SelectedIndex = selectedIndex;
            LoadCurrentProject();
        }

        private void LoadCurrentProject()
        {
            if (ProjectsList.SelectedIndex == -1)
            {
                ProjectFilesGrid.DataSource = null;
                return;
            }
            var project = currentProjects.Skip(ProjectsList.SelectedIndex).FirstOrDefault();
            if (project != null)
            {
                int index = 0;
                int display = 0;
                if (project == currentProject && ProjectFilesGrid.SelectedRows.Count == 1)
                {
                    index = ProjectFilesGrid.SelectedRows[0].Index;
                    display = ProjectFilesGrid.FirstDisplayedScrollingRowIndex;
                }
                currentProject = project;
                ProjectFilesGrid.AutoGenerateColumns = false;
                ProjectFilesGrid.DataSource = currentProjectFiles = ProjectFile.GetProjectFilesByProject(project).ToList();
                if (currentProjectFiles.Count > index)
                {
                    ProjectFilesGrid.Rows[index].Selected = true;
                    ProjectFilesGrid.FirstDisplayedScrollingRowIndex = display;
                }

                if (affectedBy != null)
                {
                    for (int i = 0; i < currentProjectFiles.Count; i++)
                    {
                        ProjectFile pf = currentProjectFiles[i];
                        foreach (DataGridViewCell cell in ProjectFilesGrid.Rows[i].Cells)
                            if (pf.SourcePathTypeId == affectedBy.PathTypeId || pf.DestinationPathTypeId == affectedBy.PathTypeId)
                                cell.Style = new DataGridViewCellStyle() { BackColor = affectedColor, SelectionBackColor = selectedColor };
                            else
                                cell.Style = new DataGridViewCellStyle() { BackColor = Color.White, SelectionBackColor = SystemColors.Highlight };
                    }
                }
                ProjectFilesGrid.Invalidate();
                DeleteProjectFileButton.Enabled = currentProjectFiles.Any();
            }
        }

        private void ProjectsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCurrentProject();
        }

        private void ProjectsList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = ProjectsList.IndexFromPoint(e.Location);
            if (index == -1)
                return;
            var project = currentProjects.Skip(index).FirstOrDefault();
            if (project != null)
                ShowProjectFormUpdateDialog(project);
        }

        private void ProjectsList_ResolveItemColor(object sender, ResolveItemColorEventArgs e)
        {
            Project p = currentProjects[e.ItemIndex];
            if (affectedProjects.Contains(p))
            {
                e.BackgroundColor = affectedColor;
                e.HighlightColor = selectedColor;
            }
        }

        private void DeleteProjectButton_Click(object sender, EventArgs e)
        {
            DeleteProjectDialog();
        }

        private bool DeleteProjectDialog()
        {
            int file = currentProjectFiles.Count();
            var form = ActiveForm;
            if (Dialog.Warning.YesNo("Deleting this project will also delete " + file + " associated project files.  Are you sure you want to delete this project?"))
            {
                foreach (ProjectFile pf in currentProjectFiles)
                    ProjectFile.Delete(pf);
                Project.DeleteProject(currentProject);
                LoadProjects();
                return true;
            }
            return false;
        }

        private void NewProjectButton_Click(object sender, EventArgs e)
        {
            ShowProjectFormInsertDialog();
        }

        private ProjectFile SelectedProjectFile
        {
            get
            {
                var row = ProjectFilesGrid.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault();
                if (row != null)
                {
                    int index = row.Index;
                    var projectFile = currentProjectFiles.Skip(index).First();
                    return projectFile;
                }
                return null;
            }
        }

        private void ProjectFilesGrid_DoubleClick(object sender, EventArgs e)
        {
            if (SelectedProjectFile != null)
                ShowProjectFileFormUpdateDialog(SelectedProjectFile);
        }


        private void NewProjectFileButton_Click(object sender, EventArgs e)
        {
            ShowProjectFileFileFormInsertDialog();
        }

        private void DeleteProjectFileButton_Click(object sender, EventArgs e)
        {
            if (SelectedProjectFile != null)
                if (Dialog.Warning.YesNo("Are you sure you want to delete this project file?"))
                {
                    ProjectFile.Delete(SelectedProjectFile);
                    LoadCurrentProject();
                }
        }

        #region Built Forms
        private void ShowProjectFormUpdateDialog(Project project)
        {
            FormBuilder fb = GenerateProjectFormBuilder("Edit Project", project);
            fb.IncludeAlternateButton = true;
            if (FormBuilder.Generate(project, fb).ShowPopoverDialog(this) == true)
                Project.UpdateProject(project);
            LoadProjects();
        }
        private void ShowProjectFormInsertDialog()
        {
            Project blankProject = new Project();
            FormBuilder fb = GenerateProjectFormBuilder("Create Project", blankProject);
            fb.IncludeAlternateButton = false;
            if (FormBuilder.Generate(blankProject, fb).ShowPopoverDialog(this) == true)
                Project.InsertProject(blankProject);
            LoadProjects();
        }
        private FormBuilder GenerateProjectFormBuilder(string titlePrepend, Project project = null)
        {
            bool newProject = project == null;
            if (newProject) project = new Project();

            FormBuilder fb = new FormBuilder(titlePrepend + " " + project.Name);
            fb.InputWidth = 300;
            fb.FormAccepted += (form) =>
            {
                return true;
            };
            fb.FormAlternate += (form) =>
            {
                return DeleteProjectDialog();
            };
            return fb;
        }
        private void ShowProjectFileFileFormInsertDialog()
        {
            ProjectFile blank = new ProjectFile();
            blank.ProjectId = currentProject.ProjectId;
            //sometimes the min path id isn't 0
            var minPath = PathType.GetAll().Select(o => o.PathTypeId).Min();
            blank.SourcePathTypeId = minPath;
            blank.DestinationPathTypeId = minPath;
            FormBuilder fb = GenerateProjectFileFormBuilder("Add Project File", blank);
            bool? result = (FormBuilder.Generate(blank, fb).ShowPopoverDialog(this));
            if (result == true)
                ProjectFile.Insert(blank);

            if (affectedBy != null)
                LoadProjects();
            else
                LoadCurrentProject();
        }

        private void ShowProjectFileFormUpdateDialog(ProjectFile pf)
        {
            FormBuilder fb = GenerateProjectFileFormBuilder("Edit Project File", pf);
            fb.IncludeAlternateButton = true;

            if (FormBuilder.Generate(pf, fb).ShowPopoverDialog(this) == true)
                ProjectFile.Update(pf);

            if (affectedBy != null)
                LoadProjects();
            else
                LoadCurrentProject();
        }

        private FormBuilder GenerateProjectFileFormBuilder(string title, ProjectFile pf)
        {
            //defining these fields because we want to reference more than one of them going forward
            IFormField<TextBox> searchPattern = null, antiSearchPattern = null;
            IFormField<FormatBox> aggregationFormatString = null, renameFormatString = null;
            FormBuilder fb = new FormBuilder("Add Project File");
            fb.InputWidth = 400;
            var sourceType = fb.AddField<CycleButton<int>>("Source Type", (t) =>
            {
                t.Options.AddRange(PathType.CachedCycleOptions);
                t.SelectedValue = pf.SourcePathTypeId;
            }, 1);
            var source = fb.AddField<EventValidationTextBox>("Source Root", (t) =>
            {
                t.Text = pf.SourceRoot;
                t.OnValidate += (sender) => {
                    if (t.Text.IsNullOrEmpty())
                        return new ValidationResults(false);
                    string root = PathType.CachedPathTypes[fb.GetControl(sourceType).SelectedValue].RootPath;
                    string text = t.Text;
                    string both = root + text;
                    if (both.Contains('/') && both.Contains('\\'))
                        return new ValidationResults(false, "Combined path (PathType + Path) contains both slashes and backslashes.  This is an incompatible path.");
                    return ValidationResults.Successful;
                };
            }, 2);
            var destType = fb.AddField<CycleButton<int>>("Destination Type", (t) =>
            {
                t.Options.AddRange(PathType.CachedCycleOptions);
                t.SelectedValue = pf.DestinationPathTypeId;
            }, 3);
            var dest = fb.AddField<EventValidationTextBox>("Destination Root", (t) =>
            {
                t.Text = pf.DestinationRoot;
                t.OnValidate += (sender) => {
                    if (t.Text.IsNullOrEmpty())
                        return new ValidationResults(false);
                    string root = PathType.CachedPathTypes[fb.GetControl(destType).SelectedValue].RootPath;
                    string text = t.Text;
                    string both = root + text;
                    if (both.Contains('/') && both.Contains('\\'))
                        return new ValidationResults(false, "Combined path (PathType + Path) contains both slashes and backslashes.  This is an incompatible path.");
                    return ValidationResults.Successful;
                };
            }, 4);
            EventHandler patternHandler = (o, ea) =>
            {
                var search = fb.GetControl(searchPattern);
                var anti = fb.GetControl(antiSearchPattern);
                if (search.Text.IsNullOrEmpty() && anti.Text.IsNullOrEmpty())
                    search.Text = "*";
            };
            searchPattern = fb.AddField<TextBox>("Search Pattern", (t) =>
            {
                t.Text = pf.SearchPattern;
                t.Leave += patternHandler;
            }, 4);
            antiSearchPattern = fb.AddField<TextBox>("Anti-Search Pattern", (t) =>
            {
                t.Text = pf.AntiSearchPattern;
                t.Leave += patternHandler;
            }, 5);
            var compressFile = fb.AddField<YesNoButton>("Compress File", (c) =>
            {
                c.SelectedValue = pf.CompressFile;
                c.Click += (o, ea) =>
                {
                    if (!c.SelectedValue) fb.GetControl(aggregationFormatString).Text = null;
                };
            }, 7);
            aggregationFormatString = fb.AddField<FormatBox>("Aggregation Format String", (t) =>
            {
                patternHandler(null, null); //initialize textbox above
                t.Text = pf.AggregationFormatString;
                t.TextChanged += (o, ea) =>
                {
                    if (t.Text.Length > 0)
                    {
                        fb.GetControl(compressFile).SelectedValue = true;
                        fb.GetControl(renameFormatString).Text = null;
                    }
                };
            }, 9);
            renameFormatString = fb.AddField<FormatBox>("Rename Format String", (t) =>
            {
                t.Text = pf.RenameFormatString;
                t.TextChanged += (o, ea) =>
                {
                    if (t.Text.Length > 0)
                        fb.GetControl(aggregationFormatString).Text = null;
                };
            }, 10);
            fb.FormAccepted += (form) =>
            {
                pf.ProjectId = currentProject.ProjectId;
                pf.SourcePathTypeId = form.GetInput(sourceType).SelectedValue;
                pf.SourceRoot = form.GetInput(source).Text.UpdatePath();
                pf.DestinationPathTypeId = form.GetInput(destType).SelectedValue;
                pf.DestinationRoot = form.GetInput(dest).Text.UpdatePath();
                pf.SearchPattern = form.GetInput(searchPattern).Text;
                pf.AntiSearchPattern = form.GetInput(antiSearchPattern).Text;
                pf.CompressFile = form.GetInput(compressFile).SelectedValue;
                pf.AggregationFormatString = form.GetInput(aggregationFormatString).Text;
                pf.RenameFormatString = form.GetInput(renameFormatString).Text;
                return true;
            };
            fb.FormAlternate += (form) =>
            {
                if (Dialog.Warning.YesNo("Are you sure you want to delete this project file?", owner: form))
                {
                    ProjectFile.Delete(SelectedProjectFile);
                    LoadCurrentProject();
                    return true;
                }
                return false;
            };
            return fb;
        }
        #endregion

        private void ProjectsForm_Load(object sender, EventArgs e)
        {
            LoadCurrentProject();
        }
    }
}

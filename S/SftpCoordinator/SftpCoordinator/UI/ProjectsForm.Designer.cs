using Uheaa.Common.WinForms;
namespace SftpCoordinator
{
    partial class ProjectsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectsForm));
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.NewProjectButton = new System.Windows.Forms.Button();
            this.DeleteProjectButton = new System.Windows.Forms.Button();
            this.NewProjectFileButton = new System.Windows.Forms.Button();
            this.DeleteProjectFileButton = new System.Windows.Forms.Button();
            this.DestinationRoot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SourceRoot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SearchPattern = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectFilesGrid = new System.Windows.Forms.DataGridView();
            this.ProjectsList = new Uheaa.Common.WinForms.ColoredListBox();
            ((System.ComponentModel.ISupportInitialize)(this.ProjectFilesGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(211, 20);
            this.label3.TabIndex = 28;
            this.label3.Text = "Projects (double-click to edit)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(263, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 20);
            this.label1.TabIndex = 30;
            this.label1.Text = "Project Files (double-click to edit)";
            // 
            // NewProjectButton
            // 
            this.NewProjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NewProjectButton.Location = new System.Drawing.Point(166, 370);
            this.NewProjectButton.Name = "NewProjectButton";
            this.NewProjectButton.Size = new System.Drawing.Size(95, 23);
            this.NewProjectButton.TabIndex = 31;
            this.NewProjectButton.Text = "New Project (+)";
            this.NewProjectButton.UseVisualStyleBackColor = true;
            this.NewProjectButton.Click += new System.EventHandler(this.NewProjectButton_Click);
            // 
            // DeleteProjectButton
            // 
            this.DeleteProjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteProjectButton.Location = new System.Drawing.Point(16, 370);
            this.DeleteProjectButton.Name = "DeleteProjectButton";
            this.DeleteProjectButton.Size = new System.Drawing.Size(95, 23);
            this.DeleteProjectButton.TabIndex = 32;
            this.DeleteProjectButton.Text = "Delete Project";
            this.DeleteProjectButton.UseVisualStyleBackColor = true;
            this.DeleteProjectButton.Click += new System.EventHandler(this.DeleteProjectButton_Click);
            // 
            // NewProjectFileButton
            // 
            this.NewProjectFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NewProjectFileButton.Location = new System.Drawing.Point(701, 370);
            this.NewProjectFileButton.Name = "NewProjectFileButton";
            this.NewProjectFileButton.Size = new System.Drawing.Size(129, 23);
            this.NewProjectFileButton.TabIndex = 33;
            this.NewProjectFileButton.Text = "New Project File (+)";
            this.NewProjectFileButton.UseVisualStyleBackColor = true;
            this.NewProjectFileButton.Click += new System.EventHandler(this.NewProjectFileButton_Click);
            // 
            // DeleteProjectFileButton
            // 
            this.DeleteProjectFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteProjectFileButton.Location = new System.Drawing.Point(267, 370);
            this.DeleteProjectFileButton.Name = "DeleteProjectFileButton";
            this.DeleteProjectFileButton.Size = new System.Drawing.Size(105, 23);
            this.DeleteProjectFileButton.TabIndex = 34;
            this.DeleteProjectFileButton.Text = "Delete Project File";
            this.DeleteProjectFileButton.UseVisualStyleBackColor = true;
            this.DeleteProjectFileButton.Click += new System.EventHandler(this.DeleteProjectFileButton_Click);
            // 
            // DestinationRoot
            // 
            this.DestinationRoot.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DestinationRoot.DataPropertyName = "DestinationRoot";
            this.DestinationRoot.FillWeight = 40F;
            this.DestinationRoot.HeaderText = "Destination";
            this.DestinationRoot.Name = "DestinationRoot";
            this.DestinationRoot.ReadOnly = true;
            // 
            // SourceRoot
            // 
            this.SourceRoot.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SourceRoot.DataPropertyName = "SourceRoot";
            this.SourceRoot.FillWeight = 40F;
            this.SourceRoot.HeaderText = "Source";
            this.SourceRoot.Name = "SourceRoot";
            this.SourceRoot.ReadOnly = true;
            // 
            // SearchPattern
            // 
            this.SearchPattern.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SearchPattern.DataPropertyName = "SearchPattern";
            this.SearchPattern.FillWeight = 20F;
            this.SearchPattern.HeaderText = "Search";
            this.SearchPattern.Name = "SearchPattern";
            this.SearchPattern.ReadOnly = true;
            // 
            // ProjectFilesGrid
            // 
            this.ProjectFilesGrid.AllowUserToAddRows = false;
            this.ProjectFilesGrid.AllowUserToDeleteRows = false;
            this.ProjectFilesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectFilesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProjectFilesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SearchPattern,
            this.SourceRoot,
            this.DestinationRoot});
            this.ProjectFilesGrid.Location = new System.Drawing.Point(267, 40);
            this.ProjectFilesGrid.MultiSelect = false;
            this.ProjectFilesGrid.Name = "ProjectFilesGrid";
            this.ProjectFilesGrid.ReadOnly = true;
            this.ProjectFilesGrid.RowHeadersVisible = false;
            this.ProjectFilesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ProjectFilesGrid.Size = new System.Drawing.Size(563, 324);
            this.ProjectFilesGrid.TabIndex = 36;
            this.ProjectFilesGrid.DoubleClick += new System.EventHandler(this.ProjectFilesGrid_DoubleClick);
            // 
            // ProjectsList
            // 
            this.ProjectsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ProjectsList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ProjectsList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProjectsList.FormattingEnabled = true;
            this.ProjectsList.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.ProjectsList.ItemHeight = 20;
            this.ProjectsList.Location = new System.Drawing.Point(16, 40);
            this.ProjectsList.Name = "ProjectsList";
            this.ProjectsList.Size = new System.Drawing.Size(245, 324);
            this.ProjectsList.TabIndex = 0;
            this.ProjectsList.ResolveItemColor += new Uheaa.Common.WinForms.ColoredListBox.OnResolveItemColor(this.ProjectsList_ResolveItemColor);
            this.ProjectsList.SelectedIndexChanged += new System.EventHandler(this.ProjectsList_SelectedIndexChanged);
            this.ProjectsList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ProjectsList_MouseDoubleClick);
            // 
            // ProjectsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 405);
            this.Controls.Add(this.ProjectFilesGrid);
            this.Controls.Add(this.DeleteProjectFileButton);
            this.Controls.Add(this.NewProjectFileButton);
            this.Controls.Add(this.DeleteProjectButton);
            this.Controls.Add(this.NewProjectButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ProjectsList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProjectsForm";
            this.Text = "Manage Projects";
            this.Load += new System.EventHandler(this.ProjectsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ProjectFilesGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ColoredListBox ProjectsList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button NewProjectButton;
        private System.Windows.Forms.Button DeleteProjectButton;
        private System.Windows.Forms.Button NewProjectFileButton;
        private System.Windows.Forms.Button DeleteProjectFileButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn DestinationRoot;
        private System.Windows.Forms.DataGridViewTextBoxColumn SourceRoot;
        private System.Windows.Forms.DataGridViewTextBoxColumn SearchPattern;
        private System.Windows.Forms.DataGridView ProjectFilesGrid;
    }
}
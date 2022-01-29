namespace SftpCoordinator
{
    partial class SettingsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.ProjectFilesLabel = new System.Windows.Forms.Label();
            this.ProjectFilesTable = new Uheaa.Common.WinForms.EditTable();
            this.ProjectsTable = new Uheaa.Common.WinForms.EditTable();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Projects";
            // 
            // ProjectFilesLabel
            // 
            this.ProjectFilesLabel.AutoSize = true;
            this.ProjectFilesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProjectFilesLabel.Location = new System.Drawing.Point(363, 9);
            this.ProjectFilesLabel.Name = "ProjectFilesLabel";
            this.ProjectFilesLabel.Size = new System.Drawing.Size(95, 20);
            this.ProjectFilesLabel.TabIndex = 2;
            this.ProjectFilesLabel.Text = "Project Files";
            // 
            // ProjectFilesTable
            // 
            this.ProjectFilesTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectFilesTable.AutoSave = true;
            this.ProjectFilesTable.Database = Uheaa.Common.DataAccess.DataAccessHelper.Database.SftpCoordinator;
            this.ProjectFilesTable.FullRowSelect = false;
            this.ProjectFilesTable.Location = new System.Drawing.Point(367, 32);
            this.ProjectFilesTable.Mode = null;
            this.ProjectFilesTable.Name = "ProjectFilesTable";
            this.ProjectFilesTable.PrimaryKeyColumn = "ProjectFileId";
            this.ProjectFilesTable.Size = new System.Drawing.Size(664, 481);
            this.ProjectFilesTable.TabIndex = 3;
            this.ProjectFilesTable.TableName = "ProjectFiles";
            this.ProjectFilesTable.Text = "editTable1";
            // 
            // ProjectsTable
            // 
            this.ProjectsTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ProjectsTable.AutoSave = true;
            this.ProjectsTable.Database = Uheaa.Common.DataAccess.DataAccessHelper.Database.SftpCoordinator;
            this.ProjectsTable.FullRowSelect = true;
            this.ProjectsTable.Location = new System.Drawing.Point(16, 32);
            this.ProjectsTable.Mode = null;
            this.ProjectsTable.Name = "ProjectsTable";
            this.ProjectsTable.PrimaryKeyColumn = "ProjectId";
            this.ProjectsTable.Size = new System.Drawing.Size(345, 481);
            this.ProjectsTable.TabIndex = 0;
            this.ProjectsTable.TableName = "Projects";
            this.ProjectsTable.Text = "editTable1";
            this.ProjectsTable.RowSelected += new Uheaa.Common.WinForms.EditTable.OnRowSelected(this.ProjectsTable_RowSelected);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 525);
            this.Controls.Add(this.ProjectFilesTable);
            this.Controls.Add(this.ProjectFilesLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ProjectsTable);
            this.Name = "SettingsForm";
            this.Text = "FileManagement";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Uheaa.Common.WinForms.EditTable ProjectsTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ProjectFilesLabel;
        private Uheaa.Common.WinForms.EditTable ProjectFilesTable;


    }
}
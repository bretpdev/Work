namespace NSLDSCONSO
{
    partial class MainForm
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
            this.FilePathBox = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.ProcessButton = new System.Windows.Forms.Button();
            this.InputGroup = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.StatusBox = new System.Windows.Forms.TextBox();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.databaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CleanMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.fixUnmappedLoansToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileGenerationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allowBorrowersWithoutReleasedLoansToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allowPreviouslyReleasedNSLDSLabelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ProcessGroup = new System.Windows.Forms.GroupBox();
            this.GenerateButton = new System.Windows.Forms.RadioButton();
            this.ImportButton = new System.Windows.Forms.RadioButton();
            this.InputGroup.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.MainMenu.SuspendLayout();
            this.ProcessGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // FilePathBox
            // 
            this.FilePathBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilePathBox.Location = new System.Drawing.Point(6, 25);
            this.FilePathBox.Name = "FilePathBox";
            this.FilePathBox.Size = new System.Drawing.Size(620, 26);
            this.FilePathBox.TabIndex = 0;
            this.FilePathBox.TextChanged += new System.EventHandler(this.FilePathBox_TextChanged);
            // 
            // BrowseButton
            // 
            this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BrowseButton.Location = new System.Drawing.Point(4, 57);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(103, 34);
            this.BrowseButton.TabIndex = 2;
            this.BrowseButton.Text = "Browse...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // ProcessButton
            // 
            this.ProcessButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ProcessButton.Location = new System.Drawing.Point(471, 57);
            this.ProcessButton.Name = "ProcessButton";
            this.ProcessButton.Size = new System.Drawing.Size(155, 34);
            this.ProcessButton.TabIndex = 3;
            this.ProcessButton.Text = "Upload File";
            this.ProcessButton.UseVisualStyleBackColor = true;
            this.ProcessButton.Click += new System.EventHandler(this.UploadButton_Click);
            // 
            // InputGroup
            // 
            this.InputGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputGroup.Controls.Add(this.FilePathBox);
            this.InputGroup.Controls.Add(this.ProcessButton);
            this.InputGroup.Controls.Add(this.BrowseButton);
            this.InputGroup.Location = new System.Drawing.Point(12, 92);
            this.InputGroup.Name = "InputGroup";
            this.InputGroup.Size = new System.Drawing.Size(632, 100);
            this.InputGroup.TabIndex = 4;
            this.InputGroup.TabStop = false;
            this.InputGroup.Text = "Select an EA80 file";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.StatusBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 198);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(632, 296);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Status";
            // 
            // StatusBox
            // 
            this.StatusBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusBox.BackColor = System.Drawing.Color.White;
            this.StatusBox.Location = new System.Drawing.Point(6, 25);
            this.StatusBox.Multiline = true;
            this.StatusBox.Name = "StatusBox";
            this.StatusBox.ReadOnly = true;
            this.StatusBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.StatusBox.Size = new System.Drawing.Size(620, 265);
            this.StatusBox.TabIndex = 0;
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.databaseToolStripMenuItem,
            this.fileGenerationToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(656, 24);
            this.MainMenu.TabIndex = 6;
            // 
            // databaseToolStripMenuItem
            // 
            this.databaseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CleanMenuButton,
            this.fixUnmappedLoansToolStripMenuItem});
            this.databaseToolStripMenuItem.Name = "databaseToolStripMenuItem";
            this.databaseToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.databaseToolStripMenuItem.Text = "&Database";
            // 
            // CleanMenuButton
            // 
            this.CleanMenuButton.Name = "CleanMenuButton";
            this.CleanMenuButton.Size = new System.Drawing.Size(209, 22);
            this.CleanMenuButton.Text = "&Clean and Re-seed Tables";
            this.CleanMenuButton.Click += new System.EventHandler(this.CleanMenuButton_Click);
            // 
            // fixUnmappedLoansToolStripMenuItem
            // 
            this.fixUnmappedLoansToolStripMenuItem.Name = "fixUnmappedLoansToolStripMenuItem";
            this.fixUnmappedLoansToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.fixUnmappedLoansToolStripMenuItem.Text = "&Fix Unmapped Loans";
            this.fixUnmappedLoansToolStripMenuItem.Click += new System.EventHandler(this.fixUnmappedLoansToolStripMenuItem_Click);
            // 
            // fileGenerationToolStripMenuItem
            // 
            this.fileGenerationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allowBorrowersWithoutReleasedLoansToolStripMenuItem,
            this.allowPreviouslyReleasedNSLDSLabelsToolStripMenuItem});
            this.fileGenerationToolStripMenuItem.Name = "fileGenerationToolStripMenuItem";
            this.fileGenerationToolStripMenuItem.Size = new System.Drawing.Size(98, 20);
            this.fileGenerationToolStripMenuItem.Text = "&File Generation";
            // 
            // allowBorrowersWithoutReleasedLoansToolStripMenuItem
            // 
            this.allowBorrowersWithoutReleasedLoansToolStripMenuItem.CheckOnClick = true;
            this.allowBorrowersWithoutReleasedLoansToolStripMenuItem.Name = "allowBorrowersWithoutReleasedLoansToolStripMenuItem";
            this.allowBorrowersWithoutReleasedLoansToolStripMenuItem.Size = new System.Drawing.Size(287, 22);
            this.allowBorrowersWithoutReleasedLoansToolStripMenuItem.Text = "Allow Borrowers without Released &Loans";
            // 
            // allowPreviouslyReleasedNSLDSLabelsToolStripMenuItem
            // 
            this.allowPreviouslyReleasedNSLDSLabelsToolStripMenuItem.CheckOnClick = true;
            this.allowPreviouslyReleasedNSLDSLabelsToolStripMenuItem.Name = "allowPreviouslyReleasedNSLDSLabelsToolStripMenuItem";
            this.allowPreviouslyReleasedNSLDSLabelsToolStripMenuItem.Size = new System.Drawing.Size(287, 22);
            this.allowPreviouslyReleasedNSLDSLabelsToolStripMenuItem.Text = "Allow Previously Released &NSLDS Labels";
            // 
            // ProcessGroup
            // 
            this.ProcessGroup.Controls.Add(this.GenerateButton);
            this.ProcessGroup.Controls.Add(this.ImportButton);
            this.ProcessGroup.Location = new System.Drawing.Point(12, 27);
            this.ProcessGroup.Name = "ProcessGroup";
            this.ProcessGroup.Size = new System.Drawing.Size(632, 59);
            this.ProcessGroup.TabIndex = 7;
            this.ProcessGroup.TabStop = false;
            this.ProcessGroup.Text = "File";
            // 
            // GenerateButton
            // 
            this.GenerateButton.AutoSize = true;
            this.GenerateButton.Location = new System.Drawing.Point(239, 25);
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.Size = new System.Drawing.Size(206, 22);
            this.GenerateButton.TabIndex = 1;
            this.GenerateButton.Text = "Generate NSLDS BG File";
            this.GenerateButton.UseVisualStyleBackColor = true;
            // 
            // ImportButton
            // 
            this.ImportButton.AutoSize = true;
            this.ImportButton.Checked = true;
            this.ImportButton.Location = new System.Drawing.Point(19, 25);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(197, 22);
            this.ImportButton.TabIndex = 0;
            this.ImportButton.TabStop = true;
            this.ImportButton.Text = "Import/Upload EA80 File";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.CheckedChanged += new System.EventHandler(this.SelectionButtons_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 506);
            this.Controls.Add(this.ProcessGroup);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.InputGroup);
            this.Controls.Add(this.MainMenu);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.MainMenu;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "NSLDSCONSO";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.InputGroup.ResumeLayout(false);
            this.InputGroup.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ProcessGroup.ResumeLayout(false);
            this.ProcessGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox FilePathBox;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Button ProcessButton;
        private System.Windows.Forms.GroupBox InputGroup;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox StatusBox;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem databaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CleanMenuButton;
        private System.Windows.Forms.GroupBox ProcessGroup;
        private System.Windows.Forms.RadioButton GenerateButton;
        private System.Windows.Forms.RadioButton ImportButton;
        private System.Windows.Forms.ToolStripMenuItem fileGenerationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allowBorrowersWithoutReleasedLoansToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allowPreviouslyReleasedNSLDSLabelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fixUnmappedLoansToolStripMenuItem;
    }
}


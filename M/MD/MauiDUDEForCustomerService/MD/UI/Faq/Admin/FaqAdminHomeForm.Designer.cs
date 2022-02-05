namespace MD
{
    partial class FaqAdminHomeForm
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
            this.components = new System.ComponentModel.Container();
            this.DetectAnswerChangedTimer = new System.Windows.Forms.Timer(this.components);
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.ExistingQuestionsTab = new System.Windows.Forms.TabPage();
            this.ExistingQuestionsTabControl = new MD.ExistingQuestionsTab();
            this.GroupsPortfoliosTab = new System.Windows.Forms.TabPage();
            this.GroupPortfolioTable = new System.Windows.Forms.TableLayoutPanel();
            this.PendingQuestionsTab = new System.Windows.Forms.TabPage();
            this.PendingQuestionsControl = new MD.PendingQuestions();
            this.MainTabControl.SuspendLayout();
            this.ExistingQuestionsTab.SuspendLayout();
            this.GroupsPortfoliosTab.SuspendLayout();
            this.PendingQuestionsTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // DetectAnswerChangedTimer
            // 
            this.DetectAnswerChangedTimer.Enabled = true;
            this.DetectAnswerChangedTimer.Interval = 500;
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.ExistingQuestionsTab);
            this.MainTabControl.Controls.Add(this.GroupsPortfoliosTab);
            this.MainTabControl.Controls.Add(this.PendingQuestionsTab);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainTabControl.Location = new System.Drawing.Point(0, 0);
            this.MainTabControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(1069, 733);
            this.MainTabControl.TabIndex = 0;
            // 
            // ExistingQuestionsTab
            // 
            this.ExistingQuestionsTab.BackColor = System.Drawing.SystemColors.Control;
            this.ExistingQuestionsTab.Controls.Add(this.ExistingQuestionsTabControl);
            this.ExistingQuestionsTab.Location = new System.Drawing.Point(4, 25);
            this.ExistingQuestionsTab.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ExistingQuestionsTab.Name = "ExistingQuestionsTab";
            this.ExistingQuestionsTab.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ExistingQuestionsTab.Size = new System.Drawing.Size(1061, 704);
            this.ExistingQuestionsTab.TabIndex = 0;
            this.ExistingQuestionsTab.Text = "Existing Questions";
            // 
            // ExistingQuestionsTabControl
            // 
            this.ExistingQuestionsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExistingQuestionsTabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExistingQuestionsTabControl.Location = new System.Drawing.Point(3, 4);
            this.ExistingQuestionsTabControl.Name = "ExistingQuestionsTabControl";
            this.ExistingQuestionsTabControl.Size = new System.Drawing.Size(1055, 696);
            this.ExistingQuestionsTabControl.TabIndex = 0;
            // 
            // GroupsPortfoliosTab
            // 
            this.GroupsPortfoliosTab.BackColor = System.Drawing.SystemColors.Control;
            this.GroupsPortfoliosTab.Controls.Add(this.GroupPortfolioTable);
            this.GroupsPortfoliosTab.Location = new System.Drawing.Point(4, 25);
            this.GroupsPortfoliosTab.Name = "GroupsPortfoliosTab";
            this.GroupsPortfoliosTab.Size = new System.Drawing.Size(1061, 704);
            this.GroupsPortfoliosTab.TabIndex = 1;
            this.GroupsPortfoliosTab.Text = "Groups && Portfolios";
            // 
            // GroupPortfolioTable
            // 
            this.GroupPortfolioTable.ColumnCount = 1;
            this.GroupPortfolioTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.GroupPortfolioTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupPortfolioTable.Location = new System.Drawing.Point(0, 0);
            this.GroupPortfolioTable.Name = "GroupPortfolioTable";
            this.GroupPortfolioTable.RowCount = 2;
            this.GroupPortfolioTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GroupPortfolioTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GroupPortfolioTable.Size = new System.Drawing.Size(1061, 704);
            this.GroupPortfolioTable.TabIndex = 0;
            // 
            // PendingQuestionsTab
            // 
            this.PendingQuestionsTab.Controls.Add(this.PendingQuestionsControl);
            this.PendingQuestionsTab.Location = new System.Drawing.Point(4, 25);
            this.PendingQuestionsTab.Name = "PendingQuestionsTab";
            this.PendingQuestionsTab.Size = new System.Drawing.Size(1061, 704);
            this.PendingQuestionsTab.TabIndex = 2;
            this.PendingQuestionsTab.Text = "Pending Questions";
            this.PendingQuestionsTab.UseVisualStyleBackColor = true;
            // 
            // PendingQuestionsControl
            // 
            this.PendingQuestionsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PendingQuestionsControl.Location = new System.Drawing.Point(0, 0);
            this.PendingQuestionsControl.Margin = new System.Windows.Forms.Padding(4);
            this.PendingQuestionsControl.Name = "PendingQuestionsControl";
            this.PendingQuestionsControl.Size = new System.Drawing.Size(1061, 704);
            this.PendingQuestionsControl.TabIndex = 0;
            this.PendingQuestionsControl.DataSynced += new MD.PendingQuestions.OnDataSynced(this.PendingQuestionsControl_DataSynced);
            this.PendingQuestionsControl.QuestionSelectedForApproval += new MD.PendingQuestions.OnQuestionSelectedForApproval(this.PendingQuestionsControl_QuestionSelectedForApproval);
            // 
            // FaqAdminHomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 733);
            this.Controls.Add(this.MainTabControl);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FaqAdminHomeForm";
            this.Text = "Manage FAQ";
            this.MainTabControl.ResumeLayout(false);
            this.ExistingQuestionsTab.ResumeLayout(false);
            this.GroupsPortfoliosTab.ResumeLayout(false);
            this.PendingQuestionsTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer DetectAnswerChangedTimer;
        private System.Windows.Forms.TabPage ExistingQuestionsTab;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage GroupsPortfoliosTab;
        private ExistingQuestionsTab ExistingQuestionsTabControl;
        private System.Windows.Forms.TabPage PendingQuestionsTab;
        private PendingQuestions PendingQuestionsControl;
        private System.Windows.Forms.TableLayoutPanel GroupPortfolioTable;
    }
}
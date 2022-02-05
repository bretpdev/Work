namespace CentralizedPrintingProcess
{
    partial class EmailHandler
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.EmailsGrid = new System.Windows.Forms.DataGridView();
            this.RecipientsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TitleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BodyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SendButton = new System.Windows.Forms.Button();
            this.AutoSendTimer = new System.Windows.Forms.Timer(this.components);
            this.AutoEmailsLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.EmailsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(521, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Emails Generated";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EmailsGrid
            // 
            this.EmailsGrid.AllowUserToAddRows = false;
            this.EmailsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.EmailsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RecipientsColumn,
            this.TitleColumn,
            this.BodyColumn,
            this.StatusColumn});
            this.EmailsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EmailsGrid.Location = new System.Drawing.Point(0, 29);
            this.EmailsGrid.Name = "EmailsGrid";
            this.EmailsGrid.ReadOnly = true;
            this.EmailsGrid.Size = new System.Drawing.Size(521, 434);
            this.EmailsGrid.TabIndex = 1;
            // 
            // RecipientsColumn
            // 
            this.RecipientsColumn.DataPropertyName = "Recipients";
            this.RecipientsColumn.HeaderText = "Recipients";
            this.RecipientsColumn.Name = "RecipientsColumn";
            this.RecipientsColumn.ReadOnly = true;
            // 
            // TitleColumn
            // 
            this.TitleColumn.DataPropertyName = "Title";
            this.TitleColumn.HeaderText = "Title";
            this.TitleColumn.Name = "TitleColumn";
            this.TitleColumn.ReadOnly = true;
            // 
            // BodyColumn
            // 
            this.BodyColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.BodyColumn.DataPropertyName = "Body";
            this.BodyColumn.HeaderText = "Body";
            this.BodyColumn.Name = "BodyColumn";
            this.BodyColumn.ReadOnly = true;
            // 
            // StatusColumn
            // 
            this.StatusColumn.DataPropertyName = "Status";
            this.StatusColumn.HeaderText = "Status";
            this.StatusColumn.Name = "StatusColumn";
            this.StatusColumn.ReadOnly = true;
            // 
            // SendButton
            // 
            this.SendButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SendButton.Location = new System.Drawing.Point(0, 431);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(521, 32);
            this.SendButton.TabIndex = 2;
            this.SendButton.Text = "Send {0} Pending Emails Now";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // AutoSendTimer
            // 
            this.AutoSendTimer.Enabled = true;
            this.AutoSendTimer.Tick += new System.EventHandler(this.AutoSendTimer_Tick);
            // 
            // AutoEmailsLabel
            // 
            this.AutoEmailsLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.AutoEmailsLabel.Location = new System.Drawing.Point(0, 402);
            this.AutoEmailsLabel.Name = "AutoEmailsLabel";
            this.AutoEmailsLabel.Size = new System.Drawing.Size(521, 29);
            this.AutoEmailsLabel.TabIndex = 3;
            this.AutoEmailsLabel.Text = "All pending emails will be automatically sent in {0}.";
            this.AutoEmailsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EmailHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.AutoEmailsLabel);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.EmailsGrid);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "EmailHandler";
            this.Size = new System.Drawing.Size(521, 463);
            ((System.ComponentModel.ISupportInitialize)(this.EmailsGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView EmailsGrid;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecipientsColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TitleColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BodyColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusColumn;
        private System.Windows.Forms.Timer AutoSendTimer;
        private System.Windows.Forms.Label AutoEmailsLabel;
    }
}

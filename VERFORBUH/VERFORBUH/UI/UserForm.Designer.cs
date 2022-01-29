namespace VERFORBUH
{
    partial class UserForm
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
            this.NumberOfMonthsLabel = new System.Windows.Forms.Label();
            this.NumberOfMonths = new System.Windows.Forms.ComboBox();
            this.ForbearanceStartDateLabel = new System.Windows.Forms.Label();
            this.ForbearanceStartDate = new System.Windows.Forms.DateTimePicker();
            this.Reason = new System.Windows.Forms.TextBox();
            this.ReasonLabel = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.Acknowledgement = new System.Windows.Forms.CheckBox();
            this.CoBwrAck = new System.Windows.Forms.CheckBox();
            this.Interest_Notice_CB = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // NumberOfMonthsLabel
            // 
            this.NumberOfMonthsLabel.AutoSize = true;
            this.NumberOfMonthsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NumberOfMonthsLabel.Location = new System.Drawing.Point(9, 9);
            this.NumberOfMonthsLabel.Name = "NumberOfMonthsLabel";
            this.NumberOfMonthsLabel.Size = new System.Drawing.Size(238, 18);
            this.NumberOfMonthsLabel.TabIndex = 0;
            this.NumberOfMonthsLabel.Text = "Number of Months Requested:";
            // 
            // NumberOfMonths
            // 
            this.NumberOfMonths.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NumberOfMonths.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.NumberOfMonths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.NumberOfMonths.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NumberOfMonths.FormattingEnabled = true;
            this.NumberOfMonths.Location = new System.Drawing.Point(253, 8);
            this.NumberOfMonths.Name = "NumberOfMonths";
            this.NumberOfMonths.Size = new System.Drawing.Size(67, 24);
            this.NumberOfMonths.TabIndex = 1;
            this.NumberOfMonths.SelectionChangeCommitted += new System.EventHandler(this.NumberOfMonths_SelectionChangeCommitted);
            // 
            // ForbearanceStartDateLabel
            // 
            this.ForbearanceStartDateLabel.AutoSize = true;
            this.ForbearanceStartDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForbearanceStartDateLabel.Location = new System.Drawing.Point(9, 44);
            this.ForbearanceStartDateLabel.Name = "ForbearanceStartDateLabel";
            this.ForbearanceStartDateLabel.Size = new System.Drawing.Size(189, 18);
            this.ForbearanceStartDateLabel.TabIndex = 2;
            this.ForbearanceStartDateLabel.Text = "Forbearance Start Date:";
            // 
            // ForbearanceStartDate
            // 
            this.ForbearanceStartDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ForbearanceStartDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForbearanceStartDate.Location = new System.Drawing.Point(204, 44);
            this.ForbearanceStartDate.Name = "ForbearanceStartDate";
            this.ForbearanceStartDate.Size = new System.Drawing.Size(239, 23);
            this.ForbearanceStartDate.TabIndex = 3;
            this.ForbearanceStartDate.ValueChanged += new System.EventHandler(this.ForbearanceStartDate_ValueChanged);
            // 
            // Reason
            // 
            this.Reason.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Reason.Enabled = false;
            this.Reason.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Reason.Location = new System.Drawing.Point(12, 102);
            this.Reason.MaxLength = 100;
            this.Reason.Multiline = true;
            this.Reason.Name = "Reason";
            this.Reason.Size = new System.Drawing.Size(483, 82);
            this.Reason.TabIndex = 4;
            // 
            // ReasonLabel
            // 
            this.ReasonLabel.AutoSize = true;
            this.ReasonLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReasonLabel.Location = new System.Drawing.Point(9, 77);
            this.ReasonLabel.Name = "ReasonLabel";
            this.ReasonLabel.Size = new System.Drawing.Size(202, 18);
            this.ReasonLabel.TabIndex = 5;
            this.ReasonLabel.Text = "Reason For Forbearance:";
            // 
            // OK
            // 
            this.OK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OK.Location = new System.Drawing.Point(301, 190);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(87, 31);
            this.OK.TabIndex = 6;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel.Location = new System.Drawing.Point(394, 190);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(87, 31);
            this.Cancel.TabIndex = 7;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // Acknowledgement
            // 
            this.Acknowledgement.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Acknowledgement.Location = new System.Drawing.Point(12, 186);
            this.Acknowledgement.Name = "Acknowledgement";
            this.Acknowledgement.Size = new System.Drawing.Size(263, 41);
            this.Acknowledgement.TabIndex = 8;
            this.Acknowledgement.Text = "Borrower has acknowledged willingness to repay this debt.";
            this.Acknowledgement.UseVisualStyleBackColor = true;
            // 
            // CoBwrAck
            // 
            this.CoBwrAck.Enabled = false;
            this.CoBwrAck.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CoBwrAck.Location = new System.Drawing.Point(12, 233);
            this.CoBwrAck.Name = "CoBwrAck";
            this.CoBwrAck.Size = new System.Drawing.Size(293, 41);
            this.CoBwrAck.TabIndex = 9;
            this.CoBwrAck.Text = "Co - Borrower has acknowledged willingness to repay this debt.";
            this.CoBwrAck.UseVisualStyleBackColor = true;
            // 
            // Interest_Notice_CB
            // 
            this.Interest_Notice_CB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.Interest_Notice_CB.Location = new System.Drawing.Point(12, 281);
            this.Interest_Notice_CB.Name = "Interest_Notice_CB";
            this.Interest_Notice_CB.Size = new System.Drawing.Size(350, 41);
            this.Interest_Notice_CB.TabIndex = 10;
            this.Interest_Notice_CB.Text = "Borrower has been notified of interest accrual and potential capitalization.";
            this.Interest_Notice_CB.UseVisualStyleBackColor = true;
            // 
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(507, 335);
            this.Controls.Add(this.Interest_Notice_CB);
            this.Controls.Add(this.CoBwrAck);
            this.Controls.Add(this.Acknowledgement);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.ReasonLabel);
            this.Controls.Add(this.Reason);
            this.Controls.Add(this.ForbearanceStartDate);
            this.Controls.Add(this.ForbearanceStartDateLabel);
            this.Controls.Add(this.NumberOfMonths);
            this.Controls.Add(this.NumberOfMonthsLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(525, 375);
            this.Name = "UserForm";
            this.ShowIcon = false;
            this.Text = "Verbal Forbearance";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NumberOfMonthsLabel;
        private System.Windows.Forms.ComboBox NumberOfMonths;
        private System.Windows.Forms.Label ForbearanceStartDateLabel;
        private System.Windows.Forms.DateTimePicker ForbearanceStartDate;
        private System.Windows.Forms.TextBox Reason;
        private System.Windows.Forms.Label ReasonLabel;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.CheckBox Acknowledgement;
        private System.Windows.Forms.CheckBox CoBwrAck;
        private System.Windows.Forms.CheckBox Interest_Notice_CB;
    }
}
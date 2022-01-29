namespace TIMETRAKUP
{
    partial class UpdateTime
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.UserName = new System.Windows.Forms.Label();
            this.UnstoppedOnly = new System.Windows.Forms.CheckBox();
            this.UserTimes = new System.Windows.Forms.DataGridView();
            this.TicketID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.StartDate = new System.Windows.Forms.DateTimePicker();
            this.StartTime = new System.Windows.Forms.DateTimePicker();
            this.EndTime = new System.Windows.Forms.DateTimePicker();
            this.EndDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.UnlockStart = new System.Windows.Forms.Button();
            this.Update = new System.Windows.Forms.Button();
            this.ElapsedTime = new System.Windows.Forms.TextBox();
            this.From = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.To = new System.Windows.Forms.DateTimePicker();
            this.VersionNumber = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.UserTimes)).BeginInit();
            this.SuspendLayout();
            // 
            // UserName
            // 
            this.UserName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.UserName.AutoSize = true;
            this.UserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.UserName.Location = new System.Drawing.Point(19, 12);
            this.UserName.Name = "UserName";
            this.UserName.Size = new System.Drawing.Size(0, 20);
            this.UserName.TabIndex = 1;
            // 
            // UnstoppedOnly
            // 
            this.UnstoppedOnly.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.UnstoppedOnly.AutoSize = true;
            this.UnstoppedOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UnstoppedOnly.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.UnstoppedOnly.Location = new System.Drawing.Point(262, 13);
            this.UnstoppedOnly.Name = "UnstoppedOnly";
            this.UnstoppedOnly.Size = new System.Drawing.Size(189, 24);
            this.UnstoppedOnly.TabIndex = 2;
            this.UnstoppedOnly.Text = "Show Unstopped Time";
            this.UnstoppedOnly.UseVisualStyleBackColor = true;
            this.UnstoppedOnly.CheckedChanged += new System.EventHandler(this.UnstoppedOnly_CheckedChanged);
            // 
            // UserTimes
            // 
            this.UserTimes.AllowUserToAddRows = false;
            this.UserTimes.AllowUserToDeleteRows = false;
            this.UserTimes.AllowUserToResizeRows = false;
            this.UserTimes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UserTimes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.UserTimes.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.UserTimes.BackgroundColor = System.Drawing.Color.DimGray;
            this.UserTimes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.UserTimes.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.UserTimes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.UserTimes.DefaultCellStyle = dataGridViewCellStyle1;
            this.UserTimes.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.UserTimes.Location = new System.Drawing.Point(15, 50);
            this.UserTimes.MultiSelect = false;
            this.UserTimes.Name = "UserTimes";
            this.UserTimes.ReadOnly = true;
            this.UserTimes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.UserTimes.Size = new System.Drawing.Size(679, 472);
            this.UserTimes.TabIndex = 3;
            this.UserTimes.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.UserTime_CellDoubleClick);
            this.UserTimes.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.UserTime_ColumnHeaderMouseClick);
            // 
            // TicketID
            // 
            this.TicketID.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.TicketID.Enabled = false;
            this.TicketID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TicketID.Location = new System.Drawing.Point(115, 541);
            this.TicketID.Name = "TicketID";
            this.TicketID.ReadOnly = true;
            this.TicketID.Size = new System.Drawing.Size(127, 26);
            this.TicketID.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(29, 544);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "Ticket ID:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(18, 575);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Start Date:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Location = new System.Drawing.Point(273, 544);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "Elapsed Time:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label5.Location = new System.Drawing.Point(296, 575);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "Start Time:";
            // 
            // StartDate
            // 
            this.StartDate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.StartDate.Enabled = false;
            this.StartDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.StartDate.Location = new System.Drawing.Point(115, 572);
            this.StartDate.Name = "StartDate";
            this.StartDate.Size = new System.Drawing.Size(127, 26);
            this.StartDate.TabIndex = 12;
            this.StartDate.ValueChanged += new System.EventHandler(this.StartDate_ValueChanged);
            // 
            // StartTime
            // 
            this.StartTime.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.StartTime.Enabled = false;
            this.StartTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.StartTime.Location = new System.Drawing.Point(388, 571);
            this.StartTime.Name = "StartTime";
            this.StartTime.ShowUpDown = true;
            this.StartTime.Size = new System.Drawing.Size(131, 26);
            this.StartTime.TabIndex = 13;
            this.StartTime.ValueChanged += new System.EventHandler(this.UpdateElapsedTime);
            // 
            // EndTime
            // 
            this.EndTime.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.EndTime.Enabled = false;
            this.EndTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EndTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.EndTime.Location = new System.Drawing.Point(388, 601);
            this.EndTime.Name = "EndTime";
            this.EndTime.ShowUpDown = true;
            this.EndTime.Size = new System.Drawing.Size(131, 26);
            this.EndTime.TabIndex = 17;
            this.EndTime.ValueChanged += new System.EventHandler(this.UpdateElapsedTime);
            // 
            // EndDate
            // 
            this.EndDate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.EndDate.Enabled = false;
            this.EndDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.EndDate.Location = new System.Drawing.Point(115, 602);
            this.EndDate.Name = "EndDate";
            this.EndDate.Size = new System.Drawing.Size(127, 26);
            this.EndDate.TabIndex = 16;
            this.EndDate.ValueChanged += new System.EventHandler(this.UpdateElapsedTime);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label6.Location = new System.Drawing.Point(302, 605);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 20);
            this.label6.TabIndex = 15;
            this.label6.Text = "End Time:";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label7.Location = new System.Drawing.Point(24, 605);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 20);
            this.label7.TabIndex = 14;
            this.label7.Text = "End Date:";
            // 
            // UnlockStart
            // 
            this.UnlockStart.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.UnlockStart.BackColor = System.Drawing.SystemColors.Control;
            this.UnlockStart.Enabled = false;
            this.UnlockStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UnlockStart.ForeColor = System.Drawing.Color.Black;
            this.UnlockStart.Location = new System.Drawing.Point(559, 544);
            this.UnlockStart.Name = "UnlockStart";
            this.UnlockStart.Size = new System.Drawing.Size(135, 35);
            this.UnlockStart.TabIndex = 18;
            this.UnlockStart.Text = "Unlock Start";
            this.UnlockStart.UseVisualStyleBackColor = true;
            this.UnlockStart.Click += new System.EventHandler(this.UnlockStart_Click);
            // 
            // Update
            // 
            this.Update.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Update.BackColor = System.Drawing.SystemColors.Window;
            this.Update.Enabled = false;
            this.Update.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Update.ForeColor = System.Drawing.Color.Black;
            this.Update.Location = new System.Drawing.Point(559, 590);
            this.Update.Name = "Update";
            this.Update.Size = new System.Drawing.Size(135, 35);
            this.Update.TabIndex = 21;
            this.Update.Text = "Update";
            this.Update.UseVisualStyleBackColor = true;
            this.Update.Click += new System.EventHandler(this.Update_Click);
            // 
            // ElapsedTime
            // 
            this.ElapsedTime.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ElapsedTime.Enabled = false;
            this.ElapsedTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ElapsedTime.Location = new System.Drawing.Point(388, 541);
            this.ElapsedTime.Name = "ElapsedTime";
            this.ElapsedTime.ReadOnly = true;
            this.ElapsedTime.Size = new System.Drawing.Size(131, 26);
            this.ElapsedTime.TabIndex = 6;
            // 
            // From
            // 
            this.From.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.From.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.From.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.From.Location = new System.Drawing.Point(464, 10);
            this.From.MaxDate = new System.DateTime(2013, 9, 4, 0, 0, 0, 0);
            this.From.Name = "From";
            this.From.Size = new System.Drawing.Size(104, 26);
            this.From.TabIndex = 22;
            this.From.Value = new System.DateTime(2012, 1, 1, 0, 0, 0, 0);
            this.From.ValueChanged += new System.EventHandler(this.From_ValueChanged);
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label8.Location = new System.Drawing.Point(573, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 20);
            this.label8.TabIndex = 23;
            this.label8.Text = "-";
            // 
            // To
            // 
            this.To.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.To.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.To.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.To.Location = new System.Drawing.Point(590, 10);
            this.To.Name = "To";
            this.To.Size = new System.Drawing.Size(104, 26);
            this.To.TabIndex = 24;
            this.To.Value = new System.DateTime(2013, 9, 4, 7, 51, 48, 0);
            this.To.ValueChanged += new System.EventHandler(this.To_ValueChanged);
            // 
            // VersionNumber
            // 
            this.VersionNumber.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.VersionNumber.AutoSize = true;
            this.VersionNumber.Location = new System.Drawing.Point(646, 631);
            this.VersionNumber.Name = "VersionNumber";
            this.VersionNumber.Size = new System.Drawing.Size(42, 13);
            this.VersionNumber.TabIndex = 25;
            this.VersionNumber.Text = "Version";
            // 
            // UpdateTime
            // 
            this.AcceptButton = this.Update;
            this.ClientSize = new System.Drawing.Size(721, 653);
            this.Controls.Add(this.VersionNumber);
            this.Controls.Add(this.To);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.From);
            this.Controls.Add(this.Update);
            this.Controls.Add(this.UnlockStart);
            this.Controls.Add(this.EndTime);
            this.Controls.Add(this.EndDate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.StartTime);
            this.Controls.Add(this.StartDate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ElapsedTime);
            this.Controls.Add(this.TicketID);
            this.Controls.Add(this.UserTimes);
            this.Controls.Add(this.UnstoppedOnly);
            this.Controls.Add(this.UserName);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MinimumSize = new System.Drawing.Size(737, 691);
            this.Name = "UpdateTime";
            this.Text = "Update Time Tracking";
            ((System.ComponentModel.ISupportInitialize)(this.UserTimes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label UserName;
        private System.Windows.Forms.CheckBox UnstoppedOnly;
        private System.Windows.Forms.DataGridView UserTimes;
        private System.Windows.Forms.TextBox TicketID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker StartDate;
        private System.Windows.Forms.DateTimePicker StartTime;
        private System.Windows.Forms.DateTimePicker EndTime;
        private System.Windows.Forms.DateTimePicker EndDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button UnlockStart;
        private System.Windows.Forms.Button Update;
        private System.Windows.Forms.TextBox ElapsedTime;
        private System.Windows.Forms.DateTimePicker From;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker To;
        private System.Windows.Forms.Label VersionNumber;
    }
}
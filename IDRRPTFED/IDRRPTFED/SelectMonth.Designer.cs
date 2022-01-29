namespace IDRRPTFED
{
    partial class SelectMonth
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
            this.startDateCurrent = new System.Windows.Forms.Label();
            this.endDateCurrent = new System.Windows.Forms.Label();
            this.ToggleSelectionButton = new System.Windows.Forms.Button();
            this.RunButton = new System.Windows.Forms.Button();
            this.PreviousDates = new System.Windows.Forms.DataGridView();
            this.RunDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Start = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.End = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RunBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RunTypeTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.EndDateTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.StartDateTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PreviousDates)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // startDateCurrent
            // 
            this.startDateCurrent.AutoSize = true;
            this.startDateCurrent.Location = new System.Drawing.Point(290, 63);
            this.startDateCurrent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.startDateCurrent.Name = "startDateCurrent";
            this.startDateCurrent.Size = new System.Drawing.Size(0, 20);
            this.startDateCurrent.TabIndex = 3;
            // 
            // endDateCurrent
            // 
            this.endDateCurrent.AutoSize = true;
            this.endDateCurrent.Location = new System.Drawing.Point(290, 149);
            this.endDateCurrent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.endDateCurrent.Name = "endDateCurrent";
            this.endDateCurrent.Size = new System.Drawing.Size(13, 20);
            this.endDateCurrent.TabIndex = 4;
            this.endDateCurrent.Text = " ";
            // 
            // ToggleSelectionButton
            // 
            this.ToggleSelectionButton.Location = new System.Drawing.Point(18, 149);
            this.ToggleSelectionButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ToggleSelectionButton.Name = "ToggleSelectionButton";
            this.ToggleSelectionButton.Size = new System.Drawing.Size(217, 38);
            this.ToggleSelectionButton.TabIndex = 10;
            this.ToggleSelectionButton.Text = "Select Previous Run";
            this.ToggleSelectionButton.UseVisualStyleBackColor = true;
            this.ToggleSelectionButton.Click += new System.EventHandler(this.ToggleSelectionButton_Click);
            // 
            // RunButton
            // 
            this.RunButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RunButton.Location = new System.Drawing.Point(422, 149);
            this.RunButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(217, 38);
            this.RunButton.TabIndex = 9;
            this.RunButton.Text = "Run This Selection";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // PreviousDates
            // 
            this.PreviousDates.AllowUserToAddRows = false;
            this.PreviousDates.AllowUserToDeleteRows = false;
            this.PreviousDates.AllowUserToResizeRows = false;
            this.PreviousDates.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.PreviousDates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PreviousDates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RunDate,
            this.Start,
            this.End,
            this.RunBy});
            this.PreviousDates.Location = new System.Drawing.Point(18, 198);
            this.PreviousDates.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PreviousDates.Name = "PreviousDates";
            this.PreviousDates.ReadOnly = true;
            this.PreviousDates.RowHeadersVisible = false;
            this.PreviousDates.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.PreviousDates.Size = new System.Drawing.Size(618, 303);
            this.PreviousDates.TabIndex = 10;
            this.PreviousDates.SelectionChanged += new System.EventHandler(this.PreviousDates_SelectionChanged);
            // 
            // RunDate
            // 
            this.RunDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.RunDate.DataPropertyName = "RunDate";
            this.RunDate.HeaderText = "Run Date";
            this.RunDate.Name = "RunDate";
            this.RunDate.ReadOnly = true;
            // 
            // Start
            // 
            this.Start.DataPropertyName = "StartDate";
            this.Start.HeaderText = "Start";
            this.Start.Name = "Start";
            this.Start.ReadOnly = true;
            this.Start.Width = 69;
            // 
            // End
            // 
            this.End.DataPropertyName = "EndDate";
            this.End.HeaderText = "End";
            this.End.Name = "End";
            this.End.ReadOnly = true;
            this.End.Width = 63;
            // 
            // RunBy
            // 
            this.RunBy.DataPropertyName = "RunBy";
            this.RunBy.HeaderText = "RunBy";
            this.RunBy.Name = "RunBy";
            this.RunBy.ReadOnly = true;
            this.RunBy.Width = 82;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RunTypeTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.EndDateTextBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.StartDateTextBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(21, 1);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(618, 143);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // RunTypeTextBox
            // 
            this.RunTypeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RunTypeTextBox.Location = new System.Drawing.Point(8, 37);
            this.RunTypeTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RunTypeTextBox.Name = "RunTypeTextBox";
            this.RunTypeTextBox.ReadOnly = true;
            this.RunTypeTextBox.Size = new System.Drawing.Size(264, 29);
            this.RunTypeTextBox.TabIndex = 14;
            this.RunTypeTextBox.Text = "Current Run";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 16);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "Run Type";
            // 
            // EndDateTextBox
            // 
            this.EndDateTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EndDateTextBox.Location = new System.Drawing.Point(344, 93);
            this.EndDateTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.EndDateTextBox.Name = "EndDateTextBox";
            this.EndDateTextBox.ReadOnly = true;
            this.EndDateTextBox.Size = new System.Drawing.Size(264, 26);
            this.EndDateTextBox.TabIndex = 12;
            this.EndDateTextBox.Text = "01/01/3000";
            this.EndDateTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(340, 68);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "End Date";
            // 
            // StartDateTextBox
            // 
            this.StartDateTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartDateTextBox.Location = new System.Drawing.Point(338, 37);
            this.StartDateTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.StartDateTextBox.Name = "StartDateTextBox";
            this.StartDateTextBox.ReadOnly = true;
            this.StartDateTextBox.Size = new System.Drawing.Size(264, 26);
            this.StartDateTextBox.TabIndex = 10;
            this.StartDateTextBox.Text = "1/1/2000";
            this.StartDateTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.StartDateTextBox.TextChanged += new System.EventHandler(this.StartDateTextBox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(334, 16);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Start Date";
            // 
            // SelectMonth
            // 
            this.AcceptButton = this.RunButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 515);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.PreviousDates);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.ToggleSelectionButton);
            this.Controls.Add(this.startDateCurrent);
            this.Controls.Add(this.endDateCurrent);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "SelectMonth";
            this.ShowIcon = false;
            this.Text = "NSLDS IDR Report Generator";
            ((System.ComponentModel.ISupportInitialize)(this.PreviousDates)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label startDateCurrent;
        private System.Windows.Forms.Label endDateCurrent;
        private System.Windows.Forms.Button ToggleSelectionButton;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.DataGridView PreviousDates;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox EndDateTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox StartDateTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox RunTypeTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn RunDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Start;
        private System.Windows.Forms.DataGridViewTextBoxColumn End;
        private System.Windows.Forms.DataGridViewTextBoxColumn RunBy;
    }
}
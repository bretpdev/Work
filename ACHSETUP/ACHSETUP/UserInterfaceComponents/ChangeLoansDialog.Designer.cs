namespace ACHSETUP
{
	partial class ChangeLoansDialog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvAvailableLoans = new System.Windows.Forms.DataGridView();
            this.SeqAvailable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.programDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.balanceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.firstDisbDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvSelectedLoans = new System.Windows.Forms.DataGridView();
            this.SeqCurrent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.programDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.balanceDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.firstDisbDateDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnAddAll = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailableLoans)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedLoans)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvAvailableLoans);
            this.groupBox1.Location = new System.Drawing.Point(20, 20);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(441, 446);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Possible Loans to Add";
            // 
            // dgvAvailableLoans
            // 
            this.dgvAvailableLoans.AllowUserToAddRows = false;
            this.dgvAvailableLoans.AllowUserToDeleteRows = false;
            this.dgvAvailableLoans.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAvailableLoans.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SeqAvailable,
            this.programDataGridViewTextBoxColumn,
            this.balanceDataGridViewTextBoxColumn,
            this.firstDisbDateDataGridViewTextBoxColumn});
            this.dgvAvailableLoans.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAvailableLoans.Location = new System.Drawing.Point(4, 24);
            this.dgvAvailableLoans.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgvAvailableLoans.Name = "dgvAvailableLoans";
            this.dgvAvailableLoans.ReadOnly = true;
            this.dgvAvailableLoans.RowHeadersVisible = false;
            this.dgvAvailableLoans.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAvailableLoans.Size = new System.Drawing.Size(433, 417);
            this.dgvAvailableLoans.TabIndex = 0;
            // 
            // SeqAvailable
            // 
            this.SeqAvailable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.SeqAvailable.DataPropertyName = "Sequence";
            this.SeqAvailable.HeaderText = "Seq";
            this.SeqAvailable.Name = "SeqAvailable";
            this.SeqAvailable.ReadOnly = true;
            this.SeqAvailable.Width = 63;
            // 
            // programDataGridViewTextBoxColumn
            // 
            this.programDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.programDataGridViewTextBoxColumn.DataPropertyName = "Program";
            this.programDataGridViewTextBoxColumn.HeaderText = "Ln Prgm";
            this.programDataGridViewTextBoxColumn.Name = "programDataGridViewTextBoxColumn";
            this.programDataGridViewTextBoxColumn.ReadOnly = true;
            this.programDataGridViewTextBoxColumn.Width = 93;
            // 
            // balanceDataGridViewTextBoxColumn
            // 
            this.balanceDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.balanceDataGridViewTextBoxColumn.DataPropertyName = "Balance";
            dataGridViewCellStyle5.Format = "$##,##0.00";
            this.balanceDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.balanceDataGridViewTextBoxColumn.HeaderText = "Curr Bal";
            this.balanceDataGridViewTextBoxColumn.Name = "balanceDataGridViewTextBoxColumn";
            this.balanceDataGridViewTextBoxColumn.ReadOnly = true;
            this.balanceDataGridViewTextBoxColumn.Width = 91;
            // 
            // firstDisbDateDataGridViewTextBoxColumn
            // 
            this.firstDisbDateDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.firstDisbDateDataGridViewTextBoxColumn.DataPropertyName = "FirstDisbDate";
            dataGridViewCellStyle6.Format = "MM/dd/yyyy";
            this.firstDisbDateDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.firstDisbDateDataGridViewTextBoxColumn.HeaderText = "1st Disb Dt";
            this.firstDisbDateDataGridViewTextBoxColumn.Name = "firstDisbDateDataGridViewTextBoxColumn";
            this.firstDisbDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.firstDisbDateDataGridViewTextBoxColumn.Width = 113;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvSelectedLoans);
            this.groupBox2.Location = new System.Drawing.Point(628, 20);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(441, 446);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Current Loans/Loans to be Added";
            // 
            // dgvSelectedLoans
            // 
            this.dgvSelectedLoans.AllowUserToAddRows = false;
            this.dgvSelectedLoans.AllowUserToDeleteRows = false;
            this.dgvSelectedLoans.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSelectedLoans.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SeqCurrent,
            this.programDataGridViewTextBoxColumn1,
            this.balanceDataGridViewTextBoxColumn1,
            this.firstDisbDateDataGridViewTextBoxColumn1});
            this.dgvSelectedLoans.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSelectedLoans.Location = new System.Drawing.Point(4, 24);
            this.dgvSelectedLoans.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgvSelectedLoans.Name = "dgvSelectedLoans";
            this.dgvSelectedLoans.ReadOnly = true;
            this.dgvSelectedLoans.RowHeadersVisible = false;
            this.dgvSelectedLoans.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelectedLoans.Size = new System.Drawing.Size(433, 417);
            this.dgvSelectedLoans.TabIndex = 0;
            // 
            // SeqCurrent
            // 
            this.SeqCurrent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SeqCurrent.DataPropertyName = "Sequence";
            this.SeqCurrent.HeaderText = "Seq";
            this.SeqCurrent.Name = "SeqCurrent";
            this.SeqCurrent.ReadOnly = true;
            this.SeqCurrent.Width = 63;
            // 
            // programDataGridViewTextBoxColumn1
            // 
            this.programDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.programDataGridViewTextBoxColumn1.DataPropertyName = "Program";
            this.programDataGridViewTextBoxColumn1.HeaderText = "Ln Prgm";
            this.programDataGridViewTextBoxColumn1.Name = "programDataGridViewTextBoxColumn1";
            this.programDataGridViewTextBoxColumn1.ReadOnly = true;
            this.programDataGridViewTextBoxColumn1.Width = 93;
            // 
            // balanceDataGridViewTextBoxColumn1
            // 
            this.balanceDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.balanceDataGridViewTextBoxColumn1.DataPropertyName = "Balance";
            dataGridViewCellStyle7.Format = "$##,##0.00";
            this.balanceDataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle7;
            this.balanceDataGridViewTextBoxColumn1.HeaderText = "Curr Bal";
            this.balanceDataGridViewTextBoxColumn1.Name = "balanceDataGridViewTextBoxColumn1";
            this.balanceDataGridViewTextBoxColumn1.ReadOnly = true;
            this.balanceDataGridViewTextBoxColumn1.Width = 91;
            // 
            // firstDisbDateDataGridViewTextBoxColumn1
            // 
            this.firstDisbDateDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.firstDisbDateDataGridViewTextBoxColumn1.DataPropertyName = "FirstDisbDate";
            dataGridViewCellStyle8.Format = "MM/dd/yyyy";
            this.firstDisbDateDataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle8;
            this.firstDisbDateDataGridViewTextBoxColumn1.HeaderText = "1st Disb Dt";
            this.firstDisbDateDataGridViewTextBoxColumn1.Name = "firstDisbDateDataGridViewTextBoxColumn1";
            this.firstDisbDateDataGridViewTextBoxColumn1.ReadOnly = true;
            this.firstDisbDateDataGridViewTextBoxColumn1.Width = 113;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(470, 109);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(150, 46);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add ->";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.Add_Click);
            // 
            // btnAddAll
            // 
            this.btnAddAll.Location = new System.Drawing.Point(470, 165);
            this.btnAddAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAddAll.Name = "btnAddAll";
            this.btnAddAll.Size = new System.Drawing.Size(150, 46);
            this.btnAddAll.TabIndex = 3;
            this.btnAddAll.Text = "Add All ->->";
            this.btnAddAll.UseVisualStyleBackColor = true;
            this.btnAddAll.Click += new System.EventHandler(this.AddAll_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(470, 220);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(150, 46);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "<- Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Location = new System.Drawing.Point(470, 275);
            this.btnRemoveAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(150, 46);
            this.btnRemoveAll.TabIndex = 5;
            this.btnRemoveAll.Text = "<-<- Remove All";
            this.btnRemoveAll.UseVisualStyleBackColor = true;
            this.btnRemoveAll.Click += new System.EventHandler(this.RemoveAll_Click);
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(378, 523);
            this.OK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(150, 46);
            this.OK.TabIndex = 6;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(560, 523);
            this.Cancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(150, 46);
            this.Cancel.TabIndex = 7;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Yellow;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(20, 477);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1050, 28);
            this.label1.TabIndex = 8;
            this.label1.Text = "All loans listed in boxes have active repayment schedules.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ChangeLoansDialog
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(1086, 585);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.btnRemoveAll);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAddAll);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1102, 625);
            this.MinimumSize = new System.Drawing.Size(1102, 624);
            this.Name = "ChangeLoansDialog";
            this.Text = "ChangeLoansDialog";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailableLoans)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedLoans)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DataGridView dgvAvailableLoans;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.DataGridView dgvSelectedLoans;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnAddAll;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnRemoveAll;
		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn SeqAvailable;
        private System.Windows.Forms.DataGridViewTextBoxColumn programDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn balanceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn firstDisbDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SeqCurrent;
        private System.Windows.Forms.DataGridViewTextBoxColumn programDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn balanceDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn firstDisbDateDataGridViewTextBoxColumn1;
	}
}
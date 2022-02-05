namespace CORNTSTQUE
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
			this.radAdd = new System.Windows.Forms.RadioButton();
			this.radRemove = new System.Windows.Forms.RadioButton();
			this.txtUserId = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbUserType = new System.Windows.Forms.ComboBox();
			this.btnAddUser = new System.Windows.Forms.Button();
			this.btnRemoveUser = new System.Windows.Forms.Button();
			this.txtQueue = new System.Windows.Forms.TextBox();
			this.txtSubQueue = new System.Windows.Forms.TextBox();
			this.btnAddQueue = new System.Windows.Forms.Button();
			this.btnRemoveQueue = new System.Windows.Forms.Button();
			this.btnProcess = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.dgvUsers = new System.Windows.Forms.DataGridView();
			this.UserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvQueues = new System.Windows.Forms.DataGridView();
			this.Queue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SubQueue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvQueues)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// radAdd
			// 
			this.radAdd.AutoSize = true;
			this.radAdd.Location = new System.Drawing.Point(251, 13);
			this.radAdd.Name = "radAdd";
			this.radAdd.Size = new System.Drawing.Size(44, 17);
			this.radAdd.TabIndex = 0;
			this.radAdd.TabStop = true;
			this.radAdd.Text = "Add";
			this.radAdd.UseVisualStyleBackColor = true;
			this.radAdd.CheckedChanged += new System.EventHandler(this.radAdd_CheckedChanged);
			// 
			// radRemove
			// 
			this.radRemove.AutoSize = true;
			this.radRemove.Location = new System.Drawing.Point(336, 13);
			this.radRemove.Name = "radRemove";
			this.radRemove.Size = new System.Drawing.Size(65, 17);
			this.radRemove.TabIndex = 1;
			this.radRemove.TabStop = true;
			this.radRemove.Text = "Remove";
			this.radRemove.UseVisualStyleBackColor = true;
			this.radRemove.CheckedChanged += new System.EventHandler(this.radRemove_CheckedChanged);
			// 
			// txtUserId
			// 
			this.txtUserId.Location = new System.Drawing.Point(68, 13);
			this.txtUserId.MaxLength = 7;
			this.txtUserId.Name = "txtUserId";
			this.txtUserId.Size = new System.Drawing.Size(83, 20);
			this.txtUserId.TabIndex = 2;
			this.txtUserId.Text = "UT00";
			this.txtUserId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserId_KeyDown);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(43, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "User ID";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "User Type";
			// 
			// cmbUserType
			// 
			this.cmbUserType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
			this.cmbUserType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cmbUserType.FormattingEnabled = true;
			this.cmbUserType.Items.AddRange(new object[] {
            "Worker",
            "Specialist"});
			this.cmbUserType.Location = new System.Drawing.Point(68, 39);
			this.cmbUserType.Name = "cmbUserType";
			this.cmbUserType.Size = new System.Drawing.Size(100, 21);
			this.cmbUserType.TabIndex = 5;
			this.cmbUserType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbUserType_KeyDown);
			// 
			// btnAddUser
			// 
			this.btnAddUser.Location = new System.Drawing.Point(68, 66);
			this.btnAddUser.Name = "btnAddUser";
			this.btnAddUser.Size = new System.Drawing.Size(100, 23);
			this.btnAddUser.TabIndex = 6;
			this.btnAddUser.Text = "Add User";
			this.btnAddUser.UseVisualStyleBackColor = true;
			this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
			// 
			// btnRemoveUser
			// 
			this.btnRemoveUser.Location = new System.Drawing.Point(68, 140);
			this.btnRemoveUser.Name = "btnRemoveUser";
			this.btnRemoveUser.Size = new System.Drawing.Size(100, 23);
			this.btnRemoveUser.TabIndex = 8;
			this.btnRemoveUser.Text = "Remove User";
			this.btnRemoveUser.UseVisualStyleBackColor = true;
			this.btnRemoveUser.Click += new System.EventHandler(this.btnRemoveUser_Click);
			// 
			// txtQueue
			// 
			this.txtQueue.Location = new System.Drawing.Point(73, 13);
			this.txtQueue.MaxLength = 2;
			this.txtQueue.Name = "txtQueue";
			this.txtQueue.Size = new System.Drawing.Size(32, 20);
			this.txtQueue.TabIndex = 9;
			this.txtQueue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtQueue_KeyDown);
			// 
			// txtSubQueue
			// 
			this.txtSubQueue.Location = new System.Drawing.Point(73, 39);
			this.txtSubQueue.MaxLength = 2;
			this.txtSubQueue.Name = "txtSubQueue";
			this.txtSubQueue.Size = new System.Drawing.Size(32, 20);
			this.txtSubQueue.TabIndex = 10;
			this.txtSubQueue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSubQueue_KeyDown);
			// 
			// btnAddQueue
			// 
			this.btnAddQueue.Location = new System.Drawing.Point(30, 66);
			this.btnAddQueue.Name = "btnAddQueue";
			this.btnAddQueue.Size = new System.Drawing.Size(75, 23);
			this.btnAddQueue.TabIndex = 11;
			this.btnAddQueue.Text = "Add";
			this.btnAddQueue.UseVisualStyleBackColor = true;
			this.btnAddQueue.Click += new System.EventHandler(this.btnAddQueue_Click);
			// 
			// btnRemoveQueue
			// 
			this.btnRemoveQueue.Location = new System.Drawing.Point(30, 140);
			this.btnRemoveQueue.Name = "btnRemoveQueue";
			this.btnRemoveQueue.Size = new System.Drawing.Size(75, 23);
			this.btnRemoveQueue.TabIndex = 12;
			this.btnRemoveQueue.Text = "Remove";
			this.btnRemoveQueue.UseVisualStyleBackColor = true;
			this.btnRemoveQueue.Click += new System.EventHandler(this.btnRemoveQueue_Click);
			// 
			// btnProcess
			// 
			this.btnProcess.Location = new System.Drawing.Point(235, 222);
			this.btnProcess.Name = "btnProcess";
			this.btnProcess.Size = new System.Drawing.Size(75, 23);
			this.btnProcess.TabIndex = 14;
			this.btnProcess.Text = "Process";
			this.btnProcess.UseVisualStyleBackColor = true;
			this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(342, 222);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 15;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(39, 13);
			this.label4.TabIndex = 17;
			this.label4.Text = "Queue";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 42);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(61, 13);
			this.label5.TabIndex = 18;
			this.label5.Text = "Sub-Queue";
			// 
			// dgvUsers
			// 
			this.dgvUsers.AllowUserToAddRows = false;
			this.dgvUsers.AllowUserToDeleteRows = false;
			this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UserId,
            this.Type});
			this.dgvUsers.Location = new System.Drawing.Point(183, 13);
			this.dgvUsers.MultiSelect = false;
			this.dgvUsers.Name = "dgvUsers";
			this.dgvUsers.ReadOnly = true;
			this.dgvUsers.RowHeadersVisible = false;
			this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvUsers.Size = new System.Drawing.Size(173, 150);
			this.dgvUsers.TabIndex = 19;
			// 
			// UserId
			// 
			this.UserId.HeaderText = "User ID";
			this.UserId.Name = "UserId";
			this.UserId.ReadOnly = true;
			this.UserId.Width = 70;
			// 
			// Type
			// 
			this.Type.HeaderText = "Type";
			this.Type.Name = "Type";
			this.Type.ReadOnly = true;
			// 
			// dgvQueues
			// 
			this.dgvQueues.AllowUserToAddRows = false;
			this.dgvQueues.AllowUserToDeleteRows = false;
			this.dgvQueues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvQueues.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Queue,
            this.SubQueue});
			this.dgvQueues.Location = new System.Drawing.Point(120, 13);
			this.dgvQueues.MultiSelect = false;
			this.dgvQueues.Name = "dgvQueues";
			this.dgvQueues.ReadOnly = true;
			this.dgvQueues.RowHeadersVisible = false;
			this.dgvQueues.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvQueues.Size = new System.Drawing.Size(133, 150);
			this.dgvQueues.TabIndex = 20;
			// 
			// Queue
			// 
			this.Queue.HeaderText = "Queue";
			this.Queue.Name = "Queue";
			this.Queue.ReadOnly = true;
			this.Queue.Width = 50;
			// 
			// SubQueue
			// 
			this.SubQueue.HeaderText = "Sub-Queue";
			this.SubQueue.Name = "SubQueue";
			this.SubQueue.ReadOnly = true;
			this.SubQueue.Width = 80;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.txtUserId);
			this.groupBox1.Controls.Add(this.dgvUsers);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.cmbUserType);
			this.groupBox1.Controls.Add(this.btnAddUser);
			this.groupBox1.Controls.Add(this.btnRemoveUser);
			this.groupBox1.Location = new System.Drawing.Point(12, 36);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(364, 171);
			this.groupBox1.TabIndex = 21;
			this.groupBox1.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.txtQueue);
			this.groupBox2.Controls.Add(this.dgvQueues);
			this.groupBox2.Controls.Add(this.txtSubQueue);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.btnAddQueue);
			this.groupBox2.Controls.Add(this.btnRemoveQueue);
			this.groupBox2.Location = new System.Drawing.Point(382, 36);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(262, 171);
			this.groupBox2.TabIndex = 22;
			this.groupBox2.TabStop = false;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(653, 257);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnProcess);
			this.Controls.Add(this.radRemove);
			this.Controls.Add(this.radAdd);
			this.Name = "MainForm";
			this.Text = "CornerStone Test Queue Access";
			((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvQueues)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RadioButton radAdd;
		private System.Windows.Forms.RadioButton radRemove;
		private System.Windows.Forms.TextBox txtUserId;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbUserType;
		private System.Windows.Forms.Button btnAddUser;
		private System.Windows.Forms.Button btnRemoveUser;
		private System.Windows.Forms.TextBox txtQueue;
		private System.Windows.Forms.TextBox txtSubQueue;
		private System.Windows.Forms.Button btnAddQueue;
		private System.Windows.Forms.Button btnRemoveQueue;
		private System.Windows.Forms.Button btnProcess;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.DataGridView dgvUsers;
		private System.Windows.Forms.DataGridView dgvQueues;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.DataGridViewTextBoxColumn UserId;
		private System.Windows.Forms.DataGridViewTextBoxColumn Type;
		private System.Windows.Forms.DataGridViewTextBoxColumn Queue;
		private System.Windows.Forms.DataGridViewTextBoxColumn SubQueue;
	}
}
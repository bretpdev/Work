namespace DatabasePermissions
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
			this.cmbDatabase = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cmbEntityType = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtFilter = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbActiveDirectoryGroup = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.btnSearch = new System.Windows.Forms.Button();
			this.btnApply = new System.Windows.Forms.Button();
			this.pnlEntities = new System.Windows.Forms.FlowLayoutPanel();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.chkInsert = new System.Windows.Forms.CheckBox();
			this.chkUpdate = new System.Windows.Forms.CheckBox();
			this.chkDelete = new System.Windows.Forms.CheckBox();
			this.chkSelect = new System.Windows.Forms.CheckBox();
			this.chkExecute = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// cmbDatabase
			// 
			this.cmbDatabase.FormattingEnabled = true;
			this.cmbDatabase.Location = new System.Drawing.Point(12, 28);
			this.cmbDatabase.Name = "cmbDatabase";
			this.cmbDatabase.Size = new System.Drawing.Size(182, 21);
			this.cmbDatabase.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Database";
			// 
			// cmbEntityType
			// 
			this.cmbEntityType.FormattingEnabled = true;
			this.cmbEntityType.Location = new System.Drawing.Point(200, 28);
			this.cmbEntityType.Name = "cmbEntityType";
			this.cmbEntityType.Size = new System.Drawing.Size(121, 21);
			this.cmbEntityType.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(197, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Entity type";
			// 
			// txtFilter
			// 
			this.txtFilter.Location = new System.Drawing.Point(327, 28);
			this.txtFilter.Name = "txtFilter";
			this.txtFilter.Size = new System.Drawing.Size(109, 20);
			this.txtFilter.TabIndex = 4;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(324, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(107, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Filter by name (regex)";
			// 
			// cmbActiveDirectoryGroup
			// 
			this.cmbActiveDirectoryGroup.FormattingEnabled = true;
			this.cmbActiveDirectoryGroup.Location = new System.Drawing.Point(12, 77);
			this.cmbActiveDirectoryGroup.Name = "cmbActiveDirectoryGroup";
			this.cmbActiveDirectoryGroup.Size = new System.Drawing.Size(309, 21);
			this.cmbActiveDirectoryGroup.TabIndex = 7;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(9, 59);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(112, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "Active Directory group";
			// 
			// btnSearch
			// 
			this.btnSearch.Location = new System.Drawing.Point(361, 75);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(75, 23);
			this.btnSearch.TabIndex = 9;
			this.btnSearch.Text = "Search";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// btnApply
			// 
			this.btnApply.Location = new System.Drawing.Point(16, 132);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(75, 23);
			this.btnApply.TabIndex = 10;
			this.btnApply.Text = "Apply";
			this.btnApply.UseVisualStyleBackColor = true;
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// pnlEntities
			// 
			this.pnlEntities.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.pnlEntities.AutoScroll = true;
			this.pnlEntities.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlEntities.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.pnlEntities.Location = new System.Drawing.Point(12, 165);
			this.pnlEntities.Name = "pnlEntities";
			this.pnlEntities.Size = new System.Drawing.Size(567, 340);
			this.pnlEntities.TabIndex = 11;
			this.pnlEntities.WrapContents = false;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(353, 121);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(33, 13);
			this.label6.TabIndex = 13;
			this.label6.Text = "Insert";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(389, 121);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(42, 13);
			this.label7.TabIndex = 14;
			this.label7.Text = "Update";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(432, 121);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(38, 13);
			this.label8.TabIndex = 15;
			this.label8.Text = "Delete";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(472, 121);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(37, 13);
			this.label9.TabIndex = 16;
			this.label9.Text = "Select";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(510, 121);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(46, 13);
			this.label10.TabIndex = 17;
			this.label10.Text = "Execute";
			// 
			// chkInsert
			// 
			this.chkInsert.AutoSize = true;
			this.chkInsert.Location = new System.Drawing.Point(365, 145);
			this.chkInsert.Name = "chkInsert";
			this.chkInsert.Size = new System.Drawing.Size(15, 14);
			this.chkInsert.TabIndex = 18;
			this.chkInsert.UseVisualStyleBackColor = true;
			this.chkInsert.CheckStateChanged += new System.EventHandler(this.chkInsert_CheckStateChanged);
			// 
			// chkUpdate
			// 
			this.chkUpdate.AutoSize = true;
			this.chkUpdate.Location = new System.Drawing.Point(405, 145);
			this.chkUpdate.Name = "chkUpdate";
			this.chkUpdate.Size = new System.Drawing.Size(15, 14);
			this.chkUpdate.TabIndex = 19;
			this.chkUpdate.UseVisualStyleBackColor = true;
			this.chkUpdate.CheckStateChanged += new System.EventHandler(this.chkUpdate_CheckStateChanged);
			// 
			// chkDelete
			// 
			this.chkDelete.AutoSize = true;
			this.chkDelete.Location = new System.Drawing.Point(445, 145);
			this.chkDelete.Name = "chkDelete";
			this.chkDelete.Size = new System.Drawing.Size(15, 14);
			this.chkDelete.TabIndex = 20;
			this.chkDelete.UseVisualStyleBackColor = true;
			this.chkDelete.CheckStateChanged += new System.EventHandler(this.chkDelete_CheckStateChanged);
			// 
			// chkSelect
			// 
			this.chkSelect.AutoSize = true;
			this.chkSelect.Location = new System.Drawing.Point(485, 145);
			this.chkSelect.Name = "chkSelect";
			this.chkSelect.Size = new System.Drawing.Size(15, 14);
			this.chkSelect.TabIndex = 21;
			this.chkSelect.UseVisualStyleBackColor = true;
			this.chkSelect.CheckStateChanged += new System.EventHandler(this.chkSelect_CheckStateChanged);
			// 
			// chkExecute
			// 
			this.chkExecute.AutoSize = true;
			this.chkExecute.Location = new System.Drawing.Point(525, 145);
			this.chkExecute.Name = "chkExecute";
			this.chkExecute.Size = new System.Drawing.Size(15, 14);
			this.chkExecute.TabIndex = 22;
			this.chkExecute.UseVisualStyleBackColor = true;
			this.chkExecute.CheckStateChanged += new System.EventHandler(this.chkExecute_CheckStateChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(591, 517);
			this.Controls.Add(this.chkExecute);
			this.Controls.Add(this.chkSelect);
			this.Controls.Add(this.chkDelete);
			this.Controls.Add(this.chkUpdate);
			this.Controls.Add(this.chkInsert);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.pnlEntities);
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.cmbActiveDirectoryGroup);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtFilter);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbEntityType);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmbDatabase);
			this.Name = "MainForm";
			this.Text = "Database Permissions";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cmbDatabase;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbEntityType;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtFilter;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cmbActiveDirectoryGroup;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.FlowLayoutPanel pnlEntities;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.CheckBox chkInsert;
		private System.Windows.Forms.CheckBox chkUpdate;
		private System.Windows.Forms.CheckBox chkDelete;
		private System.Windows.Forms.CheckBox chkSelect;
		private System.Windows.Forms.CheckBox chkExecute;
	}
}


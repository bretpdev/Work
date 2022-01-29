namespace ACDCAccess
{
    partial class AddAndRemoveAccessUserBased
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
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.pnlPossibleKeysToRemove = new System.Windows.Forms.FlowLayoutPanel();
			this.pnlPossibleKeysToAdd = new System.Windows.Forms.FlowLayoutPanel();
			this.label5 = new System.Windows.Forms.Label();
			this.keyBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.userKeyInfoForAddingAccess1 = new ACDCAccess.UserKeyInfoForAddingAccess();
			this.userKeyInfoForRemovingAccess1 = new ACDCAccess.UserKeyInfoForRemovingAccess();
			this.btnAddAndRemoveAccess = new System.Windows.Forms.Button();
			this.btnSearch = new System.Windows.Forms.Button();
			this.cmbApplication = new Q.ComboBoxWithAutoCompleteExtended();
			this.cmbUsers = new Q.ComboBoxWithAutoCompleteExtended();
			this.cmbBusinessUnit = new Q.ComboBoxWithAutoCompleteExtended();
			((System.ComponentModel.ISupportInitialize)(this.keyBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "System";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 33);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(29, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "User";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 57);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(71, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "Business Unit";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 84);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(64, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "Keys to Add";
			// 
			// pnlPossibleKeysToRemove
			// 
			this.pnlPossibleKeysToRemove.AutoScroll = true;
			this.pnlPossibleKeysToRemove.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pnlPossibleKeysToRemove.Location = new System.Drawing.Point(154, 334);
			this.pnlPossibleKeysToRemove.Name = "pnlPossibleKeysToRemove";
			this.pnlPossibleKeysToRemove.Size = new System.Drawing.Size(867, 206);
			this.pnlPossibleKeysToRemove.TabIndex = 13;
			this.pnlPossibleKeysToRemove.TabStop = true;
			// 
			// pnlPossibleKeysToAdd
			// 
			this.pnlPossibleKeysToAdd.AutoScroll = true;
			this.pnlPossibleKeysToAdd.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pnlPossibleKeysToAdd.Location = new System.Drawing.Point(155, 102);
			this.pnlPossibleKeysToAdd.Name = "pnlPossibleKeysToAdd";
			this.pnlPossibleKeysToAdd.Size = new System.Drawing.Size(867, 206);
			this.pnlPossibleKeysToAdd.TabIndex = 11;
			this.pnlPossibleKeysToAdd.TabStop = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 316);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(85, 13);
			this.label5.TabIndex = 12;
			this.label5.Text = "Keys to Remove";
			// 
			// keyBindingSource
			// 
			this.keyBindingSource.DataSource = typeof(ACDCAccess.Key);
			// 
			// userKeyInfoForAddingAccess1
			// 
			this.userKeyInfoForAddingAccess1.AutoSize = true;
			this.userKeyInfoForAddingAccess1.KeyData = null;
			this.userKeyInfoForAddingAccess1.Location = new System.Drawing.Point(159, 80);
			this.userKeyInfoForAddingAccess1.MaximumSize = new System.Drawing.Size(840, 0);
			this.userKeyInfoForAddingAccess1.MinimumSize = new System.Drawing.Size(840, 20);
			this.userKeyInfoForAddingAccess1.Name = "userKeyInfoForAddingAccess1";
			this.userKeyInfoForAddingAccess1.Size = new System.Drawing.Size(840, 20);
			this.userKeyInfoForAddingAccess1.TabIndex = 13;
			this.userKeyInfoForAddingAccess1.TabStop = false;
			// 
			// userKeyInfoForRemovingAccess1
			// 
			this.userKeyInfoForRemovingAccess1.AutoSize = true;
			this.userKeyInfoForRemovingAccess1.KeyData = null;
			this.userKeyInfoForRemovingAccess1.Location = new System.Drawing.Point(159, 312);
			this.userKeyInfoForRemovingAccess1.MaximumSize = new System.Drawing.Size(840, 20);
			this.userKeyInfoForRemovingAccess1.MinimumSize = new System.Drawing.Size(840, 20);
			this.userKeyInfoForRemovingAccess1.Name = "userKeyInfoForRemovingAccess1";
			this.userKeyInfoForRemovingAccess1.Size = new System.Drawing.Size(840, 20);
			this.userKeyInfoForRemovingAccess1.TabIndex = 14;
			this.userKeyInfoForRemovingAccess1.TabStop = false;
			// 
			// btnAddAndRemoveAccess
			// 
			this.btnAddAndRemoveAccess.Location = new System.Drawing.Point(436, 552);
			this.btnAddAndRemoveAccess.Name = "btnAddAndRemoveAccess";
			this.btnAddAndRemoveAccess.Size = new System.Drawing.Size(153, 23);
			this.btnAddAndRemoveAccess.TabIndex = 15;
			this.btnAddAndRemoveAccess.Text = "Add and Remove Access";
			this.btnAddAndRemoveAccess.UseVisualStyleBackColor = true;
			this.btnAddAndRemoveAccess.Click += new System.EventHandler(this.btnAddAndRemoveAccess_Click);
			// 
			// btnSearch
			// 
			this.btnSearch.Location = new System.Drawing.Point(490, 28);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(75, 23);
			this.btnSearch.TabIndex = 7;
			this.btnSearch.Text = "Search";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// cmbApplication
			// 
			this.cmbApplication.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cmbApplication.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cmbApplication.FormattingEnabled = true;
			this.cmbApplication.InvalidAutoCompleteEntry = Q.ComboBoxWithAutoCompleteExtended.InvalidEntryBehavior.ErrorMessage;
			this.cmbApplication.Location = new System.Drawing.Point(154, 6);
			this.cmbApplication.Name = "cmbApplication";
			this.cmbApplication.Size = new System.Drawing.Size(330, 21);
			this.cmbApplication.TabIndex = 1;
			// 
			// cmbUsers
			// 
			this.cmbUsers.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cmbUsers.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cmbUsers.FormattingEnabled = true;
			this.cmbUsers.InvalidAutoCompleteEntry = Q.ComboBoxWithAutoCompleteExtended.InvalidEntryBehavior.ErrorMessage;
			this.cmbUsers.Location = new System.Drawing.Point(154, 30);
			this.cmbUsers.Name = "cmbUsers";
			this.cmbUsers.Size = new System.Drawing.Size(330, 21);
			this.cmbUsers.TabIndex = 3;
			// 
			// cmbBusinessUnit
			// 
			this.cmbBusinessUnit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cmbBusinessUnit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cmbBusinessUnit.FormattingEnabled = true;
			this.cmbBusinessUnit.InvalidAutoCompleteEntry = Q.ComboBoxWithAutoCompleteExtended.InvalidEntryBehavior.ErrorMessage;
			this.cmbBusinessUnit.Location = new System.Drawing.Point(154, 54);
			this.cmbBusinessUnit.Name = "cmbBusinessUnit";
			this.cmbBusinessUnit.Size = new System.Drawing.Size(330, 21);
			this.cmbBusinessUnit.TabIndex = 5;
			// 
			// AddAndRemoveAccessUserBased
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.cmbBusinessUnit);
			this.Controls.Add(this.cmbUsers);
			this.Controls.Add(this.cmbApplication);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.btnAddAndRemoveAccess);
			this.Controls.Add(this.userKeyInfoForRemovingAccess1);
			this.Controls.Add(this.userKeyInfoForAddingAccess1);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.pnlPossibleKeysToAdd);
			this.Controls.Add(this.pnlPossibleKeysToRemove);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "AddAndRemoveAccessUserBased";
			((System.ComponentModel.ISupportInitialize)(this.keyBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FlowLayoutPanel pnlPossibleKeysToRemove;
        private System.Windows.Forms.FlowLayoutPanel pnlPossibleKeysToAdd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.BindingSource keyBindingSource;
        private UserKeyInfoForAddingAccess userKeyInfoForAddingAccess1;
        private UserKeyInfoForRemovingAccess userKeyInfoForRemovingAccess1;
        private System.Windows.Forms.Button btnAddAndRemoveAccess;
        private System.Windows.Forms.Button btnSearch;
        private Q.ComboBoxWithAutoCompleteExtended cmbApplication;
        private Q.ComboBoxWithAutoCompleteExtended cmbUsers;
		private Q.ComboBoxWithAutoCompleteExtended cmbBusinessUnit;
    }
}

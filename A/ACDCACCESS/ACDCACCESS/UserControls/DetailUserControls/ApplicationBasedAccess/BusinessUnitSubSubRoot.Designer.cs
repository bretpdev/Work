namespace ACDCAccess
{
    partial class BusinessUnitSubSubRoot
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
			this.pnlUsers = new System.Windows.Forms.FlowLayoutPanel();
			this.lblBusinessUnit = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnExpand = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.btnAddAccess = new System.Windows.Forms.Button();
			this.cmbUsers = new Q.ComboBoxWithAutoCompleteExtended();
			this.SuspendLayout();
			// 
			// pnlUsers
			// 
			this.pnlUsers.AutoSize = true;
			this.pnlUsers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pnlUsers.Location = new System.Drawing.Point(116, 48);
			this.pnlUsers.MaximumSize = new System.Drawing.Size(600, 4);
			this.pnlUsers.MinimumSize = new System.Drawing.Size(600, 4);
			this.pnlUsers.Name = "pnlUsers";
			this.pnlUsers.Size = new System.Drawing.Size(600, 4);
			this.pnlUsers.TabIndex = 8;
			this.pnlUsers.TabStop = true;
			// 
			// lblBusinessUnit
			// 
			this.lblBusinessUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblBusinessUnit.Location = new System.Drawing.Point(116, 0);
			this.lblBusinessUnit.MaximumSize = new System.Drawing.Size(600, 20);
			this.lblBusinessUnit.MinimumSize = new System.Drawing.Size(600, 20);
			this.lblBusinessUnit.Name = "lblBusinessUnit";
			this.lblBusinessUnit.Size = new System.Drawing.Size(600, 20);
			this.lblBusinessUnit.TabIndex = 10;
			this.lblBusinessUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(28, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82, 17);
			this.label1.TabIndex = 9;
			this.label1.Text = "Business Unit";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnExpand
			// 
			this.btnExpand.Location = new System.Drawing.Point(3, 0);
			this.btnExpand.Name = "btnExpand";
			this.btnExpand.Size = new System.Drawing.Size(19, 20);
			this.btnExpand.TabIndex = 1;
			this.btnExpand.Text = "+";
			this.btnExpand.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.btnExpand.UseVisualStyleBackColor = true;
			this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(116, 27);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 13);
			this.label2.TabIndex = 12;
			this.label2.Text = "Add Access for User";
			// 
			// btnAddAccess
			// 
			this.btnAddAccess.Location = new System.Drawing.Point(605, 22);
			this.btnAddAccess.Name = "btnAddAccess";
			this.btnAddAccess.Size = new System.Drawing.Size(111, 23);
			this.btnAddAccess.TabIndex = 5;
			this.btnAddAccess.Text = "Add Access";
			this.btnAddAccess.UseVisualStyleBackColor = true;
			this.btnAddAccess.Click += new System.EventHandler(this.btnAddAccess_Click);
			// 
			// cmbUsers
			// 
			this.cmbUsers.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cmbUsers.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cmbUsers.FormattingEnabled = true;
			this.cmbUsers.InvalidAutoCompleteEntry = Q.ComboBoxWithAutoCompleteExtended.InvalidEntryBehavior.ErrorMessage;
			this.cmbUsers.Location = new System.Drawing.Point(229, 24);
			this.cmbUsers.Name = "cmbUsers";
			this.cmbUsers.Size = new System.Drawing.Size(369, 21);
			this.cmbUsers.TabIndex = 3;
			// 
			// BusinessUnitSubSubRoot
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.cmbUsers);
			this.Controls.Add(this.btnAddAccess);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.pnlUsers);
			this.Controls.Add(this.lblBusinessUnit);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnExpand);
			this.Name = "BusinessUnitSubSubRoot";
			this.Size = new System.Drawing.Size(720, 57);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnlUsers;
        private System.Windows.Forms.Label lblBusinessUnit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExpand;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAddAccess;
        private Q.ComboBoxWithAutoCompleteExtended cmbUsers;
    }
}

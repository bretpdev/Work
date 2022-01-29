namespace ACDCAccess
{
    partial class AddAndRemoveApplicationsAndKeys
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
			this.cmbType = new Q.ComboBoxWithAutoCompleteExtended();
			this.cmbApplication = new Q.ComboBoxWithAutoCompleteExtended();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.summaryKeyInfo1 = new ACDCAccess.SummaryKeyInfo();
			this.btnAddKeyToSystem = new System.Windows.Forms.Button();
			this.pnlExistingKeys = new System.Windows.Forms.FlowLayoutPanel();
			this.label6 = new System.Windows.Forms.Label();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtKey = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmbType
			// 
			this.cmbType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cmbType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cmbType.FormattingEnabled = true;
			this.cmbType.InvalidAutoCompleteEntry = Q.ComboBoxWithAutoCompleteExtended.InvalidEntryBehavior.ErrorMessage;
			this.cmbType.Location = new System.Drawing.Point(157, 63);
			this.cmbType.Name = "cmbType";
			this.cmbType.Size = new System.Drawing.Size(220, 21);
			this.cmbType.TabIndex = 7;
			// 
			// cmbSystem
			// 
			this.cmbApplication.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cmbApplication.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cmbApplication.FormattingEnabled = true;
			this.cmbApplication.InvalidAutoCompleteEntry = Q.ComboBoxWithAutoCompleteExtended.InvalidEntryBehavior.ErrorMessage;
			this.cmbApplication.Location = new System.Drawing.Point(157, 16);
			this.cmbApplication.Name = "cmbSystem";
			this.cmbApplication.Size = new System.Drawing.Size(220, 21);
			this.cmbApplication.TabIndex = 4;
			this.cmbApplication.SelectedIndexChanged += new System.EventHandler(this.cmbSystem_SelectedIndexChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.cmbType);
			this.groupBox2.Controls.Add(this.summaryKeyInfo1);
			this.groupBox2.Controls.Add(this.cmbApplication);
			this.groupBox2.Controls.Add(this.btnAddKeyToSystem);
			this.groupBox2.Controls.Add(this.pnlExistingKeys);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.txtDescription);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.txtKey);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Location = new System.Drawing.Point(516, 3);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(508, 580);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Add A Key To A System";
			// 
			// summaryKeyInfo1
			// 
			this.summaryKeyInfo1.Location = new System.Drawing.Point(161, 152);
			this.summaryKeyInfo1.Name = "summaryKeyInfo1";
			this.summaryKeyInfo1.Size = new System.Drawing.Size(340, 20);
			this.summaryKeyInfo1.TabIndex = 13;
			this.summaryKeyInfo1.TabStop = false;
			// 
			// btnAddKeyToSystem
			// 
			this.btnAddKeyToSystem.Location = new System.Drawing.Point(195, 544);
			this.btnAddKeyToSystem.Name = "btnAddKeyToSystem";
			this.btnAddKeyToSystem.Size = new System.Drawing.Size(116, 23);
			this.btnAddKeyToSystem.TabIndex = 12;
			this.btnAddKeyToSystem.Text = "Add Key To System";
			this.btnAddKeyToSystem.UseVisualStyleBackColor = true;
			this.btnAddKeyToSystem.Click += new System.EventHandler(this.btnAddKeyToSystem_Click);
			// 
			// pnlExistingKeys
			// 
			this.pnlExistingKeys.AutoScroll = true;
			this.pnlExistingKeys.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlExistingKeys.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.pnlExistingKeys.Location = new System.Drawing.Point(157, 174);
			this.pnlExistingKeys.Name = "pnlExistingKeys";
			this.pnlExistingKeys.Size = new System.Drawing.Size(345, 358);
			this.pnlExistingKeys.TabIndex = 11;
			this.pnlExistingKeys.TabStop = true;
			this.pnlExistingKeys.WrapContents = false;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(11, 158);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(69, 13);
			this.label6.TabIndex = 10;
			this.label6.Text = "Existing Keys";
			// 
			// txtDescription
			// 
			this.txtDescription.Location = new System.Drawing.Point(157, 86);
			this.txtDescription.MaxLength = 2000;
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.Size = new System.Drawing.Size(345, 64);
			this.txtDescription.TabIndex = 9;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(11, 89);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(60, 13);
			this.label5.TabIndex = 8;
			this.label5.Text = "Description";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(11, 66);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(31, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "Type";
			// 
			// txtKey
			// 
			this.txtKey.Location = new System.Drawing.Point(157, 39);
			this.txtKey.MaxLength = 100;
			this.txtKey.Name = "txtKey";
			this.txtKey.Size = new System.Drawing.Size(220, 20);
			this.txtKey.TabIndex = 5;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(11, 42);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(25, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Key";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(11, 20);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(41, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "System";
			// 
			// AddAndRemoveSystemsAndKeys
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox2);
			this.Name = "AddAndRemoveSystemsAndKeys";
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAddKeyToSystem;
        private System.Windows.Forms.FlowLayoutPanel pnlExistingKeys;
        private System.Windows.Forms.Label label6;
        private SummaryKeyInfo summaryKeyInfo1;
        private Q.ComboBoxWithAutoCompleteExtended cmbType;
        private Q.ComboBoxWithAutoCompleteExtended cmbApplication;
    }
}

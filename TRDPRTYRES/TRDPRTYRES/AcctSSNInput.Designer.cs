namespace TRDPRTYRES
{
	partial class AcctSSNInput
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
            this.components = new System.ComponentModel.Container();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblSsnAcct = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ReferenceList = new System.Windows.Forms.ListView();
            this.RefId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RefName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NewReference = new System.Windows.Forms.Button();
            this.Go = new System.Windows.Forms.Button();
            this.AcctSsn = new Uheaa.Common.WinForms.AccountIdentifierTextBox();
            this.borInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.borInfoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Enabled = false;
            this.OK.Location = new System.Drawing.Point(304, 376);
            this.OK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(112, 35);
            this.OK.TabIndex = 3;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(24, 376);
            this.Cancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(112, 35);
            this.Cancel.TabIndex = 5;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.Location = new System.Drawing.Point(18, 14);
            this.lblInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(422, 58);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "Please enter the SSN or Account number of the borrower for whom you wish to print" +
    " a third-party authorization form.";
            // 
            // lblSsnAcct
            // 
            this.lblSsnAcct.AutoSize = true;
            this.lblSsnAcct.Location = new System.Drawing.Point(20, 77);
            this.lblSsnAcct.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSsnAcct.Name = "lblSsnAcct";
            this.lblSsnAcct.Size = new System.Drawing.Size(165, 20);
            this.lblSsnAcct.TabIndex = 4;
            this.lblSsnAcct.Text = "Account Number/SSN";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(289, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Choose the reference or add a new one";
            // 
            // ReferenceList
            // 
            this.ReferenceList.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.ReferenceList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReferenceList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.RefId,
            this.RefName});
            this.ReferenceList.HideSelection = false;
            this.ReferenceList.Location = new System.Drawing.Point(24, 130);
            this.ReferenceList.Name = "ReferenceList";
            this.ReferenceList.Size = new System.Drawing.Size(392, 228);
            this.ReferenceList.TabIndex = 2;
            this.ReferenceList.UseCompatibleStateImageBehavior = false;
            this.ReferenceList.View = System.Windows.Forms.View.Details;
            this.ReferenceList.SelectedIndexChanged += new System.EventHandler(this.ReferenceList_SelectedIndexChanged);
            this.ReferenceList.DoubleClick += new System.EventHandler(this.ReferenceList_DoubleClick);
            // 
            // RefId
            // 
            this.RefId.Text = "Reference ID";
            this.RefId.Width = 133;
            // 
            // RefName
            // 
            this.RefName.Text = "Reference Name";
            this.RefName.Width = 252;
            // 
            // NewReference
            // 
            this.NewReference.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NewReference.Enabled = false;
            this.NewReference.Location = new System.Drawing.Point(164, 376);
            this.NewReference.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NewReference.Name = "NewReference";
            this.NewReference.Size = new System.Drawing.Size(112, 35);
            this.NewReference.TabIndex = 4;
            this.NewReference.Text = "Add New";
            this.NewReference.UseVisualStyleBackColor = true;
            this.NewReference.Click += new System.EventHandler(this.NewReference_Click);
            // 
            // Go
            // 
            this.Go.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Go.Location = new System.Drawing.Point(354, 67);
            this.Go.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Go.Name = "Go";
            this.Go.Size = new System.Drawing.Size(46, 35);
            this.Go.TabIndex = 1;
            this.Go.Text = "GO";
            this.Go.UseVisualStyleBackColor = true;
            this.Go.Click += new System.EventHandler(this.Go_Click);
            // 
            // AcctSsn
            // 
            this.AcctSsn.AccountNumber = null;
            this.AcctSsn.AllowedSpecialCharacters = "";
            this.AcctSsn.Location = new System.Drawing.Point(192, 71);
            this.AcctSsn.MaxLength = 10;
            this.AcctSsn.Name = "AcctSsn";
            this.AcctSsn.Size = new System.Drawing.Size(155, 26);
            this.AcctSsn.Ssn = null;
            this.AcctSsn.TabIndex = 0;
            this.AcctSsn.TextChanged += new System.EventHandler(this.AcctSsn_TextChanged);
            this.AcctSsn.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AcctSsn_KeyPress);
            // 
            // borInfoBindingSource
            // 
            this.borInfoBindingSource.DataSource = typeof(TRDPRTYRES.BorReferenceInfo);
            // 
            // AcctSSNInput
            // 
            this.AcceptButton = this.Go;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(448, 425);
            this.Controls.Add(this.AcctSsn);
            this.Controls.Add(this.Go);
            this.Controls.Add(this.NewReference);
            this.Controls.Add(this.ReferenceList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblSsnAcct);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(468, 468);
            this.Name = "AcctSSNInput";
            this.ShowIcon = false;
            ((System.ComponentModel.ISupportInitialize)(this.borInfoBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.Label lblSsnAcct;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource borInfoBindingSource;
        private System.Windows.Forms.ListView ReferenceList;
        private System.Windows.Forms.ColumnHeader RefId;
        private System.Windows.Forms.ColumnHeader RefName;
        private System.Windows.Forms.Button NewReference;
        private System.Windows.Forms.Button Go;
        private Uheaa.Common.WinForms.AccountIdentifierTextBox AcctSsn;
    }
}
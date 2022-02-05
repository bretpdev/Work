namespace SpecialEmailCampaignFed
{
	partial class frmCampaignDetails
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
            this.lblEmailSubject = new System.Windows.Forms.Label();
            this.txtEmailSubject = new System.Windows.Forms.TextBox();
            this.campaignDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblFromEmail = new System.Windows.Forms.Label();
            this.txtFromAdd = new System.Windows.Forms.TextBox();
            this.chkCornerStone = new System.Windows.Forms.CheckBox();
            this.txtArc = new System.Windows.Forms.TextBox();
            this.lblArc = new System.Windows.Forms.Label();
            this.txtCommentTxt = new System.Windows.Forms.TextBox();
            this.lblCommentTxt = new System.Windows.Forms.Label();
            this.lblNoteToUser = new System.Windows.Forms.Label();
            this.lblDataFile = new System.Windows.Forms.Label();
            this.txtDataFile = new System.Windows.Forms.TextBox();
            this.btnBrowseDataFile = new System.Windows.Forms.Button();
            this.lblHtmlFile = new System.Windows.Forms.Label();
            this.txtHtmlFile = new System.Windows.Forms.TextBox();
            this.btnBrowseHtml = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblDataFileNote = new System.Windows.Forms.Label();
            this.lblAdditionInfo = new System.Windows.Forms.Label();
            this.openFileDialogDataFile = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialogHTMLFile = new System.Windows.Forms.OpenFileDialog();
            this.cboIncludeAccountNum = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.campaignDataBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // lblEmailSubject
            // 
            this.lblEmailSubject.AutoSize = true;
            this.lblEmailSubject.Location = new System.Drawing.Point(38, 29);
            this.lblEmailSubject.Name = "lblEmailSubject";
            this.lblEmailSubject.Size = new System.Drawing.Size(74, 13);
            this.lblEmailSubject.TabIndex = 0;
            this.lblEmailSubject.Text = "Email Subject:";
            // 
            // txtEmailSubject
            // 
            this.txtEmailSubject.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.campaignDataBindingSource, "EmailSubjectLine", true));
            this.txtEmailSubject.Location = new System.Drawing.Point(118, 26);
            this.txtEmailSubject.Name = "txtEmailSubject";
            this.txtEmailSubject.Size = new System.Drawing.Size(435, 20);
            this.txtEmailSubject.TabIndex = 1;
            // 
            // campaignDataBindingSource
            // 
            this.campaignDataBindingSource.DataSource = typeof(SpecialEmailCampaignFed.CampaignData);
            // 
            // lblFromEmail
            // 
            this.lblFromEmail.AutoSize = true;
            this.lblFromEmail.Location = new System.Drawing.Point(4, 56);
            this.lblFromEmail.Name = "lblFromEmail";
            this.lblFromEmail.Size = new System.Drawing.Size(108, 13);
            this.lblFromEmail.TabIndex = 2;
            this.lblFromEmail.Text = "From: (Email Address)";
            // 
            // txtFromAdd
            // 
            this.txtFromAdd.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.campaignDataBindingSource, "EmailFrom", true));
            this.txtFromAdd.Location = new System.Drawing.Point(118, 53);
            this.txtFromAdd.Name = "txtFromAdd";
            this.txtFromAdd.Size = new System.Drawing.Size(209, 20);
            this.txtFromAdd.TabIndex = 3;
            // 
            // chkCornerStone
            // 
            this.chkCornerStone.AutoSize = true;
            this.chkCornerStone.Location = new System.Drawing.Point(24, 92);
            this.chkCornerStone.Name = "chkCornerStone";
            this.chkCornerStone.Size = new System.Drawing.Size(129, 17);
            this.chkCornerStone.TabIndex = 4;
            this.chkCornerStone.Text = "Add Activity Comment";
            this.chkCornerStone.UseVisualStyleBackColor = true;
            this.chkCornerStone.CheckedChanged += new System.EventHandler(this.chkCornerStone_CheckedChanged);
            // 
            // txtArc
            // 
            this.txtArc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.campaignDataBindingSource, "Arc", true));
            this.txtArc.Location = new System.Drawing.Point(205, 89);
            this.txtArc.Name = "txtArc";
            this.txtArc.ReadOnly = true;
            this.txtArc.Size = new System.Drawing.Size(57, 20);
            this.txtArc.TabIndex = 5;
            // 
            // lblArc
            // 
            this.lblArc.AutoSize = true;
            this.lblArc.Location = new System.Drawing.Point(167, 93);
            this.lblArc.Name = "lblArc";
            this.lblArc.Size = new System.Drawing.Size(32, 13);
            this.lblArc.TabIndex = 6;
            this.lblArc.Text = "ARC:";
            // 
            // txtCommentTxt
            // 
            this.txtCommentTxt.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.campaignDataBindingSource, "CommentText", true));
            this.txtCommentTxt.Location = new System.Drawing.Point(104, 134);
            this.txtCommentTxt.Multiline = true;
            this.txtCommentTxt.Name = "txtCommentTxt";
            this.txtCommentTxt.Size = new System.Drawing.Size(466, 135);
            this.txtCommentTxt.TabIndex = 7;
            // 
            // lblCommentTxt
            // 
            this.lblCommentTxt.AutoSize = true;
            this.lblCommentTxt.Location = new System.Drawing.Point(23, 137);
            this.lblCommentTxt.Name = "lblCommentTxt";
            this.lblCommentTxt.Size = new System.Drawing.Size(75, 13);
            this.lblCommentTxt.TabIndex = 8;
            this.lblCommentTxt.Text = "Comment Text";
            // 
            // lblNoteToUser
            // 
            this.lblNoteToUser.AutoSize = true;
            this.lblNoteToUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoteToUser.ForeColor = System.Drawing.Color.Red;
            this.lblNoteToUser.Location = new System.Drawing.Point(101, 373);
            this.lblNoteToUser.MaximumSize = new System.Drawing.Size(400, 40);
            this.lblNoteToUser.Name = "lblNoteToUser";
            this.lblNoteToUser.Size = new System.Drawing.Size(391, 40);
            this.lblNoteToUser.TabIndex = 9;
            this.lblNoteToUser.Text = "Note to user: make sure any images referenced in HTML are saved in https://www.my" +
                "cornerstoneloan.org/images/.  You may have to coordinate\r\nwith CNOC to save the " +
                "file in the directory";
            // 
            // lblDataFile
            // 
            this.lblDataFile.AutoSize = true;
            this.lblDataFile.Location = new System.Drawing.Point(39, 295);
            this.lblDataFile.Name = "lblDataFile";
            this.lblDataFile.Size = new System.Drawing.Size(49, 13);
            this.lblDataFile.TabIndex = 10;
            this.lblDataFile.Text = "Data File";
            // 
            // txtDataFile
            // 
            this.txtDataFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.campaignDataBindingSource, "DataFile", true));
            this.txtDataFile.Location = new System.Drawing.Point(104, 292);
            this.txtDataFile.Name = "txtDataFile";
            this.txtDataFile.ReadOnly = true;
            this.txtDataFile.Size = new System.Drawing.Size(359, 20);
            this.txtDataFile.TabIndex = 11;
            // 
            // btnBrowseDataFile
            // 
            this.btnBrowseDataFile.Location = new System.Drawing.Point(488, 292);
            this.btnBrowseDataFile.Name = "btnBrowseDataFile";
            this.btnBrowseDataFile.Size = new System.Drawing.Size(82, 20);
            this.btnBrowseDataFile.TabIndex = 12;
            this.btnBrowseDataFile.Text = "Browse";
            this.btnBrowseDataFile.UseVisualStyleBackColor = true;
            this.btnBrowseDataFile.Click += new System.EventHandler(this.btnBrowseDataFile_Click);
            // 
            // lblHtmlFile
            // 
            this.lblHtmlFile.AutoSize = true;
            this.lblHtmlFile.Location = new System.Drawing.Point(32, 353);
            this.lblHtmlFile.Name = "lblHtmlFile";
            this.lblHtmlFile.Size = new System.Drawing.Size(56, 13);
            this.lblHtmlFile.TabIndex = 13;
            this.lblHtmlFile.Text = "HTML File";
            // 
            // txtHtmlFile
            // 
            this.txtHtmlFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.campaignDataBindingSource, "HtmlFile", true));
            this.txtHtmlFile.Location = new System.Drawing.Point(104, 350);
            this.txtHtmlFile.Name = "txtHtmlFile";
            this.txtHtmlFile.ReadOnly = true;
            this.txtHtmlFile.Size = new System.Drawing.Size(359, 20);
            this.txtHtmlFile.TabIndex = 14;
            // 
            // btnBrowseHtml
            // 
            this.btnBrowseHtml.Location = new System.Drawing.Point(488, 349);
            this.btnBrowseHtml.Name = "btnBrowseHtml";
            this.btnBrowseHtml.Size = new System.Drawing.Size(82, 20);
            this.btnBrowseHtml.TabIndex = 15;
            this.btnBrowseHtml.Text = "Browse";
            this.btnBrowseHtml.UseVisualStyleBackColor = true;
            this.btnBrowseHtml.Click += new System.EventHandler(this.btnBrowseHtml_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(140, 466);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(59, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(217, 466);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(59, 23);
            this.btnTest.TabIndex = 17;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(294, 466);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(59, 23);
            this.btnRun.TabIndex = 18;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(371, 466);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(59, 23);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblDataFileNote
            // 
            this.lblDataFileNote.AutoSize = true;
            this.lblDataFileNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataFileNote.ForeColor = System.Drawing.Color.Red;
            this.lblDataFileNote.Location = new System.Drawing.Point(101, 315);
            this.lblDataFileNote.MaximumSize = new System.Drawing.Size(415, 0);
            this.lblDataFileNote.Name = "lblDataFileNote";
            this.lblDataFileNote.Size = new System.Drawing.Size(406, 26);
            this.lblDataFileNote.TabIndex = 20;
            this.lblDataFileNote.Text = "The DataFile must have a header row, and must have the following fields in the fo" +
                "llowing order: (Account Number, Borrower Name, Email) ";
            // 
            // lblAdditionInfo
            // 
            this.lblAdditionInfo.AutoSize = true;
            this.lblAdditionInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdditionInfo.ForeColor = System.Drawing.Color.Red;
            this.lblAdditionInfo.Location = new System.Drawing.Point(101, 413);
            this.lblAdditionInfo.MaximumSize = new System.Drawing.Size(400, 0);
            this.lblAdditionInfo.Name = "lblAdditionInfo";
            this.lblAdditionInfo.Size = new System.Drawing.Size(393, 39);
            this.lblAdditionInfo.TabIndex = 21;
            this.lblAdditionInfo.Text = "If you want the Borrowers Name and Recipient Id to be merged into the email then " +
                "you must add the following text as a place holder [[[Name]]] and or [[[RecipId]]" +
                "]";
            // 
            // openFileDialogDataFile
            // 
            this.openFileDialogDataFile.FileName = "openFileDialogDataFile";
            // 
            // openFileDialogHTMLFile
            // 
            this.openFileDialogHTMLFile.FileName = "openFileDialogHTMLFile";
            // 
            // cboIncludeAccountNum
            // 
            this.cboIncludeAccountNum.AutoSize = true;
            this.cboIncludeAccountNum.Location = new System.Drawing.Point(294, 92);
            this.cboIncludeAccountNum.Name = "cboIncludeAccountNum";
            this.cboIncludeAccountNum.Size = new System.Drawing.Size(144, 17);
            this.cboIncludeAccountNum.TabIndex = 22;
            this.cboIncludeAccountNum.Text = "Include Account Number";
            this.cboIncludeAccountNum.UseVisualStyleBackColor = true;
            this.cboIncludeAccountNum.CheckedChanged += new System.EventHandler(this.cboIncludeAccountNum_CheckedChanged);
            // 
            // frmCampaignDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 508);
            this.Controls.Add(this.cboIncludeAccountNum);
            this.Controls.Add(this.lblAdditionInfo);
            this.Controls.Add(this.lblDataFileNote);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnBrowseHtml);
            this.Controls.Add(this.txtHtmlFile);
            this.Controls.Add(this.lblHtmlFile);
            this.Controls.Add(this.btnBrowseDataFile);
            this.Controls.Add(this.txtDataFile);
            this.Controls.Add(this.lblDataFile);
            this.Controls.Add(this.lblNoteToUser);
            this.Controls.Add(this.lblCommentTxt);
            this.Controls.Add(this.txtCommentTxt);
            this.Controls.Add(this.lblArc);
            this.Controls.Add(this.txtArc);
            this.Controls.Add(this.chkCornerStone);
            this.Controls.Add(this.txtFromAdd);
            this.Controls.Add(this.lblFromEmail);
            this.Controls.Add(this.txtEmailSubject);
            this.Controls.Add(this.lblEmailSubject);
            this.Name = "frmCampaignDetails";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "frmCampaignDetails";
            ((System.ComponentModel.ISupportInitialize)(this.campaignDataBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblEmailSubject;
		private System.Windows.Forms.TextBox txtEmailSubject;
		private System.Windows.Forms.Label lblFromEmail;
		private System.Windows.Forms.TextBox txtFromAdd;
		private System.Windows.Forms.CheckBox chkCornerStone;
		private System.Windows.Forms.TextBox txtArc;
		private System.Windows.Forms.Label lblArc;
		private System.Windows.Forms.TextBox txtCommentTxt;
		private System.Windows.Forms.Label lblCommentTxt;
		private System.Windows.Forms.Label lblNoteToUser;
		private System.Windows.Forms.Label lblDataFile;
		private System.Windows.Forms.TextBox txtDataFile;
		private System.Windows.Forms.Button btnBrowseDataFile;
		private System.Windows.Forms.Label lblHtmlFile;
		private System.Windows.Forms.TextBox txtHtmlFile;
		private System.Windows.Forms.Button btnBrowseHtml;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnTest;
		private System.Windows.Forms.Button btnRun;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblDataFileNote;
		private System.Windows.Forms.Label lblAdditionInfo;
		private System.Windows.Forms.BindingSource campaignDataBindingSource;
		private System.Windows.Forms.OpenFileDialog openFileDialogDataFile;
        private System.Windows.Forms.OpenFileDialog openFileDialogHTMLFile;
        private System.Windows.Forms.CheckBox cboIncludeAccountNum;
	}
}
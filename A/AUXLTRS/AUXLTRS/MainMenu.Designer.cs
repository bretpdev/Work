namespace AUXLTRS
{
    partial class MainMenu
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
			this.grpOther = new System.Windows.Forms.GroupBox();
			this.chkSsnConflict = new System.Windows.Forms.CheckBox();
			this.chkDobConflict = new System.Windows.Forms.CheckBox();
			this.chkNameConflict = new System.Windows.Forms.CheckBox();
			this.chkThirdPartyAuth = new System.Windows.Forms.CheckBox();
			this.chkCustomLetter = new System.Windows.Forms.CheckBox();
			this.grpSkipTracing = new System.Windows.Forms.GroupBox();
			this.chkPostOffice = new System.Windows.Forms.CheckBox();
			this.chkPropertyOwner = new System.Windows.Forms.CheckBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.grpAwg = new System.Windows.Forms.GroupBox();
			this.chkNoticeOfSatisfaction = new System.Windows.Forms.CheckBox();
			this.chkRequestForHearing = new System.Windows.Forms.CheckBox();
			this.grpNsldsSsn = new System.Windows.Forms.GroupBox();
			this.chkNsldsSsn3 = new System.Windows.Forms.CheckBox();
			this.chkNsldsSsn2 = new System.Windows.Forms.CheckBox();
			this.chkNsldsSsn1 = new System.Windows.Forms.CheckBox();
			this.grpOther.SuspendLayout();
			this.grpSkipTracing.SuspendLayout();
			this.grpAwg.SuspendLayout();
			this.grpNsldsSsn.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpOther
			// 
			this.grpOther.Controls.Add(this.chkSsnConflict);
			this.grpOther.Controls.Add(this.chkDobConflict);
			this.grpOther.Controls.Add(this.chkNameConflict);
			this.grpOther.Controls.Add(this.chkThirdPartyAuth);
			this.grpOther.Controls.Add(this.chkCustomLetter);
			this.grpOther.Location = new System.Drawing.Point(13, 13);
			this.grpOther.Name = "grpOther";
			this.grpOther.Size = new System.Drawing.Size(228, 136);
			this.grpOther.TabIndex = 0;
			this.grpOther.TabStop = false;
			this.grpOther.Text = "Other";
			// 
			// chkSsnConflict
			// 
			this.chkSsnConflict.AutoSize = true;
			this.chkSsnConflict.Location = new System.Drawing.Point(6, 112);
			this.chkSsnConflict.Name = "chkSsnConflict";
			this.chkSsnConflict.Size = new System.Drawing.Size(116, 17);
			this.chkSsnConflict.TabIndex = 11;
			this.chkSsnConflict.Text = "SSN Conflict Letter";
			this.chkSsnConflict.UseVisualStyleBackColor = true;
			this.chkSsnConflict.Click += new System.EventHandler(this.chkSsnConflict_Click);
			// 
			// chkDobConflict
			// 
			this.chkDobConflict.AutoSize = true;
			this.chkDobConflict.Location = new System.Drawing.Point(6, 89);
			this.chkDobConflict.Name = "chkDobConflict";
			this.chkDobConflict.Size = new System.Drawing.Size(117, 17);
			this.chkDobConflict.TabIndex = 10;
			this.chkDobConflict.Text = "DOB Conflict Letter";
			this.chkDobConflict.UseVisualStyleBackColor = true;
			this.chkDobConflict.Click += new System.EventHandler(this.chkDobConflict_Click);
			// 
			// chkNameConflict
			// 
			this.chkNameConflict.AutoSize = true;
			this.chkNameConflict.Location = new System.Drawing.Point(6, 66);
			this.chkNameConflict.Name = "chkNameConflict";
			this.chkNameConflict.Size = new System.Drawing.Size(122, 17);
			this.chkNameConflict.TabIndex = 9;
			this.chkNameConflict.Text = "Name Conflict Letter";
			this.chkNameConflict.UseVisualStyleBackColor = true;
			this.chkNameConflict.Click += new System.EventHandler(this.chkNameConflict_Click);
			// 
			// chkThirdPartyAuth
			// 
			this.chkThirdPartyAuth.AutoSize = true;
			this.chkThirdPartyAuth.Location = new System.Drawing.Point(6, 43);
			this.chkThirdPartyAuth.Name = "chkThirdPartyAuth";
			this.chkThirdPartyAuth.Size = new System.Drawing.Size(167, 17);
			this.chkThirdPartyAuth.TabIndex = 8;
			this.chkThirdPartyAuth.Text = "Third-Party Authorization Form";
			this.chkThirdPartyAuth.UseVisualStyleBackColor = true;
			this.chkThirdPartyAuth.Click += new System.EventHandler(this.chkThirdPartyAuth_Click);
			// 
			// chkCustomLetter
			// 
			this.chkCustomLetter.AutoSize = true;
			this.chkCustomLetter.Location = new System.Drawing.Point(6, 20);
			this.chkCustomLetter.Name = "chkCustomLetter";
			this.chkCustomLetter.Size = new System.Drawing.Size(91, 17);
			this.chkCustomLetter.TabIndex = 7;
			this.chkCustomLetter.Text = "Custom Letter";
			this.chkCustomLetter.UseVisualStyleBackColor = true;
			this.chkCustomLetter.Click += new System.EventHandler(this.chkCustomLetter_Click);
			// 
			// grpSkipTracing
			// 
			this.grpSkipTracing.Controls.Add(this.chkPostOffice);
			this.grpSkipTracing.Controls.Add(this.chkPropertyOwner);
			this.grpSkipTracing.Location = new System.Drawing.Point(13, 155);
			this.grpSkipTracing.Name = "grpSkipTracing";
			this.grpSkipTracing.Size = new System.Drawing.Size(228, 67);
			this.grpSkipTracing.TabIndex = 1;
			this.grpSkipTracing.TabStop = false;
			this.grpSkipTracing.Text = "Skip Tracing";
			// 
			// chkPostOffice
			// 
			this.chkPostOffice.AutoSize = true;
			this.chkPostOffice.Location = new System.Drawing.Point(6, 43);
			this.chkPostOffice.Name = "chkPostOffice";
			this.chkPostOffice.Size = new System.Drawing.Size(108, 17);
			this.chkPostOffice.TabIndex = 13;
			this.chkPostOffice.Text = "Post Office Letter";
			this.chkPostOffice.UseVisualStyleBackColor = true;
			this.chkPostOffice.Click += new System.EventHandler(this.chkPostOffice_Click);
			// 
			// chkPropertyOwner
			// 
			this.chkPropertyOwner.AutoSize = true;
			this.chkPropertyOwner.Location = new System.Drawing.Point(6, 20);
			this.chkPropertyOwner.Name = "chkPropertyOwner";
			this.chkPropertyOwner.Size = new System.Drawing.Size(197, 17);
			this.chkPropertyOwner.TabIndex = 12;
			this.chkPropertyOwner.Text = "Property Owner Information Request";
			this.chkPropertyOwner.UseVisualStyleBackColor = true;
			this.chkPropertyOwner.Click += new System.EventHandler(this.chkPropertyOwner_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(67, 310);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(96, 31);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// grpAwg
			// 
			this.grpAwg.Controls.Add(this.chkNoticeOfSatisfaction);
			this.grpAwg.Controls.Add(this.chkRequestForHearing);
			this.grpAwg.Location = new System.Drawing.Point(13, 228);
			this.grpAwg.Name = "grpAwg";
			this.grpAwg.Size = new System.Drawing.Size(228, 67);
			this.grpAwg.TabIndex = 2;
			this.grpAwg.TabStop = false;
			this.grpAwg.Text = "AWG";
			// 
			// chkNoticeOfSatisfaction
			// 
			this.chkNoticeOfSatisfaction.AutoSize = true;
			this.chkNoticeOfSatisfaction.Location = new System.Drawing.Point(6, 43);
			this.chkNoticeOfSatisfaction.Name = "chkNoticeOfSatisfaction";
			this.chkNoticeOfSatisfaction.Size = new System.Drawing.Size(127, 17);
			this.chkNoticeOfSatisfaction.TabIndex = 15;
			this.chkNoticeOfSatisfaction.Text = "Notice of Satisfaction";
			this.chkNoticeOfSatisfaction.UseVisualStyleBackColor = true;
			this.chkNoticeOfSatisfaction.Click += new System.EventHandler(this.chkNoticeOfSatisfaction_Click);
			// 
			// chkRequestForHearing
			// 
			this.chkRequestForHearing.AutoSize = true;
			this.chkRequestForHearing.Location = new System.Drawing.Point(6, 20);
			this.chkRequestForHearing.Name = "chkRequestForHearing";
			this.chkRequestForHearing.Size = new System.Drawing.Size(121, 17);
			this.chkRequestForHearing.TabIndex = 14;
			this.chkRequestForHearing.Text = "Request for Hearing";
			this.chkRequestForHearing.UseVisualStyleBackColor = true;
			this.chkRequestForHearing.Click += new System.EventHandler(this.chkRequestForHearing_Click);
			// 
			// grpNsldsSsn
			// 
			this.grpNsldsSsn.Controls.Add(this.chkNsldsSsn3);
			this.grpNsldsSsn.Controls.Add(this.chkNsldsSsn2);
			this.grpNsldsSsn.Controls.Add(this.chkNsldsSsn1);
			this.grpNsldsSsn.Location = new System.Drawing.Point(247, 13);
			this.grpNsldsSsn.Name = "grpNsldsSsn";
			this.grpNsldsSsn.Size = new System.Drawing.Size(182, 136);
			this.grpNsldsSsn.TabIndex = 12;
			this.grpNsldsSsn.TabStop = false;
			this.grpNsldsSsn.Text = "NSLDS SSN Conflict Process";
			// 
			// chkNsldsSsn3
			// 
			this.chkNsldsSsn3.AutoSize = true;
			this.chkNsldsSsn3.Location = new System.Drawing.Point(6, 66);
			this.chkNsldsSsn3.Name = "chkNsldsSsn3";
			this.chkNsldsSsn3.Size = new System.Drawing.Size(54, 17);
			this.chkNsldsSsn3.TabIndex = 9;
			this.chkNsldsSsn3.Text = "SSN3";
			this.chkNsldsSsn3.UseVisualStyleBackColor = true;
			this.chkNsldsSsn3.Click += new System.EventHandler(this.chkNsldsSsn3_Click);
			// 
			// chkNsldsSsn2
			// 
			this.chkNsldsSsn2.AutoSize = true;
			this.chkNsldsSsn2.Location = new System.Drawing.Point(6, 43);
			this.chkNsldsSsn2.Name = "chkNsldsSsn2";
			this.chkNsldsSsn2.Size = new System.Drawing.Size(54, 17);
			this.chkNsldsSsn2.TabIndex = 8;
			this.chkNsldsSsn2.Text = "SSN2";
			this.chkNsldsSsn2.UseVisualStyleBackColor = true;
			this.chkNsldsSsn2.Click += new System.EventHandler(this.chkNsldsSsn2_Click);
			// 
			// chkNsldsSsn1
			// 
			this.chkNsldsSsn1.AutoSize = true;
			this.chkNsldsSsn1.Location = new System.Drawing.Point(6, 20);
			this.chkNsldsSsn1.Name = "chkNsldsSsn1";
			this.chkNsldsSsn1.Size = new System.Drawing.Size(54, 17);
			this.chkNsldsSsn1.TabIndex = 7;
			this.chkNsldsSsn1.Text = "SSN1";
			this.chkNsldsSsn1.UseVisualStyleBackColor = true;
			this.chkNsldsSsn1.Click += new System.EventHandler(this.chkNsldsSsn1_Click);
			// 
			// MainMenu
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(442, 353);
			this.Controls.Add(this.grpNsldsSsn);
			this.Controls.Add(this.grpAwg);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.grpSkipTracing);
			this.Controls.Add(this.grpOther);
			this.Name = "MainMenu";
			this.Text = "Auxiliary Services Letters Main Menu";
			this.grpOther.ResumeLayout(false);
			this.grpOther.PerformLayout();
			this.grpSkipTracing.ResumeLayout(false);
			this.grpSkipTracing.PerformLayout();
			this.grpAwg.ResumeLayout(false);
			this.grpAwg.PerformLayout();
			this.grpNsldsSsn.ResumeLayout(false);
			this.grpNsldsSsn.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpOther;
        private System.Windows.Forms.GroupBox grpSkipTracing;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpAwg;
        private System.Windows.Forms.CheckBox chkSsnConflict;
        private System.Windows.Forms.CheckBox chkDobConflict;
        private System.Windows.Forms.CheckBox chkNameConflict;
        private System.Windows.Forms.CheckBox chkThirdPartyAuth;
        private System.Windows.Forms.CheckBox chkCustomLetter;
        private System.Windows.Forms.CheckBox chkPostOffice;
        private System.Windows.Forms.CheckBox chkPropertyOwner;
        private System.Windows.Forms.CheckBox chkNoticeOfSatisfaction;
        private System.Windows.Forms.CheckBox chkRequestForHearing;
        private System.Windows.Forms.GroupBox grpNsldsSsn;
        private System.Windows.Forms.CheckBox chkNsldsSsn3;
        private System.Windows.Forms.CheckBox chkNsldsSsn2;
        private System.Windows.Forms.CheckBox chkNsldsSsn1;
    }
}
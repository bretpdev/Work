namespace SPLTRCAMP
{
    partial class Entry
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
            this.txtMergeFile = new System.Windows.Forms.TextBox();
            this.btnDataFileBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLetterFile = new System.Windows.Forms.TextBox();
            this.btnLetterFileBrowse = new System.Windows.Forms.Button();
            this.ofdBrowser = new System.Windows.Forms.OpenFileDialog();
            this.chkOneLINKComments = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtActionCode = new System.Windows.Forms.TextBox();
            this.txtARC = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkCompassComments = new System.Windows.Forms.CheckBox();
            this.txtComments = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtLetterID = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbLetterRecipient = new System.Windows.Forms.ComboBox();
            this.cmbPageCountAndDestination = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtLetterDescription = new System.Windows.Forms.TextBox();
            this.campaignDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.campaignDataBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // txtMergeFile
            // 
            this.txtMergeFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.campaignDataBindingSource, "DataFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtMergeFile.Location = new System.Drawing.Point(204, 38);
            this.txtMergeFile.Name = "txtMergeFile";
            this.txtMergeFile.ReadOnly = true;
            this.txtMergeFile.Size = new System.Drawing.Size(269, 20);
            this.txtMergeFile.TabIndex = 3;
            // 
            // btnDataFileBrowse
            // 
            this.btnDataFileBrowse.Location = new System.Drawing.Point(479, 37);
            this.btnDataFileBrowse.Name = "btnDataFileBrowse";
            this.btnDataFileBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnDataFileBrowse.TabIndex = 2;
            this.btnDataFileBrowse.Text = "Browse";
            this.btnDataFileBrowse.UseVisualStyleBackColor = true;
            this.btnDataFileBrowse.Click += new System.EventHandler(this.btnDataFileBrowse_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Data/Merge File";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(185, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "Letter File/Word Document";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLetterFile
            // 
            this.txtLetterFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.campaignDataBindingSource, "LetterFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtLetterFile.Location = new System.Drawing.Point(204, 64);
            this.txtLetterFile.Name = "txtLetterFile";
            this.txtLetterFile.ReadOnly = true;
            this.txtLetterFile.Size = new System.Drawing.Size(269, 20);
            this.txtLetterFile.TabIndex = 6;
            // 
            // btnLetterFileBrowse
            // 
            this.btnLetterFileBrowse.Location = new System.Drawing.Point(479, 63);
            this.btnLetterFileBrowse.Name = "btnLetterFileBrowse";
            this.btnLetterFileBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnLetterFileBrowse.TabIndex = 5;
            this.btnLetterFileBrowse.Text = "Browse";
            this.btnLetterFileBrowse.UseVisualStyleBackColor = true;
            this.btnLetterFileBrowse.Click += new System.EventHandler(this.btnLetterFileBrowse_Click);
            // 
            // chkOneLINKComments
            // 
            this.chkOneLINKComments.AutoSize = true;
            this.chkOneLINKComments.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.campaignDataBindingSource, "AddCommentsToOneLINK", true));
            this.chkOneLINKComments.Location = new System.Drawing.Point(204, 194);
            this.chkOneLINKComments.Name = "chkOneLINKComments";
            this.chkOneLINKComments.Size = new System.Drawing.Size(15, 14);
            this.chkOneLINKComments.TabIndex = 8;
            this.chkOneLINKComments.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.chkOneLINKComments.UseVisualStyleBackColor = true;
            this.chkOneLINKComments.CheckedChanged += new System.EventHandler(this.chkOneLINKComments_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(185, 23);
            this.label3.TabIndex = 9;
            this.label3.Text = "OneLINK Comments Setup";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtActionCode
            // 
            this.txtActionCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.campaignDataBindingSource, "ActionCode", true));
            this.txtActionCode.Enabled = false;
            this.txtActionCode.Location = new System.Drawing.Point(225, 191);
            this.txtActionCode.MaxLength = 5;
            this.txtActionCode.Name = "txtActionCode";
            this.txtActionCode.Size = new System.Drawing.Size(72, 20);
            this.txtActionCode.TabIndex = 10;
            this.txtActionCode.Text = "Action Code";
            this.txtActionCode.Click += new System.EventHandler(this.txtActionCode_Click);
            this.txtActionCode.Enter += new System.EventHandler(this.txtActionCode_Enter);
            // 
            // txtARC
            // 
            this.txtARC.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.campaignDataBindingSource, "ARC", true));
            this.txtARC.Enabled = false;
            this.txtARC.Location = new System.Drawing.Point(225, 217);
            this.txtARC.MaxLength = 5;
            this.txtARC.Name = "txtARC";
            this.txtARC.Size = new System.Drawing.Size(72, 20);
            this.txtARC.TabIndex = 13;
            this.txtARC.Text = "ARC";
            this.txtARC.Click += new System.EventHandler(this.txtARC_Click);
            this.txtARC.Enter += new System.EventHandler(this.txtARC_Enter);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 215);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(185, 23);
            this.label4.TabIndex = 12;
            this.label4.Text = "Compass Comments Setup";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkCompassComments
            // 
            this.chkCompassComments.AutoSize = true;
            this.chkCompassComments.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.campaignDataBindingSource, "AddCommentsToCompass", true));
            this.chkCompassComments.Location = new System.Drawing.Point(204, 220);
            this.chkCompassComments.Name = "chkCompassComments";
            this.chkCompassComments.Size = new System.Drawing.Size(15, 14);
            this.chkCompassComments.TabIndex = 11;
            this.chkCompassComments.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.chkCompassComments.UseVisualStyleBackColor = true;
            this.chkCompassComments.CheckedChanged += new System.EventHandler(this.chkCompassComments_CheckedChanged);
            // 
            // txtComments
            // 
            this.txtComments.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.campaignDataBindingSource, "Comment", true));
            this.txtComments.Location = new System.Drawing.Point(204, 241);
            this.txtComments.MaxLength = 200;
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.Size = new System.Drawing.Size(269, 107);
            this.txtComments.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 237);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(185, 23);
            this.label5.TabIndex = 15;
            this.label5.Text = "Comment";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(4, -1);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(570, 36);
            this.label6.TabIndex = 16;
            this.label6.Text = "Before trying to run this script against a letter be sure all needed merge fields" +
                " have been set up in the letter.";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(249, 358);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 17;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(153, 358);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 18;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(344, 358);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(8, 86);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(185, 23);
            this.label7.TabIndex = 21;
            this.label7.Text = "Letter ID (From Letter Tracking)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLetterID
            // 
            this.txtLetterID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.campaignDataBindingSource, "LetterIDFromLTS", true));
            this.txtLetterID.Location = new System.Drawing.Point(204, 90);
            this.txtLetterID.Name = "txtLetterID";
            this.txtLetterID.Size = new System.Drawing.Size(93, 20);
            this.txtLetterID.TabIndex = 20;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 112);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(185, 23);
            this.label8.TabIndex = 23;
            this.label8.Text = "Letter Recipient";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbLetterRecipient
            // 
            this.cmbLetterRecipient.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.campaignDataBindingSource, "Recipient", true));
            this.cmbLetterRecipient.FormattingEnabled = true;
            this.cmbLetterRecipient.Location = new System.Drawing.Point(204, 114);
            this.cmbLetterRecipient.Name = "cmbLetterRecipient";
            this.cmbLetterRecipient.Size = new System.Drawing.Size(269, 21);
            this.cmbLetterRecipient.TabIndex = 24;
            // 
            // cmbPageCountAndDestination
            // 
            this.cmbPageCountAndDestination.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.campaignDataBindingSource, "PageCountOrDestination", true));
            this.cmbPageCountAndDestination.FormattingEnabled = true;
            this.cmbPageCountAndDestination.Location = new System.Drawing.Point(204, 140);
            this.cmbPageCountAndDestination.Name = "cmbPageCountAndDestination";
            this.cmbPageCountAndDestination.Size = new System.Drawing.Size(269, 21);
            this.cmbPageCountAndDestination.TabIndex = 26;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(8, 138);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(185, 23);
            this.label9.TabIndex = 25;
            this.label9.Text = "Page Count/Destination";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(11, 163);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(182, 23);
            this.label10.TabIndex = 28;
            this.label10.Text = "Letter Description (For Cover Sheet)";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLetterDescription
            // 
            this.txtLetterDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.campaignDataBindingSource, "LetterDescriptionForCCCCoverSheet", true));
            this.txtLetterDescription.Location = new System.Drawing.Point(204, 165);
            this.txtLetterDescription.MaxLength = 50;
            this.txtLetterDescription.Name = "txtLetterDescription";
            this.txtLetterDescription.Size = new System.Drawing.Size(269, 20);
            this.txtLetterDescription.TabIndex = 27;
            // 
            // campaignDataBindingSource
            // 
            this.campaignDataBindingSource.DataSource = typeof(SPLTRCAMP.CampaignData);
            // 
            // Entry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 386);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtLetterDescription);
            this.Controls.Add(this.cmbPageCountAndDestination);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cmbLetterRecipient);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtLetterID);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtComments);
            this.Controls.Add(this.txtARC);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkCompassComments);
            this.Controls.Add(this.txtActionCode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkOneLINKComments);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtLetterFile);
            this.Controls.Add(this.btnLetterFileBrowse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMergeFile);
            this.Controls.Add(this.btnDataFileBrowse);
            this.Name = "Entry";
            this.Text = "Letter Campaign Entry";
            ((System.ComponentModel.ISupportInitialize)(this.campaignDataBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtMergeFile;
        private System.Windows.Forms.Button btnDataFileBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLetterFile;
        private System.Windows.Forms.Button btnLetterFileBrowse;
        private System.Windows.Forms.OpenFileDialog ofdBrowser;
        private System.Windows.Forms.CheckBox chkOneLINKComments;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtActionCode;
        private System.Windows.Forms.TextBox txtARC;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkCompassComments;
        private System.Windows.Forms.TextBox txtComments;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.BindingSource campaignDataBindingSource;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtLetterID;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbLetterRecipient;
        private System.Windows.Forms.ComboBox cmbPageCountAndDestination;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtLetterDescription;
    }
}
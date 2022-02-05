namespace TPERM
{
    partial class BorrowerEntry
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BorrowerEntry));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Cancel = new System.Windows.Forms.Button();
            this.Continue = new System.Windows.Forms.Button();
            this.AccountNumber = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SSN = new System.Windows.Forms.Label();
            this.FName = new System.Windows.Forms.Label();
            this.LName = new System.Windows.Forms.Label();
            this.Signedlbl = new System.Windows.Forms.Label();
            this.SignedBwr = new System.Windows.Forms.RadioButton();
            this.SignedCo = new System.Windows.Forms.RadioButton();
            this.SignedPOA = new System.Windows.Forms.RadioButton();
            this.SignedNone = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.RefFName = new Uheaa.Common.WinForms.AlphaTextBox();
            this.RefLName = new Uheaa.Common.WinForms.AlphaTextBox();
            this.Relationship = new System.Windows.Forms.ComboBox();
            this.RefMName = new Uheaa.Common.WinForms.AlphaTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.DiffForm = new System.Windows.Forms.CheckBox();
            this.NameNoMatch = new System.Windows.Forms.CheckBox();
            this.Illegible = new System.Windows.Forms.CheckBox();
            this.CoMakerInfoBtn = new System.Windows.Forms.Button();
            this.Corr = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.Multi = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Account Number:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(80, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "First Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(76, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = " Last Name:";
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.Location = new System.Drawing.Point(541, 483);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(86, 35);
            this.Cancel.TabIndex = 4;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Continue
            // 
            this.Continue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Continue.Location = new System.Drawing.Point(444, 483);
            this.Continue.Name = "Continue";
            this.Continue.Size = new System.Drawing.Size(86, 35);
            this.Continue.TabIndex = 3;
            this.Continue.Text = "Continue";
            this.Continue.UseVisualStyleBackColor = true;
            this.Continue.Click += new System.EventHandler(this.Continue_Click);
            // 
            // AccountNumber
            // 
            this.AccountNumber.AutoSize = true;
            this.AccountNumber.Location = new System.Drawing.Point(176, 30);
            this.AccountNumber.Name = "AccountNumber";
            this.AccountNumber.Size = new System.Drawing.Size(124, 20);
            this.AccountNumber.TabIndex = 1;
            this.AccountNumber.Text = "AccountNumber";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(124, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 20);
            this.label5.TabIndex = 3;
            this.label5.Text = "SSN:";
            // 
            // SSN
            // 
            this.SSN.AutoSize = true;
            this.SSN.Location = new System.Drawing.Point(176, 60);
            this.SSN.Name = "SSN";
            this.SSN.Size = new System.Drawing.Size(42, 20);
            this.SSN.TabIndex = 4;
            this.SSN.Text = "SSN";
            // 
            // FName
            // 
            this.FName.AutoSize = true;
            this.FName.Location = new System.Drawing.Point(176, 90);
            this.FName.Name = "FName";
            this.FName.Size = new System.Drawing.Size(82, 20);
            this.FName.TabIndex = 6;
            this.FName.Text = "FirstName";
            // 
            // LName
            // 
            this.LName.AutoSize = true;
            this.LName.Location = new System.Drawing.Point(176, 120);
            this.LName.Name = "LName";
            this.LName.Size = new System.Drawing.Size(82, 20);
            this.LName.TabIndex = 8;
            this.LName.Text = "LastName";
            // 
            // Signedlbl
            // 
            this.Signedlbl.AutoSize = true;
            this.Signedlbl.Location = new System.Drawing.Point(80, 185);
            this.Signedlbl.Name = "Signedlbl";
            this.Signedlbl.Size = new System.Drawing.Size(85, 20);
            this.Signedlbl.TabIndex = 11;
            this.Signedlbl.Text = "Signed By:";
            // 
            // SignedBwr
            // 
            this.SignedBwr.AutoSize = true;
            this.SignedBwr.Location = new System.Drawing.Point(171, 183);
            this.SignedBwr.Name = "SignedBwr";
            this.SignedBwr.Size = new System.Drawing.Size(91, 24);
            this.SignedBwr.TabIndex = 12;
            this.SignedBwr.TabStop = true;
            this.SignedBwr.Text = "Borrower";
            this.SignedBwr.UseVisualStyleBackColor = true;
            // 
            // SignedCo
            // 
            this.SignedCo.AutoSize = true;
            this.SignedCo.Enabled = false;
            this.SignedCo.Location = new System.Drawing.Point(272, 183);
            this.SignedCo.Name = "SignedCo";
            this.SignedCo.Size = new System.Drawing.Size(96, 24);
            this.SignedCo.TabIndex = 13;
            this.SignedCo.TabStop = true;
            this.SignedCo.Text = "Co-Maker";
            this.SignedCo.UseVisualStyleBackColor = true;
            // 
            // SignedPOA
            // 
            this.SignedPOA.AutoSize = true;
            this.SignedPOA.Location = new System.Drawing.Point(374, 183);
            this.SignedPOA.Name = "SignedPOA";
            this.SignedPOA.Size = new System.Drawing.Size(153, 24);
            this.SignedPOA.TabIndex = 14;
            this.SignedPOA.TabStop = true;
            this.SignedPOA.Text = "Power of Attorney";
            this.SignedPOA.UseVisualStyleBackColor = true;
            // 
            // SignedNone
            // 
            this.SignedNone.AutoSize = true;
            this.SignedNone.Location = new System.Drawing.Point(533, 183);
            this.SignedNone.Name = "SignedNone";
            this.SignedNone.Size = new System.Drawing.Size(67, 24);
            this.SignedNone.TabIndex = 15;
            this.SignedNone.TabStop = true;
            this.SignedNone.Text = "Other";
            this.SignedNone.UseVisualStyleBackColor = true;
            this.SignedNone.CheckedChanged += new System.EventHandler(this.SignedNone_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(80, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "First Name:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(80, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 20);
            this.label6.TabIndex = 4;
            this.label6.Text = "Last Name:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(69, 124);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 20);
            this.label7.TabIndex = 6;
            this.label7.Text = "Relationship:";
            // 
            // RefFName
            // 
            this.RefFName.AllowedSpecialCharacters = " ";
            this.RefFName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RefFName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.RefFName.Location = new System.Drawing.Point(176, 28);
            this.RefFName.MaxLength = 13;
            this.RefFName.Name = "RefFName";
            this.RefFName.Size = new System.Drawing.Size(286, 26);
            this.RefFName.TabIndex = 1;
            // 
            // RefLName
            // 
            this.RefLName.AllowedSpecialCharacters = " ";
            this.RefLName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RefLName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.RefLName.Location = new System.Drawing.Point(176, 90);
            this.RefLName.MaxLength = 23;
            this.RefLName.Name = "RefLName";
            this.RefLName.Size = new System.Drawing.Size(286, 26);
            this.RefLName.TabIndex = 5;
            // 
            // Relationship
            // 
            this.Relationship.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Relationship.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.Relationship.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.Relationship.FormattingEnabled = true;
            this.Relationship.Location = new System.Drawing.Point(176, 121);
            this.Relationship.Name = "Relationship";
            this.Relationship.Size = new System.Drawing.Size(286, 28);
            this.Relationship.TabIndex = 7;
            // 
            // RefMName
            // 
            this.RefMName.AllowedSpecialCharacters = " ";
            this.RefMName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RefMName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.RefMName.Location = new System.Drawing.Point(176, 59);
            this.RefMName.MaxLength = 13;
            this.RefMName.Name = "RefMName";
            this.RefMName.Size = new System.Drawing.Size(286, 26);
            this.RefMName.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(63, 62);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 20);
            this.label8.TabIndex = 2;
            this.label8.Text = "Middle Name:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.Relationship);
            this.groupBox1.Controls.Add(this.RefMName);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.RefLName);
            this.groupBox1.Controls.Add(this.RefFName);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(3, 314);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(630, 163);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Reference Information";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.DiffForm);
            this.groupBox2.Controls.Add(this.NameNoMatch);
            this.groupBox2.Controls.Add(this.Illegible);
            this.groupBox2.Controls.Add(this.CoMakerInfoBtn);
            this.groupBox2.Controls.Add(this.Corr);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.SignedNone);
            this.groupBox2.Controls.Add(this.AccountNumber);
            this.groupBox2.Controls.Add(this.SignedPOA);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.SignedCo);
            this.groupBox2.Controls.Add(this.SSN);
            this.groupBox2.Controls.Add(this.SignedBwr);
            this.groupBox2.Controls.Add(this.FName);
            this.groupBox2.Controls.Add(this.Signedlbl);
            this.groupBox2.Controls.Add(this.LName);
            this.groupBox2.Location = new System.Drawing.Point(3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(631, 307);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Application Information";
            // 
            // button1
            // 
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(343, 150);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(20, 20);
            this.button1.TabIndex = 21;
            this.toolTip1.SetToolTip(this.button1, "Copy");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DiffForm
            // 
            this.DiffForm.AutoSize = true;
            this.DiffForm.Location = new System.Drawing.Point(171, 277);
            this.DiffForm.Name = "DiffForm";
            this.DiffForm.Size = new System.Drawing.Size(338, 24);
            this.DiffForm.TabIndex = 20;
            this.DiffForm.Text = "Authorization form from a different company";
            this.DiffForm.UseVisualStyleBackColor = true;
            this.DiffForm.CheckedChanged += new System.EventHandler(this.DiffForm_CheckedChanged);
            // 
            // NameNoMatch
            // 
            this.NameNoMatch.AutoSize = true;
            this.NameNoMatch.Location = new System.Drawing.Point(171, 247);
            this.NameNoMatch.Name = "NameNoMatch";
            this.NameNoMatch.Size = new System.Drawing.Size(329, 24);
            this.NameNoMatch.TabIndex = 19;
            this.NameNoMatch.Text = "Name on form does not match Bwr/Co-Bwr";
            this.NameNoMatch.UseVisualStyleBackColor = true;
            this.NameNoMatch.CheckedChanged += new System.EventHandler(this.NameNoMatch_CheckedChanged);
            // 
            // Illegible
            // 
            this.Illegible.AutoSize = true;
            this.Illegible.Location = new System.Drawing.Point(171, 217);
            this.Illegible.Name = "Illegible";
            this.Illegible.Size = new System.Drawing.Size(135, 24);
            this.Illegible.TabIndex = 18;
            this.Illegible.Text = "Form is illegible";
            this.Illegible.UseVisualStyleBackColor = true;
            this.Illegible.CheckedChanged += new System.EventHandler(this.Illegible_CheckedChanged);
            // 
            // CoMakerInfoBtn
            // 
            this.CoMakerInfoBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CoMakerInfoBtn.Enabled = false;
            this.CoMakerInfoBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CoMakerInfoBtn.Location = new System.Drawing.Point(469, 23);
            this.CoMakerInfoBtn.Name = "CoMakerInfoBtn";
            this.CoMakerInfoBtn.Size = new System.Drawing.Size(155, 27);
            this.CoMakerInfoBtn.TabIndex = 2;
            this.CoMakerInfoBtn.Text = "Show Co-Maker Info";
            this.CoMakerInfoBtn.UseVisualStyleBackColor = true;
            this.CoMakerInfoBtn.Click += new System.EventHandler(this.CoMakerInfo_Click);
            // 
            // Corr
            // 
            this.Corr.AutoSize = true;
            this.Corr.Location = new System.Drawing.Point(176, 150);
            this.Corr.Name = "Corr";
            this.Corr.Size = new System.Drawing.Size(135, 20);
            this.Corr.TabIndex = 10;
            this.Corr.Text = "CORR DOC NUM";
            this.Corr.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Corr_MouseClick);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(57, 150);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(113, 20);
            this.label9.TabIndex = 9;
            this.label9.Text = "Corr Doc Num:";
            // 
            // Multi
            // 
            this.Multi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Multi.AutoSize = true;
            this.Multi.Location = new System.Drawing.Point(12, 499);
            this.Multi.Name = "Multi";
            this.Multi.Size = new System.Drawing.Size(204, 24);
            this.Multi.TabIndex = 2;
            this.Multi.Text = "Add multiple references?";
            this.Multi.UseVisualStyleBackColor = true;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // BorrowerEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 530);
            this.Controls.Add(this.Multi);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Continue);
            this.Controls.Add(this.Cancel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(655, 569);
            this.Name = "BorrowerEntry";
            this.Text = "Enter Borrower Information";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Continue;
        private System.Windows.Forms.Label AccountNumber;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label SSN;
        private System.Windows.Forms.Label FName;
        private System.Windows.Forms.Label LName;
        private System.Windows.Forms.Label Signedlbl;
        private System.Windows.Forms.RadioButton SignedBwr;
        private System.Windows.Forms.RadioButton SignedCo;
        private System.Windows.Forms.RadioButton SignedPOA;
        private System.Windows.Forms.RadioButton SignedNone;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private Uheaa.Common.WinForms.AlphaTextBox RefFName;
        private Uheaa.Common.WinForms.AlphaTextBox RefLName;
        private System.Windows.Forms.ComboBox Relationship;
        private Uheaa.Common.WinForms.AlphaTextBox RefMName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label Corr;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox Multi;
        private System.Windows.Forms.Button CoMakerInfoBtn;
        private System.Windows.Forms.CheckBox Illegible;
        private System.Windows.Forms.CheckBox NameNoMatch;
        private System.Windows.Forms.CheckBox DiffForm;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
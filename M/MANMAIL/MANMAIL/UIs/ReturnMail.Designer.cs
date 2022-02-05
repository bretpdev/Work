namespace MANMAIL
{
    partial class ReturnMail
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
            this.label1 = new System.Windows.Forms.Label();
            this.WhoAMI = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.CreateDate = new System.Windows.Forms.MaskedTextBox();
            this.ReturnDate = new System.Windows.Forms.MaskedTextBox();
            this.LetterCode = new System.Windows.Forms.ComboBox();
            this.Reasons = new System.Windows.Forms.ComboBox();
            this.Cancel = new Uheaa.Common.WinForms.ValidationButton();
            this.Enter = new Uheaa.Common.WinForms.ValidationButton();
            this.AccountIdentifer = new Uheaa.Common.WinForms.RequiredTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(236, 47);
            this.label1.TabIndex = 0;
            this.label1.Text = "ACS Encryption Code / SSN / AccountNumber / Reference ID";
            // 
            // WhoAMI
            // 
            this.WhoAMI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.WhoAMI.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WhoAMI.Location = new System.Drawing.Point(357, 116);
            this.WhoAMI.Name = "WhoAMI";
            this.WhoAMI.Size = new System.Drawing.Size(84, 25);
            this.WhoAMI.TabIndex = 7;
            this.WhoAMI.Text = "Who Am I?";
            this.WhoAMI.UseVisualStyleBackColor = true;
            this.WhoAMI.Click += new System.EventHandler(this.WhoAMI_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(151, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Letter Code:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(147, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Create Date:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(147, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "Return Date:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(98, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(150, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "Reason For Return:";
            // 
            // CreateDate
            // 
            this.CreateDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CreateDate.Location = new System.Drawing.Point(254, 99);
            this.CreateDate.Mask = "00/00/0000";
            this.CreateDate.Name = "CreateDate";
            this.CreateDate.Size = new System.Drawing.Size(87, 26);
            this.CreateDate.TabIndex = 2;
            this.CreateDate.ValidatingType = typeof(System.DateTime);
            this.CreateDate.TextChanged += new System.EventHandler(this.CreateDate_TextChanged);
            // 
            // ReturnDate
            // 
            this.ReturnDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReturnDate.Location = new System.Drawing.Point(254, 133);
            this.ReturnDate.Mask = "00/00/0000";
            this.ReturnDate.Name = "ReturnDate";
            this.ReturnDate.Size = new System.Drawing.Size(87, 26);
            this.ReturnDate.TabIndex = 3;
            this.ReturnDate.ValidatingType = typeof(System.DateTime);
            this.ReturnDate.TextChanged += new System.EventHandler(this.ReturnDate_TextChanged);
            // 
            // LetterCode
            // 
            this.LetterCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LetterCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.LetterCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.LetterCode.FormattingEnabled = true;
            this.LetterCode.Location = new System.Drawing.Point(254, 65);
            this.LetterCode.Name = "LetterCode";
            this.LetterCode.Size = new System.Drawing.Size(156, 28);
            this.LetterCode.TabIndex = 1;
            // 
            // Reasons
            // 
            this.Reasons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Reasons.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.Reasons.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.Reasons.DropDownWidth = 300;
            this.Reasons.FormattingEnabled = true;
            this.Reasons.Location = new System.Drawing.Point(254, 167);
            this.Reasons.Name = "Reasons";
            this.Reasons.Size = new System.Drawing.Size(241, 28);
            this.Reasons.TabIndex = 4;
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(131, 219);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(84, 41);
            this.Cancel.TabIndex = 6;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // Enter
            // 
            this.Enter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Enter.Location = new System.Drawing.Point(311, 219);
            this.Enter.Name = "Enter";
            this.Enter.Size = new System.Drawing.Size(84, 41);
            this.Enter.TabIndex = 5;
            this.Enter.Text = "Enter";
            this.Enter.UseVisualStyleBackColor = true;
            this.Enter.Click += new System.EventHandler(this.Enter_Click);
            // 
            // AccountIdentifer
            // 
            this.AccountIdentifer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccountIdentifer.Location = new System.Drawing.Point(254, 31);
            this.AccountIdentifer.MaxLength = 10;
            this.AccountIdentifer.Name = "AccountIdentifer";
            this.AccountIdentifer.Size = new System.Drawing.Size(201, 26);
            this.AccountIdentifer.TabIndex = 0;
            // 
            // ReturnMail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(507, 279);
            this.Controls.Add(this.Reasons);
            this.Controls.Add(this.LetterCode);
            this.Controls.Add(this.ReturnDate);
            this.Controls.Add(this.CreateDate);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Enter);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.WhoAMI);
            this.Controls.Add(this.AccountIdentifer);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(482, 317);
            this.Name = "ReturnMail";
            this.ShowIcon = false;
            this.Text = "ReturnMail";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Uheaa.Common.WinForms.RequiredTextBox AccountIdentifer;
        private System.Windows.Forms.Button WhoAMI;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private Uheaa.Common.WinForms.ValidationButton Enter;
        private Uheaa.Common.WinForms.ValidationButton Cancel;
        private System.Windows.Forms.MaskedTextBox CreateDate;
        private System.Windows.Forms.MaskedTextBox ReturnDate;
        private System.Windows.Forms.ComboBox LetterCode;
        private System.Windows.Forms.ComboBox Reasons;
    }
}
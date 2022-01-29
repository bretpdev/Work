namespace Uheaa.Common.WinForms
{
    partial class ArcAddProcessingUI  
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
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.IsReference = new System.Windows.Forms.CheckBox();
            this.IsEndorser = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.Comment = new System.Windows.Forms.TextBox();
            this.ProcessOn = new System.Windows.Forms.DateTimePicker();
            this.RegardsCode = new System.Windows.Forms.ComboBox();
            this.Cancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ArcTypeSelection = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.RecipientId = new System.Windows.Forms.TextBox();
            this.RegardsTo = new System.Windows.Forms.TextBox();
            this.NeededBy = new System.Windows.Forms.MaskedTextBox();
            this.ProcessFrom = new System.Windows.Forms.MaskedTextBox();
            this.ProcessTo = new System.Windows.Forms.MaskedTextBox();
            this.OK = new Uheaa.Common.WinForms.ValidationButton();
            this.ScriptId = new Uheaa.Common.WinForms.RequiredTextBox();
            this.ARC = new Uheaa.Common.WinForms.RequiredTextBox();
            this.AccountIdentifer = new Uheaa.Common.WinForms.RequiredNumericTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "*Account Number/SSN:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(97, 329);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Recipient Id:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(113, 145);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "Comment:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(94, 297);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "*Process On:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(121, 266);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "*Script Id:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(146, 51);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 20);
            this.label7.TabIndex = 6;
            this.label7.Text = "*ARC";
            // 
            // IsReference
            // 
            this.IsReference.AutoSize = true;
            this.IsReference.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.IsReference.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsReference.Location = new System.Drawing.Point(344, 358);
            this.IsReference.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.IsReference.Name = "IsReference";
            this.IsReference.Size = new System.Drawing.Size(124, 24);
            this.IsReference.TabIndex = 7;
            this.IsReference.Text = "Is Reference:";
            this.IsReference.UseVisualStyleBackColor = true;
            // 
            // IsEndorser
            // 
            this.IsEndorser.AutoSize = true;
            this.IsEndorser.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.IsEndorser.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsEndorser.Location = new System.Drawing.Point(344, 327);
            this.IsEndorser.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.IsEndorser.Name = "IsEndorser";
            this.IsEndorser.Size = new System.Drawing.Size(110, 24);
            this.IsEndorser.TabIndex = 8;
            this.IsEndorser.Text = "Is Endorser";
            this.IsEndorser.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(84, 458);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(111, 20);
            this.label8.TabIndex = 9;
            this.label8.Text = "Process From:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(103, 490);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 20);
            this.label9.TabIndex = 10;
            this.label9.Text = "Process To:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(99, 359);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 20);
            this.label10.TabIndex = 11;
            this.label10.Text = "Regards To:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(79, 391);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(116, 20);
            this.label11.TabIndex = 12;
            this.label11.Text = "Regards Code:";
            // 
            // Comment
            // 
            this.Comment.Location = new System.Drawing.Point(202, 79);
            this.Comment.MaxLength = 300;
            this.Comment.Multiline = true;
            this.Comment.Name = "Comment";
            this.Comment.Size = new System.Drawing.Size(298, 141);
            this.Comment.TabIndex = 15;
            // 
            // ProcessOn
            // 
            this.ProcessOn.Location = new System.Drawing.Point(202, 292);
            this.ProcessOn.Name = "ProcessOn";
            this.ProcessOn.Size = new System.Drawing.Size(298, 26);
            this.ProcessOn.TabIndex = 17;
            // 
            // RegardsCode
            // 
            this.RegardsCode.FormattingEnabled = true;
            this.RegardsCode.Location = new System.Drawing.Point(202, 388);
            this.RegardsCode.Name = "RegardsCode";
            this.RegardsCode.Size = new System.Drawing.Size(121, 28);
            this.RegardsCode.TabIndex = 20;
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(307, 531);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(103, 35);
            this.Cancel.TabIndex = 24;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(20, 546);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(185, 20);
            this.label3.TabIndex = 25;
            this.label3.Text = "* denotes a required field";
            // 
            // ArcTypeSelection
            // 
            this.ArcTypeSelection.FormattingEnabled = true;
            this.ArcTypeSelection.Location = new System.Drawing.Point(202, 226);
            this.ArcTypeSelection.Name = "ArcTypeSelection";
            this.ArcTypeSelection.Size = new System.Drawing.Size(296, 28);
            this.ArcTypeSelection.TabIndex = 26;
            this.ArcTypeSelection.SelectionChangeCommitted += new System.EventHandler(this.ArcTypeSelection_SelectionChangeCommitted);
            this.ArcTypeSelection.Click += new System.EventHandler(this.ArcTypeSelection_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(114, 229);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(81, 20);
            this.label12.TabIndex = 27;
            this.label12.Text = "*Arc Type:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(108, 425);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(91, 20);
            this.label13.TabIndex = 29;
            this.label13.Text = "Needed By:";
            // 
            // RecipientId
            // 
            this.RecipientId.Location = new System.Drawing.Point(202, 326);
            this.RecipientId.Name = "RecipientId";
            this.RecipientId.Size = new System.Drawing.Size(100, 26);
            this.RecipientId.TabIndex = 30;
            // 
            // RegardsTo
            // 
            this.RegardsTo.Location = new System.Drawing.Point(202, 358);
            this.RegardsTo.Name = "RegardsTo";
            this.RegardsTo.Size = new System.Drawing.Size(100, 26);
            this.RegardsTo.TabIndex = 31;
            // 
            // NeededBy
            // 
            this.NeededBy.Location = new System.Drawing.Point(202, 422);
            this.NeededBy.Mask = "00/00/0000";
            this.NeededBy.Name = "NeededBy";
            this.NeededBy.Size = new System.Drawing.Size(100, 26);
            this.NeededBy.TabIndex = 32;
            this.NeededBy.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ProcessFrom
            // 
            this.ProcessFrom.Location = new System.Drawing.Point(202, 456);
            this.ProcessFrom.Mask = "00/00/0000";
            this.ProcessFrom.Name = "ProcessFrom";
            this.ProcessFrom.Size = new System.Drawing.Size(100, 26);
            this.ProcessFrom.TabIndex = 33;
            this.ProcessFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ProcessTo
            // 
            this.ProcessTo.Location = new System.Drawing.Point(202, 489);
            this.ProcessTo.Mask = "00/00/0000";
            this.ProcessTo.Name = "ProcessTo";
            this.ProcessTo.Size = new System.Drawing.Size(100, 26);
            this.ProcessTo.TabIndex = 34;
            this.ProcessTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(416, 531);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(103, 35);
            this.OK.TabIndex = 23;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.OnValidate += new Uheaa.Common.WinForms.ValidationButton.ValidationHandler(this.OK_OnValidate);
            this.OK.Click += new System.EventHandler(this.validationButton1_Click);
            // 
            // ScriptId
            // 
            this.ScriptId.Location = new System.Drawing.Point(202, 260);
            this.ScriptId.MaxLength = 10;
            this.ScriptId.Name = "ScriptId";
            this.ScriptId.Size = new System.Drawing.Size(152, 26);
            this.ScriptId.TabIndex = 16;
            // 
            // ARC
            // 
            this.ARC.Location = new System.Drawing.Point(202, 48);
            this.ARC.MaxLength = 5;
            this.ARC.Name = "ARC";
            this.ARC.Size = new System.Drawing.Size(79, 26);
            this.ARC.TabIndex = 14;
            this.ARC.Leave += new System.EventHandler(this.ARC_Leave);
            // 
            // AccountIdentifer
            // 
            this.AccountIdentifer.AllowedSpecialCharacters = "";
            this.AccountIdentifer.Location = new System.Drawing.Point(202, 17);
            this.AccountIdentifer.MaxLength = 10;
            this.AccountIdentifer.Name = "AccountIdentifer";
            this.AccountIdentifer.Size = new System.Drawing.Size(169, 26);
            this.AccountIdentifer.TabIndex = 13;
            this.AccountIdentifer.Leave += new System.EventHandler(this.AccountIdentifer_Leave);
            // 
            // ArcAddProcessingUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 576);
            this.Controls.Add(this.ProcessTo);
            this.Controls.Add(this.ProcessFrom);
            this.Controls.Add(this.NeededBy);
            this.Controls.Add(this.RegardsTo);
            this.Controls.Add(this.RecipientId);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.ArcTypeSelection);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.RegardsCode);
            this.Controls.Add(this.ProcessOn);
            this.Controls.Add(this.ScriptId);
            this.Controls.Add(this.Comment);
            this.Controls.Add(this.ARC);
            this.Controls.Add(this.AccountIdentifer);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.IsEndorser);
            this.Controls.Add(this.IsReference);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ArcAddProcessingUI";
            this.ShowIcon = false;
            this.Text = "ArcAddProcessingUI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox IsReference;
        private System.Windows.Forms.CheckBox IsEndorser;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private WinForms.RequiredNumericTextBox AccountIdentifer;
        private WinForms.RequiredTextBox ARC;
        private System.Windows.Forms.TextBox Comment;
        private WinForms.RequiredTextBox ScriptId;
        private System.Windows.Forms.DateTimePicker ProcessOn;
        private System.Windows.Forms.ComboBox RegardsCode;
        private WinForms.ValidationButton OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ArcTypeSelection;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox RecipientId;
        private System.Windows.Forms.TextBox RegardsTo;
        private System.Windows.Forms.MaskedTextBox NeededBy;
        private System.Windows.Forms.MaskedTextBox ProcessFrom;
        private System.Windows.Forms.MaskedTextBox ProcessTo;
    }
}
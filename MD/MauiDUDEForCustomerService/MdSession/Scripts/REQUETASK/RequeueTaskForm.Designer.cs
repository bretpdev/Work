namespace REQUETASK
{
    partial class RequeueTaskForm
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
            this.label3 = new System.Windows.Forms.Label();
            this.ForBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RequeueTextBox = new System.Windows.Forms.TextBox();
            this.ConfirmButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.DateBox = new Uheaa.Common.WinForms.OmniTextBox();
            this.ArcDescBox = new Uheaa.Common.WinForms.OmniTextBox();
            this.ArcBox = new Uheaa.Common.WinForms.OmniTextBox();
            this.SsnBox = new Uheaa.Common.WinForms.OmniTextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(79, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "SSN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(78, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "ARC";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "ARC Desc";
            // 
            // ForBox
            // 
            this.ForBox.FormattingEnabled = true;
            this.ForBox.Location = new System.Drawing.Point(122, 108);
            this.ForBox.Name = "ForBox";
            this.ForBox.Size = new System.Drawing.Size(238, 26);
            this.ForBox.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "Performing For";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 18);
            this.label5.TabIndex = 8;
            this.label5.Text = "Re-Queue Date";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RequeueTextBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 174);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(348, 141);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Re-Queue Text";
            // 
            // RequeueTextBox
            // 
            this.RequeueTextBox.Location = new System.Drawing.Point(6, 25);
            this.RequeueTextBox.Multiline = true;
            this.RequeueTextBox.Name = "RequeueTextBox";
            this.RequeueTextBox.Size = new System.Drawing.Size(336, 110);
            this.RequeueTextBox.TabIndex = 0;
            // 
            // ConfirmButton
            // 
            this.ConfirmButton.Enabled = false;
            this.ConfirmButton.Location = new System.Drawing.Point(236, 321);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.Size = new System.Drawing.Size(124, 47);
            this.ConfirmButton.TabIndex = 11;
            this.ConfirmButton.Text = "Re-Queue";
            this.ConfirmButton.UseVisualStyleBackColor = true;
            this.ConfirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(12, 321);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(124, 47);
            this.CancelButton.TabIndex = 12;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // DateBox
            // 
            this.DateBox.AllowAllCharacters = false;
            this.DateBox.AllowAlphaCharacters = false;
            this.DateBox.AllowedAdditionalCharacters = "";
            this.DateBox.AllowNumericCharacters = true;
            this.DateBox.InvalidColor = System.Drawing.Color.LightPink;
            this.DateBox.Location = new System.Drawing.Point(123, 141);
            this.DateBox.Margin = new System.Windows.Forms.Padding(4);
            this.DateBox.Mask = "__/__/____";
            this.DateBox.MaxLength = 10;
            this.DateBox.Name = "DateBox";
            this.DateBox.Size = new System.Drawing.Size(237, 26);
            this.DateBox.TabIndex = 9;
            this.DateBox.ValidationMessage = null;
            this.DateBox.ValidColor = System.Drawing.Color.LightGreen;
            this.DateBox.ValidationOnLeave += new Uheaa.Common.WinForms.ValidationEventHandler(this.DateBox_ValidationOnLeave);
            // 
            // ArcDescBox
            // 
            this.ArcDescBox.AllowAllCharacters = false;
            this.ArcDescBox.AllowAlphaCharacters = true;
            this.ArcDescBox.AllowedAdditionalCharacters = "";
            this.ArcDescBox.AllowNumericCharacters = false;
            this.ArcDescBox.InvalidColor = System.Drawing.Color.LightPink;
            this.ArcDescBox.Location = new System.Drawing.Point(122, 75);
            this.ArcDescBox.Margin = new System.Windows.Forms.Padding(4);
            this.ArcDescBox.Mask = "";
            this.ArcDescBox.Name = "ArcDescBox";
            this.ArcDescBox.ReadOnly = true;
            this.ArcDescBox.Size = new System.Drawing.Size(237, 26);
            this.ArcDescBox.TabIndex = 4;
            this.ArcDescBox.ValidationMessage = null;
            this.ArcDescBox.ValidColor = System.Drawing.Color.LightGreen;
            // 
            // ArcBox
            // 
            this.ArcBox.AllowAllCharacters = false;
            this.ArcBox.AllowAlphaCharacters = true;
            this.ArcBox.AllowedAdditionalCharacters = "";
            this.ArcBox.AllowNumericCharacters = true;
            this.ArcBox.InvalidColor = System.Drawing.Color.LightPink;
            this.ArcBox.Location = new System.Drawing.Point(122, 41);
            this.ArcBox.Margin = new System.Windows.Forms.Padding(4);
            this.ArcBox.Mask = "";
            this.ArcBox.MaxLength = 9;
            this.ArcBox.Name = "ArcBox";
            this.ArcBox.Size = new System.Drawing.Size(237, 26);
            this.ArcBox.TabIndex = 2;
            this.ArcBox.ValidationMessage = null;
            this.ArcBox.ValidColor = System.Drawing.Color.LightGreen;
            // 
            // SsnBox
            // 
            this.SsnBox.AllowAllCharacters = false;
            this.SsnBox.AllowAlphaCharacters = false;
            this.SsnBox.AllowedAdditionalCharacters = "";
            this.SsnBox.AllowNumericCharacters = true;
            this.SsnBox.InvalidColor = System.Drawing.Color.LightPink;
            this.SsnBox.Location = new System.Drawing.Point(122, 7);
            this.SsnBox.Margin = new System.Windows.Forms.Padding(4);
            this.SsnBox.Mask = "";
            this.SsnBox.MaxLength = 9;
            this.SsnBox.Name = "SsnBox";
            this.SsnBox.Size = new System.Drawing.Size(237, 26);
            this.SsnBox.TabIndex = 0;
            this.SsnBox.ValidationMessage = null;
            this.SsnBox.ValidColor = System.Drawing.Color.LightGreen;
            this.SsnBox.ValidationOnLeave += new Uheaa.Common.WinForms.ValidationEventHandler(this.SsnBox_ValidationOnLeave);
            this.SsnBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SsnBox_KeyPress);
            // 
            // RequeueTaskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 380);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.ConfirmButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.DateBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ForBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ArcDescBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ArcBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SsnBox);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "RequeueTaskForm";
            this.Text = "Re-Queue Task";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Uheaa.Common.WinForms.OmniTextBox SsnBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Uheaa.Common.WinForms.OmniTextBox ArcBox;
        private System.Windows.Forms.Label label3;
        private Uheaa.Common.WinForms.OmniTextBox ArcDescBox;
        private System.Windows.Forms.ComboBox ForBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private Uheaa.Common.WinForms.OmniTextBox DateBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox RequeueTextBox;
        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.Button CancelButton;
    }
}
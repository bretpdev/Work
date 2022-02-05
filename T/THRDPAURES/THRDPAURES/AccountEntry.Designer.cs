namespace THRDPAURES
{
    partial class AccountEntry
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
            this.POA = new System.Windows.Forms.RadioButton();
            this.TPA = new System.Windows.Forms.RadioButton();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.RefLastName = new Uheaa.Common.WinForms.RequiredTextBox();
            this.RefFirstName = new Uheaa.Common.WinForms.RequiredTextBox();
            this.AccountIdentifier = new Uheaa.Common.WinForms.RequiredNumericTextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Account Number/SSN:";
            // 
            // POA
            // 
            this.POA.AutoSize = true;
            this.POA.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.POA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.POA.Location = new System.Drawing.Point(0, 24);
            this.POA.Name = "POA";
            this.POA.Size = new System.Drawing.Size(215, 24);
            this.POA.TabIndex = 4;
            this.POA.TabStop = true;
            this.POA.Text = "Power Of Attorney";
            this.POA.UseVisualStyleBackColor = true;
            this.POA.CheckedChanged += new System.EventHandler(this.POA_CheckedChanged);
            // 
            // TPA
            // 
            this.TPA.Dock = System.Windows.Forms.DockStyle.Top;
            this.TPA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TPA.Location = new System.Drawing.Point(0, 0);
            this.TPA.Name = "TPA";
            this.TPA.Size = new System.Drawing.Size(215, 24);
            this.TPA.TabIndex = 3;
            this.TPA.TabStop = true;
            this.TPA.Text = "Third Party Authorization";
            this.TPA.UseVisualStyleBackColor = true;
            this.TPA.CheckedChanged += new System.EventHandler(this.TPA_CheckedChanged);
            // 
            // OK
            // 
            this.OK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OK.Location = new System.Drawing.Point(233, 136);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 41);
            this.OK.TabIndex = 5;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel.Location = new System.Drawing.Point(314, 136);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 41);
            this.Cancel.TabIndex = 6;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.TPA);
            this.panel1.Controls.Add(this.POA);
            this.panel1.Location = new System.Drawing.Point(12, 129);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(215, 48);
            this.panel1.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(169, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Reference First Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Reference Last Name:";
            // 
            // RefLastName
            // 
            this.RefLastName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.RefLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RefLastName.Location = new System.Drawing.Point(188, 85);
            this.RefLastName.MaxLength = 23;
            this.RefLastName.Name = "RefLastName";
            this.RefLastName.Size = new System.Drawing.Size(201, 26);
            this.RefLastName.TabIndex = 2;
            // 
            // RefFirstName
            // 
            this.RefFirstName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.RefFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RefFirstName.Location = new System.Drawing.Point(188, 49);
            this.RefFirstName.MaxLength = 13;
            this.RefFirstName.Name = "RefFirstName";
            this.RefFirstName.Size = new System.Drawing.Size(201, 26);
            this.RefFirstName.TabIndex = 1;
            // 
            // AccountIdentifier
            // 
            this.AccountIdentifier.AllowedSpecialCharacters = "";
            this.AccountIdentifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AccountIdentifier.Location = new System.Drawing.Point(188, 13);
            this.AccountIdentifier.MaxLength = 10;
            this.AccountIdentifier.Name = "AccountIdentifier";
            this.AccountIdentifier.Size = new System.Drawing.Size(201, 26);
            this.AccountIdentifier.TabIndex = 0;
            // 
            // AccountEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 187);
            this.Controls.Add(this.RefLastName);
            this.Controls.Add(this.RefFirstName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AccountIdentifier);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AccountEntry";
            this.ShowIcon = false;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Uheaa.Common.WinForms.RequiredNumericTextBox AccountIdentifier;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton TPA;
        private System.Windows.Forms.RadioButton POA;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Uheaa.Common.WinForms.RequiredTextBox RefFirstName;
        private Uheaa.Common.WinForms.RequiredTextBox RefLastName;

    }
}
namespace LENDERLTRS
{
    partial class LenderForm
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
            this.Mod = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LenderId = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.StaOpen = new System.Windows.Forms.CheckBox();
            this.StaClosed = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.Street2 = new System.Windows.Forms.TextBox();
            this.LenderType = new System.Windows.Forms.ComboBox();
            this.Exit = new System.Windows.Forms.Button();
            this.Continue = new Uheaa.Common.WinForms.ValidationButton();
            this.ZipCode = new Uheaa.Common.WinForms.RequiredTextBox();
            this.State = new Uheaa.Common.WinForms.RequiredTextBox();
            this.City = new Uheaa.Common.WinForms.RequiredTextBox();
            this.Street1 = new Uheaa.Common.WinForms.RequiredTextBox();
            this.ShortName = new Uheaa.Common.WinForms.RequiredTextBox();
            this.FullName = new Uheaa.Common.WinForms.RequiredTextBox();
            this.Unlocated = new Uheaa.Common.WinForms.ValidationButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(85, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "MOD:";
            // 
            // Mod
            // 
            this.Mod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Mod.AutoSize = true;
            this.Mod.Location = new System.Drawing.Point(148, 8);
            this.Mod.Name = "Mod";
            this.Mod.Size = new System.Drawing.Size(20, 20);
            this.Mod.TabIndex = 1;
            this.Mod.Text = "A";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Lender ID:";
            // 
            // LenderId
            // 
            this.LenderId.AutoSize = true;
            this.LenderId.Location = new System.Drawing.Point(141, 41);
            this.LenderId.Name = "LenderId";
            this.LenderId.Size = new System.Drawing.Size(77, 20);
            this.LenderId.TabIndex = 3;
            this.LenderId.Text = "Lender Id";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(75, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Status:";
            // 
            // StaOpen
            // 
            this.StaOpen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StaOpen.AutoSize = true;
            this.StaOpen.Location = new System.Drawing.Point(141, 75);
            this.StaOpen.Name = "StaOpen";
            this.StaOpen.Size = new System.Drawing.Size(67, 24);
            this.StaOpen.TabIndex = 5;
            this.StaOpen.Text = "Open";
            this.StaOpen.UseVisualStyleBackColor = true;
            this.StaOpen.CheckedChanged += new System.EventHandler(this.StaOpen_CheckedChanged);
            // 
            // StaClosed
            // 
            this.StaClosed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StaClosed.AutoSize = true;
            this.StaClosed.Location = new System.Drawing.Point(239, 75);
            this.StaClosed.Name = "StaClosed";
            this.StaClosed.Size = new System.Drawing.Size(77, 24);
            this.StaClosed.TabIndex = 6;
            this.StaClosed.Text = "Closed";
            this.StaClosed.UseVisualStyleBackColor = true;
            this.StaClosed.CheckedChanged += new System.EventHandler(this.StaClosed_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(51, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Full Name:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 143);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "Short Name:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(65, 175);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 20);
            this.label6.TabIndex = 11;
            this.label6.Text = "Street 1:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(65, 207);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 20);
            this.label7.TabIndex = 13;
            this.label7.Text = "Street 2:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(96, 239);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 20);
            this.label8.TabIndex = 15;
            this.label8.Text = "City:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 271);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(123, 20);
            this.label9.TabIndex = 17;
            this.label9.Text = "Domestic State:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(58, 303);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 20);
            this.label10.TabIndex = 19;
            this.label10.Text = "Zip Code:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(34, 335);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 20);
            this.label11.TabIndex = 21;
            this.label11.Text = "Lender Type:";
            // 
            // Street2
            // 
            this.Street2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Street2.Location = new System.Drawing.Point(141, 204);
            this.Street2.MaxLength = 30;
            this.Street2.Name = "Street2";
            this.Street2.Size = new System.Drawing.Size(373, 26);
            this.Street2.TabIndex = 14;
            // 
            // LenderType
            // 
            this.LenderType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LenderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LenderType.FormattingEnabled = true;
            this.LenderType.Location = new System.Drawing.Point(141, 332);
            this.LenderType.Name = "LenderType";
            this.LenderType.Size = new System.Drawing.Size(327, 28);
            this.LenderType.TabIndex = 22;
            // 
            // Exit
            // 
            this.Exit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Exit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Exit.Location = new System.Drawing.Point(422, 380);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(92, 42);
            this.Exit.TabIndex = 24;
            this.Exit.Text = "Exit";
            this.Exit.UseVisualStyleBackColor = true;
            // 
            // Continue
            // 
            this.Continue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Continue.Location = new System.Drawing.Point(317, 380);
            this.Continue.Name = "Continue";
            this.Continue.Size = new System.Drawing.Size(92, 42);
            this.Continue.TabIndex = 25;
            this.Continue.Text = "Continue";
            this.Continue.UseVisualStyleBackColor = true;
            this.Continue.OnValidate += new Uheaa.Common.WinForms.ValidationButton.ValidationHandler(this.Continue_OnValidate);
            // 
            // ZipCode
            // 
            this.ZipCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ZipCode.Location = new System.Drawing.Point(141, 300);
            this.ZipCode.MaxLength = 9;
            this.ZipCode.Name = "ZipCode";
            this.ZipCode.Size = new System.Drawing.Size(153, 26);
            this.ZipCode.TabIndex = 20;
            // 
            // State
            // 
            this.State.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.State.Location = new System.Drawing.Point(141, 268);
            this.State.MaxLength = 2;
            this.State.Name = "State";
            this.State.Size = new System.Drawing.Size(50, 26);
            this.State.TabIndex = 18;
            // 
            // City
            // 
            this.City.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.City.Location = new System.Drawing.Point(141, 236);
            this.City.MaxLength = 20;
            this.City.Name = "City";
            this.City.Size = new System.Drawing.Size(229, 26);
            this.City.TabIndex = 16;
            // 
            // Street1
            // 
            this.Street1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Street1.Location = new System.Drawing.Point(141, 172);
            this.Street1.MaxLength = 30;
            this.Street1.Name = "Street1";
            this.Street1.Size = new System.Drawing.Size(373, 26);
            this.Street1.TabIndex = 12;
            // 
            // ShortName
            // 
            this.ShortName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ShortName.Location = new System.Drawing.Point(141, 140);
            this.ShortName.MaxLength = 20;
            this.ShortName.Name = "ShortName";
            this.ShortName.Size = new System.Drawing.Size(239, 26);
            this.ShortName.TabIndex = 10;
            // 
            // FullName
            // 
            this.FullName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FullName.Location = new System.Drawing.Point(141, 108);
            this.FullName.MaxLength = 40;
            this.FullName.Name = "FullName";
            this.FullName.Size = new System.Drawing.Size(373, 26);
            this.FullName.TabIndex = 8;
            // 
            // Unlocated
            // 
            this.Unlocated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Unlocated.Location = new System.Drawing.Point(38, 380);
            this.Unlocated.Name = "Unlocated";
            this.Unlocated.Size = new System.Drawing.Size(112, 43);
            this.Unlocated.TabIndex = 26;
            this.Unlocated.Text = "Unlocated";
            this.Unlocated.UseVisualStyleBackColor = true;
            this.Unlocated.OnValidate += new Uheaa.Common.WinForms.ValidationButton.ValidationHandler(this.Unlocated_OnValidate);
            this.Unlocated.Click += new System.EventHandler(this.Unlocated_Click);
            // 
            // LenderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 434);
            this.Controls.Add(this.Unlocated);
            this.Controls.Add(this.Continue);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.LenderType);
            this.Controls.Add(this.ZipCode);
            this.Controls.Add(this.State);
            this.Controls.Add(this.City);
            this.Controls.Add(this.Street2);
            this.Controls.Add(this.Street1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ShortName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.FullName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.StaClosed);
            this.Controls.Add(this.StaOpen);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LenderId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Mod);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(538, 473);
            this.Name = "LenderForm";
            this.ShowIcon = false;
            this.Text = "Lender Info";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Mod;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LenderId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox StaOpen;
        private System.Windows.Forms.CheckBox StaClosed;
        private System.Windows.Forms.Label label4;
        private Uheaa.Common.WinForms.RequiredTextBox FullName;
        private System.Windows.Forms.Label label5;
        private Uheaa.Common.WinForms.RequiredTextBox ShortName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private Uheaa.Common.WinForms.RequiredTextBox Street1;
        private System.Windows.Forms.TextBox Street2;
        private Uheaa.Common.WinForms.RequiredTextBox City;
        private Uheaa.Common.WinForms.RequiredTextBox State;
        private Uheaa.Common.WinForms.RequiredTextBox ZipCode;
        private System.Windows.Forms.ComboBox LenderType;
        private System.Windows.Forms.Button Exit;
        private Uheaa.Common.WinForms.ValidationButton Continue;
        //private Uheaa.Common.WinForms.ValidationButton validationButton1;
        private Uheaa.Common.WinForms.ValidationButton Unlocated;
    }
}
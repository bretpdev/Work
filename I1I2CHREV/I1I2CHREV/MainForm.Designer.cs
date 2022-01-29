namespace I1I2CHREV
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.StateBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.LocateButton = new System.Windows.Forms.Button();
            this.NoLocateButton = new System.Windows.Forms.Button();
            this.NotFoundButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.ZipBox = new Uheaa.Common.WinForms.OmniTextBox();
            this.AddressStatusBox = new Uheaa.Common.WinForms.OmniTextBox();
            this.CountryBox = new Uheaa.Common.WinForms.OmniTextBox();
            this.CityBox = new Uheaa.Common.WinForms.OmniTextBox();
            this.NameBox = new Uheaa.Common.WinForms.OmniTextBox();
            this.SsnBox = new Uheaa.Common.WinForms.SsnTextBox();
            this.Address1Box = new Uheaa.Common.WinForms.OmniTextBox();
            this.Address2Box = new Uheaa.Common.WinForms.OmniTextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(541, 81);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(553, 250);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Instructions";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(6, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(541, 77);
            this.label3.TabIndex = 2;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(6, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(541, 64);
            this.label2.TabIndex = 1;
            this.label2.Text = "2.  If the borrower was found on the Clearinghouse website but no new information" +
    " was found, click No Locate to add an activity record and complete the task with" +
    "out updating TX1J or LP22.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 277);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "SSN";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(227, 277);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 18);
            this.label5.TabIndex = 5;
            this.label5.Text = "Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 325);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 18);
            this.label6.TabIndex = 7;
            this.label6.Text = "Address 1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 357);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 18);
            this.label7.TabIndex = 9;
            this.label7.Text = "Address 2";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(54, 389);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 18);
            this.label8.TabIndex = 11;
            this.label8.Text = "City";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(283, 389);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 18);
            this.label9.TabIndex = 14;
            this.label9.Text = "State";
            // 
            // StateBox
            // 
            this.StateBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StateBox.FormattingEnabled = true;
            this.StateBox.Items.AddRange(new object[] {
            "",
            "AK",
            "AL",
            "AR",
            "AZ",
            "CA",
            "CO",
            "CT",
            "DC",
            "DE",
            "FL",
            "GA",
            "HI",
            "IA",
            "ID",
            "IL",
            "IN",
            "KS",
            "KY",
            "LA",
            "MA",
            "MD",
            "ME",
            "MI",
            "MN",
            "MO",
            "MS",
            "MT",
            "NC",
            "ND",
            "NE",
            "NH",
            "NJ",
            "NM",
            "NV",
            "NY",
            "OH",
            "OK",
            "OR",
            "PA",
            "RI",
            "SC",
            "SD",
            "TN",
            "TX",
            "UT",
            "VA",
            "VT",
            "WA",
            "WI",
            "WV",
            "WY"});
            this.StateBox.Location = new System.Drawing.Point(334, 386);
            this.StateBox.Name = "StateBox";
            this.StateBox.Size = new System.Drawing.Size(75, 26);
            this.StateBox.TabIndex = 15;
            this.StateBox.SelectedIndexChanged += new System.EventHandler(this.StateBox_SelectedIndexChanged);
            this.StateBox.TextChanged += new System.EventHandler(this.StateBox_TextChanged);
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(415, 389);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 18);
            this.label10.TabIndex = 16;
            this.label10.Text = "Zip";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(28, 421);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(61, 18);
            this.label11.TabIndex = 18;
            this.label11.Text = "Country";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(331, 424);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(115, 18);
            this.label12.TabIndex = 20;
            this.label12.Text = "Address Status";
            // 
            // LocateButton
            // 
            this.LocateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LocateButton.Location = new System.Drawing.Point(12, 453);
            this.LocateButton.Name = "LocateButton";
            this.LocateButton.Size = new System.Drawing.Size(97, 39);
            this.LocateButton.TabIndex = 22;
            this.LocateButton.Text = "Locate";
            this.LocateButton.UseVisualStyleBackColor = true;
            // 
            // NoLocateButton
            // 
            this.NoLocateButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NoLocateButton.Location = new System.Drawing.Point(156, 453);
            this.NoLocateButton.Name = "NoLocateButton";
            this.NoLocateButton.Size = new System.Drawing.Size(106, 39);
            this.NoLocateButton.TabIndex = 23;
            this.NoLocateButton.Text = "No Locate";
            this.NoLocateButton.UseVisualStyleBackColor = true;
            // 
            // NotFoundButton
            // 
            this.NotFoundButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NotFoundButton.Location = new System.Drawing.Point(309, 453);
            this.NotFoundButton.Name = "NotFoundButton";
            this.NotFoundButton.Size = new System.Drawing.Size(110, 39);
            this.NotFoundButton.TabIndex = 24;
            this.NotFoundButton.Text = "Not Found";
            this.NotFoundButton.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.Location = new System.Drawing.Point(466, 453);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(97, 39);
            this.CancelButton.TabIndex = 25;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // ZipBox
            // 
            this.ZipBox.AllowAllCharacters = false;
            this.ZipBox.AllowAlphaCharacters = false;
            this.ZipBox.AllowedAdditionalCharacters = "";
            this.ZipBox.AllowNumericCharacters = true;
            this.ZipBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ZipBox.InvalidColor = System.Drawing.Color.LightPink;
            this.ZipBox.Location = new System.Drawing.Point(451, 386);
            this.ZipBox.Mask = "";
            this.ZipBox.Name = "ZipBox";
            this.ZipBox.Size = new System.Drawing.Size(114, 26);
            this.ZipBox.TabIndex = 26;
            this.ZipBox.ValidationMessage = null;
            this.ZipBox.ValidColor = System.Drawing.Color.LightGreen;
            this.ZipBox.ValidationOnLeave += new Uheaa.Common.WinForms.ValidationEventHandler(this.ValidationOnLeave);
            // 
            // AddressStatusBox
            // 
            this.AddressStatusBox.AllowAllCharacters = true;
            this.AddressStatusBox.AllowAlphaCharacters = true;
            this.AddressStatusBox.AllowedAdditionalCharacters = "";
            this.AddressStatusBox.AllowNumericCharacters = true;
            this.AddressStatusBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddressStatusBox.InvalidColor = System.Drawing.Color.LightPink;
            this.AddressStatusBox.Location = new System.Drawing.Point(451, 421);
            this.AddressStatusBox.Mask = "";
            this.AddressStatusBox.Name = "AddressStatusBox";
            this.AddressStatusBox.ReadOnly = true;
            this.AddressStatusBox.Size = new System.Drawing.Size(114, 26);
            this.AddressStatusBox.TabIndex = 21;
            this.AddressStatusBox.ValidationMessage = null;
            this.AddressStatusBox.ValidColor = System.Drawing.Color.LightGreen;
            // 
            // CountryBox
            // 
            this.CountryBox.AllowAllCharacters = false;
            this.CountryBox.AllowAlphaCharacters = true;
            this.CountryBox.AllowedAdditionalCharacters = " ";
            this.CountryBox.AllowNumericCharacters = true;
            this.CountryBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CountryBox.InvalidColor = System.Drawing.Color.LightPink;
            this.CountryBox.Location = new System.Drawing.Point(95, 418);
            this.CountryBox.Mask = "";
            this.CountryBox.Name = "CountryBox";
            this.CountryBox.Size = new System.Drawing.Size(182, 26);
            this.CountryBox.TabIndex = 19;
            this.CountryBox.ValidationMessage = null;
            this.CountryBox.ValidColor = System.Drawing.Color.LightGreen;
            this.CountryBox.ValidationOnLeave += new Uheaa.Common.WinForms.ValidationEventHandler(this.ValidationOnLeave);
            this.CountryBox.TextChanged += new System.EventHandler(this.CountryBox_TextChanged);
            // 
            // CityBox
            // 
            this.CityBox.AllowAllCharacters = false;
            this.CityBox.AllowAlphaCharacters = true;
            this.CityBox.AllowedAdditionalCharacters = " ";
            this.CityBox.AllowNumericCharacters = true;
            this.CityBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CityBox.InvalidColor = System.Drawing.Color.LightPink;
            this.CityBox.Location = new System.Drawing.Point(95, 386);
            this.CityBox.Mask = "";
            this.CityBox.Name = "CityBox";
            this.CityBox.Size = new System.Drawing.Size(182, 26);
            this.CityBox.TabIndex = 12;
            this.CityBox.ValidationMessage = null;
            this.CityBox.ValidColor = System.Drawing.Color.LightGreen;
            this.CityBox.ValidationOnLeave += new Uheaa.Common.WinForms.ValidationEventHandler(this.ValidationOnLeave);
            // 
            // NameBox
            // 
            this.NameBox.AllowAllCharacters = true;
            this.NameBox.AllowAlphaCharacters = true;
            this.NameBox.AllowedAdditionalCharacters = "";
            this.NameBox.AllowNumericCharacters = true;
            this.NameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NameBox.InvalidColor = System.Drawing.Color.LightPink;
            this.NameBox.Location = new System.Drawing.Point(283, 274);
            this.NameBox.Mask = "";
            this.NameBox.Name = "NameBox";
            this.NameBox.ReadOnly = true;
            this.NameBox.Size = new System.Drawing.Size(282, 26);
            this.NameBox.TabIndex = 6;
            this.NameBox.ValidationMessage = null;
            this.NameBox.ValidColor = System.Drawing.Color.LightGreen;
            // 
            // SsnBox
            // 
            this.SsnBox.AllowedSpecialCharacters = "";
            this.SsnBox.Location = new System.Drawing.Point(95, 274);
            this.SsnBox.MaxLength = 9;
            this.SsnBox.Name = "SsnBox";
            this.SsnBox.ReadOnly = true;
            this.SsnBox.Size = new System.Drawing.Size(126, 26);
            this.SsnBox.Ssn = null;
            this.SsnBox.TabIndex = 4;
            // 
            // Address1Box
            // 
            this.Address1Box.AllowAllCharacters = false;
            this.Address1Box.AllowAlphaCharacters = true;
            this.Address1Box.AllowedAdditionalCharacters = "-# ";
            this.Address1Box.AllowNumericCharacters = true;
            this.Address1Box.InvalidColor = System.Drawing.Color.LightPink;
            this.Address1Box.Location = new System.Drawing.Point(95, 325);
            this.Address1Box.Mask = "";
            this.Address1Box.Name = "Address1Box";
            this.Address1Box.Size = new System.Drawing.Size(470, 26);
            this.Address1Box.TabIndex = 27;
            this.Address1Box.ValidationMessage = null;
            this.Address1Box.ValidColor = System.Drawing.Color.LightGreen;
            // 
            // Address2Box
            // 
            this.Address2Box.AllowAllCharacters = false;
            this.Address2Box.AllowAlphaCharacters = true;
            this.Address2Box.AllowedAdditionalCharacters = "-# ";
            this.Address2Box.AllowNumericCharacters = true;
            this.Address2Box.InvalidColor = System.Drawing.Color.LightPink;
            this.Address2Box.Location = new System.Drawing.Point(95, 354);
            this.Address2Box.Mask = "";
            this.Address2Box.Name = "Address2Box";
            this.Address2Box.Size = new System.Drawing.Size(470, 26);
            this.Address2Box.TabIndex = 28;
            this.Address2Box.ValidationMessage = null;
            this.Address2Box.ValidColor = System.Drawing.Color.LightGreen;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 504);
            this.Controls.Add(this.Address2Box);
            this.Controls.Add(this.Address1Box);
            this.Controls.Add(this.ZipBox);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.NotFoundButton);
            this.Controls.Add(this.NoLocateButton);
            this.Controls.Add(this.LocateButton);
            this.Controls.Add(this.AddressStatusBox);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.CountryBox);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.StateBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.CityBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.SsnBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(593, 542);
            this.Name = "MainForm";
            this.Text = "I1/I2 Clearinghouse Review";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private Uheaa.Common.WinForms.SsnTextBox SsnBox;
        private System.Windows.Forms.Label label5;
        private Uheaa.Common.WinForms.OmniTextBox NameBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private Uheaa.Common.WinForms.OmniTextBox CityBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox StateBox;
        private System.Windows.Forms.Label label10;
        private Uheaa.Common.WinForms.OmniTextBox CountryBox;
        private System.Windows.Forms.Label label11;
        private Uheaa.Common.WinForms.OmniTextBox AddressStatusBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button LocateButton;
        private System.Windows.Forms.Button NoLocateButton;
        private System.Windows.Forms.Button NotFoundButton;
        private System.Windows.Forms.Button CancelButton;
        private Uheaa.Common.WinForms.OmniTextBox ZipBox;
        private Uheaa.Common.WinForms.OmniTextBox Address1Box;
        private Uheaa.Common.WinForms.OmniTextBox Address2Box;
    }
}
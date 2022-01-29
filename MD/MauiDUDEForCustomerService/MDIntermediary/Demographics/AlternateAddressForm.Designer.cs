namespace MDIntermediary
{
    partial class AlternateAddressForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlternateAddressForm));
            this.ZipLabel = new System.Windows.Forms.Label();
            this.ZipBox = new System.Windows.Forms.TextBox();
            this.StateLabel = new System.Windows.Forms.Label();
            this.CityLabel = new System.Windows.Forms.Label();
            this.CityBox = new System.Windows.Forms.TextBox();
            this.Address2Label = new System.Windows.Forms.Label();
            this.Address1Label = new System.Windows.Forms.Label();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.Address2Box = new Uheaa.Common.WinForms.OmniTextBox();
            this.Address1Box = new Uheaa.Common.WinForms.OmniTextBox();
            this.CountryBox = new Uheaa.Common.WinForms.OmniTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.StateBox = new Uheaa.Common.WinForms.OmniTextBox();
            this.AddressesList = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.AddButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ZipLabel
            // 
            this.ZipLabel.Location = new System.Drawing.Point(455, 99);
            this.ZipLabel.Name = "ZipLabel";
            this.ZipLabel.Size = new System.Drawing.Size(40, 20);
            this.ZipLabel.TabIndex = 7;
            this.ZipLabel.Text = "Zip";
            this.ZipLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ZipBox
            // 
            this.ZipBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.ZipBox.Enabled = false;
            this.ZipBox.Location = new System.Drawing.Point(501, 96);
            this.ZipBox.MaxLength = 9;
            this.ZipBox.Name = "ZipBox";
            this.ZipBox.Size = new System.Drawing.Size(89, 23);
            this.ZipBox.TabIndex = 8;
            // 
            // StateLabel
            // 
            this.StateLabel.Location = new System.Drawing.Point(202, 99);
            this.StateLabel.Name = "StateLabel";
            this.StateLabel.Size = new System.Drawing.Size(73, 20);
            this.StateLabel.TabIndex = 21;
            this.StateLabel.Text = "State";
            this.StateLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // CityLabel
            // 
            this.CityLabel.Location = new System.Drawing.Point(202, 70);
            this.CityLabel.Name = "CityLabel";
            this.CityLabel.Size = new System.Drawing.Size(73, 20);
            this.CityLabel.TabIndex = 20;
            this.CityLabel.Text = "City";
            this.CityLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // CityBox
            // 
            this.CityBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.CityBox.Enabled = false;
            this.CityBox.Location = new System.Drawing.Point(281, 67);
            this.CityBox.MaxLength = 20;
            this.CityBox.Name = "CityBox";
            this.CityBox.Size = new System.Drawing.Size(309, 23);
            this.CityBox.TabIndex = 5;
            // 
            // Address2Label
            // 
            this.Address2Label.Location = new System.Drawing.Point(202, 41);
            this.Address2Label.Name = "Address2Label";
            this.Address2Label.Size = new System.Drawing.Size(73, 20);
            this.Address2Label.TabIndex = 18;
            this.Address2Label.Text = "Address 2";
            this.Address2Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Address1Label
            // 
            this.Address1Label.Location = new System.Drawing.Point(202, 12);
            this.Address1Label.Name = "Address1Label";
            this.Address1Label.Size = new System.Drawing.Size(73, 20);
            this.Address1Label.TabIndex = 16;
            this.Address1Label.Text = "Address 1";
            this.Address1Label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(501, 168);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(89, 28);
            this.OkButton.TabIndex = 10;
            this.OkButton.Text = "Save";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(12, 168);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(89, 28);
            this.CancelButton.TabIndex = 12;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // Address2Box
            // 
            this.Address2Box.AllowAllCharacters = false;
            this.Address2Box.AllowAlphaCharacters = true;
            this.Address2Box.AllowedAdditionalCharacters = " #/-";
            this.Address2Box.AllowNumericCharacters = true;
            this.Address2Box.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Address2Box.Enabled = false;
            this.Address2Box.InvalidColor = System.Drawing.Color.LightPink;
            this.Address2Box.Location = new System.Drawing.Point(281, 38);
            this.Address2Box.Mask = "";
            this.Address2Box.MaxLength = 30;
            this.Address2Box.Name = "Address2Box";
            this.Address2Box.Size = new System.Drawing.Size(309, 23);
            this.Address2Box.TabIndex = 4;
            this.Address2Box.ValidationMessage = null;
            this.Address2Box.ValidColor = System.Drawing.Color.LightGreen;
            // 
            // Address1Box
            // 
            this.Address1Box.AllowAllCharacters = false;
            this.Address1Box.AllowAlphaCharacters = true;
            this.Address1Box.AllowedAdditionalCharacters = " #/-";
            this.Address1Box.AllowNumericCharacters = true;
            this.Address1Box.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Address1Box.Enabled = false;
            this.Address1Box.InvalidColor = System.Drawing.Color.LightPink;
            this.Address1Box.Location = new System.Drawing.Point(281, 12);
            this.Address1Box.Mask = "";
            this.Address1Box.MaxLength = 30;
            this.Address1Box.Name = "Address1Box";
            this.Address1Box.Size = new System.Drawing.Size(309, 23);
            this.Address1Box.TabIndex = 3;
            this.Address1Box.ValidationMessage = null;
            this.Address1Box.ValidColor = System.Drawing.Color.LightGreen;
            this.Address1Box.TextChanged += new System.EventHandler(this.Address1Box_TextChanged);
            // 
            // CountryBox
            // 
            this.CountryBox.AllowAllCharacters = false;
            this.CountryBox.AllowAlphaCharacters = true;
            this.CountryBox.AllowedAdditionalCharacters = " #/-";
            this.CountryBox.AllowNumericCharacters = true;
            this.CountryBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.CountryBox.Enabled = false;
            this.CountryBox.InvalidColor = System.Drawing.Color.LightPink;
            this.CountryBox.Location = new System.Drawing.Point(281, 126);
            this.CountryBox.Mask = "";
            this.CountryBox.MaxLength = 30;
            this.CountryBox.Name = "CountryBox";
            this.CountryBox.Size = new System.Drawing.Size(309, 23);
            this.CountryBox.TabIndex = 9;
            this.CountryBox.ValidationMessage = null;
            this.CountryBox.ValidColor = System.Drawing.Color.LightGreen;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(202, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 20);
            this.label1.TabIndex = 25;
            this.label1.Text = "Country";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // StateBox
            // 
            this.StateBox.AllowAllCharacters = false;
            this.StateBox.AllowAlphaCharacters = true;
            this.StateBox.AllowedAdditionalCharacters = " #/-";
            this.StateBox.AllowNumericCharacters = true;
            this.StateBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.StateBox.Enabled = false;
            this.StateBox.InvalidColor = System.Drawing.Color.LightPink;
            this.StateBox.Location = new System.Drawing.Point(281, 96);
            this.StateBox.Mask = "";
            this.StateBox.MaxLength = 30;
            this.StateBox.Name = "StateBox";
            this.StateBox.Size = new System.Drawing.Size(168, 23);
            this.StateBox.TabIndex = 6;
            this.StateBox.ValidationMessage = null;
            this.StateBox.ValidColor = System.Drawing.Color.LightGreen;
            // 
            // AddressesList
            // 
            this.AddressesList.DisplayMember = "Address1";
            this.AddressesList.FormattingEnabled = true;
            this.AddressesList.IntegralHeight = false;
            this.AddressesList.ItemHeight = 16;
            this.AddressesList.Location = new System.Drawing.Point(12, 6);
            this.AddressesList.Name = "AddressesList";
            this.AddressesList.Size = new System.Drawing.Size(184, 113);
            this.AddressesList.TabIndex = 0;
            this.AddressesList.SelectedIndexChanged += new System.EventHandler(this.AddressesList_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(12, 164);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(578, 1);
            this.label2.TabIndex = 28;
            // 
            // DeleteButton
            // 
            this.DeleteButton.Enabled = false;
            this.DeleteButton.Location = new System.Drawing.Point(12, 123);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(75, 28);
            this.DeleteButton.TabIndex = 1;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(121, 123);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(75, 28);
            this.AddButton.TabIndex = 2;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // AlternateAddressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 208);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AddressesList);
            this.Controls.Add(this.StateBox);
            this.Controls.Add(this.CountryBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Address1Box);
            this.Controls.Add(this.Address2Box);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.ZipLabel);
            this.Controls.Add(this.ZipBox);
            this.Controls.Add(this.StateLabel);
            this.Controls.Add(this.CityLabel);
            this.Controls.Add(this.CityBox);
            this.Controls.Add(this.Address2Label);
            this.Controls.Add(this.Address1Label);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlternateAddressForm";
            this.Text = "MD - Alternate Addresses";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ZipLabel;
        private System.Windows.Forms.TextBox ZipBox;
        private System.Windows.Forms.Label StateLabel;
        private System.Windows.Forms.Label CityLabel;
        private System.Windows.Forms.TextBox CityBox;
        private System.Windows.Forms.Label Address2Label;
        private System.Windows.Forms.Label Address1Label;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
        private Uheaa.Common.WinForms.OmniTextBox Address2Box;
        private Uheaa.Common.WinForms.OmniTextBox Address1Box;
        private Uheaa.Common.WinForms.OmniTextBox CountryBox;
        private System.Windows.Forms.Label label1;
        private Uheaa.Common.WinForms.OmniTextBox StateBox;
        private System.Windows.Forms.ListBox AddressesList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button AddButton;

    }
}
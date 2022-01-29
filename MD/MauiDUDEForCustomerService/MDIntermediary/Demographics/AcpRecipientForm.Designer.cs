namespace MDIntermediary.Demographics
{
    partial class AcpRecipientForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AcpRecipientForm));
            this.label1 = new System.Windows.Forms.Label();
            this.RecipientNameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.RelationshipBox = new System.Windows.Forms.ComboBox();
            this.ContactPhoneBox = new MDIntermediary.PhoneBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.AuthorizedButton = new Uheaa.Common.WinForms.YesNoButton();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Recipient Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // RecipientNameBox
            // 
            this.RecipientNameBox.Location = new System.Drawing.Point(134, 13);
            this.RecipientNameBox.MaxLength = 30;
            this.RecipientNameBox.Name = "RecipientNameBox";
            this.RecipientNameBox.Size = new System.Drawing.Size(258, 23);
            this.RecipientNameBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Relationship";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Contact Phone";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // RelationshipBox
            // 
            this.RelationshipBox.FormattingEnabled = true;
            this.RelationshipBox.Items.AddRange(new object[] {
            "",
            "Parent",
            "Spouse",
            "Child",
            "Sibling",
            "Relative",
            "Non-Relative"});
            this.RelationshipBox.Location = new System.Drawing.Point(134, 44);
            this.RelationshipBox.Name = "RelationshipBox";
            this.RelationshipBox.Size = new System.Drawing.Size(258, 24);
            this.RelationshipBox.TabIndex = 5;
            // 
            // ContactPhoneBox
            // 
            this.ContactPhoneBox.Extension = "";
            this.ContactPhoneBox.Location = new System.Drawing.Point(134, 74);
            this.ContactPhoneBox.Name = "ContactPhoneBox";
            this.ContactPhoneBox.PhoneNumber = "";
            this.ContactPhoneBox.Size = new System.Drawing.Size(124, 23);
            this.ContactPhoneBox.TabIndex = 6;
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(317, 144);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 29);
            this.OkButton.TabIndex = 7;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(12, 144);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 29);
            this.CancelButton.TabIndex = 8;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // AuthorizedButton
            // 
            this.AuthorizedButton.Font = new System.Drawing.Font("Arial", 10F);
            this.AuthorizedButton.Location = new System.Drawing.Point(134, 104);
            this.AuthorizedButton.Name = "AuthorizedButton";
            this.AuthorizedButton.SelectedIndex = 0;
            this.AuthorizedButton.SelectedValue = true;
            this.AuthorizedButton.Size = new System.Drawing.Size(51, 29);
            this.AuthorizedButton.TabIndex = 9;
            this.AuthorizedButton.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 18);
            this.label4.TabIndex = 10;
            this.label4.Text = "Authorized";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AcpRecipientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 187);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.AuthorizedButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.ContactPhoneBox);
            this.Controls.Add(this.RelationshipBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RecipientNameBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AcpRecipientForm";
            this.Text = "Recipient Information";
            this.Shown += new System.EventHandler(this.AcpRecipientForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox RecipientNameBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox RelationshipBox;
        private PhoneBox ContactPhoneBox;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
        private Uheaa.Common.WinForms.YesNoButton AuthorizedButton;
        private System.Windows.Forms.Label label4;
    }
}
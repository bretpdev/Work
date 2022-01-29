namespace BLKADDFED
{
    partial class SelectRecords
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
            this.flwAddresses = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.flwPhone = new System.Windows.Forms.FlowLayoutPanel();
            this.lblAddresss = new System.Windows.Forms.Label();
            this.lblPhones = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.txtComments = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // flwAddresses
            // 
            this.flwAddresses.AutoScroll = true;
            this.flwAddresses.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flwAddresses.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flwAddresses.Location = new System.Drawing.Point(16, 104);
            this.flwAddresses.Name = "flwAddresses";
            this.flwAddresses.Size = new System.Drawing.Size(481, 146);
            this.flwAddresses.TabIndex = 0;
            this.flwAddresses.WrapContents = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(312, 448);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // flwPhone
            // 
            this.flwPhone.AutoScroll = true;
            this.flwPhone.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flwPhone.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flwPhone.Location = new System.Drawing.Point(16, 280);
            this.flwPhone.Name = "flwPhone";
            this.flwPhone.Size = new System.Drawing.Size(481, 146);
            this.flwPhone.TabIndex = 2;
            this.flwPhone.WrapContents = false;
            // 
            // lblAddresss
            // 
            this.lblAddresss.AutoSize = true;
            this.lblAddresss.Location = new System.Drawing.Point(16, 88);
            this.lblAddresss.Name = "lblAddresss";
            this.lblAddresss.Size = new System.Drawing.Size(56, 13);
            this.lblAddresss.TabIndex = 3;
            this.lblAddresss.Text = "Addresses";
            // 
            // lblPhones
            // 
            this.lblPhones.AutoSize = true;
            this.lblPhones.Location = new System.Drawing.Point(16, 264);
            this.lblPhones.Name = "lblPhones";
            this.lblPhones.Size = new System.Drawing.Size(83, 13);
            this.lblPhones.TabIndex = 4;
            this.lblPhones.Text = "Phone Numbers";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(120, 448);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnNo
            // 
            this.btnNo.Location = new System.Drawing.Point(216, 448);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(80, 23);
            this.btnNo.TabIndex = 6;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Visible = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // lblInstructions
            // 
            this.lblInstructions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblInstructions.Location = new System.Drawing.Point(16, 16);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(480, 48);
            this.lblInstructions.TabIndex = 7;
            this.lblInstructions.Text = "Select up to 1 address and/or up to 3 phone numbers.  Click OK to continue or cli" +
                "ck Cancel to end the script.";
            this.lblInstructions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtComments
            // 
            this.txtComments.Location = new System.Drawing.Point(16, 104);
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.Size = new System.Drawing.Size(480, 320);
            this.txtComments.TabIndex = 8;
            this.txtComments.Visible = false;
            // 
            // SelectRecords
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 487);
            this.Controls.Add(this.txtComments);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblPhones);
            this.Controls.Add(this.lblAddresss);
            this.Controls.Add(this.flwPhone);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.flwAddresses);
            this.Name = "SelectRecords";
            this.Text = "Block Address/Phone - FED";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flwAddresses;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.FlowLayoutPanel flwPhone;
        private System.Windows.Forms.Label lblAddresss;
        private System.Windows.Forms.Label lblPhones;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.TextBox txtComments;

    }
}
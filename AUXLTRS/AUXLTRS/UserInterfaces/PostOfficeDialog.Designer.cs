namespace AUXLTRS
{
    partial class PostOfficeDialog
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
            this.lblEnterInformation = new System.Windows.Forms.Label();
            this.lblCity = new System.Windows.Forms.Label();
            this.lblState = new System.Windows.Forms.Label();
            this.lblZip = new System.Windows.Forms.Label();
            this.grpPostOfficeInformation = new System.Windows.Forms.GroupBox();
            this.cmbState = new System.Windows.Forms.ComboBox();
            this.postOfficeBorrowerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblSsn = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new Uheaa.Common.WinForms.ValidationButton();
            this.txtSsn = new Uheaa.Common.WinForms.RequiredTextBox();
            this.txtCity = new Uheaa.Common.WinForms.RequiredTextBox();
            this.txtZip = new Uheaa.Common.WinForms.RequiredTextBox();
            this.postOfficeBorrowerBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.postOfficeBorrowerBindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.grpPostOfficeInformation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.postOfficeBorrowerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.postOfficeBorrowerBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.postOfficeBorrowerBindingSource2)).BeginInit();
            this.SuspendLayout();
            // 
            // lblEnterInformation
            // 
            this.lblEnterInformation.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblEnterInformation.AutoSize = true;
            this.lblEnterInformation.Location = new System.Drawing.Point(81, 14);
            this.lblEnterInformation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEnterInformation.Name = "lblEnterInformation";
            this.lblEnterInformation.Size = new System.Drawing.Size(227, 20);
            this.lblEnterInformation.TabIndex = 0;
            this.lblEnterInformation.Text = "Enter the following information.";
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(9, 34);
            this.lblCity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(35, 20);
            this.lblCity.TabIndex = 0;
            this.lblCity.Text = "City";
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(9, 74);
            this.lblState.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(48, 20);
            this.lblState.TabIndex = 0;
            this.lblState.Text = "State";
            // 
            // lblZip
            // 
            this.lblZip.AutoSize = true;
            this.lblZip.Location = new System.Drawing.Point(184, 74);
            this.lblZip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblZip.Name = "lblZip";
            this.lblZip.Size = new System.Drawing.Size(34, 20);
            this.lblZip.TabIndex = 0;
            this.lblZip.Text = "ZIP";
            // 
            // grpPostOfficeInformation
            // 
            this.grpPostOfficeInformation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpPostOfficeInformation.Controls.Add(this.txtZip);
            this.grpPostOfficeInformation.Controls.Add(this.txtCity);
            this.grpPostOfficeInformation.Controls.Add(this.cmbState);
            this.grpPostOfficeInformation.Controls.Add(this.lblCity);
            this.grpPostOfficeInformation.Controls.Add(this.lblZip);
            this.grpPostOfficeInformation.Controls.Add(this.lblState);
            this.grpPostOfficeInformation.Location = new System.Drawing.Point(18, 92);
            this.grpPostOfficeInformation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpPostOfficeInformation.Name = "grpPostOfficeInformation";
            this.grpPostOfficeInformation.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpPostOfficeInformation.Size = new System.Drawing.Size(358, 109);
            this.grpPostOfficeInformation.TabIndex = 4;
            this.grpPostOfficeInformation.TabStop = false;
            this.grpPostOfficeInformation.Text = "Post Office Information";
            // 
            // cmbState
            // 
            this.cmbState.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.postOfficeBorrowerBindingSource, "State", true));
            this.cmbState.FormattingEnabled = true;
            this.cmbState.Location = new System.Drawing.Point(68, 69);
            this.cmbState.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(79, 28);
            this.cmbState.TabIndex = 3;
            // 
            // lblSsn
            // 
            this.lblSsn.AutoSize = true;
            this.lblSsn.Location = new System.Drawing.Point(14, 57);
            this.lblSsn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSsn.Name = "lblSsn";
            this.lblSsn.Size = new System.Drawing.Size(152, 20);
            this.lblSsn.TabIndex = 0;
            this.lblSsn.Text = "Borrower\'s Ssn/Acct";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(232, 226);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 45);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(49, 226);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(116, 42);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtSsn
            // 
            this.txtSsn.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.postOfficeBorrowerBindingSource, "AccountNumber", true));
            this.txtSsn.Location = new System.Drawing.Point(173, 54);
            this.txtSsn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSsn.Name = "txtSsn";
            this.txtSsn.Size = new System.Drawing.Size(193, 26);
            this.txtSsn.TabIndex = 8;
            // 
            // txtCity
            // 
            this.txtCity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.postOfficeBorrowerBindingSource, "City", true));
            this.txtCity.Location = new System.Drawing.Point(68, 28);
            this.txtCity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(280, 26);
            this.txtCity.TabIndex = 5;
            // 
            // txtZip
            // 
            this.txtZip.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.postOfficeBorrowerBindingSource, "Zip", true));
            this.txtZip.Location = new System.Drawing.Point(225, 71);
            this.txtZip.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtZip.Name = "txtZip";
            this.txtZip.Size = new System.Drawing.Size(123, 26);
            this.txtZip.TabIndex = 6;
            // 
            // postOfficeBorrowerBindingSource1
            // 
            //this.postOfficeBorrowerBindingSource1.DataSource = typeof(AUXLTRS.PostOfficeBorrower);
            // 
            // postOfficeBorrowerBindingSource2
            // 
            //this.postOfficeBorrowerBindingSource2.DataSource = typeof(AUXLTRS.PostOfficeBorrower);
            // 
            // PostOfficeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 294);
            this.Controls.Add(this.txtSsn);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblSsn);
            this.Controls.Add(this.grpPostOfficeInformation);
            this.Controls.Add(this.lblEnterInformation);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "PostOfficeDialog";
            this.Text = "Post Office Letter";
            this.grpPostOfficeInformation.ResumeLayout(false);
            this.grpPostOfficeInformation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.postOfficeBorrowerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.postOfficeBorrowerBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.postOfficeBorrowerBindingSource2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEnterInformation;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Label lblZip;
        private System.Windows.Forms.GroupBox grpPostOfficeInformation;
        private System.Windows.Forms.Label lblSsn;
        private System.Windows.Forms.ComboBox cmbState;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.BindingSource postOfficeBorrowerBindingSource;
        private Uheaa.Common.WinForms.ValidationButton btnOK;
        private Uheaa.Common.WinForms.RequiredTextBox txtSsn;
        private Uheaa.Common.WinForms.RequiredTextBox txtCity;
        private Uheaa.Common.WinForms.RequiredTextBox txtZip;
        private System.Windows.Forms.BindingSource postOfficeBorrowerBindingSource1;
        private System.Windows.Forms.BindingSource postOfficeBorrowerBindingSource2;
    }
}
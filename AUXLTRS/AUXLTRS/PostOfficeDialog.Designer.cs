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
            this.txtZip = new System.Windows.Forms.TextBox();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.lblSsn = new System.Windows.Forms.Label();
            this.txtSsn = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.postOfficeBorrowerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grpPostOfficeInformation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.postOfficeBorrowerBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // lblEnterInformation
            // 
            this.lblEnterInformation.AutoSize = true;
            this.lblEnterInformation.Location = new System.Drawing.Point(54, 9);
            this.lblEnterInformation.Name = "lblEnterInformation";
            this.lblEnterInformation.Size = new System.Drawing.Size(151, 13);
            this.lblEnterInformation.TabIndex = 0;
            this.lblEnterInformation.Text = "Enter the following information.";
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(6, 22);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(24, 13);
            this.lblCity.TabIndex = 0;
            this.lblCity.Text = "City";
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(6, 48);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(32, 13);
            this.lblState.TabIndex = 0;
            this.lblState.Text = "State";
            // 
            // lblZip
            // 
            this.lblZip.AutoSize = true;
            this.lblZip.Location = new System.Drawing.Point(123, 48);
            this.lblZip.Name = "lblZip";
            this.lblZip.Size = new System.Drawing.Size(24, 13);
            this.lblZip.TabIndex = 0;
            this.lblZip.Text = "ZIP";
            // 
            // grpPostOfficeInformation
            // 
            this.grpPostOfficeInformation.Controls.Add(this.cmbState);
            this.grpPostOfficeInformation.Controls.Add(this.txtZip);
            this.grpPostOfficeInformation.Controls.Add(this.txtCity);
            this.grpPostOfficeInformation.Controls.Add(this.lblCity);
            this.grpPostOfficeInformation.Controls.Add(this.lblZip);
            this.grpPostOfficeInformation.Controls.Add(this.lblState);
            this.grpPostOfficeInformation.Location = new System.Drawing.Point(12, 60);
            this.grpPostOfficeInformation.Name = "grpPostOfficeInformation";
            this.grpPostOfficeInformation.Size = new System.Drawing.Size(239, 71);
            this.grpPostOfficeInformation.TabIndex = 4;
            this.grpPostOfficeInformation.TabStop = false;
            this.grpPostOfficeInformation.Text = "Post Office Information";
            // 
            // cmbState
            // 
            this.cmbState.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.postOfficeBorrowerBindingSource, "State", true));
            this.cmbState.FormattingEnabled = true;
            this.cmbState.Location = new System.Drawing.Point(45, 45);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(54, 21);
            this.cmbState.TabIndex = 3;
            // 
            // txtZip
            // 
            this.txtZip.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.postOfficeBorrowerBindingSource, "Zip", true));
            this.txtZip.Location = new System.Drawing.Point(153, 45);
            this.txtZip.Name = "txtZip";
            this.txtZip.Size = new System.Drawing.Size(80, 20);
            this.txtZip.TabIndex = 4;
            // 
            // txtCity
            // 
            this.txtCity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.postOfficeBorrowerBindingSource, "City", true));
            this.txtCity.Location = new System.Drawing.Point(45, 19);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(188, 20);
            this.txtCity.TabIndex = 2;
            // 
            // lblSsn
            // 
            this.lblSsn.AutoSize = true;
            this.lblSsn.Location = new System.Drawing.Point(9, 37);
            this.lblSsn.Name = "lblSsn";
            this.lblSsn.Size = new System.Drawing.Size(108, 13);
            this.lblSsn.TabIndex = 0;
            this.lblSsn.Text = "Borrower\'s SSN/Acct";
            // 
            // txtSsn
            // 
            this.txtSsn.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.postOfficeBorrowerBindingSource, "AccountNumber", true));
            this.txtSsn.Location = new System.Drawing.Point(126, 34);
            this.txtSsn.Name = "txtSsn";
            this.txtSsn.Size = new System.Drawing.Size(119, 20);
            this.txtSsn.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(31, 147);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 29);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(155, 147);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 29);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // postOfficeBorrowerBindingSource
            // 
            this.postOfficeBorrowerBindingSource.DataSource = typeof(AUXLTRS.PostOfficeBorrower);
            // 
            // PostOfficeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 191);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtSsn);
            this.Controls.Add(this.lblSsn);
            this.Controls.Add(this.grpPostOfficeInformation);
            this.Controls.Add(this.lblEnterInformation);
            this.Name = "PostOfficeDialog";
            this.Text = "Post Office Letter";
            this.grpPostOfficeInformation.ResumeLayout(false);
            this.grpPostOfficeInformation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.postOfficeBorrowerBindingSource)).EndInit();
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
        private System.Windows.Forms.TextBox txtZip;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.TextBox txtSsn;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.BindingSource postOfficeBorrowerBindingSource;
    }
}
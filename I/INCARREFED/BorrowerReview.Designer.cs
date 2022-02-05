namespace INCARREFED
{
    partial class BorrowerReview
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
            this.lblAcctNum = new System.Windows.Forms.Label();
            this.lblIsBwr = new System.Windows.Forms.Label();
            this.lblBwrName = new System.Windows.Forms.Label();
            this.txtBwrName = new System.Windows.Forms.TextBox();
            this.commentDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblActualAcctNum = new System.Windows.Forms.Label();
            this.lblEndorAcctNum = new System.Windows.Forms.Label();
            this.txtEndAcctNum = new System.Windows.Forms.TextBox();
            this.lblFacilityName = new System.Windows.Forms.Label();
            this.txtFacName = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblCity = new System.Windows.Forms.Label();
            this.txtFacCity = new System.Windows.Forms.TextBox();
            this.lblFacilityState = new System.Windows.Forms.Label();
            this.cmbState = new System.Windows.Forms.ComboBox();
            this.lblFacilityZip = new System.Windows.Forms.Label();
            this.txtZip = new System.Windows.Forms.TextBox();
            this.lblFacilityNumber = new System.Windows.Forms.Label();
            this.txtPhoneNum = new System.Windows.Forms.TextBox();
            this.lblInmateNumber = new System.Windows.Forms.Label();
            this.txtInmateNumber = new System.Windows.Forms.TextBox();
            this.lblReleaseDate = new System.Windows.Forms.Label();
            this.dtpReleaseDate = new System.Windows.Forms.DateTimePicker();
            this.lblFollowUpDate = new System.Windows.Forms.Label();
            this.dtpFollowUpDate = new System.Windows.Forms.DateTimePicker();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSource = new System.Windows.Forms.ComboBox();
            this.lblOtherInfo = new System.Windows.Forms.Label();
            this.txtOtherInfo = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.commentDataBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // lblAcctNum
            // 
            this.lblAcctNum.AutoSize = true;
            this.lblAcctNum.Location = new System.Drawing.Point(12, 37);
            this.lblAcctNum.Name = "lblAcctNum";
            this.lblAcctNum.Size = new System.Drawing.Size(142, 13);
            this.lblAcctNum.TabIndex = 1;
            this.lblAcctNum.Text = "Borrower\'s Account Number:";
            // 
            // lblIsBwr
            // 
            this.lblIsBwr.AutoSize = true;
            this.lblIsBwr.Location = new System.Drawing.Point(12, 9);
            this.lblIsBwr.Name = "lblIsBwr";
            this.lblIsBwr.Size = new System.Drawing.Size(106, 13);
            this.lblIsBwr.TabIndex = 2;
            this.lblIsBwr.Text = "Person Type             ";
            // 
            // lblBwrName
            // 
            this.lblBwrName.AutoSize = true;
            this.lblBwrName.Location = new System.Drawing.Point(12, 93);
            this.lblBwrName.Name = "lblBwrName";
            this.lblBwrName.Size = new System.Drawing.Size(85, 13);
            this.lblBwrName.TabIndex = 3;
            this.lblBwrName.Text = "Borrowers Name";
            // 
            // txtBwrName
            // 
            this.txtBwrName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.commentDataBindingSource, "BorrowerName", true));
            this.txtBwrName.Location = new System.Drawing.Point(103, 90);
            this.txtBwrName.Name = "txtBwrName";
            this.txtBwrName.Size = new System.Drawing.Size(155, 20);
            this.txtBwrName.TabIndex = 1;
            // 
            // commentDataBindingSource
            // 
            this.commentDataBindingSource.DataSource = typeof(INCARREFED.CommentData);
            // 
            // lblActualAcctNum
            // 
            this.lblActualAcctNum.AutoSize = true;
            this.lblActualAcctNum.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.commentDataBindingSource, "BorrowerAccountNumber", true));
            this.lblActualAcctNum.Location = new System.Drawing.Point(160, 37);
            this.lblActualAcctNum.Name = "lblActualAcctNum";
            this.lblActualAcctNum.Size = new System.Drawing.Size(62, 13);
            this.lblActualAcctNum.TabIndex = 5;
            this.lblActualAcctNum.Text = "UpdateThis";
            // 
            // lblEndorAcctNum
            // 
            this.lblEndorAcctNum.AutoSize = true;
            this.lblEndorAcctNum.Location = new System.Drawing.Point(12, 65);
            this.lblEndorAcctNum.Name = "lblEndorAcctNum";
            this.lblEndorAcctNum.Size = new System.Drawing.Size(147, 13);
            this.lblEndorAcctNum.TabIndex = 6;
            this.lblEndorAcctNum.Text = "Endorser/ Student Acct Num:";
            // 
            // txtEndAcctNum
            // 
            this.txtEndAcctNum.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.commentDataBindingSource, "StudentAccountNumber", true));
            this.txtEndAcctNum.Enabled = false;
            this.txtEndAcctNum.Location = new System.Drawing.Point(165, 62);
            this.txtEndAcctNum.MaxLength = 10;
            this.txtEndAcctNum.Name = "txtEndAcctNum";
            this.txtEndAcctNum.Size = new System.Drawing.Size(100, 20);
            this.txtEndAcctNum.TabIndex = 0;
            // 
            // lblFacilityName
            // 
            this.lblFacilityName.AutoSize = true;
            this.lblFacilityName.Location = new System.Drawing.Point(12, 121);
            this.lblFacilityName.Name = "lblFacilityName";
            this.lblFacilityName.Size = new System.Drawing.Size(73, 13);
            this.lblFacilityName.TabIndex = 8;
            this.lblFacilityName.Text = "Facility Name:";
            // 
            // txtFacName
            // 
            this.txtFacName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.commentDataBindingSource, "FacilityName", true));
            this.txtFacName.Location = new System.Drawing.Point(91, 118);
            this.txtFacName.MaxLength = 29;
            this.txtFacName.Name = "txtFacName";
            this.txtFacName.Size = new System.Drawing.Size(201, 20);
            this.txtFacName.TabIndex = 2;
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(12, 149);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(83, 13);
            this.lblAddress.TabIndex = 10;
            this.lblAddress.Text = "Facility Address:";
            // 
            // txtAddress
            // 
            this.txtAddress.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.commentDataBindingSource, "FacilityAddress", true));
            this.txtAddress.Location = new System.Drawing.Point(101, 146);
            this.txtAddress.MaxLength = 17;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(303, 20);
            this.txtAddress.TabIndex = 3;
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(12, 177);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(59, 13);
            this.lblCity.TabIndex = 12;
            this.lblCity.Text = "Facility City";
            // 
            // txtFacCity
            // 
            this.txtFacCity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.commentDataBindingSource, "FacilityCity", true));
            this.txtFacCity.Location = new System.Drawing.Point(77, 174);
            this.txtFacCity.MaxLength = 19;
            this.txtFacCity.Name = "txtFacCity";
            this.txtFacCity.Size = new System.Drawing.Size(124, 20);
            this.txtFacCity.TabIndex = 4;
            // 
            // lblFacilityState
            // 
            this.lblFacilityState.AutoSize = true;
            this.lblFacilityState.Location = new System.Drawing.Point(12, 205);
            this.lblFacilityState.Name = "lblFacilityState";
            this.lblFacilityState.Size = new System.Drawing.Size(70, 13);
            this.lblFacilityState.TabIndex = 14;
            this.lblFacilityState.Text = "Facility State:";
            // 
            // cmbState
            // 
            this.cmbState.FormattingEnabled = true;
            this.cmbState.Location = new System.Drawing.Point(88, 202);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(121, 21);
            this.cmbState.TabIndex = 5;
            // 
            // lblFacilityZip
            // 
            this.lblFacilityZip.AutoSize = true;
            this.lblFacilityZip.Location = new System.Drawing.Point(12, 233);
            this.lblFacilityZip.Name = "lblFacilityZip";
            this.lblFacilityZip.Size = new System.Drawing.Size(57, 13);
            this.lblFacilityZip.TabIndex = 16;
            this.lblFacilityZip.Text = "Facility Zip";
            // 
            // txtZip
            // 
            this.txtZip.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.commentDataBindingSource, "FacilityZip", true));
            this.txtZip.Location = new System.Drawing.Point(75, 230);
            this.txtZip.MaxLength = 16;
            this.txtZip.Name = "txtZip";
            this.txtZip.Size = new System.Drawing.Size(71, 20);
            this.txtZip.TabIndex = 6;
            // 
            // lblFacilityNumber
            // 
            this.lblFacilityNumber.AutoSize = true;
            this.lblFacilityNumber.Location = new System.Drawing.Point(12, 261);
            this.lblFacilityNumber.Name = "lblFacilityNumber";
            this.lblFacilityNumber.Size = new System.Drawing.Size(116, 13);
            this.lblFacilityNumber.TabIndex = 18;
            this.lblFacilityNumber.Text = "Facility Phone Number:";
            // 
            // txtPhoneNum
            // 
            this.txtPhoneNum.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.commentDataBindingSource, "FacilityPhone", true));
            this.txtPhoneNum.Location = new System.Drawing.Point(134, 258);
            this.txtPhoneNum.MaxLength = 10;
            this.txtPhoneNum.Name = "txtPhoneNum";
            this.txtPhoneNum.Size = new System.Drawing.Size(100, 20);
            this.txtPhoneNum.TabIndex = 7;
            // 
            // lblInmateNumber
            // 
            this.lblInmateNumber.AutoSize = true;
            this.lblInmateNumber.Location = new System.Drawing.Point(12, 289);
            this.lblInmateNumber.Name = "lblInmateNumber";
            this.lblInmateNumber.Size = new System.Drawing.Size(79, 13);
            this.lblInmateNumber.TabIndex = 20;
            this.lblInmateNumber.Text = "Inmate Number";
            // 
            // txtInmateNumber
            // 
            this.txtInmateNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.commentDataBindingSource, "InmateNumber", true));
            this.txtInmateNumber.Location = new System.Drawing.Point(97, 286);
            this.txtInmateNumber.MaxLength = 10;
            this.txtInmateNumber.Name = "txtInmateNumber";
            this.txtInmateNumber.Size = new System.Drawing.Size(100, 20);
            this.txtInmateNumber.TabIndex = 8;
            // 
            // lblReleaseDate
            // 
            this.lblReleaseDate.AutoSize = true;
            this.lblReleaseDate.Location = new System.Drawing.Point(12, 317);
            this.lblReleaseDate.Name = "lblReleaseDate";
            this.lblReleaseDate.Size = new System.Drawing.Size(75, 13);
            this.lblReleaseDate.TabIndex = 22;
            this.lblReleaseDate.Text = "Release Date ";
            // 
            // dtpReleaseDate
            // 
            this.dtpReleaseDate.Location = new System.Drawing.Point(93, 312);
            this.dtpReleaseDate.Name = "dtpReleaseDate";
            this.dtpReleaseDate.Size = new System.Drawing.Size(200, 20);
            this.dtpReleaseDate.TabIndex = 8;
            // 
            // lblFollowUpDate
            // 
            this.lblFollowUpDate.AutoSize = true;
            this.lblFollowUpDate.Location = new System.Drawing.Point(12, 345);
            this.lblFollowUpDate.Name = "lblFollowUpDate";
            this.lblFollowUpDate.Size = new System.Drawing.Size(80, 13);
            this.lblFollowUpDate.TabIndex = 24;
            this.lblFollowUpDate.Text = "Follow Up Date";
            // 
            // dtpFollowUpDate
            // 
            this.dtpFollowUpDate.Location = new System.Drawing.Point(98, 339);
            this.dtpFollowUpDate.Name = "dtpFollowUpDate";
            this.dtpFollowUpDate.Size = new System.Drawing.Size(200, 20);
            this.dtpFollowUpDate.TabIndex = 9;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(283, 441);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 12;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(371, 441);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 373);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Source";
            // 
            // cmbSource
            // 
            this.cmbSource.FormattingEnabled = true;
            this.cmbSource.Location = new System.Drawing.Point(59, 370);
            this.cmbSource.Name = "cmbSource";
            this.cmbSource.Size = new System.Drawing.Size(121, 21);
            this.cmbSource.TabIndex = 10;
            // 
            // lblOtherInfo
            // 
            this.lblOtherInfo.AutoSize = true;
            this.lblOtherInfo.Location = new System.Drawing.Point(12, 401);
            this.lblOtherInfo.Name = "lblOtherInfo";
            this.lblOtherInfo.Size = new System.Drawing.Size(88, 13);
            this.lblOtherInfo.TabIndex = 30;
            this.lblOtherInfo.Text = "Other Information";
            // 
            // txtOtherInfo
            // 
            this.txtOtherInfo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.commentDataBindingSource, "OtherInfo", true));
            this.txtOtherInfo.Location = new System.Drawing.Point(106, 398);
            this.txtOtherInfo.MaxLength = 30;
            this.txtOtherInfo.Name = "txtOtherInfo";
            this.txtOtherInfo.Size = new System.Drawing.Size(156, 20);
            this.txtOtherInfo.TabIndex = 11;
            // 
            // BorrowerReview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(460, 473);
            this.Controls.Add(this.txtOtherInfo);
            this.Controls.Add(this.lblOtherInfo);
            this.Controls.Add(this.cmbSource);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.dtpFollowUpDate);
            this.Controls.Add(this.lblFollowUpDate);
            this.Controls.Add(this.dtpReleaseDate);
            this.Controls.Add(this.lblReleaseDate);
            this.Controls.Add(this.txtInmateNumber);
            this.Controls.Add(this.lblInmateNumber);
            this.Controls.Add(this.txtPhoneNum);
            this.Controls.Add(this.lblFacilityNumber);
            this.Controls.Add(this.txtZip);
            this.Controls.Add(this.lblFacilityZip);
            this.Controls.Add(this.cmbState);
            this.Controls.Add(this.lblFacilityState);
            this.Controls.Add(this.txtFacCity);
            this.Controls.Add(this.lblCity);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.txtFacName);
            this.Controls.Add(this.lblFacilityName);
            this.Controls.Add(this.txtEndAcctNum);
            this.Controls.Add(this.lblEndorAcctNum);
            this.Controls.Add(this.lblActualAcctNum);
            this.Controls.Add(this.txtBwrName);
            this.Controls.Add(this.lblBwrName);
            this.Controls.Add(this.lblIsBwr);
            this.Controls.Add(this.lblAcctNum);
            this.Name = "BorrowerReview";
            this.Text = "BorrowerReview";
            ((System.ComponentModel.ISupportInitialize)(this.commentDataBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAcctNum;
        private System.Windows.Forms.Label lblIsBwr;
        private System.Windows.Forms.Label lblBwrName;
        private System.Windows.Forms.TextBox txtBwrName;
        private System.Windows.Forms.Label lblActualAcctNum;
        private System.Windows.Forms.Label lblEndorAcctNum;
        private System.Windows.Forms.TextBox txtEndAcctNum;
        private System.Windows.Forms.Label lblFacilityName;
        private System.Windows.Forms.TextBox txtFacName;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.TextBox txtFacCity;
        private System.Windows.Forms.Label lblFacilityState;
        private System.Windows.Forms.ComboBox cmbState;
        private System.Windows.Forms.Label lblFacilityZip;
        private System.Windows.Forms.TextBox txtZip;
        private System.Windows.Forms.Label lblFacilityNumber;
        private System.Windows.Forms.TextBox txtPhoneNum;
        private System.Windows.Forms.Label lblInmateNumber;
        private System.Windows.Forms.TextBox txtInmateNumber;
        private System.Windows.Forms.Label lblReleaseDate;
        private System.Windows.Forms.DateTimePicker dtpReleaseDate;
        private System.Windows.Forms.Label lblFollowUpDate;
        private System.Windows.Forms.DateTimePicker dtpFollowUpDate;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSource;
        private System.Windows.Forms.Label lblOtherInfo;
        private System.Windows.Forms.TextBox txtOtherInfo;
        private System.Windows.Forms.BindingSource commentDataBindingSource;
    }
}
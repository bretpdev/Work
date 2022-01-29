namespace CONPMTPST
{
    partial class PaymentTypeSetup
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CompassLoanType = new System.Windows.Forms.ComboBox();
            this.TivaLoanType = new System.Windows.Forms.ComboBox();
            this.Active = new System.Windows.Forms.CheckBox();
            this.Ok = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Compass Loan Type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 80);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Tiva File Loan Type";
            // 
            // CompassLoanType
            // 
            this.CompassLoanType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CompassLoanType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CompassLoanType.FormattingEnabled = true;
            this.CompassLoanType.Location = new System.Drawing.Point(17, 34);
            this.CompassLoanType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CompassLoanType.Name = "CompassLoanType";
            this.CompassLoanType.Size = new System.Drawing.Size(108, 28);
            this.CompassLoanType.TabIndex = 3;
            this.CompassLoanType.SelectedIndexChanged += new System.EventHandler(this.CompassLoanType_SelectedIndexChanged);
            // 
            // TivaLoanType
            // 
            this.TivaLoanType.FormattingEnabled = true;
            this.TivaLoanType.Location = new System.Drawing.Point(17, 105);
            this.TivaLoanType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TivaLoanType.Name = "TivaLoanType";
            this.TivaLoanType.Size = new System.Drawing.Size(64, 28);
            this.TivaLoanType.TabIndex = 4;
            // 
            // Active
            // 
            this.Active.AutoSize = true;
            this.Active.Checked = true;
            this.Active.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Active.Location = new System.Drawing.Point(17, 143);
            this.Active.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Active.Name = "Active";
            this.Active.Size = new System.Drawing.Size(71, 24);
            this.Active.TabIndex = 5;
            this.Active.Text = "Active";
            this.Active.UseVisualStyleBackColor = true;
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(102, 175);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(76, 31);
            this.Ok.TabIndex = 7;
            this.Ok.Text = "OK";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(17, 175);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(76, 31);
            this.Cancel.TabIndex = 8;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // PaymentTypeSetup
            // 
            this.AcceptButton = this.Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(190, 216);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.Active);
            this.Controls.Add(this.TivaLoanType);
            this.Controls.Add(this.CompassLoanType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(206, 254);
            this.MinimumSize = new System.Drawing.Size(206, 254);
            this.Name = "PaymentTypeSetup";
            this.Text = "Type Setup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CompassLoanType;
        private System.Windows.Forms.ComboBox TivaLoanType;
        private System.Windows.Forms.CheckBox Active;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Button Cancel;
    }
}
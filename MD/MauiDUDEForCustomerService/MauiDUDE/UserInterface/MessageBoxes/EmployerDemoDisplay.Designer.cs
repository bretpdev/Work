namespace MauiDUDE
{
    partial class EmployerDemoDisplay
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.textBoxAddress1 = new System.Windows.Forms.TextBox();
            this.textBoxAddress2 = new System.Windows.Forms.TextBox();
            this.textBoxCity = new System.Windows.Forms.TextBox();
            this.textBoxState = new System.Windows.Forms.TextBox();
            this.maskedTextBoxZIP = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxPhone = new System.Windows.Forms.MaskedTextBox();
            this.employerDemographicsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.employerDemographicsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Employer Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Address #1:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Address #2:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "City/State/ZIP:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Phone#:";
            // 
            // textBoxName
            // 
            this.textBoxName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.employerDemographicsBindingSource, "Name", true));
            this.textBoxName.Location = new System.Drawing.Point(129, 3);
            this.textBoxName.Multiline = true;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.ReadOnly = true;
            this.textBoxName.Size = new System.Drawing.Size(228, 32);
            this.textBoxName.TabIndex = 5;
            // 
            // textBoxAddress1
            // 
            this.textBoxAddress1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.employerDemographicsBindingSource, "Addr1", true));
            this.textBoxAddress1.Location = new System.Drawing.Point(129, 36);
            this.textBoxAddress1.Name = "textBoxAddress1";
            this.textBoxAddress1.ReadOnly = true;
            this.textBoxAddress1.Size = new System.Drawing.Size(228, 20);
            this.textBoxAddress1.TabIndex = 6;
            // 
            // textBoxAddress2
            // 
            this.textBoxAddress2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.employerDemographicsBindingSource, "Addr2", true));
            this.textBoxAddress2.Location = new System.Drawing.Point(129, 58);
            this.textBoxAddress2.Name = "textBoxAddress2";
            this.textBoxAddress2.ReadOnly = true;
            this.textBoxAddress2.Size = new System.Drawing.Size(228, 20);
            this.textBoxAddress2.TabIndex = 7;
            // 
            // textBoxCity
            // 
            this.textBoxCity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.employerDemographicsBindingSource, "City", true));
            this.textBoxCity.Location = new System.Drawing.Point(129, 80);
            this.textBoxCity.Name = "textBoxCity";
            this.textBoxCity.ReadOnly = true;
            this.textBoxCity.Size = new System.Drawing.Size(136, 20);
            this.textBoxCity.TabIndex = 8;
            // 
            // textBoxState
            // 
            this.textBoxState.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.employerDemographicsBindingSource, "State", true));
            this.textBoxState.Location = new System.Drawing.Point(267, 80);
            this.textBoxState.Name = "textBoxState";
            this.textBoxState.ReadOnly = true;
            this.textBoxState.Size = new System.Drawing.Size(27, 20);
            this.textBoxState.TabIndex = 9;
            // 
            // maskedTextBoxZIP
            // 
            this.maskedTextBoxZIP.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.employerDemographicsBindingSource, "Zip", true));
            this.maskedTextBoxZIP.Location = new System.Drawing.Point(296, 80);
            this.maskedTextBoxZIP.Mask = "00000-9999";
            this.maskedTextBoxZIP.Name = "maskedTextBoxZIP";
            this.maskedTextBoxZIP.ReadOnly = true;
            this.maskedTextBoxZIP.Size = new System.Drawing.Size(61, 20);
            this.maskedTextBoxZIP.TabIndex = 10;
            // 
            // maskedTextBoxPhone
            // 
            this.maskedTextBoxPhone.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.employerDemographicsBindingSource, "Phone", true));
            this.maskedTextBoxPhone.Location = new System.Drawing.Point(129, 102);
            this.maskedTextBoxPhone.Mask = "(999)000-0000";
            this.maskedTextBoxPhone.Name = "maskedTextBoxPhone";
            this.maskedTextBoxPhone.ReadOnly = true;
            this.maskedTextBoxPhone.Size = new System.Drawing.Size(136, 20);
            this.maskedTextBoxPhone.TabIndex = 11;
            // 
            // employerDemographicsBindingSource
            // 
            this.employerDemographicsBindingSource.DataSource = typeof(MauiDUDE.EmployerDemographics);
            // 
            // EmployerDemoDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.maskedTextBoxPhone);
            this.Controls.Add(this.maskedTextBoxZIP);
            this.Controls.Add(this.textBoxState);
            this.Controls.Add(this.textBoxCity);
            this.Controls.Add(this.textBoxAddress2);
            this.Controls.Add(this.textBoxAddress1);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "EmployerDemoDisplay";
            this.Size = new System.Drawing.Size(360, 126);
            ((System.ComponentModel.ISupportInitialize)(this.employerDemographicsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.TextBox textBoxAddress1;
        private System.Windows.Forms.TextBox textBoxAddress2;
        private System.Windows.Forms.TextBox textBoxCity;
        private System.Windows.Forms.TextBox textBoxState;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxZIP;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxPhone;
        private System.Windows.Forms.BindingSource employerDemographicsBindingSource;
    }
}

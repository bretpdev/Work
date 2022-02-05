namespace MauiDUDE
{
    partial class HomePageDemographics
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
            this.labelAddress1 = new System.Windows.Forms.Label();
            this.labelAddress2 = new System.Windows.Forms.Label();
            this.labelCityState = new System.Windows.Forms.Label();
            this.labelZIP = new System.Windows.Forms.Label();
            this.labelForeignCountryState = new System.Windows.Forms.Label();
            this.labelHomePhone = new System.Windows.Forms.Label();
            this.labelAltPhone = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxAddress1 = new System.Windows.Forms.TextBox();
            this.textBoxAddress2 = new System.Windows.Forms.TextBox();
            this.textBoxCity = new System.Windows.Forms.TextBox();
            this.textBoxState = new System.Windows.Forms.TextBox();
            this.textBoxZIP = new System.Windows.Forms.TextBox();
            this.textBoxForeignCountry = new System.Windows.Forms.TextBox();
            this.textBoxForeignState = new System.Windows.Forms.TextBox();
            this.textBoxHomePhone = new System.Windows.Forms.TextBox();
            this.textBoxHomeExt = new System.Windows.Forms.TextBox();
            this.textBoxAltPhone = new System.Windows.Forms.TextBox();
            this.textBoxAltExt = new System.Windows.Forms.TextBox();
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.demographicsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.demographicsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // labelAddress1
            // 
            this.labelAddress1.AutoSize = true;
            this.labelAddress1.Location = new System.Drawing.Point(4, 4);
            this.labelAddress1.Name = "labelAddress1";
            this.labelAddress1.Size = new System.Drawing.Size(64, 13);
            this.labelAddress1.TabIndex = 0;
            this.labelAddress1.Text = "Address #1:";
            // 
            // labelAddress2
            // 
            this.labelAddress2.AutoSize = true;
            this.labelAddress2.Location = new System.Drawing.Point(4, 27);
            this.labelAddress2.Name = "labelAddress2";
            this.labelAddress2.Size = new System.Drawing.Size(64, 13);
            this.labelAddress2.TabIndex = 1;
            this.labelAddress2.Text = "Address #2:";
            // 
            // labelCityState
            // 
            this.labelCityState.AutoSize = true;
            this.labelCityState.Location = new System.Drawing.Point(4, 49);
            this.labelCityState.Name = "labelCityState";
            this.labelCityState.Size = new System.Drawing.Size(57, 13);
            this.labelCityState.TabIndex = 2;
            this.labelCityState.Text = "City/State:";
            // 
            // labelZIP
            // 
            this.labelZIP.AutoSize = true;
            this.labelZIP.Location = new System.Drawing.Point(4, 71);
            this.labelZIP.Name = "labelZIP";
            this.labelZIP.Size = new System.Drawing.Size(27, 13);
            this.labelZIP.TabIndex = 3;
            this.labelZIP.Text = "ZIP:";
            // 
            // labelForeignCountryState
            // 
            this.labelForeignCountryState.AutoSize = true;
            this.labelForeignCountryState.Location = new System.Drawing.Point(4, 91);
            this.labelForeignCountryState.Name = "labelForeignCountryState";
            this.labelForeignCountryState.Size = new System.Drawing.Size(91, 13);
            this.labelForeignCountryState.TabIndex = 4;
            this.labelForeignCountryState.Text = "Fn. Country/State";
            // 
            // labelHomePhone
            // 
            this.labelHomePhone.AutoSize = true;
            this.labelHomePhone.Location = new System.Drawing.Point(4, 114);
            this.labelHomePhone.Name = "labelHomePhone";
            this.labelHomePhone.Size = new System.Drawing.Size(99, 13);
            this.labelHomePhone.TabIndex = 5;
            this.labelHomePhone.Text = "Home Phone#/Ext:";
            // 
            // labelAltPhone
            // 
            this.labelAltPhone.AutoSize = true;
            this.labelAltPhone.Location = new System.Drawing.Point(4, 138);
            this.labelAltPhone.Name = "labelAltPhone";
            this.labelAltPhone.Size = new System.Drawing.Size(83, 13);
            this.labelAltPhone.TabIndex = 6;
            this.labelAltPhone.Text = "Alt Phone#/Ext:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 160);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Email:";
            // 
            // textBoxAddress1
            // 
            this.textBoxAddress1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.demographicsBindingSource, "Addr1", true));
            this.textBoxAddress1.Location = new System.Drawing.Point(126, 1);
            this.textBoxAddress1.Name = "textBoxAddress1";
            this.textBoxAddress1.Size = new System.Drawing.Size(228, 20);
            this.textBoxAddress1.TabIndex = 8;
            // 
            // textBoxAddress2
            // 
            this.textBoxAddress2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.demographicsBindingSource, "Addr2", true));
            this.textBoxAddress2.Location = new System.Drawing.Point(126, 23);
            this.textBoxAddress2.Name = "textBoxAddress2";
            this.textBoxAddress2.Size = new System.Drawing.Size(228, 20);
            this.textBoxAddress2.TabIndex = 9;
            // 
            // textBoxCity
            // 
            this.textBoxCity.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.demographicsBindingSource, "City", true));
            this.textBoxCity.Location = new System.Drawing.Point(126, 45);
            this.textBoxCity.Name = "textBoxCity";
            this.textBoxCity.Size = new System.Drawing.Size(136, 20);
            this.textBoxCity.TabIndex = 10;
            // 
            // textBoxState
            // 
            this.textBoxState.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.demographicsBindingSource, "State", true));
            this.textBoxState.Location = new System.Drawing.Point(264, 45);
            this.textBoxState.Name = "textBoxState";
            this.textBoxState.Size = new System.Drawing.Size(27, 20);
            this.textBoxState.TabIndex = 11;
            // 
            // textBoxZIP
            // 
            this.textBoxZIP.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.demographicsBindingSource, "Zip", true));
            this.textBoxZIP.Location = new System.Drawing.Point(126, 66);
            this.textBoxZIP.Name = "textBoxZIP";
            this.textBoxZIP.Size = new System.Drawing.Size(136, 20);
            this.textBoxZIP.TabIndex = 12;
            // 
            // textBoxForeignCountry
            // 
            this.textBoxForeignCountry.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.demographicsBindingSource, "Country", true));
            this.textBoxForeignCountry.Location = new System.Drawing.Point(126, 88);
            this.textBoxForeignCountry.Name = "textBoxForeignCountry";
            this.textBoxForeignCountry.Size = new System.Drawing.Size(136, 20);
            this.textBoxForeignCountry.TabIndex = 13;
            // 
            // textBoxForeignState
            // 
            this.textBoxForeignState.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.demographicsBindingSource, "ForeignState", true));
            this.textBoxForeignState.Location = new System.Drawing.Point(264, 88);
            this.textBoxForeignState.Name = "textBoxForeignState";
            this.textBoxForeignState.Size = new System.Drawing.Size(90, 20);
            this.textBoxForeignState.TabIndex = 14;
            // 
            // textBoxHomePhone
            // 
            this.textBoxHomePhone.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.demographicsBindingSource, "HomePhoneNum", true));
            this.textBoxHomePhone.Location = new System.Drawing.Point(126, 110);
            this.textBoxHomePhone.Name = "textBoxHomePhone";
            this.textBoxHomePhone.Size = new System.Drawing.Size(136, 20);
            this.textBoxHomePhone.TabIndex = 15;
            // 
            // textBoxHomeExt
            // 
            this.textBoxHomeExt.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.demographicsBindingSource, "HomePhoneExt", true));
            this.textBoxHomeExt.Location = new System.Drawing.Point(264, 110);
            this.textBoxHomeExt.Name = "textBoxHomeExt";
            this.textBoxHomeExt.Size = new System.Drawing.Size(27, 20);
            this.textBoxHomeExt.TabIndex = 16;
            // 
            // textBoxAltPhone
            // 
            this.textBoxAltPhone.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.demographicsBindingSource, "OtherPhoneNum", true));
            this.textBoxAltPhone.Location = new System.Drawing.Point(126, 131);
            this.textBoxAltPhone.Name = "textBoxAltPhone";
            this.textBoxAltPhone.Size = new System.Drawing.Size(136, 20);
            this.textBoxAltPhone.TabIndex = 17;
            // 
            // textBoxAltExt
            // 
            this.textBoxAltExt.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.demographicsBindingSource, "OtherPhoneExt", true));
            this.textBoxAltExt.Location = new System.Drawing.Point(264, 131);
            this.textBoxAltExt.Name = "textBoxAltExt";
            this.textBoxAltExt.Size = new System.Drawing.Size(27, 20);
            this.textBoxAltExt.TabIndex = 18;
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.demographicsBindingSource, "Email", true));
            this.textBoxEmail.Location = new System.Drawing.Point(126, 153);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new System.Drawing.Size(228, 20);
            this.textBoxEmail.TabIndex = 19;
            // 
            // demographicsBindingSource
            // 
            this.demographicsBindingSource.DataSource = typeof(MauiDUDE.Demographics);
            // 
            // HomePageDemographics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxEmail);
            this.Controls.Add(this.textBoxAltExt);
            this.Controls.Add(this.textBoxAltPhone);
            this.Controls.Add(this.textBoxHomeExt);
            this.Controls.Add(this.textBoxHomePhone);
            this.Controls.Add(this.textBoxForeignState);
            this.Controls.Add(this.textBoxForeignCountry);
            this.Controls.Add(this.textBoxZIP);
            this.Controls.Add(this.textBoxState);
            this.Controls.Add(this.textBoxCity);
            this.Controls.Add(this.textBoxAddress2);
            this.Controls.Add(this.textBoxAddress1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.labelAltPhone);
            this.Controls.Add(this.labelHomePhone);
            this.Controls.Add(this.labelForeignCountryState);
            this.Controls.Add(this.labelZIP);
            this.Controls.Add(this.labelCityState);
            this.Controls.Add(this.labelAddress2);
            this.Controls.Add(this.labelAddress1);
            this.Name = "HomePageDemographics";
            this.Size = new System.Drawing.Size(359, 191);
            ((System.ComponentModel.ISupportInitialize)(this.demographicsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAddress1;
        private System.Windows.Forms.Label labelAddress2;
        private System.Windows.Forms.Label labelCityState;
        private System.Windows.Forms.Label labelZIP;
        private System.Windows.Forms.Label labelForeignCountryState;
        private System.Windows.Forms.Label labelHomePhone;
        private System.Windows.Forms.BindingSource demographicsBindingSource;
        public System.Windows.Forms.TextBox textBoxAddress1;
        public System.Windows.Forms.TextBox textBoxAddress2;
        public System.Windows.Forms.TextBox textBoxCity;
        public System.Windows.Forms.TextBox textBoxState;
        public System.Windows.Forms.TextBox textBoxZIP;
        public System.Windows.Forms.TextBox textBoxForeignCountry;
        public System.Windows.Forms.TextBox textBoxForeignState;
        public System.Windows.Forms.TextBox textBoxHomePhone;
        public System.Windows.Forms.TextBox textBoxHomeExt;
        public System.Windows.Forms.TextBox textBoxAltPhone;
        public System.Windows.Forms.TextBox textBoxAltExt;
        public System.Windows.Forms.TextBox textBoxEmail;
        public System.Windows.Forms.Label label7;
        public System.Windows.Forms.Label labelAltPhone;
    }
}

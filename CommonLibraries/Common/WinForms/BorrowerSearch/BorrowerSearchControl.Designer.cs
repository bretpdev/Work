namespace Uheaa.Common.WinForms
{
    partial class BorrowerSearchControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.StatusBox = new System.Windows.Forms.PictureBox();
            this.ResetButton = new System.Windows.Forms.Button();
            this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.EmailBox = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.PhoneBox = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.RegionButton = new Uheaa.Common.WinForms.RegionSelectionCycleButton();
            this.FirstNameBox = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.ZipBox = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.StateBox = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.CityBox = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.AddressBox = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.DOBBox = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.MiddleInitialBox = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.LastNameBox = new Uheaa.Common.WinForms.WatermarkTextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StatusBox)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SearchButton);
            this.groupBox1.Controls.Add(this.StatusBox);
            this.groupBox1.Controls.Add(this.EmailBox);
            this.groupBox1.Controls.Add(this.PhoneBox);
            this.groupBox1.Controls.Add(this.ResetButton);
            this.groupBox1.Controls.Add(this.RegionButton);
            this.groupBox1.Controls.Add(this.FirstNameBox);
            this.groupBox1.Controls.Add(this.ZipBox);
            this.groupBox1.Controls.Add(this.StateBox);
            this.groupBox1.Controls.Add(this.CityBox);
            this.groupBox1.Controls.Add(this.AddressBox);
            this.groupBox1.Controls.Add(this.DOBBox);
            this.groupBox1.Controls.Add(this.MiddleInitialBox);
            this.groupBox1.Controls.Add(this.LastNameBox);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(370, 230);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Borrower Search";
            // 
            // SearchButton
            // 
            this.SearchButton.Enabled = false;
            this.SearchButton.Location = new System.Drawing.Point(269, 190);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(87, 25);
            this.SearchButton.TabIndex = 13;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // StatusBox
            // 
            this.StatusBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.StatusBox.Location = new System.Drawing.Point(332, 190);
            this.StatusBox.Name = "StatusBox";
            this.StatusBox.Size = new System.Drawing.Size(24, 25);
            this.StatusBox.TabIndex = 12;
            this.StatusBox.TabStop = false;
            // 
            // ResetButton
            // 
            this.ResetButton.Enabled = false;
            this.ResetButton.Location = new System.Drawing.Point(184, 190);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(79, 25);
            this.ResetButton.TabIndex = 11;
            this.ResetButton.Text = "(reset)";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // EmailBox
            // 
            this.EmailBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.EmailBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.EmailBox.Location = new System.Drawing.Point(184, 153);
            this.EmailBox.Name = "EmailBox";
            this.EmailBox.Size = new System.Drawing.Size(172, 26);
            this.EmailBox.TabIndex = 9;
            this.EmailBox.Text = "Email Address";
            this.EmailBox.Watermark = "Email Address";
            this.EmailBox.TextChanged += new System.EventHandler(this.Set_Clearfields);
            this.EmailBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Enter_Pressed);
            // 
            // PhoneBox
            // 
            this.PhoneBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.PhoneBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.PhoneBox.Location = new System.Drawing.Point(6, 153);
            this.PhoneBox.Name = "PhoneBox";
            this.PhoneBox.Size = new System.Drawing.Size(172, 26);
            this.PhoneBox.TabIndex = 4;
            this.PhoneBox.Text = "Phone Number";
            this.PhoneBox.Watermark = "Phone Number";
            this.PhoneBox.TextChanged += new System.EventHandler(this.Set_Clearfields);
            this.PhoneBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Enter_Pressed);
            // 
            // RegionButton
            // 
            this.RegionButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RegionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.RegionButton.Location = new System.Drawing.Point(6, 190);
            this.RegionButton.Name = "RegionButton";
            this.RegionButton.OnelinkEnabled = true;
            this.RegionButton.SelectedIndex = 0;
            this.RegionButton.SelectedValue = Uheaa.Common.WinForms.RegionSelectionEnum.Uheaa;
            this.RegionButton.Size = new System.Drawing.Size(172, 25);
            this.RegionButton.TabIndex = 10;
            this.RegionButton.UheaaEnabled = true;
            this.RegionButton.UseVisualStyleBackColor = true;
            this.RegionButton.Cycle += new Uheaa.Common.WinForms.CycleButton<Uheaa.Common.WinForms.RegionSelectionEnum>.OnCycle(this.RegionButton_Cycle);
            // 
            // FirstNameBox
            // 
            this.FirstNameBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.FirstNameBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.FirstNameBox.Location = new System.Drawing.Point(6, 25);
            this.FirstNameBox.Name = "FirstNameBox";
            this.FirstNameBox.Size = new System.Drawing.Size(172, 26);
            this.FirstNameBox.TabIndex = 0;
            this.FirstNameBox.Text = "First Name";
            this.FirstNameBox.Watermark = "First Name";
            this.FirstNameBox.TextChanged += new System.EventHandler(this.Set_Clearfields);
            this.FirstNameBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Enter_Pressed);
            // 
            // ZipBox
            // 
            this.ZipBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.ZipBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.ZipBox.Location = new System.Drawing.Point(184, 121);
            this.ZipBox.Name = "ZipBox";
            this.ZipBox.Size = new System.Drawing.Size(172, 26);
            this.ZipBox.TabIndex = 8;
            this.ZipBox.Text = "Zip";
            this.ZipBox.Watermark = "Zip";
            this.ZipBox.TextChanged += new System.EventHandler(this.Set_Clearfields);
            this.ZipBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Enter_Pressed);
            // 
            // StateBox
            // 
            this.StateBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.StateBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.StateBox.Location = new System.Drawing.Point(184, 89);
            this.StateBox.MaxLength = 2;
            this.StateBox.Name = "StateBox";
            this.StateBox.Size = new System.Drawing.Size(172, 26);
            this.StateBox.TabIndex = 7;
            this.StateBox.Text = "StateCode";
            this.StateBox.Watermark = "StateCode";
            this.StateBox.TextChanged += new System.EventHandler(this.Set_Clearfields);
            this.StateBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Enter_Pressed);
            // 
            // CityBox
            // 
            this.CityBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.CityBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.CityBox.Location = new System.Drawing.Point(184, 57);
            this.CityBox.Name = "CityBox";
            this.CityBox.Size = new System.Drawing.Size(172, 26);
            this.CityBox.TabIndex = 6;
            this.CityBox.Text = "City";
            this.CityBox.Watermark = "City";
            this.CityBox.TextChanged += new System.EventHandler(this.Set_Clearfields);
            this.CityBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Enter_Pressed);
            // 
            // AddressBox
            // 
            this.AddressBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.AddressBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.AddressBox.Location = new System.Drawing.Point(184, 25);
            this.AddressBox.Name = "AddressBox";
            this.AddressBox.Size = new System.Drawing.Size(172, 26);
            this.AddressBox.TabIndex = 5;
            this.AddressBox.Text = "Address";
            this.AddressBox.Watermark = "Address";
            this.AddressBox.TextChanged += new System.EventHandler(this.Set_Clearfields);
            this.AddressBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Enter_Pressed);
            // 
            // DOBBox
            // 
            this.DOBBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.DOBBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.DOBBox.Location = new System.Drawing.Point(6, 121);
            this.DOBBox.Name = "DOBBox";
            this.DOBBox.Size = new System.Drawing.Size(172, 26);
            this.DOBBox.TabIndex = 3;
            this.DOBBox.Text = "Date of Birth";
            this.DOBBox.Watermark = "Date of Birth";
            this.DOBBox.TextChanged += new System.EventHandler(this.Set_Clearfields);
            this.DOBBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Enter_Pressed);
            // 
            // MiddleInitialBox
            // 
            this.MiddleInitialBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.MiddleInitialBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.MiddleInitialBox.Location = new System.Drawing.Point(6, 89);
            this.MiddleInitialBox.MaxLength = 1;
            this.MiddleInitialBox.Name = "MiddleInitialBox";
            this.MiddleInitialBox.Size = new System.Drawing.Size(172, 26);
            this.MiddleInitialBox.TabIndex = 2;
            this.MiddleInitialBox.Text = "Initial";
            this.MiddleInitialBox.Watermark = "Initial";
            this.MiddleInitialBox.TextChanged += new System.EventHandler(this.Set_Clearfields);
            this.MiddleInitialBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Enter_Pressed);
            // 
            // LastNameBox
            // 
            this.LastNameBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic);
            this.LastNameBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.LastNameBox.Location = new System.Drawing.Point(6, 57);
            this.LastNameBox.Name = "LastNameBox";
            this.LastNameBox.Size = new System.Drawing.Size(172, 26);
            this.LastNameBox.TabIndex = 1;
            this.LastNameBox.Text = "Last Name";
            this.LastNameBox.Watermark = "Last Name";
            this.LastNameBox.TextChanged += new System.EventHandler(this.Set_Clearfields);
            this.LastNameBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Enter_Pressed);
            // 
            // BorrowerSearchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "BorrowerSearchControl";
            this.Size = new System.Drawing.Size(370, 230);
            this.SizeChanged += new System.EventHandler(this.BorrowerSearchControl_SizeChanged);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.BorrowerSearchControl_Layout);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StatusBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Uheaa.Common.WinForms.WatermarkTextBox MiddleInitialBox;
        private Uheaa.Common.WinForms.WatermarkTextBox LastNameBox;
        private WatermarkTextBox DOBBox;
        private WatermarkTextBox ZipBox;
        private WatermarkTextBox StateBox;
        private WatermarkTextBox CityBox;
        private WatermarkTextBox AddressBox;
        private WatermarkTextBox FirstNameBox;
        private RegionSelectionCycleButton RegionButton;
        private System.Windows.Forms.Button ResetButton;
        private WatermarkTextBox EmailBox;
        private WatermarkTextBox PhoneBox;
        private System.Windows.Forms.PictureBox StatusBox;
        private System.Windows.Forms.ToolTip MainToolTip;
        private System.Windows.Forms.Button SearchButton;
    }
}

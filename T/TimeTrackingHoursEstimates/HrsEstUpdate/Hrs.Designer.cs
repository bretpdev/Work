namespace HrsEstUpdate
{
    partial class Hrs
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
            this.label1 = new System.Windows.Forms.Label();
            this.RequestType = new System.Windows.Forms.ComboBox();
            this.EstData = new System.Windows.Forms.DataGridView();
            this.Update = new System.Windows.Forms.Button();
            this.Close = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Clear = new System.Windows.Forms.Button();
            this.Emp = new System.Windows.Forms.Label();
            this.rea = new System.Windows.Forms.Label();
            this.reaText = new System.Windows.Forms.TextBox();
            this.attach = new System.Windows.Forms.Button();
            this.file = new System.Windows.Forms.Label();
            this.addhrs = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TestHrs = new Uheaa.Common.WinForms.RequiredNumericTextBox();
            this.AdditionalHrs = new Uheaa.Common.WinForms.NumericTextBox();
            this.RequestNumber = new Uheaa.Common.WinForms.NumericTextBox();
            this.HrsEst = new Uheaa.Common.WinForms.NumericTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.EstData)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Request Type:";
            // 
            // RequestType
            // 
            this.RequestType.FormattingEnabled = true;
            this.RequestType.Location = new System.Drawing.Point(150, 42);
            this.RequestType.Name = "RequestType";
            this.RequestType.Size = new System.Drawing.Size(171, 28);
            this.RequestType.TabIndex = 2;
            // 
            // EstData
            // 
            this.EstData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EstData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.EstData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.EstData.Location = new System.Drawing.Point(338, 37);
            this.EstData.Name = "EstData";
            this.EstData.RowHeadersVisible = false;
            this.EstData.Size = new System.Drawing.Size(441, 487);
            this.EstData.TabIndex = 17;
            this.EstData.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.EstData_CellDoubleClick);
            // 
            // Update
            // 
            this.Update.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Update.Location = new System.Drawing.Point(257, 488);
            this.Update.Name = "Update";
            this.Update.Size = new System.Drawing.Size(75, 37);
            this.Update.TabIndex = 15;
            this.Update.Text = "Save";
            this.Update.UseVisualStyleBackColor = true;
            this.Update.Click += new System.EventHandler(this.Update_Click);
            // 
            // Close
            // 
            this.Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close.Location = new System.Drawing.Point(16, 488);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(75, 37);
            this.Close.TabIndex = 13;
            this.Close.Text = "Close";
            this.Close.UseVisualStyleBackColor = true;
            this.Close.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(340, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 20);
            this.label3.TabIndex = 16;
            this.label3.Text = "Existing Estimates";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "Request Number:";
            // 
            // Clear
            // 
            this.Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Clear.Location = new System.Drawing.Point(176, 488);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(75, 37);
            this.Clear.TabIndex = 14;
            this.Clear.Text = "Clear";
            this.Clear.UseVisualStyleBackColor = true;
            this.Clear.Click += new System.EventHandler(this.button3_Click);
            // 
            // Emp
            // 
            this.Emp.AutoSize = true;
            this.Emp.Location = new System.Drawing.Point(12, 10);
            this.Emp.Name = "Emp";
            this.Emp.Size = new System.Drawing.Size(94, 20);
            this.Emp.TabIndex = 0;
            this.Emp.Text = "Jarom Ryan";
            // 
            // rea
            // 
            this.rea.AutoSize = true;
            this.rea.Location = new System.Drawing.Point(66, 219);
            this.rea.Name = "rea";
            this.rea.Size = new System.Drawing.Size(173, 20);
            this.rea.TabIndex = 9;
            this.rea.Text = "Reason for Adjustment";
            this.rea.Visible = false;
            // 
            // reaText
            // 
            this.reaText.Location = new System.Drawing.Point(12, 242);
            this.reaText.Multiline = true;
            this.reaText.Name = "reaText";
            this.reaText.Size = new System.Drawing.Size(309, 157);
            this.reaText.TabIndex = 10;
            this.reaText.Visible = false;
            // 
            // attach
            // 
            this.attach.Location = new System.Drawing.Point(211, 402);
            this.attach.Name = "attach";
            this.attach.Size = new System.Drawing.Size(110, 30);
            this.attach.TabIndex = 12;
            this.attach.Text = "Attach Email";
            this.attach.UseVisualStyleBackColor = true;
            this.attach.Visible = false;
            this.attach.Click += new System.EventHandler(this.attach_Click);
            // 
            // file
            // 
            this.file.AutoSize = true;
            this.file.Location = new System.Drawing.Point(8, 382);
            this.file.Name = "file";
            this.file.Size = new System.Drawing.Size(0, 20);
            this.file.TabIndex = 11;
            this.file.Visible = false;
            // 
            // addhrs
            // 
            this.addhrs.AutoSize = true;
            this.addhrs.Location = new System.Drawing.Point(14, 187);
            this.addhrs.Name = "addhrs";
            this.addhrs.Size = new System.Drawing.Size(130, 20);
            this.addhrs.TabIndex = 7;
            this.addhrs.Text = "Additional Hours:";
            this.addhrs.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Code Hours:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 151);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 20);
            this.label5.TabIndex = 18;
            this.label5.Text = "Testing Hours:";
            // 
            // TestHrs
            // 
            this.TestHrs.AllowedSpecialCharacters = "";
            this.TestHrs.Location = new System.Drawing.Point(150, 145);
            this.TestHrs.Name = "TestHrs";
            this.TestHrs.Size = new System.Drawing.Size(171, 26);
            this.TestHrs.TabIndex = 19;
            // 
            // AdditionalHrs
            // 
            this.AdditionalHrs.AllowedSpecialCharacters = ".";
            this.AdditionalHrs.Location = new System.Drawing.Point(150, 184);
            this.AdditionalHrs.Name = "AdditionalHrs";
            this.AdditionalHrs.Size = new System.Drawing.Size(171, 26);
            this.AdditionalHrs.TabIndex = 8;
            this.AdditionalHrs.Visible = false;
            // 
            // RequestNumber
            // 
            this.RequestNumber.AllowedSpecialCharacters = ".";
            this.RequestNumber.Location = new System.Drawing.Point(150, 77);
            this.RequestNumber.Name = "RequestNumber";
            this.RequestNumber.Size = new System.Drawing.Size(171, 26);
            this.RequestNumber.TabIndex = 4;
            // 
            // HrsEst
            // 
            this.HrsEst.AllowedSpecialCharacters = ".";
            this.HrsEst.Location = new System.Drawing.Point(150, 111);
            this.HrsEst.Name = "HrsEst";
            this.HrsEst.Size = new System.Drawing.Size(171, 26);
            this.HrsEst.TabIndex = 6;
            // 
            // Hrs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 536);
            this.Controls.Add(this.TestHrs);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.AdditionalHrs);
            this.Controls.Add(this.addhrs);
            this.Controls.Add(this.file);
            this.Controls.Add(this.attach);
            this.Controls.Add(this.reaText);
            this.Controls.Add(this.rea);
            this.Controls.Add(this.Emp);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.RequestNumber);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.Update);
            this.Controls.Add(this.EstData);
            this.Controls.Add(this.HrsEst);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RequestType);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(807, 518);
            this.Name = "Hrs";
            this.Text = "Time Update";
            this.Load += new System.EventHandler(this.Hrs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.EstData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox RequestType;
        private Uheaa.Common.WinForms.NumericTextBox HrsEst;
        private System.Windows.Forms.DataGridView EstData;
        private System.Windows.Forms.Button Update;
        private System.Windows.Forms.Button Close;
        private System.Windows.Forms.Label label3;
        private Uheaa.Common.WinForms.NumericTextBox RequestNumber;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Clear;
        private System.Windows.Forms.Label Emp;
        private System.Windows.Forms.Label rea;
        private System.Windows.Forms.TextBox reaText;
        private System.Windows.Forms.Button attach;
        private System.Windows.Forms.Label file;
        private Uheaa.Common.WinForms.NumericTextBox AdditionalHrs;
        private System.Windows.Forms.Label addhrs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private Uheaa.Common.WinForms.RequiredNumericTextBox TestHrs;
    }
}


namespace FileSeparator
{
    partial class SeparationSelection
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
            this.Cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.chkheaderRow = new System.Windows.Forms.CheckBox();
            this.NumberPerFile = new System.Windows.Forms.TextBox();
            this.NumberOfFiles = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.totalRows = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(18, 152);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 17;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(139, 152);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 16;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // chkheaderRow
            // 
            this.chkheaderRow.AutoSize = true;
            this.chkheaderRow.Checked = true;
            this.chkheaderRow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkheaderRow.Location = new System.Drawing.Point(127, 127);
            this.chkheaderRow.Name = "chkheaderRow";
            this.chkheaderRow.Size = new System.Drawing.Size(86, 17);
            this.chkheaderRow.TabIndex = 15;
            this.chkheaderRow.Text = "Header Row";
            this.chkheaderRow.UseVisualStyleBackColor = true;
            // 
            // NumberPerFile
            // 
            this.NumberPerFile.Location = new System.Drawing.Point(127, 51);
            this.NumberPerFile.Name = "NumberPerFile";
            this.NumberPerFile.Size = new System.Drawing.Size(57, 20);
            this.NumberPerFile.TabIndex = 14;
            this.NumberPerFile.TextChanged += new System.EventHandler(this.NumberPerFile_TextChanged);
            // 
            // NumberOfFiles
            // 
            this.NumberOfFiles.AutoSize = true;
            this.NumberOfFiles.Location = new System.Drawing.Point(127, 91);
            this.NumberOfFiles.Name = "NumberOfFiles";
            this.NumberOfFiles.Size = new System.Drawing.Size(0, 13);
            this.NumberOfFiles.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Number of new Files:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Number per file:";
            // 
            // totalRows
            // 
            this.totalRows.AutoSize = true;
            this.totalRows.Location = new System.Drawing.Point(127, 16);
            this.totalRows.Name = "totalRows";
            this.totalRows.Size = new System.Drawing.Size(0, 13);
            this.totalRows.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Total Rows:";
            // 
            // SeparationSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(228, 191);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.chkheaderRow);
            this.Controls.Add(this.NumberPerFile);
            this.Controls.Add(this.NumberOfFiles);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.totalRows);
            this.Controls.Add(this.label1);
            this.Name = "SeparationSelection";
            this.Text = "SeparationSelection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.CheckBox chkheaderRow;
        private System.Windows.Forms.TextBox NumberPerFile;
        private System.Windows.Forms.Label NumberOfFiles;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label totalRows;
        private System.Windows.Forms.Label label1;
    }
}
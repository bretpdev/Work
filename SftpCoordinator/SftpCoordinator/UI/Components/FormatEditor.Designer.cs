namespace SftpCoordinator
{
    partial class FormatEditor
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
            this.FormatStringBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DateTimeBox = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.FilenameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.OutputBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.DateOnlyButton = new System.Windows.Forms.Button();
            this.FileExtButton = new System.Windows.Forms.Button();
            this.TimeOnlyButton = new System.Windows.Forms.Button();
            this.FilenameButton = new System.Windows.Forms.Button();
            this.DateTimeButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // FormatStringBox
            // 
            this.FormatStringBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormatStringBox.Location = new System.Drawing.Point(9, 39);
            this.FormatStringBox.Name = "FormatStringBox";
            this.FormatStringBox.Size = new System.Drawing.Size(305, 26);
            this.FormatStringBox.TabIndex = 0;
            this.FormatStringBox.TextChanged += new System.EventHandler(this.Input_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Format String";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.DateTimeBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.FilenameBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.OutputBox);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(347, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(327, 182);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sample Output";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Sample Timestamp";
            // 
            // DateTimeBox
            // 
            this.DateTimeBox.Location = new System.Drawing.Point(9, 139);
            this.DateTimeBox.Name = "DateTimeBox";
            this.DateTimeBox.Size = new System.Drawing.Size(305, 23);
            this.DateTimeBox.TabIndex = 6;
            this.DateTimeBox.ValueChanged += new System.EventHandler(this.Input_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Sample Filename";
            // 
            // FilenameBox
            // 
            this.FilenameBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FilenameBox.Location = new System.Drawing.Point(9, 88);
            this.FilenameBox.Name = "FilenameBox";
            this.FilenameBox.Size = new System.Drawing.Size(305, 26);
            this.FilenameBox.TabIndex = 4;
            this.FilenameBox.Text = "filename.txt";
            this.FilenameBox.TextChanged += new System.EventHandler(this.Input_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Output";
            // 
            // OutputBox
            // 
            this.OutputBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OutputBox.Location = new System.Drawing.Point(9, 39);
            this.OutputBox.Name = "OutputBox";
            this.OutputBox.ReadOnly = true;
            this.OutputBox.Size = new System.Drawing.Size(305, 26);
            this.OutputBox.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.FormatStringBox);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(329, 182);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Editor";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.DateOnlyButton);
            this.groupBox3.Controls.Add(this.FileExtButton);
            this.groupBox3.Controls.Add(this.TimeOnlyButton);
            this.groupBox3.Controls.Add(this.FilenameButton);
            this.groupBox3.Controls.Add(this.DateTimeButton);
            this.groupBox3.Location = new System.Drawing.Point(9, 71);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(305, 91);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Insert";
            // 
            // DateOnlyButton
            // 
            this.DateOnlyButton.Location = new System.Drawing.Point(6, 22);
            this.DateOnlyButton.Name = "DateOnlyButton";
            this.DateOnlyButton.Size = new System.Drawing.Size(75, 29);
            this.DateOnlyButton.TabIndex = 2;
            this.DateOnlyButton.Text = "Date";
            this.DateOnlyButton.UseVisualStyleBackColor = true;
            this.DateOnlyButton.Click += new System.EventHandler(this.DateOnlyButton_Click);
            // 
            // FileExtButton
            // 
            this.FileExtButton.Location = new System.Drawing.Point(87, 56);
            this.FileExtButton.Name = "FileExtButton";
            this.FileExtButton.Size = new System.Drawing.Size(114, 29);
            this.FileExtButton.TabIndex = 6;
            this.FileExtButton.Text = "File Extension";
            this.FileExtButton.UseVisualStyleBackColor = true;
            this.FileExtButton.Click += new System.EventHandler(this.FileExtButton_Click);
            // 
            // TimeOnlyButton
            // 
            this.TimeOnlyButton.Location = new System.Drawing.Point(87, 22);
            this.TimeOnlyButton.Name = "TimeOnlyButton";
            this.TimeOnlyButton.Size = new System.Drawing.Size(75, 29);
            this.TimeOnlyButton.TabIndex = 3;
            this.TimeOnlyButton.Text = "Time";
            this.TimeOnlyButton.UseVisualStyleBackColor = true;
            this.TimeOnlyButton.Click += new System.EventHandler(this.TimeOnlyButton_Click);
            // 
            // FilenameButton
            // 
            this.FilenameButton.Location = new System.Drawing.Point(6, 57);
            this.FilenameButton.Name = "FilenameButton";
            this.FilenameButton.Size = new System.Drawing.Size(75, 29);
            this.FilenameButton.TabIndex = 5;
            this.FilenameButton.Text = "Filename";
            this.FilenameButton.UseVisualStyleBackColor = true;
            this.FilenameButton.Click += new System.EventHandler(this.FilenameButton_Click);
            // 
            // DateTimeButton
            // 
            this.DateTimeButton.Location = new System.Drawing.Point(168, 22);
            this.DateTimeButton.Name = "DateTimeButton";
            this.DateTimeButton.Size = new System.Drawing.Size(93, 29);
            this.DateTimeButton.TabIndex = 4;
            this.DateTimeButton.Text = "Date/Time";
            this.DateTimeButton.UseVisualStyleBackColor = true;
            this.DateTimeButton.Click += new System.EventHandler(this.DateTimeButton_Click);
            // 
            // OkButton
            // 
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OkButton.Location = new System.Drawing.Point(593, 200);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(81, 32);
            this.OkButton.TabIndex = 4;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelButton.Location = new System.Drawing.Point(506, 200);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(81, 32);
            this.CancelButton.TabIndex = 5;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // FormatEditor
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 244);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormatEditor";
            this.Text = "Edit Format String";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox FormatStringBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker DateTimeBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox FilenameBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox OutputBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button DateOnlyButton;
        private System.Windows.Forms.Button FileExtButton;
        private System.Windows.Forms.Button TimeOnlyButton;
        private System.Windows.Forms.Button FilenameButton;
        private System.Windows.Forms.Button DateTimeButton;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
    }
}
namespace BTCHPWUPD
{
    partial class BaseWordAndFormat
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
            this.BaseWord = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.M1 = new System.Windows.Forms.Label();
            this.M2 = new System.Windows.Forms.Label();
            this.Y2 = new System.Windows.Forms.Label();
            this.Y1 = new System.Windows.Forms.Label();
            this.B1 = new System.Windows.Forms.Label();
            this.B3 = new System.Windows.Forms.Label();
            this.B2 = new System.Windows.Forms.Label();
            this.B4 = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.P1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // BaseWord
            // 
            this.BaseWord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BaseWord.Location = new System.Drawing.Point(110, 68);
            this.BaseWord.MaxLength = 4;
            this.BaseWord.Name = "BaseWord";
            this.BaseWord.Size = new System.Drawing.Size(69, 26);
            this.BaseWord.TabIndex = 2;
            this.BaseWord.TextChanged += new System.EventHandler(this.BaseWord_TextChanged);
            this.BaseWord.Leave += new System.EventHandler(this.BaseWord_Leave);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(375, 45);
            this.label1.TabIndex = 0;
            this.label1.Text = "Base word must contain at least 1 capitial letter, and must be 4 total characters" +
    ".";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Base Word:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(375, 74);
            this.label3.TabIndex = 3;
            this.label3.Text = "Adjust the format of the password below: M1 refers to the fitst digit of the mont" +
    "h M2 is the second digit. The same pattern applys for the year.";
            this.label3.Visible = false;
            // 
            // M1
            // 
            this.M1.AutoSize = true;
            this.M1.Location = new System.Drawing.Point(43, 257);
            this.M1.Name = "M1";
            this.M1.Size = new System.Drawing.Size(31, 20);
            this.M1.TabIndex = 4;
            this.M1.Text = "M1";
            this.M1.Visible = false;
            this.M1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.M1_MouseDown);
            // 
            // M2
            // 
            this.M2.AutoSize = true;
            this.M2.Location = new System.Drawing.Point(80, 257);
            this.M2.Name = "M2";
            this.M2.Size = new System.Drawing.Size(31, 20);
            this.M2.TabIndex = 5;
            this.M2.Text = "M2";
            this.M2.Visible = false;
            this.M2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.M2_MouseDown);
            // 
            // Y2
            // 
            this.Y2.AutoSize = true;
            this.Y2.Location = new System.Drawing.Point(150, 257);
            this.Y2.Name = "Y2";
            this.Y2.Size = new System.Drawing.Size(29, 20);
            this.Y2.TabIndex = 7;
            this.Y2.Text = "Y2";
            this.Y2.Visible = false;
            this.Y2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Y2_MouseDown);
            // 
            // Y1
            // 
            this.Y1.AutoSize = true;
            this.Y1.Location = new System.Drawing.Point(117, 257);
            this.Y1.Name = "Y1";
            this.Y1.Size = new System.Drawing.Size(29, 20);
            this.Y1.TabIndex = 6;
            this.Y1.Text = "Y1";
            this.Y1.Visible = false;
            this.Y1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Y1_MouseDown);
            // 
            // B1
            // 
            this.B1.AutoSize = true;
            this.B1.Location = new System.Drawing.Point(185, 257);
            this.B1.Name = "B1";
            this.B1.Size = new System.Drawing.Size(29, 20);
            this.B1.TabIndex = 8;
            this.B1.Text = "B1";
            this.B1.Visible = false;
            this.B1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.B1_MouseDown);
            // 
            // B3
            // 
            this.B3.AutoSize = true;
            this.B3.Location = new System.Drawing.Point(255, 257);
            this.B3.Name = "B3";
            this.B3.Size = new System.Drawing.Size(29, 20);
            this.B3.TabIndex = 10;
            this.B3.Text = "B3";
            this.B3.Visible = false;
            this.B3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.B3_MouseDown);
            // 
            // B2
            // 
            this.B2.AutoSize = true;
            this.B2.Location = new System.Drawing.Point(220, 257);
            this.B2.Name = "B2";
            this.B2.Size = new System.Drawing.Size(29, 20);
            this.B2.TabIndex = 9;
            this.B2.Text = "B2";
            this.B2.Visible = false;
            this.B2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.B2_MouseDown);
            // 
            // B4
            // 
            this.B4.AutoSize = true;
            this.B4.Location = new System.Drawing.Point(290, 257);
            this.B4.Name = "B4";
            this.B4.Size = new System.Drawing.Size(29, 20);
            this.B4.TabIndex = 11;
            this.B4.Text = "B4";
            this.B4.Visible = false;
            this.B4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.B4_MouseDown);
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.Enabled = false;
            this.OK.Location = new System.Drawing.Point(321, 393);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 32);
            this.OK.TabIndex = 13;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(240, 393);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 32);
            this.Cancel.TabIndex = 14;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // P1
            // 
            this.P1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.P1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.P1.Location = new System.Drawing.Point(12, 308);
            this.P1.Name = "P1";
            this.P1.Size = new System.Drawing.Size(384, 45);
            this.P1.TabIndex = 12;
            this.P1.DragDrop += new System.Windows.Forms.DragEventHandler(this.P1_1_DragDrop);
            this.P1.DragEnter += new System.Windows.Forms.DragEventHandler(this.P1_1_DragEnter);
            this.P1.DragOver += new System.Windows.Forms.DragEventHandler(this.P1_1_DragOver);
            // 
            // BaseWordAndFormat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 446);
            this.Controls.Add(this.P1);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.B4);
            this.Controls.Add(this.B2);
            this.Controls.Add(this.B3);
            this.Controls.Add(this.B1);
            this.Controls.Add(this.Y1);
            this.Controls.Add(this.Y2);
            this.Controls.Add(this.M2);
            this.Controls.Add(this.M1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BaseWord);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(424, 484);
            this.Name = "BaseWordAndFormat";
            this.ShowIcon = false;
            this.Text = "Enter the base word and password format";
            this.Load += new System.EventHandler(this.BaseWordAndFormat_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox BaseWord;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label M1;
        private System.Windows.Forms.Label M2;
        private System.Windows.Forms.Label Y2;
        private System.Windows.Forms.Label Y1;
        private System.Windows.Forms.Label B1;
        private System.Windows.Forms.Label B3;
        private System.Windows.Forms.Label B2;
        private System.Windows.Forms.Label B4;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Panel P1;
    }
}
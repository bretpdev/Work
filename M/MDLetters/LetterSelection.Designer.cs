namespace MDLetters
{
    partial class LetterSelection
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
            this.Acct = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FuteCorr = new System.Windows.Forms.RadioButton();
            this.PrevLetters = new System.Windows.Forms.RadioButton();
            this.MdLetters = new System.Windows.Forms.RadioButton();
            this.Letters = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.Search = new System.Windows.Forms.TextBox();
            this.OK = new System.Windows.Forms.Button();
            this.Exit = new System.Windows.Forms.Button();
            this.CorrespondenceFormat = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Letters)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Account Number:";
            // 
            // Acct
            // 
            this.Acct.AutoSize = true;
            this.Acct.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Acct.Location = new System.Drawing.Point(152, 9);
            this.Acct.Name = "Acct";
            this.Acct.Size = new System.Drawing.Size(109, 20);
            this.Acct.TabIndex = 1;
            this.Acct.Text = "0123456789";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FuteCorr);
            this.groupBox1.Controls.Add(this.PrevLetters);
            this.groupBox1.Controls.Add(this.MdLetters);
            this.groupBox1.Location = new System.Drawing.Point(340, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(504, 60);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Correspondence Type";
            // 
            // FuteCorr
            // 
            this.FuteCorr.AutoSize = true;
            this.FuteCorr.Location = new System.Drawing.Point(303, 30);
            this.FuteCorr.Name = "FuteCorr";
            this.FuteCorr.Size = new System.Drawing.Size(196, 24);
            this.FuteCorr.TabIndex = 2;
            this.FuteCorr.TabStop = true;
            this.FuteCorr.Text = "Future Correspondence";
            this.FuteCorr.UseVisualStyleBackColor = true;
            this.FuteCorr.CheckedChanged += new System.EventHandler(this.FuteCorr_CheckedChanged);
            // 
            // PrevLetters
            // 
            this.PrevLetters.AutoSize = true;
            this.PrevLetters.Location = new System.Drawing.Point(118, 30);
            this.PrevLetters.Name = "PrevLetters";
            this.PrevLetters.Size = new System.Drawing.Size(179, 24);
            this.PrevLetters.TabIndex = 1;
            this.PrevLetters.TabStop = true;
            this.PrevLetters.Text = "Previous Letters Sent";
            this.PrevLetters.UseVisualStyleBackColor = true;
            this.PrevLetters.CheckedChanged += new System.EventHandler(this.PrevLetters_CheckedChanged);
            // 
            // MdLetters
            // 
            this.MdLetters.AutoSize = true;
            this.MdLetters.Location = new System.Drawing.Point(6, 30);
            this.MdLetters.Name = "MdLetters";
            this.MdLetters.Size = new System.Drawing.Size(106, 24);
            this.MdLetters.TabIndex = 0;
            this.MdLetters.TabStop = true;
            this.MdLetters.Text = "MD Letters";
            this.MdLetters.UseVisualStyleBackColor = true;
            this.MdLetters.CheckedChanged += new System.EventHandler(this.MdLetters_CheckedChanged);
            // 
            // Letters
            // 
            this.Letters.AllowUserToAddRows = false;
            this.Letters.AllowUserToDeleteRows = false;
            this.Letters.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.Letters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Letters.Location = new System.Drawing.Point(17, 121);
            this.Letters.Name = "Letters";
            this.Letters.RowHeadersVisible = false;
            this.Letters.Size = new System.Drawing.Size(827, 273);
            this.Letters.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Letter Search";
            // 
            // Search
            // 
            this.Search.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.Search.Location = new System.Drawing.Point(125, 37);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(209, 26);
            this.Search.TabIndex = 5;
            this.Search.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(769, 400);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 41);
            this.OK.TabIndex = 6;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Exit
            // 
            this.Exit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Exit.Location = new System.Drawing.Point(17, 400);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(75, 41);
            this.Exit.TabIndex = 7;
            this.Exit.Text = "Exit";
            this.Exit.UseVisualStyleBackColor = true;
            // 
            // CorrespondenceFormat
            // 
            this.CorrespondenceFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CorrespondenceFormat.FormattingEnabled = true;
            this.CorrespondenceFormat.Location = new System.Drawing.Point(553, 74);
            this.CorrespondenceFormat.Name = "CorrespondenceFormat";
            this.CorrespondenceFormat.Size = new System.Drawing.Size(286, 28);
            this.CorrespondenceFormat.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(357, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(190, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Correspondence Format: ";
            // 
            // LetterSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 453);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CorrespondenceFormat);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.Search);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Letters);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Acct);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "LetterSelection";
            this.ShowIcon = false;
            this.Text = "Letter Selection";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Letters)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Acct;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton FuteCorr;
        private System.Windows.Forms.RadioButton PrevLetters;
        private System.Windows.Forms.RadioButton MdLetters;
        private System.Windows.Forms.DataGridView Letters;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Search;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Exit;
        private System.Windows.Forms.ComboBox CorrespondenceFormat;
        private System.Windows.Forms.Label label3;
    }
}


﻿namespace LetterTrackingDataTransfer
{
    partial class TransferData
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
            this.LetterId = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.Transfered = new System.Windows.Forms.Label();
            this.Mode = new System.Windows.Forms.Label();
            this.AddEcorr = new System.Windows.Forms.CheckBox();
            this.GenerateEcorr = new System.Windows.Forms.CheckBox();
            this.AddLtdb = new System.Windows.Forms.CheckBox();
            this.GenerateLtdb = new System.Windows.Forms.CheckBox();
            this.HasLoanDetail = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // LetterId
            // 
            this.LetterId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LetterId.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.LetterId.FormattingEnabled = true;
            this.LetterId.Location = new System.Drawing.Point(13, 45);
            this.LetterId.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LetterId.Name = "LetterId";
            this.LetterId.Size = new System.Drawing.Size(272, 28);
            this.LetterId.TabIndex = 0;
            this.LetterId.SelectedIndexChanged += new System.EventHandler(this.LetterId_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(61, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Choose the letter:";
            // 
            // OK
            // 
            this.OK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.OK.Location = new System.Drawing.Point(90, 247);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(118, 32);
            this.OK.TabIndex = 2;
            this.OK.Text = "Transfer";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Transfered
            // 
            this.Transfered.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Transfered.AutoSize = true;
            this.Transfered.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Transfered.ForeColor = System.Drawing.Color.Red;
            this.Transfered.Location = new System.Drawing.Point(101, 296);
            this.Transfered.Name = "Transfered";
            this.Transfered.Size = new System.Drawing.Size(96, 20);
            this.Transfered.TabIndex = 3;
            this.Transfered.Text = "Transfered";
            this.Transfered.Visible = false;
            // 
            // Mode
            // 
            this.Mode.AutoSize = true;
            this.Mode.Location = new System.Drawing.Point(97, 78);
            this.Mode.Name = "Mode";
            this.Mode.Size = new System.Drawing.Size(81, 20);
            this.Mode.TabIndex = 4;
            this.Mode.Text = "Live Mode";
            // 
            // AddEcorr
            // 
            this.AddEcorr.AutoSize = true;
            this.AddEcorr.Location = new System.Drawing.Point(13, 104);
            this.AddEcorr.Name = "AddEcorr";
            this.AddEcorr.Size = new System.Drawing.Size(274, 24);
            this.AddEcorr.TabIndex = 5;
            this.AddEcorr.Text = "Add data to EcorrFed Letters table";
            this.AddEcorr.UseVisualStyleBackColor = true;
            // 
            // GenerateEcorr
            // 
            this.GenerateEcorr.AutoSize = true;
            this.GenerateEcorr.Location = new System.Drawing.Point(13, 128);
            this.GenerateEcorr.Name = "GenerateEcorr";
            this.GenerateEcorr.Size = new System.Drawing.Size(212, 24);
            this.GenerateEcorr.TabIndex = 6;
            this.GenerateEcorr.Text = "Generate EcorrFed Sproc";
            this.GenerateEcorr.UseVisualStyleBackColor = true;
            // 
            // AddLtdb
            // 
            this.AddLtdb.AutoSize = true;
            this.AddLtdb.Location = new System.Drawing.Point(13, 152);
            this.AddLtdb.Name = "AddLtdb";
            this.AddLtdb.Size = new System.Drawing.Size(225, 24);
            this.AddLtdb.TabIndex = 7;
            this.AddLtdb.Text = "Add data to Letter Tracking ";
            this.AddLtdb.UseVisualStyleBackColor = true;
            // 
            // GenerateLtdb
            // 
            this.GenerateLtdb.AutoSize = true;
            this.GenerateLtdb.Location = new System.Drawing.Point(13, 176);
            this.GenerateLtdb.Name = "GenerateLtdb";
            this.GenerateLtdb.Size = new System.Drawing.Size(252, 24);
            this.GenerateLtdb.TabIndex = 8;
            this.GenerateLtdb.Text = "Generate Letter Tracking Sproc";
            this.GenerateLtdb.UseVisualStyleBackColor = true;
            // 
            // HasLoanDetail
            // 
            this.HasLoanDetail.AutoSize = true;
            this.HasLoanDetail.Location = new System.Drawing.Point(13, 200);
            this.HasLoanDetail.Name = "HasLoanDetail";
            this.HasLoanDetail.Size = new System.Drawing.Size(186, 24);
            this.HasLoanDetail.TabIndex = 9;
            this.HasLoanDetail.Text = "Has a loan detail page";
            this.HasLoanDetail.UseVisualStyleBackColor = true;
            // 
            // TransferData
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 329);
            this.Controls.Add(this.HasLoanDetail);
            this.Controls.Add(this.GenerateLtdb);
            this.Controls.Add(this.AddLtdb);
            this.Controls.Add(this.GenerateEcorr);
            this.Controls.Add(this.AddEcorr);
            this.Controls.Add(this.Mode);
            this.Controls.Add(this.Transfered);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LetterId);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(316, 367);
            this.Name = "TransferData";
            this.Text = "Letter Tracking Data Transfer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox LetterId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Label Transfered;
        private System.Windows.Forms.Label Mode;
        public System.Windows.Forms.CheckBox AddEcorr;
        public System.Windows.Forms.CheckBox GenerateEcorr;
        public System.Windows.Forms.CheckBox AddLtdb;
        public System.Windows.Forms.CheckBox GenerateLtdb;
        public System.Windows.Forms.CheckBox HasLoanDetail;
    }
}

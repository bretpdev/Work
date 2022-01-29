﻿namespace ACHSETUPFD
{
	partial class ActiveAchOptionsDialog
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radStop = new System.Windows.Forms.RadioButton();
            this.radChange = new System.Windows.Forms.RadioButton();
            this.radAdd = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(56, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 46);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please choose one of the following options.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radStop);
            this.groupBox1.Controls.Add(this.radChange);
            this.groupBox1.Controls.Add(this.radAdd);
            this.groupBox1.Location = new System.Drawing.Point(20, 71);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(248, 131);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // radStop
            // 
            this.radStop.AutoSize = true;
            this.radStop.Location = new System.Drawing.Point(10, 29);
            this.radStop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radStop.Name = "radStop";
            this.radStop.Size = new System.Drawing.Size(142, 24);
            this.radStop.TabIndex = 2;
            this.radStop.TabStop = true;
            this.radStop.Text = "Stop processing";
            this.radStop.UseVisualStyleBackColor = true;
            // 
            // radChange
            // 
            this.radChange.AutoSize = true;
            this.radChange.Location = new System.Drawing.Point(8, 63);
            this.radChange.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radChange.Name = "radChange";
            this.radChange.Size = new System.Drawing.Size(216, 24);
            this.radChange.TabIndex = 1;
            this.radChange.TabStop = true;
            this.radChange.Text = "Change the existing record";
            this.radChange.UseVisualStyleBackColor = true;
            // 
            // radAdd
            // 
            this.radAdd.AutoSize = true;
            this.radAdd.Enabled = false;
            this.radAdd.Location = new System.Drawing.Point(8, 97);
            this.radAdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radAdd.Name = "radAdd";
            this.radAdd.Size = new System.Drawing.Size(188, 24);
            this.radAdd.TabIndex = 0;
            this.radAdd.Text = "Add the record anyway";
            this.radAdd.UseVisualStyleBackColor = true;
            this.radAdd.Visible = false;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(66, 215);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(150, 46);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // ActiveAchOptionsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 274);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(300, 315);
            this.MinimumSize = new System.Drawing.Size(300, 313);
            this.Name = "ActiveAchOptionsDialog";
            this.Text = "Active ACH Found Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radStop;
		private System.Windows.Forms.RadioButton radChange;
		private System.Windows.Forms.RadioButton radAdd;
		private System.Windows.Forms.Button btnOk;
	}
}
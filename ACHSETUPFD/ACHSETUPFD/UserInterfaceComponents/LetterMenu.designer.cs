namespace ACHSETUPFD
{
	partial class LetterMenu
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
            this.radNone = new System.Windows.Forms.RadioButton();
            this.radBoth = new System.Windows.Forms.RadioButton();
            this.radDenial = new System.Windows.Forms.RadioButton();
            this.radApproval = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(260, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "What letter(s) do you want to send?";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radNone);
            this.groupBox1.Controls.Add(this.radBoth);
            this.groupBox1.Controls.Add(this.radDenial);
            this.groupBox1.Controls.Add(this.radApproval);
            this.groupBox1.Location = new System.Drawing.Point(20, 52);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(300, 171);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Letter Options";
            // 
            // radNone
            // 
            this.radNone.AutoSize = true;
            this.radNone.Location = new System.Drawing.Point(10, 137);
            this.radNone.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radNone.Name = "radNone";
            this.radNone.Size = new System.Drawing.Size(65, 24);
            this.radNone.TabIndex = 3;
            this.radNone.TabStop = true;
            this.radNone.Text = "None";
            this.radNone.UseVisualStyleBackColor = true;
            // 
            // radBoth
            // 
            this.radBoth.AutoSize = true;
            this.radBoth.Location = new System.Drawing.Point(10, 102);
            this.radBoth.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radBoth.Name = "radBoth";
            this.radBoth.Size = new System.Drawing.Size(271, 24);
            this.radBoth.TabIndex = 2;
            this.radBoth.TabStop = true;
            this.radBoth.Text = "Both (Approval and Denial Letters)";
            this.radBoth.UseVisualStyleBackColor = true;
            // 
            // radDenial
            // 
            this.radDenial.AutoSize = true;
            this.radDenial.Location = new System.Drawing.Point(10, 66);
            this.radDenial.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radDenial.Name = "radDenial";
            this.radDenial.Size = new System.Drawing.Size(118, 24);
            this.radDenial.TabIndex = 1;
            this.radDenial.TabStop = true;
            this.radDenial.Text = "Denial Letter";
            this.radDenial.UseVisualStyleBackColor = true;
            // 
            // radApproval
            // 
            this.radApproval.AutoSize = true;
            this.radApproval.Location = new System.Drawing.Point(10, 31);
            this.radApproval.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radApproval.Name = "radApproval";
            this.radApproval.Size = new System.Drawing.Size(126, 24);
            this.radApproval.TabIndex = 0;
            this.radApproval.TabStop = true;
            this.radApproval.Text = "Aproval Letter";
            this.radApproval.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(93, 238);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(150, 46);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // LetterMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 298);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(355, 340);
            this.MinimumSize = new System.Drawing.Size(354, 337);
            this.Name = "LetterMenu";
            this.Text = "ACH Letter Menu";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radNone;
		private System.Windows.Forms.RadioButton radBoth;
		private System.Windows.Forms.RadioButton radDenial;
		private System.Windows.Forms.RadioButton radApproval;
		private System.Windows.Forms.Button btnOk;
	}
}
namespace ACHSETUP
{
	partial class ChangeMenu
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radAddRemove = new System.Windows.Forms.RadioButton();
            this.radModify = new System.Windows.Forms.RadioButton();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radAddRemove);
            this.groupBox1.Controls.Add(this.radModify);
            this.groupBox1.Location = new System.Drawing.Point(18, 9);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(243, 154);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // radAddRemove
            // 
            this.radAddRemove.Location = new System.Drawing.Point(10, 88);
            this.radAddRemove.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radAddRemove.Name = "radAddRemove";
            this.radAddRemove.Size = new System.Drawing.Size(228, 48);
            this.radAddRemove.TabIndex = 1;
            this.radAddRemove.TabStop = true;
            this.radAddRemove.Text = "Add Loans to ACH and/or Remove Loans from ACH";
            this.radAddRemove.UseVisualStyleBackColor = true;
            // 
            // radModify
            // 
            this.radModify.Location = new System.Drawing.Point(10, 31);
            this.radModify.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radModify.Name = "radModify";
            this.radModify.Size = new System.Drawing.Size(156, 48);
            this.radModify.TabIndex = 0;
            this.radModify.TabStop = true;
            this.radModify.Text = "Modify Existing ACH Information";
            this.radModify.UseVisualStyleBackColor = true;
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(64, 180);
            this.OK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(150, 46);
            this.OK.TabIndex = 1;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.Ok_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(64, 235);
            this.Cancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(150, 46);
            this.Cancel.TabIndex = 2;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // ChangeMenu
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(279, 295);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 341);
            this.MinimumSize = new System.Drawing.Size(295, 334);
            this.Name = "ChangeMenu";
            this.Text = "ACH Change Options";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radAddRemove;
		private System.Windows.Forms.RadioButton radModify;
		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.Button Cancel;
	}
}
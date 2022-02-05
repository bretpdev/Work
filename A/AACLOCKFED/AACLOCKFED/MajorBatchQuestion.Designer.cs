namespace AACLOCKFED
{
	partial class MajorBatchQuestion
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
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(13, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(242, 58);
			this.label1.TabIndex = 0;
			this.label1.Text = "Please note, this script will only go in and lock or unlock the minor batches for" +
				" all Major Batches that are entered on the next screen.";
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Location = new System.Drawing.Point(21, 83);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(224, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "I have the Major Batch Numbers";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(21, 125);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(224, 23);
			this.button2.TabIndex = 2;
			this.button2.Text = "I don\'t have the Major Batch Numbers yet";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// MajorBatchQuestion
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(267, 165);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Name = "MajorBatchQuestion";
			this.Text = "AAC--Lock and Unlock Minor Batches--Fed";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
	}
}
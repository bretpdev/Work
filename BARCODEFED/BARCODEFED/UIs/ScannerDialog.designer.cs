namespace BARCODEFED
{
	partial class ScannerDialog
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.btnQuit = new System.Windows.Forms.Button();
            this.txtScannerInput = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnNoBarcode = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(214, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start scanning barcodes. When you are finished, click the Process button.";
            // 
            // txtStatus
            // 
            this.txtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStatus.Location = new System.Drawing.Point(0, 119);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.Size = new System.Drawing.Size(408, 598);
            this.txtStatus.TabIndex = 4;
            // 
            // btnQuit
            // 
            this.btnQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnQuit.Location = new System.Drawing.Point(318, 47);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(80, 32);
            this.btnQuit.TabIndex = 2;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = true;
            // 
            // txtScannerInput
            // 
            this.txtScannerInput.Location = new System.Drawing.Point(16, 55);
            this.txtScannerInput.Name = "txtScannerInput";
            this.txtScannerInput.Size = new System.Drawing.Size(0, 26);
            this.txtScannerInput.TabIndex = 4;
            this.txtScannerInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtScannerInput_KeyDown);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.BackColor = System.Drawing.Color.DarkOrange;
            this.toolTip.InitialDelay = 200;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 100;
            // 
            // btnNoBarcode
            // 
            this.btnNoBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNoBarcode.Location = new System.Drawing.Point(232, 16);
            this.btnNoBarcode.Name = "btnNoBarcode";
            this.btnNoBarcode.Size = new System.Drawing.Size(80, 50);
            this.btnNoBarcode.TabIndex = 3;
            this.btnNoBarcode.Text = "No Barcode";
            this.btnNoBarcode.UseVisualStyleBackColor = true;
            this.btnNoBarcode.Click += new System.EventHandler(this.btnNoBarcode_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProcess.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnProcess.Location = new System.Drawing.Point(318, 7);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(80, 32);
            this.btnProcess.TabIndex = 1;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // ScannerDialog
            // 
            this.ClientSize = new System.Drawing.Size(408, 717);
            this.Controls.Add(this.btnNoBarcode);
            this.Controls.Add(this.txtScannerInput);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(424, 756);
            this.Name = "ScannerDialog";
            this.Text = "Return Mail FED Scanner";
            this.Shown += new System.EventHandler(this.ScannerDialog_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtStatus;
		private System.Windows.Forms.Button btnQuit;
		private System.Windows.Forms.TextBox txtScannerInput;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Button btnNoBarcode;
        private System.Windows.Forms.Button btnProcess;
    }
}

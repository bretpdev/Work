namespace ACURINTFED
{
	partial class MainForm
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblContinueFrom = new System.Windows.Forms.Label();
            this.btnContinue = new System.Windows.Forms.Button();
            this.btnProcessResponse = new System.Windows.Forms.Button();
            this.btnGetResponse = new System.Windows.Forms.Button();
            this.btnSendRequest = new System.Windows.Forms.Button();
            this.btnCreateRequest = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.lblContinueFrom);
            this.groupBox2.Controls.Add(this.btnContinue);
            this.groupBox2.Controls.Add(this.btnProcessResponse);
            this.groupBox2.Controls.Add(this.btnGetResponse);
            this.groupBox2.Controls.Add(this.btnSendRequest);
            this.groupBox2.Controls.Add(this.btnCreateRequest);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(141, 227);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Select Process to Run";
            // 
            // lblContinueFrom
            // 
            this.lblContinueFrom.AutoSize = true;
            this.lblContinueFrom.Location = new System.Drawing.Point(7, 56);
            this.lblContinueFrom.Name = "lblContinueFrom";
            this.lblContinueFrom.Size = new System.Drawing.Size(54, 13);
            this.lblContinueFrom.TabIndex = 12;
            this.lblContinueFrom.Text = "Beginning";
            // 
            // btnContinue
            // 
            this.btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnContinue.Location = new System.Drawing.Point(8, 19);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(125, 30);
            this.btnContinue.TabIndex = 11;
            this.btnContinue.Text = "Continue  From:";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // btnProcessResponse
            // 
            this.btnProcessResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProcessResponse.Location = new System.Drawing.Point(8, 189);
            this.btnProcessResponse.Name = "btnProcessResponse";
            this.btnProcessResponse.Size = new System.Drawing.Size(125, 30);
            this.btnProcessResponse.TabIndex = 10;
            this.btnProcessResponse.Text = "Process Response File";
            this.btnProcessResponse.UseVisualStyleBackColor = true;
            this.btnProcessResponse.Click += new System.EventHandler(this.btnProcessResponse_Click);
            // 
            // btnGetResponse
            // 
            this.btnGetResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetResponse.Location = new System.Drawing.Point(8, 153);
            this.btnGetResponse.Name = "btnGetResponse";
            this.btnGetResponse.Size = new System.Drawing.Size(125, 30);
            this.btnGetResponse.TabIndex = 9;
            this.btnGetResponse.Text = "Get Response File";
            this.btnGetResponse.UseVisualStyleBackColor = true;
            this.btnGetResponse.Click += new System.EventHandler(this.btnGetResponse_Click);
            // 
            // btnSendRequest
            // 
            this.btnSendRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendRequest.Location = new System.Drawing.Point(8, 117);
            this.btnSendRequest.Name = "btnSendRequest";
            this.btnSendRequest.Size = new System.Drawing.Size(125, 30);
            this.btnSendRequest.TabIndex = 8;
            this.btnSendRequest.Text = "Send Request File";
            this.btnSendRequest.UseVisualStyleBackColor = true;
            this.btnSendRequest.Click += new System.EventHandler(this.btnSendRequest_Click);
            // 
            // btnCreateRequest
            // 
            this.btnCreateRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateRequest.Location = new System.Drawing.Point(8, 81);
            this.btnCreateRequest.Name = "btnCreateRequest";
            this.btnCreateRequest.Size = new System.Drawing.Size(125, 30);
            this.btnCreateRequest.TabIndex = 7;
            this.btnCreateRequest.Text = "Create Request File";
            this.btnCreateRequest.UseVisualStyleBackColor = true;
            this.btnCreateRequest.Click += new System.EventHandler(this.btnCreateRequest_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(20, 245);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(125, 30);
            this.btnExit.TabIndex = 12;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(164, 279);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Accurint Recovery";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button btnProcessResponse;
		private System.Windows.Forms.Button btnGetResponse;
		private System.Windows.Forms.Button btnSendRequest;
		private System.Windows.Forms.Button btnCreateRequest;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Button btnContinue;
		private System.Windows.Forms.Label lblContinueFrom;
	}
}
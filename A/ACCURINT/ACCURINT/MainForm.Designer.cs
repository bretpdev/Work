namespace ACCURINT
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
            this.components = new System.ComponentModel.Container();
            this.credentialsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSendInput = new System.Windows.Forms.Button();
            this.btnProcessResponse = new System.Windows.Forms.Button();
            this.btnGetResponse = new System.Windows.Forms.Button();
            this.btnSendRequest = new System.Windows.Forms.Button();
            this.btnCreateRequest = new System.Windows.Forms.Button();
            this.btnRunAll = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.FirstProcCheck = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.credentialsBindingSource)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSendInput);
            this.groupBox2.Controls.Add(this.btnProcessResponse);
            this.groupBox2.Controls.Add(this.btnGetResponse);
            this.groupBox2.Controls.Add(this.btnSendRequest);
            this.groupBox2.Controls.Add(this.btnCreateRequest);
            this.groupBox2.Controls.Add(this.btnRunAll);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(15, 46);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(306, 228);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Select Process to Run";
            // 
            // btnSendInput
            // 
            this.btnSendInput.Location = new System.Drawing.Point(196, 28);
            this.btnSendInput.Name = "btnSendInput";
            this.btnSendInput.Size = new System.Drawing.Size(91, 183);
            this.btnSendInput.TabIndex = 11;
            this.btnSendInput.Text = "Send Input File";
            this.btnSendInput.UseVisualStyleBackColor = true;
            this.btnSendInput.Click += new System.EventHandler(this.btnSendInput_Click);
            // 
            // btnProcessResponse
            // 
            this.btnProcessResponse.Location = new System.Drawing.Point(56, 181);
            this.btnProcessResponse.Name = "btnProcessResponse";
            this.btnProcessResponse.Size = new System.Drawing.Size(125, 30);
            this.btnProcessResponse.TabIndex = 10;
            this.btnProcessResponse.Text = "Process Response File";
            this.btnProcessResponse.UseVisualStyleBackColor = true;
            this.btnProcessResponse.Click += new System.EventHandler(this.btnProcessResponse_Click);
            // 
            // btnGetResponse
            // 
            this.btnGetResponse.Location = new System.Drawing.Point(56, 145);
            this.btnGetResponse.Name = "btnGetResponse";
            this.btnGetResponse.Size = new System.Drawing.Size(125, 30);
            this.btnGetResponse.TabIndex = 9;
            this.btnGetResponse.Text = "Get Response File";
            this.btnGetResponse.UseVisualStyleBackColor = true;
            this.btnGetResponse.Click += new System.EventHandler(this.btnGetResponse_Click);
            // 
            // btnSendRequest
            // 
            this.btnSendRequest.Location = new System.Drawing.Point(56, 109);
            this.btnSendRequest.Name = "btnSendRequest";
            this.btnSendRequest.Size = new System.Drawing.Size(125, 30);
            this.btnSendRequest.TabIndex = 8;
            this.btnSendRequest.Text = "Send Request File";
            this.btnSendRequest.UseVisualStyleBackColor = true;
            this.btnSendRequest.Click += new System.EventHandler(this.btnSendRequest_Click);
            // 
            // btnCreateRequest
            // 
            this.btnCreateRequest.Location = new System.Drawing.Point(56, 73);
            this.btnCreateRequest.Name = "btnCreateRequest";
            this.btnCreateRequest.Size = new System.Drawing.Size(125, 30);
            this.btnCreateRequest.TabIndex = 7;
            this.btnCreateRequest.Text = "Create Request File";
            this.btnCreateRequest.UseVisualStyleBackColor = true;
            this.btnCreateRequest.Click += new System.EventHandler(this.btnCreateRequest_Click);
            // 
            // btnRunAll
            // 
            this.btnRunAll.Location = new System.Drawing.Point(56, 28);
            this.btnRunAll.Name = "btnRunAll";
            this.btnRunAll.Size = new System.Drawing.Size(125, 30);
            this.btnRunAll.TabIndex = 6;
            this.btnRunAll.Text = "Run All";
            this.btnRunAll.UseVisualStyleBackColor = true;
            this.btnRunAll.Click += new System.EventHandler(this.btnRunAll_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(13, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "3";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 190);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(13, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "4";
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(71, 289);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(125, 30);
            this.btnExit.TabIndex = 12;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // FirstProcCheck
            // 
            this.FirstProcCheck.AutoSize = true;
            this.FirstProcCheck.Location = new System.Drawing.Point(12, 12);
            this.FirstProcCheck.Name = "FirstProcCheck";
            this.FirstProcCheck.Size = new System.Drawing.Size(157, 17);
            this.FirstProcCheck.TabIndex = 13;
            this.FirstProcCheck.Text = "Only Process First Selection";
            this.FirstProcCheck.UseVisualStyleBackColor = true;
            this.FirstProcCheck.Visible = false;
            this.FirstProcCheck.CheckedChanged += new System.EventHandler(this.FirstProcCheck_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 327);
            this.Controls.Add(this.FirstProcCheck);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Accurint";
            ((System.ComponentModel.ISupportInitialize)(this.credentialsBindingSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button btnRunAll;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button btnSendInput;
		private System.Windows.Forms.Button btnProcessResponse;
		private System.Windows.Forms.Button btnGetResponse;
		private System.Windows.Forms.Button btnSendRequest;
		private System.Windows.Forms.Button btnCreateRequest;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.BindingSource credentialsBindingSource;
        private System.Windows.Forms.CheckBox FirstProcCheck;
	}
}
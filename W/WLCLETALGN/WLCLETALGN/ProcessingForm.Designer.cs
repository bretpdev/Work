namespace WLCLETALGN
{
    partial class ProcessingForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.ArcProgress = new System.Windows.Forms.ProgressBar();
            this.Open = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.Process = new System.Windows.Forms.Button();
            this.UserIdDisplay = new System.Windows.Forms.Label();
            this.Counter = new System.Windows.Forms.Label();
            this.Status = new System.Windows.Forms.Label();
            this.StatusTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(204, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 20);
            this.label1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(129, 16);
            this.label2.MaximumSize = new System.Drawing.Size(400, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(384, 40);
            this.label2.TabIndex = 1;
            this.label2.Text = "This application creates Align welcome letters. Please choose a file to process.";
            // 
            // ArcProgress
            // 
            this.ArcProgress.Location = new System.Drawing.Point(24, 66);
            this.ArcProgress.Name = "ArcProgress";
            this.ArcProgress.Size = new System.Drawing.Size(593, 36);
            this.ArcProgress.TabIndex = 2;
            // 
            // Open
            // 
            this.Open.Location = new System.Drawing.Point(381, 139);
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(109, 39);
            this.Open.TabIndex = 3;
            this.Open.Text = "Open";
            this.Open.UseVisualStyleBackColor = true;
            this.Open.Click += new System.EventHandler(this.Open_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(24, 139);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(109, 39);
            this.Cancel.TabIndex = 4;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Process
            // 
            this.Process.Enabled = false;
            this.Process.Location = new System.Drawing.Point(508, 139);
            this.Process.Name = "Process";
            this.Process.Size = new System.Drawing.Size(109, 39);
            this.Process.TabIndex = 5;
            this.Process.Text = "Process";
            this.Process.UseVisualStyleBackColor = true;
            this.Process.Click += new System.EventHandler(this.Process_Click);
            // 
            // UserIdDisplay
            // 
            this.UserIdDisplay.AutoSize = true;
            this.UserIdDisplay.Location = new System.Drawing.Point(196, 148);
            this.UserIdDisplay.Name = "UserIdDisplay";
            this.UserIdDisplay.Size = new System.Drawing.Size(0, 20);
            this.UserIdDisplay.TabIndex = 6;
            // 
            // Counter
            // 
            this.Counter.AutoSize = true;
            this.Counter.Location = new System.Drawing.Point(377, 105);
            this.Counter.Name = "Counter";
            this.Counter.Size = new System.Drawing.Size(0, 20);
            this.Counter.TabIndex = 7;
            // 
            // Status
            // 
            this.Status.AutoSize = true;
            this.Status.ForeColor = System.Drawing.Color.Black;
            this.Status.Location = new System.Drawing.Point(40, 105);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(0, 20);
            this.Status.TabIndex = 8;
            // 
            // StatusTimer
            // 
            this.StatusTimer.Tick += new System.EventHandler(this.StatusTimer_Tick);
            // 
            // ProcessingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 199);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.Counter);
            this.Controls.Add(this.UserIdDisplay);
            this.Controls.Add(this.Process);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Open);
            this.Controls.Add(this.ArcProgress);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximumSize = new System.Drawing.Size(651, 241);
            this.MinimumSize = new System.Drawing.Size(651, 241);
            this.Name = "ProcessingForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Process File";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProcessingForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar ArcProgress;
        private System.Windows.Forms.Button Open;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Process;
        private System.Windows.Forms.Label UserIdDisplay;
        private System.Windows.Forms.Label Counter;
        private System.Windows.Forms.Label Status;
        private System.Windows.Forms.Timer StatusTimer;
    }
}
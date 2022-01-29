namespace CentralizedPrintingProcess
{
    partial class StatusForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.FaxingStatus = new CentralizedPrintingProcess.JobStatus();
            this.EmailHandlerControl = new CentralizedPrintingProcess.EmailHandler();
            this.PrintingStatus = new CentralizedPrintingProcess.JobStatus();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.19268F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.076426F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.62325F));
            this.tableLayoutPanel1.Controls.Add(this.FaxingStatus, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.EmailHandlerControl, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.PrintingStatus, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(933, 506);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // FaxingStatus
            // 
            this.FaxingStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FaxingStatus.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FaxingStatus.Location = new System.Drawing.Point(472, 3);
            this.FaxingStatus.Name = "FaxingStatus";
            this.FaxingStatus.Size = new System.Drawing.Size(458, 247);
            this.FaxingStatus.TabIndex = 8;
            this.FaxingStatus.TitleColor = System.Drawing.SystemColors.Control;
            this.FaxingStatus.TitleText = "Faxing";
            // 
            // EmailHandlerControl
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.EmailHandlerControl, 3);
            this.EmailHandlerControl.DataAccess = null;
            this.EmailHandlerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EmailHandlerControl.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EmailHandlerControl.Location = new System.Drawing.Point(3, 256);
            this.EmailHandlerControl.Name = "EmailHandlerControl";
            this.EmailHandlerControl.Size = new System.Drawing.Size(927, 247);
            this.EmailHandlerControl.TabIndex = 5;
            // 
            // PrintingStatus
            // 
            this.PrintingStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PrintingStatus.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PrintingStatus.Location = new System.Drawing.Point(3, 3);
            this.PrintingStatus.Name = "PrintingStatus";
            this.PrintingStatus.Size = new System.Drawing.Size(453, 247);
            this.PrintingStatus.TabIndex = 1;
            this.PrintingStatus.TitleColor = System.Drawing.SystemColors.Control;
            this.PrintingStatus.TitleText = "Printing";
            // 
            // StatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 506);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(945, 545);
            this.Name = "StatusForm";
            this.Text = "Centralized Printing - Status";
            this.Shown += new System.EventHandler(this.StatusForm_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private JobStatus PrintingStatus;
        private JobStatus FaxingStatus;
        private EmailHandler EmailHandlerControl;
    }
}
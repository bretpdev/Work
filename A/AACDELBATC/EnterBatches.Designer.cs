namespace AACDELBATC
{
    partial class EnterBatches
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
            this.lblbatchesInfo = new System.Windows.Forms.Label();
            this.txtMajorBatch = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lstBatches = new System.Windows.Forms.ListBox();
            this.btnContinue = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblbatchesInfo
            // 
            this.lblbatchesInfo.Location = new System.Drawing.Point(12, 16);
            this.lblbatchesInfo.Name = "lblbatchesInfo";
            this.lblbatchesInfo.Size = new System.Drawing.Size(260, 33);
            this.lblbatchesInfo.TabIndex = 0;
            this.lblbatchesInfo.Text = "Enter major batches that contain minor batches that need to be deleted.";
            // 
            // txtMajorBatch
            // 
            this.txtMajorBatch.Location = new System.Drawing.Point(15, 52);
            this.txtMajorBatch.MaxLength = 10;
            this.txtMajorBatch.Name = "txtMajorBatch";
            this.txtMajorBatch.Size = new System.Drawing.Size(182, 20);
            this.txtMajorBatch.TabIndex = 1;
            this.txtMajorBatch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMajorBatch_KeyPress);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(215, 52);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(57, 20);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lstBatches
            // 
            this.lstBatches.FormattingEnabled = true;
            this.lstBatches.Location = new System.Drawing.Point(15, 90);
            this.lstBatches.Name = "lstBatches";
            this.lstBatches.Size = new System.Drawing.Size(257, 199);
            this.lstBatches.TabIndex = 3;
            this.lstBatches.DoubleClick += new System.EventHandler(this.lstBatches_DoubleClick);
            // 
            // btnContinue
            // 
            this.btnContinue.Enabled = false;
            this.btnContinue.Location = new System.Drawing.Point(107, 310);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(75, 23);
            this.btnContinue.TabIndex = 4;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(197, 310);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // EnterBatches
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 345);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.lstBatches);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtMajorBatch);
            this.Controls.Add(this.lblbatchesInfo);
            this.Name = "EnterBatches";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblbatchesInfo;
        private System.Windows.Forms.TextBox txtMajorBatch;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListBox lstBatches;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Button btnCancel;
    }
}
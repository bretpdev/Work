namespace AACLOCKUNL
{
    partial class BatchEntry
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
            this.button2 = new System.Windows.Forms.Button();
            this.btnContinue = new System.Windows.Forms.Button();
            this.radUnlock = new System.Windows.Forms.RadioButton();
            this.radLock = new System.Windows.Forms.RadioButton();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBatches = new System.Windows.Forms.TextBox();
            this.batchesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.txtNewBatch = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.batchesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(152, 293);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnContinue
            // 
            this.btnContinue.Enabled = false;
            this.btnContinue.Location = new System.Drawing.Point(34, 293);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(75, 23);
            this.btnContinue.TabIndex = 14;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // radUnlock
            // 
            this.radUnlock.AutoSize = true;
            this.radUnlock.Location = new System.Drawing.Point(152, 262);
            this.radUnlock.Name = "radUnlock";
            this.radUnlock.Size = new System.Drawing.Size(59, 17);
            this.radUnlock.TabIndex = 13;
            this.radUnlock.TabStop = true;
            this.radUnlock.Text = "Unlock";
            this.radUnlock.UseVisualStyleBackColor = true;
            this.radUnlock.CheckedChanged += new System.EventHandler(this.radUnlock_CheckedChanged);
            // 
            // radLock
            // 
            this.radLock.AutoSize = true;
            this.radLock.Location = new System.Drawing.Point(60, 262);
            this.radLock.Name = "radLock";
            this.radLock.Size = new System.Drawing.Size(49, 17);
            this.radLock.TabIndex = 12;
            this.radLock.TabStop = true;
            this.radLock.Text = "Lock";
            this.radLock.UseVisualStyleBackColor = true;
            this.radLock.CheckedChanged += new System.EventHandler(this.radLock_CheckedChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(186, 46);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 34);
            this.label1.TabIndex = 10;
            this.label1.Text = "Enter major batches that contain minor batches that need to be locked.";
            // 
            // txtBatches
            // 
            this.txtBatches.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.batchesBindingSource, "NewlineSeparatedValues", true));
            this.txtBatches.Location = new System.Drawing.Point(11, 75);
            this.txtBatches.Multiline = true;
            this.txtBatches.Name = "txtBatches";
            this.txtBatches.Size = new System.Drawing.Size(250, 171);
            this.txtBatches.TabIndex = 9;
            // 
            // batchesBindingSource
            // 
            this.batchesBindingSource.DataSource = typeof(AACLOCKUNL.Batches);
            // 
            // txtNewBatch
            // 
            this.txtNewBatch.Location = new System.Drawing.Point(11, 48);
            this.txtNewBatch.MaxLength = 10;
            this.txtNewBatch.Name = "txtNewBatch";
            this.txtNewBatch.Size = new System.Drawing.Size(169, 20);
            this.txtNewBatch.TabIndex = 8;
            // 
            // BatchEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 333);
            this.ControlBox = false;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.radUnlock);
            this.Controls.Add(this.radLock);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBatches);
            this.Controls.Add(this.txtNewBatch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "BatchEntry";
            this.Text = "BatchEntry";
            ((System.ComponentModel.ISupportInitialize)(this.batchesBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.RadioButton radUnlock;
        private System.Windows.Forms.RadioButton radLock;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBatches;
        private System.Windows.Forms.TextBox txtNewBatch;
        private System.Windows.Forms.BindingSource batchesBindingSource;

    }
}
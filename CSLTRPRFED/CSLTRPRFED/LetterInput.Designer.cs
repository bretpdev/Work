namespace CSLTRPRFED
{
    partial class LetterInput
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
            this.lblLetterId = new System.Windows.Forms.Label();
            this.txtLetterId = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lstLetterIds = new System.Windows.Forms.ListBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblLetterId
            // 
            this.lblLetterId.AutoSize = true;
            this.lblLetterId.Location = new System.Drawing.Point(12, 24);
            this.lblLetterId.Name = "lblLetterId";
            this.lblLetterId.Size = new System.Drawing.Size(51, 13);
            this.lblLetterId.TabIndex = 0;
            this.lblLetterId.Text = "Letter ID:";
            // 
            // txtLetterId
            // 
            this.txtLetterId.Location = new System.Drawing.Point(69, 21);
            this.txtLetterId.MaxLength = 10;
            this.txtLetterId.Name = "txtLetterId";
            this.txtLetterId.Size = new System.Drawing.Size(100, 20);
            this.txtLetterId.TabIndex = 1;
            this.txtLetterId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLetterId_KeyDown);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(188, 19);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lstLetterIds
            // 
            this.lstLetterIds.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.lstLetterIds.FormattingEnabled = true;
            this.lstLetterIds.Location = new System.Drawing.Point(12, 90);
            this.lstLetterIds.Name = "lstLetterIds";
            this.lstLetterIds.Size = new System.Drawing.Size(260, 160);
            this.lstLetterIds.TabIndex = 3;
            this.lstLetterIds.DoubleClick += new System.EventHandler(this.lstLetterIds_DoubleClick);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(12, 74);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(142, 13);
            this.lblInfo.TabIndex = 4;
            this.lblInfo.Text = "Letter Id\'s that will be added.";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(197, 263);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Visible = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(107, 263);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // LetterInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 298);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lstLetterIds);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtLetterId);
            this.Controls.Add(this.lblLetterId);
            this.Name = "LetterInput";
            this.Text = "LetterInput";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLetterId;
        private System.Windows.Forms.TextBox txtLetterId;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListBox lstLetterIds;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}
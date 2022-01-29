namespace FUTRQUEFED
{
    partial class QueueInfoEntry
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtAccountNumber = new System.Windows.Forms.TextBox();
            this.queueInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblAccountNumber = new System.Windows.Forms.Label();
            this.lblRecipientId = new System.Windows.Forms.Label();
            this.txtRecipientId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtArc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.dtpArcAddDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.queueInfoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(224, 256);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(80, 256);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtAccountNumber
            // 
            this.txtAccountNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queueInfoBindingSource, "AccountNumber", true));
            this.txtAccountNumber.Location = new System.Drawing.Point(160, 80);
            this.txtAccountNumber.MaxLength = 10;
            this.txtAccountNumber.Name = "txtAccountNumber";
            this.txtAccountNumber.Size = new System.Drawing.Size(100, 20);
            this.txtAccountNumber.TabIndex = 1;
            // 
            // queueInfoBindingSource
            // 
            this.queueInfoBindingSource.DataSource = typeof(FUTRQUEFED.QueueInfo);
            // 
            // lblAccountNumber
            // 
            this.lblAccountNumber.Location = new System.Drawing.Point(16, 80);
            this.lblAccountNumber.Name = "lblAccountNumber";
            this.lblAccountNumber.Size = new System.Drawing.Size(132, 20);
            this.lblAccountNumber.TabIndex = 3;
            this.lblAccountNumber.Text = "Borrower Account Number";
            this.lblAccountNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRecipientId
            // 
            this.lblRecipientId.Location = new System.Drawing.Point(16, 104);
            this.lblRecipientId.Name = "lblRecipientId";
            this.lblRecipientId.Size = new System.Drawing.Size(132, 20);
            this.lblRecipientId.TabIndex = 5;
            this.lblRecipientId.Text = "Recipient ID";
            this.lblRecipientId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRecipientId
            // 
            this.txtRecipientId.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queueInfoBindingSource, "RecipientId", true));
            this.txtRecipientId.Location = new System.Drawing.Point(160, 104);
            this.txtRecipientId.MaxLength = 10;
            this.txtRecipientId.Name = "txtRecipientId";
            this.txtRecipientId.Size = new System.Drawing.Size(100, 20);
            this.txtRecipientId.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "ARC";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtArc
            // 
            this.txtArc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queueInfoBindingSource, "Arc", true));
            this.txtArc.Location = new System.Drawing.Point(160, 128);
            this.txtArc.MaxLength = 5;
            this.txtArc.Name = "txtArc";
            this.txtArc.Size = new System.Drawing.Size(100, 20);
            this.txtArc.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "ARC Add Date";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 176);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "Comment";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtComment
            // 
            this.txtComment.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queueInfoBindingSource, "Comment", true));
            this.txtComment.Location = new System.Drawing.Point(160, 176);
            this.txtComment.MaxLength = 300;
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(200, 60);
            this.txtComment.TabIndex = 5;
            // 
            // dtpArcAddDate
            // 
            this.dtpArcAddDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.queueInfoBindingSource, "ArcAddDate", true));
            this.dtpArcAddDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpArcAddDate.Location = new System.Drawing.Point(160, 152);
            this.dtpArcAddDate.Name = "dtpArcAddDate";
            this.dtpArcAddDate.Size = new System.Drawing.Size(100, 20);
            this.dtpArcAddDate.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(16, 16);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 7, 5, 5);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(18, 5, 5, 5);
            this.label2.Size = new System.Drawing.Size(344, 40);
            this.label2.TabIndex = 13;
            this.label2.Text = "Enter the information below and click OK to add a future dated queue task.";
            // 
            // QueueInfoEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 295);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtpArcAddDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtArc);
            this.Controls.Add(this.lblRecipientId);
            this.Controls.Add(this.txtRecipientId);
            this.Controls.Add(this.lblAccountNumber);
            this.Controls.Add(this.txtAccountNumber);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "QueueInfoEntry";
            this.Text = "Create Future Dated Queue FED";
            ((System.ComponentModel.ISupportInitialize)(this.queueInfoBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtAccountNumber;
        private System.Windows.Forms.Label lblAccountNumber;
        private System.Windows.Forms.Label lblRecipientId;
        private System.Windows.Forms.TextBox txtRecipientId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtArc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.DateTimePicker dtpArcAddDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.BindingSource queueInfoBindingSource;
    }
}
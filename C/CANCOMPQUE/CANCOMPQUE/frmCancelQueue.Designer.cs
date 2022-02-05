namespace CANCOMPQUE
{
    partial class frmCancelQueue
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoBoth = new System.Windows.Forms.RadioButton();
            this.rdoQueueOnly = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtErrorMessage = new System.Windows.Forms.TextBox();
            this.cancelDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.txtQueue = new System.Windows.Forms.TextBox();
            this.txtSubQueue = new System.Windows.Forms.TextBox();
            this.gbxError = new System.Windows.Forms.GroupBox();
            this.txtDateThrough = new System.Windows.Forms.DateTimePicker();
            this.txtDateFrom = new System.Windows.Forms.DateTimePicker();
            this.txtArc = new System.Windows.Forms.TextBox();
            this.lblDateThrough = new System.Windows.Forms.Label();
            this.lblArc = new System.Windows.Forms.Label();
            this.lblDateFrom = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cancelDataBindingSource)).BeginInit();
            this.gbxError.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoBoth);
            this.groupBox1.Controls.Add(this.rdoQueueOnly);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(445, 51);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "What Do You Want To Do?";
            // 
            // rdoBoth
            // 
            this.rdoBoth.AutoSize = true;
            this.rdoBoth.Location = new System.Drawing.Point(182, 20);
            this.rdoBoth.Name = "rdoBoth";
            this.rdoBoth.Size = new System.Drawing.Size(255, 17);
            this.rdoBoth.TabIndex = 1;
            this.rdoBoth.Text = "Cancel Queue Task and Error Activity Comments";
            this.rdoBoth.UseVisualStyleBackColor = true;
            this.rdoBoth.CheckedChanged += new System.EventHandler(this.rdoBoth_CheckedChanged);
            // 
            // rdoQueueOnly
            // 
            this.rdoQueueOnly.AutoSize = true;
            this.rdoQueueOnly.Checked = true;
            this.rdoQueueOnly.Location = new System.Drawing.Point(14, 20);
            this.rdoQueueOnly.Name = "rdoQueueOnly";
            this.rdoQueueOnly.Size = new System.Drawing.Size(144, 17);
            this.rdoQueueOnly.TabIndex = 0;
            this.rdoQueueOnly.TabStop = true;
            this.rdoQueueOnly.Text = "Cancel Queue Task Only";
            this.rdoQueueOnly.UseVisualStyleBackColor = true;
            this.rdoQueueOnly.CheckedChanged += new System.EventHandler(this.rdoQueueOnly_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtErrorMessage);
            this.groupBox2.Controls.Add(this.txtQueue);
            this.groupBox2.Controls.Add(this.txtSubQueue);
            this.groupBox2.Location = new System.Drawing.Point(12, 69);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(445, 129);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Queue Task Canceling";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(114, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Sub Queue:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 68);
            this.label6.MaximumSize = new System.Drawing.Size(55, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 26);
            this.label6.TabIndex = 4;
            this.label6.Text = "Error Message:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Queue:";
            // 
            // txtErrorMessage
            // 
            this.txtErrorMessage.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.cancelDataBindingSource, "Message", true));
            this.txtErrorMessage.Location = new System.Drawing.Point(76, 45);
            this.txtErrorMessage.Multiline = true;
            this.txtErrorMessage.Name = "txtErrorMessage";
            this.txtErrorMessage.Size = new System.Drawing.Size(361, 78);
            this.txtErrorMessage.TabIndex = 2;
            // 
            // cancelDataBindingSource
            // 
            this.cancelDataBindingSource.DataSource = typeof(CANCOMPQUE.CancelData);
            // 
            // txtQueue
            // 
            this.txtQueue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.cancelDataBindingSource, "Queue", true));
            this.txtQueue.Location = new System.Drawing.Point(76, 19);
            this.txtQueue.Name = "txtQueue";
            this.txtQueue.Size = new System.Drawing.Size(32, 20);
            this.txtQueue.TabIndex = 0;
            this.txtQueue.TextChanged += new System.EventHandler(this.txtQueue_TextChanged);
            // 
            // txtSubQueue
            // 
            this.txtSubQueue.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.cancelDataBindingSource, "SubQueue", true));
            this.txtSubQueue.Location = new System.Drawing.Point(181, 19);
            this.txtSubQueue.Name = "txtSubQueue";
            this.txtSubQueue.Size = new System.Drawing.Size(32, 20);
            this.txtSubQueue.TabIndex = 1;
            this.txtSubQueue.TextChanged += new System.EventHandler(this.txtSubQueue_TextChanged);
            // 
            // gbxError
            // 
            this.gbxError.Controls.Add(this.txtDateThrough);
            this.gbxError.Controls.Add(this.txtDateFrom);
            this.gbxError.Controls.Add(this.txtArc);
            this.gbxError.Controls.Add(this.lblDateThrough);
            this.gbxError.Controls.Add(this.lblArc);
            this.gbxError.Controls.Add(this.lblDateFrom);
            this.gbxError.Enabled = false;
            this.gbxError.Location = new System.Drawing.Point(12, 204);
            this.gbxError.Name = "gbxError";
            this.gbxError.Size = new System.Drawing.Size(445, 80);
            this.gbxError.TabIndex = 1;
            this.gbxError.TabStop = false;
            this.gbxError.Text = "Error Activity Comments";
            // 
            // txtDateThrough
            // 
            this.txtDateThrough.CustomFormat = "";
            this.txtDateThrough.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtDateThrough.Location = new System.Drawing.Point(250, 41);
            this.txtDateThrough.Name = "txtDateThrough";
            this.txtDateThrough.Size = new System.Drawing.Size(100, 20);
            this.txtDateThrough.TabIndex = 2;
            this.txtDateThrough.Value = new System.DateTime(2011, 5, 17, 0, 0, 0, 0);
            // 
            // txtDateFrom
            // 
            this.txtDateFrom.CustomFormat = "";
            this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtDateFrom.Location = new System.Drawing.Point(76, 41);
            this.txtDateFrom.Name = "txtDateFrom";
            this.txtDateFrom.Size = new System.Drawing.Size(100, 20);
            this.txtDateFrom.TabIndex = 1;
            this.txtDateFrom.Value = new System.DateTime(2011, 5, 17, 0, 0, 0, 0);
            // 
            // txtArc
            // 
            this.txtArc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.cancelDataBindingSource, "ARC", true));
            this.txtArc.Location = new System.Drawing.Point(76, 20);
            this.txtArc.Name = "txtArc";
            this.txtArc.Size = new System.Drawing.Size(100, 20);
            this.txtArc.TabIndex = 0;
            // 
            // lblDateThrough
            // 
            this.lblDateThrough.AutoSize = true;
            this.lblDateThrough.Location = new System.Drawing.Point(191, 44);
            this.lblDateThrough.Name = "lblDateThrough";
            this.lblDateThrough.Size = new System.Drawing.Size(43, 13);
            this.lblDateThrough.TabIndex = 3;
            this.lblDateThrough.Text = "through";
            // 
            // lblArc
            // 
            this.lblArc.AutoSize = true;
            this.lblArc.Location = new System.Drawing.Point(38, 23);
            this.lblArc.Name = "lblArc";
            this.lblArc.Size = new System.Drawing.Size(32, 13);
            this.lblArc.TabIndex = 1;
            this.lblArc.Text = "ARC:";
            // 
            // lblDateFrom
            // 
            this.lblDateFrom.AutoSize = true;
            this.lblDateFrom.Location = new System.Drawing.Point(11, 43);
            this.lblDateFrom.Name = "lblDateFrom";
            this.lblDateFrom.Size = new System.Drawing.Size(59, 13);
            this.lblDateFrom.TabIndex = 0;
            this.lblDateFrom.Text = "Date From:";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(129, 295);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(235, 295);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // frmCancelQueue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 330);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gbxError);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmCancelQueue";
            this.Text = "Cancel Queue Tasks/Error Out Activity Comments";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cancelDataBindingSource)).EndInit();
            this.gbxError.ResumeLayout(false);
            this.gbxError.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox gbxError;
        private System.Windows.Forms.TextBox txtArc;
        private System.Windows.Forms.Label lblDateThrough;
        private System.Windows.Forms.Label lblArc;
        private System.Windows.Forms.Label lblDateFrom;
        private System.Windows.Forms.RadioButton rdoBoth;
        private System.Windows.Forms.RadioButton rdoQueueOnly;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtErrorMessage;
        private System.Windows.Forms.TextBox txtQueue;
        private System.Windows.Forms.TextBox txtSubQueue;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DateTimePicker txtDateFrom;
        private System.Windows.Forms.DateTimePicker txtDateThrough;
        private System.Windows.Forms.BindingSource cancelDataBindingSource;
    }
}
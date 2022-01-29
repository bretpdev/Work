namespace PUTSUSPCOM
{
    partial class DeleteAndReapplyPayment
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("STFFRD");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("UNSTFD");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("PLUS/PLUSGB");
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlReapply = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlEDServicer = new System.Windows.Forms.FlowLayoutPanel();
            this.btnEDServicerAdd = new System.Windows.Forms.Button();
            this.btnEDServicerRemove = new System.Windows.Forms.Button();
            this.btnReapplyRemove = new System.Windows.Forms.Button();
            this.btnReapplyAdd = new System.Windows.Forms.Button();
            this.lvwDeals = new System.Windows.Forms.ListView();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lvwLoanTypes = new System.Windows.Forms.ListView();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.optionProcessorBaseBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.Location = new System.Drawing.Point(3, 228);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 23);
            this.label2.TabIndex = 41;
            this.label2.Text = "Reapplied";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label1.Location = new System.Drawing.Point(3, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 23);
            this.label1.TabIndex = 40;
            this.label1.Text = "Sent To ED Servicer";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlReapply
            // 
            this.pnlReapply.AutoScroll = true;
            this.pnlReapply.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlReapply.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlReapply.Location = new System.Drawing.Point(172, 228);
            this.pnlReapply.Name = "pnlReapply";
            this.pnlReapply.Size = new System.Drawing.Size(660, 80);
            this.pnlReapply.TabIndex = 39;
            this.pnlReapply.WrapContents = false;
            // 
            // pnlEDServicer
            // 
            this.pnlEDServicer.AutoScroll = true;
            this.pnlEDServicer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlEDServicer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlEDServicer.Location = new System.Drawing.Point(172, 144);
            this.pnlEDServicer.Name = "pnlEDServicer";
            this.pnlEDServicer.Size = new System.Drawing.Size(660, 81);
            this.pnlEDServicer.TabIndex = 38;
            this.pnlEDServicer.WrapContents = false;
            // 
            // btnEDServicerAdd
            // 
            this.btnEDServicerAdd.Location = new System.Drawing.Point(114, 170);
            this.btnEDServicerAdd.Name = "btnEDServicerAdd";
            this.btnEDServicerAdd.Size = new System.Drawing.Size(25, 23);
            this.btnEDServicerAdd.TabIndex = 42;
            this.btnEDServicerAdd.Text = "+";
            this.btnEDServicerAdd.UseVisualStyleBackColor = true;
            this.btnEDServicerAdd.Click += new System.EventHandler(this.btnEDServicerAdd_Click);
            // 
            // btnEDServicerRemove
            // 
            this.btnEDServicerRemove.Location = new System.Drawing.Point(141, 170);
            this.btnEDServicerRemove.Name = "btnEDServicerRemove";
            this.btnEDServicerRemove.Size = new System.Drawing.Size(25, 23);
            this.btnEDServicerRemove.TabIndex = 43;
            this.btnEDServicerRemove.Text = "-";
            this.btnEDServicerRemove.UseVisualStyleBackColor = true;
            this.btnEDServicerRemove.Click += new System.EventHandler(this.btnEDServicerRemove_Click);
            // 
            // btnReapplyRemove
            // 
            this.btnReapplyRemove.Location = new System.Drawing.Point(141, 254);
            this.btnReapplyRemove.Name = "btnReapplyRemove";
            this.btnReapplyRemove.Size = new System.Drawing.Size(25, 23);
            this.btnReapplyRemove.TabIndex = 45;
            this.btnReapplyRemove.Text = "-";
            this.btnReapplyRemove.UseVisualStyleBackColor = true;
            this.btnReapplyRemove.Click += new System.EventHandler(this.btnReapplyRemove_Click);
            // 
            // btnReapplyAdd
            // 
            this.btnReapplyAdd.Location = new System.Drawing.Point(114, 254);
            this.btnReapplyAdd.Name = "btnReapplyAdd";
            this.btnReapplyAdd.Size = new System.Drawing.Size(25, 23);
            this.btnReapplyAdd.TabIndex = 44;
            this.btnReapplyAdd.Text = "+";
            this.btnReapplyAdd.UseVisualStyleBackColor = true;
            this.btnReapplyAdd.Click += new System.EventHandler(this.btnReapplyAdd_Click);
            // 
            // lvwDeals
            // 
            this.lvwDeals.FullRowSelect = true;
            this.lvwDeals.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvwDeals.HideSelection = false;
            this.lvwDeals.Location = new System.Drawing.Point(172, 3);
            this.lvwDeals.Name = "lvwDeals";
            this.lvwDeals.Size = new System.Drawing.Size(283, 60);
            this.lvwDeals.TabIndex = 46;
            this.lvwDeals.UseCompatibleStateImageBehavior = false;
            this.lvwDeals.View = System.Windows.Forms.View.List;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 24);
            this.label3.TabIndex = 47;
            this.label3.Text = "Deal #";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 23);
            this.label4.TabIndex = 49;
            this.label4.Text = "Loan Type";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lvwLoanTypes
            // 
            this.lvwLoanTypes.FullRowSelect = true;
            this.lvwLoanTypes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvwLoanTypes.HideSelection = false;
            this.lvwLoanTypes.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3});
            this.lvwLoanTypes.Location = new System.Drawing.Point(172, 64);
            this.lvwLoanTypes.Name = "lvwLoanTypes";
            this.lvwLoanTypes.Size = new System.Drawing.Size(283, 60);
            this.lvwLoanTypes.TabIndex = 48;
            this.lvwLoanTypes.UseCompatibleStateImageBehavior = false;
            this.lvwLoanTypes.View = System.Windows.Forms.View.List;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(176, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(150, 18);
            this.label5.TabIndex = 50;
            this.label5.Text = "Loan Sequence #";
            this.label5.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(328, 127);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(150, 16);
            this.label6.TabIndex = 51;
            this.label6.Text = "Amount";
            this.label6.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(479, 127);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(150, 16);
            this.label7.TabIndex = 52;
            this.label7.Text = "Disb.Date (optional)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(630, 127);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(150, 16);
            this.label8.TabIndex = 53;
            this.label8.Text = "Transaction Type";
            this.label8.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // DeleteAndReapplyPayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lvwDeals);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lvwLoanTypes);
            this.Controls.Add(this.btnReapplyRemove);
            this.Controls.Add(this.btnReapplyAdd);
            this.Controls.Add(this.btnEDServicerRemove);
            this.Controls.Add(this.btnEDServicerAdd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlReapply);
            this.Controls.Add(this.pnlEDServicer);
            this.Name = "DeleteAndReapplyPayment";
            this.Load += new System.EventHandler(this.DeleteAndReapplyPayment_Load);
            this.Controls.SetChildIndex(this.label13, 0);
            this.Controls.SetChildIndex(this.textBox14, 0);
            this.Controls.SetChildIndex(this.pnlEDServicer, 0);
            this.Controls.SetChildIndex(this.pnlReapply, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.btnEDServicerAdd, 0);
            this.Controls.SetChildIndex(this.btnEDServicerRemove, 0);
            this.Controls.SetChildIndex(this.btnReapplyAdd, 0);
            this.Controls.SetChildIndex(this.btnReapplyRemove, 0);
            this.Controls.SetChildIndex(this.lvwLoanTypes, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.lvwDeals, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            ((System.ComponentModel.ISupportInitialize)(this.optionProcessorBaseBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Label label2;
        protected System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel pnlReapply;
        private System.Windows.Forms.FlowLayoutPanel pnlEDServicer;
        private System.Windows.Forms.Button btnEDServicerAdd;
        private System.Windows.Forms.Button btnEDServicerRemove;
        private System.Windows.Forms.Button btnReapplyRemove;
        private System.Windows.Forms.Button btnReapplyAdd;
        private System.Windows.Forms.ListView lvwDeals;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView lvwLoanTypes;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;

    }
}

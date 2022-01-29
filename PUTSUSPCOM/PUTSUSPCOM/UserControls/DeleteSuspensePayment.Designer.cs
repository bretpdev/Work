namespace PUTSUSPCOM
{
    partial class DeleteSuspensePayment
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
            this.lvwDeals = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lvwLoanTypes = new System.Windows.Forms.ListView();
            ((System.ComponentModel.ISupportInitialize)(this.optionProcessorBaseBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox14
            // 
            this.textBox14.Location = new System.Drawing.Point(172, 294);
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(3, 294);
            // 
            // lvwDeals
            // 
            this.lvwDeals.FullRowSelect = true;
            this.lvwDeals.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvwDeals.HideSelection = false;
            this.lvwDeals.Location = new System.Drawing.Point(172, 3);
            this.lvwDeals.Name = "lvwDeals";
            this.lvwDeals.Size = new System.Drawing.Size(283, 142);
            this.lvwDeals.TabIndex = 0;
            this.lvwDeals.UseCompatibleStateImageBehavior = false;
            this.lvwDeals.View = System.Windows.Forms.View.List;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Deal #";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Loan Type";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.lvwLoanTypes.Location = new System.Drawing.Point(172, 147);
            this.lvwLoanTypes.Name = "lvwLoanTypes";
            this.lvwLoanTypes.Size = new System.Drawing.Size(283, 147);
            this.lvwLoanTypes.TabIndex = 2;
            this.lvwLoanTypes.UseCompatibleStateImageBehavior = false;
            this.lvwLoanTypes.View = System.Windows.Forms.View.List;
            // 
            // DeleteSuspensePayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvwDeals);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lvwLoanTypes);
            this.Name = "DeleteSuspensePayment";
            this.Size = new System.Drawing.Size(835, 356);
            this.Load += new System.EventHandler(this.DeleteSuspensePayment_Load);
            this.Controls.SetChildIndex(this.label13, 0);
            this.Controls.SetChildIndex(this.textBox14, 0);
            this.Controls.SetChildIndex(this.lvwLoanTypes, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.lvwDeals, 0);
            ((System.ComponentModel.ISupportInitialize)(this.optionProcessorBaseBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvwDeals;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView lvwLoanTypes;

    }
}

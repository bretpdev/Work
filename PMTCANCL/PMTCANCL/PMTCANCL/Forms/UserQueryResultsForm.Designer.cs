namespace PMTCANCL
{
    partial class UserQueryResultsForm
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
            this.DisplaySelectListView = new System.Windows.Forms.ListView();
            this.Conf = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Ssn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AccountNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Borower = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PayType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PayAmt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PaySource = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PayCreated = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PayEffective = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ProcessedDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Deleted = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ConfirmButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.BackButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DisplaySelectListView
            // 
            this.DisplaySelectListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DisplaySelectListView.CheckBoxes = true;
            this.DisplaySelectListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Conf,
            this.Ssn,
            this.AccountNumber,
            this.Borower,
            this.PayType,
            this.PayAmt,
            this.PaySource,
            this.PayCreated,
            this.PayEffective,
            this.ProcessedDate,
            this.Deleted});
            this.DisplaySelectListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.DisplaySelectListView.FullRowSelect = true;
            this.DisplaySelectListView.GridLines = true;
            this.DisplaySelectListView.Location = new System.Drawing.Point(12, 12);
            this.DisplaySelectListView.MultiSelect = false;
            this.DisplaySelectListView.Name = "DisplaySelectListView";
            this.DisplaySelectListView.Size = new System.Drawing.Size(974, 668);
            this.DisplaySelectListView.TabIndex = 0;
            this.DisplaySelectListView.UseCompatibleStateImageBehavior = false;
            this.DisplaySelectListView.View = System.Windows.Forms.View.Details;
            this.DisplaySelectListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.DisplaySelectListView_ItemChecked);
            // 
            // Conf
            // 
            this.Conf.Text = "Confirmation";
            this.Conf.Width = 84;
            // 
            // Ssn
            // 
            this.Ssn.Text = "Ssn";
            this.Ssn.Width = 100;
            // 
            // AccountNumber
            // 
            this.AccountNumber.Text = "Account Number";
            this.AccountNumber.Width = 108;
            // 
            // Borower
            // 
            this.Borower.Text = "Borrower";
            this.Borower.Width = 158;
            // 
            // PayType
            // 
            this.PayType.Text = "Payment Type";
            this.PayType.Width = 96;
            // 
            // PayAmt
            // 
            this.PayAmt.Text = "Payment Amount";
            this.PayAmt.Width = 98;
            // 
            // PaySource
            // 
            this.PaySource.Text = "Payment Source";
            this.PaySource.Width = 108;
            // 
            // PayCreated
            // 
            this.PayCreated.Text = "Payment Created";
            this.PayCreated.Width = 95;
            // 
            // PayEffective
            // 
            this.PayEffective.Text = "PaymentEffective";
            this.PayEffective.Width = 98;
            // 
            // ProcessedDate
            // 
            this.ProcessedDate.Text = "Processed Date";
            this.ProcessedDate.Width = 94;
            // 
            // Deleted
            // 
            this.Deleted.Text = "Deleted";
            // 
            // ConfirmButton
            // 
            this.ConfirmButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ConfirmButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ConfirmButton.Location = new System.Drawing.Point(837, 687);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.Size = new System.Drawing.Size(150, 35);
            this.ConfirmButton.TabIndex = 1;
            this.ConfirmButton.Text = "Commit Deletion";
            this.ConfirmButton.UseVisualStyleBackColor = true;
            this.ConfirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.CancelButton.Location = new System.Drawing.Point(676, 687);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(150, 35);
            this.CancelButton.TabIndex = 2;
            this.CancelButton.Text = "Exit";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // BackButton
            // 
            this.BackButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BackButton.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.BackButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.BackButton.Location = new System.Drawing.Point(12, 687);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(150, 35);
            this.BackButton.TabIndex = 3;
            this.BackButton.Text = "Back";
            this.BackButton.UseVisualStyleBackColor = true;
            // 
            // UserQueryResultsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.BackButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.ConfirmButton);
            this.Controls.Add(this.DisplaySelectListView);
            this.MinimumSize = new System.Drawing.Size(713, 286);
            this.Name = "UserQueryResultsForm";
            this.Text = "UserQueryResultsForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView DisplaySelectListView;
        private System.Windows.Forms.ColumnHeader Conf;
        private System.Windows.Forms.ColumnHeader Ssn;
        private System.Windows.Forms.ColumnHeader AccountNumber;
        private System.Windows.Forms.ColumnHeader Borower;
        private System.Windows.Forms.ColumnHeader PayType;
        private System.Windows.Forms.ColumnHeader PayAmt;
        private System.Windows.Forms.ColumnHeader PaySource;
        private System.Windows.Forms.ColumnHeader PayCreated;
        private System.Windows.Forms.ColumnHeader PayEffective;
        private System.Windows.Forms.ColumnHeader ProcessedDate;
        private System.Windows.Forms.ColumnHeader Deleted;
        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button BackButton;
    }
}
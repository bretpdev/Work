namespace MauiDUDE
{
    partial class PreviousBorrowerDisplay
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
            this.components = new System.ComponentModel.Container();
            this.labelPound = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.labelSSN = new System.Windows.Forms.Label();
            this.buttonGO = new System.Windows.Forms.Button();
            this.previousBorrowerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.previousBorrowerBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // labelPound
            // 
            this.labelPound.Location = new System.Drawing.Point(3, 4);
            this.labelPound.Name = "labelPound";
            this.labelPound.Size = new System.Drawing.Size(20, 14);
            this.labelPound.TabIndex = 0;
            this.labelPound.Text = "#";
            this.labelPound.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelName
            // 
            this.labelName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.previousBorrowerBindingSource, "Name", true));
            this.labelName.Location = new System.Drawing.Point(24, 4);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(160, 14);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "Previous Borrower\'s Name";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelSSN
            // 
            this.labelSSN.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.previousBorrowerBindingSource, "SSN", true));
            this.labelSSN.Location = new System.Drawing.Point(184, 4);
            this.labelSSN.Name = "labelSSN";
            this.labelSSN.Size = new System.Drawing.Size(73, 17);
            this.labelSSN.TabIndex = 2;
            this.labelSSN.Text = "SSN";
            this.labelSSN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonGO
            // 
            this.buttonGO.Location = new System.Drawing.Point(258, 2);
            this.buttonGO.Name = "buttonGO";
            this.buttonGO.Size = new System.Drawing.Size(31, 19);
            this.buttonGO.TabIndex = 3;
            this.buttonGO.Text = "GO";
            this.buttonGO.UseVisualStyleBackColor = true;
            this.buttonGO.Click += new System.EventHandler(this.buttonGO_Click);
            // 
            // previousBorrowerBindingSource
            // 
            this.previousBorrowerBindingSource.DataSource = typeof(MauiDUDE.PreviousBorrower);
            // 
            // PreviousBorrowerDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonGO);
            this.Controls.Add(this.labelSSN);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.labelPound);
            this.Name = "PreviousBorrowerDisplay";
            this.Size = new System.Drawing.Size(290, 22);
            ((System.ComponentModel.ISupportInitialize)(this.previousBorrowerBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelPound;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelSSN;
        private System.Windows.Forms.Button buttonGO;
        private System.Windows.Forms.BindingSource previousBorrowerBindingSource;
    }
}

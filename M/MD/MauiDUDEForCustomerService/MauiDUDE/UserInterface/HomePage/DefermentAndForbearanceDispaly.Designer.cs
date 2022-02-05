namespace MauiDUDE
{ 
    partial class DefermentAndForbearanceDispaly
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
            this.labelType = new System.Windows.Forms.Label();
            this.labelBeginDate = new System.Windows.Forms.Label();
            this.labelEndDate = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelMonthsUsed = new System.Windows.Forms.Label();
            this.labelMonthsLeft = new System.Windows.Forms.Label();
            this.buttonGetMonthsLeft = new System.Windows.Forms.Button();
            this.defermentForbearanceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.defermentForbearanceBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // labelType
            // 
            this.labelType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.defermentForbearanceBindingSource, "Type", true));
            this.labelType.Location = new System.Drawing.Point(-1, 0);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(216, 29);
            this.labelType.TabIndex = 0;
            this.labelType.Text = "Type";
            this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelBeginDate
            // 
            this.labelBeginDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelBeginDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.defermentForbearanceBindingSource, "BeginDate", true));
            this.labelBeginDate.Location = new System.Drawing.Point(221, 0);
            this.labelBeginDate.Name = "labelBeginDate";
            this.labelBeginDate.Size = new System.Drawing.Size(73, 29);
            this.labelBeginDate.TabIndex = 1;
            this.labelBeginDate.Text = "Begin Date";
            this.labelBeginDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelEndDate
            // 
            this.labelEndDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.defermentForbearanceBindingSource, "EndDate", true));
            this.labelEndDate.Location = new System.Drawing.Point(290, 0);
            this.labelEndDate.Name = "labelEndDate";
            this.labelEndDate.Size = new System.Drawing.Size(73, 29);
            this.labelEndDate.TabIndex = 2;
            this.labelEndDate.Text = "End Date";
            this.labelEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.defermentForbearanceBindingSource, "CertDate", true));
            this.label2.Location = new System.Drawing.Point(359, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 29);
            this.label2.TabIndex = 3;
            this.label2.Text = "Cert Date";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.defermentForbearanceBindingSource, "CapInd", true));
            this.label3.Location = new System.Drawing.Point(427, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 29);
            this.label3.TabIndex = 4;
            this.label3.Text = "Cap Ind";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMonthsUsed
            // 
            this.labelMonthsUsed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.labelMonthsUsed.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.defermentForbearanceBindingSource, "MonthsUsed", true));
            this.labelMonthsUsed.Location = new System.Drawing.Point(497, 0);
            this.labelMonthsUsed.Name = "labelMonthsUsed";
            this.labelMonthsUsed.Size = new System.Drawing.Size(73, 29);
            this.labelMonthsUsed.TabIndex = 5;
            this.labelMonthsUsed.Text = "Months Used";
            this.labelMonthsUsed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMonthsLeft
            // 
            this.labelMonthsLeft.Location = new System.Drawing.Point(567, 0);
            this.labelMonthsLeft.Name = "labelMonthsLeft";
            this.labelMonthsLeft.Size = new System.Drawing.Size(73, 29);
            this.labelMonthsLeft.TabIndex = 6;
            this.labelMonthsLeft.Text = "Months Left";
            this.labelMonthsLeft.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonGetMonthsLeft
            // 
            this.buttonGetMonthsLeft.Location = new System.Drawing.Point(647, 3);
            this.buttonGetMonthsLeft.Name = "buttonGetMonthsLeft";
            this.buttonGetMonthsLeft.Size = new System.Drawing.Size(100, 23);
            this.buttonGetMonthsLeft.TabIndex = 7;
            this.buttonGetMonthsLeft.Text = "Get Months Left";
            this.buttonGetMonthsLeft.UseVisualStyleBackColor = true;
            this.buttonGetMonthsLeft.Click += new System.EventHandler(this.buttonGetMonthsLeft_Click);
            // 
            // defermentForbearanceBindingSource
            // 
            this.defermentForbearanceBindingSource.DataSource = typeof(MauiDUDE.DefermentForbearance);
            // 
            // DefermentAndForbearanceDispaly
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonGetMonthsLeft);
            this.Controls.Add(this.labelMonthsLeft);
            this.Controls.Add(this.labelMonthsUsed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelEndDate);
            this.Controls.Add(this.labelBeginDate);
            this.Controls.Add(this.labelType);
            this.Name = "DefermentAndForbearanceDispaly";
            this.Size = new System.Drawing.Size(760, 29);
            this.Load += new System.EventHandler(this.DefermentAndForbearanceDispaly_Load);
            ((System.ComponentModel.ISupportInitialize)(this.defermentForbearanceBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.Label labelBeginDate;
        private System.Windows.Forms.Label labelEndDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelMonthsUsed;
        private System.Windows.Forms.Label labelMonthsLeft;
        private System.Windows.Forms.BindingSource defermentForbearanceBindingSource;
        public System.Windows.Forms.Button buttonGetMonthsLeft;
    }
}

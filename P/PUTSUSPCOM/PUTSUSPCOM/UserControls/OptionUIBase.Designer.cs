namespace PUTSUSPCOM
{
    partial class OptionUIBase
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
            this.textBox14 = new System.Windows.Forms.TextBox();
            this.optionProcessorBaseBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label13 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.optionProcessorBaseBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox14
            // 
            this.textBox14.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.textBox14.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.optionProcessorBaseBindingSource, "Comments", true));
            this.textBox14.Location = new System.Drawing.Point(172, 310);
            this.textBox14.Multiline = true;
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new System.Drawing.Size(660, 59);
            this.textBox14.TabIndex = 32;
            // 
            // optionProcessorBaseBindingSource
            // 
            this.optionProcessorBaseBindingSource.DataSource = typeof(PUTSUSPCOM.OptionProcessorBase);
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label13.Location = new System.Drawing.Point(3, 310);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(163, 23);
            this.label13.TabIndex = 33;
            this.label13.Text = "Comments";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // OptionUIBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox14);
            this.Controls.Add(this.label13);
            this.Name = "OptionUIBase";
            this.Size = new System.Drawing.Size(835, 371);
            ((System.ComponentModel.ISupportInitialize)(this.optionProcessorBaseBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.BindingSource optionProcessorBaseBindingSource;
        protected System.Windows.Forms.TextBox textBox14;
        protected System.Windows.Forms.Label label13;
    }
}

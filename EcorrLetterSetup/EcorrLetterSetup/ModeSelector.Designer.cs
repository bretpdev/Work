namespace EcorrLetterSetup
{
    partial class ModeSelector
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
            this.Test = new System.Windows.Forms.Button();
            this.Live = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Test
            // 
            this.Test.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Test.Location = new System.Drawing.Point(13, 13);
            this.Test.Name = "Test";
            this.Test.Size = new System.Drawing.Size(91, 46);
            this.Test.TabIndex = 0;
            this.Test.Text = "Test";
            this.Test.UseVisualStyleBackColor = true;
            this.Test.Click += new System.EventHandler(this.Test_Click);
            // 
            // Live
            // 
            this.Live.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Live.Location = new System.Drawing.Point(13, 65);
            this.Live.Name = "Live";
            this.Live.Size = new System.Drawing.Size(91, 46);
            this.Live.TabIndex = 1;
            this.Live.Text = "Live";
            this.Live.UseVisualStyleBackColor = true;
            this.Live.Click += new System.EventHandler(this.Live_Click);
            // 
            // ModeSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(116, 121);
            this.Controls.Add(this.Live);
            this.Controls.Add(this.Test);
            this.Name = "ModeSelector";
            this.Text = "ModeSelector";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Test;
        private System.Windows.Forms.Button Live;
    }
}
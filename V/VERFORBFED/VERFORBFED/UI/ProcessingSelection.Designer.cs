namespace VERFORBFED
{
    partial class ProcessingSelection
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
            this.Verb = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.Collection = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.Continue = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Verb
            // 
            this.Verb.AutoSize = true;
            this.Verb.Location = new System.Drawing.Point(12, 12);
            this.Verb.Name = "Verb";
            this.Verb.Size = new System.Drawing.Size(168, 24);
            this.Verb.TabIndex = 0;
            this.Verb.TabStop = true;
            this.Verb.Text = "Verbal Forbearance";
            this.Verb.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(44, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(344, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Temporary financial hardship.";
            // 
            // Collection
            // 
            this.Collection.AutoSize = true;
            this.Collection.Location = new System.Drawing.Point(12, 108);
            this.Collection.Name = "Collection";
            this.Collection.Size = new System.Drawing.Size(184, 24);
            this.Collection.TabIndex = 1;
            this.Collection.TabStop = true;
            this.Collection.Text = "Collection Suspension";
            this.Collection.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(44, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(344, 32);
            this.label2.TabIndex = 1;
            this.label2.Text = "60-Day suspension of collection activities only.";
            // 
            // Continue
            // 
            this.Continue.Location = new System.Drawing.Point(188, 209);
            this.Continue.Name = "Continue";
            this.Continue.Size = new System.Drawing.Size(102, 39);
            this.Continue.TabIndex = 2;
            this.Continue.Text = "Continue";
            this.Continue.UseVisualStyleBackColor = true;
            this.Continue.Click += new System.EventHandler(this.Continue_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(305, 209);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(102, 39);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // ProcessingSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 260);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Continue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Collection);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Verb);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(440, 300);
            this.MinimumSize = new System.Drawing.Size(440, 299);
            this.Name = "ProcessingSelection";
            this.ShowIcon = false;
            this.Text = "Forbearance Selection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton Verb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton Collection;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Continue;
        private System.Windows.Forms.Button Cancel;
    }
}
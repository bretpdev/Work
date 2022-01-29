namespace CRPQASSIGN
{
    partial class QueueAssignment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueueAssignment));
            this.Qs = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Assign = new System.Windows.Forms.Button();
            this.Unassign = new System.Windows.Forms.Button();
            this.Sel = new System.Windows.Forms.CheckBox();
            this.Us = new CRPQASSIGN.MyListBox();
            this.SuspendLayout();
            // 
            // Qs
            // 
            this.Qs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Qs.FormattingEnabled = true;
            this.Qs.Location = new System.Drawing.Point(70, 6);
            this.Qs.Name = "Qs";
            this.Qs.Size = new System.Drawing.Size(160, 26);
            this.Qs.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Queue";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Users";
            // 
            // Assign
            // 
            this.Assign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Assign.Location = new System.Drawing.Point(155, 303);
            this.Assign.Name = "Assign";
            this.Assign.Size = new System.Drawing.Size(75, 37);
            this.Assign.TabIndex = 4;
            this.Assign.Text = "Assign";
            this.Assign.UseVisualStyleBackColor = true;
            this.Assign.Click += new System.EventHandler(this.Assign_Click);
            // 
            // Unassign
            // 
            this.Unassign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Unassign.Location = new System.Drawing.Point(12, 303);
            this.Unassign.Name = "Unassign";
            this.Unassign.Size = new System.Drawing.Size(80, 37);
            this.Unassign.TabIndex = 5;
            this.Unassign.Text = "Unassign";
            this.Unassign.UseVisualStyleBackColor = true;
            this.Unassign.Click += new System.EventHandler(this.Unassign_Click);
            // 
            // Sel
            // 
            this.Sel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Sel.AutoSize = true;
            this.Sel.Location = new System.Drawing.Point(126, 38);
            this.Sel.Name = "Sel";
            this.Sel.Size = new System.Drawing.Size(104, 22);
            this.Sel.TabIndex = 6;
            this.Sel.Text = "Deselect All";
            this.Sel.UseVisualStyleBackColor = true;
            this.Sel.CheckedChanged += new System.EventHandler(this.Sel_CheckedChanged);
            // 
            // Us
            // 
            this.Us.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Us.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Us.CheckOnClick = true;
            this.Us.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Us.FormattingEnabled = true;
            this.Us.Location = new System.Drawing.Point(12, 65);
            this.Us.Name = "Us";
            this.Us.Size = new System.Drawing.Size(218, 222);
            this.Us.TabIndex = 2;
            this.Us.UseCompatibleTextRendering = true;
            // 
            // QueueAssignment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 350);
            this.Controls.Add(this.Sel);
            this.Controls.Add(this.Unassign);
            this.Controls.Add(this.Assign);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Us);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Qs);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "QueueAssignment";
            this.Text = "Queue Assignment";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox Qs;
        private System.Windows.Forms.Label label1;
        private MyListBox Us;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Assign;
        private System.Windows.Forms.Button Unassign;
        private System.Windows.Forms.CheckBox Sel;
    }
}


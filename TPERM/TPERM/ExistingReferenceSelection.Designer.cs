namespace TPERM
{
    partial class ExistingReferenceSelection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExistingReferenceSelection));
            this.label1 = new System.Windows.Forms.Label();
            this.ReferelceSelection = new System.Windows.Forms.DataGridView();
            this.Continue = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ReferelceSelection)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(437, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please review the following reference(s) to determine if the entered reference al" +
    "ready exists on the system.  If there is not a match select continue without sel" +
    "ecting a reference.";
            // 
            // ReferelceSelection
            // 
            this.ReferelceSelection.AllowUserToAddRows = false;
            this.ReferelceSelection.AllowUserToDeleteRows = false;
            this.ReferelceSelection.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.ReferelceSelection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ReferelceSelection.Location = new System.Drawing.Point(12, 72);
            this.ReferelceSelection.Name = "ReferelceSelection";
            this.ReferelceSelection.RowHeadersVisible = false;
            this.ReferelceSelection.Size = new System.Drawing.Size(454, 266);
            this.ReferelceSelection.TabIndex = 1;
            // 
            // Continue
            // 
            this.Continue.Location = new System.Drawing.Point(391, 344);
            this.Continue.Name = "Continue";
            this.Continue.Size = new System.Drawing.Size(75, 30);
            this.Continue.TabIndex = 2;
            this.Continue.Text = "Continue";
            this.Continue.UseVisualStyleBackColor = true;
            this.Continue.Click += new System.EventHandler(this.Continue_Click);
            // 
            // ExistingReferenceSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 386);
            this.Controls.Add(this.Continue);
            this.Controls.Add(this.ReferelceSelection);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ExistingReferenceSelection";
            this.Text = "Existing Reference";
            ((System.ComponentModel.ISupportInitialize)(this.ReferelceSelection)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView ReferelceSelection;
        private System.Windows.Forms.Button Continue;
    }
}
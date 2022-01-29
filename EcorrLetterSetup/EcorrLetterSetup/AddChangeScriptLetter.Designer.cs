namespace EcorrLetterSetup
{
    partial class AddChangeScriptLetter
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
            this.LetterIdLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Headers = new System.Windows.Forms.ComboBox();
            this.HeaderTypes = new System.Windows.Forms.ComboBox();
            this.Order = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.OK = new System.Windows.Forms.Button();
            this.Active = new Uheaa.Common.WinForms.YesNoButton();
            ((System.ComponentModel.ISupportInitialize)(this.Order)).BeginInit();
            this.SuspendLayout();
            // 
            // LetterIdLabel
            // 
            this.LetterIdLabel.AutoSize = true;
            this.LetterIdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LetterIdLabel.Location = new System.Drawing.Point(12, 9);
            this.LetterIdLabel.Name = "LetterIdLabel";
            this.LetterIdLabel.Size = new System.Drawing.Size(77, 20);
            this.LetterIdLabel.TabIndex = 0;
            this.LetterIdLabel.Text = "Letter Id: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(50, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Header:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Header Type:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(63, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Order:";
            // 
            // Headers
            // 
            this.Headers.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.Headers.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.Headers.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Headers.FormattingEnabled = true;
            this.Headers.Location = new System.Drawing.Point(122, 44);
            this.Headers.Name = "Headers";
            this.Headers.Size = new System.Drawing.Size(262, 26);
            this.Headers.TabIndex = 5;
            // 
            // HeaderTypes
            // 
            this.HeaderTypes.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.HeaderTypes.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.HeaderTypes.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderTypes.FormattingEnabled = true;
            this.HeaderTypes.Location = new System.Drawing.Point(122, 79);
            this.HeaderTypes.Name = "HeaderTypes";
            this.HeaderTypes.Size = new System.Drawing.Size(262, 26);
            this.HeaderTypes.TabIndex = 6;
            // 
            // Order
            // 
            this.Order.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Order.Location = new System.Drawing.Point(122, 115);
            this.Order.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Order.Name = "Order";
            this.Order.Size = new System.Drawing.Size(120, 24);
            this.Order.TabIndex = 7;
            this.Order.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(60, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Active:";
            // 
            // OK
            // 
            this.OK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OK.Location = new System.Drawing.Point(309, 144);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 34);
            this.OK.TabIndex = 10;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Active
            // 
            this.Active.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Active.Location = new System.Drawing.Point(122, 146);
            this.Active.Name = "Active";
            this.Active.SelectedIndex = 0;
            this.Active.SelectedValue = true;
            this.Active.Size = new System.Drawing.Size(89, 31);
            this.Active.TabIndex = 8;
            this.Active.UseVisualStyleBackColor = true;
            // 
            // AddChangeScriptLetter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 196);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Active);
            this.Controls.Add(this.Order);
            this.Controls.Add(this.HeaderTypes);
            this.Controls.Add(this.Headers);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LetterIdLabel);
            this.Name = "AddChangeScriptLetter";
            this.ShowIcon = false;
            this.Text = "AddChangeScriptLetter";
            ((System.ComponentModel.ISupportInitialize)(this.Order)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LetterIdLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox Headers;
        private System.Windows.Forms.ComboBox HeaderTypes;
        private System.Windows.Forms.NumericUpDown Order;
        private Uheaa.Common.WinForms.YesNoButton Active;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button OK;
    }
}
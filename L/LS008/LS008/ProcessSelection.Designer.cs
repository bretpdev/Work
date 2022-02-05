namespace LS008
{
    partial class ProcessSelection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessSelection));
            this.label1 = new System.Windows.Forms.Label();
            this.DocNum = new System.Windows.Forms.Label();
            this.AN = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SSN = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ActSeq = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.AddComment = new System.Windows.Forms.Button();
            this.Pro = new System.Windows.Forms.CheckedListBox();
            this.Cancel = new System.Windows.Forms.Button();
            this.Continue = new System.Windows.Forms.Button();
            this.TC = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.CDN = new System.Windows.Forms.Button();
            this.CAN = new System.Windows.Forms.Button();
            this.CSSN = new System.Windows.Forms.Button();
            this.CAS = new System.Windows.Forms.Button();
            this.CTCN = new System.Windows.Forms.Button();
            this.DupSel = new System.Windows.Forms.Button();
            this.PlaceOnHold = new System.Windows.Forms.CheckBox();
            this.HistCmts = new System.Windows.Forms.Button();
            this.LastTask = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Document Number:";
            // 
            // DocNum
            // 
            this.DocNum.AutoSize = true;
            this.DocNum.Location = new System.Drawing.Point(180, 9);
            this.DocNum.Name = "DocNum";
            this.DocNum.Size = new System.Drawing.Size(162, 20);
            this.DocNum.TabIndex = 1;
            this.DocNum.Text = "99916259028925393";
            this.DocNum.Click += new System.EventHandler(this.DocNum_Click);
            // 
            // AN
            // 
            this.AN.AutoSize = true;
            this.AN.Location = new System.Drawing.Point(180, 42);
            this.AN.Name = "AN";
            this.AN.Size = new System.Drawing.Size(108, 20);
            this.AN.TabIndex = 4;
            this.AN.Text = "01234566789";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Account Number:";
            // 
            // SSN
            // 
            this.SSN.AutoSize = true;
            this.SSN.Location = new System.Drawing.Point(180, 75);
            this.SSN.Name = "SSN";
            this.SSN.Size = new System.Drawing.Size(90, 20);
            this.SSN.TabIndex = 7;
            this.SSN.Text = "123456789";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(128, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 20);
            this.label5.TabIndex = 6;
            this.label5.Text = "SSN:";
            // 
            // ActSeq
            // 
            this.ActSeq.AutoSize = true;
            this.ActSeq.Location = new System.Drawing.Point(180, 108);
            this.ActSeq.Name = "ActSeq";
            this.ActSeq.Size = new System.Drawing.Size(54, 20);
            this.ActSeq.TabIndex = 10;
            this.ActSeq.Text = "00281";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(79, 108);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 20);
            this.label7.TabIndex = 9;
            this.label7.Text = "Activity Seq:";
            // 
            // AddComment
            // 
            this.AddComment.Location = new System.Drawing.Point(31, 210);
            this.AddComment.Name = "AddComment";
            this.AddComment.Size = new System.Drawing.Size(128, 40);
            this.AddComment.TabIndex = 16;
            this.AddComment.Text = "Add Comment";
            this.AddComment.UseVisualStyleBackColor = true;
            this.AddComment.Click += new System.EventHandler(this.AddComment_Click);
            // 
            // Pro
            // 
            this.Pro.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Pro.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Pro.FormattingEnabled = true;
            this.Pro.Location = new System.Drawing.Point(169, 180);
            this.Pro.Name = "Pro";
            this.Pro.Size = new System.Drawing.Size(483, 292);
            this.Pro.TabIndex = 17;
            this.Pro.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.Pro_ItemCheck);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.Location = new System.Drawing.Point(557, 482);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(95, 40);
            this.Cancel.TabIndex = 22;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Continue
            // 
            this.Continue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Continue.Enabled = false;
            this.Continue.Location = new System.Drawing.Point(456, 482);
            this.Continue.Name = "Continue";
            this.Continue.Size = new System.Drawing.Size(95, 40);
            this.Continue.TabIndex = 21;
            this.Continue.Text = "Continue";
            this.Continue.UseVisualStyleBackColor = true;
            this.Continue.Click += new System.EventHandler(this.Continue_Click);
            // 
            // TC
            // 
            this.TC.AutoSize = true;
            this.TC.Location = new System.Drawing.Point(180, 141);
            this.TC.Name = "TC";
            this.TC.Size = new System.Drawing.Size(171, 20);
            this.TC.TabIndex = 13;
            this.TC.Text = "013689943000000178";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(162, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "Task Control Number:";
            // 
            // CDN
            // 
            this.CDN.Image = ((System.Drawing.Image)(resources.GetObject("CDN.Image")));
            this.CDN.Location = new System.Drawing.Point(348, 9);
            this.CDN.Name = "CDN";
            this.CDN.Size = new System.Drawing.Size(20, 20);
            this.CDN.TabIndex = 2;
            this.CDN.UseVisualStyleBackColor = true;
            this.CDN.Click += new System.EventHandler(this.CDN_Click);
            // 
            // CAN
            // 
            this.CAN.Image = ((System.Drawing.Image)(resources.GetObject("CAN.Image")));
            this.CAN.Location = new System.Drawing.Point(294, 42);
            this.CAN.Name = "CAN";
            this.CAN.Size = new System.Drawing.Size(20, 20);
            this.CAN.TabIndex = 5;
            this.CAN.UseVisualStyleBackColor = true;
            this.CAN.Click += new System.EventHandler(this.CAN_Click);
            // 
            // CSSN
            // 
            this.CSSN.Image = ((System.Drawing.Image)(resources.GetObject("CSSN.Image")));
            this.CSSN.Location = new System.Drawing.Point(276, 75);
            this.CSSN.Name = "CSSN";
            this.CSSN.Size = new System.Drawing.Size(20, 20);
            this.CSSN.TabIndex = 8;
            this.CSSN.UseVisualStyleBackColor = true;
            this.CSSN.Click += new System.EventHandler(this.CSSN_Click);
            // 
            // CAS
            // 
            this.CAS.Image = ((System.Drawing.Image)(resources.GetObject("CAS.Image")));
            this.CAS.Location = new System.Drawing.Point(240, 108);
            this.CAS.Name = "CAS";
            this.CAS.Size = new System.Drawing.Size(20, 20);
            this.CAS.TabIndex = 11;
            this.CAS.UseVisualStyleBackColor = true;
            this.CAS.Click += new System.EventHandler(this.CAS_Click);
            // 
            // CTCN
            // 
            this.CTCN.Image = ((System.Drawing.Image)(resources.GetObject("CTCN.Image")));
            this.CTCN.Location = new System.Drawing.Point(357, 141);
            this.CTCN.Name = "CTCN";
            this.CTCN.Size = new System.Drawing.Size(20, 20);
            this.CTCN.TabIndex = 14;
            this.CTCN.UseVisualStyleBackColor = true;
            this.CTCN.Click += new System.EventHandler(this.CTCN_Click);
            // 
            // DupSel
            // 
            this.DupSel.Enabled = false;
            this.DupSel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DupSel.Location = new System.Drawing.Point(389, 8);
            this.DupSel.Name = "DupSel";
            this.DupSel.Size = new System.Drawing.Size(111, 28);
            this.DupSel.TabIndex = 18;
            this.DupSel.Text = "Duplicate DCN\'s";
            this.DupSel.UseVisualStyleBackColor = true;
            this.DupSel.Click += new System.EventHandler(this.DupSel_Click);
            // 
            // PlaceOnHold
            // 
            this.PlaceOnHold.AutoSize = true;
            this.PlaceOnHold.Location = new System.Drawing.Point(31, 180);
            this.PlaceOnHold.Name = "PlaceOnHold";
            this.PlaceOnHold.Size = new System.Drawing.Size(123, 24);
            this.PlaceOnHold.TabIndex = 15;
            this.PlaceOnHold.Text = "Place on hold";
            this.PlaceOnHold.UseVisualStyleBackColor = true;
            this.PlaceOnHold.CheckedChanged += new System.EventHandler(this.PlaceOnHold_CheckedChanged);
            // 
            // HistCmts
            // 
            this.HistCmts.Enabled = false;
            this.HistCmts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HistCmts.Location = new System.Drawing.Point(389, 42);
            this.HistCmts.Name = "HistCmts";
            this.HistCmts.Size = new System.Drawing.Size(111, 28);
            this.HistCmts.TabIndex = 19;
            this.HistCmts.Text = "Show Comments";
            this.HistCmts.UseVisualStyleBackColor = true;
            this.HistCmts.Click += new System.EventHandler(this.HistCmts_Click);
            // 
            // LastTask
            // 
            this.LastTask.AutoSize = true;
            this.LastTask.Location = new System.Drawing.Point(12, 498);
            this.LastTask.Name = "LastTask";
            this.LastTask.Size = new System.Drawing.Size(100, 24);
            this.LastTask.TabIndex = 20;
            this.LastTask.Text = "Final Task";
            this.LastTask.UseVisualStyleBackColor = true;
            // 
            // ProcessSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 534);
            this.Controls.Add(this.LastTask);
            this.Controls.Add(this.HistCmts);
            this.Controls.Add(this.PlaceOnHold);
            this.Controls.Add(this.DupSel);
            this.Controls.Add(this.CTCN);
            this.Controls.Add(this.CAS);
            this.Controls.Add(this.CSSN);
            this.Controls.Add(this.CAN);
            this.Controls.Add(this.CDN);
            this.Controls.Add(this.TC);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Continue);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Pro);
            this.Controls.Add(this.AddComment);
            this.Controls.Add(this.ActSeq);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.SSN);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.AN);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DocNum);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ProcessSelection";
            this.Text = "Select the Document Type Below";
            this.Load += new System.EventHandler(this.ProcessSelection_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label DocNum;
        private System.Windows.Forms.Label AN;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label SSN;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label ActSeq;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button AddComment;
        private System.Windows.Forms.CheckedListBox Pro;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Continue;
        private System.Windows.Forms.Label TC;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button CDN;
        private System.Windows.Forms.Button CAN;
        private System.Windows.Forms.Button CSSN;
        private System.Windows.Forms.Button CAS;
        private System.Windows.Forms.Button CTCN;
        private System.Windows.Forms.Button DupSel;
        private System.Windows.Forms.CheckBox PlaceOnHold;
        private System.Windows.Forms.Button HistCmts;
        private System.Windows.Forms.CheckBox LastTask;
    }
}
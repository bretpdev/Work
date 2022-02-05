namespace CSharpScriptGenerator
{
	partial class AttributeChooser
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label3;
            this.sackerScriptBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkCallableFromDude = new System.Windows.Forms.CheckBox();
            this.scriptAttributesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.cmbScriptId = new System.Windows.Forms.ComboBox();
            this.chkBatch = new System.Windows.Forms.CheckBox();
            this.chkFederalDirect = new System.Windows.Forms.CheckBox();
            this.cboUseCSharp = new System.Windows.Forms.CheckBox();
            this.lblCSharp = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.sackerScriptBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scriptAttributesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 40);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(201, 13);
            label2.TabIndex = 2;
            label2.Text = "Choose a name for the script\'s main class";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(94, 13);
            label1.TabIndex = 0;
            label1.Text = "Choose a script ID";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(12, 147);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(226, 13);
            label4.TabIndex = 10;
            label4.Text = "Should the script be callable from MauiDUDE?";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(12, 120);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(162, 13);
            label5.TabIndex = 11;
            label5.Text = "Is this a batch/user batch script?";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(12, 93);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(146, 13);
            label3.TabIndex = 13;
            label3.Text = "Is this a Federal Direct script?";
            // 
            // sackerScriptBindingSource
            // 
            this.sackerScriptBindingSource.DataSource = typeof(CSharpScriptGenerator.SackerScript);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(31, 223);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(163, 223);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkCallableFromDude
            // 
            this.chkCallableFromDude.AutoSize = true;
            this.chkCallableFromDude.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.scriptAttributesBindingSource, "CanRunFromMauiDude", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkCallableFromDude.Location = new System.Drawing.Point(252, 147);
            this.chkCallableFromDude.Name = "chkCallableFromDude";
            this.chkCallableFromDude.Size = new System.Drawing.Size(15, 14);
            this.chkCallableFromDude.TabIndex = 5;
            this.chkCallableFromDude.UseVisualStyleBackColor = true;
            // 
            // scriptAttributesBindingSource
            // 
            this.scriptAttributesBindingSource.DataSource = typeof(CSharpScriptGenerator.ScriptAttributes);
            // 
            // txtClassName
            // 
            this.txtClassName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.scriptAttributesBindingSource, "StartingClassName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtClassName.Location = new System.Drawing.Point(15, 56);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(252, 20);
            this.txtClassName.TabIndex = 4;
            // 
            // cmbScriptId
            // 
            this.cmbScriptId.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbScriptId.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbScriptId.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.scriptAttributesBindingSource, "ScriptID", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbScriptId.DataSource = this.sackerScriptBindingSource;
            this.cmbScriptId.DisplayMember = "Id";
            this.cmbScriptId.FormattingEnabled = true;
            this.cmbScriptId.Location = new System.Drawing.Point(135, 6);
            this.cmbScriptId.Name = "cmbScriptId";
            this.cmbScriptId.Size = new System.Drawing.Size(132, 21);
            this.cmbScriptId.TabIndex = 3;
            this.cmbScriptId.ValueMember = "Id";
            this.cmbScriptId.SelectedIndexChanged += new System.EventHandler(this.cmbScriptId_SelectedIndexChanged);
            // 
            // chkBatch
            // 
            this.chkBatch.AutoSize = true;
            this.chkBatch.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.scriptAttributesBindingSource, "IsBatchScript", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkBatch.Location = new System.Drawing.Point(252, 120);
            this.chkBatch.Name = "chkBatch";
            this.chkBatch.Size = new System.Drawing.Size(15, 14);
            this.chkBatch.TabIndex = 12;
            this.chkBatch.UseVisualStyleBackColor = true;
            // 
            // chkFederalDirect
            // 
            this.chkFederalDirect.AutoSize = true;
            this.chkFederalDirect.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.scriptAttributesBindingSource, "IsFederalDirectScript", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkFederalDirect.Location = new System.Drawing.Point(252, 93);
            this.chkFederalDirect.Name = "chkFederalDirect";
            this.chkFederalDirect.Size = new System.Drawing.Size(15, 14);
            this.chkFederalDirect.TabIndex = 14;
            this.chkFederalDirect.UseVisualStyleBackColor = true;
            // 
            // cboUseCSharp
            // 
            this.cboUseCSharp.AutoSize = true;
            this.cboUseCSharp.Checked = true;
            this.cboUseCSharp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboUseCSharp.Location = new System.Drawing.Point(251, 175);
            this.cboUseCSharp.Name = "cboUseCSharp";
            this.cboUseCSharp.Size = new System.Drawing.Size(15, 14);
            this.cboUseCSharp.TabIndex = 15;
            this.cboUseCSharp.UseVisualStyleBackColor = true;
            // 
            // lblCSharp
            // 
            this.lblCSharp.AutoSize = true;
            this.lblCSharp.Location = new System.Drawing.Point(12, 176);
            this.lblCSharp.Name = "lblCSharp";
            this.lblCSharp.Size = new System.Drawing.Size(201, 13);
            this.lblCSharp.TabIndex = 16;
            this.lblCSharp.Text = "Should the script use C# Common Code?";
            // 
            // AttributeChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(283, 258);
            this.Controls.Add(this.lblCSharp);
            this.Controls.Add(this.cboUseCSharp);
            this.Controls.Add(this.chkFederalDirect);
            this.Controls.Add(label3);
            this.Controls.Add(this.chkBatch);
            this.Controls.Add(label5);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(label4);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(label1);
            this.Controls.Add(this.cmbScriptId);
            this.Controls.Add(this.chkCallableFromDude);
            this.Controls.Add(label2);
            this.Controls.Add(this.txtClassName);
            this.MaximizeBox = false;
            this.Name = "AttributeChooser";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "C# Script Generator";
            ((System.ComponentModel.ISupportInitialize)(this.sackerScriptBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scriptAttributesBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.BindingSource sackerScriptBindingSource;
		private System.Windows.Forms.CheckBox chkCallableFromDude;
		private System.Windows.Forms.TextBox txtClassName;
		private System.Windows.Forms.ComboBox cmbScriptId;
		private System.Windows.Forms.CheckBox chkBatch;
		private System.Windows.Forms.CheckBox chkFederalDirect;
		private System.Windows.Forms.BindingSource scriptAttributesBindingSource;
        private System.Windows.Forms.CheckBox cboUseCSharp;
        private System.Windows.Forms.Label lblCSharp;

	}
}
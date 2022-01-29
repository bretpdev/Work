using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Q;


namespace CompassARCAccess
{
	/// <summary>
	/// Summary description for frmCompassQAccess.
	/// </summary>
	public class frmCompassARCAccess : System.Windows.Forms.Form
	{
		enum TheRegion
		{
			Test,
			Live
		}
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ToolTip tTip;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.GroupBox gbQ;
		private System.Windows.Forms.GroupBox gbUser;
		private System.Windows.Forms.GroupBox gbModel;
		private System.ComponentModel.IContainer components;
		private System.Data.SqlClient.SqlConnection ConDA;
		private System.Windows.Forms.ComboBox cboUser;
		private string UserID;
		private System.Windows.Forms.RadioButton radModel;
		private System.Windows.Forms.RadioButton radRemove;
		private System.Windows.Forms.RadioButton radAdd;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.ListView lstUsers;
		private System.Windows.Forms.Button btnRemoveUser;
		private System.Windows.Forms.ComboBox cboModel;
		private System.Windows.Forms.Button btnProcess;
		private string Pass;
		private System.Windows.Forms.ColumnHeader ARC;
		private System.Windows.Forms.Button btnRemoveARC;
		private System.Windows.Forms.Button btnAddARC;
		private System.Windows.Forms.ListView lstARC;
		private System.Windows.Forms.TextBox txtARC;
		private string LiveConnStr = @"workstation id=""LGP-1191"";packet size=4096;integrated security=SSPI;data source=""NOCHOUSE"";persist security info=False;initial catalog=BSYS";
		private string TestConnStr = @"workstation id=""LGP-1191"";packet size=4096;integrated security=SSPI;data source=""OPSDEV"";persist security info=False;initial catalog=BSYS";
		private string ConnStr;
		private bool TestMode;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4; //modify when promted to live
		private frmApproval App;
		private System.Windows.Forms.Button btnclose; //approval form
		private string ARCErrsFound = ""; 

		public frmCompassARCAccess(string U, string P, bool TTestMode)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			UserID = U;
			Pass = P;
			TestMode = TTestMode;
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmCompassARCAccess));
			this.lstUsers = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.gbQ = new System.Windows.Forms.GroupBox();
			this.btnRemoveARC = new System.Windows.Forms.Button();
			this.btnAddARC = new System.Windows.Forms.Button();
			this.lstARC = new System.Windows.Forms.ListView();
			this.ARC = new System.Windows.Forms.ColumnHeader();
			this.label2 = new System.Windows.Forms.Label();
			this.txtARC = new System.Windows.Forms.TextBox();
			this.gbUser = new System.Windows.Forms.GroupBox();
			this.cboUser = new System.Windows.Forms.ComboBox();
			this.btnRemoveUser = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.gbModel = new System.Windows.Forms.GroupBox();
			this.cboModel = new System.Windows.Forms.ComboBox();
			this.tTip = new System.Windows.Forms.ToolTip(this.components);
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.radModel = new System.Windows.Forms.RadioButton();
			this.radRemove = new System.Windows.Forms.RadioButton();
			this.radAdd = new System.Windows.Forms.RadioButton();
			this.btnProcess = new System.Windows.Forms.Button();
			this.btnclose = new System.Windows.Forms.Button();
			this.gbQ.SuspendLayout();
			this.gbUser.SuspendLayout();
			this.gbModel.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// lstUsers
			// 
			this.lstUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					   this.columnHeader1,
																					   this.columnHeader2});
			this.lstUsers.FullRowSelect = true;
			this.lstUsers.Location = new System.Drawing.Point(8, 80);
			this.lstUsers.MultiSelect = false;
			this.lstUsers.Name = "lstUsers";
			this.lstUsers.Size = new System.Drawing.Size(192, 136);
			this.lstUsers.TabIndex = 3;
			this.tTip.SetToolTip(this.lstUsers, "Users");
			this.lstUsers.View = System.Windows.Forms.View.Details;
			this.lstUsers.DoubleClick += new System.EventHandler(this.lstUsers_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "UserID";
			this.columnHeader1.Width = 59;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Name";
			this.columnHeader2.Width = 129;
			// 
			// gbQ
			// 
			this.gbQ.Controls.Add(this.btnRemoveARC);
			this.gbQ.Controls.Add(this.btnAddARC);
			this.gbQ.Controls.Add(this.lstARC);
			this.gbQ.Controls.Add(this.label2);
			this.gbQ.Controls.Add(this.txtARC);
			this.gbQ.Enabled = false;
			this.gbQ.Location = new System.Drawing.Point(224, 80);
			this.gbQ.Name = "gbQ";
			this.gbQ.Size = new System.Drawing.Size(192, 184);
			this.gbQ.TabIndex = 3;
			this.gbQ.TabStop = false;
			this.gbQ.Text = "ARC";
			// 
			// btnRemoveARC
			// 
			this.btnRemoveARC.Location = new System.Drawing.Point(8, 112);
			this.btnRemoveARC.Name = "btnRemoveARC";
			this.btnRemoveARC.Size = new System.Drawing.Size(64, 23);
			this.btnRemoveARC.TabIndex = 4;
			this.btnRemoveARC.Text = "Remove";
			this.btnRemoveARC.Click += new System.EventHandler(this.btnRemoveQ_Click);
			// 
			// btnAddARC
			// 
			this.btnAddARC.Location = new System.Drawing.Point(8, 80);
			this.btnAddARC.Name = "btnAddARC";
			this.btnAddARC.Size = new System.Drawing.Size(64, 23);
			this.btnAddARC.TabIndex = 3;
			this.btnAddARC.Text = "Add";
			this.btnAddARC.Click += new System.EventHandler(this.btnAddQ_Click);
			// 
			// lstARC
			// 
			this.lstARC.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					 this.ARC});
			this.lstARC.Location = new System.Drawing.Point(80, 48);
			this.lstARC.MultiSelect = false;
			this.lstARC.Name = "lstARC";
			this.lstARC.Size = new System.Drawing.Size(104, 128);
			this.lstARC.TabIndex = 5;
			this.lstARC.View = System.Windows.Forms.View.Details;
			this.lstARC.DoubleClick += new System.EventHandler(this.lstQ_DoubleClick);
			// 
			// ARC
			// 
			this.ARC.Text = "ARC";
			this.ARC.Width = 100;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "ARC";
			// 
			// txtARC
			// 
			this.txtARC.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtARC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtARC.Location = new System.Drawing.Point(8, 48);
			this.txtARC.MaxLength = 5;
			this.txtARC.Name = "txtARC";
			this.txtARC.Size = new System.Drawing.Size(64, 22);
			this.txtARC.TabIndex = 0;
			this.txtARC.Text = "";
			this.txtARC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtARC_KeyPress);
			// 
			// gbUser
			// 
			this.gbUser.Controls.Add(this.cboUser);
			this.gbUser.Controls.Add(this.btnRemoveUser);
			this.gbUser.Controls.Add(this.btnAdd);
			this.gbUser.Controls.Add(this.lstUsers);
			this.gbUser.Enabled = false;
			this.gbUser.Location = new System.Drawing.Point(8, 80);
			this.gbUser.Name = "gbUser";
			this.gbUser.Size = new System.Drawing.Size(208, 256);
			this.gbUser.TabIndex = 2;
			this.gbUser.TabStop = false;
			this.gbUser.Text = "Users";
			// 
			// cboUser
			// 
			this.cboUser.Location = new System.Drawing.Point(8, 24);
			this.cboUser.Name = "cboUser";
			this.cboUser.Size = new System.Drawing.Size(184, 21);
			this.cboUser.TabIndex = 1;
			this.cboUser.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboUser_KeyPress);
			this.cboUser.TextChanged += new System.EventHandler(this.FindComboItem);
			// 
			// btnRemoveUser
			// 
			this.btnRemoveUser.Location = new System.Drawing.Point(112, 224);
			this.btnRemoveUser.Name = "btnRemoveUser";
			this.btnRemoveUser.Size = new System.Drawing.Size(88, 24);
			this.btnRemoveUser.TabIndex = 4;
			this.btnRemoveUser.Text = "Remove User";
			this.btnRemoveUser.Click += new System.EventHandler(this.btnRemoveUser_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(8, 48);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(184, 23);
			this.btnAdd.TabIndex = 2;
			this.btnAdd.Text = "Add User";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// gbModel
			// 
			this.gbModel.Controls.Add(this.cboModel);
			this.gbModel.Enabled = false;
			this.gbModel.Location = new System.Drawing.Point(224, 272);
			this.gbModel.Name = "gbModel";
			this.gbModel.Size = new System.Drawing.Size(192, 64);
			this.gbModel.TabIndex = 4;
			this.gbModel.TabStop = false;
			this.gbModel.Text = "Model After";
			// 
			// cboModel
			// 
			this.cboModel.Location = new System.Drawing.Point(4, 24);
			this.cboModel.Name = "cboModel";
			this.cboModel.Size = new System.Drawing.Size(184, 21);
			this.cboModel.TabIndex = 1;
			this.cboModel.TextChanged += new System.EventHandler(this.FindComboItem);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.label4);
			this.groupBox4.Controls.Add(this.label3);
			this.groupBox4.Controls.Add(this.label1);
			this.groupBox4.Controls.Add(this.radModel);
			this.groupBox4.Controls.Add(this.radRemove);
			this.groupBox4.Controls.Add(this.radAdd);
			this.groupBox4.Location = new System.Drawing.Point(8, 8);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(408, 64);
			this.groupBox4.TabIndex = 1;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Action";
			// 
			// label4
			// 
			this.label4.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(192)));
			this.label4.Location = new System.Drawing.Point(304, 40);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(96, 16);
			this.label4.TabIndex = 5;
			this.label4.Text = "Single User Only";
			// 
			// label3
			// 
			this.label3.ForeColor = System.Drawing.Color.Maroon;
			this.label3.Location = new System.Drawing.Point(160, 40);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 16);
			this.label3.TabIndex = 4;
			this.label3.Text = "Multiple Users";
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.Color.Green;
			this.label1.Location = new System.Drawing.Point(24, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Multiple Users";
			// 
			// radModel
			// 
			this.radModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.radModel.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(192)));
			this.radModel.Location = new System.Drawing.Point(288, 16);
			this.radModel.Name = "radModel";
			this.radModel.Size = new System.Drawing.Size(112, 24);
			this.radModel.TabIndex = 2;
			this.radModel.Text = "Model After";
			this.radModel.CheckedChanged += new System.EventHandler(this.radModel_CheckedChanged);
			// 
			// radRemove
			// 
			this.radRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.radRemove.ForeColor = System.Drawing.Color.Maroon;
			this.radRemove.Location = new System.Drawing.Point(144, 16);
			this.radRemove.Name = "radRemove";
			this.radRemove.Size = new System.Drawing.Size(112, 24);
			this.radRemove.TabIndex = 1;
			this.radRemove.Text = "Remove";
			this.radRemove.CheckedChanged += new System.EventHandler(this.radRemove_CheckedChanged);
			// 
			// radAdd
			// 
			this.radAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.radAdd.ForeColor = System.Drawing.Color.Green;
			this.radAdd.Location = new System.Drawing.Point(8, 16);
			this.radAdd.Name = "radAdd";
			this.radAdd.TabIndex = 0;
			this.radAdd.Text = "Add";
			this.radAdd.CheckedChanged += new System.EventHandler(this.radAdd_CheckedChanged);
			// 
			// btnProcess
			// 
			this.btnProcess.Location = new System.Drawing.Point(128, 344);
			this.btnProcess.Name = "btnProcess";
			this.btnProcess.TabIndex = 5;
			this.btnProcess.Text = "Process";
			this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
			// 
			// btnclose
			// 
			this.btnclose.Location = new System.Drawing.Point(224, 344);
			this.btnclose.Name = "btnclose";
			this.btnclose.TabIndex = 6;
			this.btnclose.Text = "Close";
			this.btnclose.Click += new System.EventHandler(this.btnclose_Click);
			// 
			// frmCompassARCAccess
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(424, 373);
			this.Controls.Add(this.btnclose);
			this.Controls.Add(this.btnProcess);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.gbModel);
			this.Controls.Add(this.gbUser);
			this.Controls.Add(this.gbQ);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmCompassARCAccess";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Compass ARC Access";
			this.Load += new System.EventHandler(this.frmCompassARCAccess_Load);
			this.gbQ.ResumeLayout(false);
			this.gbUser.ResumeLayout(false);
			this.gbModel.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void frmCompassARCAccess_Load(object sender, System.EventArgs e)
		{
			//init test mode variables
			if (TestMode)
			{
				ConnStr = TestConnStr;
			}
			else
			{	
				ConnStr = LiveConnStr;
			}
			ConDA = new System.Data.SqlClient.SqlConnection();
			System.Data.SqlClient.SqlCommand SC = new System.Data.SqlClient.SqlCommand();

			//Gather UserIDs
			SC.CommandText = "Select B.UserID, A.FirstName, A.LastName FROM SYSA_LST_Users A JOIN SYSA_LST_UserIDInfo B ON A.WindowsUserName = B.WindowsUserName WHERE B.UserID LIKE 'UT%' AND B.[Date Access Removed] IS NULL ORDER BY B.UserID";
			ConDA.ConnectionString = ConnStr;
			SC.Connection = ConDA;
			ConDA.Open(); 
			System.Data.SqlClient.SqlDataReader DR = SC.ExecuteReader();
			if (DR.HasRows )
			{
				while (DR.Read())
				{
					cboUser.Items.Add(DR.GetString(0) + "     " + DR.GetString(1) + " " + DR.GetString(2));
					cboModel.Items.Add(DR.GetString(0) + "     " + DR.GetString(1) + " " + DR.GetString(2));
				}
			
			}
			

			//Gather ARCs out of DB


			ConDA.Close();
		}

		private void radAdd_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radAdd.Checked == true)
			{
				gbUser.Enabled = true;
				gbQ.Enabled = true;
				gbModel.Enabled = false;
				cboModel.ResetText();
				while (lstARC.Items.Count != 0)
				{
					lstARC.Items.RemoveAt(0);
				}
			}
		}

		private void radRemove_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radRemove.Checked == true)
			{
				gbUser.Enabled = true;
				gbQ.Enabled = true;
				gbModel.Enabled = false;
				cboModel.ResetText();
				while (lstARC.Items.Count != 0)
				{
					lstARC.Items.RemoveAt(0);
				}
			}
		}

		private void radModel_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radModel.Checked == true)
			{
				gbUser.Enabled = true;
				gbQ.Enabled = false;
				gbModel.Enabled = true;
				txtARC.Clear();
				while (lstARC.Items.Count != 0)
				{
					lstARC.Items.RemoveAt(0);
				}
			}
		}

		private void FindComboItem(object sender, System.EventArgs e)
		{
			int intPos;
			ComboBox cboSent = new ComboBox();
			cboSent = (ComboBox)sender;
			if (cboSent.Text == "")
			{
				return;
			}
			if (cboSent.FindString(cboSent.Text) != -1)
			{
				intPos = cboSent.Text.Length;
				cboSent.TextChanged -= new EventHandler(FindComboItem);

				cboSent.SelectedItem = cboSent.Items[cboSent.FindString(cboSent.Text)];
				cboSent.TextChanged += new EventHandler(FindComboItem);
				cboSent.SelectionStart = intPos;
				cboSent.SelectionLength = cboSent.Text.Length - cboSent.SelectionStart;
			}
			else
			{
				MessageBox.Show("Unable to locate User.");
			}
				
		}


		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			addUser();
		}

		private void addUser()
		{
			if (cboUser.Text.Length == 0)
			{
				return;
			}
			if ((radModel.Checked == false) || (lstUsers.Items.Count != 1))
			{
				//if model after isn't selected or user list is not = 1 then process
				ListViewItem I = new ListViewItem();
				I.Text = cboUser.Text.Substring(0,7);
				I.SubItems.Add(cboUser.Text.Substring(12));
				bool Found;
				Found = false;
				for (int x = 0; x < lstUsers.Items.Count; x++)
				{
					if (lstUsers.Items[x].Text == cboUser.Text.Substring(0,7))
					{
						Found = true;
						break;
					}
				}

				if (!Found)
				{
					lstUsers.Items.Add(I);
					cboUser.Text = "";
				}
			}
			else
			{
				MessageBox.Show("Only one user at a time can be processed when the model after option is selected.","Only one user at a time",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
		}

		private void lstUsers_DoubleClick(object sender, System.EventArgs e)
		{
			RemoveUser();
		}

		private void btnRemoveUser_Click(object sender, System.EventArgs e)
		{
			RemoveUser();
		}

		private void RemoveUser()
		{
			if (lstUsers.SelectedItems.Count > 0)
			{
				lstUsers.Items.RemoveAt(lstUsers.SelectedItems[0].Index );
			}
		}


		private void btnAddQ_Click(object sender, System.EventArgs e)
		{
			AddARC();
		}

		private void AddARC()
		{
			int Idx;
			if (txtARC.Text.Length != 5)
			{
				MessageBox.Show("The ARC must be five characters.");
			}
			else
			{
				foreach (ListViewItem I in lstARC.Items)
				{
					if (I.Text == txtARC.Text)
					{
						return;
					}
				}
				Idx = lstARC.Items.Add(txtARC.Text).Index;
				if (radAdd.Checked)
				{
					lstARC.Items[Idx].SubItems.Add("Add");
				}
				else
				{
					lstARC.Items[Idx].SubItems.Add("Remove");
				}
				txtARC.Clear();
			}
		}

		private void btnRemoveQ_Click(object sender, System.EventArgs e)
		{
			RemoveARC();
		}

		private void lstQ_DoubleClick(object sender, System.EventArgs e)
		{
			RemoveARC();
		}

		private void RemoveARC()
		{
			if (lstARC.SelectedIndices.Count > 0)
			{
				lstARC.Items.RemoveAt(lstARC.SelectedIndices[0]);
			}
		}

		private void cboUser_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == "\r".ToCharArray(0,1)[0])
			{
				addUser();
			}
		}

		private void btnProcess_Click(object sender, System.EventArgs e)
		{
			//Validation//////////////////////////////
			if (radAdd.Checked == false && radRemove.Checked == false && radModel.Checked == false)
			{
				MessageBox.Show("You must select an action to take. (Add,Remove,Model After)");
				radAdd.Focus();
				return;
			}
			if (lstUsers.Items.Count < 1)
			{
				MessageBox.Show("You must select a user to modify.");
				cboUser.Focus();
				return;
			}
			if (lstARC.Items.Count < 1 && radModel.Checked == false)
			{
				MessageBox.Show("You must select a ARC to processes.");
				txtARC.Focus();
				return;
			}
			if (radModel.Checked == true && cboModel.Text == "")
			{
				MessageBox.Show("You must select a user to Model After.");
				cboModel.Focus();
				return;
			}
			/////////////////////////////////////////
			if (radAdd.Checked == true)
			{
				AddARCProcess();
			}
			else if (radRemove.Checked == true)
			{
				RemoveARCProcess();
			}
			else if (radModel.Checked == true)
			{
				if (lstUsers.Items.Count == 1)
				{
					ModelARCProcess();
				}
				else
				{
					MessageBox.Show("Only one user can be modified at a time when the model option is selected.");
					cboUser.Focus();
					return;
				}
			}
		}

		private void AddARCProcess()
		{
			ListViewItem[] Arr;
			ListViewItem[] Arr2;
			Arr = new ListViewItem[lstUsers.Items.Count];
			Arr2 = new ListViewItem[lstARC.Items.Count];
			lstUsers.Items.CopyTo(Arr,0);
			lstARC.Items.CopyTo(Arr2,0);
			lstUsers.Items.Clear();
			lstARC.Items.Clear();
			App = new frmApproval("Add ARC(s)",Arr,Arr2);
			App.MdiParent = this.MdiParent;
			App.btnCancel.Click += new EventHandler(AppBtnClickHdl);
			App.btnOk.Click += new EventHandler(AppBtnClickHdl);
			this.Visible = false;
			App.Show();
		}

		private void RemoveARCProcess()
		{
			ListViewItem[] Arr;
			ListViewItem[] Arr2;
			Arr = new ListViewItem[lstUsers.Items.Count];
			Arr2 = new ListViewItem[lstARC.Items.Count];
			lstUsers.Items.CopyTo(Arr,0);
			lstARC.Items.CopyTo(Arr2,0);
			lstUsers.Items.Clear();
			lstARC.Items.Clear();
			App = new frmApproval("Remove ARC(s)",Arr,Arr2);
			App.MdiParent = this.MdiParent;
			App.btnCancel.Click += new EventHandler(AppBtnClickHdl);
			App.btnOk.Click += new EventHandler(AppBtnClickHdl);
			this.Visible = false;
			App.Show();
		}

		private void ModelARCProcess()
		{
			int I = 0;
			ListViewItem[] Arr;
			ArrayList ArrLst = new ArrayList();
			ListViewItem[] Arr2;
			Arr = new ListViewItem[lstUsers.Items.Count];
			lstUsers.Items.CopyTo(Arr,0);
			lstUsers.Items.Clear();
			//MessageBox.Show("Ready To Figure");
			FigureOutModelAfterAddRemv(ref ArrLst,Arr[0].SubItems[0].Text);
			if (ArrLst.Count == 0)
			{
				//there is no difference between the two user IDs
				MessageBox.Show("Those two user IDs have the exact same ARC access.","Same Access",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
			else
			{
				//if there are differences between the two user IDs
				Arr2 = new ListViewItem[ArrLst.Count];
				while (I < ArrLst.Count)
				{
					Arr2[I] = (ListViewItem) ArrLst[I];
					I++;
				}
				App = new frmApproval("Model After",Arr,Arr2);
				App.MdiParent = this.MdiParent;
				App.btnCancel.Click += new EventHandler(AppBtnClickHdl);
				App.btnOk.Click += new EventHandler(AppBtnClickHdl);
				this.Visible = false;
				App.Show();
			}
		}

		//figure out which ARCs should be removed and which should be added based off model after userid
		//private void FigureOutModelAfterAddRemv(ref ListViewItem[] Arr, string LUID)
		private void FigureOutModelAfterAddRemv(ref ArrayList Arr, string LUID)
		{
			//MessageBox.Show("Start of FigureOutModelAfterAddRemv");
			System.Data.SqlClient.SqlCommand Comm = new System.Data.SqlClient.SqlCommand();
			System.Data.SqlClient.SqlDataReader Reader;
			ArrayList UserHas = new ArrayList();
			ArrayList ModelHas = new ArrayList();
			string[] ModelUID;
			int I;
			
			//MessageBox.Show("Split string for UT number");
			ModelUID = cboModel.Text.Split("     ".ToCharArray());
			//
			//MessageBox.Show("Ready 4 First Query");
			System.Data.SqlClient.SqlConnection Conn = new System.Data.SqlClient.SqlConnection(ConnStr);
			//get what model user has
			Comm.CommandText = "SELECT ARC FROM SYSA_REF_UserID_COMPASSARCs WHERE UserID = '" + ModelUID[0] + "'";
			Comm.Connection = Conn;
			Conn.Open();
			Reader = Comm.ExecuteReader();
			while (Reader.Read())
			{
				ModelHas.Add(Reader[0].ToString());
			}
			Reader.Close();
			//get what modifying user has
			//MessageBox.Show("Ready 4 Second Query");
			Comm.CommandText = "SELECT ARC FROM SYSA_REF_UserID_COMPASSARCs WHERE UserID = '" + LUID + "'";
			Comm.Connection = Conn;
			Reader = Comm.ExecuteReader();
			while (Reader.Read())
			{
				UserHas.Add(Reader[0].ToString());
			}
			Reader.Close();
			Conn.Close();
			//Figure out what modifying user needs added and removed
			//Added
			I = 0;
			while (I < ModelHas.Count)
			{
				if (UserHas.Contains(ModelHas[I].ToString()) == false)
				{
					//if in model ARCs and not in Modify ARCs then add to list as an add
					ListViewItem LI = new ListViewItem(ModelHas[I].ToString());
					LI.SubItems.Add("Add");
					Arr.Add(LI);
				}
				I++;
			}
			//Removed
			I = 0;
			while (I < UserHas.Count)
			{
				if (ModelHas.Contains(UserHas[I].ToString()) == false)
				{
					//if in model ARCs and not in Modify ARCs then add to list as an add
					ListViewItem LI = new ListViewItem(UserHas[I].ToString());
					LI.SubItems.Add("Remove");
					Arr.Add(LI);
				}
				I++;
			}
		}

		//event for hitting enter in ARC field
		private void txtARC_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == "\r".ToCharArray(0,1)[0])
			{
				AddARC();
			}
		}

		private void AppBtnClickHdl(object sender, EventArgs e)
		{
			System.Data.SqlClient.SqlConnection Conn = new System.Data.SqlClient.SqlConnection(ConnStr);
			System.Data.SqlClient.SqlCommand Comm = new System.Data.SqlClient.SqlCommand();
			System.Data.SqlClient.SqlDataReader reader;
			string ErrorEmailRecipientLst = "";
			ListViewItem[] Arr;
			ListViewItem[] Arr2;
			Arr = new ListViewItem[App.lstUsers.Items.Count];
			Arr2 = new ListViewItem[App.lstQ.Items.Count];
			ReflectionInterface RI;


			if (sender.Equals(App.btnCancel)) 
			{
				//copy data back to main form
				App.lstUsers.Items.CopyTo(Arr,0);
				App.lstQ.Items.CopyTo(Arr2,0);
				App.Close();
				lstUsers.Items.AddRange(Arr);
				//only copy data back for ARCs if modeled after is not selected
				if (radAdd.Checked || radRemove.Checked) {lstARC.Items.AddRange(Arr2);}
				this.Visible = true;
			}
			else
			{
				//user accepted summary screen and therefore the changes can be made.
				App.lstUsers.Items.CopyTo(Arr,0);
				App.lstQ.Items.CopyTo(Arr2,0);


				//process test region
				RI = new ReflectionInterface(true); //login to test region
				if (RI.Login(UserID,Pass)) //login to COMPASS)
				{
					//MessageBox.Show("OK");
					foreach (ListViewItem Itm in Arr)
					{
						ARCErrsFound = ""; //reset error string
						AddAndRemoveARCs(Itm.SubItems[0].Text,Arr2, TheRegion.Test, "test", ref RI);
						//send error email if errors where encountered
						if (ARCErrsFound.Length > 0)
						{
							Comm.Connection = Conn;
							Comm.CommandText = "SELECT WinUName FROM GENR_REF_MiscEmailNotif WHERE (TypeKey = 'Compass ARC Access Error')";
							Conn.Open();
							reader = Comm.ExecuteReader();
							while (reader.Read())
							{
								if (ErrorEmailRecipientLst.Length == 0)
								{
									ErrorEmailRecipientLst = ErrorEmailRecipientLst + reader["WinUName"].ToString() + "@utahsbr.edu";
								}
								else
								{
									ErrorEmailRecipientLst = ErrorEmailRecipientLst + "," + reader["WinUName"].ToString() + "@utahsbr.edu";
								}
							}
                            Common.SendMail(TestMode, ErrorEmailRecipientLst, Environment.UserName + "@utahsbr.edu", "COMPASS ARC Access Update Error", ARCErrsFound, "", "", "", Common.EmailImportanceLevel.Normal, true);							
						}
					}
					RI.CloseSession(); //end session

					RI = new ReflectionInterface(false); //login to live region
					//process live region
					if (RI.Login(UserID,Pass)) //login to COMPASS)
					{	
						foreach (ListViewItem Itm in Arr)
						{
							ARCErrsFound = ""; //reset error string
							AddAndRemoveARCs(Itm.SubItems[0].Text,Arr2, TheRegion.Live, "live", ref RI);
							//send error email if errors where encountered
							if (ARCErrsFound.Length > 0)
							{
								Comm.Connection = Conn;
								Comm.CommandText = "SELECT WinUName FROM GENR_REF_MiscEmailNotif WHERE (TypeKey = 'Compass ARC Access Error')";
								Conn.Open();
								reader = Comm.ExecuteReader();
								while (reader.Read())
								{
									if (ErrorEmailRecipientLst.Length == 0)
									{
										ErrorEmailRecipientLst = ErrorEmailRecipientLst + reader["WinUName"].ToString() + "@utahsbr.edu";
									}
									else
									{
										ErrorEmailRecipientLst = ErrorEmailRecipientLst + "," + reader["WinUName"].ToString() + "@utahsbr.edu";
									}
								}
                                Common.SendMail(TestMode, ErrorEmailRecipientLst, Environment.UserName + "@utahsbr.edu", "COMPASS ARC Access Update Error", ARCErrsFound, "", "", "", Common.EmailImportanceLevel.Normal, true);
							}
						}
                        RI.CloseSession(); //end session

						App.Close();
						MessageBox.Show("Processing Complete!","Processing Complete!",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
						this.Dispose();
					}
					else
					{
						//give user error message because app wasn't able to login
						MessageBox.Show("The user id is in use.  Please fix the problem and try again.","User ID In Use",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        RI.CloseSession(); //end session
						//copy data back to main form
						App.lstUsers.Items.CopyTo(Arr,0);
						App.lstQ.Items.CopyTo(Arr2,0);
						App.Close();
						lstUsers.Items.AddRange(Arr);
						//only copy data back for ARCs if modeled after is not selected
						if (radAdd.Checked || radRemove.Checked) {lstARC.Items.AddRange(Arr2);}
						this.Visible = true;
						return;
					}
				}
				else
				{
					//give user error message because app wasn't able to login
					MessageBox.Show("The user id is in use.  Please fix the problem and try again.","User ID In Use",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    RI.CloseSession(); //end session
					//copy data back to main form
					App.lstUsers.Items.CopyTo(Arr,0);
					App.lstQ.Items.CopyTo(Arr2,0);
					App.Close();
					lstUsers.Items.AddRange(Arr);
					//only copy data back for ARCs if modeled after is not selected
					if (radAdd.Checked || radRemove.Checked) {lstARC.Items.AddRange(Arr2);}
					this.Visible = true;
					return;
				}
			}
		}

		//Add and Remove ARC to user if they need them
		private void AddAndRemoveARCs(string UID, ListViewItem[] Chgs, TheRegion R, string region, ref ReflectionInterface RI)
		{
			//only perform updates if not testmode or testmode and updating test region
			if ((TestMode == false) || (TestMode == true && R == TheRegion.Test))
			{
				//check access to screens before processing starts
				RI.FastPath("TX3ZATX68");
				if (radAdd.Checked)
				{
					//check for add access on system
					RI.FastPath("TX3ZATX68");
					if (RI.Check4Text(1,72,"J0X03"))
					{
						this.MdiParent.Activate();
						MessageBox.Show("You don't have access to perform the needed functionality in the " + region + " region.","Unable To Process",MessageBoxButtons.OK,MessageBoxIcon.Error);
						this.Close();
					}
				}
				else if (radRemove.Checked)
				{
					//check for delete access on system
					RI.FastPath("TX3ZDTX68");
					if (RI.Check4Text(1,72,"J0X03"))
					{
						this.MdiParent.Activate();
						MessageBox.Show("You don't have access to perform the needed functionality in the " + region + " region.","Unable To Process",MessageBoxButtons.OK,MessageBoxIcon.Error);
						this.Close();
					}
				}
				else //model after
				{
					//check for add and delete access on system
					RI.FastPath("TX3ZATX68");
					if (RI.Check4Text(1,72,"J0X03"))
					{
						this.MdiParent.Activate();
						MessageBox.Show("You don't have access to perform the needed functionality in the " + region + " region.","Unable To Process",MessageBoxButtons.OK,MessageBoxIcon.Error);
						this.Close();
					}
					RI.FastPath("TX3ZDTX68");
					if (RI.Check4Text(1,72,"J0X03"))
					{
						this.MdiParent.Activate();
						MessageBox.Show("You don't have access to perform the needed functionality in the " + region + " region.","Unable To Process",MessageBoxButtons.OK,MessageBoxIcon.Error);
						this.Close();
					}
				}
				//process each change listed
				foreach (ListViewItem Itm in Chgs)
				{
					if (Itm.SubItems[1].Text == "Remove")
					{
						//Remove
						RemoveARCs(UID,Itm.SubItems[0].Text,RI,region);
					}
					else
					{
						//Add
						AddARCs(UID,Itm.SubItems[0].Text,RI,region);
					}
				}
			}
		}

		private void RemoveARCs(string UID, string ARC, ReflectionInterface ri,string region)
		{
			System.Data.SqlClient.SqlCommand Comm = new System.Data.SqlClient.SqlCommand();
			System.Data.SqlClient.SqlConnection Conn = new System.Data.SqlClient.SqlConnection(ConnStr); //create connection
			ri.FastPath("TX3ZITD00" + ARC);
			if (ri.Check4Text(1,72,"TDX03") == false)
			{
				this.MdiParent.Activate();
				MessageBox.Show(@"ARC """ + ARC + @""" does not exist in the " + region + " region.","ARC Doesn't Exist",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
			else
			{
				ri.FastPath("TX3ZDTX68" + UID + ";" + ARC);
				//check for error codes
				if (ri.Check4Text(23,2,"90018"))
				{
                    if (region != "test")
                    {
                        //invalid user ID
                        this.MdiParent.Activate();
                        MessageBox.Show("That user ID either doesn't exist or it doesn't have access to the COMPASS " + region + " region.", "User ID Not Found On System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
				}
				else if (ri.Check4Text(23,2,"01487"))
				{
					//invalid ARC
					this.MdiParent.Activate();
					MessageBox.Show("ARC " + ARC + " doesn't exist in the COMPASS " + region + " region.","ARC Not Found On System",MessageBoxButtons.OK,MessageBoxIcon.Warning);
					return;
				}
				else if (ri.Check4Text(23,2,"01019"))
				{
					//User already has access to the ARC
					this.MdiParent.Activate();
					MessageBox.Show("The user doesn't have access to the " + ARC + " ARC.","User ID Doesn't Have Access To ARC",MessageBoxButtons.OK,MessageBoxIcon.Warning);
					return;
				}
				else if (ri.Check4Text(1,72,"TXX6C") == false)
				{
                    if (region != "test")
                    {
                        //not on right screen for some reason
                        this.MdiParent.Activate();
                        MessageBox.Show("The application encountered a screen that it didn't expect.  The application is now shutting down.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                    }
				}
				else
				{
					//everything seems to be OK
					ri.Hit(ReflectionInterface.Key.Enter);
					if (ri.Check4Text(23,2,"01006") == false)
					{
                        if (region != "test")
                        {
                            //problem removing ARC
                            this.MdiParent.Activate();
                            MessageBox.Show("The application was unable to update the system.  The application is now shutting down.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Close();
                        }
					}
					//double check that the changes took
					if (ARCFound(ARC,UID,ri))
					{
						//changes didn't take for one reason or another
						if (ARCErrsFound.Length == 0) 
						{
							ARCErrsFound = "User ID = " + UID + "\n";
							if (radAdd.Checked)
							{
								ARCErrsFound = ARCErrsFound + "User Selection = Add\n\n";
							}
							else if (radRemove.Checked)
							{
								ARCErrsFound = ARCErrsFound + "User Selection = Remove\n\n";
							}
							else
							{
								ARCErrsFound = ARCErrsFound + "User Selection = Model After\n\n";
							}
							ARCErrsFound = ARCErrsFound + "ARC = " + ARC + " Region = " + region + " Action = Remove";
						}
						else
						{	
							ARCErrsFound = ARCErrsFound + "\nARC = " + ARC + " Region = " + region + " Action = Remove";
						}
					}
					else
					{
                            //everything went as it should have
                            Comm.Connection = Conn;
                            Comm.CommandText = "DELETE FROM SYSA_REF_UserID_COMPASSARCs WHERE UserID = '" + UID + "' AND ARC = '" + ARC + "'";
                            Conn.Open();
                            Comm.ExecuteNonQuery();
                            Conn.Close();
					}
				}
			}
		}

		//adds one ARC at a time
		private void AddARCs(string UID, string ARC, ReflectionInterface ri, string region)
		{
			System.Data.SqlClient.SqlCommand Comm = new System.Data.SqlClient.SqlCommand();
			System.Data.SqlClient.SqlConnection Conn = new System.Data.SqlClient.SqlConnection(ConnStr); //create connection
			ri.FastPath("TX3ZITD00" + ARC);
			if (ri.Check4Text(1,72,"TDX03") == false)
			{
				this.MdiParent.Activate();
				MessageBox.Show(@"ARC """ + ARC + @""" does not exist in the " + region + " region.","ARC Doesn't Exist",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
			else
			{
				ri.FastPath("TX3ZATX68" + UID + ";" + ARC);
				//check for error codes
				if (ri.Check4Text(23,2,"90018"))
				{
                    if (region != "test")
                    {
                        //invalid user ID
                        this.MdiParent.Activate();
                        MessageBox.Show("That user ID either doesn't exist or it doesn't have access to the COMPASS " + region + " region.", "User ID Not Found On System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
				}
				else if (ri.Check4Text(23,2,"01487"))
				{
					//invalid ARC
					this.MdiParent.Activate();
					MessageBox.Show("ARC " + ARC + " doesn't exist in the COMPASS " + region + " region.","ARC Not Found On System",MessageBoxButtons.OK,MessageBoxIcon.Warning);
					return;
				}
				else if (ri.Check4Text(23,2,"02716"))
				{
					//ARC not user requestable
					this.MdiParent.Activate();
					MessageBox.Show("ARC " + ARC + " isn't user requestable in the COMPASS " + region + " region.","ARC Not User Requestable",MessageBoxButtons.OK,MessageBoxIcon.Warning);
					return;
				}
				else if (ri.Check4Text(23,2,"01018"))
				{
					//User already has access to the ARC
					this.MdiParent.Activate();
					MessageBox.Show("The user already has access to the " + ARC + " ARC.","User ID Already Has Access To ARC",MessageBoxButtons.OK,MessageBoxIcon.Warning);
					return;
				}
				else if (ri.Check4Text(1,72,"TXX6C") == false)
				{
                    if (region != "test")
                    {
                        //not on right screen for some reason
                        this.MdiParent.Activate();
                        MessageBox.Show("The application encountered a screen that it didn't expect.  The application is now shutting down.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                    }
				}
				else
				{
					//everything seems to be OK
                    ri.Hit(ReflectionInterface.Key.Enter);
					if (ri.Check4Text(23,2,"01004") == false)
					{
                        if (region != "test")
                        {
                            //problem removing ARC
                            this.MdiParent.Activate();
                            MessageBox.Show("The application was unable to update the system.  The application is now shutting down.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Close();
                        }
					}
					//double check that the changes took
					ri.FastPath("TX3ZITX68" + UID + ";" + ARC);
					if (ri.Check4Text(1,72,"TXX6C") == false)
					{
						//changes didn't take for one reason or another
						if (ARCErrsFound.Length == 0) 
						{
							ARCErrsFound = "User ID = " + UID + "\n";
							if (radAdd.Checked)
							{
								ARCErrsFound = ARCErrsFound + "User Selection = Add\n\n";
							}
							else if (radRemove.Checked)
							{
								ARCErrsFound = ARCErrsFound + "User Selection = Remove\n\n";
							}
							else
							{
								ARCErrsFound = ARCErrsFound + "User Selection = Model After\n\n";
							}
							ARCErrsFound = ARCErrsFound + "ARC = " + ARC + " Region = " + region + " Action = Add";
						}
						else
						{	
							ARCErrsFound = ARCErrsFound + "\nARC = " + ARC + " Region = " + region + " Action = Add";
						}
					}
					else
					{
						//everything went as it should have
						Comm.Connection = Conn;
						Comm.CommandText = "INSERT INTO SYSA_REF_UserID_COMPASSARCs (UserID, ARC) VALUES ('" + UID + "', '" + ARC + "')";
						Conn.Open();
						try
						{
							Comm.ExecuteNonQuery();
						}
						catch
						{
							//do nothing, just don't error
							//MessageBox.Show("Error Caught");
						}
						Conn.Close();
					}
				}
			}
		}

		private void btnclose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		//returns true if the ARC is found for the user
		private bool ARCFound(string ARC, string UID, ReflectionInterface ri)
		{
			int row = 8;
			ri.FastPath("TX3ZITX68" + UID);
			if (ri.Check4Text(1,72,"TXX6A"))
			{
				//selection screen
				while (ri.Check4Text(23,2,"90007 NO MORE DATA TO DISPLAY") == false)
				{
					if (ri.Check4Text(row,6,ARC))
					{
						//ARC was found
						if (ri.Check4Text(row,35,"REQUEST"))
						{
							return true;
						}
						else
						{
							return false;
						}
					}
					row++;
					//page forward and reset row counter
					if (row == 20)
					{
						row = 8;
						if (ri.Check4Text(23,2,"01033 PRESS ENTER TO DISPLAY MORE DATA"))
						{
							//hit enter to page forward if message comes up
                            ri.Hit(ReflectionInterface.Key.Enter);
						}
						else
						{
							//hit f8 to page forward
                            ri.Hit(ReflectionInterface.Key.F8);
						}
					}
				}
				//if control wasn't returned during the loop above then the ARC wasn't found and false can be returned
				//MessageBox.Show("false");
				return false;
			}
			else if (ri.Check4Text(1,72,"TXX6C"))
			{
				//MessageBox.Show("OK");
				//target screen
				return ri.Check4Text(11,18,ARC);
			}
			else
			{
				//MessageBox.Show("OK");
				//nothing
				return false;
			}
		}


	}



}

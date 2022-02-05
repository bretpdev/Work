using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;


namespace CompassQAccess
{
	/// <summary>
	/// Summary description for frmCompassQAccess.
	/// </summary>
	public class frmCompassQAccess : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ToolTip tTip;
		private System.Windows.Forms.GroupBox gbQ;
		private System.Windows.Forms.GroupBox gbUser;
		private System.Windows.Forms.GroupBox gbModel;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ComboBox cboUser;
		private string UserID;
		private System.Windows.Forms.RadioButton radModel;
		private System.Windows.Forms.RadioButton radRemove;
		private System.Windows.Forms.RadioButton radAdd;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.ListView lstUsers;
		private System.Windows.Forms.Button btnRemoveUser;
		private System.Windows.Forms.ComboBox cboModel;
		private System.Windows.Forms.ListView lstQ;
		private System.Windows.Forms.Button btnAll;
		private System.Windows.Forms.TextBox txtSubQ;
		private System.Windows.Forms.TextBox txtQ;
		private System.Windows.Forms.Button btnProcess;
		private string Pass;
		private System.Windows.Forms.ColumnHeader Queue;
		private System.Windows.Forms.Button btnAddQ;
		private System.Windows.Forms.Button btnRemoveQ;
		private ArrayList QueueArr;
		
		private frmApproval App;
		private System.Windows.Forms.Button btnClose;
		private bool TestMode;
		private ArrayList UserIDArr;
//		private ArrayList NameArr;
		private System.Data.SqlClient.SqlConnection conBSYS;
		private System.Data.SqlClient.SqlDataAdapter daBSYS;
		private System.Data.SqlClient.SqlCommand SelUsers;
		private System.Data.SqlClient.SqlCommand SelQ;
		private System.Data.SqlClient.SqlCommand SelModelQ;

		//private ArrayList QUserTypeArr;
		private Hashtable tbUserName;
		private System.Windows.Forms.GroupBox gbAction;
		private System.Windows.Forms.Label lblQueue;
		private Hashtable tbUserType;


		public frmCompassQAccess(string U, string P,bool TTestMode)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			Bitmap Pic;
			Pic = new Bitmap(this.Width,this.Height);
			Graphics gr;
			gr = Graphics.FromImage(Pic);
			gr.Clear(this.BackColor );
			gr.DrawString("Q",new Font("Bremen Bd BT",300),Brushes.DarkGray,0,-100);
			this.BackgroundImage = Pic;



			TestMode = TTestMode;
			UserID = U;
			Pass = P;
			tbUserName = new Hashtable();
			tbUserType = new Hashtable();
			conBSYS =  new System.Data.SqlClient.SqlConnection();
			daBSYS = new System.Data.SqlClient.SqlDataAdapter(); 
			SelUsers = new System.Data.SqlClient.SqlCommand();
			SelQ = new System.Data.SqlClient.SqlCommand();
			SelModelQ = new System.Data.SqlClient.SqlCommand();
			if (TestMode == true)
			{
				conBSYS.ConnectionString = @"workstation id=""LPP-1494"";packet size=4096;integrated security=SSPI;data source=""OPSDEV"";persist security info=False;initial catalog=BSYS";
			}
			else
			{
				conBSYS.ConnectionString = @"workstation id=""LPP-1494"";packet size=4096;integrated security=SSPI;data source=""NOCHOUSE"";persist security info=False;initial catalog=BSYS";
			}
			//Text to select Users
			SelUsers.CommandText = "SELECT SYSA_LST_UserIDInfo.UserID, SYSA_LST_Users.LastName, SYSA_LST_Users.FirstName, SYSA_LST_UserIDInfo.QUserType FROM SYSA_LST_UserIDInfo INNER JOIN SYSA_LST_Users ON SYSA_LST_UserIDInfo.WindowsUserName = SYSA_LST_Users.WindowsUserName WHERE (SYSA_LST_UserIDInfo.[Date Access Removed] IS NULL) AND (SYSA_LST_UserIDInfo.UserID LIKE N'UT00%')";
			//Text to select Queues
			SelQ.CommandText = "SELECT QueueName FROM QSTA_LST_QueueDetail WHERE (SystemIndicator = 'Compass')";
			//Text to select "Model After" Queues
			//SelModelQ.CommandText = "SELECT Queue FROM SYSA_REF_UserID_COMPASSQueue WHERE (UserID = '')";
			//set connection
			SelUsers.Connection = conBSYS;
			SelQ.Connection = conBSYS;
			SelModelQ.Connection = conBSYS;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmCompassQAccess));
			this.lstUsers = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.gbQ = new System.Windows.Forms.GroupBox();
			this.btnRemoveQ = new System.Windows.Forms.Button();
			this.btnAddQ = new System.Windows.Forms.Button();
			this.lstQ = new System.Windows.Forms.ListView();
			this.Queue = new System.Windows.Forms.ColumnHeader();
			this.btnAll = new System.Windows.Forms.Button();
			this.txtSubQ = new System.Windows.Forms.TextBox();
			this.lblQueue = new System.Windows.Forms.Label();
			this.txtQ = new System.Windows.Forms.TextBox();
			this.gbUser = new System.Windows.Forms.GroupBox();
			this.cboUser = new System.Windows.Forms.ComboBox();
			this.btnRemoveUser = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.gbModel = new System.Windows.Forms.GroupBox();
			this.cboModel = new System.Windows.Forms.ComboBox();
			this.tTip = new System.Windows.Forms.ToolTip(this.components);
			this.gbAction = new System.Windows.Forms.GroupBox();
			this.radModel = new System.Windows.Forms.RadioButton();
			this.radRemove = new System.Windows.Forms.RadioButton();
			this.radAdd = new System.Windows.Forms.RadioButton();
			this.btnProcess = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.gbQ.SuspendLayout();
			this.gbUser.SuspendLayout();
			this.gbModel.SuspendLayout();
			this.gbAction.SuspendLayout();
			this.SuspendLayout();
			// 
			// lstUsers
			// 
			this.lstUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					   this.columnHeader1,
																					   this.columnHeader2});
			this.lstUsers.FullRowSelect = true;
			this.lstUsers.Location = new System.Drawing.Point(8, 80);
			this.lstUsers.Name = "lstUsers";
			this.lstUsers.Size = new System.Drawing.Size(192, 136);
			this.lstUsers.TabIndex = 3;
			this.tTip.SetToolTip(this.lstUsers, "Double Click on a User to remove it.");
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
			this.gbQ.BackColor = System.Drawing.Color.Transparent;
			this.gbQ.Controls.Add(this.btnRemoveQ);
			this.gbQ.Controls.Add(this.btnAddQ);
			this.gbQ.Controls.Add(this.lstQ);
			this.gbQ.Controls.Add(this.btnAll);
			this.gbQ.Controls.Add(this.txtSubQ);
			this.gbQ.Controls.Add(this.lblQueue);
			this.gbQ.Controls.Add(this.txtQ);
			this.gbQ.Enabled = false;
			this.gbQ.Location = new System.Drawing.Point(224, 72);
			this.gbQ.Name = "gbQ";
			this.gbQ.Size = new System.Drawing.Size(192, 184);
			this.gbQ.TabIndex = 3;
			this.gbQ.TabStop = false;
			this.gbQ.Text = "Queues";
			// 
			// btnRemoveQ
			// 
			this.btnRemoveQ.Location = new System.Drawing.Point(8, 144);
			this.btnRemoveQ.Name = "btnRemoveQ";
			this.btnRemoveQ.Size = new System.Drawing.Size(64, 23);
			this.btnRemoveQ.TabIndex = 4;
			this.btnRemoveQ.Text = "Remove";
			this.btnRemoveQ.Click += new System.EventHandler(this.btnRemoveQ_Click);
			// 
			// btnAddQ
			// 
			this.btnAddQ.Location = new System.Drawing.Point(8, 112);
			this.btnAddQ.Name = "btnAddQ";
			this.btnAddQ.Size = new System.Drawing.Size(64, 23);
			this.btnAddQ.TabIndex = 3;
			this.btnAddQ.Text = "Add";
			this.btnAddQ.Click += new System.EventHandler(this.btnAddQ_Click);
			// 
			// lstQ
			// 
			this.lstQ.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																				   this.Queue});
			this.lstQ.Location = new System.Drawing.Point(80, 48);
			this.lstQ.Name = "lstQ";
			this.lstQ.Size = new System.Drawing.Size(104, 128);
			this.lstQ.TabIndex = 5;
			this.tTip.SetToolTip(this.lstQ, "Double Click on a Queue to remove it.");
			this.lstQ.View = System.Windows.Forms.View.Details;
			this.lstQ.DoubleClick += new System.EventHandler(this.lstQ_DoubleClick);
			// 
			// Queue
			// 
			this.Queue.Text = "Queue";
			this.Queue.Width = 100;
			// 
			// btnAll
			// 
			this.btnAll.Location = new System.Drawing.Point(8, 80);
			this.btnAll.Name = "btnAll";
			this.btnAll.Size = new System.Drawing.Size(64, 23);
			this.btnAll.TabIndex = 2;
			this.btnAll.Text = "All";
			this.tTip.SetToolTip(this.btnAll, "Get All SubQueues Matching the Queue");
			this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
			// 
			// txtSubQ
			// 
			this.txtSubQ.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtSubQ.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtSubQ.Location = new System.Drawing.Point(40, 48);
			this.txtSubQ.MaxLength = 2;
			this.txtSubQ.Name = "txtSubQ";
			this.txtSubQ.Size = new System.Drawing.Size(32, 22);
			this.txtSubQ.TabIndex = 1;
			this.txtSubQ.Text = "";
			this.txtSubQ.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSubQ_KeyPress);
			// 
			// lblQueue
			// 
			this.lblQueue.Location = new System.Drawing.Point(8, 24);
			this.lblQueue.Name = "lblQueue";
			this.lblQueue.Size = new System.Drawing.Size(104, 23);
			this.lblQueue.TabIndex = 1;
			this.lblQueue.Text = "Queue/SubQueue";
			// 
			// txtQ
			// 
			this.txtQ.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtQ.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtQ.Location = new System.Drawing.Point(8, 48);
			this.txtQ.MaxLength = 2;
			this.txtQ.Name = "txtQ";
			this.txtQ.Size = new System.Drawing.Size(32, 22);
			this.txtQ.TabIndex = 0;
			this.txtQ.Text = "";
			this.txtQ.TextChanged += new System.EventHandler(this.txtQ_TextChanged);
			// 
			// gbUser
			// 
			this.gbUser.BackColor = System.Drawing.Color.Transparent;
			this.gbUser.Controls.Add(this.cboUser);
			this.gbUser.Controls.Add(this.btnRemoveUser);
			this.gbUser.Controls.Add(this.btnAdd);
			this.gbUser.Controls.Add(this.lstUsers);
			this.gbUser.Enabled = false;
			this.gbUser.Location = new System.Drawing.Point(8, 72);
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
			this.gbModel.BackColor = System.Drawing.Color.Transparent;
			this.gbModel.Controls.Add(this.cboModel);
			this.gbModel.Enabled = false;
			this.gbModel.Location = new System.Drawing.Point(224, 264);
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
			// gbAction
			// 
			this.gbAction.BackColor = System.Drawing.Color.Transparent;
			this.gbAction.Controls.Add(this.radModel);
			this.gbAction.Controls.Add(this.radRemove);
			this.gbAction.Controls.Add(this.radAdd);
			this.gbAction.Location = new System.Drawing.Point(8, 8);
			this.gbAction.Name = "gbAction";
			this.gbAction.Size = new System.Drawing.Size(408, 56);
			this.gbAction.TabIndex = 1;
			this.gbAction.TabStop = false;
			this.gbAction.Text = "Action";
			// 
			// radModel
			// 
			this.radModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.radModel.ForeColor = System.Drawing.Color.Navy;
			this.radModel.Location = new System.Drawing.Point(288, 24);
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
			this.radRemove.Location = new System.Drawing.Point(144, 24);
			this.radRemove.Name = "radRemove";
			this.radRemove.Size = new System.Drawing.Size(112, 24);
			this.radRemove.TabIndex = 1;
			this.radRemove.Text = "Remove";
			this.radRemove.CheckedChanged += new System.EventHandler(this.radRemove_CheckedChanged);
			// 
			// radAdd
			// 
			this.radAdd.BackColor = System.Drawing.Color.Transparent;
			this.radAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.radAdd.ForeColor = System.Drawing.Color.Green;
			this.radAdd.Location = new System.Drawing.Point(8, 24);
			this.radAdd.Name = "radAdd";
			this.radAdd.TabIndex = 0;
			this.radAdd.Text = "Add";
			this.radAdd.CheckedChanged += new System.EventHandler(this.radAdd_CheckedChanged);
			// 
			// btnProcess
			// 
			this.btnProcess.Location = new System.Drawing.Point(136, 336);
			this.btnProcess.Name = "btnProcess";
			this.btnProcess.TabIndex = 5;
			this.btnProcess.Text = "Process";
			this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(224, 336);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 6;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// frmCompassQAccess
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(422, 371);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnProcess);
			this.Controls.Add(this.gbAction);
			this.Controls.Add(this.gbModel);
			this.Controls.Add(this.gbUser);
			this.Controls.Add(this.gbQ);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(432, 400);
			this.MinimumSize = new System.Drawing.Size(432, 400);
			this.Name = "frmCompassQAccess";
			this.Text = "Compass Queue Access";
			this.Load += new System.EventHandler(this.frmCompassQAccess_Load);
			this.gbQ.ResumeLayout(false);
			this.gbUser.ResumeLayout(false);
			this.gbModel.ResumeLayout(false);
			this.gbAction.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void frmCompassQAccess_Load(object sender, System.EventArgs e)
		{
			string RUserID = "";
			string RName = "";
			string RQUserType = "";
			
			UserIDArr = new ArrayList();
//			NameArr = new ArrayList();
			
			tbUserName = new Hashtable();
			tbUserType = new Hashtable();
			//Gather UserIDs
			conBSYS.Open(); 
			System.Data.SqlClient.SqlDataReader  DR = SelUsers.ExecuteReader();

			if (DR.HasRows )
			{
				while (DR.Read())
				{
					RUserID = DR.GetString(0);
					RName = DR.GetString(1) + " " + DR.GetString(2);
					RQUserType = DR.GetString(3);
					cboUser.Items.Add(RUserID + "     " + RName);
					cboModel.Items.Add(RUserID + "     " + RName);
					UserIDArr.Add(RUserID);
					tbUserName.Add(RUserID,RName);
					tbUserType.Add(RUserID,RQUserType);
				}
			
			}
			conBSYS.Close();

			//Gather Queues and SubQueues
			conBSYS.Open();
			daBSYS.SelectCommand = SelQ;
			System.Data.SqlClient.SqlDataReader  DR2 = SelQ.ExecuteReader();

			QueueArr = new ArrayList();
			if (DR2.HasRows )
			{
				string Que = "";
				while (DR2.Read())
				{
					Que = DR2.GetString(0);
					if (Que.Length > 2)
					{
						QueueArr.Add(Que);
					}
				}
			}
			conBSYS.Close();
			

		}

		private void radAdd_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radAdd.Checked == true)
			{
				gbUser.Enabled = true;
				gbQ.Enabled = true;
				gbModel.Enabled = false;
			}
			gbAction.Refresh();
		}

		private void radRemove_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radRemove.Checked == true)
			{
				gbUser.Enabled = true;
				gbQ.Enabled = true;
				gbModel.Enabled = false;
			}
			gbAction.Refresh();
		}

		private void radModel_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radModel.Checked == true)
			{
				gbUser.Enabled = true;
				gbQ.Enabled = false;
				gbModel.Enabled = true;
				if (lstUsers.Items.Count > 1)
				{
					lstUsers.Items.Clear();
				}
			}
			gbAction.Refresh();
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
			if (lstUsers.Items.Count > 0 && radModel.Checked == true)
			{
				MessageBox.Show("You may only model one User ID at a time.");
				return;
			}
			ListViewItem I = new ListViewItem();
			I.Text = cboUser.Text.Substring(0,7);
			I.SubItems.Add(cboUser.Text.Substring(12));
			I.SubItems.Add((string)tbUserType[I.Text]);
			bool Found;
			Found = false;
			foreach (ListViewItem R in lstUsers.Items )
			{
				if (R.Text == cboUser.Text.Substring(0,7))
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

		private void btnAll_Click(object sender, System.EventArgs e)
		{
			if (txtQ.Text.Length == 2 )
			{
				foreach (string item in QueueArr)
				{
					if (item.Length >= 2)
					{
						if (item.Substring(0,2) == txtQ.Text )
						{
							bool found;
							found = false;
							foreach (ListViewItem I in lstQ.Items)
							{
								if (I.Text == item)
								{
									found = true;
								}
							}
							if (found == false)
							{
								lstQ.Items.Add(item);
							}
						}
					}
				}
				txtQ.Text = "";
				txtSubQ.Text = "";
				txtQ.Focus();
			}
			else
			{
				MessageBox.Show("The Queue and Sub-Queue must be two characters each.");
			}
		}

		private void btnAddQ_Click(object sender, System.EventArgs e)
		{
			if (txtQ.Text.Length != 2 || txtSubQ.Text.Length != 2)
			{
				MessageBox.Show("The Queue and Sub-Queue must be two characters each.");
			}
			else
			{
				foreach (ListViewItem I in lstQ.Items)
				{
					if (I.Text == txtQ.Text + txtSubQ.Text)
					{
						return;
					}
				}
				lstQ.Items.Add(txtQ.Text + txtSubQ.Text);
				txtQ.Text = "";
				txtSubQ.Text = "";
				txtQ.Focus();
			}

		}

		private void btnRemoveQ_Click(object sender, System.EventArgs e)
		{
			RemoveQueue();
		}

		private void lstQ_DoubleClick(object sender, System.EventArgs e)
		{
			RemoveQueue();
		}

		private void RemoveQueue()
		{
			if (lstQ.SelectedIndices.Count > 0)
			{
				lstQ.Items.RemoveAt(lstQ.SelectedIndices[0]);
			}
		}


		private void cboUser_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == "\r".ToCharArray(0,1)[0])
			{
				addUser();
				e.Handled = true;
			}
		}

		private void btnProcess_Click(object sender, System.EventArgs e)
		{
			if (btnProcess.Focused == false)
			{
				return;
			}
			//			btnProcess.Enabled = false;
			//			btnProcess.Enabled = true;
			cboUser.Focus();
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
			if (lstQ.Items.Count < 1 && radModel.Checked == false)
			{
				MessageBox.Show("You must select a Queue to processes.");
				txtQ.Focus();
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
				foreach (ListViewItem I in lstQ.Items)
				{	
					I.SubItems.Add("Add");
					I.SubItems[0].ForeColor = Color.Green;
				}
			}
			else if (radRemove.Checked == true)
			{
				foreach (ListViewItem I in lstQ.Items)
				{	
					I.SubItems.Add("Remove");
					I.SubItems[0].ForeColor = Color.Red;
				}
			}
			else if (radModel.Checked == true)
			{
				ArrayList ModelQs = new ArrayList();
				ArrayList TargetQs = new ArrayList();
				conBSYS.Open(); 
				//Get list of Queues to model after
				SelModelQ.CommandText = "SELECT Queue FROM SYSA_REF_UserID_COMPASSQueue WHERE (UserID = '" + cboModel.Text.Substring(0,7) + "')";
				System.Data.SqlClient.SqlDataReader  DR = SelModelQ.ExecuteReader();
				if (DR.HasRows )
				{
					while (DR.Read())
					{
						ModelQs.Add(DR.GetString(0));
					}
				}
				DR.Close();
				//Get list of Target UserID's Queues
				SelModelQ.CommandText = "SELECT Queue FROM SYSA_REF_UserID_COMPASSQueue WHERE (UserID = '" + lstUsers.Items[0].Text + "')";
				System.Data.SqlClient.SqlDataReader  DR2 = SelModelQ.ExecuteReader();
				if (DR2.HasRows )
				{
					while (DR2.Read())
					{
						TargetQs.Add(DR2.GetString(0));
					}
				}
				DR2.Close();
				conBSYS.Close();
				for (int x = 0; x < ModelQs.Count; x++)
				{
					if (TargetQs.IndexOf(ModelQs[x]) >= 0)
					{
						//Already has Queue
						ListViewItem I = new ListViewItem();
						I.Text = ModelQs[x].ToString();
						I.SubItems.Add("Keep");
						//I.SubItems[0].ForeColor = Color.Green;
						lstQ.Items.Add(I);
					}
					else
					{
						//Add Queue
						ListViewItem I = new ListViewItem();
						I.Text = ModelQs[x].ToString();
						I.SubItems.Add("Add");
						I.SubItems[0].ForeColor = Color.Green;
						lstQ.Items.Add(I);
					}
				}
				for (int x = 0; x < TargetQs.Count; x++)
				{
					if (ModelQs.IndexOf(TargetQs[x]) < 0)
					{
						//Queue is not in the Model list
						//Remove Queue
						ListViewItem I = new ListViewItem();
						I.Text = TargetQs[x].ToString();
						I.SubItems.Add("Remove");
						I.SubItems[0].ForeColor = Color.Red;
						lstQ.Items.Add(I);
					}
				}
			}

			/////////////////////////////////////////
			//convert to listview arrays to be passed to approval form
			ListViewItem[] Arr;
			ListViewItem[] Arr2;
			Arr = new ListViewItem[lstUsers.Items.Count];
			Arr2 = new ListViewItem[lstQ.Items.Count];
			int cnt = new int();
			cnt = 0;
			
			foreach (ListViewItem I in lstQ.Items)
			{
				ListViewItem I2 = new ListViewItem();
				I2 = (ListViewItem)I.Clone();
				Arr2[cnt] = I2;
				cnt++;
			}
			cnt = 0;
			foreach (ListViewItem I in lstUsers.Items)
			{
				ListViewItem I2 = new ListViewItem();
				I2 = (ListViewItem)I.Clone();
				Arr[cnt] = I2;
				cnt++;
			}
			//show approval form
			App = new frmApproval(Arr,Arr2,tbUserType,TestMode,UserID,Pass,cboModel.Text);
			App.VisibleChanged += new EventHandler(ApprovalEvent);
			App.MdiParent = this.ParentForm; 
			this.Visible = false;
			App.Show();
		}

		private void ApprovalEvent(Object sender, EventArgs e)
		{	
			if (App.Visible == false)
			{
				if (App.CanceledPressed())
				{
					this.Visible = true;	
					App.Close();
					App.Dispose();
				}
				else
				{
					//Approval done refresh form
					ClearForm();
					this.Visible = true;
					App.Close();
					App.Dispose();
				}
			}
		}

		private void ClearForm()
		{
			lstUsers.Items.Clear();
			lstQ.Items.Clear();
			cboUser.SelectedIndex = -1;
			cboModel.SelectedIndex = -1;
			txtQ.Text = "";
			txtSubQ.Text = "";
			this.Visible = true;
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void txtQ_TextChanged(object sender, System.EventArgs e)
		{
			if (txtQ.Text.Length == 2)
			{
				txtSubQ.Focus();
			}
		}

		private void txtSubQ_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == "\r".ToCharArray(0,1)[0])
			{
				btnAddQ.PerformClick();
				e.Handled = true;
			}
		}

		
	}


}

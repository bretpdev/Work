using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Q;

namespace CompassQAccess
{
	/// <summary>
	/// Summary description for frmApproval.
	/// </summary>
	public class frmApproval : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private bool Canceled;
		private System.Windows.Forms.ListView lstUsers;
		private System.Windows.Forms.ListView lstQ;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private bool TestMode;
		private string UserID;
		private string Pass;

		private Hashtable tbUserType;

		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.Label lblModel;
		private System.Data.SqlClient.SqlConnection conTestBSYS;
		private System.Data.SqlClient.SqlConnection conBSYS;
		private System.Data.SqlClient.SqlDataAdapter daBSYS;
		private System.Data.SqlClient.SqlCommand InsertQ;
		private System.Data.SqlClient.SqlCommand DeleteQ;


		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmApproval(ListViewItem[] Users, ListViewItem[] Queues, Hashtable TtbUserType ,bool TTestMode, string TUserID, string TPass, string ModelAfter)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Canceled = true;
			conTestBSYS =  new System.Data.SqlClient.SqlConnection();
			conBSYS =  new System.Data.SqlClient.SqlConnection();
			InsertQ = new System.Data.SqlClient.SqlCommand();
			DeleteQ = new System.Data.SqlClient.SqlCommand();
			daBSYS = new System.Data.SqlClient.SqlDataAdapter();
			conTestBSYS.ConnectionString = @"workstation id=""LPP-1494"";packet size=4096;integrated security=SSPI;data source=""OPSDEV"";persist security info=False;initial catalog=BSYS";
			conBSYS.ConnectionString = @"workstation id=""LPP-1494"";packet size=4096;integrated security=SSPI;data source=""NOCHOUSE"";persist security info=False;initial catalog=BSYS";
			tbUserType = TtbUserType;
			TestMode = TTestMode;
			UserID = TUserID;
			lblModel.Text = "Model After: " + ModelAfter;
			Pass = TPass;
			lstUsers.Items.AddRange(Users);
			lstQ.Items.AddRange(Queues);
			
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmApproval));
			this.lstUsers = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.lstQ = new System.Windows.Forms.ListView();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.label1 = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblModel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lstUsers
			// 
			this.lstUsers.BackColor = System.Drawing.SystemColors.Control;
			this.lstUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					   this.columnHeader1,
																					   this.columnHeader2,
																					   this.columnHeader5});
			this.lstUsers.FullRowSelect = true;
			this.lstUsers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstUsers.Location = new System.Drawing.Point(16, 80);
			this.lstUsers.Name = "lstUsers";
			this.lstUsers.Size = new System.Drawing.Size(256, 304);
			this.lstUsers.TabIndex = 0;
			this.lstUsers.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "UserID";
			this.columnHeader1.Width = 80;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Name";
			this.columnHeader2.Width = 115;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "type";
			// 
			// lstQ
			// 
			this.lstQ.BackColor = System.Drawing.SystemColors.Control;
			this.lstQ.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																				   this.columnHeader3,
																				   this.columnHeader4});
			this.lstQ.FullRowSelect = true;
			this.lstQ.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstQ.Location = new System.Drawing.Point(288, 80);
			this.lstQ.Name = "lstQ";
			this.lstQ.Size = new System.Drawing.Size(176, 304);
			this.lstQ.TabIndex = 1;
			this.lstQ.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Queue";
			this.columnHeader3.Width = 85;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Action";
			this.columnHeader4.Width = 86;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(448, 24);
			this.label1.TabIndex = 2;
			this.label1.Text = "Are the User IDs and Queues below Correct?";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnOk
			// 
			this.btnOk.Location = new System.Drawing.Point(152, 400);
			this.btnOk.Name = "btnOk";
			this.btnOk.TabIndex = 5;
			this.btnOk.Text = "OK";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(264, 400);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lblModel
			// 
			this.lblModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblModel.Location = new System.Drawing.Point(8, 32);
			this.lblModel.Name = "lblModel";
			this.lblModel.Size = new System.Drawing.Size(456, 23);
			this.lblModel.TabIndex = 7;
			this.lblModel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// frmApproval
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(480, 430);
			this.Controls.Add(this.lblModel);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lstQ);
			this.Controls.Add(this.lstUsers);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmApproval";
			this.Text = "Compass Queue Approval";
			this.ResumeLayout(false);

		}
		#endregion

		public void btnCancel_Click(object sender, System.EventArgs e)
		{
			Canceled = true;
			this.Hide();
		}

		public void btnOk_Click(object sender, System.EventArgs e)
		{
			Canceled = false;
			//TEST///////////////////////////////////////////
			ProcessQs(true);
			//LIVE/////////////////////////////////////////////
			//MessageBox.Show("Testmode = " + TestMode.ToString() );
			if (TestMode == false)
			{
				ProcessQs(false);
			}
			this.Hide();
		}

		public void ProcessQs(bool TTestMode)
		{//TTestMode is true if you are processing for test mode else false = LIVE.
			string EmailBody = "";
			string StrTestMode = "";
			string eRecipient = "";
			if (TTestMode)
			{
				StrTestMode = "Test";
		
				//Get list of email addresses
				System.Data.SqlClient.SqlCommand SelEmailAddr = new System.Data.SqlClient.SqlCommand();
				SelEmailAddr.Connection = conTestBSYS;
				conTestBSYS.Open(); 
				SelEmailAddr.CommandText = "SELECT WinUName FROM GENR_REF_MiscEmailNotif WHERE (TypeKey = 'Compass Queue Access Error')";
				System.Data.SqlClient.SqlDataReader  DR = SelEmailAddr.ExecuteReader();
				if (DR.HasRows )
				{
					//eRecipient = DR.GetString(0);
					while (DR.Read())
					{
						eRecipient = eRecipient + "," + DR.GetString(0);
					}
				}
				DR.Close();
				conTestBSYS.Close();
				eRecipient = eRecipient.Substring(1);
			}
			else
			{
				StrTestMode = "Live";
				//MessageBox.Show("This is LIVE Abort!!!!!!!!!!");
				//Get list of email addresses
				System.Data.SqlClient.SqlCommand SelEmailAddr = new System.Data.SqlClient.SqlCommand();
				SelEmailAddr.Connection = conBSYS;
				conBSYS.Open(); 
				SelEmailAddr.CommandText = "SELECT WinUName FROM GENR_REF_MiscEmailNotif WHERE (TypeKey = 'Compass Queue Access Error')";
				System.Data.SqlClient.SqlDataReader  DR = SelEmailAddr.ExecuteReader();
				if (DR.HasRows )
				{
					//eRecipient = DR.GetString(0);
					while (DR.Read())
					{
						eRecipient = eRecipient + "," + DR.GetString(0);
					}
				}
				DR.Close();
				conBSYS.Close();
				eRecipient = eRecipient.Substring(1);
//				MessageBox.Show(eRecipient);
//				this.Close();
			}

			ReflectionInterface R;
			R = new ReflectionInterface(TTestMode);
			if (R.Login(UserID,Pass))
			{
				
				foreach (ListViewItem U in lstUsers.Items)
				{
					string UID = U.Text;
					foreach (ListViewItem I in lstQ.Items)
					{
						string Queue = I.Text.Substring(0,2);
						string SubQueue = I.Text.Substring(2,2);
						//MessageBox.Show(UID + " " + Queue + SubQueue);
						if (I.SubItems[1].Text == "Add")
						{
							//Add////////////////////////////
							R.FastPath("TX3Z/ITX5Z");
							R.PutText(8,38,Queue);
							R.PutText(10,38,SubQueue,ReflectionInterface.Key.Enter);
							if (R.Check4Text(1,72,"TXX63"))
							{
								R.FastPath("TX3Z/ATX64");
								if (R.Check4Text(23,2,"01010"))
								{
									this.MdiParent.Activate();
									MessageBox.Show("You do not have access to ATX64!");
									this.Close();
								}
								R.PutText(8,46,Queue);
								R.PutText(10,46,SubQueue);
								R.PutText(12,46,UID,ReflectionInterface.Key.Enter);
								if (R.Check4Text(23,2,"90018"))
								{
                                    if (StrTestMode != "Test")
                                    {
                                        this.MdiParent.Activate();
                                        MessageBox.Show("The User ID " + UID + " entered is either not valid or does not have access to the Compass " + StrTestMode + " Region");
                                        break;
                                    }
								}
								else if (R.Check4Text(23,2,"80011") || R.Check4Text(23,2,"80039"))
								{
									this.MdiParent.Activate();
									MessageBox.Show("The Queue " + Queue + SubQueue + " does not exist in the Compass " + StrTestMode + " Region.");
								}
								else if (R.Check4Text(23,2,"02716"))
								{
									this.MdiParent.Activate();
									MessageBox.Show("The Queue " + Queue + SubQueue + " is not assignable to a user to process.");
								}
								else if (R.Check4Text(23,2,"01018"))
								{
//									this.MdiParent.Activate();
//									MessageBox.Show("The UserID " + UID + " already has access to Queue " + Queue + SubQueue + ".");
								}
								else if (R.Check4Text(1,72,"TXX67"))
								{
									R.PutText(10,33,U.SubItems[2].Text ,ReflectionInterface.Key.Enter); //UserType
									//Verify queue was added
									R.FastPath("TX3Z/ITX64");
									R.PutText(8,46,Queue);
									R.PutText(10,46,SubQueue);
									R.PutText(12,46,UID,ReflectionInterface.Key.Enter);
									if (R.Check4Text(1,72,"TXX67") == false)
									{
										EmailBody = EmailBody + UID + " " + Queue + SubQueue + " " + "Add" + " " + StrTestMode + "\n";
									}
									else
									{
										//Add record To DB
										try 
										{
											TestAddQToDB(UID,Queue + SubQueue);
											if (TTestMode == false)
											{
												LiveAddQToDB(UID,Queue + SubQueue);
											}
											
										}
										catch (Exception e) 
										{
											//this.MdiParent.Activate();
											//MessageBox.Show(e.Message);
										}

									}
								}
								else
								{
									//send off error email
									EmailBody = EmailBody + UID + " " + Queue + SubQueue + " " + "Add" + " " + StrTestMode + "\n";
                                    Common.SendMail(TestMode, eRecipient, Environment.UserName + "@utahsbr.edu", "Compass Queue Access Error", EmailBody, "", "", "", Common.EmailImportanceLevel.Normal, true);
                                    if (StrTestMode != "Test")
                                    {
                                        this.MdiParent.Activate();
                                        MessageBox.Show("An Error occured while trying to add a Queue. Contact System Support.");
                                        this.Close();
                                    }
								}

							}
							else
							{
								this.MdiParent.Activate();
								MessageBox.Show("The Queue '" + Queue + SubQueue + "' Does not Exists!");
							}
						}
						else if (I.SubItems[1].Text == "Remove")
						{
							//Remove///////////////////////////
							R.FastPath("TX3Z/ITX5Z");
							R.PutText(8,38,Queue);
							R.PutText(10,38,SubQueue,ReflectionInterface.Key.Enter);
							if (R.Check4Text(1,72,"TXX63"))
							{
								R.FastPath("TX3Z/DTX64");
								if (R.Check4Text(23,2,"01010"))
								{
									this.MdiParent.Activate();
									MessageBox.Show("You do not have access to DTX64!");
									this.Close();
								}
								R.PutText(8,46,Queue);
								R.PutText(10,46,SubQueue);
								R.PutText(12,46,UID,ReflectionInterface.Key.Enter);
                                R.Hit(ReflectionInterface.Key.Enter);
								if (R.Check4Text(23,2,"01019"))
								{
                                    if (StrTestMode != "Test")
                                    {
                                        this.MdiParent.Activate();
                                        MessageBox.Show("The User ID " + UID + " entered is either not valid, does not have that queue or does not have access to the Compass " + StrTestMode + " Region");
                                        break;
                                    }
								}
								else if (R.Check4Text(23,2,"80011") || R.Check4Text(23,2,"80039"))
								{
//									this.MdiParent.Activate();
//									MessageBox.Show("The Queue " + Queue + SubQueue + " does not exist in the Compass " + StrTestMode + " Region.");
								}
								else if (R.Check4Text(1,72,"TXX67"))
								{
									
									//verify queue was removed
									R.FastPath("TX3Z/ITX64");
									R.PutText(8,46,Queue);
									R.PutText(10,46,SubQueue);
									R.PutText(12,46,UID,ReflectionInterface.Key.Enter);
									if (R.Check4Text(23,2,"01019 ") == false)
									{
										EmailBody = EmailBody + UID + " " + Queue + SubQueue + " " + "Remove" + " " + StrTestMode + "\n";
									}
									else
									{
										try 
										{
										TestRemoveQFromDB(UID,Queue + SubQueue);
										if (TTestMode == false)
										{LiveRemoveQFromDB(UID,Queue + SubQueue);}
										}
										catch (Exception e) 
										{
											//this.MdiParent.Activate();
											//MessageBox.Show(e.Message);
										}
									}
								}
								else
								{
									//Send off error email
									EmailBody = EmailBody + UID + " " + Queue + SubQueue + " " + "Remove" + " " + StrTestMode + "\n";
                                    Common.SendMail(TestMode, eRecipient, Environment.UserName + "@utahsbr.edu", "Compass Queue Access Error", EmailBody, "", "", "", Common.EmailImportanceLevel.Normal, true);
                                    if (StrTestMode != "Test")
                                    {
                                        this.MdiParent.Activate();
                                        MessageBox.Show("An Error occured while trying to add a Queue. Contact System Support.");
                                        this.Close();
                                    }
								}

							}
							else
							{
								this.MdiParent.Activate();
								MessageBox.Show("The Queue '" + Queue + SubQueue + "' Does not Exists!");
							}
						}
					}
				}
				if (EmailBody != "")
				{
					this.MdiParent.Activate();
					MessageBox.Show("An Error occured while trying to add or remove a Queue. And email has been sent notifying System Support.");
                    Common.SendMail(TestMode, eRecipient, Environment.UserName + "@utahsbr.edu", "COMPASS ARC Access Update Error", EmailBody, "", "", "", Common.EmailImportanceLevel.Normal, true);
				}
					
			}
			else
			{
				this.MdiParent.Activate();
				MessageBox.Show("Login Failed! No action was taken.");
			}
			R.CloseSession();
			//this.Close();
		}

		public bool CanceledPressed()
		{
			return Canceled;
		}

		private bool LiveAddQToDB(string UID, string Q)
		{
			//Add Queue to live DB
			try
			{
				InsertQ.Connection = conBSYS;
				conBSYS.Open(); 
				InsertQ.CommandText = "INSERT INTO SYSA_REF_UserID_COMPASSQueue (UserID, Queue) VALUES ('" + UID + "', '" + Q + "')";
				InsertQ.ExecuteNonQuery();
				conBSYS.Close();
			}
			catch (Exception e) 
			{
				//MessageBox.Show(e.Message);
			}
			return true;
		}
		private bool TestAddQToDB(string UID, string Q)
		{
			//Add Queue to Test DB
			try
			{
				InsertQ.Connection = conTestBSYS;
				conTestBSYS.Open(); 
				InsertQ.CommandText = "INSERT INTO SYSA_REF_UserID_COMPASSQueue (UserID, Queue) VALUES ('" + UID + "', '" + Q + "')";
				InsertQ.ExecuteNonQuery();
				conTestBSYS.Close();
			}
			catch (Exception e) 
			{
				//MessageBox.Show(e.Message);
			}
			return true;
			
		}
		private bool LiveRemoveQFromDB(string UID, string Q)
		{
			//Remove Queue from live DB
			try
			{
				InsertQ.Connection = conBSYS;
				conBSYS.Open(); 
				InsertQ.CommandText = "DELETE FROM SYSA_REF_UserID_COMPASSQueue WHERE (UserID = '" + UID + "') AND (Queue = '" + Q + "')";
				InsertQ.ExecuteNonQuery();
				conBSYS.Close();
			}
			catch (Exception e) 
			{
				//MessageBox.Show(e.Message);
			}
			return true;
		}
		private bool TestRemoveQFromDB(string UID, string Q)
		{
			//Remove Queue from test DB
			try
			{
				InsertQ.Connection = conTestBSYS;
				conTestBSYS.Open(); 
				InsertQ.CommandText = "DELETE FROM SYSA_REF_UserID_COMPASSQueue WHERE (UserID = '" + UID + "') AND (Queue = '" + Q + "')";
				InsertQ.ExecuteNonQuery();
				conTestBSYS.Close();
			}
			catch (Exception e) 
			{
				//MessageBox.Show(e.Message);
			}
			return true;
		}
	}
}

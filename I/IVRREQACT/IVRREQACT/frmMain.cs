using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Text;
using Q;
using System.Threading;

namespace IVRRequestProcessing
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.TextBox tbUserID;
		private System.Windows.Forms.Label label;
		private System.Windows.Forms.Label Label1;
		private System.Windows.Forms.Button btnOK;
		private System.Threading.Thread ProcThread;
		private System.Data.SqlClient.SqlConnection _ulsConn;
		private System.Data.SqlClient.SqlConnection _ulsUpdateConn;
		private System.Data.SqlClient.SqlConnection _ulsErrorConn;
		private System.Windows.Forms.NotifyIcon NI;
		private System.Data.SqlClient.SqlConnection BSYSConn;
		private System.Threading.Thread MonitorThread;
		private string Status;
		private bool TestMode;

		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.tbUserID = new System.Windows.Forms.TextBox();
			this.label = new System.Windows.Forms.Label();
			this.Label1 = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.NI = new System.Windows.Forms.NotifyIcon(this.components);
			this.SuspendLayout();
			// 
			// tbPassword
			// 
			this.tbPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbPassword.Location = new System.Drawing.Point(120, 48);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.PasswordChar = '*';
			this.tbPassword.Size = new System.Drawing.Size(112, 24);
			this.tbPassword.TabIndex = 8;
			// 
			// tbUserID
			// 
			this.tbUserID.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbUserID.Location = new System.Drawing.Point(120, 16);
			this.tbUserID.Name = "tbUserID";
			this.tbUserID.Size = new System.Drawing.Size(112, 24);
			this.tbUserID.TabIndex = 6;
			// 
			// label
			// 
			this.label.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label.Location = new System.Drawing.Point(24, 56);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(96, 16);
			this.label.TabIndex = 10;
			this.label.Text = "Password:";
			// 
			// Label1
			// 
			this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label1.Location = new System.Drawing.Point(24, 24);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(96, 16);
			this.Label1.TabIndex = 7;
			this.Label1.Text = "User ID:";
			// 
			// btnOK
			// 
			this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnOK.Location = new System.Drawing.Point(80, 88);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(88, 24);
			this.btnOK.TabIndex = 9;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// NI
			// 
			this.NI.Icon = ((System.Drawing.Icon)(resources.GetObject("NI.Icon")));
			this.NI.Text = "IVR Request Processing";
			this.NI.Visible = true;
			// 
			// frmMain
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(13, 29);
			this.ClientSize = new System.Drawing.Size(250, 134);
			this.Controls.Add(this.tbPassword);
			this.Controls.Add(this.tbUserID);
			this.Controls.Add(this.label);
			this.Controls.Add(this.Label1);
			this.Controls.Add(this.btnOK);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 19F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "IVR Request Processing";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.Run(new frmMain());
		}

		private void frmMain_Load(object sender, System.EventArgs e)
		{
			TestMode = Environment.GetCommandLineArgs().Contains("test");

			if (TestMode)
			{
				_ulsConn = new System.Data.SqlClient.SqlConnection(@"Data Source=OPSDEV;Initial Catalog=ULS;Integrated Security=SSPI;");
				_ulsUpdateConn = new System.Data.SqlClient.SqlConnection(@"Data Source=OPSDEV;Initial Catalog=ULS;Integrated Security=SSPI;");
				_ulsErrorConn = new System.Data.SqlClient.SqlConnection(@"Data Source=OPSDEV;Initial Catalog=ULS;Integrated Security=SSPI;");
				BSYSConn = new System.Data.SqlClient.SqlConnection(@"Data Source=""BART\BART"";Initial Catalog=BSYS;Integrated Security=SSPI;");
			}
			else
			{
				_ulsConn = new System.Data.SqlClient.SqlConnection(@"Data Source=UHEAASQLDB;Initial Catalog=ULS;Integrated Security=SSPI;");
				_ulsUpdateConn = new System.Data.SqlClient.SqlConnection(@"Data Source=UHEAASQLDB;Initial Catalog=ULS;Integrated Security=SSPI;");
				_ulsErrorConn = new System.Data.SqlClient.SqlConnection(@"Data Source=UHEAASQLDB;Initial Catalog=ULS;Integrated Security=SSPI;");
				BSYSConn = new System.Data.SqlClient.SqlConnection(@"Data Source=NOCHOUSE;Initial Catalog=BSYS;Integrated Security=SSPI;");
			}
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{

			if (tbUserID.TextLength == 0 || tbPassword.TextLength == 0)
			{
				//something was not entered
				MessageBox.Show("You must enter a user id and password.", "No User ID or Password", MessageBoxButtons.OK);
			}
			else
			{
				//if everything appears to be OK
				this.Hide(); //hide form
				ProcThread = new System.Threading.Thread(new System.Threading.ThreadStart(Processing));
				ProcThread.Start();
				MonitorThread = new System.Threading.Thread(new System.Threading.ThreadStart(OverSeer));
				MonitorThread.Start();
			}
		}

		private void OverSeer()
		{
			//This thread checks the status every 15 minutes to see if the script is frozen
			string OldStatus = "";
			while (true)
			{
				if (Status == OldStatus && Status != "Sleep")
				{
					//Frozen
					//Send Email and die
					ReflectionInterface RI;
					RI = new ReflectionInterface(TestMode);
					System.Data.SqlClient.SqlCommand Comm;
					System.Data.SqlClient.SqlDataReader Reader;
					string To = "";
					Comm = new System.Data.SqlClient.SqlCommand("SELECT WinUName + '@utahsbr.edu' AS EmailAddr FROM GENR_REF_MiscEmailNotif WHERE TypeKey = 'IVRRequestProcessor'", BSYSConn);
					Comm.Connection.Open();
					Reader = Comm.ExecuteReader();
					while (Reader.Read())
					{
						if (To == "") To = Reader["EmailAddr"].ToString(); else To = To + "," + Reader["EmailAddr"].ToString();
					}
					Reader.Close();
					Comm.Connection.Close();
					//send email if the application can't access the system after 4 attempts
					Common.SendMail(TestMode, To, "IVRRequestProcessorMonitor", "IVR Requested Activity Process Monitor", "IVR Processing is frozen in status " + Status + ".", "", "", "", Common.EmailImportanceLevel.High, TestMode);
					Environment.Exit(0);
				}
				OldStatus = Status;
				System.Threading.Thread.Sleep(1500000); //Sleep 25 minutes

			}
		}

		//does thread processing
		private void Processing()
		{
			//declare vars
			Status = "Start";
			bool FirstTime = true; //**
			ReflectionInterface RI;
			System.TimeSpan TS20Min = new TimeSpan(0, 20, 0); //**
			int BadLogins;
			System.Data.SqlClient.SqlCommand Comm;
			System.Data.SqlClient.SqlDataReader Reader;
			string To = "";
			System.DateTime TenOClock = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 0, 0);
			System.DateTime TwoOClock = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 0, 0);

			//endless loop for processing
			while (true)
			{
				BadLogins = 0;
				RI = new ReflectionInterface(TestMode);
				Thread.Sleep(2000);
				if (FirstTime)
				{
					FirstTime = false;
					Status = "Login" + BadLogins;
					//first time processing, user just logged in
					if (RI.Login(tbUserID.Text, tbPassword.Text) == false)
					{
						//if login wasn't successful then prompt the user, get the login info again, and try again
						MessageBox.Show("The application wasn't able to login to the system.  This could happen for several reasons, but the most likely reasons are: \n\n  -the user id is already in use \n\n   or\n\n  -the user id or password were inccorrect\n\n Please investigate and re-enter the login information.", "Unable To Access System", MessageBoxButtons.OK);
						Environment.Exit(0);  //end application
					}
				}
				else
				{
					//subsequent runs after first run 
					Status = "Login" + BadLogins;
					while (RI.Login(tbUserID.Text, tbPassword.Text) == false)
					{
						RI.CloseSession(); //exit session if log in didn't work
						if (BadLogins == 4)
						{
							//get email recipients from BSYS
							Comm = new System.Data.SqlClient.SqlCommand("SELECT WinUName + '@utahsbr.edu' AS EmailAddr FROM GENR_REF_MiscEmailNotif WHERE TypeKey = 'IVRRequestProcessor'", BSYSConn);
							Comm.Connection.Open();
							Reader = Comm.ExecuteReader();
							while (Reader.Read())
							{
								if (To == "") To = Reader["EmailAddr"].ToString(); else To = To + "," + Reader["EmailAddr"].ToString();
							}
							Reader.Close();
							Comm.Connection.Close();
							//send email if the application can't access the system after 4 attempts
							Common.SendMail(TestMode, To, "IVRRequestProcessor", "Process IVR Requested Activity", "IVR Processing cannot log in.", "", "", "", Common.EmailImportanceLevel.High, TestMode);
							Environment.Exit(0);
						}
						BadLogins++;
						Status = "Sleep";
						System.Threading.Thread.Sleep(TS20Min); //sleep for twenty minutes and try again
						RI = new ReflectionInterface(TestMode); //create new session for logging in
					}
				}
				//process DB entries
				DBProcessing(ref RI);
				CheckByPhoneProc(ref RI); //do check by phone thing
				CreateErrorReport(); //create and print error report
				RI.CloseSession(); //log out of session
				//sleep until figured time
				if (System.DateTime.Now < TenOClock)
				{
					Status = "Sleep";
					System.Threading.Thread.Sleep(TenOClock.Subtract(DateTime.Now)); //sleep until 10:00 am
				}
				else if ((System.DateTime.Now > TenOClock) && (System.DateTime.Now < TwoOClock))
				{
					Status = "Sleep";
					System.Threading.Thread.Sleep(TwoOClock.Subtract(DateTime.Now)); //sleep until 2:00 pm
				}
				else
				{
					Environment.Exit(0); //end application
				}
			}
		}

		//create and print error report
		private void CreateErrorReport()
		{
			Status = "CreateErrorReport";
			string To = "";
			System.Data.SqlClient.SqlDataReader Reader;
			System.Data.DataSet DS = new DataSet();
			//System.Data.SqlClient.SqlDataAdapter DA = new System.Data.SqlClient.SqlDataAdapter("SELECT AccountNumber, Request FROM RequestProcessingErrors",_ulsConn);
			System.Data.SqlClient.SqlDataAdapter DA = new System.Data.SqlClient.SqlDataAdapter("EXEC spIvrGetRequestProcessingErrors", _ulsConn);
			ErrorReport Rpt = new ErrorReport();
			System.Data.SqlClient.SqlCommand Comm;
			DA.Fill(DS, "ErrorData");
			//only create report and delete rows if there is something found
			if (DS.Tables["ErrorData"].Rows.Count > 0)
			{
				Rpt.SummaryInfo.ReportTitle = "IVR Requests Not Processed";
				Rpt.SetDataSource(DS.Tables["ErrorData"]);
				//export to disk and email as attachement
				Rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.WordForWindows, @"T:\IVRRpt.doc");
				Comm = new System.Data.SqlClient.SqlCommand("SELECT WinUName + '@utahsbr.edu' AS EmailAddr FROM GENR_REF_MiscEmailNotif WHERE TypeKey = 'IVRRequestProcessor'", BSYSConn);
				Comm.Connection.Open();
				Reader = Comm.ExecuteReader();
				while (Reader.Read())
				{
					if (To == "") To = Reader["EmailAddr"].ToString(); else To = To + "," + Reader["EmailAddr"].ToString();
				}
				Reader.Close();
				Comm.Connection.Close();
				//send email if the application can't access the system after 4 attempts
				Common.SendMail(TestMode, To, "IVRRequestProcessor", "IVR Requests Not Processed", "Attached is the IVR Requests Not Processed report.", "", "", @"T:\IVRRpt.doc", Common.EmailImportanceLevel.High, TestMode);
				//clean up
				if (File.Exists(@"T:\IVRRpt.doc"))
				{
					File.Delete(@"T:\IVRRpt.doc");
				}

				//error delete records
				//Comm = new System.Data.SqlClient.SqlCommand("DELETE FROM RequestProcessingErrors WHERE RecNum > 0",_ulsConn);
				Comm = new System.Data.SqlClient.SqlCommand("EXEC spIvrDeleteRequestProcessingErrors", _ulsConn);
				Comm.Connection.Open();
				Comm.ExecuteNonQuery();
				Comm.Connection.Close();
			}
		}

		//does processing from DB and system
		private void DBProcessing(ref ReflectionInterface RI)
		{
			Status = "DBProcessing";
			System.Data.SqlClient.SqlDataReader Reader;
			//System.Data.SqlClient.SqlCommand Comm = new System.Data.SqlClient.SqlCommand("SELECT RecNum, AccountNumber, Request FROM IVRRequestTracking WHERE ProcessedDate IS NULL",_ulsConn);
			System.Data.SqlClient.SqlCommand Comm = new System.Data.SqlClient.SqlCommand("EXEC spIvrGetUnprocessedRequests", _ulsConn);
			System.Data.SqlClient.SqlCommand Comm2 = new System.Data.SqlClient.SqlCommand();
			Comm.Connection.Open();
			string RecNum;
			string AccNum;
			string Request;
			Reader = Comm.ExecuteReader();
			//endless loop, the loop will end when the function is exited
			while (true)
			{
				if (Reader.Read() == false)
				{
					Reader.Close();
					Comm.Connection.Close();
					return; //exit sub and loop if nothing more is found in the table to process
				}
				else
				{
					RecNum = Reader["RecNum"].ToString();
					AccNum = Reader["AccountNumber"].ToString();
					Request = Reader["Request"].ToString();
					Status = "DBProcessing " + RecNum;
					//decide what leg to complete
					if (Request.Length == 5)
					{
						//add ARC
						if (TD22AllLoans(AccNum, Request, ref RI, "REQUEST PROCESSED BY IVR"))
						{
							Comm2.Connection = _ulsUpdateConn;
							_ulsUpdateConn.Open();
							Comm2.CommandText = string.Format("EXEC spIvrUpdateRequestProcessedStatus {0}", RecNum);
							Comm2.ExecuteNonQuery();
							_ulsUpdateConn.Close();
						}
					}
					else
					{
						//Error
						WriteError(AccNum, Request, ref RI);
					}
				}
			}
		}

		//This function adds comments to TD22 and marks all loans
		private bool TD22AllLoans(string accountNumber, string ARC, ref ReflectionInterface RI, string Comments)
		{
			RI.FastPath("TX3Z/ATD22*");
			RI.PutText(8, 37, accountNumber.SafeSubstring(0, 2));
			RI.PutText(8, 40, accountNumber.SafeSubstring(2, 4));
			RI.PutText(8, 45, accountNumber.SafeSubstring(6, 4));

			RI.Hit(ReflectionInterface.Key.Enter);
			if (RI.Check4Text(1, 72, "TDX23") == false)
			{
				WriteError(accountNumber, ARC, ref RI);
				return false;
			}
			//look for ARC
			if (FindingARCOnTD22(ARC, ref RI) == false)
			{
				WriteError(accountNumber, ARC, ref RI);
				return false;
			}
			RI.Hit(ReflectionInterface.Key.Enter);

			if (RI.Check4Text(23, 2, "50108"))
			{
				WriteError(accountNumber, ARC, ref RI);
				return false;
			}
			//select all loans
			RI.PutText(11, 3, "X", false);
			while (RI.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY") == false)
			{
				RI.PutText(11, 3, "XXXXXXXX", false);
				RI.Hit(ReflectionInterface.Key.F8);
			}
			RI.PutText(21, 2, Comments, ReflectionInterface.Key.Enter, true);

			//check if comment took
			if (RI.Check4Text(23, 2, "02860 PROCESSING FOR SELECTED ACTION CODES HAS BEEN COMPLETED") == false)
			{
				WriteError(accountNumber, ARC, ref RI);
				return false;
			}
			return true;
		}

		//This Function Searches for a Queue on TD22 if it finds it, the function selects it and returns true, else it returns false.
		private bool FindingARCOnTD22(string ARC, ref ReflectionInterface RI)
		{
			int row = 8;
			while (!RI.Check4Text(23, 2, "90007 NO MORE DATA TO DISPLAY"))
			{
				if (RI.Check4Text(row, 7, " " + ARC + " "))
				{
					RI.PutText(row, 3, "01", true);
					return true;
				}
				else if (RI.Check4Text(row, 47, " " + ARC + " "))
				{
					RI.PutText(row, 43, "01", true);
					return true;
				}
				row++;
				if (row > 23)
				{
					RI.Hit(ReflectionInterface.Key.F8);
					row = 8;
				}
			}
			return false;
		}

		//writes out processing errors to DB
		private void WriteError(string accountNumber, string ARC, ref ReflectionInterface RI)
		{
			//System.Data.SqlClient.SqlConnection IVRConn2;
			//if (TestMode) 
			//{
			//    IVRConn2 = new System.Data.SqlClient.SqlConnection(@"Data Source=""BART\BART"";Initial Catalog=IVR;Integrated Security=SSPI;");
			//}
			//else 
			//{
			//    IVRConn2 = new System.Data.SqlClient.SqlConnection(@"Data Source=NOCHOUSE;Initial Catalog=IVR;Integrated Security=SSPI;");
			//}
			string desc = "";
			if (ARC != "")
			{
				RI.FastPath("TX3Z/ITD00;;;;;;");
				RI.PutText(6, 49, ARC);
				RI.Hit(ReflectionInterface.Key.Enter);
				if (RI.Check4Text(1, 72, "TDX03"))
				{
					desc = RI.GetText(19, 15, 50);
				}
				else
				{
					desc = "Unknown";
				}
			}
			_ulsErrorConn.Open();
			System.Data.SqlClient.SqlCommand Comm2 = new System.Data.SqlClient.SqlCommand(string.Format("EXEC spIvrAddRequestProcessingErrors '{0}', '{1}', '{2}'", accountNumber, ARC, desc), _ulsErrorConn);
			Comm2.ExecuteNonQuery();
			_ulsErrorConn.Close();
		}

		private void CheckByPhoneProc(ref ReflectionInterface RI)
		{
			Status = "CheckByPhoneProc";
			string RecNum;
			System.IO.FileStream FileHandle;
			System.IO.StreamWriter FileWriter;
			System.Data.SqlClient.SqlCommand Comm = new System.Data.SqlClient.SqlCommand();
			System.Data.SqlClient.SqlCommand Comm2 = new System.Data.SqlClient.SqlCommand();
			Comm.Connection = _ulsConn;
			System.Data.SqlClient.SqlDataReader Reader;
			bool HasData = false;
			_ulsConn.Open();
			//**1** **3**
			//Comm.CommandText = "SELECT A.RecNum,A.AccountNumber,A.BankAccountNum,A.AccountType,A.RoutingNum,CONVERT(VARCHAR(20),A.Amount,1) as  Amount,CASE B.MiddleName WHEN '' THEN B.FirstName + ' ' + SUBSTRING(B.MiddleName,0,1) + ' ' + B.LastName ELSE B.FirstName + ' ' + B.LastName END AS Name, B.DateOfBirth ,CONVERT(varchar(12),A.AuthDate,101) as AuthDate FROM CheckByPhone A JOIN Borrowers B ON A.AccountNumber = B.AccountNumber WHERE A.ProcessedDate IS NULL";
			Comm.CommandText = "EXEC spIvrGetUnprocessedPayments";
			Reader = Comm.ExecuteReader();
			FileWriter = new StreamWriter("T:\\IVRTextFile.txt");
			FileWriter.WriteLine("KeyLine,Name,Address1,Address2,City,State,ZIP,Country,AccountNumber,DateOralAuth,PayAmount,DebitDate,FGN_ST,COST_CENTER_CODE");
			while (Reader.Read())
			{
				CheckByPhoneData data = new CheckByPhoneData();
				//note record so it can be deleted after it has been processed
				RecNum = Reader["RecNum"].ToString();
				Status = "CheckByPhoneProc " + RecNum;
				//go to TX1J for SSN
				RI.FastPath("TX3ZITX1J;" + Reader["AccountNumber"]);
				if (RI.Check4Text(1, 72, "TXX1K") == true)
				{
					WriteError((string)Reader["AccountNumber"], "-----", ref RI);
				}
				else
				{
					HasData = true;
					data.SSN = RI.GetText(3, 12, 11).Replace(" ", ""); //SSN
					//**2**
					//data.Name = Reader["Name"].ToString(); //Name
					data.Name = string.Format("{0} {1} {2}", RI.GetText(4, 34, 12).Trim(), RI.GetText(4, 53, 1), RI.GetText(4, 6, 20).Trim()).Replace("  ", " "); //Name
					data.DOB = RI.GetText(20, 6, 10).Replace(" ", "/"); //DOB
					data.RoutingNumber = Reader["RoutingNum"].ToString(); //routing number
					data.BankAccountNumber = Reader["BankAccountNum"].ToString(); //banking account number
					//account type
					if (Reader["AccountType"].ToString().Substring(0, 1).ToUpper() == "C")
					{
						data.AccountType = "Checking";
					}
					else
					{
						data.AccountType = "Savings";
					}
					data.PaymentAmount = Reader["Amount"].ToString().Replace(",", ""); //payment amount
					data.EffectiveDate = DateTime.Now.ToShortDateString();
					data.EmailAddress = string.Empty;
					data.AccountHolderName = data.Name; //Holder name
					//Fields for Confirmation Letter
					FileWriter.WriteLine(@"""" + Common.ACSKeyLine(RI.GetText(3, 12, 11).Replace(" ", ""), Common.ACSKeyLinePersonType.Borrower, Common.ACSKeyLineAddressType.Legal) + @""",""" + data.Name + @""",""" + RI.GetText(11, 10, 30).Trim() + @""",""" + (RI.GetText(12, 10, 30).Trim() + RI.GetText(13, 10, 30).Trim()).Replace("_", "") + @""",""" + RI.GetText(14, 8, 20).Trim() + @""",""" + RI.GetText(14, 32, 2) + @""",""" + RI.GetText(14, 40, 18) + @""",""" + RI.GetText(13, 52, 25).Trim().Replace("_", "") + @""",""" + Reader["AccountNumber"] + @""",""" + String.Format("{0:MM/dd/yyyy}", Reader["AuthDate"].ToString()) + @""",""$" + Reader["Amount"].ToString().Replace(",", "") + @""",""" + String.Format("{0:MM/dd/yyyy}", DateTime.Now) + @""",""" + RI.GetText(14, 32, 2) + @""",""" + @"MA2324" + @"""");
					////encrypt bank account **4**
					//data.BankAccountNumber = new StringEncryption("To use OR nOt To use tHe OpS web site For Check bY PhOnEs, ThaT is the QuestIon?").EncryptString(data.BankAccountNumber);
					//write out record to DB
					DataAccess.AddEntryToDB(TestMode, data);
					//add comments to system
					if (TD22AllLoans(Reader["AccountNumber"].ToString(), "PHNPI", ref RI, "IVR CHECK BY PHONE FOR: " + string.Format("{0:0.00}", Reader["Amount"])) == false)
					{
						MessageBox.Show("An error occured while trying to add a comment on TD22.  Please contact Systems Support.  The application will now shut down please restart it when the problem is solved.", "TD22 Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						Environment.Exit(1);
					}
					//close reader and update record just processed **5**
					Comm2.Connection = _ulsUpdateConn;
					_ulsUpdateConn.Open();
					//Comm2.CommandText = "UPDATE CheckByPhone SET ProcessedDate = GETDATE() WHERE RecNum = " + RecNum;
					Comm2.CommandText = string.Format("EXEC spIvrUpdatePaymentProcessedStatus {0}", RecNum);
					Comm2.ExecuteNonQuery();
					_ulsUpdateConn.Close();
					//check if there is another record for processing
				}
			}
			FileWriter.Close();
			_ulsConn.Close();
			if (HasData)
			{
				System.Windows.Forms.MessageBox.Show("IVR Payment Confirmation Letters will be printed, please set printer to single sided and press OK when ready.", "Set printer to single sided");
				DocumentHandling.CostCenterPrinting(TestMode, "IVRPMTCNF", "T:\\IVRTextFile.txt", "COST_CENTER_CODE", "State", "IVRREQACT");
				//clean up after
				if (File.Exists("T:\\IVRTextFile.txt"))
				{
					File.Delete("T:\\IVRTextFile.txt");
				}
			}
		}

	}
}

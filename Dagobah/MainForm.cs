using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Dagobah
{
	public partial class MainForm : Form
	{
		private readonly Color OFFLINE_COLOR = Color.Silver;
		private readonly Color ONLINE_COLOR = Color.Lime;
		private const int PANEL_TAB_SIZE = 35;

		//A RequestType variable is needed to remember what kinds of requests we're looking at
		//when we switch between programmer and court, and from one programmer to another.
		private DataAccess.RequestType _selectedRequestType;
		//Since the title bar is hidden, we'll have to watch for MouseDown and do manually-implemented dragging.
		private bool _dragging;
		private Point _dragStartPoint;
		private Messenger _messenger;
		private User _selectedUser;

		public MainForm()
		{
			InitializeComponent();

			_dragging = false;

			//Have the _selectedUser default to the current user when the program starts,
			//provided the current user is in the Users table.
			_selectedUser = DataAccess.Users.Where(p => p.ID == Environment.UserName).SingleOrDefault();
			if (_selectedUser == null)
			{
				string message = "Your user ID was not found in the table of Dagobah users.";
				MessageBox.Show(message, Properties.Settings.Default.ErrorTitle);
				Application.Exit();
			}

			//Have the _selectedRequestType default to Dagobah when the program starts.
			_selectedRequestType = DataAccess.RequestType.Dagobah;

			//Start with the panels contracted.
			pnlBottom.Top = pnlFront.Bottom - pnlBottom.Height + PANEL_TAB_SIZE;
			pnlLeft.Left = pnlFront.Left - PANEL_TAB_SIZE;
			pnlRight.Left = pnlFront.Right - pnlRight.Width + PANEL_TAB_SIZE;
			pnlFront.BringToFront();

			//Set the dynamic UI elements.
			ShowSkin();
			ShowTitleText();
			ShowUserList();
			ShowSkinList();
			ShowRequests();

			//Instantiate our Messenger and subscribe to its RemoteUserOnlineStatusChanged and MessageReceived events.
			string password = Microsoft.VisualBasic.Interaction.InputBox("Please provide your Windows login password to authenticate with the message server.", "Password", "Password", 450, 350);
			_messenger = new Messenger(password);
			_messenger.RemoteUserOnlineStatusChanged += new EventHandler<OnlineStatusEventArgs>(messenger_ConnectionClosed);
			_messenger.MessageReceived += new EventHandler<MessageReceivedEventArgs>(messenger_MessageReceived);
		}

		private void SendMessage(string text)
		{
			//See if we're whispering to anybody.
			List<string> recipients = new List<string>();
			if (chkWhisper.Checked)
			{
				foreach (ListViewItem selectedItem in lstUsers.SelectedItems)
				{
					if (selectedItem.ForeColor == ONLINE_COLOR)
					{
						recipients.Add(DataAccess.Users.Where(p => p.Name == selectedItem.Text).Single().ID);
					}
				}
			}
			else
			{
				foreach (ListViewItem item in lstUsers.Items)
				{
					if (item.ForeColor == ONLINE_COLOR)
					{
						recipients.Add(DataAccess.Users.Where(p => p.Name == item.Text).Single().ID);
					}
				}
			}
			//Create a Message object and send it through the Messenger.
			Message message = new Message();
			message.Sender = DataAccess.CurrentUser.ID;
			message.Recipients = recipients;
			message.Text = text;
			_messenger.SendMessage(message);
		}//SendMessage()

		private void SetUserOnlineStatus(User user, bool isOnline)
		{
			lstUsers.Items[user.ID].ForeColor = (isOnline ? ONLINE_COLOR : OFFLINE_COLOR);
		}//SetUserOnlineStatus()

		private void ShowMessage(Message message)
		{
			//Format the message to show the sender, whisper recipients (if applicable), and message text.
			StringBuilder formattedTextBuilder = new StringBuilder();
			string senderFirstName = DataAccess.Users.Where(p => p.ID == message.Sender).Single().Name.Split(' ')[0];
			formattedTextBuilder.Append(string.Format("{0}  {1}", senderFirstName, DateTime.Now.ToString("h:mm tt")));
			formattedTextBuilder.Append(Environment.NewLine);

			string[] recipientFirstNames = DataAccess.Users.Where(p => message.Recipients.Contains(p.ID)).Select(p => p.Name.Split(' ')[0]).ToArray();
			formattedTextBuilder.Append(string.Format("<{0}>", string.Join(", ", recipientFirstNames)));
			formattedTextBuilder.Append(Environment.NewLine);

			formattedTextBuilder.Append(message.Text);
			formattedTextBuilder.Append(Environment.NewLine);
			formattedTextBuilder.Append(Environment.NewLine);

			//Append the whole shebang to the Receive text box.
			txtReceive.AppendText(formattedTextBuilder.ToString());

			//Show the message in the tray icon if the form isn't visible.
			if (this.WindowState == FormWindowState.Minimized || Form.ActiveForm == null)
			{
				notifyIcon1.BalloonTipTitle = string.Format("Message from {0}", senderFirstName);
				notifyIcon1.BalloonTipText = formattedTextBuilder.ToString();
				notifyIcon1.ShowBalloonTip(0);
			}
		}//ShowMessage()

		//Display requests for the selected user and request type.
		private void ShowRequests()
		{
			dgvRequests.DataSource = DataAccess.GetRequests(_selectedUser.Name, _selectedRequestType, radByCourt.Checked);
		}//ShowRequests()

		//Set the panel and button images according to the skin.
		private void ShowSkin()
		{
			lblDagobah.Image = Image.FromFile(Skin.DagobahImage);
			lblLetter.Image = Image.FromFile(Skin.LettersImage);
			lblSas.Image = Image.FromFile(Skin.SasImage);
			lblScript.Image = Image.FromFile(Skin.ScriptsImage);
			pnlBottom.BackgroundImage = Image.FromFile(Skin.BottomPanelImage);
			pnlFront.BackgroundImage = Image.FromFile(Skin.CenterPanelImage);
			pnlLeft.BackgroundImage = Image.FromFile(Skin.LeftPanelImage);
			pnlRight.BackgroundImage = Image.FromFile(Skin.RightPanelImage);
		}//ShowSkin()

		//Display the selected user and request type in the title area.
		private void ShowTitleText()
		{
			string requestType = string.Empty;
			switch (_selectedRequestType)
			{
				case DataAccess.RequestType.Dagobah:
					requestType = "Dagobah";
					break;
				case DataAccess.RequestType.Script:
					requestType = "Scripts";
					break;
				case DataAccess.RequestType.Sas:
					requestType = "SAS";
					break;
				case DataAccess.RequestType.Letter:
					requestType = "Letters";
					break;
			}
			lblTitle.Text = string.Format("{0}: {1}", _selectedUser.Name, requestType);
		}//ShowTitleText()

		//See what skins are available and display the list in the skins ListView.
		private void ShowSkinList()
		{
			lstSkins.Items.Clear();
			lstSkins.DataSource = DataAccess.GetSkins();
		}//ShowSkinList()

		//Display all Dagobah users in the user ListView.
		private void ShowUserList()
		{
			foreach (User user in DataAccess.Users)
			{
				lstUsers.Items.Add(user.ID, user.Name, 0);
			}
		}//ShowUserList()

		private void SlideBottomPanel()
		{
			int targetPosition = pnlFront.Bottom + pnlBottom.Height;
			int stepSize = 5;
			if (pnlBottom.Bottom >= pnlFront.Bottom + pnlBottom.Height)
			{
				targetPosition = pnlFront.Bottom + PANEL_TAB_SIZE;
				stepSize = -stepSize;
			}
			while (pnlBottom.Bottom != targetPosition)
			{
				pnlBottom.Top += stepSize;
			}
		}//SlideBottomPanel()

		private void SlideLeftPanel()
		{
			int targetPosition = pnlFront.Left - pnlLeft.Width;
			int stepSize = -5;
			if (pnlLeft.Left <= pnlFront.Left - pnlLeft.Width)
			{
				targetPosition = pnlFront.Left - PANEL_TAB_SIZE;
				stepSize = -stepSize;
			}
			while (pnlLeft.Left != targetPosition)
			{
				pnlLeft.Left += stepSize;
			}
		}//SlideLeftPanel()

		private void SlideRightPanel()
		{
			int targetPosition = pnlFront.Right + pnlRight.Width;
			int stepSize = 5;
			if (pnlRight.Right >= pnlFront.Right + pnlRight.Width)
			{
				targetPosition = pnlFront.Right + PANEL_TAB_SIZE;
				stepSize = -stepSize;
			}
			while (pnlRight.Right != targetPosition)
			{
				pnlRight.Left += stepSize;
			}
		}//SlideRightPanel()

		private void StartDrag(Point currentMousePosition)
		{
			_dragStartPoint = new Point(currentMousePosition.X, currentMousePosition.Y);
			_dragging = true;
		}//StartDrag()

		private void Drag(Point currentMousePosition)
		{
			if (!_dragging) { return; }
			this.Location = new Point(this.Location.X + (currentMousePosition.X - _dragStartPoint.X), this.Location.Y + (currentMousePosition.Y - _dragStartPoint.Y));
		}//Drag()

		private void EndDrag()
		{
			_dragging = false;
		}//EndDrag()

		#region Event Handlers
		private void btnExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnMinimize_Click(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Minimized;
		}

		private void lblDagobah_Click(object sender, EventArgs e)
		{
			_selectedRequestType = DataAccess.RequestType.Dagobah;
			ShowTitleText();
			ShowRequests();
		}

		private void lblScript_Click(object sender, EventArgs e)
		{
			_selectedRequestType = DataAccess.RequestType.Script;
			ShowTitleText();
			ShowRequests();
		}

		private void lblSas_Click(object sender, EventArgs e)
		{
			_selectedRequestType = DataAccess.RequestType.Sas;
			ShowTitleText();
			ShowRequests();
		}

		private void lblLetter_Click(object sender, EventArgs e)
		{
			_selectedRequestType = DataAccess.RequestType.Letter;
			ShowTitleText();
			ShowRequests();
		}

		private void lblTitle_MouseDown(object sender, MouseEventArgs e)
		{
			StartDrag(new Point(e.X, e.Y));
		}

		private void lblTitle_MouseMove(object sender, MouseEventArgs e)
		{
			Drag(new Point(e.X, e.Y));
		}

		private void lblTitle_MouseUp(object sender, MouseEventArgs e)
		{
			EndDrag();
		}

		private void lstSkins_DoubleClick(object sender, EventArgs e)
		{
			Skin.Name = lstSkins.SelectedItem.ToString();
			ShowSkin();
		}

		private void lstUsers_DoubleClick(object sender, EventArgs e)
		{
			_selectedUser = DataAccess.Users.Where(p => p.Name == lstUsers.SelectedItems[0].Text).Single();
			ShowTitleText();
			ShowRequests();
		}

		private void MainForm_Resize(object sender, EventArgs e)
		{
			//Minimize to the tray icon.
			if (this.WindowState == FormWindowState.Minimized) { this.Hide(); }
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			//Fade the form into view.
			while (this.Opacity < 1)
			{
				this.Opacity += 0.02;
				Thread.Sleep(1);
			}
		}

		private void notifyIcon1_DoubleClick(object sender, EventArgs e)
		{
			//Restore from the tray icon when double-clicked.
			this.Show();
			this.WindowState = FormWindowState.Normal;
			this.BringToFront();
		}

		private void pnlBottom_DoubleClick(object sender, EventArgs e)
		{
			SlideBottomPanel();
		}

		private void pnlFront_MouseDown(object sender, MouseEventArgs e)
		{
			StartDrag(new Point(e.X, e.Y));
		}

		private void pnlFront_MouseMove(object sender, MouseEventArgs e)
		{
			Drag(new Point(e.X, e.Y));
		}

		private void pnlFront_MouseUp(object sender, MouseEventArgs e)
		{
			EndDrag();
		}

		private void pnlLeft_DoubleClick(object sender, EventArgs e)
		{
			SlideLeftPanel();
		}

		private void pnlRight_DoubleClick(object sender, EventArgs e)
		{
			SlideRightPanel();
		}

		private void radByCourt_CheckedChanged(object sender, EventArgs e)
		{
			ShowRequests();
		}

		private void radByProgrammer_CheckedChanged(object sender, EventArgs e)
		{
			ShowRequests();
		}

		private void txtSend_KeyPress(object sender, KeyPressEventArgs e)
		{
			//Check if the key was Enter.
			if (e.KeyChar == (char)Keys.Return)
			{
				//Send the TextBox text and clear the TextBox.
				SendMessage(txtSend.Text);
				txtSend.Clear();
				//Mark the event as handled.
				e.Handled = true;
			}
		}

		//The Messenger events are raised on other threads, so they can't act directly on items in this class.
		//We have to use Control.Invoke() and a MethodInvoker delegate to handle the cross-thread interaction.
		private void messenger_ConnectionClosed(object sender, OnlineStatusEventArgs e)
		{
			this.Invoke((MethodInvoker)delegate() { SetUserOnlineStatus(e.User, false); });
		}

		private void messenger_MessageReceived(object sender, MessageReceivedEventArgs e)
		{
			//Calls to this.Invoke() need to be wrapped in a try/catch
			//in case a message is received after the form closes.
			try
			{
				//Display the message.
				this.Invoke((MethodInvoker)delegate() { ShowMessage(e.Message); });
			}
			catch (Exception) { }
		}
		#endregion Event Handlers
	}//class
}//namespace

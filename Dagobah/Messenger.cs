using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.Collections;
using agsXMPP.protocol.iq.roster;


namespace Dagobah
{
	class Messenger
	{
		public event EventHandler<OnlineStatusEventArgs> RemoteUserOnlineStatusChanged;
		public event EventHandler<MessageReceivedEventArgs> MessageReceived;
		private XmppClientConnection _xmpp;

		public Messenger(string password)
		{
			//Create an XMPP client and register some callbacks.
			Jid jabberId = new Jid(DataAccess.CurrentUser.ID, "wordpress", "spark");
			_xmpp = new XmppClientConnection(jabberId.Server);
			_xmpp.OnBinded += new ObjectHandler(xmpp_OnLogin); //HACK
			_xmpp.OnAuthError += new XmppElementHandler(xmpp_OnRegisterError); //HACK
			_xmpp.OnRegistered += new ObjectHandler(xmpp_OnLogin); //HACK
			_xmpp.OnRegisterError += new XmppElementHandler(xmpp_OnRegisterError); //HACK
			_xmpp.OnRegisterInformation += new agsXMPP.protocol.iq.register.RegisterEventHandler(xmpp_OnRegisterInformation); //HACK
			_xmpp.OnSocketError += new ErrorHandler(xmpp_OnError); //HACK
			_xmpp.OnXmppError += new XmppElementHandler(xmpp_OnRegisterError); //HACK
			_xmpp.OnError += new ErrorHandler(xmpp_OnError);
			_xmpp.OnLogin += new ObjectHandler(xmpp_OnLogin);
			_xmpp.OnPresence += new PresenceHandler(xmpp_OnPresence);
			_xmpp.MesagageGrabber.Add(jabberId, new BareJidComparer(), new MessageCB(xmpp_MessageCallBack), null);
			//Connect to the server.
			_xmpp.AutoResolveConnectServer = true;
			_xmpp.Open(jabberId.User, password);
		}

		void xmpp_OnRegisterInformation(object sender, agsXMPP.protocol.iq.register.RegisterEventArgs args)
		{
			throw new NotImplementedException();
		}

		void xmpp_OnRegisterError(object sender, agsXMPP.Xml.Dom.Element e)
		{
			throw new NotImplementedException();
		}//Constructor

		~Messenger()
		{
			_xmpp.Close();
		}//Destructor

		/// <summary>
		/// Send a message to Dagobah users.
		/// </summary>
		/// <param name="message">The Message object to send.</param>
		public void SendMessage(Message message)
		{
			foreach (string recipient in message.Recipients)
			{
				Jid receiverJid = new Jid(recipient + "@uheaa");
				_xmpp.Send(new agsXMPP.protocol.client.Message(receiverJid, MessageType.chat, message.ToString()));
			}
		}//SendMessage()

		protected virtual void OnRemoteUserOnlineStatusChanged(OnlineStatusEventArgs e)
		{
			//Send the event to any subscribers.
			if (RemoteUserOnlineStatusChanged != null) { RemoteUserOnlineStatusChanged(this, e); }
		}//OnRemoteUserOnlineStatusChanged()

		protected virtual void OnMessageReceived(MessageReceivedEventArgs e)
		{
			//Send the event to any subscribers.
			if (MessageReceived != null) { MessageReceived(this, e); }
		}//OnMessageReceived()

		private void xmpp_OnError(object sender, Exception ex)
		{
			throw ex;
		}//xmpp_OnError()

		private void xmpp_OnLogin(object sender)
		{
			if (!_xmpp.Authenticated)
			{
				throw new Exception("Could not connect to the chat server.");
			}
			else
			{
				//Raise an event in Dagobah so the UI shows us an online.
				OnRemoteUserOnlineStatusChanged(new OnlineStatusEventArgs(DataAccess.CurrentUser, true));
				//Establish our presence as online.
				Presence presence = new Presence(ShowType.chat, "Online");
				presence.Type = PresenceType.available;
				_xmpp.Send(presence);
			}
		}//xmpp_OnLogin()

		private void xmpp_MessageCallBack(object sender, agsXMPP.protocol.client.Message xmppMessage, object data)
		{
			if (xmppMessage.Body == null) { return; }
			Message message = new Message();
			message.Sender = xmppMessage.From.User;
			message.Text = xmppMessage.Body;
			OnMessageReceived(new MessageReceivedEventArgs(message));
		}//xmpp_MessageCallBack()

		private void xmpp_OnPresence(object sender, Presence presence)
		{
			//See if the user is in our list of interesting people.
			User user = DataAccess.Users.Where(p => p.ID == presence.From.User).SingleOrDefault();
			if (user == null) { return; }

			//Get the user's online status.
			bool isOnline = (presence.Type == PresenceType.available);

			//Raise an event to update the UI.
			OnRemoteUserOnlineStatusChanged(new OnlineStatusEventArgs(user, isOnline));
		}//xmpp_OnPresence()
	}//class

	//EventArgs class for RemoteUserOnlineStatusChanged events
	public class OnlineStatusEventArgs : EventArgs
	{
		public readonly User User;
		public readonly bool IsOnline;

		public OnlineStatusEventArgs(User user, bool isOnline)
		{
			User = user;
			IsOnline = isOnline;
		}
	}//class

	//EventArgs class for MessageReceived events
	public class MessageReceivedEventArgs : EventArgs
	{
		public readonly Message Message;

		public MessageReceivedEventArgs(Message message)
		{
			Message = message;
		}
	}//class
}//namespace

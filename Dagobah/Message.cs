using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dagobah
{
    public class Message
    {
        /// <summary>
        /// User ID of the sender.
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// User IDs of all recipients.
        /// </summary>
        public List<string> Recipients { get; set; }

        /// <summary>
        /// The body of the text.
        /// </summary>
        public string Text { get; set; }

		/// <summary>
		/// Message with a header line that includes the sender's name, a list of recipients, and a time stamp.
		/// </summary>
		public override string ToString()
		{
			string displaySender = DataAccess.Users.Where(p => p.ID == Sender).Single().Name;
			List<string> displayRecipients = new List<string>();
			foreach (string recipient in Recipients.Where(p => p != DataAccess.CurrentUser.ID))
			{
				string displayName = DataAccess.Users.Where(p => p.ID == recipient).Single().Name;
				displayRecipients.Add(displayName);
			}
			string recipientList = string.Join(", ", displayRecipients.ToArray());
			string timeStamp = DateTime.Now.ToString("h:mm tt");
			string headerLine = string.Format("{0} <{1}> {2}", displaySender, recipientList, timeStamp);

			StringBuilder messageBuilder = new StringBuilder();
			messageBuilder.Append(headerLine);
			messageBuilder.Append(Environment.NewLine);
			messageBuilder.Append(Text);
			messageBuilder.Append(Environment.NewLine);
			messageBuilder.Append(Environment.NewLine);

			return messageBuilder.ToString();
		}
    }//class
}//namespace

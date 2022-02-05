using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.WinForms;

namespace MDIntermediary
{
    public class AcpRecipientInfo
    {
        public string RecipientName { get; set; }
        public string Relationship { get; set; }
        public string ContactPhoneNumber { get; set; }
        public bool Authorized { get; set; }

        public string Validate()
        {
            List<string> messages = new List<string>();
            if (string.IsNullOrEmpty(RecipientName))
                messages.Add("Please enter a Recipient Name");
            if (string.IsNullOrEmpty(Relationship))
                messages.Add("Please select a Relationship for the Recipient");
            if (string.IsNullOrEmpty(ContactPhoneNumber))
                messages.Add("Please enter a Contact Phone Number for the Recipient.");
            if (messages.Any())
                return string.Join(Environment.NewLine, messages);
            return null;
        }
    }
}

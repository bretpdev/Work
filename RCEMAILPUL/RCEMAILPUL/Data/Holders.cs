using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCEMAILPUL
{
    class MessageHolder
    {
        public List<SendGridMessage> messages { get; set; }
    }
    class UnsubscribesHolder
    {
        public List<UnsubscribedEmail> email_recipients { get; set; }
    }
}

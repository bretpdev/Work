using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    public class Alert
    {
        public enum AlertUrgency
        {
            Low,
            Medium,
            High
        }

        public string Text { get; }
        public AlertUrgency Urgency { get; }
        
        public Alert(string text, AlertUrgency urgency)
        {
            Text = text;
            Urgency = urgency;
        }
    }
}

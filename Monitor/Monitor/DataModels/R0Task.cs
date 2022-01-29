using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public class R0Task
    {
        public string TaskControl { get; set; }
        public string ActionRequest { get; set; }
        public DateTime DateRequested { get; set; }
        public string MonitorReason { get; set; }
        public string Ssn { get; set; }
        public string AccountNumber { get; set; }
        public bool IsInvalid { get; set; }
        public bool IsAbend { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is R0Task))
                return false;
            var task = (R0Task)obj;
            return GetHashCode() == task.GetHashCode();
        }

        public override int GetHashCode()
        {
            return (TaskControl + ActionRequest + DateRequested.ToShortDateString()).GetHashCode();
        }

        public override string ToString()
        {
            return string.Join("|", TaskControl, ActionRequest, DateRequested.ToShortDateString());
        }
    }
}

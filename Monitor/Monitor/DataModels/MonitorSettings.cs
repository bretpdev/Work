using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public class MonitorSettings
    {
        public decimal MaxIncrease { get; set; }
        public int MaxPreNote { get; set; }
        public int MaxForce { get; set; }
        public int? LastRecoveryPage { get; set; }

        public int TotalPreNoteCounter { get { return prenoteSsns.Count; } }
        public int TotalForceCounter { get { return forceSsns.Count; } }

        HashSet<string> prenoteSsns = new HashSet<string>();
        HashSet<string> forceSsns = new HashSet<string>();
        public void PrenoteAdded(string ssn)
        {
            lock (prenoteSsns)
                if (!prenoteSsns.Contains(ssn))
                    prenoteSsns.Add(ssn);
        }
        public void ForceAdded(string ssn)
        {
            lock (forceSsns)
                if (!forceSsns.Contains(ssn))
                    forceSsns.Add(ssn);
        }
    }
}

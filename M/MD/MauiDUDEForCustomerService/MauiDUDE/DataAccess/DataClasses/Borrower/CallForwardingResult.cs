using MDIntermediary.Calls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiDUDE
{
    public class CallForwardingResult
    {
        public string Location { get; set; }
        public string Number { get; set; }
        public Alert.AlertUrgency AlertUrgency { get; set; }
        public string OverrideMessage { get; set; }

        public CallForwardingResult(string tLocation, string tNumber, Alert.AlertUrgency alertUrgency)
        {
            Location = tLocation;
            Number = tNumber;
            this.AlertUrgency = alertUrgency;
        }

        public CallForwardingResult Copy()
        {
            return new CallForwardingResult(Location, Number, AlertUrgency);
        }
    }
}

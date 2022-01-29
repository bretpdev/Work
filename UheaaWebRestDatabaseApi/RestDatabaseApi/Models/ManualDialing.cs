using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestDatabaseApi
{
    public class ManualDialing
    {
        public string CountLast45 { get; set; }
        public string State { get; set; }
        public string Name { get; set; }
        public int? DaysDelinquent { get; set; }
        public string AmountDue { get; set; }
        public string AccountNumber { get; set; }
        public string Primary { get; set; }
        public string Alternate { get; set; }
    }
}
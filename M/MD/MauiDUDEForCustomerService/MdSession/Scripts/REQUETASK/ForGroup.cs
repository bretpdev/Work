using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REQUETASK
{
    public static class ForGroups
    {
        public static List<ForGroup> All { get; private set; }
        static ForGroups()
        {
            All = new List<ForGroup>();
            All.Add(new ForGroup("D", "Borrower Services", "DCALL"));
            All.Add(new ForGroup("S", "Auxiliary Service", "SCALL"));
            All.Add(new ForGroup("O", "Loan Originations", "OCALL"));
            All.Add(new ForGroup("R", "Account Services", "BCALL"));
            All.Add(new ForGroup("X", "Administrative", "XCALL"));
        }
    }
    public class ForGroup
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Arc { get; set; }
        public override string ToString()
        {
            return Code + " - " + Name;
        }
        public ForGroup(string code, string name, string arc)
        {
            Code = code; Name = name; Arc = arc;
        }
    }
}

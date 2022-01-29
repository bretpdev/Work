using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.WinForms;

namespace KEYIDCHNG
{
    public class KeyIdentifierChangeModel
    {
        public string Ssn { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        public string Comments { get; set; }
        public bool Approve { get; set; }

        public bool UpdateCompass { get; set; }
        public bool UpdateOneLink { get; set; }
    }
}

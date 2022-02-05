using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class TestAccount
    {
        public string Ssn { get; set; }
        public string LastName { get; set; }

        public TestAccount(string ssn, string lastName)
        {
            Ssn = ssn;
            LastName = lastName;
        }
    }
}

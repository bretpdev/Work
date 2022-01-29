using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CODXMLCPY
{
    public class CodApp
    {
        public string Ssn { get; set; }
        public string ApplicationId { get; set; }

        public CodApp(string ssn, string applicationId)
        {
            Ssn = ssn;
            ApplicationId = applicationId;
        }

    }
}

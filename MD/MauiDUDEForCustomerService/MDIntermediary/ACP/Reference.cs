using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDIntermediary
{
    public class Reference
    {
        public string ReferenceId { get; set; }
        public string AuthorizedThirdPartyIndicator { get; set; }
        public string RelationshipToBorrower { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string LastContact { get; set; }
        public string LastAttempt { get; set; }
        public string StatusIndicator { get; set; }
    }
}

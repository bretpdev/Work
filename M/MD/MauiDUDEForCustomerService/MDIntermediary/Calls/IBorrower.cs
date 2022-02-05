using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDIntermediary
{
    public interface IBorrower
    {
        string AccountNumber { get; set; }
        string SSN { get; set; }
        AcpResponses AcpResponses { get; set; }
        DemographicVerifications DemographicsVerifications { get; set; }
        string ContactPhoneNumber { get; set; }
        List<Reference> References { get; set; }
        bool NeedsDeconArc { get; set; }
    }
}

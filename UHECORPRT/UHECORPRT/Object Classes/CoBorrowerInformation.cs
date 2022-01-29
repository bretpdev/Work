using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UHECORPRT
{
    public class CoBorrowerInformation
    {
        public string CoBorrowerSSN { get; set; }
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ValidAddress { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ForeignState { get; set; }
        public string ForeignCountry { get; set; }
        public string ValidEmail { get; set; }
        public string OnEcorr { get; set; }
    }
}
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AUXLTRS.Object_Classes
{
       class BorrowerData
    {
        public string Ssn { get; set; }
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public string Prev {get;set;}
        public string Dob { get; set; }
        //public List<LoanData> LoanInfo { get; set; }
        public string UpdateType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ForState { get; set; }
        public string ForCountry {  get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string AltPhone1 { get; set; }
        public string AltPhone2 { get; set; }
        public string Email { get; set; }
        public string ForeignPhone { get; set; }
        public string ForeignAlt1 { get; set; }
        public string ForeignAlt2 { get; set; }

        public BorrowerData()
        {
            //LoanInfo = new List<LoanData>();
        }
    }

}

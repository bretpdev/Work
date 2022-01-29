using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACHSETUPFD
{
    public class CoborrowerDemographics
    {
        //POP.BorrowerSSN,
        //POP.CoBorrowerSSN,
        //POP.LoanSequence,
        //POP.FirstName,
        //POP.MiddleName,
        //POP.LastName,
        //POP.AccountNumber,
        //POP.BorrowerAccountNumber,
        //POP.ValidAddress,
        //POP.[Address1],
        //POP.[Address2],
        //POP.[Address3],
        //POP.City,
        //POP.[State],
        //POP.Zip,
        //POP.ForeignState,
        //POP.ForeignCountry,
        //MAX(POP.OnEcorr) AS[OnEcorr]
        public string BorrowerSSN { get; set; }
        public string CoBorrowerSSN { get; set; }
        public int LoanSequence { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string AccountNumber { get; set; }
        public string BorrowerAccountNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ForeignState { get; set; }
        public string ForeignCountry { get; set; }
        public bool OnEcorr { get; set; }


    }
}

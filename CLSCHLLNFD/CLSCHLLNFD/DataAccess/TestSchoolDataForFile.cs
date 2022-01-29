using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLSCHLLNFD
{
    /// <summary>
    /// This object is only used in the UnitTests.cs file in order
    /// to write out to a test file.
    /// </summary>
    public class TestSchoolDataForFile
    {
        public string SchoolCode { get; set; }
        public string SchoolBranchCode { get; set; }
        public string SchoolName { get; set; }
        public DateTime? CloseDate { get; set; }
        public string CurrentGaCode { get; set; }
        public string StudentSsn { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? StudentDob { get; set; }
        public string PlusSsn { get; set; }
        public string PlusLastName { get; set; }
        public string PlusFirstName { get; set; }
        public DateTime? PlusDob { get; set; }
        public string LoanType { get; set; }
        public DateTime? LoanDate { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal TotalDisbursed { get; set; }
        public decimal TotalCancelled { get; set; }
        public string CurrentLoanStatus { get; set; }
        public DateTime? CurrentLoanStatusDate { get; set; }
        public decimal OutstandingPrincipalBalance { get; set; }
        public DateTime? OutstandingPrincipalBalanceDate { get; set; }
        public decimal OutstandingInterestBalance { get; set; }
        public DateTime? OutstandingInterestBalanceDate { get; set; }
        public string AwardId { get; set; }
        public string LoanIdentifier { get; set; }
        public string StudentNumber { get; set; }
        public string StudentIdentifier { get; set; }
        public string ContactCode { get; set; }
        public string Email { get; set; }
        public char AddressType { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
    }
}

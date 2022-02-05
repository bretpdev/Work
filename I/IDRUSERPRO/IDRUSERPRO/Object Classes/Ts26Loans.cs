using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using Uheaa.Common.Scripts;

namespace IDRUSERPRO
{
    public class Ts26Loans
    {
        public int BorrowerId { get; set; }
        public int AppId { get; set; }
        public string LoanType { get; set; }
        public string LoanSeq { get; set; }
        public string AwardId { get; set; }
        public string DisbDate { get; set; }
        [DbIgnore]
        public bool HasBalance { get; set; } = true;
        [DbIgnore]
        public bool IsEligible { get; set; } = true;
    }
}
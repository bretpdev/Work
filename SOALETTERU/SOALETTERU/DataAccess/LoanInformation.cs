using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace SOALETTERU
{
    public class LoanInformation
    {
        [LoanAttribute("LOAN SEQUENCE: ", 1)]
        [DbName("Loan Seq")]
        public string LoanSequence { get; set; }
        [LoanAttribute("LOAN PROGRAM: ", 2)]
        [DbName("Loan Program")]
        public string LoanProgram { get; set; }
        [LoanAttribute("FIRST DISBURSEMENT DATE: ", 3, @"MM/dd/yyyy")]
        [DbName("1st Disb Date")]
        public string FirstDisbursementDate { get; set; }
        [LoanAttribute("PRINCIPAL BALANCE AT TRANSFER: ", 5)]
        [DbName("Principal Balance at Transfer")]
        public decimal PrincipalBalanceAtTransfer { get; set; }
        [LoanAttribute("CURRENT PRINCIPAL BALANCE: ", 7)]
        [DbName("Current Principal Balance")]
        public decimal CurrentPrincipalBalance { get; set; }
    }
}

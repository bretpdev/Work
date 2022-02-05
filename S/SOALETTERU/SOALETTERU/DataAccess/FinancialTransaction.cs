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
    public class FinancialTransaction
    {
        [FinancialAttribute("LOAN SEQUENCE: ", 1)]
        [DbName("Loan Seq")]
        public string LoanSequence { get; set; }
        [FinancialAttribute("TRANSACTION DATE: ", 2, @"MM/dd/yyyy")]
        [DbName("Transaction Date")]
        public string TransactionDate { get; set; }
        [FinancialAttribute("TRANSACTION TYPE: ", 3)]
        [DbName("Transaction Type")]
        public string TransactionType { get; set; }
        [FinancialAttribute("TRANSACTION AMOUNT: ", 5)]
        [DbName("Total Amount of Transaction")]
        public decimal TotalAmountOfTransaction { get; set; }
        [FinancialAttribute("APPLIED TO PRINCIPAL: ", 7)]
        [DbName("Amount to Principal")]
        public decimal AppliedToPrincipal { get; set; }
        [FinancialAttribute("APPLIED TO INTEREST: ", 9)]
        [DbName("Amount to Interest")]
        public decimal AppliedToInterest { get; set; }
    }
}

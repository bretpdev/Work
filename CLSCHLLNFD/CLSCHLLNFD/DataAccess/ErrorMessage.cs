using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLSCHLLNFD
{
    public class ErrorMessage
    {
        public const string LOAN_STATUS = "Discharge couldn’t be processed due to loan status.";
        public const string UNREFUNDED_PAYMENTS = "Discharge couldn’t be processed due to payments needing to be reversed.";
        public const string TX30_SESSION_MESSAGE = "On TSX30, the following Session message occurred: ";
        public const string ATS3Q_SESSION_MESSAGE = "On ATS3Q, the following Session message occurred: ";
        public const string LOAN_NOT_FOUND = "Loan was not found.";
        public const string WRITE_OFF = "While processing write off, the following Session message occurred: ";
    }
}

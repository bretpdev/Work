using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace SERF_File_Generator
{
    public static class LoanHelper
    {
        private class InvalidLoanNum
        {
            public string Ssn { get; set; }
            public int Num { get; set; }
        }
        static List<InvalidLoanNum> Invalids = new List<InvalidLoanNum>();
        static LoanHelper()
        {
            Invalids = DataAccessHelper.ExecuteList<InvalidLoanNum>("Validation_GetInvalidLoans", DataAccessHelper.Database.AlignImport);
        }

        public static bool ObjectIsValid(string ssn, object o)
        {
            var prop = o.GetType().GetProperty("LN_SEQ");
            if (prop == null) return true; //not loan related
            string stringLoanNum = prop.GetValue(o).ToString();
            int loanNum = 0;
            try
            {
                loanNum = int.Parse(stringLoanNum);
            }
            catch (FormatException)
            {
                if (o.GetType() == typeof(StudentData))
                    return true;
                return false;
            }
            InvalidLoanNum invalid = new InvalidLoanNum() { Ssn = ssn, Num = loanNum };
            var found = Invalids.SingleOrDefault(x => x.Ssn == ssn && x.Num == loanNum);
            return found == null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace DFACDFED
{
    public class LoanInfo
    {
        public char LetterAction { get; set; }
        public char LetterType { get; set; }
        public int LetterTypeCode { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LoanSequence { get; set; }
        public string LoanProgram { get; set; }
    }
}

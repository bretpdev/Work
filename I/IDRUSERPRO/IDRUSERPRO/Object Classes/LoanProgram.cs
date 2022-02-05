using System.Collections.Generic;
using System.Linq;

namespace IDRUSERPRO
{
    public class LoanPrograms
    {
        public string LoanProgram { get; set; }
        public string NsldsCode { get; set; }
        public bool IsDirect { get; set; }

        public static bool Contains(List<LoanPrograms> pgms, string text)
        {
            if (pgms.Where(p => p.LoanProgram.ToUpper() == text.ToUpper()).Any())
                return true;
            return false;
        }
    }
}
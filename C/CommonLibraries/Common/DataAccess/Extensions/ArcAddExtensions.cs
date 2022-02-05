using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa.Common.DataAccess
{
    public static class ArcAddExtensions
    {

        /// <summary>
        /// Converts a List<string> into a List<LoanPrograms>
        /// </summary>
        /// <param name="loanPrograms">list of strings containing loan programs</param>
        /// <returns>list of loanprograms object</returns>
        public static IEnumerable<LoanPrograms> ToLoanProgramList(this IEnumerable<string> loanPrograms)
        {
            List<LoanPrograms> programs = new List<LoanPrograms>();
            foreach (string loan in loanPrograms)
            {
                programs.Add(new LoanPrograms() { LoanProgram = loan });
            }
            return programs;
        }

        /// <summary>
        /// Converts a List<int> to a List<LoanSequences>
        /// </summary>
        /// <param name="loanSequences">list of ints containing loan sequences</param>
        /// <returns>list of loansequences object</returns>
        public static IEnumerable<LoanSequences> ToLoanSequenceList(this IEnumerable<int> loanSequences)
        {
            List<LoanSequences> sequences = new List<LoanSequences>();
            foreach (int loan in loanSequences)
            {
                sequences.Add(new LoanSequences() { LoanSequence = loan });
            }
            return sequences;
        }
    }
}

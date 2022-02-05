using System.Collections.Generic;

namespace ACHSETUP
{
    public class TD22CommentData
    {
        public string Comment { get; set; }
        public List<int> Loans { get; set; }
        public bool PausePlease { get; set; }

        /// <summary>
        /// Constructor to be used when loan sequence numbers have been identified.
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="loans"></param>
        /// <param name="pausePlease"></param>
        public TD22CommentData(string comment, List<int> loans, bool pausePlease)
        {
            Comment = comment;
            Loans = loans;
            PausePlease = pausePlease;
        }

        /// <summary>
        /// Constructor to be used when no loan sequence numbers have been identified.
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="pausePlease"></param>
        public TD22CommentData(string comment, bool pausePlease)
        {
            Comment = comment;
            Loans = new List<int>();
            PausePlease = pausePlease;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Uheaa.Common
{
    /// <summary>
    /// Functionality for parsing a file with multiple ssn/loanseq combinations
    /// </summary>
    public class FileBorrower
    {
        public string AccountNumber { get; set; }
        public string SSN { get; set; }
        public List<int> LoanSequences { get; set; }
        public FileBorrower()
        {
            LoanSequences = new List<int>();
        }
        /// <summary>
        /// Parse the file at the given location in csv format and return a list of borrowers and their loan sequences.
        /// </summary>
        /// <param name="location">The location of the file.</param>
        /// <param name="delimiter">The field delimiter on each line.</param>
        /// <param name="getOtherIdentifier">Function to convert account identifier to ssn, or vice versa.</param>
        /// <returns></returns>
        public static IEnumerable<FileBorrower> ParseBorrowers(string location, string delimiter = ",", Func<string, string> getOtherIdentifier = null)
        {
            List<FileBorrower> results = new List<FileBorrower>();
            new CsvLineParser(location, delimiter, null, CsvLineParser.EmptyLineAndValueValidator, (line, parser) =>
            {
                string accountIdentifier = line.Content[0];
                int seq = int.Parse(line.Content[1]);
                FileBorrower borrower = null;
                if (accountIdentifier.Length == 9)
                    borrower = results.Where(r => r.SSN == accountIdentifier).SingleOrDefault();
                else
                {
                    borrower = results.Where(r => r.AccountNumber == accountIdentifier).SingleOrDefault();
                    if (borrower == null)
                    {
                        borrower = new FileBorrower();
                        if (accountIdentifier.Length == 9)
                        {
                            borrower.SSN = accountIdentifier;
                            if (getOtherIdentifier != null)
                                borrower.AccountNumber = getOtherIdentifier(borrower.SSN);
                        }
                        else
                        {
                            borrower.AccountNumber = accountIdentifier;
                            if (getOtherIdentifier != null)
                                borrower.SSN = getOtherIdentifier(borrower.AccountNumber);
                        }
                        results.Add(borrower);
                    }
                }
                borrower.LoanSequences.Add(seq);
            }).Parse();
            return results;
        }
    }
}

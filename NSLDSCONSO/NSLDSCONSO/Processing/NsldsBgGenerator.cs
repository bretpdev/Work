using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace NSLDSCONSO
{
    public class NsldsBgGenerator
    {
        ProcessLogRun plr;
        DataAccess da;
        private Action<string> logItem;
        private Action<string, Exception, NotificationSeverityType> logError;
        private bool allowUnreleasedLoans;
        private bool allowAlreadyUsedNsldsLabels;
        public NsldsBgGenerator(ProcessLogRun plr, DataAccess da, Action<string> logItem, Action<string, Exception, NotificationSeverityType> logError, bool allowUnreleasedLoans, bool allowAlreadyUsedNsldsLabels)
        {
            this.plr = plr;
            this.da = da;
            this.logItem = logItem;
            this.logError = logError;
            this.allowUnreleasedLoans = allowUnreleasedLoans;
            this.allowAlreadyUsedNsldsLabels = allowAlreadyUsedNsldsLabels;
        }

        public void Generate(string outputPath)
        {
            if (File.Exists(outputPath))
                File.Delete(outputPath);
            var parent = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(parent))
                Directory.CreateDirectory(parent);
            var data = da.GetBorrowersToReport();
            var nsldsLabels = da.GetAllReportedNsldsLabels();
            logItem("Found " + data.Count + " borrower(s) to process.");
            foreach (var borrower in data.OrderBy(o => o.BorrowerId).ToArray())
            {
                if (!borrower.HasReleasedLoans && !allowUnreleasedLoans)
                {
                    logError("Skipping borrower " + borrower.Ssn + ", borrower has no released loan(s).", null, NotificationSeverityType.Warning);
                    continue;
                }
                var details = da.GetBorrowerDetails(borrower.BorrowerId);
                decimal calculatedTotal = details.UnderlyingLoans.Sum(o => o.TotalAmount);
                if (calculatedTotal != details.MaxTotalAmount)
                {
                    logError(string.Format("Borrower {0} has a total amount of ${1}, but the combined value of mapped loans is ${2}.  Attempting to resolve with alternate mapping.",
                        borrower.Ssn, details.MaxTotalAmount, calculatedTotal), null, NotificationSeverityType.Warning);
                    details.UnderlyingLoans = details.AlternatelyCalculatedUnderlyingLoans;
                    calculatedTotal = details.UnderlyingLoans.Sum(o => o.TotalAmount);
                    if (calculatedTotal != details.MaxTotalAmount)
                        logError(string.Format("Borrower {0} has a total amount of ${1}, but the combined value of mapped loans is ${2}.  Please ensure there are no BorrowerUnderlyingLoan to BorrowerUnderlyingLoanFunding mapping issues.",
                            borrower.Ssn, details.MaxTotalAmount, calculatedTotal), null, NotificationSeverityType.Warning);
                }

                var mappedLoans = MapLoans(details, nsldsLabels, borrower);
                if (mappedLoans.Any() && mappedLoans.Count == details.UnderlyingLoans.Count)
                {
                    foreach (var record in mappedLoans)
                    {
                        var bcl = details.ConsolidationLoans.Single(o => o.NewLoanId == record.AwardId);
                        string line = "BG";
                        line += (record.AwardId ?? "").PadLeft(20, ' ');
                        line += (record.NsldsLabel ?? "").PadLeft(16, ' ');
                        line += ((int)Math.Ceiling(record.TotalAmount)).ToString().PadLeft(8, '0');
                        if (record.DateFunded.HasValue)
                            line += record.DateFunded.Value.ToString("yyyyMMdd");
                        else
                            line += "".PadRight(8, ' ');
                        line += TranslateGroupType(record.LoanType);
                        line += new string(' ', 174);
                        line += bcl.Fs10LoanSequence.ToString().PadLeft(4, '0');
                        line = line.PadRight(250, ' ');
                        File.AppendAllLines(outputPath, new string[] { line });
                    }
                    if (!da.MarkBorrowerAsReported(borrower.BorrowerId))
                        logError("Unable to mark BorrowerId " + borrower.BorrowerId + " as Reported to NSLDS.", null, NotificationSeverityType.Critical);
                }
                var loansForUpdate = mappedLoans.Select(record => new BorrowerUnderlyingLoanForUpdate() { BorrowerUnderlyingLoanId = record.BorrowerUnderlyingLoanId, NewLoanId = record.AwardId, NsldsLabel = record.NsldsLabel }).ToList();
                if (!da.UpdateBorrowerLoans(loansForUpdate))
                    logError("Unable to update loans for BorrowerId " + borrower.BorrowerId, null, NotificationSeverityType.Critical);
            }
            logItem("Finished generating " + outputPath);
        }

        private List<MappedBorrowerUnderlyingLoan> MapLoans(BorrowerDetails details, List<string> alreadyReportedNsldsLabels, ReportBorrower borrower)
        {
            details.Sanitize();
            List<MappedBorrowerUnderlyingLoan> results = new List<MappedBorrowerUnderlyingLoan>();
            if (!allowAlreadyUsedNsldsLabels)
                foreach (var grsp in details.Grsps.ToArray())
                    if (grsp.NsldsLabel.IsIn(alreadyReportedNsldsLabels.ToArray()))
                        details.Grsps.Remove(grsp); //can't work with already reported labels

            var availableGrsps = details.Grsps.ToArray().ToList();
            var availableGrs2s = details.Grs2s.ToArray().ToList();
            foreach (var bul in details.UnderlyingLoans.OrderBy(o => o.BorrowerUnderlyingLoanId))
            {
                if (string.IsNullOrEmpty(bul.NsldsLabel) || string.IsNullOrEmpty(bul.NewLoanId))
                {
                    BorrowerDetails.Grsp matchingGrsp = FindAndRemoveMatchingGrsp(bul, availableGrsps, availableGrs2s);
                    if (matchingGrsp != null)
                    {
                        bul.NsldsLabel = matchingGrsp.NsldsLabel;
                        foreach (var consol in details.ConsolidationLoans)
                        {
                            string newLoanId = consol.NewLoanId;
                            var char10 = newLoanId[9];
                            var match = NewLoanIdLoanProgramMatch(newLoanId, matchingGrsp.LoanProgram);
                            if (!match)
                                match = matchingGrsp.LoanProgram == "CL" && ((bul.LoanType == "J" && char10 == 'U') || (bul.LoanType == "O" && char10 == 'S'));
                            if (match)
                            {
                                bul.NewLoanId = newLoanId;
                                break;
                            }
                        }
                    }
                }
            }

            foreach (var bul in details.UnderlyingLoans)
            {
                if (string.IsNullOrWhiteSpace(bul.NewLoanId) || string.IsNullOrWhiteSpace(bul.NsldsLabel))
                {
                    var remainingGrsps = details.Grsps.ToArray().ToList();
                    var remainingGrs2s = details.Grs2s.ToArray().ToList();
                    while (remainingGrsps.Any() && (string.IsNullOrWhiteSpace(bul.NewLoanId) || string.IsNullOrWhiteSpace(bul.NsldsLabel)))
                    {
                        BorrowerDetails.Grsp matchingGrsp = FindAndRemoveMatchingGrsp(bul, remainingGrsps, remainingGrs2s);
                        if (matchingGrsp == null)
                            break;
                        foreach (var consol in details.ConsolidationLoans)
                        {
                            string newLoanId = consol.NewLoanId;
                            var char10 = newLoanId[9];
                            var match = matchingGrsp.LoanProgram == "CL" && ((bul.LoanType == "J" && char10 == 'U') || (bul.LoanType == "O" && char10 == 'S'));
                            if (match)
                            {
                                bul.NsldsLabel = matchingGrsp.NsldsLabel;
                                bul.NewLoanId = newLoanId;
                                break;
                            }
                        }
                    }
                }
            }

            foreach (var bul in details.UnderlyingLoans)
            {
                if (string.IsNullOrWhiteSpace(bul.NewLoanId) || string.IsNullOrWhiteSpace(bul.NsldsLabel))
                    logError(string.Format("No Award ID/NSLDS Label found or mapped for Borrower {0}, BorrowerId {1}, BorrowerUnderlyingLoanId {2}", borrower.Ssn, borrower.BorrowerId, bul.BorrowerUnderlyingLoanId), null, NotificationSeverityType.Critical);
                else
                    results.Add(new MappedBorrowerUnderlyingLoan()
                    {
                        AwardId = bul.NewLoanId,
                        NsldsLabel = bul.NsldsLabel,
                        BorrowerUnderlyingLoanId = bul.BorrowerUnderlyingLoanId,
                        DateFunded = bul.DateFunded,
                        LoanType = bul.LoanType,
                        TotalAmount = bul.TotalAmount
                    });
            }

            return results;
        }

        private BorrowerDetails.Grsp FindAndRemoveMatchingGrsp(BorrowerDetails.BorrowerUnderlyingLoan bul, List<BorrowerDetails.Grsp> availableGrsps, List<BorrowerDetails.Grs2> availableGrs2s)
        {
            foreach (var grsp in availableGrsps.ToArray())
                if (grsp.AwardId == bul.UnderlyingLoanId || grsp.AwardId + grsp.AwardSequence.ToString().PadLeft(3, '0') == bul.UnderlyingLoanId)
                {
                    availableGrsps.Remove(grsp);
                    return grsp;
                }

            var currentGrs2s = availableGrs2s.ToArray().ToList();
            while (availableGrs2s.Any())
            {
                BorrowerDetails.Grs2 matchingGrs2 = null;
                foreach (var grs2 in currentGrs2s.OrderBy(o => o.DisbursementSequence))
                    if (grs2.DisbursementDate >= bul.FirstDisbursement && grs2.DisbursementDate <= bul.FirstDisbursement.AddMonths(1))
                    {
                        matchingGrs2 = grs2;
                        break;
                    }
                if (matchingGrs2 == null) //expand search criteria back one week
                    foreach (var grs2 in currentGrs2s.OrderBy(o => o.DisbursementSequence))
                        if (grs2.DisbursementDate?.AddDays(7) >= bul.FirstDisbursement && grs2.DisbursementDate <= bul.FirstDisbursement.AddMonths(1))
                        {
                            matchingGrs2 = grs2;
                            break;
                        }
                if (matchingGrs2 == null)
                    break; //couldn't find a match
                foreach (var grsp in availableGrsps.ToArray())
                {
                    if (matchingGrs2.NsldsLabel == grsp.NsldsLabel)
                    {
                        if (LoanTypeLoanProgramMatch(bul.LoanType, grsp.LoanProgram))
                        {
                            availableGrsps.Remove(grsp);
                            currentGrs2s.Remove(matchingGrs2);
                            return grsp;
                        }
                    }
                }
                currentGrs2s.Remove(matchingGrs2);
            }
            return null;
        }

        private string TranslateGroupType(string original)
        {
            original = original.Trim();
            if (original.IsIn("A", "C", "D", "E", "O", "0", "9"))
                return "SUBS";
            if (original.IsIn("G", "H", "J", "K", "L"))
                return "USUB";
            if (original.IsIn("T", "U", "V"))
                return "PPLU";
            if (original.IsIn("I", "S"))
                return "GPLU";
            if (original.IsIn("M", "F"))
                return "CAMP";
            return "OTHR";
        }

        private bool LoanTypeLoanProgramMatch(string loanType, string loanProgram)
        {
            switch (loanType)
            {
                case "A":
                    return loanProgram.IsIn("D0", "D1", "SF");
                case "C":
                    return loanProgram.IsIn("FI");
                case "D":
                    return loanProgram.IsIn("D0", "D1", "SF");
                case "E":
                    return loanProgram.IsIn("D6", "D9");
                case "F":
                    return loanProgram.IsIn("PU", "EU");
                case "G":
                    return loanProgram.IsIn("EU", "PU", "SU", "D2", "D8");
                case "H":
                    return loanProgram.IsIn("SL");
                case "I":
                    return loanProgram.IsIn("D3", "GB");
                case "J":
                    return loanProgram.IsIn("CL", "D5", "EU", "PU", "SU");
                case "K":
                    return loanProgram.IsIn("D5", "EU", "PU", "SU");
                case "L":
                    return loanProgram.IsIn("D2", "D8");
                case "M":
                    return loanProgram.IsIn("NU");
                case "N":
                    return loanProgram.IsIn("DU");
                case "O":
                    return loanProgram.IsIn("SF", "D0", "D6", "D9", "CL");
                case "S":
                    return loanProgram.IsIn("D3", "GB");
                case "T":
                    return loanProgram.IsIn("PL", "D4");
                case "U":
                    return loanProgram.IsIn("PL", "D4");
                case "V":
                    return loanProgram.IsIn("D7");
                case "0":
                    return loanProgram.IsIn("D0", "D1", "SF");
                case "9":
                    return loanProgram.IsIn("SF", "D6", "D9");
            }
            return false;
        }

        private bool NewLoanIdLoanProgramMatch(string newLoanId, string loanProgram)
        {
            var char10 = newLoanId[9];
            switch (char10)
            {
                case 'P':
                    return loanProgram.IsIn("D4", "D7", "PL", "D3", "GB");
                case 'U':
                    return loanProgram.IsIn("D4", "D7", "PL", "D3", "GB", "D2", "D5", "D8", "SU", "SL", "PU", "EU", "NU");
                case 'S':
                    return loanProgram.IsIn("D0", "D1", "D6", "D9", "SF", "FI");
            }
            return false;
        }
    }
}

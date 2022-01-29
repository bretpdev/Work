using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using Uheaa.Common.ProcessLogger;
using System.IO;
using Uheaa.Common;

namespace NSLDSCONSO
{
    public class DataHistoryParser : IDisposable
    {
        private DataAccess da;
        private string zipPath;
        private Action<string> logItem;
        private Action<string, Exception, NotificationSeverityType> logError;
        public int DataLoadRunId { get; private set; }
        List<Borrower> borrowers;
        public int TotalBorrowerCount { get { return borrowers.Count; } }
        public List<string> ProcessedBorrowers { get; private set; } = new List<string>();
        public DataHistoryParser(DataAccess da, string zipPath, Action<string> logItem, Action<string, Exception, NotificationSeverityType> logError, DataLoadRun recovery = null)
        {
            this.da = da;
            this.zipPath = zipPath;
            this.logItem = logItem;
            this.logError = logError;
            using (var zip = ZipFile.Open(zipPath, ZipArchiveMode.Read))
            {
                var indexFile = zip.Entries.Where(o => o.FullName.ToLower().EndsWith(".idx"));
                if (!indexFile.Any())
                {
                    logError("No index file found, cannot process.", null, NotificationSeverityType.Critical);
                    return;
                }
                else if (indexFile.Count() != 1)
                {
                    logError("Multiple index files found, cannot process.", null, NotificationSeverityType.Critical);
                    return;
                }
                this.borrowers = ParseIndex(indexFile.Single());
            }

            DataLoadRunId = da.StartDataLoadRun(this.borrowers.Count, zipPath);
            if (recovery != null)
            {
                ProcessedBorrowers = da.RecoveryMigration(recovery.DataLoadRunId, DataLoadRunId);
                logItem(string.Format("Recovery: Migrated {0} borrowers from DataLoadRun {1} to DataLoadRun {2}.  Uploading remaining borrowers.", ProcessedBorrowers.Count, recovery.DataLoadRunId, DataLoadRunId));
            }
        }

        public IEnumerable<Borrower> ParseFile()
        {
            if (this.borrowers == null)
                yield break;
            using (var zip = ZipFile.Open(zipPath, ZipArchiveMode.Read))
            {
                foreach (var borrower in borrowers)
                {
                    if (ProcessedBorrowers.Contains(borrower.Ssn))
                        continue;
                    var borrowerFile = zip.Entries.Where(o => o.FullName.ToLower().EndsWith(borrower.FileName.ToLower()));
                    if (!borrowerFile.Any())
                    {
                        logItem(string.Format("Couldn't find file for entry {0}", borrower.Ssn));
                        continue;
                    }
                    else if (borrowerFile.Count() > 1)
                    {
                        logItem(string.Format("Found multiple files for entry {0}", borrower.Ssn));
                        continue;
                    }
                    LoadBorrower(borrower, borrowerFile.Single());
                    ProcessedBorrowers.Add(borrower.Ssn);
                    yield return borrower;
                }
            }
        }

        private List<Borrower> ParseIndex(ZipArchiveEntry indexFile)
        {
            List<Borrower> results = new List<Borrower>();
            using (var reader = new StreamReader(indexFile.Open()))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    var b = new Borrower();
                    var splitLine = line.Split('|');
                    b.Ssn = splitLine[0];
                    b.Name = splitLine[2] + " " + splitLine[1];
                    b.FileName = splitLine[10].Trim();
                    if (b.FileName.ToLower().EndsWith(".txt") && splitLine[3].ToLower() == "payh")
                        results.Add(b);  //we can't process the .pdf versions and we only want the PAYH records
                }
            }
            return results;
        }

        private bool LoadBorrower(Borrower borrower, ZipArchiveEntry infoFile)
        {
            List<Borrower> results = new List<Borrower>();
            using (var reader = new StreamReader(infoFile.Open()))
            {
                string contents = reader.ReadToEnd();
                var sections = new ReportParser(contents);
                var csvBorrower = GetEntities<CsvBorrower>(sections.Section1);
                if (csvBorrower == null)
                    return false;
                borrower.Name = csvBorrower.Single().Name.Trim();
                borrower.DateOfBirth = csvBorrower.Single().DateOfBirth;
                borrower.ConsolidationLoans = GetEntities<BorrowerConsolidationLoan>(sections.Section2);
                borrower.UnderlyingLoans = GetEntities<BorrowerUnderlyingLoan>(sections.Section3);
                borrower.UnderlyingLoanFunding = GetEntities<BorrowerUnderlyingLoanFunding>(sections.Section4);
                if (borrower.ConsolidationLoans == null || borrower.UnderlyingLoans == null || borrower.UnderlyingLoanFunding == null)
                    return false;

                foreach (var underlyingLoan in borrower.UnderlyingLoans)
                    underlyingLoan.NewLoanId = FindNewLoanId(underlyingLoan.UnderlyingLoanId, borrower.ConsolidationLoans);
            }
            return true;
        }

        private string FindNewLoanId(string oldLoanId, List<BorrowerConsolidationLoan> consolidationLoans)
        {
            char oldChar = oldLoanId[10];
            foreach (var consol in consolidationLoans)
            {
                char consolChar = consol.NewLoanId[10];
                if (consolChar == 'P' && oldChar.IsIn('U', 'P'))
                    return consol.NewLoanId;
                if (consolChar == 'S' && oldChar == 'S')
                    return consol.NewLoanId;
                if (consolChar == 'U' && oldChar == 'H')
                    return consol.NewLoanId;
            }
            return null;
        }

        private List<T> GetEntities<T>(List<string> sectionLines) where T : new()
        {
            var result = CsvHelper.ParseTo<T>(sectionLines.ToArray());
            if (result.HasErrors)
            {
                var errorMessage = result.GenerateErrorMessage();
                logError(errorMessage, null, NotificationSeverityType.Critical);
                return null;
            }
            return result.ValidLines.Select(o => o.ParsedEntity).ToList();
        }

        public void Dispose()
        {
            da.EndDataLoadRun(DataLoadRunId);
        }
    }
}

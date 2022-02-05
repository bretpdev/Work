oousing System;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace DMCSNTFYFE
{
    public class DmcsApprovalNotify
    {
        private List<string> FilePatterns { get; set; }
        private string scriptId;
        private DataAccess DA;
        private ProcessLogRun logRun;
        private RecoveryLog Recovery;
        private BatchProcessingHelper bph;
        private ReflectionInterface ri;

        public DmcsApprovalNotify(ReflectionInterface Ri, BatchProcessingHelper Helper, ProcessLogRun LogRun, string Script)
            
        {
            ri = Ri;
            bph = Helper;
            logRun = LogRun;
            scriptId = Script;
            Recovery = new RecoveryLog(scriptId);
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            FilePatterns = new List<string>() { "UNWS62.NWS62R3.*", "UNWS62.NWS62R2.*" };
            DA = new DataAccess(logRun.ProcessLogId);
        }

        private void DoRecovery()
        {
            string file = Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[2];
            ProcessFile(file);
        }

        public int Process()
        {
            
            if (!Recovery.RecoveryValue.IsNullOrEmpty())
                DoRecovery();

            foreach (string filePattern in FilePatterns)
            {
                string fileName = MultipleFiles(EnterpriseFileSystem.FtpFolder, false, filePattern);
                if (fileName == string.Empty)
                    continue;

                ProcessFile(fileName);
            }
            Console.WriteLine("All files processed.");
            Console.WriteLine("Deleting recovery file.");
            Recovery.Delete();
            return 0;
        }

        private void ProcessFile(string fileName)
        {
            List<BorrowerData> fileData = LoadBorrowerData(fileName);
            if (fileData.Count != 0)
            {
                PrintAndArchive(fileData, fileName, fileName.Contains("R2"));

                if (File.Exists(fileName))
                    File.Delete(fileName);
            }
        }

        private string MultipleFiles(string path, bool hasExtension, string searchPattern)
        {

            string folder = path.Substring(0, path.Length - (path.Length - path.LastIndexOf("\\") - 1));
            string extension = hasExtension ? path.Substring(path.LastIndexOf(".")) : "";
            string fileName = searchPattern == "" ? path.Substring(path.LastIndexOf("\\") + 1, path.Length - path.LastIndexOf("\\") - 1) : searchPattern;
            Console.WriteLine($"Looking for files: { fileName }.");
            IEnumerable<string> files;
            if (hasExtension)
            {
                fileName = fileName.Remove(fileName.LastIndexOf("."));
                files = Directory.GetFiles(folder, extension).Where(p => p.ToString().Contains(fileName));
            }
            else
                files = Directory.GetFiles(folder, fileName);

            if (files.Count() == 0)
            {
                logRun.AddNotification($"Search pattern {searchPattern} finds zero files.", NotificationType.NoFile, NotificationSeverityType.Informational);
                return string.Empty;
            }

            if (files.Count() > 1)
            {
                logRun.AddNotification($"Search pattern {searchPattern} finds multiple files.", NotificationType.Other, NotificationSeverityType.Informational);
                return string.Empty;
            }
            
            return files.First();
        }

        /// <summary>
        /// Load the data file into a BorrowerData list
        /// </summary>
        /// <param name="file">The data file</param>
        /// <returns>List of BorrowerData</returns>
        private List<BorrowerData> LoadBorrowerData(string file)
        {
            List<BorrowerData> data = new List<BorrowerData>();
            data.Clear();
            using (StreamReader sr = new StreamReader(file))
            {
                if (sr.EndOfStream)
                {
                    logRun.AddNotification($"File {file} finds empty file.", NotificationType.EmptyFile, NotificationSeverityType.Informational);
                    return data;
                }


                string header = sr.ReadLine();
                string borAcct = string.Empty;
                BorrowerData bor = new BorrowerData();

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.IsNullOrEmpty())
                        continue;

                    List<string> split = line.SplitAndRemoveQuotes(",");

                    if (split[0] == borAcct)
                    {
                        bor.LoanSeq.Add(split[10].ToInt());
                        continue;
                    }
                    else if (split[0] != borAcct && !bor.AccountNumber.IsNullOrEmpty())
                    {
                        data.Add(bor);
                    }

                    bor = BorrowerData.Populate(split);
                    if (bor == null)
                    {
                        logRun.AddNotification($"There was missing data in the following line: \r\n {line} /r/n Please review.",
                        NotificationType.ErrorReport, NotificationSeverityType.Informational);
                        bor = new BorrowerData();
                    }

                    borAcct = split[0];

                    if (string.IsNullOrEmpty(bor.AccountNumber) && (!string.IsNullOrEmpty(bor.FirstName) && !string.IsNullOrEmpty(bor.LastName)))
                    {
                        logRun.AddNotification($"There was data missing in the file for the following borrower: Account Number: {bor.AccountNumber} Borrowers Name: {bor.FirstName} {bor.LastName}.",
                        NotificationType.ErrorReport, NotificationSeverityType.Informational);
                    }
                }
                if (bor.AccountNumber != null)
                    data.Add(bor);
            }
            return data;
        }

        /// <summary>
        /// Prints and archives letters and comments borrowers account
        /// </summary>
        /// <param name="fileData">List of BorrowerData objects</param>
        private void PrintAndArchive(List<BorrowerData> fileData, string file, bool invalidAddress = false)
        {
            string archived = "archived";
            string commented = "commented";
            string coborrowed = "coborrowed";

            //Set the recovery counter and the recovery step
            int counter = string.IsNullOrEmpty(Recovery.RecoveryValue) ? 0 : int.Parse(Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[0]);
            string recStep = string.IsNullOrEmpty(Recovery.RecoveryValue) ? "" : Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[1];

            if (recStep == coborrowed)
                recStep = "";

            foreach (BorrowerData bor in fileData.Skip(counter))
            {
                string borrowerData = "";
                foreach (string line in CsvHelper.GetHeaderAndLineFromObject(bor, ","))
                    borrowerData = line;

                try
                {
                    SystemBorrowerDemographics demos = ri.GetDemographicsFromTx1j(bor.AccountNumber);
                    if (invalidAddress)
                    {
                        // Process the borrower to central printing.
                        Console.WriteLine($"Invalid address, manually creating file for { bor.AccountNumber }.");
                        string printingFile = EnterpriseFileSystem.TempFolder + "ApprovalDataFile.txt"; 
                        WriteFile(bor, printingFile);
                        if (string.IsNullOrEmpty(recStep))
                        {
                            EcorrProcessing.EcorrCentralizedPrintingAndImage(demos.AccountNumber, demos.Ssn, "LDMCSTNFED", printingFile, Environment.UserName, scriptId,
                                "AccountNumber", "State", demos, logRun.ProcessLogId, invalidAddress, "CRARC");

                            Recovery.RecoveryValue = $"{counter},{archived},{file},{""}";
                        }
                        if (File.Exists(printingFile))
                            File.Delete(printingFile);
                    }
                    else
                    {
                        // Add letter into FEDCORPRT the letter, this is the new way.
                        Console.WriteLine($"Add record to print Processing for {bor.AccountNumber}.");
                        EcorrProcessing.AddRecordToPrintProcessing("DMCSNTFYFE", "LDMCSTNFED", borrowerData, bor.AccountNumber, "MA4481");
                        Recovery.RecoveryValue = $"{counter},{archived},{file}";
                        recStep = archived;
                    }

                    if (string.IsNullOrEmpty(recStep) || recStep == archived)
                    {
                        recStep = AddComment(commented, counter, recStep, bor, file, true, bor.LoanSeq[0]);
                        Recovery.RecoveryValue = $"{counter},{recStep},{file},{""}";
                    }

                    // Check for co borrower for both the R2 and R3 SAS files.
                    string startingIndex = string.IsNullOrEmpty(Recovery.RecoveryValue) ? "0" : Recovery.RecoveryValue.SplitAndRemoveQuotes(",")[3];
                    int startIndex = startingIndex.ToInt();

                    for (int n = startIndex; n < bor.LoanSeq.Count; n++)
                    {
                        string lfEds = DA.CoBorrowerExists(demos.Ssn, bor.LoanSeq[n], 'M');
                       if (lfEds.IsPopulated())
                        {
                            recStep = "coborrowers";
                            Console.WriteLine($"Add record to print Processing for co Borrower {lfEds}.");
                            ProcessCoBorrower(lfEds, bor.KeyLine, invalidAddress, file, recStep, demos.AccountNumber, bor.LoanSeq[n], borrowerData);
                            // Drop the Arc
                            // Since no co-borrower, add comment to borrower account.
                            recStep = AddComment(recStep, counter, recStep, bor, file, false, bor.LoanSeq[n]);
                            if (recStep == "")
                               return;
                            Recovery.RecoveryValue = $"{counter},{recStep},{file},{n + 1}";
                        }

                    }
                    recStep = string.Empty;
                    Recovery.RecoveryValue = $"{++counter},{recStep},{file},{""}";

                }
                catch (Exception ex)
                {
                    logRun.AddNotification($"Error occured on account {bor.AccountNumber}, ex: {ex.Message}.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
            }//foreach
        }

        private void ProcessCoBorrower(string ln20Eds, string keyline, bool addressInvalid, string file, string recStep, string accountNumber, int sequence, string letterData)
        {
            BorrowerData borrower = new BorrowerData();
            SystemBorrowerDemographics sbdCoBorr;
            BorrowerData dbCoBorr;

            borrower.AccountNumber = accountNumber;
            borrower.LoanSeq.Add(sequence);
            sbdCoBorr = ri.GetDemographicsFromTx1j(ln20Eds);
            dbCoBorr = translate(sbdCoBorr, keyline);
            if (addressInvalid)
            {
                string coBorFile = EnterpriseFileSystem.TempFolder + "CoBorrowerFile.txt";
                WriteFile(dbCoBorr, coBorFile);
                EcorrProcessing.EcorrCentralizedPrintingAndImage(sbdCoBorr.AccountNumber, sbdCoBorr.Ssn, "LDMCSTNFED", coBorFile, Environment.UserName, scriptId,
                                    "AccountNumber", "State", sbdCoBorr, logRun.ProcessLogId, addressInvalid, "CRARC");

                if (File.Exists(coBorFile))
                    File.Delete(coBorFile);
            }
            else
            {
                EcorrProcessing.AddCoBwrRecordToPrintProcessing("DMCSNTFYFE", "LDMCSTNFED", letterData, sbdCoBorr.AccountNumber, "MA4481",sbdCoBorr.Ssn);
            }

        }
        BorrowerData translate(SystemBorrowerDemographics input, string keyline)
        {
            return new BorrowerData
            {
                AccountNumber =  input.AccountNumber,
                KeyLine = keyline,
                FirstName = input.FirstName,
                LastName = input.LastName,
                Address1 = input.Address1,
                Address2 = input.Address2,
                City = input.City,
                State = input.State,
                Zip = input.ZipCode,
                Country = input.Country

            };
        }


        /// <summary>
        /// Adds a comment to ATD22
        /// </summary>
        /// <param name="commented">Recovery Values</param>
        /// <param name="counter">Line counter</param>
        /// <param name="recStep">Current recovery step</param>
        /// <param name="bor">Current Borrower</param>
        /// <returns>Recovery Step.</returns>
        private string AddComment(string commented, int counter, string recStep, BorrowerData bor, string file, bool isBorrower, int seq)
        {
            List<int> loanSeqs = new List<int>();
            string returnVal = recStep;
            loanSeqs.Add(seq);
            List<string> loanPrgms = new List<string>();
            string com = isBorrower == true ? "Borrower notified loans transferred to DMCS." : "CoBorrower notified loans transferred to DMCS";
            ArcData arc = new ArcData(DataAccessHelper.Region.CornerStone)
            {
                Arc = "DMCSL",
                Comment = com,
                LoanSequences = loanSeqs,
                LoanPrograms = loanPrgms,
                AccountNumber = bor.AccountNumber,
                ArcTypeSelected = ArcData.ArcType.Atd22ByLoan,
                ScriptId = scriptId

            };
            ArcAddResults result = arc.AddArc();
            
            if (!result.ArcAdded)
            {
                logRun.AddNotification($"An error occurred while adding an activity comment for the following borrowers account: Account Number: {bor.AccountNumber} Borrowers Name: {bor.FirstName} {bor.LastName} Error: {result.Ex.Message}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Recovery.RecoveryValue = $"{counter},{recStep},{file}";
            }
            else
            {
                Console.WriteLine("Arc Added ");
                Recovery.RecoveryValue = $"{counter},{commented},{file}";
                returnVal = commented;
            }
            return returnVal;
        }

        /// <summary>
        /// Writes out to a given file.
        /// </summary>
        /// <param name="bor">Borrower Object</param>
        /// <param name="printingFile">FIle to write out.</param>
        private static void WriteFile(BorrowerData bor, string printingFile)
        {
            using (StreamWriter sw = new StreamWriter(printingFile))
            {
                foreach (string line in CsvHelper.GetHeaderAndLineFromObject(bor, ","))
                    sw.WriteLine(line);
            }
        }

    }//class
}//namespace

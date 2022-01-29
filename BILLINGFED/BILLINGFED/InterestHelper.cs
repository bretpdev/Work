using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace BILLINGFED
{
    public class InterestHelper
    {
        public ReaderWriterLockSlim IntNoticeLock { get; set; }
        public ReaderWriterLockSlim IntStatementLock { get; set; }
        public ReaderWriterLockSlim IntStatementImagingLock { get; set; }
        public ReaderWriterLockSlim IntNoticeImagingLock { get; set; }
        public string IntStatementFile { get; set; }
        public string IntStatementDataFile { get; set; }
        public string IntNoticeDataFile { get; set; }
        public string ScriptId { get; set; }
        public string AccountNumber { get; set; }
        public string CoBorrowerAccountNumber { get; set; }
        public DataAccess Da { get; set; }
        private ProcessLogRun LogRun { get; set; }
        public List<string> PrintFileData { get; set; }

        public InterestHelper(ProcessLogRun logRun)
        {
            IntNoticeLock = new ReaderWriterLockSlim();
            IntNoticeImagingLock = new ReaderWriterLockSlim();
            IntStatementLock = new ReaderWriterLockSlim();
            IntStatementImagingLock = new ReaderWriterLockSlim();
            PrintFileData = new List<string>();

            LogRun = logRun;
        }

        /// <summary>
        /// Write the interest notice data to the interest notice file
        /// </summary>
        public void AddToInterestNoticeFile(string line)
        {
            try
            {
                IntNoticeLock.EnterWriteLock();
                //using (StreamWriter sw = new StreamWriter(IntNoticeFile, true))
                //    sw.WriteLine(line);
                PrintFileData.Add(line);
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("Error writing data to the Interest Notice file", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
            }
            finally
            {
                IntNoticeLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Write the interest statement data to the interest statement file
        /// </summary>
        public void AddToInterestStatementFile(string line)
        {
            try
            {
                IntStatementLock.EnterWriteLock();
                using (StreamW sw = new StreamW(IntStatementFile, true))
                    sw.WriteLine(line);
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("There was an error writing data to the Interest Statement file", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
            }
            finally
            {
                IntStatementLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Creates a data file to be used to image interest statements
        /// </summary>
        public void CreateIntStatementDataFile(Borrower bor, List<InterestStatementData> iData)
        {
            try
            {
                IntStatementImagingLock.EnterWriteLock();
                InterestStatementData data = iData[0];
                using (StreamW sw = new StreamW(IntStatementDataFile, false))
                {
                    sw.WriteLine(GetInterestStatementHeader());
                    sw.WriteLine(InterestStatement.CreatePrintingFileLine(iData, bor.PrintProcessingId));
                }
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("There was an error writing data to the Interest Statement data file", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
            }
            finally
            {
                IntStatementImagingLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Creates a data file to be used to image interest notices
        /// </summary>
        public void CreateIntNoticeDataFile(Borrower bor, List<InterestNoticeData> iData)
        {
            try
            {
                IntNoticeImagingLock.EnterWriteLock();
                InterestNoticeData data = iData[0];
                using (StreamW sw = new StreamW(IntNoticeDataFile, false))
                {
                    sw.WriteLine(GetInterestNoticeHeader());
                    sw.WriteLine(InterestNotice.CreatePrintingFileLine(iData, bor.PrintProcessingId));
                }
            }
            catch (Exception ex)
            {
                LogRun.AddNotification("There was an error writing data to the Interest Notice data file", NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
            }
            finally
            {
                IntNoticeImagingLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Gets all the header information for the Interest Notices
        /// </summary>
        public static string GetInterestNoticeHeader()
        {
            List<string> header = new List<string>()
            {
                "PrintProcessingId","KeyLine","FirstName","LastName","Address1","Address2","City","State","ZIP", "Country","AccountNumber"
            };

            for (int row = 1; row <= 30; row++)
            {
                header.AddRange(new List<string>() { "DisbDate" + row, "LoanPgm" + row, "IntLast" + row, "IntTotal" + row, "CurrPrin" + row });
            }
            return string.Join(",", header);
        }

        /// <summary>
        /// Gets all the header information for the Interest Statements
        /// </summary>
        /// <returns></returns>
        public string GetInterestStatementHeader()
        {
            List<string> header = new List<string>()
            {
                "PrintProcessingId","KeyLine","FirstName","LastName","Address1","Address2","City","State","ZIP","Country","AccountNumber", "CoBorrowerAccountNumber","LD_BIL_CRT","LD_BIL_DU","LD_FAT_EFF","ACCT_LA_FAT_CUR_PRI","ACCT_LA_FAT_NSI","ACCT_TAP","LA_BIL_PAS_DU","LA_BIL_DU_PRT","LA_CUR_INT_DU"
            };

            for (int row = 1; row <= 30; row++)
                header.AddRange(new List<string>() { "LoanProgram" + row, "DisbDate" + row, "IntRate" + row, "OrigPrin" + row, "CurrBal" + row });

            return string.Join(",", header);
        }
    }
}
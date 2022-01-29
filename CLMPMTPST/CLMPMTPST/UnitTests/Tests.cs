using CLMPMTPST;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace UnitTests
{
    [TestClass]
    public class Tests
    {
        private static string SAS_FILE_FULL = @"X:\PADD\FTP\Test\ULWR10.LWR10R2_TEST";
        private static string SAS_FILE_NAME = "ULWR10.LWR10R2_TEST";
        private static readonly string ScriptId = "CLMPMTPST";
        List<TestAccount> TestAccounts = new List<TestAccount>();
        ReflectionInterface RI { get; set; }
        ProcessLogRun PLR { get; set; }

        [ClassInitialize]
        public static void InitClass(TestContext context)
        {
            string secondFile = $"{SAS_FILE_FULL}2";
            if (File.Exists(secondFile))
                File.Delete(secondFile);
        }

        [TestInitialize]
        public void InitTest()
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (PLR == null)
                PLR = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, true);
            if (RI == null)
            {
                RI = new ReflectionInterface();
                BatchProcessingHelper helper = BatchProcessingLoginHelper.Login(PLR, RI, ScriptId, "BatchUheaa");
            }

            if (!File.Exists(SAS_FILE_FULL)) // If file already exists, don't need to create a new one
            {
                CreateTestFile(SAS_FILE_FULL);
            }

            string secondFile = $"{SAS_FILE_FULL}2";
            if (File.Exists(secondFile))
            {
                Repeater.TryRepeatedly(() =>
                {
                    File.Delete(secondFile);
                }, 5, 3000);
            }

            string emptyFile = $"{SAS_FILE_FULL}_EMPTY";
            if (File.Exists(emptyFile))
            {
                Repeater.TryRepeatedly(() =>
                {
                    File.Delete(emptyFile); // Delete empty file
                }, 5, 3000);
            }
        }

        [TestMethod]
        public void Test1_TestFileCreated()
        {
            Assert.IsTrue(File.Exists(SAS_FILE_FULL));
        }

        [TestMethod]
        public void Test2_FindsSasFile_Found()
        {
            LppClaimPaymentPosting lppTest = new LppClaimPaymentPosting(RI);
            PrivateObject po = new PrivateObject(lppTest);
            object[] args = new object[] { SAS_FILE_NAME };
            FileSearchResult fsr = (FileSearchResult)po.Invoke("FindSasFile", args);
            bool result = true;
            result &= fsr.FileFound && (SAS_FILE_FULL == fsr.FileName) && string.IsNullOrEmpty(fsr.Error);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Test5_FindsSasFile_Missing()
        {
            if (File.Exists(SAS_FILE_FULL))
                File.Delete(SAS_FILE_FULL);

            LppClaimPaymentPosting lppTest = new LppClaimPaymentPosting(RI);
            PrivateObject po = new PrivateObject(lppTest);
            object[] args = new object[] { SAS_FILE_NAME };
            FileSearchResult fsr = (FileSearchResult)po.Invoke("FindSasFile", args);
            bool result = true;
            string FILE_MISSING = "The Claim Payment file is missing. Please contact Systems Support for assistance.";
            result &= fsr != null && !fsr.FileFound && !string.IsNullOrEmpty(fsr.Error) && fsr.Error == FILE_MISSING;

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Test4_FindsSasFile_Empty()
        {
            string emptyFile = $"{SAS_FILE_FULL}_EMPTY";
            if (!File.Exists(emptyFile))
                File.Create(emptyFile); // Create empty file

            if (File.Exists(SAS_FILE_FULL)) // Remove base file so that we don't multiple files during test run
                File.Delete(SAS_FILE_FULL);

            LppClaimPaymentPosting lppTest = new LppClaimPaymentPosting(RI);
            PrivateObject po = new PrivateObject(lppTest);
            object[] args = new object[] { emptyFile };
            FileSearchResult fsr = (FileSearchResult)po.Invoke("FindSasFile", args);
            bool result = true;
            string FILE_EMPTY = "The Claim Payment file is empty. Please contact Systems Support for assistance.";
            result &= fsr != null && !fsr.FileFound && !string.IsNullOrEmpty(fsr.Error) && fsr.Error == FILE_EMPTY;
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Test6_FindsSasFile_Multiple()
        {
            if (!File.Exists(SAS_FILE_FULL))
                File.Create(SAS_FILE_FULL); // Create

            string secondFile = $"{SAS_FILE_FULL}2";
            File.Create(secondFile); // Create second file

            LppClaimPaymentPosting lppTest = new LppClaimPaymentPosting(RI);
            PrivateObject po = new PrivateObject(lppTest);
            object[] args = new object[] { SAS_FILE_NAME };
            FileSearchResult fsr = (FileSearchResult)po.Invoke("FindSasFile", args);
            bool result = true;
            string FILE_MULTIPLE = "Multiple files exist. Please review the old file.";
            result &= fsr != null && !fsr.FileFound && !string.IsNullOrEmpty(fsr.Error) && fsr.Error == FILE_MULTIPLE;

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Test3_ReadFile_Success()
        {
            FileReader fileReader = new FileReader(PLR);
            PrivateObject po = new PrivateObject(fileReader);
            object[] args = new object[] { SAS_FILE_FULL };
            List<Payment> payments = (List<Payment>)po.Invoke("ReadFile", args);
            bool result = true;
            result &= payments != null && payments.Count == 10;
            foreach (Payment pmt in payments)
            {
                result &= pmt.PaymentAmount == 1000.00 && pmt.LoanSequences != null && pmt.LoanSequences.Count() == 2 && pmt.LoanSequences.Contains(1) && pmt.LoanSequences.Contains(2) && !string.IsNullOrEmpty(pmt.LastName);
            }

            Assert.AreEqual(true, result);
        }



        private void CreateTestFile(string fileName)
        {
            TestAccounts = GetTenTestAccounts();
            try
            {
                using (StreamWriter sw = new StreamWriter(fileName, true)) // Write out test data to test file
                {
                    sw.WriteCommaDelimitedLine(true, "BF_SSN", "POAMT", "EFF_DATE", "GUAR_CODE", "LN_SEQ", "DM_PRS_LST");
                    foreach (TestAccount ta in TestAccounts)
                    {
                        double amt = 1000.00;
                        List<int> seqs = new List<int>() { 1, 2 };
                        string seqsCombined = seqs.Aggregate("", (current, next) => current + " " + next).Trim();
                        Payment pmt = new Payment(ta.Ssn, amt, DateTime.Now.Date.AddDays(-1), "000749", seqs, ta.LastName);
                        sw.WriteCommaDelimitedLine(true, pmt.Ssn, amt.ToString("##.00"), pmt.EffectiveDate.ToString(Payment.DATE_FORMAT), pmt.GuarantorCode, seqsCombined, pmt.LastName);

                    }
                }
            }
            catch (Exception ex)
            {
                PLR.AddNotification("Hit an exception trying to write out the test file", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
            }
        }

        private List<TestAccount> GetTenTestAccounts()
        {
            List<TestAccount> testAccounts = new List<TestAccount>();
            int ssnSeed = 450;

            while (testAccounts.Count() < 10)
                GetAccountsWithSeed(testAccounts, ssnSeed++);

            return testAccounts;
        }

        private void GetAccountsWithSeed(List<TestAccount> testAccounts, int ssnSeed)
        {
            RI.FastPath($"TX3Z/ITX1JB;{ssnSeed}*");
            int page = 1;
            int row = 6;
            while (testAccounts.Count < 10 && RI.MessageCode != "90007")
            {
                if (RI.GetText(row, 3, 2).IsNumeric())
                {
                    RI.PutText(22, 12, RI.GetText(row, 3, 2), ReflectionInterface.Key.Enter);
                    if (RI.GetText(1, 71, 5) == "TXX1R")
                    {
                        string ssn = RI.GetText(1, 11, 9);
                        RI.FastPath($"TX3Z/ITS24{ssn}");
                        if (RI.GetText(21, 20, 50).Contains("DEFERMENT") || RI.GetText(21, 20, 50).Contains("REPAYMENT"))
                        {
                            TestAccount ta = new TestAccount(ssn, RI.GetText(4, 37, 40).Split(' ').Last());
                            testAccounts.Add(ta);
                        }
                        RI.FastPath($"TX3Z/ITX1JB;{ssnSeed}*");
                        for (int pageNum = 1; pageNum < page; pageNum++)
                            RI.Hit(ReflectionInterface.Key.F8);
                    }
                    row += 2;
                    if (row > 20)
                    {
                        RI.Hit(ReflectionInterface.Key.F8);
                        page++;
                        row = 6;
                    }
                }
                else
                {
                    RI.Hit(ReflectionInterface.Key.F8);
                    page++;
                    row = 6;
                }
            }
        }

        [TestCleanup]
        public void CleanUp()
        {
            RI.CloseSession();
            string secondFile = $"{SAS_FILE_FULL}2";

            if (File.Exists(secondFile))
            {
                Repeater.TryRepeatedly(() =>
                {
                    File.Delete(secondFile);
                }, 5, 3000);
            }

            string emptyFile = $"{SAS_FILE_FULL}_EMPTY";
            if (File.Exists(emptyFile))
            {
                Repeater.TryRepeatedly(() =>
                {
                    File.Delete(emptyFile); // Delete empty file
                }, 5, 3000);
            }
        }
    }
}

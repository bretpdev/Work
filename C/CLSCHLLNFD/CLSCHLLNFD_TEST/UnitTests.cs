using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CLSCHLLNFD;
using Uheaa.Common.ProcessLogger;
using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;
using System.Reflection;
using System.IO;

namespace CLSCHLLNFD_TEST
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestFormatComment()
        {
            ClosedSchoolLoan csl = new ClosedSchoolLoan(null, null);
            ErrorData ed1 = CreateErrorDataRecord("123456789", "1234567890", 1, 1, "ATCSK", ErrorMessage.LOAN_NOT_FOUND, "", 141, 217, DateTime.Now);
            ErrorData ed2 = CreateErrorDataRecord("123456789", "1234567890", 2, 1, "ATCSK", ErrorMessage.LOAN_NOT_FOUND, "", 141, 217, DateTime.Now);
            string collectiveComment = csl.FormatComment(new List<ErrorData>() { ed1, ed2 });
            string control = "For Loan Seq 1, 2: " + ErrorMessage.LOAN_NOT_FOUND;
            Assert.AreEqual(collectiveComment, control);
        }

        /// <summary>
        /// Verifies comments for diff reasons are compiled correctly.
        /// </summary>
        [TestMethod]
        public void TestFormatComment2()
        {
            ClosedSchoolLoan csl = new ClosedSchoolLoan(null, null);
            ErrorData ed1 = CreateErrorDataRecord("123456789", "1234567890", 1, 1, "ATCSK", ErrorMessage.LOAN_NOT_FOUND, "", 141, 217, DateTime.Now);
            ErrorData ed2 = CreateErrorDataRecord("123456789", "1234567890", 2, 1, "ATCSK", ErrorMessage.LOAN_STATUS, "", 141, 217, DateTime.Now);
            ErrorData ed3 = CreateErrorDataRecord("123456789", "1234567890", 3, 1, "ATCSK", ErrorMessage.LOAN_NOT_FOUND, "", 141, 217, DateTime.Now);
            string collectiveComment = csl.FormatComment(new List<ErrorData>() { ed1, ed2, ed3 });
            string control = "For Loan Seq 1, 3: " + ErrorMessage.LOAN_NOT_FOUND + " For Loan Seq 2: " + ErrorMessage.LOAN_STATUS;
            Assert.AreEqual(collectiveComment, control);
        }

        [TestMethod] //Check indexes per BA request
        public void TestFileHeader()
        {
            string dbLiveHeaders = "ACSkeyLine,StaticCurrentDate,AccountNumber,Name,Address1,Address2,City,State,ZIP,Country,DischargeAmount,ClosedSchool,Loan Program1,Date Disbursed1,Current Principal1,Loan Program2,Date Disbursed2,Current Principal2,Loan Program3,Date Disbursed3,Current Principal3,Loan Program4,Date Disbursed4,Current Principal4,Loan Program5,Date Disbursed5,Current Principal5,Loan Program6,Date Disbursed6,Current Principal6,Loan Program7,Date Disbursed7,Current Principal7,Loan Program8,Date Disbursed8,Current Principal8,Loan Program9,Date Disbursed9,Current Principal9,Loan Program10,Date Disbursed10,Current Principal10,Loan Program11,Date Disbursed11,Current Principal11,Loan Program12,Date Disbursed12,Current Principal12,Loan Program13,Date Disbursed13,Current Principal13,Loan Program14,Date Disbursed14,Current Principal14,Loan Program15,Date Disbursed15,Current Principal15,Loan Program16,Date Disbursed16,Current Principal16,Loan Program17,Date Disbursed17,Current Principal17,Loan Program18,Date Disbursed18,Current Principal18,Loan Program19,Date Disbursed19,Current Principal19,Loan Program20,Date Disbursed20,Current Principal20,Loan Program21,Date Disbursed21,Current Principal21,Loan Program22,Date Disbursed22,Current Principal22,Loan Program23,Date Disbursed23,Current Principal23,Loan Program24,Date Disbursed24,Current Principal24,Loan Program25,Date Disbursed25,Current Principal25,Loan Program26,Date Disbursed26,Current Principal26,Loan Program27,Date Disbursed27,Current Principal27,Loan Program28,Date Disbursed28,Current Principal28,Loan Program29,Date Disbursed29,Current Principal29,Loan Program30,Date Disbursed30,Current Principal30,CostCenter";
            string[] liveHeaders = dbLiveHeaders.Split(',');
            string index102 = liveHeaders[102];

            string dbTestHeaders = "ACSKeyLine,StaticCurrentDate,AccountNumber,Name,Address1,Address2,City,State,ZIP,Country,DischargeAmount,ClosedSchool,Loan Program1,Date Disbursed1,Current Principal1,Loan Program2,Date Disbursed2,Current Principal2,Loan Program3,Date Disbursed3,Current Principal3,Loan Program4,Date Disbursed4,Current Principal4,Loan Program5,Date Disbursed5,Current Principal5,Loan Program6,Date Disbursed6,Current Principal6,Loan Program7,Date Disbursed7,Current Principal7,Loan Program8,Date Disbursed8,Current Principal8,Loan Program9,Date Disbursed9,Current Principal9,Loan Program10,Date Disbursed10,Current Principal10,Loan Program11,Date Disbursed11,Current Principal11,Loan Program12,Date Disbursed12,Current Principal12,Loan Program13,Date Disbursed13,Current Principal13,Loan Program14,Date Disbursed14,Current Principal14,Loan Program15,Date Disbursed15,Current Principal15,Loan Program16,Date Disbursed16,Current Principal16,Loan Program17,Date Disbursed17,Current Principal17,Loan Program18,Date Disbursed18,Current Principal18,Loan Program19,Date Disbursed19,Current Principal19,Loan Program20,Date Disbursed20,Current Principal20,Loan Program21,Date Disbursed21,Current Principal21,Loan Program22,Date Disbursed22,Current Principal22,Loan Program23,Date Disbursed23,Current Principal23,Loan Program24,Date Disbursed24,Current Principal24,Loan Program25,Date Disbursed25,Current Principal25,Loan Program26,Date Disbursed26,Current Principal26,Loan Program27,Date Disbursed27,Current Principal27,Loan Program28,Date Disbursed28,Current Principal28,Loan Program29,Date Disbursed29,Current Principal29,Loan Program30,Date Disbursed30,Current Principal30,CostCenter";
            string[] testHeaders = dbTestHeaders.Split(',');
            string indexOfCostCenter = testHeaders[102];
            Assert.AreEqual(index102, indexOfCostCenter);
        }

        [TestMethod] //Index checking contd.
        public void TestFileHeader2()
        {
            string letterData = "#ACS#,5/15/2019,123456789,Donald Duck,507 Disney DR APT 6,,Los Angeles,CA,123456789,,\"3500\",\"Hollywood School\",Direct Subsidized Stafford,4/13/2015,\"$.00\",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,MA4481";
            string[] exampleLetterData = letterData.SplitAndRemoveQuotes(",").ToArray();
            string exampleCostCenter = exampleLetterData[102];
            Assert.AreEqual("MA4481", exampleCostCenter);
        }

        [TestMethod] //Create Test file
        public void TestCreateNewFormatFile()
        {
            string dir = @"C:\502_TestFiles\";
            string path = Path.Combine(dir, $"502_{Guid.NewGuid()}");
            Directory.CreateDirectory(dir);
            List<string> files = new List<string>(Directory.GetFiles(dir, "502*"));
            foreach (string file in files)
                File.Delete(file);

            TestSchoolDataForFile td = GetSchoolDataForBorrower("XXXXXXXXX", 1); //TODO: Input test borrower ssn into first parameter
            using (StreamWriter sw = new StreamWriter(path))
            {
                CreateHeaderRecord(sw);
                CreateDetailRecord(sw, td);
            }
            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public void TestCurrentBalanceFormatForZeroValue()
        {
            double amount = 0.0;
            string dollarValue = amount.ToString("$#,##0.00");
            Assert.AreEqual("$0.00", dollarValue);
        }

        [TestMethod]
        public void TestCurrentBalanceFormatForNonZeroValue()
        {
            double amount = 27.53;
            string dollarValue = amount.ToString("$#,##0.00");
            Assert.AreEqual("$27.53", dollarValue);
        }

        /// <summary>
        /// Creates first row of file.
        /// </summary>
        private void CreateHeaderRecord(StreamWriter sw)
        {
            sw.Write($"00{FormatDate(DateTime.Now.AddMonths(-1))}{FormatDate(DateTime.Now)}{FormatDate(DateTime.Now.AddYears(-3))}502");

            for (int i = 0; i < 619; i++)
                sw.Write(" ");
            sw.WriteLine("");
        }

        /// <summary>
        /// Creates one of the detail rows for a given borrower and loan sequence.
        /// </summary>
        private void CreateDetailRecord(StreamWriter sw, TestSchoolDataForFile td)
        {
            string bcountry = $"{FormatElement(td.Country, 2)}";
            string detailRecord = $"01{FormatElement(td.SchoolCode, 6)}{FormatElement(td.SchoolBranchCode, 8)}{FormatElement(td.SchoolName, 65)}{FormatDate(td.CloseDate)}{FormatElement(td.CurrentGaCode, 3)}{FormatElement(td.StudentSsn, 9)}{FormatElement(td.LastName, 35)}{FormatElement(td.FirstName, 35)}{FormatDate(td.StudentDob)}{FormatElement(td.PlusSsn, 9)}{FormatElement(td.PlusLastName, 35)}{FormatElement(td.PlusFirstName, 35)}{FormatDate(td.PlusDob)}{FormatElement(td.LoanType, 2)}{FormatDate(td.LoanDate)}{FormatElement(td.LoanAmount.ToString(), 6)}{FormatElement(td.TotalDisbursed.ToString(), 6)}{FormatElement(td.TotalCancelled.ToString(), 6)}{FormatElement(td.CurrentLoanStatus, 2)}{FormatDate(td.CurrentLoanStatusDate)}{FormatElement(td.OutstandingPrincipalBalance.ToString(), 6)}{FormatDate(td.OutstandingPrincipalBalanceDate)}{FormatElement(td.OutstandingInterestBalance.ToString(), 6)}{FormatDate(td.OutstandingInterestBalanceDate)}{FormatElement(td.AwardId, 21)}{FormatElement(td.LoanIdentifier, 17)}{FormatElement(td.StudentIdentifier, 13)}{FormatElement(td.CurrentGaCode, 3)}{FormatElement(td.Email, 70)}{td.AddressType}{FormatElement(td.StreetAddress1, 40)}{FormatElement(td.StreetAddress2, 40)}{FormatElement(td.City, 30)}{FormatElement(td.State, 2)}{FormatElement(td.Country, 2)}{FormatElement(td.Zip, 17)}";
            sw.Write(detailRecord);

            for (int i = 0; i < 52; i++)
                sw.Write(" ");
            sw.WriteLine("");
        }

        /// <summary>
        /// Formats the given string so that it meets the length reqs
        /// of the file and has proper space char padding.
        /// </summary>
        private string FormatElement(string s, int length)
        {
            decimal d;
            if (s.Contains(".") && decimal.TryParse(s, out d))
            {
                s = $"{(int)d}";
            }

            s = s.Trim();

            if (s == null)
                return "FAILURE_TO_FORMAT_ELEMENT";
            if (s.Length == length)
                return s;
            else if (s.Length > length) //This is bad.  It should not happen, but if it does, we want to know about it.
                return "FAILURE_TO_FORMAT_ELEMENT";
            else if (s.Length < length)
            {
                int diff = length - s.Length;
                for (int i = 0; i < diff; i++)
                    s = s + " ";
            }
            return s;
        }

        /// <summary>
        /// Creates an ErrorData record for error message (used in test cases for message compilation testing)
        /// </summary>
        private ErrorData CreateErrorDataRecord(string borrowerSsn, string accountNumber, int loanSeq, int disbursementSeq, string arc, string errorMessage, string sessionMessage, int schoolClosureId, int arcAddProcessingId, DateTime addedAt)
        {
            ErrorData ed = new ErrorData()
            {
                BorrowerSsn = borrowerSsn,
                AccountNumber = accountNumber,
                LoanSeq = loanSeq,
                DisbursementSeq = disbursementSeq,
                Arc = arc,
                ErrorMessage = errorMessage,
                SessionMessage = sessionMessage,
                SchoolClosureDataId = schoolClosureId,
                ArcAddProcessingId = arcAddProcessingId,
                AddedAt = addedAt
            };
            return ed;
        }

        /// <summary>
        /// Launches test instance of script 
        /// </summary>
        private TestSchoolDataForFile GetSchoolDataForBorrower(string ssn, int loanSequence)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            if (!DataAccessHelper.StandardArgsCheck(new string[] { "dev" }, "CLSCHLLNFD") || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return null;

            ReflectionInterface ri = new ReflectionInterface();
            ProcessLogRun logRun = new ProcessLogRun("CLSCHLLNFD", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.Region.CornerStone, DataAccessHelper.CurrentMode);
            ClosedSchoolLoan csl = new ClosedSchoolLoan(ri, logRun);
            csl.DA = new DataAccess(logRun);
            TestSchoolDataForFile testData = csl.DA.Test_GetSchoolDataForBorrower(ssn, loanSequence);
            ri.CloseSession();

            return testData;
        }

        /// <summary>
        /// Converts DateTime to properly formatted date string sans special characters.
        /// </summary>
        private string FormatDate(DateTime? dts)
        {
            if (dts == null)
                return "        ";
            else
                return dts.Value.ToString("yyyyMMdd").Replace("/", string.Empty);
        }
    }
}

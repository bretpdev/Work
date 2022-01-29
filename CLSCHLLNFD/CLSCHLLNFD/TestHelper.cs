using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace CLSCHLLNFD
{
    class TestHelper
    {

        /// <summary>
        /// This method is purely for testing the script and creating a file.  Should not be run in Prod.
        /// </summary>
        /// <returns></returns>
        public int CreateTestFile(string[] args, ReflectionInterface ri, ProcessLogRun logRun)
        {
            string dir = @"C:\502_TestFiles\";
            string path = Path.Combine(dir, $"502_{Guid.NewGuid()}");
            Directory.CreateDirectory(dir);
            List<string> files = new List<string>(Directory.GetFiles(dir, "502*"));
            foreach (string file in files)
                File.Delete(file);

            TestSchoolDataForFile testData = GetTestData(args, ri, logRun);

            if (testData == null)
            {
                ri.CloseSession();
                return 1;
            }

            using (StreamWriter sw = new StreamWriter(path))
            {
                CreateHeaderRecord(sw);
                CreateDetailRecord(sw, testData);
            }

            logRun.LogEnd();
            ri.CloseSession();
            DataAccessHelper.CloseAllManagedConnections();
            return 0;
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
        /// Retrieves test data for borrower and loan from DB.
        /// </summary>
        /// <returns></returns>
        public TestSchoolDataForFile GetTestData (string[] args, ReflectionInterface ri, ProcessLogRun logRun)
        {
            ClosedSchoolLoan csl = new ClosedSchoolLoan(ri, logRun);
            csl.DA = new DataAccess(logRun);
            string ssn = "";
            int loanSequence = -1;
            if (args.Length > 3)
            {
                ssn = args[2];
                loanSequence = args[3].ToInt();
            }
            else
            {
                Console.WriteLine("Incorrect number of args passed. Include ssn and loan sequence as second and third args respectively.");
                return null;
            }

            TestSchoolDataForFile testData = csl.DA.Test_GetSchoolDataForBorrower(ssn, loanSequence);

            return testData;
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

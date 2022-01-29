using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;
using SCHRPT_Batch;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SCHRPT_Batch.Tests
{
    public class BatchCorrectnessChecks
    {
        //SchrptProcess Tests

        /// <summary>
        /// Creates a mock table to be written to file and then
        /// writes the data to a file in CSV format. The test then
        /// parses the file and compares it a to a string literal of the
        /// desired CSV result
        /// </summary>
        [Fact]
        public void CsvFileWriting()
        {
            const string scriptId = "SCHRPT_Batch.Tests";
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            ProcessLogRun LogRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

            SchrptProcess process = new SchrptProcess(LogRun, DataAccessHelper.CurrentRegion, @"c:\SCHRPT_Batch\schrpt_temp.csv");
            PrivateObject process_obj = new PrivateObject(process);

            DataTable testTable = new DataTable();

            DataColumn col1 = new DataColumn("test1");
            col1.DataType = System.Type.GetType("System.String");
            testTable.Columns.Add(col1);

            DataColumn col2 = new DataColumn("test2");
            col1.DataType = System.Type.GetType("System.String");
            testTable.Columns.Add(col2);

            DataRow row1 = testTable.NewRow();
            row1["test1"] = "1";
            row1["test2"] = "2";
            testTable.Rows.Add(row1);

            DataRow row2 = testTable.NewRow();
            row2["test1"] = "3";
            row2["test2"] = "4";
            testTable.Rows.Add(row2);

            object[] arr = new object[] { testTable, true };
            StringBuilder ret = (StringBuilder)process_obj.Invoke("WriteToCsvFile", arr);

            //string path = EnterpriseFileSystem.TempFolder + "schrpt_temp.csv";
            string csvFile = ret.ToString();

            //Ensure that the written file conforms to csv parsing as (numbers are of type int 
            Debug.Assert(csvFile.CompareTo("test1,test2\r\n1,2\r\n3,4\r\n") == 0);
        }


        /// <summary>
        /// Creates a record to be run through process
        /// and then confirms the record was succesfully
        /// added to the class's field
        /// </summary>
        [Fact]
        public void ProcessRecipient()
        {
            string scriptId = "SCHRPT_Batch.Tests";
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            ProcessLogRun LogRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

            SchrptProcess process = new SchrptProcess(LogRun, DataAccessHelper.CurrentRegion, @"c:\SCHRPT_Batch\schrpt_temp.csv");
            PrivateObject process_obj = new PrivateObject(process);

            List<SchrptRecord> records = new List<SchrptRecord>();
            SchrptRecord record = new SchrptRecord();
            record.Email = "TestEmail@Test.Test";
            record.RecipientId = 1;
            records.Add(record);

            var recordGrouping = records.GroupBy(p => p.RecipientId);

            object[] recordObjs = new object[] { recordGrouping.First() };

            process_obj.Invoke("ProcessRecipients", recordObjs);

            Dictionary<int, string> recipientsVal = (Dictionary<int, string>)process_obj.GetField("Recipients");
            Debug.Assert(recipientsVal.ContainsValue("TestEmail@Test.Test"));
        }

        /// <summary>
        /// Creates a record to be run through process
        /// and then confirms the record was succesfully
        /// added to the class's field
        /// </summary>
        [Fact]
        public void ProcessSchools()
        {
            const string scriptId = "SCHRPT_Batch.Tests";
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            ProcessLogRun LogRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

            SchrptProcess process = new SchrptProcess(LogRun, DataAccessHelper.CurrentRegion, @"c:\SCHRPT_Batch\schrpt_temp.csv");
            PrivateObject process_obj = new PrivateObject(process);

            List<SchrptRecord> records = new List<SchrptRecord>();
            SchrptRecord record = new SchrptRecord();
            record.SchoolId = 1;
            record.SchoolCode = "111111";
            record.BranchCode = "11";
            records.Add(record);

            var recordGrouping = records.GroupBy(p => p.SchoolId);

            object[] recordObjs = new object[] { recordGrouping.First() };

            process_obj.Invoke("ProcessSchools", recordObjs);

            Dictionary<int, string> schoolsVal = (Dictionary<int, string>)process_obj.GetField("Schools");
            Debug.Assert(schoolsVal.ContainsValue("11111111"));
        }

        /// <summary>
        /// Creates a record to be run through process
        /// and then confirms the record was succesfully
        /// added to the class's field
        /// </summary>
        [Fact]
        public void ProcessReportTypes()
        {
            const string scriptId = "SCHRPT_Batch.Tests";
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            ProcessLogRun LogRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

            SchrptProcess process = new SchrptProcess(LogRun, DataAccessHelper.CurrentRegion, @"c:\SCHRPT_Batch\schrpt_temp.csv");
            PrivateObject process_obj = new PrivateObject(process);

            List<SchrptRecord> records = new List<SchrptRecord>();
            SchrptRecord record = new SchrptRecord();
            record.ReportTypeId = 1;
            record.StoredProcedureName = "TestProcedure";
            records.Add(record);

            var recordGrouping = records.GroupBy(p => p.SchoolId);

            object[] recordObjs = new object[] { recordGrouping.First() };

            process_obj.Invoke("ProcessReportTypes", recordObjs);

            Dictionary<int, string> reportTypesVal = (Dictionary<int, string>)process_obj.GetField("ReportTypes");
            Debug.Assert(reportTypesVal.ContainsValue("TestProcedure"));
        }

        //DataAccess Tests

        /// <summary>
        /// Does an add to the Recipients table
        /// and uses the cooresponding DataAccess
        /// accessor method to pull the data to 
        /// confirm the entry is present
        /// </summary>
        [Fact]
        public void GetRecipients()
        {
            const string scriptId = "SCHRPT_Batch.Tests";
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            ProcessLogRun LogRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            LogDataAccess LDA = new LogDataAccess(DataAccessHelper.CurrentMode, LogRun.ProcessLogId, false, false);
            DataAccessHelper.Database DB = DataAccessHelper.Database.Cls; //EXECUTE ON CLS DATABASE
            DataAccess DA = new DataAccess(LDA, DB);

            var result = LDA.ExecuteSingle<int>("schrpt.AddRecipient", DB, SqlParams.Single("Name", "TestName"), SqlParams.Single("Email", "TestEmail@Test.Test"), SqlParams.Single("CompanyName", "TestCompany"));

            List<SchrptRecord> records = DA.GetRecipients();
            bool found = false;
            foreach (var record in records)
            {   
                bool recipientId = record.RecipientId.CompareTo(result.Result) == 0;  
                if (recipientId)
                {
                    found = true;
                    break;
                }   
            }
            Debug.Assert(found);
        }

        /// <summary>
        /// Does an add to the Schools table
        /// and SchoolRecipients table 
        /// and uses the cooresponding DataAccess
        /// accessor methods to pull the data to 
        /// confirm the entries are present
        /// </summary>
        [Fact]
        public void GetSchoolsAndGetSchoolRecipients()
        {
            const string scriptId = "SCHRPT_Batch.Tests";
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            ProcessLogRun LogRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            LogDataAccess LDA = new LogDataAccess(DataAccessHelper.CurrentMode, LogRun.ProcessLogId, false, false);
            DataAccessHelper.Database DB = DataAccessHelper.Database.Cls;
            DataAccess DA = new DataAccess(LDA, DB);

            var result1 = LDA.ExecuteSingle<int>("schrpt.AddSchool", DB, SqlParams.Single("Name", "TestSchool"), SqlParams.Single("SchoolCode", "111111"), SqlParams.Single("BranchCode", "11"));
            var result2 = LDA.ExecuteSingle<int>("schrpt.AddSchoolRecipient", DB, SqlParams.Single("SchoolId", result1.Result), SqlParams.Single("RecipientId", 1), SqlParams.Single("ReportTypeId", 1));

            List<SchrptRecord> records = DA.GetSchools();
            bool found = false;
            foreach (var record in records)
            {
                bool schoolId = record.SchoolId.CompareTo(result1.Result) == 0;
                if (schoolId)
                {
                    found = true;
                    break;
                }
            }
            Debug.Assert(found);

            records = DA.GetSchoolRecipients();
            found = false;
            foreach (var record in records)
            {
                bool schoolRecipientId = record.SchoolRecipientId.CompareTo(result2.Result) == 0;
                if (schoolRecipientId)
                {
                    found = true;
                    break;
                }
            }
            Debug.Assert(found);
        }

        /// <summary>
        /// Does an add to the ReportTypes table
        /// and uses the cooresponding DataAccess
        /// accessor method to pull the data to 
        /// confirm the entry is present
        /// </summary>
        [Fact]
        public void GetReportTypes()
        {
            const string scriptId = "SCHRPT_Batch.Tests";
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            ProcessLogRun LogRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            LogDataAccess LDA = new LogDataAccess(DataAccessHelper.CurrentMode, LogRun.ProcessLogId, false, false);
            DataAccessHelper.Database DB = DataAccessHelper.Database.Cls; //EXECUTE ON CLS DATABASE
            DataAccess DA = new DataAccess(LDA, DB);

            var result = LDA.ExecuteSingle<int>("schrpt.AddReportType", DB, SqlParams.Single("StoredProcedureName", "TestProcedure"));

            List<SchrptRecord> records = DA.GetReportTypes();
            bool found = false;
            foreach (var record in records)
            {  
                bool reportTypeId = record.ReportTypeId.CompareTo(result.Result) == 0;
                if (reportTypeId)
                {
                    found = true;
                    break;
                }      
            }
            Debug.Assert(found);
        }

        /// <summary>
        /// Does an add to the SchoolEmailHistory table
        /// and uses the cooresponding DataAccess
        /// accessor method to pull the data to 
        /// confirm the entry is present
        /// </summary>
        [Fact]
        public void SchoolEmailHistory()
        {
            const string scriptId = "SCHRPT_Batch.Tests";
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            ProcessLogRun LogRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
            LogDataAccess LDA = new LogDataAccess(DataAccessHelper.CurrentMode, LogRun.ProcessLogId, false, false);
            DataAccessHelper.Database DB = DataAccessHelper.Database.Cls; //EXECUTE ON CLS DATABASE
            DataAccess DA = new DataAccess(LDA, DB);

            DateTime time = DateTime.Now;

            var result1 = LDA.ExecuteSingle<int>("schrpt.AddSchool", DB, SqlParams.Single("Name", "TestSchool"), SqlParams.Single("SchoolCode", "111111"), SqlParams.Single("BranchCode", "11"));
            var result2 = LDA.ExecuteSingle<int>("schrpt.AddSchoolRecipient", DB, SqlParams.Single("SchoolId", result1.Result), SqlParams.Single("RecipientId", 1), SqlParams.Single("ReportTypeId", 1));
            var result3 = LDA.ExecuteSingle<int>("schrpt.AddSchoolEmailHistory", DB, SqlParams.Single("SchoolRecipientId", result2.Result), SqlParams.Single("EmailSentAt", time));

            List<SchrptRecord> records = DA.GetSchoolEmailHistory();
            bool found = false;
            foreach (var record in records)
            {
               
                bool schoolEmailHistoryId = record.SchoolEmailHistoryId.CompareTo(result3.Result) == 0;
                if (schoolEmailHistoryId)
                {
                    found = true;
                    break;
                }
                
            }
            Debug.Assert(found);
        }

        [Fact]
        public void TestHeaderReturnedWithEmptyResult()
        {
            const string scriptId = "SCHRPT_Batch.Tests";
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            ProcessLogRun LogRun = new ProcessLogRun(scriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);

            SchrptProcess process = new SchrptProcess(LogRun, DataAccessHelper.CurrentRegion, @"c:\SCHRPT_Batch\schrpt_temp.csv");
            PrivateObject process_obj = new PrivateObject(process);

            FieldInfo fis = process.GetType().GetField("Schools", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Dictionary<int, string> School = new Dictionary<int, string>();
            School.Add(0, "00105110");
            fis.SetValue(process, School);

            FieldInfo fir = process.GetType().GetField("ReportTypes", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Dictionary<int, string> ReportTypes = new Dictionary<int, string>();
            ReportTypes.Add(0, "GetBorrowersForSchool");
            fir.SetValue(process, ReportTypes);

            object[] args = new object[] { 0, 0 };
            CsvResult ret = (CsvResult)process_obj.Invoke("ProcessSingleSchoolRequest", args);
            Debug.Assert(ret.RowCount == 0);
        }

    }
}

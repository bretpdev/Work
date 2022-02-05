using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SYSTMLTRSU;
using System.Data;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using System.Reflection;
using Uheaa.Common.DocumentProcessing;

namespace SYSTMLTRSU_Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetdataLine_NoLoanDetail_NoFormFields()
        {
            HeaderFooterData addressLine = new HeaderFooterData { Name = "John Doe", Address1 = "123 Fake Street", Address2 = "", ForeignState = "", Country = "", AccountNumber = "1234567890", BarcodeAccountNumber = "1234567890", City = "Salt Lake City", State = "UT", Zip = "84101", HasValidAddress = true, Hours1 = "9 AM to 5 PM", Hours2 = "Mountain Time" };
            DataTable loanDetail = null;
            Dictionary<string, string> formFields = null;
            LetterParameters letterParams = new LetterParameters { AddressLine = addressLine, LoanDetail = loanDetail, FormFields = formFields };
            string ssn = "000000000";
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ProcessLogRun plr = new ProcessLogRun("SYSTMLTRSU_test", AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false, false);
            string dataline = new LetterGenerator(plr).GetDataLine(letterParams, ssn);
            Assert.AreEqual(dataline, "John Doe,123 Fake Street,,Salt Lake City,UT,84101,,,1234567890,1234567890,9 AM to 5 PM,Mountain Time," + DocumentProcessing.ACSKeyLine(ssn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal) + ",MA2324");
        }

        [TestMethod]
        public void HeaderFooterDataToString()
        {
            var data = new HeaderFooterData();
            data.AccountNumber = "1234567890";
            data.Name = "n";
            data.Address1 = "a1";
            data.Address2 = "a2";
            data.City = "c";
            data.State = "s";
            data.Zip = "zip";
            data.Country = "cty";
            data.ForeignState = "fs";
            data.BarcodeAccountNumber = "1234567890";
            data.Hours1 = "h1";
            data.Hours2 = "h2";

            Assert.IsTrue(data.ToString() == "n,a1,a2,c,s,zip,cty,fs,1234567890,1234567890,h1,h2");
        }
    }
}

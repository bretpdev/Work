using CentralizedPrintingProcess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;

namespace CNTRPRNT_Tests
{
    [TestClass]
    public class UnitTests
    {
        public MiscDat Data { get; set; }

        [TestMethod]
        public void ChangePrinterSimplexTest()
        {
            PrinterInfo pInfo = Printing.SetPrinterSiding(new LetterRecord() { Duplex = false });
            Assert.AreEqual(1, pInfo.Duplex);
        }

        [TestMethod]
        public void ChangePrinterDuplexTest()
        {
            PrinterInfo pInfo = Printing.SetPrinterSiding(new LetterRecord() { Duplex = true });
            Assert.AreEqual(2, pInfo.Duplex);
        }

        public UnitTests()
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;

            Data = new MiscDat("CNTRPRNT");
        }

        [TestMethod]
        public void LetterDirectoryExists()
        {
            Assert.IsTrue(Directory.Exists(Data.LetterDirectory));
        }

        [TestMethod]
        public void EcorrDirectoryExists()
        {
            Assert.IsTrue(Directory.Exists(Data.EcorrDirectory));
        }

        [TestMethod]
        public void FaxDirectoryExists()
        {
            Assert.IsTrue(Directory.Exists(Data.FaxDirectory));
        }
    }
}
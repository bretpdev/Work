using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;
using Uheaa.Common.DataAccess;


namespace UnitTest
{
    [TestClass]
    public class Regimen
    {
        [TestMethod]
        public void TestTemplateFileExists()
        {
            // Test the template file is out there.
            string payofffeddocx = @"Y:\Codebase\Correspondence\PAYOFFFED.docx";
            Assert.IsTrue(File.Exists(payofffeddocx));
        }

        [TestMethod]
        public void TestAccessToSprocs()
        {
            // Test sproc access
            string msg = DatabaseAccessHelper.GenerateSprocAccessAlert(Assembly.GetExecutingAssembly());
            Assert.IsTrue(msg == null);
        }

        [TestMethod]
        public void TestValidate()
        {
            PYOFFLTRFD.BorrowerData bData = new PYOFFLTRFD.BorrowerData();
            bData.Demos.AccountNumber = "1234567890";
            bData.PayoffDate = "07/04/2010";
            PYOFFLTRFD.PayoffInformation payOff = new PYOFFLTRFD.PayoffInformation(bData);
            bool test = payOff.ValidateData();
            Assert.IsFalse(test);
        }
    }   
}
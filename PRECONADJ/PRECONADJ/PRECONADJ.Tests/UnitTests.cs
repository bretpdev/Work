using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PRECONADJ.Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestTabNameConvertor()
        {
            //should zero extend the result to 3 characters, null otherwise
            string one = ExcelHelper.TranslateToTabName(1);
            string ten = ExcelHelper.TranslateToTabName(10);
            string hundred = ExcelHelper.TranslateToTabName(100);
            string thousand = ExcelHelper.TranslateToTabName(1000);
            string negative = ExcelHelper.TranslateToTabName(-1);
            Assert.AreEqual(one, "001");
            Assert.AreEqual(ten, "010");
            Assert.AreEqual(hundred, "100");
            Assert.AreEqual(thousand, null);
            Assert.AreEqual(negative, null);
        }

        [TestMethod]
        public void TestTypeSubType()
        {
            Types type = new Types();
            //If Cap Is Y: type = 70, subtype = 01
            type = type.GetTypeSubtype("0.00", "Y");
            Assert.AreEqual(type.Type, "70");
            Assert.AreEqual(type.SubType, "01");
            //If Disbursement Amount Greater Than 0: type = 01, subtype = 01
            type = type.GetTypeSubtype("1.00", "");
            Assert.AreEqual(type.Type, "01");
            Assert.AreEqual(type.SubType, "01");
            //If Neither Cap Is Y And Disbursement Amount Greater Than 0: type = 01, subtype = 01
            type = type.GetTypeSubtype("0.00", "");
            Assert.AreEqual(type.Type, "10");
            Assert.AreEqual(type.SubType, "10");
            //If Cap Is Y And Disbursement Amount Greater Than 0: ERROR
            try
            {
                type = type.GetTypeSubtype("0.00", "Y");
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.IsTrue(true);
            }
            //If Disbursement Amount NAN: ERROR
            try
            {
                type = type.GetTypeSubtype("a", "");
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void TestDoubleToBalance()
        {
            //Should format a double to two decimal places, rounding up if necessary
            string balance0 = ExcelHelper.DoubleToBalance(1.111111111);
            string balance1 = ExcelHelper.DoubleToBalance(1.119999999);
            string balance2 = ExcelHelper.DoubleToBalance(1.1);
            string balance3 = ExcelHelper.DoubleToBalance(1);
            string balance4 = ExcelHelper.DoubleToBalance(-1);
            string balance5 = ExcelHelper.DoubleToBalance(-1.1);
            string balance6 = ExcelHelper.DoubleToBalance(-1.119999999);
            string balance7 = ExcelHelper.DoubleToBalance(-1.111111111);
            Assert.AreEqual(balance0, "1.11");
            Assert.AreEqual(balance1, "1.12");
            Assert.AreEqual(balance2, "1.10");
            Assert.AreEqual(balance3, "1.00");
            Assert.AreEqual(balance4, "-1.00");
            Assert.AreEqual(balance5, "-1.10");
            Assert.AreEqual(balance6, "-1.12");
            Assert.AreEqual(balance7, "-1.11");
        }
    }
}
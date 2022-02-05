using System;
using System.Linq;
using Xunit;
using PEPSFED;
using System.IO;
using Uheaa.Common.DataAccess;
using System.Reflection;

namespace PEPS_FEDTESTS
{
    public class UnitTest1
    {

        [Theory]
        [InlineData(" Test")]
        public void TestTrim(string name)
        {
            ContactData d = new ContactData();
            d.ContType = "";
            d.ContFirstName = name;
            d.ContLastName = "";
            d.ContAreaCode = "";
            d.ContExchange = "";
            d.ContExt = "";
            d.ContExt2 = "";

            d.ContFax = "";
            d.ContForeignPhone = "";
            d.FormatProperties();
            Assert.True(d.ContFirstName == "Test");
        }

        [Theory]
        [InlineData("Test*")]
        public void TestRemoveSpecialChar(string name)
        {
            ContactData d = new ContactData();
            d.ContType = "";
            d.ContFirstName = name;
            d.ContLastName = "";
            d.ContAreaCode = "";
            d.ContExchange = "";
            d.ContExt = "";
            d.ContExt2 = "";

            d.ContFax = "";
            d.ContForeignPhone = "";
            d.FormatProperties();
            Assert.True(d.ContFirstName == "Test");
        }

        [Theory]
        [InlineData("20180503")]
        public void TestParseDate(string date)
        {
            ProgramData d = new ProgramData();
            var val = d.ParseDate(date, d);
            Assert.True(val == new DateTime(2018, 05, 03));
        }

        [Theory]
        [InlineData("(011)-5225-52252")]
        public void TestPhoneFormatting(string phone)
        {
            ContactData d = new ContactData();
            d.ContType = "";
            d.ContFirstName = "";
            d.ContLastName = "";
            d.ContAreaCode = "";
            d.ContExchange = "";
            d.ContExt = "";
            d.ContExt2 = "";

            d.ContFax = "";
            d.ContForeignPhone = phone;
            d.FormatProperties();
            Assert.True(d.ContForeignPhone == "011522552252");
        }
    }
}

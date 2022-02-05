using System;
using System.Linq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using ECORRSLFED;
using System.Reflection;
using Uheaa.Common.ProcessLogger;

namespace ECORRSLFED.Tests
{
    public class MergeFieldTests
    {
        public HeaderFooterData LocalTestAddress { get; set; }
        public HeaderFooterData ForeignTestAddress { get; set; }
        public MergeFieldTests()
        {
            HeaderFooterData localTestAddress = new HeaderFooterData();
            localTestAddress.Name = "John Smith";
            localTestAddress.Address1 = "123 fake street";
            localTestAddress.Address2 = "";
            localTestAddress.ForeignState = "";
            localTestAddress.Country = "";
            localTestAddress.AccountNumber = "1234567890";
            localTestAddress.City = "Salt Lake City";
            localTestAddress.State = "Utah";
            localTestAddress.Zip = "05432";
            localTestAddress.HasValidAddress = true;
            LocalTestAddress = localTestAddress;

            HeaderFooterData foreignTestAddress = new HeaderFooterData();
            foreignTestAddress.Name = "Mary IV";
            foreignTestAddress.Address1 = "123 fake street";
            foreignTestAddress.Address2 = "";
            foreignTestAddress.ForeignState = "State";
            foreignTestAddress.Country = "England";
            foreignTestAddress.AccountNumber = "1234567890";
            foreignTestAddress.City = "Liverpool";
            foreignTestAddress.State = "";
            foreignTestAddress.Zip = "05432";
            foreignTestAddress.HasValidAddress = true;
            ForeignTestAddress = foreignTestAddress;
        }
        [Fact]
        public void ToEcorrListForeign()
        {
            List<string> testList = ForeignTestAddress.ToEcorrList();
            Assert.False(testList[3].Contains(","));
        }

        [Fact]
        public void ToEcorrListLocal()
        {
            List<string> testList = LocalTestAddress.ToEcorrList();
            Assert.False(testList[3].Contains(","));
        }

        [Fact]
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

            Assert.True(data.ToString() == "n,a1,a2,c,s,zip,cty,fs,1234567890,1234567890");
        }
    }
}

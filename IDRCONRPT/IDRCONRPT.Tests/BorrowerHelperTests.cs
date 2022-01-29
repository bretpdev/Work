using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Uheaa.Common.ProcessLogger;
using Xunit;

namespace IDRCONRPT.Tests
{
    public class BorrowerHelperTests
    {
        XPathHelper helper = new XPathHelper(null);

        [Fact]
        public void CreateNavigatorTestPositive()
        {
            var nav = helper.GetNavigatorFromContents("TestData", TestData.NoRejectedBorrowers);
            Assert.NotNull(nav);
        }

        [Fact]
        public void CreateNavigatorTestNegative()
        {
            XPathNavigator nav = null;
            try
            {
                nav = helper.GetNavigatorFromContents("Garbage", "NOT XML");
            }
            catch (Exception)
            {

            }
            finally
            {
                Assert.Null(nav);
            }
        }

        [Fact]
        public void NoRejectsTest()
        {
            var nav = helper.GetNavigatorFromContents("TestData", TestData.NoRejectedBorrowers);
            var rejects = helper.GetRejects(nav, "");
            Assert.Empty(rejects);
        }

        [Fact]
        public void OneRejectTest()
        {
            var nav = helper.GetNavigatorFromContents("TestData", TestData.OneRejectedBorrower);
            var rejects = helper.GetRejects(nav, "");
            Assert.Single(rejects);
        }

        [Fact]
        public void TwoRejectTest()
        {
            var nav = helper.GetNavigatorFromContents("TestData", TestData.TwoRejectedBorrowers);
            var rejects = helper.GetRejects(nav, "");
            Assert.Equal(2, rejects.Count);
        }
    }
}

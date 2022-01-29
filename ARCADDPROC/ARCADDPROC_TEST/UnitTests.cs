using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ARCADDPROC_TEST
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void FailureOnReturnMessageCode()
        {
            string message = "01527";
            var ri = new MockArcAddProcRI();
            ri.MessageCode = message;
            var aap = new ArcAddHelper(ri);
            ArcHelperResults results = aap.AddLP50Comment("RF@000000", "MC", "ARCXS", "R", "Comment", "ARCADDPROC");
            Assert.IsFalse(results.CommentAdded);
        }

        [TestMethod]
        public void SuccessOnReturnMessageCode()
        {
            var ri = new MockArcAddProcRI();
            ri.MessageCode = "Testing true";
            var aap = new ArcAddHelper(ri);
            ArcHelperResults results = aap.AddLP50Comment("RF@000000", "MC", "ARCXS", "R", "Comment", "ARCADDPROC");
            Assert.IsTrue(results.CommentAdded);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestException()
        {
            string comment = "IfCharactersAdded,ThrowException..";   // 33 chars, including periods.
            ArcAddHelper.ThrowsExceptionIfCommentTooLarge(comment);
        }

        [TestMethod]
        public void TestSSNs()
        {
            TestSSN("123456789");
            TestSSN("483829247");
            TestSSN("666123456");
            TestSSN("900999123");
        }

        public void TestSSN(string parameter)
        {
            Assert.IsTrue(Regex.IsMatch(parameter, @"(\d{3}-\d{2}-\d{4}|XXX-XX-XXXX|\d{9}|\d{3} ?\d{2} ?\d{4}|XXX XX XXXX)"));
        }
    }
}

using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ONELINKAAP_TEST
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestFailureOnReturnMessageCode()
        {
            FailureOnReturnMessageCode("01527");
        }

        public void FailureOnReturnMessageCode(string message)
        {
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
        public void ToggleCompassFlag()
        {
            var ri = new MockArcAddProcRI();
            ONELINKAAP.ArcRecord arc = new ONELINKAAP.ArcRecord();
            arc.ActivityType = String.Empty;
            ri.MessageCode = "Testing true";
            var aap = new ArcAddHelper(ri);
            ArcHelperResults result = aap.ProcessCompass(arc);
            Assert.IsTrue(result.CommentAdded);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestException()
        {
            string comment = "IfCharactersAdded,ThrowException..";   // 33 chars, including periods.
            ArcAddHelper.ThrowsExceptionIfCommentTooLarge(comment);
        }

        [TestMethod]
        public void TestAllSSN()
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

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;

namespace OneLinkAAPTEST
{
    public class ArcAddHelper_Tests
    {
        [Theory]
        [InlineData("01527"), InlineData("01004")]
        public void FailureOnReturnMessageCode(string message)
        {
            var ri = new MockArcAddProcRI();
            ri.MessageCode = message;
            var aap = new ArcAddHelper(ri);
            ArcHelperResults results = aap.AddLP50Comment("RF@000000", "MC", "ARCXS", "R", "Comment", "ARCADDPROC");
            Assert.False(results.CommentAdded);
        }

        [Fact]
        public void SuccessOnReturnMessageCode()
        {
            var ri = new MockArcAddProcRI();
            ri.MessageCode = "Testing true";
            var aap = new ArcAddHelper(ri);
            ArcHelperResults results = aap.AddLP50Comment("RF@000000", "MC", "ARCXS", "R", "Comment", "ARCADDPROC");
            Assert.False(results.CommentAdded);
        }

        [Fact]
        public void ToggleCompassFlag()
        {
            var ri = new MockArcAddProcRI();
            ARCADDPROCESSING.ArcRecord arc = new ARCADDPROCESSING.ArcRecord();
            arc.ActivityType = String.Empty;
            ri.MessageCode = "Testing true";
            var aap = new ArcAddHelper(ri);
            ArcHelperResults result = aap.ProcessCompass(arc);
            Assert.True(result.CommentAdded);
        }

        [Fact]
        public void TestException()
        {
            string comment = "IfCharactersAdded,ThrowException.";   // 33 chars, including period.
            Assert.Throws<InvalidOperationException>(()=> ArcAddHelper.ThrowsExceptionIfCommentTooLarge(comment));
        }

        [Theory]
        [InlineData("123456789")]
        [InlineData("483829247")]
        [InlineData("666123456")]
        [InlineData("900999123")]
        public void TestSSN(string parameter)
        {
            Assert.Matches(@"(\d{3}-\d{2}-\d{4}|XXX-XX-XXXX|\d{9}|\d{3} ?\d{2} ?\d{4}|XXX XX XXXX)", parameter);
        }
        

    }
}

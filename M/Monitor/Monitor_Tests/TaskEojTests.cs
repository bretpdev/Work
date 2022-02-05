using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor_Tests
{
    [TestClass]
    public class TaskEojTests
    {
        [TestMethod]
        public void TaskEojCancel()
        {
            var eoj = new TaskEoj();
            eoj.EojType = EojReport.Cancelled;
            Assert.AreEqual(eoj.GetTaskResult(), TaskResult.CancelTask);
        }

        [TestMethod]
        public void TaskEojSkip()
        {
            var eoj = new TaskEoj();
            eoj.EojType = EojReport.ExemptConditionSkipped;
            Assert.AreEqual(eoj.GetTaskResult(), TaskResult.SkipTask);
            eoj.EojType = EojReport.ForwardedSkipped;
            Assert.AreEqual(eoj.GetTaskResult(), TaskResult.SkipTask);
            eoj.EojType = EojReport.MaxLimitSkipped;
            Assert.AreEqual(eoj.GetTaskResult(), TaskResult.SkipTask);
            eoj.EojType = EojReport.PaymentsTooHighPrenotifications;
            Assert.AreEqual(eoj.GetTaskResult(), TaskResult.SkipTask);
        }

        [TestMethod]
        public void TaskEojComplete()
        {
            var eoj = new TaskEoj();
            eoj.EojType = EojReport.Redisclosed;
            Assert.AreEqual(eoj.GetTaskResult(), TaskResult.CompleteTask);
        }
    }
}

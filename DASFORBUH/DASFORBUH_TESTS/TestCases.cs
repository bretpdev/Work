﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DASFORBUH;

namespace DASFORBUH_TESTS
{
    [TestClass]
    public class TestCases
    {
        [TestMethod]
        public void ShouldFailOnMessageCode1()
        {
            var ri = new TestForbearanceRI();
            ri.MessageCode = "01527";
            var fh = new ForbearanceHelper(ri);
            var dummy = new ProcessQueueData();
            var results = fh.AddForbearance(dummy);
            Assert.IsFalse(results.ForbearanceAdded);
        }

        [TestMethod]
        public void ShouldFailOnMessageCode2()
        {
            var ri = new TestForbearanceRI();
            ri.MessageCode = "50108";
            var fh = new ForbearanceHelper(ri);
            var dummy = new ProcessQueueData();
            var results = fh.AddForbearance(dummy);
            Assert.IsFalse(results.ForbearanceAdded);
        }

        [TestMethod]
        public void ShouldSucceedOnSuccessMessageCode() //Test interaction with TSX7E screen
        {
            var ri = new TestForbearanceRI();
            ri.MessageCode = "01004";
            ri.ScreenCode = "TSX7E";
            var fh = new ForbearanceHelper(ri);
            var dummy = new ProcessQueueData();
            var results = fh.AddForbearance(dummy);
            Assert.IsTrue(results.ForbearanceAdded);
        }

        [TestMethod]
        public void ShouldFailOnUnexpectedScreen1() //Test interaction with unexpected screen
        {
            var ri = new TestForbearanceRI();
            ri.MessageCode = "01004";
            ri.ScreenCode = "TSXBAD";
            var fh = new ForbearanceHelper(ri);
            var dummy = new ProcessQueueData();
            var results = fh.AddForbearance(dummy);
            Assert.IsFalse(results.ForbearanceAdded);
        }

        [TestMethod]
        public void ShouldFailOnUnexpectedScreen2() //Test interaction with unexpected screen
        {
            var ri = new TestForbearanceRI();
            ri.MessageCode = "01004";
            ri.ScreenCode = "TSX31";
            var fh = new ForbearanceHelper(ri);
            var dummy = new ProcessQueueData();
            var results = fh.AddForbearance(dummy);
            Assert.IsFalse(results.ForbearanceAdded);
        }

        [TestMethod]
        public void ShouldFailOnMessageCode1Override()
        {
            var ri = new TestForbearanceRI();
            ri.MessageCode = "01527";
            var fh = new ForbearanceHelper(ri);
            var dummy = new ProcessQueueData();
            dummy.BeginDate = dummy.BeginDate.AddDays(1);
            var results = fh.AddDelinquencyOverrideForbearance(dummy);
            Assert.IsFalse(results.ForbearanceAdded);
        }

        [TestMethod]
        public void ShouldFailOnMessageCode2Override()
        {
            var ri = new TestForbearanceRI();
            ri.MessageCode = "50108";
            var fh = new ForbearanceHelper(ri);
            var dummy = new ProcessQueueData();
            dummy.BeginDate = dummy.BeginDate.AddDays(1);
            var results = fh.AddDelinquencyOverrideForbearance(dummy);
            Assert.IsFalse(results.ForbearanceAdded);
        }

        [TestMethod]
        public void ShouldSucceedOnSuccessMessageCodeOverride() //Test interaction with TSX7E screen
        {
            var ri = new TestForbearanceRI();
            ri.MessageCode = "01004";
            ri.ScreenCode = "TSX7E";
            var fh = new ForbearanceHelper(ri);
            var dummy = new ProcessQueueData();
            dummy.BeginDate = dummy.BeginDate.AddDays(1);
            var results = fh.AddDelinquencyOverrideForbearance(dummy);
            Assert.IsTrue(results.ForbearanceAdded);
        }

        [TestMethod]
        public void ShouldFailOnUnexpectedScreen1Override() //Test interaction with unexpected screen
        {
            var ri = new TestForbearanceRI();
            ri.MessageCode = "01004";
            ri.ScreenCode = "TSXBAD";
            var fh = new ForbearanceHelper(ri);
            var dummy = new ProcessQueueData();
            dummy.BeginDate = dummy.BeginDate.AddDays(1);
            var results = fh.AddDelinquencyOverrideForbearance(dummy);
            Assert.IsFalse(results.ForbearanceAdded);
        }

        [TestMethod]
        public void ShouldFailOnUnexpectedScreen2Override() //Test interaction with unexpected screen
        {
            var ri = new TestForbearanceRI();
            ri.MessageCode = "01004";
            ri.ScreenCode = "TSX31";
            var fh = new ForbearanceHelper(ri);
            var dummy = new ProcessQueueData();
            dummy.BeginDate = dummy.BeginDate.AddDays(1);
            var results = fh.AddDelinquencyOverrideForbearance(dummy);
            Assert.IsFalse(results.ForbearanceAdded);
        }

        [TestMethod]
        public void ShouldReturnNullIfDelinquencyAfterBeginOverride() //Test interaction with unexpected screen
        {
            var ri = new TestForbearanceRI();
            var fh = new ForbearanceHelper(ri);
            var dummy = new ProcessQueueData();
            var results = fh.AddDelinquencyOverrideForbearance(dummy);
            Assert.IsTrue(results == null);
        }

    }
}

using KEYIDCHNG;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Uheaa.Common.Scripts;

namespace KEYIDCHNG_Tests
{
    [TestClass]
    public class KeyIdentifierComparisonTests
    {
        KeyIdentifierChangeModel defaultModel;
        SystemBorrowerDemographics defaultDemos;
        public KeyIdentifierComparisonTests()
        {
            defaultModel = new KeyIdentifierChangeModel()
            {
                FirstName = "John",
                MiddleInitial = "X",
                LastName = "Mothman",
                DOB = new DateTime(1999, 9, 9)
            };

            defaultDemos = new SystemBorrowerDemographics()
            {
                FirstName = defaultModel.FirstName,
                MiddleIntial = defaultModel.MiddleInitial,
                LastName = defaultModel.LastName,
                DateOfBirth = defaultModel.DOB.Value.ToShortDateString()
            };
        }

        private KeyIdentifierChangeComparison GetComparison()
        {
            return new KeyIdentifierChangeComparison(defaultModel, defaultDemos);
        }
        [TestMethod]
        public void FirstNameNotChanged()
        {
            Assert.IsFalse(GetComparison().FirstNameChanged);
        }
        [TestMethod]
        public void FirstNameChanged()
        {
            defaultModel.FirstName = "Jameson";
            Assert.IsTrue(GetComparison().FirstNameChanged);
        }

        [TestMethod]
        public void MiddleInitialNotChanged()
        {
            Assert.IsFalse(GetComparison().MiddleInitialChanged);
        }
        [TestMethod]
        public void MiddleInitialChanged()
        {
            defaultModel.MiddleInitial = "J";
            Assert.IsTrue(GetComparison().MiddleInitialChanged);
        }
        [TestMethod]
        public void LastNameNotChanged()
        {
            Assert.IsFalse(GetComparison().LastNameChanged);
        }
        [TestMethod]
        public void LastNameChanged()
        {
            defaultModel.LastName = "Charlemagne";
            Assert.IsTrue(GetComparison().LastNameChanged);
        }
    }
}

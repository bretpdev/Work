using System;
using System.Linq;
using System.Transactions;
using NUnit.Framework;
using CentralTracking;

namespace CentralTrackingTesting
{
    [TestFixture]
    public class ValueTest : BaseTest
    {
        CentralTrackingEntities ct;
        string Description = "Test Value 1";
        [SetUp]
        public override void Init()
        {
            base.Init();
            ct = new CentralTrackingEntities();
        }

        [TearDown]
        public override void CleanUp()
        {
            base.CleanUp();
            ct.Dispose();
        }

        [Test]
        public void ShouldFindExistingValueByStringValue()
        {
            Value newValue = ct.Values.AddOrFindByStringValue(Description);
            ct.SaveChanges();
            Assert.AreEqual(newValue, ct.Values.FindByStringValue(Description), "Expected the Value's string value to match string value inserted into the database");
        }

        [Test]
        public void ShouldAddNewValueByStringValue()
        {
            Assert.IsNull(ct.Values.FindByStringValue(Description), "Expected the Value would not exist but it does");
            ct.Values.AddOrFindByStringValue(Description);
            ct.SaveChanges();
            Assert.IsNotNull(ct.Values.FindByStringValue(Description), "Expected the Value would exist but it does not");
        }
    }
}

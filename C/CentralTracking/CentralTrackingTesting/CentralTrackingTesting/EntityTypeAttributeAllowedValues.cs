using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using NUnit.Framework;
using CentralTracking;

namespace CentralTrackingTesting
{
    class EntityTypeAttributeAllowedValues : BaseTest
    {
        CentralTrackingEntities ct;

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
        public void ShouldAddNewEntityTypeAttributeAllowedValue()
        {
            EntityTypeAttributeAllowedValue et = new EntityTypeAttributeAllowedValue()
            {
                EntityTypeAttributeId = 1,
                ValueId = 1
            };

            ct.EntityTypeAttributeAllowedValues.Add(et);
            ct.SaveChanges();

            Assert.IsTrue(et.EntityTypeAttributeAllowedValuesId > 0);
        }
    }
}

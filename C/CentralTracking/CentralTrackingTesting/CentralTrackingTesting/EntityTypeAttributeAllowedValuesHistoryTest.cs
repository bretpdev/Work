using System;
using System.Linq;
using System.Transactions;
using NUnit.Framework;
using CentralTracking;

namespace CentralTrackingTesting
{
    class EntityTypeAttributeAllowedValuesHistoryTest : BaseTest
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
    }
}

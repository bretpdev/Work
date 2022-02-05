using System;
using System.Linq;
using System.Transactions;
using NUnit.Framework;
using CentralTracking;

namespace CentralTrackingTesting
{
    [TestFixture]
    public class EntityTypeTest : BaseTest
    {
        public const string Description = "Test Entity Type 1";
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
        public void ShouldFindExistingEntityType()
        {
            EntityType et = ct.EntityTypes.AddOrFind(Description);
            ct.SaveChanges();
            EntityType matchEntity = ct.EntityTypes.AddOrFind(Description);
            ct.SaveChanges();
            Assert.AreSame(et, matchEntity, "Expected AddOrFind to return the same EntityType but it returned a different EntityType");
        }

        [Test]
        public void ShouldFindEntityTypeByDescription()
        {
            EntityType et = ct.EntityTypes.AddOrFind(Description);
            ct.SaveChanges();
            Assert.IsNotNull(ct.EntityTypes.FindByDescription(Description), "Expected to find an EntityType by description");
        }

        [Test]
        public void ShouldSetCreatedAt()
        {
                        EntityType et = ct.EntityTypes.AddOrFind(Description);
            ct.SaveChanges();
            Assert.AreNotEqual(et.CreatedAt, DateTime.MinValue, "Expected CreatedAt to not equal {0}", DateTime.MinValue);
            Assert.IsNotNull(et.CreatedAt, string.Format("Expected CreatedAt to be populated"));
        }

        [Test]
        public void ShouldInactivateAnActiveEntityType()
        {
            EntityType et = ct.EntityTypes.AddOrFind(Description);
            ct.SaveChanges();
            et.Inactivate();
            ct.SaveChanges();
            Assert.IsFalse(et.Active, String.Format("Expected to inactivate EntityType {0}, but it did not", et.EntityTypeDescription));
        }

        [Test]
        public void ShouldSetInactivatedByWhenInactivated()
        {
            EntityType et = ct.EntityTypes.AddOrFind(Description);
            ct.SaveChanges();
            et.Inactivate();
            ct.SaveChanges();
            Assert.AreEqual((int)et.InactivatedBy, LoginHelper.CurrentUser.Id, string.Format("Expected InactivatedBy to be {0} but was {1}", LoginHelper.CurrentUser.Id, et.InactivatedBy));
        }

        //[Test]
        //public void ShouldSetInactivatedAtWhenInactivated()
        //{
        //    EntityType et = ct.EntityTypes.AddOrFind(Description);
        //    ct.SaveChanges();
        //    et.Inactivate();
        //    ct.SaveChanges();
        //    Assert.AreNotEqual(et.InactivatedAt, DateTime.MinValue, "Expected InactivatedAt to not equal {0}", DateTime.MinValue);
        //    Assert.IsNotNull(et.InactivatedAt, string.Format("Expected InactivatedAt to be populated"));
        //}


        [Test]
        public void ShouldActivateAnInactiveEntityType()
        {
            EntityType et = ct.EntityTypes.AddOrFind(Description);
            ct.SaveChanges();
            et.Inactivate();
            ct.SaveChanges();
            et.Activate();
            ct.SaveChanges();
            Assert.IsTrue(et.Active, "Expected to re-activate user but it did not");
        }

        [Test]
        public void ShouldUpdateInactivatedByToNullWhenEntityTypeReactivated()
        {
            EntityType et = ct.EntityTypes.AddOrFind(Description);
            ct.SaveChanges();
            et.Inactivate();
            ct.SaveChanges();
            et.Activate();
            ct.SaveChanges();
            Assert.IsNull(et.InactivatedBy, string.Format("Expected InactivatedBy to be null but was {0}", et.InactivatedBy));
        }

        [Test]
        public void ShouldUpdateInactivatedAtToNullWhenEntityTypeReactivated()
        {
            EntityType et = ct.EntityTypes.AddOrFind(Description);
            ct.SaveChanges();
            et.Inactivate();
            ct.SaveChanges();
            et.Activate();
            ct.SaveChanges();
            Assert.IsNull(et.InactivatedAt, string.Format("Expected InactivatedAt to be null but it was {0}", et.InactivatedAt));
        }

        [Test]
        public void ShouldAddNewEntityType()
        {
            Assert.IsNull(ct.EntityTypes.FindByDescription(Description), "Expected the EntityType would not exist but it does");
            EntityType et = ct.EntityTypes.AddOrFind(Description);
            ct.SaveChanges();
            Assert.IsNotNull(et, "Expected the EntityType to exist but it does not");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using NUnit.Framework;
using CentralTracking;

namespace CentralTrackingTesting
{
    [TestFixture]
    public class EntityTypeAttributeTest : BaseTest
    {
        public const int EntityTypeId = 1;
        public const int AttributeId = 1;
        CentralTrackingEntities ct;
        EntityTypeAttribute Eta;

        [SetUp]
        public override void Init()
        {
            base.Init();
            ct = new CentralTrackingEntities();
            Eta = new EntityTypeAttribute()
            {
                EntityTypeId = EntityTypeId,
                AttributeId = AttributeId,
                CreatedBy = LoginHelper.CurrentUser.Id
            };
        }

        [TearDown]
        public override void CleanUp()
        {
            base.CleanUp();
            ct.Dispose();
        }

        [Test]
        public void ShouldAddNewEntityTypeAttribute()
        {
            Assert.IsNull(ct.EntityTypeAttributes.FindByEntityTypeAndAttribute(EntityTypeId, AttributeId), "Expected the EntityTypeAttribute would not exist but it does");
            ct.EntityTypeAttributes.Add(Eta);
            ct.SaveChanges();
            Assert.IsNotNull(ct.EntityTypeAttributes.FindByEntityTypeAndAttribute(EntityTypeId, AttributeId), "Expected the EntityTypeAttribute would exist but it does not");
        }

        [Test]
        public void ShouldFindExistingEntityTypeAttributeByEntityTypeIdAndAttributeId()
        {
            ct.EntityTypeAttributes.Add(Eta);
            ct.SaveChanges();
            EntityTypeAttribute foundEta = ct.EntityTypeAttributes.FindByEntityTypeAndAttribute(Eta.EntityTypeId, Eta.AttributeId);
            Assert.AreSame(Eta, foundEta, "Expected to find the same EntityTypeAttribute but it returned a different EntityTypeAttribute");
        }

        [Test]
        public void ShouldFindExistingEntityTypeAttributeByEntityTypeId()
        {
            int objectCount = 4;
            
            for(int count = 1; count <= objectCount; count++)
                ct.EntityTypeAttributes.Add(new EntityTypeAttribute() { EntityTypeId = 1, AttributeId = count, CreatedBy = 0 });

            ct.SaveChanges();
            List<EntityTypeAttribute> foundEta = ct.EntityTypeAttributes.FindByEntityType(Eta.EntityTypeId);
            Assert.AreEqual(objectCount, foundEta.Count, "Expected to find the same EntityTypeAttribute but it returned a different EntityTypeAttribute");
        }

        [Test]
        public void ShouldInactivateAnActiveAttribute()
        {
            EntityTypeAttribute eta = ct.EntityTypeAttributes.Add(new EntityTypeAttribute() { EntityTypeId = 1, AttributeId = 1, CreatedBy = 0 });
            ct.SaveChanges();
            eta.Inactivate();
            ct.SaveChanges();
            Assert.IsFalse(eta.Active, "Expected to inactivate EntityTypeAttribute but it did not");
        }

        [Test]
        public void ShouldSetInactivatedByWhenInactivated()
        {
            EntityTypeAttribute eta = ct.EntityTypeAttributes.Add(new EntityTypeAttribute() { EntityTypeId = 1, AttributeId = 1, CreatedBy = 0 });
            ct.SaveChanges();
            eta.Inactivate();
            ct.SaveChanges();
            Assert.AreEqual((int)eta.InactivatedBy, LoginHelper.CurrentUser.Id, string.Format("Expected InactivatedBy to be {0} but was {1}", LoginHelper.CurrentUser.Id, eta.InactivatedBy));
        }

        [Test]
        public void ShouldSetInactivatedAtWhenInactivatedAttribute()
        {
            EntityTypeAttribute eta = ct.EntityTypeAttributes.Add(new EntityTypeAttribute() { EntityTypeId = 1, AttributeId = 1, CreatedBy = 0 });
            ct.SaveChanges();
            eta.Inactivate();
            ct.SaveChanges();
            Assert.IsNotNull(eta.InactivatedAt, string.Format("Expected InactivatedAt to be populated"));
        }

        [Test]
        public void ShouldActivateAnInactiveAttribute()
        {
            EntityTypeAttribute eta = ct.EntityTypeAttributes.Add(new EntityTypeAttribute() { EntityTypeId = 1, AttributeId = 1, CreatedBy = 0 });
            ct.SaveChanges();
            eta.Inactivate();
            ct.SaveChanges();
            eta.Activate();
            ct.SaveChanges();
            Assert.IsTrue(eta.Active, string.Format("Expected to re-activate AttributeId {0} but it did not", eta.EntityTypeAttributeId));
        }

        [Test]
        public void ShouldUpdateInactivatedByToNullWhenAttributeReactivated()
        {
            EntityTypeAttribute eta = ct.EntityTypeAttributes.Add(new EntityTypeAttribute() { EntityTypeId = 1, AttributeId = 1, CreatedBy = 0 });
            ct.SaveChanges();
            eta.Inactivate();
            ct.SaveChanges();
            eta.Activate();
            ct.SaveChanges();
            Assert.IsNull(eta.InactivatedBy, string.Format("Expected InactivatedBy to be null but was {0}", eta.InactivatedBy));
        }

        [Test]
        public void ShouldUpdateInactivatedAtToNullWhenAttributeReactivated()
        {
            EntityTypeAttribute eta = ct.EntityTypeAttributes.Add(new EntityTypeAttribute() { EntityTypeId = 1, AttributeId = 1, CreatedBy = 0 });
            ct.SaveChanges();
            eta.Inactivate();
            ct.SaveChanges();
            eta.Activate();
            ct.SaveChanges();
            Assert.IsNull(eta.InactivatedAt, string.Format("Expected InactivatedAt to be null but it was {0}", eta.InactivatedAt));
        }
    }
}

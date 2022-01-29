using System;
using System.Linq;
using System.Transactions;
using NUnit.Framework;
using CentralTracking;

namespace CentralTrackingTesting
{
    [TestFixture]
    public class EntityTest : BaseTest
    {
        CentralTrackingEntities ct;
        Entity e;

        [SetUp]
        public override void Init()
        {
            base.Init();

            string entityName = "Test Entity 1";
            int entityType = 1; // User
            
            ct = new CentralTrackingEntities();
            e = ct.Entities.AddByEntityNameAndType(entityName, entityType);
            ct.SaveChanges();
        }

        [TearDown]
        public override void CleanUp()
        {
            base.CleanUp();
            ct.Dispose();
        }

        [Test]
        public void ShouldAddNewEntity()
        {
            string entityName = "Test Entity 2";
            int entityType = 1; // User

            Assert.IsEmpty(ct.Entities.FindByEntityName(entityName), String.Format("Expected that entity {0} would not exist, but it does", entityName));
            Entity newEntity = ct.Entities.AddByEntityNameAndType(entityName, entityType);
            ct.SaveChanges();

            Assert.AreSame(newEntity, ct.Entities.FindByEntityName(entityName).First(), "Expected entities to be the same but they were different.");
        }

        [Test]
        public void ShouldSaveAnEntitiesEntityValues()
        {
            Assert.Fail("Test not implemented");
        }

        [Test]
        public void ShouldSetCreatedBy()
        {
            Assert.AreEqual(LoginHelper.CurrentUser.Id, e.CreatedBy, String.Format("Expected the CreatedBy property to equal {0}, but it was {1}", LoginHelper.CurrentUser.Id, e.CreatedBy));
        }

        [Test]
        public void ShouldSetCreatedAt()
        {
            Assert.AreNotEqual(e.CreatedAt, DateTime.MinValue, "Expected CreatedAt to not equal {0}", DateTime.MinValue);
            Assert.IsNotNull(e.CreatedAt, string.Format("Expected CreatedAt to be populated"));
        }

        [Test]
        public void ShouldSetEntityName()
        {
            Assert.Fail("Test not implemented");
        }

         [Test]
        public void ShouldFindExistingEntityByEntityId()
        {
            Assert.AreSame(e, ct.Entities.FindByEntityId(e.EntityId), String.Format("Expected that EntityId {0}; EntityName {1} would be found, but it was not", e.EntityId, e.EntityName));
        }


        [Test]
        public void ShouldFindExistingEntityByEntityName()
        {
            Assert.AreSame(e, ct.Entities.FindByEntityName(e.EntityName).First(), String.Format("Expected that EntityId {0}; EntityName {1} would be found, but it was not", e.EntityId, e.EntityName));
        }

        [Test]
        public void ShouldInactivateAnActiveEntity()
        {
            e.Inactivate();
            ct.SaveChanges();
            Assert.IsFalse(e.Active, String.Format("Expected to inactivate entity {0} but it did not", e.EntityName));
        }

        [Test]
        public void ShouldSetInactivatedByWhenInactivated()
        {
            e.Inactivate();
            ct.SaveChanges();
            Assert.IsNotNull(e.InactivatedBy, string.Format("Expected InactivatedBy to be populated"));
        }

        [Test]
        public void ShouldSetInactivatedAtWhenInactivated()
        {
            e.Inactivate();
            ct.SaveChanges();
            Assert.AreNotEqual(e.InactivatedAt, DateTime.MinValue, "Expected InactivatedAt to not equal {0}", DateTime.MinValue);
            Assert.IsNotNull(e.InactivatedAt, string.Format("Expected InactivatedAt to be populated"));
        }

        [Test]
        public void ShouldActivateAnInactiveEntity()
        {
            e.Inactivate();
            ct.SaveChanges();
            e.Activate(); 
            ct.SaveChanges();
            Assert.IsTrue(e.Active, string.Format("Expected to EntityId {0} to be Active and it was Inactive",e.EntityId));
        }

        [Test]
        public void ShouldUpdateInactivatedByToNullWhenEntityReactivated()
        {
            e.Inactivate();
            ct.SaveChanges();
            e.Activate();
            ct.SaveChanges();
            Assert.IsNull(e.InactivatedBy, string.Format("Expected EntityId {0} InactivatedBy to be null but it was {1}.", e.EntityTypeId, e.InactivatedBy));
        }

        [Test]
        public void ShouldUpdateInactivatedAtToNullWhenEntityReactivated()
        {
            e.Inactivate();
            ct.SaveChanges();
            e.Activate();
            ct.SaveChanges();
            Assert.IsNull(e.InactivatedAt, string.Format("Expected EntityId {0} InactivatedAt to be null but it was {1}.", e.EntityTypeId, e.InactivatedBy));
        }
    }
}

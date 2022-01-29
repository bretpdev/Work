using System;
using System.Linq;
using System.Transactions;
using NUnit.Framework;
using CentralTracking;

namespace CentralTrackingTesting
{
    [TestFixture]
    public class AttributeSelectionTypeTest : BaseTest
    {
        public const string Description = "Multiple Select";
        public CentralTrackingEntities ct;

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
        public void ShouldFindAttributeSelectionTypeByDescription()
        {
            AttributeSelectionType selectType = ct.AttributeSelectionTypes.AddByDescription(Description);
            ct.SaveChanges();
            Assert.IsNotNull(ct.AttributeSelectionTypes.FindByDescription(Description), "Expected to find an AttributeSelectionType by description");
        }

        [Test]
        public void ShouldInactivateAnActiveAttributeSelectionType()
        {
            AttributeSelectionType selectType = ct.AttributeSelectionTypes.AddByDescription(Description);
            ct.SaveChanges();
            selectType.Inactivate();
            ct.SaveChanges();
            Assert.IsFalse(selectType.Active, "Expected to inactivate user but it did not");
        }

        [Test]
        public void ShouldSetInactivatedByWhenInactivated()
        {
            AttributeSelectionType selectType = ct.AttributeSelectionTypes.AddByDescription(Description);
            ct.SaveChanges();
            selectType.Inactivate();
            ct.SaveChanges();
            Assert.AreEqual((int)selectType.InactivatedBy, LoginHelper.CurrentUser.Id, string.Format("Expected InactivatedBy to be {0} but was {1}", LoginHelper.CurrentUser.Id, selectType.InactivatedBy));
        }

        [Test]
        public void ShouldSetInactivatedAtWhenInactivated()
        {
            AttributeSelectionType selectType = ct.AttributeSelectionTypes.AddByDescription(Description);
            ct.SaveChanges();
            selectType.Inactivate();
            ct.SaveChanges();
            Assert.IsNotNull(selectType.InactivatedAt, string.Format("Expected InactivatedAt to be populated"));
        }

        [Test]
        public void ShouldActivateAnInactiveAttributeSelectionType()
        {
            AttributeSelectionType selectType = ct.AttributeSelectionTypes.AddByDescription(Description);
            ct.SaveChanges();
            selectType.Inactivate();
            ct.SaveChanges();
            selectType.Activate();
            ct.SaveChanges();
            Assert.IsTrue(selectType.Active, "Expected to re-activate user but it did not");
        }

        [Test]
        public void ShouldUpdateInactivatedByToNullWhenAttributeSelectionTypeReactivated()
        {
            AttributeSelectionType selectType = ct.AttributeSelectionTypes.AddByDescription(Description);
            ct.SaveChanges();
            selectType.Inactivate();
            ct.SaveChanges();
            selectType.Activate();
            ct.SaveChanges();
            Assert.IsNull(selectType.InactivatedBy, string.Format("Expected InactivatedBy to be null but was {0}", selectType.InactivatedBy));
        }

        [Test]
        public void ShouldUpdateInactivatedAtToNullWhenAttributeSelectionTypeReactivated()
        {
            AttributeSelectionType selectType = ct.AttributeSelectionTypes.AddByDescription(Description);
            ct.SaveChanges();
            selectType.Inactivate();
            ct.SaveChanges();
            selectType.Activate();
            ct.SaveChanges();
            Assert.IsNull(selectType.InactivatedAt, string.Format("Expected InactivatedAt to be null but it was {0}", selectType.InactivatedAt));
        }

        [Test]
        public void ShouldAddNewAttributeSelectionType()
        {
            Assert.IsNull(ct.AttributeSelectionTypes.FindByDescription(Description), "Expected the AttributeSelectionType would not exist but it does");
            AttributeSelectionType selectType = ct.AttributeSelectionTypes.AddByDescription(Description);
            ct.SaveChanges();
            Assert.IsNotNull(selectType, "Expected the AttributeSelectionType to exist but it does not");
        }
    }
}

using System;
using System.Linq;
using System.Transactions;
using NUnit.Framework;
using CentralTracking;

namespace CentralTrackingTesting
{
    [TestFixture]
    public class AttributeDataTypeTest : BaseTest
    {
        public const string Description = "Double";
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
        public void ShouldFindAttributeDataTypeByDescription()
        {
            AttributeDataType dataType = ct.AttributeDataTypes.AddByDescription(Description);
            ct.SaveChanges();
            Assert.IsNotNull(ct.AttributeDataTypes.FindByDescription(Description), "Expected to find an AttributeDataType by description");
        }

        [Test]
        public void ShouldInactivateAnActiveAttributeDataType()
        {
            AttributeDataType dataType = ct.AttributeDataTypes.AddByDescription(Description);
            ct.SaveChanges();
            dataType.Inactivate();
            ct.SaveChanges();
            Assert.IsFalse(dataType.Active, "Expected to inactivate user but it did not");
        }

        [Test]
        public void ShouldSetInactivatedByWhenInactivated()
        {
            AttributeDataType dataType = ct.AttributeDataTypes.AddByDescription(Description);
            ct.SaveChanges();
            dataType.Inactivate();
            ct.SaveChanges();
            Assert.AreEqual((int)dataType.InactivatedBy, LoginHelper.CurrentUser.Id, string.Format("Expected InactivatedBy to be {0} but was {1}", LoginHelper.CurrentUser.Id, dataType.InactivatedBy));
        }

        [Test]
        public void ShouldSetInactivatedAtWhenInactivated()
        {
            AttributeDataType dataType = ct.AttributeDataTypes.AddByDescription(Description);
            ct.SaveChanges();
            dataType.Inactivate();
            ct.SaveChanges();
            Assert.IsNotNull(dataType.InactivatedAt, string.Format("Expected InactivatedAt to be populated"));
        }

        [Test]
        public void ShouldActivateAnInactiveAttributeDataType()
        {
            AttributeDataType dataType = ct.AttributeDataTypes.AddByDescription(Description);
            ct.SaveChanges();
            dataType.Inactivate();
            ct.SaveChanges();
            dataType.Activate();
            ct.SaveChanges();
            Assert.IsTrue(dataType.Active, "Expected to re-activate user but it did not");
        }

        [Test]
        public void ShouldUpdateInactivatedByToNullWhenAttributeDataTypeReactivated()
        {
            AttributeDataType dataType = ct.AttributeDataTypes.AddByDescription(Description);
            ct.SaveChanges();
            dataType.Inactivate();
            ct.SaveChanges();
            dataType.Activate();
            ct.SaveChanges();
            Assert.IsNull(dataType.InactivatedBy, string.Format("Expected InactivatedBy to be null but was {0}", dataType.InactivatedBy));
        }

        [Test]
        public void ShouldUpdateInactivatedAtToNullWhenAttributeDataTypeReactivated()
        {
            AttributeDataType dataType = ct.AttributeDataTypes.AddByDescription(Description);
            ct.SaveChanges();
            dataType.Inactivate();
            ct.SaveChanges();
            dataType.Activate();
            ct.SaveChanges();
            Assert.IsNull(dataType.InactivatedAt, string.Format("Expected InactivatedAt to be null but it was {0}", dataType.InactivatedAt));
        }

        [Test]
        public void ShouldAddNewAttributeDataType()
        {
            Assert.IsNull(ct.AttributeDataTypes.FindByDescription(Description), "Expected the AttributeDataType would not exist but it does");
            AttributeDataType dataType = ct.AttributeDataTypes.AddByDescription(Description);
            ct.SaveChanges();
            Assert.IsNotNull(dataType, "Expected the AttributeDataType to exist but it does not");
        }
    }
}

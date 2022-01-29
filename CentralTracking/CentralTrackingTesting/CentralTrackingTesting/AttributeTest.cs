using System;
using System.Linq;
using System.Transactions;
using NUnit.Framework;
using CentralTracking;

namespace CentralTrackingTesting
{
    [TestFixture]
    public class AttributeTest : BaseTest
    {
        CentralTrackingEntities ct;
        CentralTracking.Attribute Att;
        private string Description = "Attribute Unit Test 1";
        private string LongDescription = "Attribute Unit Test 1 Long Description";

        [SetUp]
        public override void Init()
        {
            base.Init();
            ct = new CentralTrackingEntities();
            Att = new CentralTracking.Attribute()
            {
                AttributeDescription = Description,
                AttributeLongDescription = LongDescription,
                AttributeDataTypeId = 1,
                AttributeSelectionTypeId = 1,
                CreatedBy = LoginHelper.CurrentUser.Id
            };
        }

        [TearDown]
        public override void CleanUp()
        {
            base.CleanUp();
            ct.Dispose();
            Att = null;
        }

        [Test]
        public void ShouldFindExistingAttribute()
        {
            ct.Attributes.Add(Att);
            ct.SaveChanges();
            CentralTracking.Attribute foundAttribute = ct.Attributes.FindByDescription(Description);
            Assert.AreSame(Att, foundAttribute, "Expected AddOrFind to return the same EntityType but it returned a different EntityType");
        }

        [Test]
        public void ShouldInactivateAnActiveAttribute()
        {
            ct.Attributes.Add(Att);
            ct.SaveChanges();
            Att.Inactivate();
            ct.SaveChanges();
            Assert.IsFalse(Att.Active, "Expected to inactivate user but it did not");
        }

        [Test]
        public void ShouldSetInactivatedByWhenInactivated()
        {
            ct.Attributes.Add(Att);
            ct.SaveChanges();
            Att.Inactivate();
            ct.SaveChanges();
            Assert.AreEqual((int)Att.InactivatedBy, LoginHelper.CurrentUser.Id, string.Format("Expected InactivatedBy to be {0} but was {1}", LoginHelper.CurrentUser.Id, Att.InactivatedBy));
        }

        [Test]
        public void ShouldSetInactivatedAtWhenInactivatedAttribute()
        {
            ct.Attributes.Add(Att);
            ct.SaveChanges();
            Att.Inactivate();
            ct.SaveChanges();
            Assert.IsNotNull(Att.InactivatedAt, string.Format("Expected InactivatedAt to be populated"));
        }

        [Test]
        public void ShouldActivateAnInactiveAttribute()
        {
            ct.Attributes.Add(Att);
            ct.SaveChanges();
            Att.Inactivate();
            ct.SaveChanges();
            Att.Activate();
            ct.SaveChanges();
            Assert.IsTrue(Att.Active, string.Format("Expected to re-activate AttributeId {0} but it did not", Att.AttributeId));
        }

        [Test]
        public void ShouldUpdateInactivatedByToNullWhenAttributeReactivated()
        {
            ct.Attributes.Add(Att);
            ct.SaveChanges();
            Att.Inactivate();
            ct.SaveChanges();
            Att.Activate();
            ct.SaveChanges();
            Assert.IsNull(Att.InactivatedBy, string.Format("Expected InactivatedBy to be null but was {0}", Att.InactivatedBy));
        }

        [Test]
        public void ShouldUpdateInactivatedAtToNullWhenAttributeReactivated()
        {
            ct.Attributes.Add(Att);
            ct.SaveChanges();
            Att.Inactivate();
            ct.SaveChanges();
            Att.Activate();
            ct.SaveChanges();
            Assert.IsNull(Att.InactivatedAt, string.Format("Expected InactivatedAt to be null but it was {0}", Att.InactivatedAt));
        }

        [Test]
        public void ShouldAddNewAttribute()
        {
            Assert.IsNull(ct.Attributes.FindByDescription(Description), "Expected the Attribute would not exist but it does");
            CentralTracking.Attribute newAtt = ct.Attributes.Add(Att);
            ct.SaveChanges();
            Assert.IsNotNull(ct.Attributes.FindByDescription(Description), "Expected the Attribute would exist but it does not");
            Assert.AreEqual(Att.AttributeDescription, newAtt.AttributeDescription, string.Format("Expected AttributeDescription to be {0} but it was {1}", Att.AttributeDescription, newAtt.AttributeDescription));
            Assert.AreEqual(Att.AttributeLongDescription, newAtt.AttributeLongDescription, string.Format("Expected AttributeLongDescription to be {0} but it was {1}", Att.AttributeLongDescription, newAtt.AttributeLongDescription));
            Assert.AreEqual(Att.AttributeDataTypeId, newAtt.AttributeDataTypeId, string.Format("Expected AttributeDataTypeId to be {0} but it was {1}", Att.AttributeDataTypeId, newAtt.AttributeDataTypeId));
            Assert.AreEqual(Att.AttributeSelectionTypeId, newAtt.AttributeSelectionTypeId, string.Format("Expected AttributeSelectionTypeId to be {0} but it was {1}", Att.AttributeSelectionTypeId, newAtt.AttributeSelectionTypeId));
            Assert.AreEqual(Att.CreatedBy, newAtt.CreatedBy, string.Format("Expected CreatedAt to be {0} but it was {1}", Att.CreatedBy, newAtt.CreatedBy));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using NUnit.Framework;
using CentralTracking;

namespace CentralTrackingTesting
{
    [TestFixture]
    public class EntityAttributeValueTest : BaseTest
    {
        CentralTrackingEntities ct;
        [SetUp]
        public override void Init()
        {
            base.Init();
            ct = new CentralTrackingEntities();
            LoginHelper.CurrentUser.Id = 3;
        }

        [TearDown]
        public override void CleanUp()
        {
            base.CleanUp();
            ct.Dispose();
        }

        [Test]
        public void ShouldUpdateCreatedByForModifiedAttributeValues()
        {
            List<EntityAttributeValue> eta = ct.EntityAttributeValues.ToList();
            foreach (EntityAttributeValue item in eta)
                item.ValueId = item.ValueId + 1;

            ct.SaveChanges();

            foreach (EntityAttributeValue item in eta)
                Assert.AreEqual(item.CreatedBy, LoginHelper.CurrentUser.Id,
                    string.Format("Expected CreatedBy to be {0} for EntityAttributeValueId {1} but it was {2}", LoginHelper.CurrentUser.Id, item.EntityAttributeValueId, item.CreatedBy));
        }

        [Test]
        public void ShouldNotUpdateCreatedByForUnModifiedAttributeValues()
        {
            List<EntityAttributeValue> eta = ct.EntityAttributeValues.ToList();
            List<EntityAttributeValue> unmodifiedEta = eta;

            foreach (EntityAttributeValue item in eta.Where(p => p.EntityId == 2))
                item.ValueId = item.ValueId + 1;

            ct.SaveChanges();

            foreach (EntityAttributeValue item in eta.Where(p => p.EntityId == 2))
                Assert.AreEqual(item.CreatedBy, LoginHelper.CurrentUser.Id,
                    string.Format("Expected CreatedBy to be {0} for EntityAttributeValueId {1} but it was {2}", LoginHelper.CurrentUser.Id, item.EntityAttributeValueId, item.CreatedBy));

            foreach (EntityAttributeValue item in eta.Where(p => p.EntityId != 2))
                Assert.AreEqual(item.CreatedBy, unmodifiedEta.Where(p => p.EntityAttributeValueId == item.EntityAttributeValueId).Select(q => q.CreatedBy).SingleOrDefault(),
                    string.Format("Expected CreatedBy to be {0} for EntityAttributeValueId {1} but it was {2}", unmodifiedEta.Where(p => p.EntityAttributeValueId == item.EntityAttributeValueId).Select(q => q.CreatedBy).SingleOrDefault()
                        , item.EntityAttributeValueId, item.CreatedBy));
        }

        [Test]
        public void ShouldAllowInsertingOfMultipleAnswerSelectionType()
        {
            CentralTracking.Attribute att = new CentralTracking.Attribute()
            {
                AttributeDescription = "Favorite Color",
                AttributeLongDescription = " ",
                AttributeDataTypeId = 3,
                AttributeSelectionTypeId = 3,
                CreatedBy = 0
            };
            ct.Attributes.Add(att);

            ct.SaveChanges();

            EntityAttributeValue eta1 = new EntityAttributeValue()
            {
                EntityId = 1,
                AttributeId = att.AttributeId,
                ValueId = 1,
                CreatedBy = 0
            };

            EntityAttributeValue eta2 = new EntityAttributeValue()
            {
                EntityId = 1,
                AttributeId = att.AttributeId,
                ValueId = 2,
                CreatedBy = 0
            };

            EntityAttributeValue eta3 = new EntityAttributeValue()
            {
                EntityId = 1,
                AttributeId = att.AttributeId,
                ValueId = 3,
                CreatedBy = 0
            };

            ct.EntityAttributeValues.Add(eta1);
            ct.EntityAttributeValues.Add(eta2);
            ct.EntityAttributeValues.Add(eta3);
            int numberOfRowsInserted = ct.SaveChanges();
            Assert.AreEqual(3, numberOfRowsInserted, string.Format("Expected to add 3 EntityAttributeValue records, number inserted was {0} ", numberOfRowsInserted));
        }

        [Test]
        public void ShouldAllowInsertingOfSingleAnswerSelectionType()
        {
            EntityAttributeValue eta1 = new EntityAttributeValue()
            {
                EntityId = 1,
                AttributeId = 2,
                ValueId = 1,
                CreatedBy = 0
            };

            ct.EntityAttributeValues.Add(eta1);

            int numberOfRowsInserted = ct.SaveChanges();
            Assert.AreEqual(1, numberOfRowsInserted, string.Format("Expected to add 1 EntityAttributeValue records, number inserted was {0} ", numberOfRowsInserted));
        }

        [Test]
        public void ShouldAllowInsertingOfSingleChoiceSelectionType()
        {
            CentralTracking.Attribute att = new CentralTracking.Attribute()
            {
                AttributeDescription = "Favorite Color",
                AttributeLongDescription = " ",
                AttributeDataTypeId = 3,
                AttributeSelectionTypeId = 2,
                CreatedBy = 0
            };
            ct.Attributes.Add(att);

            ct.SaveChanges();

            EntityAttributeValue eta1 = new EntityAttributeValue()
            {
                EntityId = 1,
                AttributeId = att.AttributeId,
                ValueId = 1,
                CreatedBy = 0
            };

            ct.EntityAttributeValues.Add(eta1);

            int numberOfRowsInserted = ct.SaveChanges();
            Assert.AreEqual(1, numberOfRowsInserted, string.Format("Expected to add 1 EntityAttributeValue records, number inserted was {0} ", numberOfRowsInserted));
        }

        [Test]
        public void ShouldRemoveSingleAnswerFromMultipleSelectionType()
        {
            CentralTracking.Attribute att = new CentralTracking.Attribute()
            {
                AttributeDescription = "Favorite Color",
                AttributeLongDescription = " ",
                AttributeDataTypeId = 3,
                AttributeSelectionTypeId = 3,
                CreatedBy = 0
            };
            ct.Attributes.Add(att);

            ct.SaveChanges();

            EntityAttributeValue eta1 = new EntityAttributeValue()
            {
                EntityId = 1,
                AttributeId = att.AttributeId,
                ValueId = 1,
                CreatedBy = 0
            };

            EntityAttributeValue eta2 = new EntityAttributeValue()
            {
                EntityId = 1,
                AttributeId = att.AttributeId,
                ValueId = 2,
                CreatedBy = 0
            };

            EntityAttributeValue eta3 = new EntityAttributeValue()
            {
                EntityId = 1,
                AttributeId = att.AttributeId,
                ValueId = 3,
                CreatedBy = 0
            };

            ct.EntityAttributeValues.Add(eta1);
            ct.EntityAttributeValues.Add(eta2);
            ct.EntityAttributeValues.Add(eta3);
            ct.SaveChanges();

            ct.EntityAttributeValues.Remove(eta1);
            ct.SaveChanges();

            Assert.IsNotNull(ct.EntityAttributeValues.Find(eta2.EntityAttributeValueId), string.Format("Expected EntityAttributeValueId {0} to not be removed but it was removed.", eta2.EntityAttributeValueId));
            Assert.IsNotNull(ct.EntityAttributeValues.Find(eta3.EntityAttributeValueId), string.Format("Expected EntityAttributeValueId {0} to not be removed but it was removed.", eta3.EntityAttributeValueId));
            Assert.IsNull(ct.EntityAttributeValues.Find(eta1.EntityAttributeValueId), string.Format("Expected EntityAttributeValueId {0} to  be removed but it was not removed.", eta2.EntityAttributeValueId));
        }

        [Test]
        public void ShouldDeleteEntityAttributeValueRecord()
        {
            List<EntityAttributeValue> etas = ct.EntityAttributeValues.ToList();
            EntityAttributeValue remvoedEta = etas[0];

            ct.EntityAttributeValues.Remove(etas[0]);
            ct.SaveChanges();

            EntityAttributeValue foundVal = ct.EntityAttributeValues.Find(remvoedEta.EntityAttributeValueId);

            Assert.IsNull(foundVal, string.Format("Expected not to remove EntityAttributeId {0} but it was not removed.", remvoedEta.EntityAttributeValueId));
        }
    }
}

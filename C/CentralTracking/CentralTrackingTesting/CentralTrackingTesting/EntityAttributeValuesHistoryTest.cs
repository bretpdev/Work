using System;
using System.Linq;
using System.Transactions;
using NUnit.Framework;
using CentralTracking;

namespace CentralTrackingTesting
{
    class EntityAttributeValuesHistoryTest : BaseTest
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
        public void ShouldInsertHistoryRecordWhenEntityAttributeValueRecordIsInserted()
        {
            EntityAttributeValue eta = new EntityAttributeValue()
            {
                EntityId = 1,
                AttributeId = 2,
                ValueId = 2,
                CreatedBy = 0
            };

            ct.EntityAttributeValues.Add(eta);
            ct.SaveChanges();

            EntityAttributeValuesHistory etah = ct.EntityAttributeValuesHistories.Where(p =>
                (p.EntityAttributeValueId == eta.EntityAttributeValueId && p.EntityId == eta.EntityId && p.AttributeId == eta.AttributeId && p.ValueId == eta.ValueId && p.CreatedBy == eta.CreatedBy && p.HistoryStatusTypeId == 2)).SingleOrDefault();


            Assert.IsNotNull(etah, string.Format("Expected to Insert History Record but one was not inserted."));
        }

        [Test]
        public void ShouldInsertHistoryRecordWhenEntityAttributeValueRecordIsDeleted()
        {
            EntityAttributeValue eta = new EntityAttributeValue()
            {
                EntityId = 1,
                AttributeId = 2,
                ValueId = 2,
                CreatedBy = 0
            };

            ct.EntityAttributeValues.Add(eta);
            ct.SaveChanges();
            ct.EntityAttributeValues.Remove(eta);
            ct.SaveChanges();

            EntityAttributeValuesHistory etah = ct.EntityAttributeValuesHistories.Where(p =>
                (p.EntityAttributeValueId == eta.EntityAttributeValueId && p.EntityId == eta.EntityId && p.AttributeId == eta.AttributeId && p.ValueId == eta.ValueId && p.CreatedBy == eta.CreatedBy && p.HistoryStatusTypeId == 3)).SingleOrDefault();


            Assert.IsNotNull(etah, string.Format("Expected to Insert History Record but one was not inserted."));
        }

        [Test]
        public void ShouldInsertHistoryRecordWhenEntityAttributeValueRecordIsUpdated()
        {
            EntityAttributeValue eta = new EntityAttributeValue()
            {
                EntityId = 1,
                AttributeId = 2,
                ValueId = 2,
                CreatedBy = 0
            };

            ct.EntityAttributeValues.Add(eta);
            ct.SaveChanges();
            eta.ValueId = 1;
            ct.SaveChanges();

            EntityAttributeValuesHistory etah = ct.EntityAttributeValuesHistories.Where(p =>
                (p.EntityAttributeValueId == eta.EntityAttributeValueId && p.EntityId == eta.EntityId && p.AttributeId == eta.AttributeId && p.ValueId == eta.ValueId && p.CreatedBy == eta.CreatedBy && p.HistoryStatusTypeId == 2)).SingleOrDefault();


            Assert.IsNotNull(etah, string.Format("Expected to Insert History Record but one was not inserted."));
        }

        [Test]
        public void ShouldInsertHistoryStatusDateWhenInsertinEntityAttributeValue()
        {
            EntityAttributeValue eta = new EntityAttributeValue()
            {
                EntityId = 1,
                AttributeId = 2,
                ValueId = 2,
                CreatedBy = 0
            };

            ct.EntityAttributeValues.Add(eta);
            ct.SaveChanges();

            EntityAttributeValuesHistory etah = ct.EntityAttributeValuesHistories.Where(p =>
                (p.EntityAttributeValueId == eta.EntityAttributeValueId && p.EntityId == eta.EntityId && p.AttributeId == eta.AttributeId && p.ValueId == eta.ValueId && p.CreatedBy == eta.CreatedBy && p.HistoryStatusTypeId == 2)).SingleOrDefault();


            Assert.IsNotNull(etah.HistoryStatusDate, string.Format("Expected the HistoryStatusDate to be populated but it was null."));
        }

        [Test]
        public void ShouldInsertHistoryStatusCreatedByWhenInsertinEntityAttributeValue()
        {
            EntityAttributeValue eta = new EntityAttributeValue()
            {
                EntityId = 1,
                AttributeId = 2,
                ValueId = 2,
                CreatedBy = 0
            };

            ct.EntityAttributeValues.Add(eta);
            ct.SaveChanges();

            EntityAttributeValuesHistory etah = ct.EntityAttributeValuesHistories.Where(p =>
                (p.EntityAttributeValueId == eta.EntityAttributeValueId && p.EntityId == eta.EntityId && p.AttributeId == eta.AttributeId && p.ValueId == eta.ValueId && p.CreatedBy == eta.CreatedBy && p.HistoryStatusTypeId == 2)).SingleOrDefault();


            Assert.IsNotNull(etah.HistoryStatusCreatedBy, string.Format("Expected the HistoryStatusCreatedBy to be populated but it was null."));
        }

        [Test]
        public void ShouldInsertHistoryStatusTypeIdWhenInsertinEntityAttributeValue()
        {
            EntityAttributeValue eta = new EntityAttributeValue()
            {
                EntityId = 1,
                AttributeId = 2,
                ValueId = 2,
                CreatedBy = 0
            };

            ct.EntityAttributeValues.Add(eta);
            ct.SaveChanges();

            EntityAttributeValuesHistory etah = ct.EntityAttributeValuesHistories.Where(p =>
                (p.EntityAttributeValueId == eta.EntityAttributeValueId && p.EntityId == eta.EntityId && p.AttributeId == eta.AttributeId && p.ValueId == eta.ValueId && p.CreatedBy == eta.CreatedBy && p.HistoryStatusTypeId == 2)).SingleOrDefault();


            Assert.IsNotNull(etah.HistoryStatusTypeId, string.Format("Expected the HistoryStatusTypeId to be populated but it was null."));
        }
    }
}

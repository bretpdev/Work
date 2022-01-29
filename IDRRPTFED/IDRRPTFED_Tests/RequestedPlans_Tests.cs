using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IDRRPTFED;

namespace IDRRPTFED_Tests
{
    /// <summary>
    /// Summary description for RequestedPlans_Tests
    /// </summary>
    [TestClass]
    public class RequestedPlans_Tests
    {
        [TestMethod]
        public void RequestForIbrPlan_ProcessedOnIbr()
        {
            var request = new RequestedPlans()
            {
                ProcessedRepaymentPlan = "IB",
                RequestedRepaymentPlans = "IBR",
                LowestPlanRequested = false,
                PlanStatuses = new List<IdrPlan>(),
                IBR = null,
                ICR = null,
                PAYE = null,
                REPAYE = null
            };
            request.PopulatePlans();

            Assert.AreEqual(false, request.ICR.RequestedByBorrower);
            Assert.AreEqual(false, request.ICR.ProcessedByAgent);

            Assert.AreEqual(false, request.PAYE.RequestedByBorrower);
            Assert.AreEqual(false, request.PAYE.ProcessedByAgent);

            Assert.AreEqual(false, request.REPAYE.RequestedByBorrower);
            Assert.AreEqual(false, request.REPAYE.ProcessedByAgent);

            Assert.AreEqual(true, request.IBR.RequestedByBorrower);
            Assert.AreEqual(true, request.IBR.ProcessedByAgent);
        }

        [TestMethod]
        public void RequestForIcrPlan_ProcessedOnIcr()
        {
            var request = new RequestedPlans()
            {
                ProcessedRepaymentPlan = "IC",
                RequestedRepaymentPlans = "ICR",
                LowestPlanRequested = false,
                PlanStatuses = new List<IdrPlan>(),
                IBR = null,
                ICR = null,
                PAYE = null,
                REPAYE = null
            };
            request.PopulatePlans();

            Assert.AreEqual(true, request.ICR.RequestedByBorrower);
            Assert.AreEqual(true, request.ICR.ProcessedByAgent);

            Assert.AreEqual(false, request.PAYE.RequestedByBorrower);
            Assert.AreEqual(false, request.PAYE.ProcessedByAgent);

            Assert.AreEqual(false, request.REPAYE.RequestedByBorrower);
            Assert.AreEqual(false, request.REPAYE.ProcessedByAgent);

            Assert.AreEqual(false, request.IBR.RequestedByBorrower);
            Assert.AreEqual(false, request.IBR.ProcessedByAgent);
        }

        [TestMethod]
        public void RequestForPayePlan_ProcessedOnPaye()
        {
            var request = new RequestedPlans()
            {
                ProcessedRepaymentPlan = "PA",
                RequestedRepaymentPlans = "PAYE",
                LowestPlanRequested = false,
                PlanStatuses = new List<IdrPlan>(),
                IBR = null,
                ICR = null,
                PAYE = null,
                REPAYE = null
            };
            request.PopulatePlans();

            Assert.AreEqual(false, request.ICR.RequestedByBorrower);
            Assert.AreEqual(false, request.ICR.ProcessedByAgent);

            Assert.AreEqual(true, request.PAYE.RequestedByBorrower);
            Assert.AreEqual(true, request.PAYE.ProcessedByAgent);

            Assert.AreEqual(false, request.REPAYE.RequestedByBorrower);
            Assert.AreEqual(false, request.REPAYE.ProcessedByAgent);

            Assert.AreEqual(false, request.IBR.RequestedByBorrower);
            Assert.AreEqual(false, request.IBR.ProcessedByAgent);
        }

        [TestMethod]
        public void RequestForRepayePlan_ProcessedOnRepaye()
        {
            var request = new RequestedPlans()
            {
                ProcessedRepaymentPlan = "RA",
                RequestedRepaymentPlans = "REPAYE",
                LowestPlanRequested = false,
                PlanStatuses = new List<IdrPlan>(),
                IBR = null,
                ICR = null,
                PAYE = null,
                REPAYE = null
            };
            request.PopulatePlans();

            Assert.AreEqual(false, request.ICR.RequestedByBorrower);
            Assert.AreEqual(false, request.ICR.ProcessedByAgent);

            Assert.AreEqual(false, request.PAYE.RequestedByBorrower);
            Assert.AreEqual(false, request.PAYE.ProcessedByAgent);

            Assert.AreEqual(true, request.REPAYE.RequestedByBorrower);
            Assert.AreEqual(true, request.REPAYE.ProcessedByAgent);

            Assert.AreEqual(false, request.IBR.RequestedByBorrower);
            Assert.AreEqual(false, request.IBR.ProcessedByAgent);
        }

        [TestMethod]
        public void RequestForLp_ProcessedOnIbr()
        {
            var request = new RequestedPlans()
            {
                ProcessedRepaymentPlan = "IB",
                RequestedRepaymentPlans = "REPAYE, IBR, ICR, PAYE",
                LowestPlanRequested = true,
                PlanStatuses = new List<IdrPlan>(),
                IBR = null,
                ICR = null,
                PAYE = null,
                REPAYE = null
            };
            request.PopulatePlans();

            Assert.AreEqual(true, request.ICR.RequestedByBorrower);
            Assert.AreEqual(false, request.ICR.ProcessedByAgent);

            Assert.AreEqual(true, request.PAYE.RequestedByBorrower);
            Assert.AreEqual(false, request.PAYE.ProcessedByAgent);

            Assert.AreEqual(true, request.REPAYE.RequestedByBorrower);
            Assert.AreEqual(false, request.REPAYE.ProcessedByAgent);

            Assert.AreEqual(true, request.IBR.RequestedByBorrower);
            Assert.AreEqual(true, request.IBR.ProcessedByAgent);
        }

        [TestMethod]
        public void RequestForRepayeIbr_ProcessedOnRepaye()
        {
            var request = new RequestedPlans()
            {
                ProcessedRepaymentPlan = "RA",
                RequestedRepaymentPlans = "REPAYE, IBR",
                LowestPlanRequested = false,
                PlanStatuses = new List<IdrPlan>(),
                IBR = null,
                ICR = null,
                PAYE = null,
                REPAYE = null
            };
            request.PopulatePlans();

            Assert.AreEqual(false, request.ICR.RequestedByBorrower);
            Assert.AreEqual(false, request.ICR.ProcessedByAgent);

            Assert.AreEqual(false, request.PAYE.RequestedByBorrower);
            Assert.AreEqual(false, request.PAYE.ProcessedByAgent);

            Assert.AreEqual(true, request.REPAYE.RequestedByBorrower);
            Assert.AreEqual(true, request.REPAYE.ProcessedByAgent);

            Assert.AreEqual(true, request.IBR.RequestedByBorrower);
            Assert.AreEqual(false, request.IBR.ProcessedByAgent);
        }

        [TestMethod]
        public void RequestForIbrRepaye_ProcessedOnRepaye()
        {
            var request = new RequestedPlans()
            {
                ProcessedRepaymentPlan = "RA",
                RequestedRepaymentPlans = "IBR, REPAYE",
                LowestPlanRequested = false,
                PlanStatuses = new List<IdrPlan>(),
                IBR = null,
                ICR = null,
                PAYE = null,
                REPAYE = null
            };
            request.PopulatePlans();

            Assert.AreEqual(false, request.ICR.RequestedByBorrower);
            Assert.AreEqual(false, request.ICR.ProcessedByAgent);

            Assert.AreEqual(false, request.PAYE.RequestedByBorrower);
            Assert.AreEqual(false, request.PAYE.ProcessedByAgent);

            Assert.AreEqual(true, request.REPAYE.RequestedByBorrower);
            Assert.AreEqual(true, request.REPAYE.ProcessedByAgent);

            Assert.AreEqual(true, request.IBR.RequestedByBorrower);
            Assert.AreEqual(false, request.IBR.ProcessedByAgent);
        }

        [TestMethod]
        public void RequestForPayeRepaye_ProcessedOnRepaye()
        {
            var request = new RequestedPlans()
            {
                ProcessedRepaymentPlan = "RA",
                RequestedRepaymentPlans = "REPAYE, PAYE",
                LowestPlanRequested = false,
                PlanStatuses = new List<IdrPlan>(),
                IBR = null,
                ICR = null,
                PAYE = null,
                REPAYE = null
            };
            request.PopulatePlans();

            Assert.AreEqual(false, request.ICR.RequestedByBorrower);
            Assert.AreEqual(false, request.ICR.ProcessedByAgent);

            Assert.AreEqual(true, request.PAYE.RequestedByBorrower);
            Assert.AreEqual(false, request.PAYE.ProcessedByAgent);

            Assert.AreEqual(true, request.REPAYE.RequestedByBorrower);
            Assert.AreEqual(true, request.REPAYE.ProcessedByAgent);

            Assert.AreEqual(false, request.IBR.RequestedByBorrower);
            Assert.AreEqual(false, request.IBR.ProcessedByAgent);
        }

        [TestMethod]
        public void RequestForPayeIcr_ProcessedOnIcr()
        {
            var request = new RequestedPlans()
            {
                ProcessedRepaymentPlan = "IC",
                RequestedRepaymentPlans = "ICR, PAYE",
                LowestPlanRequested = false,
                PlanStatuses = new List<IdrPlan>(),
                IBR = null,
                ICR = null,
                PAYE = null,
                REPAYE = null
            };
            request.PopulatePlans();

            Assert.AreEqual(true, request.ICR.RequestedByBorrower);
            Assert.AreEqual(true, request.ICR.ProcessedByAgent);

            Assert.AreEqual(true, request.PAYE.RequestedByBorrower);
            Assert.AreEqual(false, request.PAYE.ProcessedByAgent);

            Assert.AreEqual(false, request.REPAYE.RequestedByBorrower);
            Assert.AreEqual(false, request.REPAYE.ProcessedByAgent);

            Assert.AreEqual(false, request.IBR.RequestedByBorrower);
            Assert.AreEqual(false, request.IBR.ProcessedByAgent);
        }
    }
}

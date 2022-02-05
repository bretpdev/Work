using System.Collections.Generic;

namespace IDRRPTFED
{
    public class RequestedPlans
    {
        public string ProcessedRepaymentPlan { get; set; }
        public string RequestedRepaymentPlans { get; set; }
        public bool LowestPlanRequested { get; set; }
        public List<IdrPlan> PlanStatuses { get; set; } = new List<IdrPlan>();
        public IbrPlan IBR { get; set; }
        public IcrPlan ICR { get; set; }
        public PayePlan PAYE { get; set; }
        public RepayePlan REPAYE { get; set; }

        /// <summary>
        /// Constructs each IDR plan object and adds them to PlanStatuses.
        /// </summary>
        public void PopulatePlans()
        {
            IBR = new IbrPlan(ProcessedRepaymentPlan, RequestedRepaymentPlans);
            PlanStatuses.Add(IBR);

            ICR = new IcrPlan(ProcessedRepaymentPlan, RequestedRepaymentPlans);
            PlanStatuses.Add(ICR);

            PAYE = new PayePlan(ProcessedRepaymentPlan, RequestedRepaymentPlans);
            PlanStatuses.Add(PAYE);

            REPAYE = new RepayePlan(ProcessedRepaymentPlan, RequestedRepaymentPlans);
            PlanStatuses.Add(REPAYE);
        }
    }

    /// <summary>
    /// Establish object blueprint for various IDR plans, which are then stored in PlanStatuses in the RequestedPlans class.
    /// </summary>
    public abstract class IdrPlan
    {
        public abstract bool ProcessedByAgent { get; set; }
        public abstract bool RequestedByBorrower { get; set; }
        public abstract string IdrPlanReportingCode { get; set; } //This field houses the code used in the reporting file
        public abstract string IdrPlanAcronym { get; set; } //This field houses the plan's acronym that is commonly used on the DB

    }

    /// <summary>
    /// Class creates an IbrPlan object that is used by the RequestedPlans class.
    /// Object holds values for whether it was the plan that was processed, whether or not it was a requested plan, as well as its database acronym and reporting code.
    /// </summary>
    public class IbrPlan : IdrPlan
    {
        public override bool ProcessedByAgent { get; set; }
        public override bool RequestedByBorrower { get; set; }
        public override string IdrPlanReportingCode { get; set; } = "IB";
        public override string IdrPlanAcronym { get; set; } = "IBR"; //"Income-Based Repayment Plan"

        public IbrPlan(string processedPlan, string requestedPlans)
        {
            ProcessedByAgent = processedPlan == IdrPlanReportingCode;
            RequestedByBorrower = requestedPlans.Contains(IdrPlanAcronym);
        }
    }

    /// <summary>
    /// Class creates an IcrPlan object that is used by the RequestedPlans class.
    /// Object holds values for whether it was the plan that was processed, whether or not it was a requested plan, as well as its database acronym and reporting code.
    /// </summary>
    public class IcrPlan : IdrPlan
    {
        public override bool ProcessedByAgent { get; set; }
        public override bool RequestedByBorrower { get; set; }
        public override string IdrPlanReportingCode { get; set; } = "IC";
        public override string IdrPlanAcronym { get; set; } = "ICR"; //"Income-Contingent Repayment Plan"

        public IcrPlan(string processedPlan, string requestedPlans)
        {
            ProcessedByAgent = processedPlan == IdrPlanReportingCode;
            RequestedByBorrower = requestedPlans.Contains(IdrPlanAcronym);
        }
    }

    /// <summary>
    /// Class creates a PayePlan object that is used by the RequestedPlans class.
    /// Object holds values for whether it was the plan that was processed, whether or not it was a requested plan, as well as its database acronym and reporting code.
    /// </summary>
    public class PayePlan : IdrPlan
    {
        public override bool ProcessedByAgent { get; set; }
        public override bool RequestedByBorrower { get; set; }
        public override string IdrPlanReportingCode { get; set; } = "PA";
        public override string IdrPlanAcronym { get; set; } = "PAYE"; //"Pay As You Earn Plan"

        public PayePlan(string processedPlan, string requestedPlans)
        {
            ProcessedByAgent = processedPlan == IdrPlanReportingCode;
            requestedPlans = requestedPlans.Contains("REPAYE") ? requestedPlans.Replace("REPAYE", "") : requestedPlans; //If string contains REPAYE, remove it
            RequestedByBorrower = requestedPlans.Contains(IdrPlanAcronym);
        }
    }

    /// <summary>
    /// Class creates a RepayePlan object that is used by the RequestedPlans class.  
    /// Object holds values for whether it was the plan that was processed, whether or not it was a requested plan, as well as its database acronym and reporting code.
    /// </summary>
    public class RepayePlan : IdrPlan
    {
        public override bool ProcessedByAgent { get; set; }
        public override bool RequestedByBorrower { get; set; }
        public override string IdrPlanReportingCode { get; set; } = "RA";
        public override string IdrPlanAcronym { get; set; } = "REPAYE"; //"Revised Pay As You Earn Plan"

        public RepayePlan(string processedPlan, string requestedPlans)
        {
            ProcessedByAgent = processedPlan == IdrPlanReportingCode;
            RequestedByBorrower = requestedPlans.Contains(IdrPlanAcronym);
        }
    }
}

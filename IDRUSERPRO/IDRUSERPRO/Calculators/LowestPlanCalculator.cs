using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using System.Reflection;
using Uheaa.Common.DataAccess;

namespace IDRUSERPRO
{
    /// <summary>
    /// Given a filled LpcInput object, calculates the monthly installments for each type of plan available.
    /// </summary>
    public class LowestPlanCalculator
    {
        ReflectionInterface RI { get; set; }
        WarehouseDataAccess WDA { get; set; }
        LpcInput CI { get; set; }
        public LowestPlanCalculator(ReflectionInterface ri, WarehouseDataAccess wda, LpcInput ci)
        {
            RI = ri;
            CI = ci;
            WDA = wda;
        }
        public LpcResults Calculate(IndicatorsResult previouslyCalculatedIndicators)
        {
            var results = new LpcResults();
            results.EligibilityIndicators = previouslyCalculatedIndicators;
            results.Input = CI;
            var processedPlan = CI.RepaymentPlan;
            foreach (var repaymentPlan in CalculationArguments.Keys)
            {
                CI.RepaymentPlan = repaymentPlan;
                var planResults = new LpcResults.PlanResult();
                results.SetPlanResults(repaymentPlan, planResults);
                var badUheaa = results.EligibilityIndicators.AllLoansWithNonZeroBalanceMatchLoanProgram("PLUS", "TILP", "COMPLT");
                var isUheaa = DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa;
                if (badUheaa && isUheaa)
                {
                    planResults.Status = LpcResults.ResultStatus.Unsuccessful;
                    planResults.ErrorMessage = "No Loans Eligible";
                    continue;
                }

                var args = CalculationArguments[repaymentPlan];
                if (!LoansAreEligible(previouslyCalculatedIndicators.Indicators, args.EligibleIndicators))
                {
                    planResults.Status = LpcResults.ResultStatus.Unsuccessful;
                    planResults.ErrorMessage = "NO LOANS ELIGIBLE";
                    continue;
                }

                var finalInstallment = CalculateInstallment(args);
                if (args.UseAmortizationIfLower)
                {
                    var amortization = CalculateAmortization(results);
                    if (amortization.HasValue && args.UseAmortizationIfLower)
                        finalInstallment = Math.Min(finalInstallment, amortization.Value);
                }

                if (finalInstallment < 0)
                    finalInstallment = 0;
                planResults.MonthlyInstallment = finalInstallment;
                planResults.Status = LpcResults.ResultStatus.Successful;
            }
            CI.RepaymentPlan = processedPlan;

            return results;
        }

        private decimal CalculateInstallment(InternalCalculationArguments arguments)
        {
            var finalDeductionAmount = CI.FinalDeductionAmount * arguments.FinalDeductionModifier;
            var installment = (CI.AGI - finalDeductionAmount) * arguments.TotalPercentageModifier / 12;
            decimal debtServicerPercentage = 1;
            if (CI.ExternalLoans.Any())
            {
                var eligibleExternalLoans = CI.ExternalLoans.ToList();
                var totalExternalLoanBalance = eligibleExternalLoans.Sum(o => o.OutstandingBalance);
                var totalWarehouseDebt = WDA.GetTotalDebt(CI.AccountNumber);
                var totalLoanDebt = totalExternalLoanBalance + totalWarehouseDebt;
                debtServicerPercentage = totalWarehouseDebt / totalLoanDebt;
            }
            decimal finalInstallment = installment * debtServicerPercentage;
            if (arguments.Adjust510)
            {
                if (finalInstallment > 0 && finalInstallment < 5)
                    finalInstallment = 0;
                else if (finalInstallment >= 5 && finalInstallment < 10)
                    finalInstallment = 10;
            }
            else
            {
                if (finalInstallment > 0 && finalInstallment < 5)
                    finalInstallment = 5;
            }
            return finalInstallment;
        }

        private bool LoansAreEligible(List<LoanSequenceEligibility> indicators, params string[] eligibleIndicators)
        {
            if (!indicators.Any(o => o.FutureEligibilityIndicator.IsIn(eligibleIndicators)))
                return false;
            return true;
        }

        private decimal? CalculateAmortization(LpcResults results)
        {
            var factor = GetIpf(results);
            if (factor == null)
                return null;
            var originalLoans = WDA.GetOriginalLoans(CI.AccountNumber);
            var eligibleLoanSequences = results.EligibilityIndicators.Indicators.Where(o => o.FutureEligibilityIndicator != "I")
                .Select(o => o.LoanSequence).ToArray();
            originalLoans = originalLoans.Where(o => o.LoanSequence.IsIn(eligibleLoanSequences)).ToList();
            decimal installment = 0;
            foreach (var ol in originalLoans)
            {
                var monthlyInterestRate = ol.InterestRate / 12m;
                var totalInterest = 1 - (decimal)Math.Pow((double)monthlyInterestRate + 1, -144);
                var quantizedInterest = monthlyInterestRate / totalInterest;
                var initialRepayment = ol.OriginalBalance * quantizedInterest;
                installment += initialRepayment;
            }
            installment = installment * factor.Value;
            if (installment > 0 && installment < 5)
                installment = 5;
            return installment;
        }

        private decimal? GetIpf(LpcResults results)
        {
            var factors = CI.IncomePercentageFactors.OrderBy(o => o.Income);
            var familySizeMatches = factors.Where(o => (o.Married && CI.FamilySize > 1) || (!o.Married && CI.FamilySize == 1));
            var possibleExactMatch = familySizeMatches.SingleOrDefault(o => o.Income == CI.AGI);
            if (possibleExactMatch != null)
                return possibleExactMatch.Factor;
            if (!familySizeMatches.Any())
                return null;
            var below = familySizeMatches.First();
            var above = familySizeMatches.Last();
            if (CI.AGI < below.Income || CI.AGI > above.Income)
                return null;
            below = familySizeMatches.Last(o => o.Income <= CI.AGI);
            above = familySizeMatches.First(o => o.Income >= CI.AGI);
            var incomeDifference = above.Income - below.Income;
            var factorDifference = above.Factor - below.Factor;
            var lowerIncomeDifference = CI.AGI - below.Income;
            var ratioOfIncomeDifference = lowerIncomeDifference / incomeDifference;
            var addToLowerFactor = ratioOfIncomeDifference * factorDifference;
            var factorToUse = below.Factor + addToLowerFactor;
            return factorToUse;
        }

        private class InternalCalculationArguments
        {
            public decimal FinalDeductionModifier { get; set; }
            public decimal TotalPercentageModifier { get; set; }
            public bool Adjust510 { get; set; }
            public string[] EligibleIndicators { get; set; }
            public bool UseAmortizationIfLower { get; set; }
            public InternalCalculationArguments(decimal finaldeductionModifier, decimal totalPercentageModifier, bool adjust510, bool useAmortizationIfLower, params string[] eligibleIndicators)
            {
                FinalDeductionModifier = finaldeductionModifier;
                TotalPercentageModifier = totalPercentageModifier;
                Adjust510 = adjust510;
                UseAmortizationIfLower = useAmortizationIfLower;
                EligibleIndicators = eligibleIndicators;
            }
        }
        #region Static Reflection Calculation
        static Dictionary<RepaymentPlans, InternalCalculationArguments> CalculationArguments = new Dictionary<RepaymentPlans, InternalCalculationArguments>();
        static LowestPlanCalculator()
        {
            //CalculationArguments[RepaymentPlans.ICR] = new InternalCalculationArguments(1, 0.2m, false, true, "C", "P", "R", "4");
            CalculationArguments[RepaymentPlans.IBR] = new InternalCalculationArguments(1.5m, 0.15m, true, false, "B", "P", "R");
            CalculationArguments[RepaymentPlans.NEWIBR] = new InternalCalculationArguments(1.5m, 0.10m, true, false, "4");
            //CalculationArguments[RepaymentPlans.PAYE] = new InternalCalculationArguments(1.5m, 0.10m, true, false, "P", "4");
            //CalculationArguments[RepaymentPlans.REPAYE] = new InternalCalculationArguments(1.5m, 0.10m, true, false, "P", "R", "4");

            #endregion
        }
    }
}
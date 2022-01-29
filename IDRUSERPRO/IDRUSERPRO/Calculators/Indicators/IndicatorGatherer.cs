using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using Uheaa.Common;

namespace IDRUSERPRO
{
    public class IndicatorGatherer
    {
        public IndicatorsResult LoadEligibilityIndicators(ReflectionInterface ri, WarehouseDataAccess wda, string accountNumber)
        {
            var indicators = new List<LoanSequenceEligibility>();
            var result = new IndicatorsResult()
            {
                Indicators = indicators
            };
            ri.FastPath("TX3Z/ITS7C" + accountNumber);
            if (ri.MessageCode == "50108")
                return result;
            var settings = PageHelper.IterationSettings.Default();
            settings.MinRow = 7;
            settings.MaxRow = 20;
            Action<int> addIndicator = new Action<int>(seq =>
            {
                var ind = new LoanSequenceEligibility();
                ind.LoanSequence = seq;
                ind.LoanProgram = ri.GetText(6, 38, 6);
                ind.CurrentBalance = wda.GetCurrentBalance(accountNumber, ind.LoanSequence);
                ind.EligibilityIndicator = ri.GetText(14, 74, 1);
                if (ind.EligibilityIndicator.IsIn("I", "_"))
                    ind.EligibilityIndicator = null;
                if (ind.LoanProgram.IsIn("DLPLUS", "DLPCNS"))
                    ind.EligibilityIndicator = "I";

                if (ind.CurrentBalance > 0)
                    indicators.Add(ind);
            });
            if (ri.ScreenCode == "TSX7D")
            {
                var onlyLoanSequence = GetOnlyLoanSequenceWithBalance(ri, accountNumber);
                ri.FastPath("TX3Z/ITS7C" + accountNumber);
                addIndicator(onlyLoanSequence); //only one loan sequence
            }
            else
            {
                PageHelper.Iterate(ri, row =>
                {
                    var sel = ri.GetText(row, 3, 2).ToIntNullable();
                    if (sel.HasValue)
                    {
                        int seq = ri.GetText(row, 20, 4).ToInt();
                        ri.PutText(22, 19, sel.Value.ToString().PadLeft(2, '0'), ReflectionInterface.Key.Enter);
                        addIndicator(seq);
                        ri.Hit(ReflectionInterface.Key.F12);
                    }
                }, settings);
            }
            return result;
        }

        private int GetOnlyLoanSequenceWithBalance(ReflectionInterface ri, string accountNumber)
        {
            ri.FastPath("TX3Z/ITS26" + accountNumber);
            int foundSequence = -1;
            if (ri.ScreenCode == "TSX29")
                foundSequence = ri.GetText(7, 35, 4).ToInt();
            else
                PageHelper.Iterate(ri, (row, settings) =>
                {
                    var balance = ri.GetText(row, 59, 10).ToDecimalNullable();
                    if (ri.CheckForText(row, 69, "CR"))
                        balance = 0;
                    if (balance > 0)
                    {
                        foundSequence = ri.GetText(row, 14, 4).ToInt();
                        settings.ContinueIterating = false;
                    }
                });
            return foundSequence;
        }

    }
}

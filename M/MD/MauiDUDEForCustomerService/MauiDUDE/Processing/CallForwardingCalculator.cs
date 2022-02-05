using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace MauiDUDE
{
    class CallForwardingCalculator
    {
        private static Dictionary<string, CallForwardingResult> _resultOptions { get; set; }

        public CallForwardingCalculator()
        {
            
        }

        private static void PopulateResultOptions()
        {
            if (_resultOptions == null || _resultOptions.Keys.Count == 0)
            {
                _resultOptions = new Dictionary<string, CallForwardingResult>();
                _resultOptions.Add("01", new CallForwardingResult("Default Collections", "7272", Alert.AlertUrgency.High));
                _resultOptions.Add("02", new CallForwardingResult("Sallie Mae", "Refer the borrower to the servicer's website.", Alert.AlertUrgency.Low));
                _resultOptions.Add("03", new CallForwardingResult("Nelnet", "Refer the borrower to the servicer's website.", Alert.AlertUrgency.Medium));
                _resultOptions.Add("06", new CallForwardingResult("DA Collections (TILP)", "7246", Alert.AlertUrgency.High));
                _resultOptions.Add("07", new CallForwardingResult("DA (SLMA – Closed)", "7246", Alert.AlertUrgency.Low));
                _resultOptions.Add("08", new CallForwardingResult("DA (NLNT – Closed)", "7246", Alert.AlertUrgency.Low));
                _resultOptions.Add("04", new CallForwardingResult("UHEAA Customer Service", "1094", Alert.AlertUrgency.High));
                _resultOptions.Add("05", new CallForwardingResult("Fed Loan Servicing", "800-699-2908", Alert.AlertUrgency.High));
                //_resultOptions.Add("09", new CallForwardingResult("CornerStone Customer Service", "800-663-1662", Alert.AlertUrgency.High));
                //_resultOptions.Add("10", new CallForwardingResult("CornerStone Closed", "800-663-1662", Alert.AlertUrgency.High));
                _resultOptions.Add("11", new CallForwardingResult("DMCS", "800-621-3115", Alert.AlertUrgency.Low));
                _resultOptions.Add("12", new CallForwardingResult("TPD", "888-303-7818", Alert.AlertUrgency.High));
                //* The alert urgency was added so alerts could be displayed in different colors in the field on the homepage but then they changed their minds
                //and decided they didn't want the colors but it would have taken too long to remove all of the infrastructure that had alredy been created
            }
        }

        public static void CalculateCallForwardingResults(Borrower borrower, DataAccessHelper.Region region)
        {
            PopulateResultOptions();
            List<CallForwardingResult> results = new List<CallForwardingResult>();
            List<string> tempWarehouseCallForwardingData = new List<string>();

            //calculate results
            CalculateIndividualCallForwardingResult(borrower, results, region);
            //all 01
            tempWarehouseCallForwardingData = borrower.CallForwardingWarehouseData.Where(p => p.FORWARDING == "01").Select(p => p.FORWARDING).ToList();
            if(tempWarehouseCallForwardingData.Count == borrower.CallForwardingWarehouseData.Count)
            {
                borrower.ContinueToUseHomePage = false;
                borrower.CallForwardingUserResultData = results;
                return;
            }
            tempWarehouseCallForwardingData = borrower.CallForwardingWarehouseData.Where(p => 
                p.FORWARDING == "02" ||
                p.FORWARDING == "03" ||
                p.FORWARDING == "07" ||
                p.FORWARDING == "08" || 
                p.FORWARDING == "11" || 
                p.FORWARDING == "12")
                .Select(p => p.FORWARDING).ToList();

            if(tempWarehouseCallForwardingData.Count == borrower.CallForwardingWarehouseData.Count)
            {
                borrower.ContinueToUseHomePage = false;
                borrower.CallForwardingUserResultData = results;
                return;
            }

            tempWarehouseCallForwardingData = borrower.CallForwardingWarehouseData.Where(p =>
                p.FORWARDING == "04" ||
                p.FORWARDING == "09" ||
                p.FORWARDING == "10")
                .Select(p => p.FORWARDING).ToList();
            if (tempWarehouseCallForwardingData.Count == borrower.CallForwardingWarehouseData.Count)
            {
                //all 04 (UHEAA) or 09 (Cornerstone)
                //if all are "04" or "09" then don't show any alert on Home Page
                results.Clear();
                borrower.ContinueToUseHomePage = true;
                borrower.CallForwardingUserResultData = results;
                return;
            }
            else if(tempWarehouseCallForwardingData.Count > 0)
            {
                //one or more 04
                borrower.ContinueToUseHomePage = true;
                //use list created but strip out customer service result ("04") because it doesn't need to appear on the home page
                borrower.CallForwardingUserResultData = results.Where(p =>
                    p.Location != _resultOptions["04"].Location &&
                    p.Location != _resultOptions["09"].Location &&
                    p.Location != _resultOptions["10"].Location)
                    .ToList();
                return;
            }

            //05
            tempWarehouseCallForwardingData = borrower.CallForwardingWarehouseData.Where(p => p.FORWARDING == "05").Select(p => p.FORWARDING).ToList();
            if (tempWarehouseCallForwardingData.Count > 0)
            {
                //one or more 05
                borrower.ContinueToUseHomePage = true;
                borrower.CallForwardingUserResultData = results;
                return;
            }

            //all 06
            tempWarehouseCallForwardingData = borrower.CallForwardingWarehouseData.Where(p => p.FORWARDING == "06").Select(p => p.FORWARDING).ToList();
            if(tempWarehouseCallForwardingData.Count == borrower.CallForwardingWarehouseData.Count)
            {
                borrower.ContinueToUseHomePage = false;
                borrower.CallForwardingUserResultData = results;
                return;
            }

            //the else statement to all the logic above is to be sure none of the results are 04 or 05(which has already been checked up top)
            borrower.ContinueToUseHomePage = false;
            borrower.CallForwardingUserResultData = results;
        }

        private static void CalculateIndividualCallForwardingResult(Borrower borrower, List<CallForwardingResult> results, DataAccessHelper.Region region)
        {
            //PopulateResultOptions(); not needed
            List<DataAccess.CallForwardingData> tempWarehouseCallForwardingData = new List<DataAccess.CallForwardingData>();
            foreach(var resultOption in _resultOptions)
            {
                string key = resultOption.Key;
                if(key == "11")
                {
                    continue;
                }
                tempWarehouseCallForwardingData = borrower.CallForwardingWarehouseData.Where(p => p.FORWARDING == key).ToList();
                if (tempWarehouseCallForwardingData.Count > 0)
                {
                    CallForwardingResult copy = resultOption.Value.Copy();
                    if(tempWarehouseCallForwardingData.First().IF_GTR != "000749")
                    {
                        if(tempWarehouseCallForwardingData.First().FORWARDING == "06")
                        {
                            copy.OverrideMessage = "Call Forward: Default Collections – 7272";
                        }
                        else
                        {
                            copy.OverrideMessage = "Borrower is Defaulted. Please verify where loans are being serviced on NSLDS.";
                        }
                    }
                    else if(tempWarehouseCallForwardingData.Any(p => p.IF_GTR != "000749") && tempWarehouseCallForwardingData.All(p => p.FORWARDING == "01"))
                    {
                        var defaultCopy = copy.Copy();
                        defaultCopy.OverrideMessage = "Borrower is Defaulted. Please verify where loans are being serviced on NSLDS.";
                        results.Add(defaultCopy);
                    }
                    results.Add(copy);
                }
            }
        }

    }
}

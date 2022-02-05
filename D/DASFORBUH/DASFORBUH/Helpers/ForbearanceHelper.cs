using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using static Uheaa.Common.Scripts.ReflectionInterface;

namespace DASFORBUH
{
    public class ForbearanceHelper
    {
        private IReflectionInterface RI;
        private string DelinquencyOverrideForbearance = "39";

        public ForbearanceHelper(IReflectionInterface ri)
        {
            this.RI = ri;
        }
        public ForbearanceAddResults AddDelinquencyOverrideForbearance(ProcessQueueData borrowerInfo)
        {
            if (borrowerInfo.LD_DLQ_OCC < borrowerInfo.BeginDate)
            {
                Console.WriteLine($"Adding delinquency override forb for account: {borrowerInfo.AccountNumber}. Begin date: {borrowerInfo.LD_DLQ_OCC}. End date: {borrowerInfo.BeginDate.AddDays(-1)}.");
                var results = new ForbearanceAddResults();
                RI.FastPath("TX3Z/ATS0H");
                RI.PutText(7, 33, borrowerInfo.AccountNumber, true);
                RI.PutText(9, 33, "F");
                RI.PutText(11, 33, DateTime.Now.ToString("MMddyy"), ReflectionInterface.Key.Enter, true);

                if (RI.MessageCode == "01527")
                {
                    results.ErrorMessage = "Can't find borrower on system " + borrowerInfo.AccountNumber;
                    return results;
                }

                if (RI.ScreenCode == "TSX7E")
                    RI.PutText(21, 13, DelinquencyOverrideForbearance, Key.Enter);
                else
                {
                    results.ErrorMessage = $"Error navigating to the TSX7E screen for Account: {borrowerInfo.AccountNumber}.  Session Message: {RI.Message}";
                    return results;
                }

                if (RI.MessageCode == "50108")
                {
                    results.ErrorMessage = $"Unable to access the forbearance menu selection screen  Account: {borrowerInfo.AccountNumber}.  Session Message: {RI.Message}";
                    return results;
                }

                if (RI.ScreenCode == "TSXA5")
                    RI.PutText(8, 14, "X", Key.Enter);

                RI.PutText(7, 18, borrowerInfo.LD_DLQ_OCC.ToString("MMddyy"));
                RI.PutText(8, 18, borrowerInfo.BeginDate.AddDays(-1).ToString("MMddyy"));
                RI.PutText(9, 18, DateTime.Now.ToString("MMddyy"));

                RI.PutText(16, 37, "N");  //clear delinquency field

                RI.PutText(20, 17, "", true);
                if (RI.CheckForText(14, 73, "_"))
                    RI.PutText(14, 73, "Y");
                if (RI.CheckForText(17, 34, "_"))
                    RI.PutText(17, 34, "N");

                if (DelinquencyOverrideForbearance == "39")
                    RI.PutText(9, 37, "CV");

                RI.Hit(Key.Enter);

                if (RI.ScreenCode == "TSX31" || RI.ScreenCode == "TSX30") //Supposedly sometimes the session requires going back and hitting enter again
                {
                    RI.Hit(Key.F12);
                    RI.Hit(Key.Enter);
                }

                if (RI.ScreenCode == "TSX30") //If repeitition of input results on TSX30 screen, this is due to bad dates, usually an overlap
                {
                    results.ErrorMessage = $"ProcessQueueId: {borrowerInfo.ProcessQueueId}. Account: {borrowerInfo.AccountNumber}. Error: Unable to add the delinquency override forbearance. Check for overlapping d/f date range.";
                    return results;
                }
                if (RI.MessageCode != "01004")
                {
                    results.ErrorMessage = $"ProcessQueueId: {borrowerInfo.ProcessQueueId}. Account: {borrowerInfo.AccountNumber}. Error: Unable to add the delinquency override forbearance. Session Message Code: {RI.MessageCode}. Session Message: {RI.Message}. Session screen: {RI.ScreenCode}.";
                    return results;
                }
                results.ForbearanceAdded = true;
                return results;
            }
            return null;
        }

        public ForbearanceAddResults AddForbearance(ProcessQueueData borrowerInfo)
        {
            Console.WriteLine($"Adding forb for account: {borrowerInfo.AccountNumber}. Begin date: {borrowerInfo.BeginDate}. End date: {borrowerInfo.EndDate}.");
            var results = new ForbearanceAddResults();
            RI.FastPath("TX3Z/ATS0H");
            RI.PutText(7, 33, borrowerInfo.AccountNumber, true);
            RI.PutText(9, 33, "F");
            RI.PutText(11, 33, DateTime.Now.ToString("MMddyy"), ReflectionInterface.Key.Enter, true);

            if (RI.MessageCode == "01527")
            {
                results.ErrorMessage = "Can't find borrower on system " + borrowerInfo.AccountNumber;
                return results;
            }

            if (RI.ScreenCode == "TSX7E")
                RI.PutText(21, 13, Program.ForbType, Key.Enter);
            else
            {
                results.ErrorMessage = $"Error navigating to the TSX7E screen for Account: {borrowerInfo.AccountNumber}.  Session Message: {RI.Message}";
                return results;
            }

            if (RI.MessageCode == "50108")
            {
                results.ErrorMessage = $"Unable to access the forbearance menu selection screen {RI.Message} Account: {borrowerInfo.AccountNumber}";
                return results;
            }

            if (RI.ScreenCode == "TSXA5")
                RI.PutText(8, 14, "X", Key.Enter);

            RI.PutText(7, 18, borrowerInfo.BeginDate.ToString("MMddyy"));
            RI.PutText(8, 18, borrowerInfo.EndDate.ToString("MMddyy"));
            RI.PutText(9, 18, DateTime.Now.ToString("MMddyy"));

            //Only want to clear the delinquency field if there
            if (!borrowerInfo.DelinquencyOverride)
            {
                RI.PutText(16, 37, "Y");  //clear delinquency field
            }
            else
            {
                RI.PutText(16, 37, "N");  //clear delinquency field
            }

            RI.PutText(20, 17, "", true);
            if (RI.CheckForText(14, 73, "_"))
                RI.PutText(14, 73, "Y");
            if (RI.CheckForText(17, 34, "_"))
                RI.PutText(17, 34, "N");

            if (Program.ForbType == "39")
                RI.PutText(9, 37, "CV");
            RI.Hit(Key.Enter);

            if (RI.ScreenCode == "TSX31" || RI.ScreenCode == "TSX30") //Supposedly sometimes the session requires going back and hitting enter again
            {
                RI.Hit(Key.F12);
                RI.Hit(Key.Enter);
            }

            if (RI.ScreenCode == "TSX30") //If repeitition of input results on TSX30 screen, this is due to bad dates, usually an overlap
            {
                results.ErrorMessage = $"ProcessQueueId: {borrowerInfo.ProcessQueueId}. Account: {borrowerInfo.AccountNumber}. Error: Unable to add the forbearance. Check for overlapping d/f date range.";
                return results;
            }
            if (RI.MessageCode != "01004")
            {
                results.ErrorMessage = $"ProcessQueueId: {borrowerInfo.ProcessQueueId}. Account: {borrowerInfo.AccountNumber}. Error: Unable to add the forbearance. Session Message Code: {RI.MessageCode}. Session Message: {RI.Message}. Session screen: {RI.ScreenCode}.";
                return results;
            }
            results.ForbearanceAdded = true;
            return results;
        }

        public class ForbearanceAddResults
        {
            public bool ForbearanceAdded { get; internal set; }
            public string ErrorMessage { get; internal set; }
        }
    }
}

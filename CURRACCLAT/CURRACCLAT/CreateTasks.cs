using System;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace CURRACCLAT
{
    public class CreateTasks
    {
        public string ScriptId { get; set; }
        public ReflectionInterface RI { get; set; }
        public string UserId { get; set; }
        public DataAccess DA { get; set; }
        public bool PauseBetweenRecords { get; set; }

        public CreateTasks(string scriptId, ReflectionInterface ri)
        {
            ScriptId = scriptId;
            RI = ri;
            _ = RI.UserId; //calling this here guarantees it has been set before trying to use it.
            DA = new DataAccess(ri.LogRun.LDA);
        }

        /// <summary>
        /// Pulls data from warehouse and stores in local table. Loads BorrowerData object wil unprocessed accounts
        /// </summary>
        public int Process(bool pauseBetweenRecords)
        {
            PauseBetweenRecords = pauseBetweenRecords;
            Console.WriteLine("Loading borrower data");
            DA.PullDataFromWarehouse(); //Gets the data from the warehouse and loads in local table
            List<BorrowerData> data = DA.GetData(); //Gets the unprocessed data from the local table
            Console.WriteLine($"{data.Count} borrower accounts to process");

            return AddQueue(data);
        }

        /// <summary>
        /// Add WRK GP01 queue for each borrower
        /// </summary>
        private int AddQueue(List<BorrowerData> data)
        {
            int counter = 1;
            int returnValue = 0;
            foreach (BorrowerData bor in data)
            {
                Console.WriteLine($"{counter}: Processing borrower: {bor.Ssn}");
                RI.FastPath($"LP9OA{bor.Ssn};;WRK GP01");
                string addMessage = $"Please add WRK GP01 in LP9OA for borrower: {bor.Ssn}. A DCR will be needed to update the OLS.curracclat.ProcessData table to delete the record in ProcessDataId: {bor.ProcessDataId} if the record was not processed correctly.";
                if (RI.CheckForText(1, 61, "OPEN ACTIVITY DETAIL"))
                {
                    RI.PutText(16, 12, "Review for current payment");
                    RI.PutText(17, 12, "{CURRACCLATEPAY} / " + UserId, ReflectionInterface.Key.F6);

                    if (RI.CheckForText(22, 3, "48003"))
                        DA.SetProcessed(bor.ProcessDataId);
                    else
                    {
                        returnValue = 1;
                        string message = string.Format($"There was an error adding WRK GP01 to borrower: {bor.Ssn}; Error message: {RI.GetText(22, 3, 50)}; {addMessage}");
                        RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        Console.WriteLine(message);
                    }
                }
                else
                {
                    returnValue = 1;
                    string message = string.Format($"Error accessing LP9OA for borrower: {bor.Ssn}; Screen reached: {RI.ScreenCode}; {addMessage}");
                    RI.LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    Console.WriteLine(message);
                }
                counter++;
                CheckForPause();
            }

            return returnValue;
        }

        public void CheckForPause()
        {
            if (PauseBetweenRecords)
            {
                Console.WriteLine("Please review the account then set this console in focus and hit enter to process the next account");
                _ = Console.ReadLine();
            }
        }
    }
}
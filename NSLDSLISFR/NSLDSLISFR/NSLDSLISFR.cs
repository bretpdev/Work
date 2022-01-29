using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace NSLDSLISFR
{
    public class NSLDSLISFR
    {
        ReflectionInterface ri;
        ProcessLogRun plr;
        const string printPopulationCommand = "printpopulationonly";
        public NSLDSLISFR(ReflectionInterface ri, ProcessLogRun plr)
        {
            this.ri = ri;
            this.plr = plr;
        }
        public void Process(string singleBorrowerIdentifier = null)
        {
            var data = new DataAccess(plr.ProcessLogId);
            Console.WriteLine("Gathering Borrower Population");
            var borrowers = GetFinalBorrowerPopulation(data);
            Console.WriteLine("Found {0} Borrowers", borrowers.Count);
            if (singleBorrowerIdentifier != null) //only run against one borrower
            {
                if (singleBorrowerIdentifier.ToLower() == printPopulationCommand)
                {
                    foreach (var borrower in borrowers)
                        Console.WriteLine(borrower.Ssn);
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey();
                    return;
                }
                borrowers = borrowers.Where(o => o.AccountNumber == singleBorrowerIdentifier || o.Ssn == singleBorrowerIdentifier).ToList();
                Info("Borrower population limited to identifier {0}, new population contains {1} borrower(s).", NotificationSeverityType.Informational, singleBorrowerIdentifier, borrowers.Count);
            }

            foreach (var borrower in borrowers)
            {
                ri.FastPath("tx3z/atl40" + borrower.Ssn);
                if (ri.ScreenCode == "TLX") // person not found in system, enter name and DOB
                {
                    ri.PutText(9, 36, "IDR");
                    if (borrower.Dob.HasValue && ri.CheckForText(15, 36, "_", "M"))
                        ri.PutText(15, 36, borrower.Dob.Value.ToString("MM/dd/yyyy"));
                    if (ri.CheckForText(11, 36, "_"))
                        ri.PutText(11, 36, borrower.LastName);
                    if (ri.CheckForText(13, 36, "_"))
                        ri.PutText(13, 36, borrower.FirstName);

                    ri.Hit(ReflectionInterface.Key.Enter);
                }
                else
                {
                    if (ri.CheckForText(11, 36, "_") || ri.CheckForText(13, 36, "_"))
                    {
                        Info("Couldn't find borrower {0} in ATL40.", NotificationSeverityType.Critical, borrower.AccountNumber);
                        continue;
                    }
                    ri.PutText(9, 36, "IDR", ReflectionInterface.Key.Enter);
                }
                if (ri.WaitForText(22, 2, "01004", 3))
                    Info("Successfully Processed {0}", NotificationSeverityType.Informational, borrower.AccountNumber);
                else if (ri.CheckForText(22, 2, "01018")) //entered key already found
                    Info("Borrower Already Processed {0}: {1}", NotificationSeverityType.Informational, borrower.AccountNumber, ri.GetText(22, 1, 80));
                else
                    Info("Unknown error processing borrower {0}.  {1}", NotificationSeverityType.Critical, borrower.AccountNumber, ri.GetText(22, 1, 80));
            }
            Console.WriteLine("Processing Complete");
        }

        private void Info(string message, NotificationSeverityType severityLevel, params object[] args)
        {
            message = string.Format(message, args);
            Console.WriteLine(message);
            plr.AddNotification(message, NotificationType.ErrorReport, severityLevel);
        }

        private List<FinalBorrowerInfo> GetFinalBorrowerPopulation(DataAccess data)
        {
            List<FinalBorrowerInfo> finalPopulation = data.GetBorrowersWithOpenIdrQueueTasks().Result;
            foreach (var borrower in finalPopulation.ToArray())
            {
                foreach (var spouse in data.GetBorrowerSpouses(borrower.Ssn).Result)
                {
                    if (finalPopulation.Any(o => o.Ssn == spouse.Ssn)) // If pop already has spouse (bc they are possibly bwr themselves) then don't add spouse
                        continue;
                    if (!data.BorrowerHasOpenNsldsInfoRequest(spouse.Ssn).Result) // If no request exists for NSLDS info for spouse, add them to list to make request for
                        finalPopulation.Add(new FinalBorrowerInfo()
                        {
                            //AccountNumber = acct,
                            Ssn = spouse.Ssn,
                            FirstName = spouse.FirstName,
                            LastName = spouse.LastName,
                            Dob = spouse.BirthDate
                        });
                }
                if (data.BorrowerHasOpenNsldsInfoRequest(borrower.Ssn).Result) // There is already a request for bwr NSLDS info, so no need to make a new request for them
                    finalPopulation.Remove(borrower);
            }
            return finalPopulation;
        }
    }
}

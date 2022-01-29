using System;
using System.Collections.Generic;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace PHONE4RMVL
{
    public class RemovePhoneFour
    {
        private ReflectionInterface ri { set; get; }
        private ProcessLogRun logRun { set; get; }
        private DataAccess da { set; get; }
        string[] HAW { get; set; }

        private readonly DataAccessHelper.Database Db;

        public RemovePhoneFour(ProcessLogRun LogRun, string ScriptId)
        {
            logRun = LogRun;
            ri = new ReflectionInterface();
            BatchProcessingLoginHelper.Login(logRun, ri, ScriptId, "BatchUheaa");
            ri.LogRun = logRun;
            da = new DataAccess(logRun);
            HAW = new string[] { "H", "A", "W" };
        }



        public int processPhones()
        {
            int returnValue = 0;
            logRun.AddNotification($"Phone4Rmvl commencing.", NotificationType.Other, NotificationSeverityType.Informational);
            Console.WriteLine($"This is PHONE4RMVL script.");

            ri.FastPath("TX3Z/ITX1J");
            if (ri.GetText(1, 72, 5) != "TXX1K")
            {
                logRun.AddNotification($"Session Could not be completed, no error reported.  The script expected to be on TXX1K screen but was on {ri.GetText(1, 72, 5)}.  Session message code: {ri.Message}", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                ri.CloseSession();
                return 1;
            }

            Console.WriteLine("Gathering borrower data. This could take a minute.");
            List<Borrowers> rawBorrowers = new List<Borrowers>();
            rawBorrowers = da.GetValues();
            if (rawBorrowers.Count == 0)
            {
                logRun.AddNotification($"No borrowers were found for processing, exiting script.", NotificationType.EndOfJob, NotificationSeverityType.Informational); 
                ri.CloseSession();
                return 0;
            }

            Console.WriteLine($"Count = {rawBorrowers.Count}");

            List<Borrowers> borrowers = new List<Borrowers>();
            borrowers = rawBorrowers
                        .GroupBy(o => new { o.Df_Prs_Id })
                        .Select(o => o.FirstOrDefault()).ToList();
#if DEBUG
            foreach (var bor in borrowers)
                Console.WriteLine($"bor = {bor.Df_Prs_Id}.");
            Console.WriteLine("----------------------------");
#endif
            
            Process(borrowers);

           
            ri.CloseSession();
            logRun.AddNotification($"Phone4Rmvl ending execution.", NotificationType.Other, NotificationSeverityType.Informational);
            return returnValue;
        }

        private void Process(List<Borrowers> borrowers)
        {
            int totalNF = 0;

            foreach (var borrower in borrowers)
            {
                string fastPath = "TX3Z/CTX1J;" + borrower.Df_Prs_Id ;
                ri.FastPath(fastPath);

                if (ri.GetText(23, 2, 5) == "01019") // ENTERED KEY NOT FOUND
                {
                    string msg = $"Entered Key {borrower.Df_Prs_Id} not found.";
                    logRun.AddNotification(msg, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    totalNF++;
                    continue;
                }
                
                ri.Hit(ReflectionInterface.Key.F6, 3);
                ri.PutText(16, 14, "M");                // Look at Mobile
                ri.Hit(ReflectionInterface.Key.Enter);
                OriginalMobile origMbl = origMBL(borrower);
                ri.PutText(16, 14, "H");                // Start on H no matter what.
                ri.Hit(ReflectionInterface.Key.Enter);
                processScreens(borrower, origMbl);
                bool isUpdated = invalidateMobile(borrower, true, origMbl);
                if(isUpdated)
                    addAnArc(borrower.borrSsn, borrower.IsCoborow);
            }
            Console.WriteLine($"Entered Keys Not Found: {totalNF} of {borrowers.Count} borrowers."); // ({totalNF > 0 ? (borrowers.Count / totalNF) : 0;}%.");
        }


        private void processScreens(Borrowers borrower, OriginalMobile origMbl)
        {
            for (int n = 0; n < 3; n++)
            {
                ri.PutText(16, 14, HAW[n]);
                ri.Hit(ReflectionInterface.Key.Enter);
                HAWStatus(borrower);
                if(ri.GetText(23, 2, 5) == "01105") // No screen currently
                    break;

                if (ri.GetText(17, 54, 1) == "N" || isEmptyValid())
                {
                    replaceWithMobile(borrower, origMbl);
                    replaceWithOrigM(origMbl);
                    ri.Hit(ReflectionInterface.Key.Enter);
                    if (ri.GetText(23, 2, 24) != "01097 PHONE DATA UPDATED" && ri.GetText(23, 2, 31) != "01100 PHONE TYPE HAS BEEN ADDED")
                    {
                        string msg = $"Unable to update mobile for account {borrower.Df_Prs_Id }. Msg:{ri.GetText(23, 2, 80)}.";
                        logRun.AddNotification(msg, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    }
                }
            }
        }

        private void HAWStatus(Borrowers bor)
        {
            bor.phoneScrape(ri);
            bor.determineStatus();
        }



        private bool invalidateMobile(Borrowers bor, bool isM, OriginalMobile orig)
        {
            ri.PutText(16, 14, "M");
            ri.Hit(ReflectionInterface.Key.Enter);
            if (ri.GetText(17, 54, 1) == "N")
                return false;
            if (ri.GetText(23, 2, 5) == "01105") // No screen currently
                return false;

            //ScreenInfo scrape = screenScrape(orig);
            orig.ValidPhone = "N";
            //replaceScreen(scrape, bor, isM);
            replaceWithOrigM(orig);
            ri.PutText(17, 54, "N");
            ri.Hit(ReflectionInterface.Key.Enter);
            if (ri.GetText(23, 2, 5) == "01097") // Success, required acxtion
                return true;
            else
                logRun.AddNotification($"Screen could not be updated for borrower {bor.Df_Prs_Id}. Contact support services.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            return false;
            //updateScreen(bor, isM);
        }

        private void replaceWithMobile(Borrowers borrower, OriginalMobile orig)
        {
            if(borrower.domestic && orig.domestic)
            {
                replaceDomestic(orig);
            } 
            else if (borrower.foreign && orig.domestic)
            {
                replaceDomestic(orig);
                clearForeign();
            }
            else if (borrower.domestic && orig.foreign)
            {
                replaceForeign(orig);
                clearDomestic();
            }
            else if (borrower.foreign && orig.foreign)
            {
                replaceForeign(orig);
            } 
        }
    

        private void replaceDomestic(OriginalMobile orig)
        {
            ri.PutText(17, 14, orig.domNpa);
            ri.PutText(17, 23, orig.domNxx);
            ri.PutText(17, 31, orig.domLcl);
            if (orig.domExt.Length > 0)
                ri.PutText(17, 40, orig.domExt);
        }

        private void replaceForeign(OriginalMobile orig)
        {
            ri.PutText(18, 15, orig.forNpa);
            ri.PutText(18, 24, orig.forNxx);
            ri.PutText(18, 36, orig.forLcl );
            if (orig.forExt.Length > 0 )
                ri.PutText(18, 53, orig.forExt );
        }

        private void clearDomestic()
        {
            ri.PutText(17, 14, " ", true);
            ri.PutText(17, 23, " ", true);
            ri.PutText(17, 31, " ", true);
            ri.PutText(17, 40, " ", true);
        }

        private void clearForeign()
        {
            ri.PutText(18, 15, "", true);
            ri.PutText(18, 24, "", true);
            ri.PutText(18, 36, "", true);
            ri.PutText(18, 53, "", true);
        }

        private void replaceWithOrigM(OriginalMobile orig)
        {
            ri.PutText(16, 20, orig.MBL);
            ri.PutText(16, 30, orig.Consent);
            ri.PutText(16, 45, orig.lastVer.SafeSubString(0, 2));
            ri.PutText(16, 48, orig.lastVer.SafeSubString(2, 2));
            ri.PutText(16, 51, orig.lastVer.SafeSubString(4, 2));
            ri.PutText(16, 78, "A");
            ri.PutText(19, 14, orig.SourceCode);
            ri.PutText(17, 54, "Y");

            if (orig.foreign)
            {
                clearForeign();
                ri.PutText(18, 15, orig.forNpa);
                ri.PutText(18, 24, orig.forNxx);
                ri.PutText(18, 36, orig.forLcl);
                ri.PutText(18, 53, orig.forExt);
            }
            else
            {
                clearDomestic();
                ri.PutText(17, 14, orig.domNpa);
                ri.PutText(17, 23, orig.domNxx);
                ri.PutText(17, 31, orig.domLcl);
                ri.PutText(17, 40, orig.domExt);
            }
            
        }


        private string todaysDate()
        {
            return DateTime.Now.ToString("MMddyy");
        }


        OriginalMobile origMBL(Borrowers account)
        {
            OriginalMobile om = new OriginalMobile();
            om.PhoneType = ri.GetText(16, 14, 1);
            //om.MBL = ri.GetText(16, 20, 1);
            //om.Consent = ri.GetText(16, 30, 1);
            //om.SourceCode = ri.GetText(19, 14, 2);
            om.lastVer = todaysDate();
            om.MBL = account.MBL;
            om.Consent = account.consent;
            om.SourceCode = account.phSource;
            om.domNpa = ri.GetText(17, 14, 3);
            
            om.domNxx = ri.GetText(17, 23, 3);
            om.domLcl = ri.GetText(17, 31, 4);
            om.domExt = ri.GetText(17, 40, 5);

            om.forNpa = ri.GetText(18, 15, 3);
            om.forNxx = ri.GetText(18, 24, 5);
            om.forLcl = ri.GetText(18, 36, 11);
            om.forExt = ri.GetText(18, 53, 5);
            
            om.domExt = om.domExt.Replace("_", "");
            om.forExt = om.forExt.Replace("_", "");
            om.determineStatus();
            
            return om;
        }


        public bool isEmptyValid()
        {
            if (
                (ri.GetText(17, 23, 3) == "___" && ri.GetText(17, 31, 4) == "____" && ri.GetText(17, 40, 5) == "_____" &&
                ri.GetText(18, 15, 3) == "___" && ri.GetText(18, 24, 5) == "_____" && ri.GetText(18, 36, 11) == "___________")
                ||
                (ri.GetText(17, 23, 3) == "   " && ri.GetText(17, 31, 4) == "    " && ri.GetText(17, 40, 5) == "     " &&
                ri.GetText(18, 15, 3) == "   " && ri.GetText(18, 24, 5) == "     " && ri.GetText(18, 36, 11) == "           ")
              )
                return true;
            return false;
        }

        private void addAnArc(string ssn, int coBorrower)
        {
            string comment = coBorrower == 1 ? $"Phone four removed for endorser / coborrower." : $"Phone four removed for borrower.";
            ArcData arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = ssn, // Always borrower ssn
                Arc = "XPHN4",
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Comment = comment,
                IsEndorser = false,
                IsReference = false,
                ScriptId = "PHONE4RMVL",
            };
            ArcAddResults result = arc.AddArc();

            if (!result.ArcAdded)
            {
                string msg = $"Failure adding arc XPHN4 for ssn {ssn}.";
                logRun.AddNotification(msg, NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
        }
    }
}

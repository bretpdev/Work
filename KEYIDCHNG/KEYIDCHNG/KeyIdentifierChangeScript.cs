using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.Baa;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace KEYIDCHNG
{
    public class KeyIdentifierChangeScript : ScriptBase
    {
        ProcessLogRun plr;
        public KeyIdentifierChangeScript(ReflectionInterface ri)
            : base(ri, "KEYIDCHNG", DataAccessHelper.Region.Uheaa)
        {
            plr = new ProcessLogRun(ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode);
        }

        public override void Main()
        {
            var plr = new ProcessLogRun(ProcessLogData.ProcessLogId, ScriptId, AppDomain.CurrentDomain, Assembly.GetExecutingAssembly(), DataAccessHelper.CurrentRegion, DataAccessHelper.CurrentMode, false);
            using (var form = new MainForm(plr, RI))
                if (form.ShowDialog() == DialogResult.OK)
                    Process(form.KeyIdentifierChangeModel);
            DataAccessHelper.CloseAllManagedConnections();
        }

        List<string> messages = new List<string>();
        private void Process(KeyIdentifierChangeModel model)
        {
            if (model.Approve)
                Approve(model);
            else
                Reject(model);
            if (model.UpdateCompass)
                CloseAkQueues(model.Ssn);
            if (model.UpdateOneLink)
                AssignAndCloseAllAskeyQueues(model.Ssn, model.Comments);

            Dialog.Def.Ok(string.Join(Environment.NewLine, messages));
        }

        private void Reject(KeyIdentifierChangeModel model)
        {
            if (model.UpdateCompass)
            {
                var arc = new ArcData(DataAccessHelper.Region.Uheaa)
                {
                    ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                    Arc = "FJDAF",
                    Comment = model.Comments,
                    ScriptId = ScriptId,
                    AccountNumber = model.Ssn
                };
                var results = arc.AddArc();
                bool fjdaf = results.ArcAdded;
                if (!fjdaf)
                    messages.Add("Unable to leave FJDAF rejection ARC.");
                else
                    messages.Add("Successfully left FJDAF rejection ARC.");
            }
            if (model.UpdateOneLink)
            {
                bool sfollow = CreateSfollowTask(model.Ssn, model.Comments);
                if (!sfollow)
                    messages.Add("Unable to leave SFOLLOW rejection Task.");
                else
                    messages.Add("Successfully left SFOLLOW rejection Task.");
            }
        }

        private void Approve(KeyIdentifierChangeModel model)
        {
            var checkResults = new Action<ApprovalResult, string, string>((results, region, riMessage) =>
            {
                if (results == ApprovalResult.BorrowerNotFound)
                    messages.Add($"Unable to locate borrower in {region}.");
                if (results == ApprovalResult.NoChangesFound)
                    messages.Add($"No changes found in {region}.  Please ensure new information is entered correctly.  If it is correct, no adjustment is necessary.");
                if (results == ApprovalResult.ErrorMakingChanges)
                    messages.Add($"Changes on {region} were unsuccessful, the following error was encountered: {riMessage}");
                if (results == ApprovalResult.ErrorMakingChangesBadAccess)
                    messages.Add($"Changes on {region} were unsuccessful, you may not have full access to this screen: {RI.ScreenCode}");
                if (results == ApprovalResult.ChangesMadeNoArc)
                    messages.Add($"Changes on {region} were successful, but ARC was not able to be added.");
                if (results == ApprovalResult.ChangedMadeArcSuccessful)
                    messages.Add($"Changes on {region} were successful.");

            });
            if (model.UpdateCompass)
            {
                var compassResults = ApproveCompass(model);
                checkResults(compassResults, "Compass", RI.Message);
            }
            if (model.UpdateOneLink)
            {
                var oneLinkResults = ApproveOneLink(model);
                checkResults(oneLinkResults, "OneLink", RI.AltMessage);
            }
            if (!model.UpdateCompass && !model.UpdateOneLink)
                messages.Add("Unable to find any warehouse data for this borrower.");
        }

        const string shortDateFormat = "MMddyy";
        const string longDateFormat = "MMddyyyy";
        public ApprovalResult ApproveCompass(KeyIdentifierChangeModel model)
        {
            var demos = GetCompassDemos(model.Ssn);
            if (demos == null)
                return ApprovalResult.BorrowerNotFound;
            var comparison = new KeyIdentifierChangeComparison(model, demos);
            if (!comparison.AnyChanges)
                return ApprovalResult.NoChangesFound;

            RI.FastPath("TX3ZCTX1JB;" + model.Ssn);
            RI.PutText(3, 61, DateTime.Now.ToString(shortDateFormat));

            try
            {
                if (comparison.FirstNameChanged)
                    RI.PutText(4, 34, model.FirstName, true);
                if (comparison.MiddleInitialChanged)
                    RI.PutText(4, 53, model.MiddleInitial, true);
                if (comparison.LastNameChanged)
                    RI.PutText(4, 6, model.LastName, true);
                if (comparison.DobChanged)
                    RI.PutText(20, 6, model.DOB.Value.ToString(longDateFormat));
            }
            catch (COMException)
            {
                RI.Hit(ReflectionInterface.Key.Esc);
                return ApprovalResult.ErrorMakingChangesBadAccess;
            }
            RI.Hit(ReflectionInterface.Key.Enter);
            if (RI.MessageCode != "01093")
                return ApprovalResult.ErrorMakingChanges;
            var arc = new ArcData(DataAccessHelper.Region.Uheaa)
            {
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Arc = "KNOTE",
                Comment = model.Comments,
                ScriptId = ScriptId,
                AccountNumber = model.Ssn
            };
            var results = arc.AddArc();
            if (!results.ArcAdded)
                if (!RI.Atd37FirstLoan(model.Ssn, arc.Arc, arc.Comment, ScriptId, RI.UserId))
                    return ApprovalResult.ChangesMadeNoArc;
            return ApprovalResult.ChangedMadeArcSuccessful;
        }

        public ApprovalResult ApproveOneLink(KeyIdentifierChangeModel model)
        {
            var demos = GetOneLinkDemos(model.Ssn);
            if (demos == null)
                return ApprovalResult.BorrowerNotFound;
            var comparison = new KeyIdentifierChangeComparison(model, demos);
            if (!comparison.AnyChanges)
                return ApprovalResult.NoChangesFound;

            RI.FastPath("LP22C" + model.Ssn);
            RI.PutText(3, 9, "C"); //source code

            try
            {
                if (comparison.FirstNameChanged)
                    RI.PutText(4, 44, model.FirstName, true);
                if (comparison.MiddleInitialChanged)
                    RI.PutText(4, 60, model.MiddleInitial, true);
                if (comparison.LastNameChanged)
                    RI.PutText(4, 5, model.LastName, true);
                if (comparison.DobChanged)
                    RI.PutText(4, 72, model.DOB.Value.ToString(longDateFormat));
            }
            catch (COMException)
            {
                RI.Hit(ReflectionInterface.Key.Esc);
                return ApprovalResult.ErrorMakingChangesBadAccess;
            }
            RI.Hit(ReflectionInterface.Key.Enter);
            RI.Hit(ReflectionInterface.Key.F6);
            if (!RI.AltMessageCode.IsIn("01093", "49000", "48003"))
                return ApprovalResult.ErrorMakingChanges;
            return ApprovalResult.ChangedMadeArcSuccessful;
        }

        private void AssignAndCloseAllAskeyQueues(string borrowerSsn, string comments, bool retryOnFail = true)
        {
            string queue = "ASKEY";
            string department = "SKP";
            string activityType = "AM";
            string activityContact = "36";
            string activityActionCode = "GSSNB";

            bool anyChangesPosted = false;
            do
            {
                RI.FastPath("LP8YC");
                RI.PutText(6, 37, department);
                RI.PutText(8, 37, queue);
                RI.PutText(12, 37, borrowerSsn);
                RI.Hit(ReflectionInterface.Key.Enter);

                if (RI.AltMessageCode.IsIn("47004", "47432")) //no data for entered borrower
                {
                    RI.AddCommentInLP50(borrowerSsn, activityType, activityContact, activityActionCode, comments, ScriptId);
                    return;
                }
                anyChangesPosted = false;
                //assign all records to current user
                PageHelper.IteratePagesOnly(RI, s =>
                {
                    for (int row = 7; row <= 20; row++)
                    {
                        var status = RI.GetText(row, 33, 1);
                        if (status == "W" || status == "A")
                        {
                            RI.PutText(row, 33, "A");
                            RI.PutText(row, 38, UserId, true);
                        }
                    }
                    RI.Hit(ReflectionInterface.Key.Enter); //validate screen
                    RI.Hit(ReflectionInterface.Key.F6); //post changes
                    if (RI.AltMessageCode != "49007") //changes were made
                    {
                        anyChangesPosted = true;
                        s.ContinueIterating = false;
                        //a posted change knocks us right back to page 1, so restart the iterations.
                    }
                });
            } while (anyChangesPosted);


            RI.FastPath("LP9AC;;;;;");
            bool correctSsn = RI.CheckForText(6, 57, borrowerSsn);
            bool correctQueue = RI.CheckForText(3, 24, queue);
            if (RI.CheckForText(1, 71, "QUEUE") && (!correctSsn || !correctQueue))
            {
                messages.Add("Unable to close ASKEY queues because another task is in a work status, please close all ASKEY queues manually.");
                RI.AddCommentInLP50(borrowerSsn, activityType, activityContact, activityActionCode, comments, ScriptId);
                return;
            }



            if (!RI.CheckForText(1, 71, "QUEUE"))
            {
                if (RI.CheckForText(1, 66, "QUEUE SELECTION"))
                {
                    RI.PutText(6, 37, queue);
                    RI.PutText(12, 37, "Y"); //assigned tasks only
                    RI.Hit(ReflectionInterface.Key.Enter);
                }

                if (RI.AltMessageCode == "47423" || RI.AltMessageCode == "47420")  //no more records found, still need to leave an LP50A though
                {
                    RI.AddCommentInLP50(borrowerSsn, activityType, activityContact, activityActionCode, comments, ScriptId);
                    return;
                }
            }


            bool successfullyLeftLp50A = false;
            string[] noMoreRecordsCodes = new string[] { "46004", "46003" };
            while (!RI.AltMessageCode.IsIn(noMoreRecordsCodes))
            {
                while (ShouldHitF8OnLp9a(borrowerSsn, noMoreRecordsCodes))
                    RI.Hit(ReflectionInterface.Key.F8);
                if (!RI.AltMessageCode.IsIn(noMoreRecordsCodes))
                {
                    Thread.Sleep(750);  //needed delay, otherwise half the items get skipped 3/10/20 DEW
                    RI.Hit(ReflectionInterface.Key.F2);
                    RI.Hit(ReflectionInterface.Key.F7);
                    //should now be on LP50A
                    RI.PutText(7, 20, activityType);
                    RI.PutText(8, 20, activityContact);
                    RI.PutText(9, 20, activityActionCode);
                    RI.Hit(ReflectionInterface.Key.Enter);

                    RI.PutText(13, 2, comments, true);
                    RI.PutText(7, 2, activityType, true);
                    RI.PutText(7, 5, activityContact, true);
                    RI.Hit(ReflectionInterface.Key.F6);
                    RI.Hit(ReflectionInterface.Key.F12);

                    RI.Hit(ReflectionInterface.Key.F2);
                    RI.Hit(ReflectionInterface.Key.F6);
                    if (RI.AltMessageCode == "47460")
                    {
                        if (retryOnFail)
                            AssignAndCloseAllAskeyQueues(borrowerSsn, comments, false); //try once more
                        else
                            messages.Add("Unable to close tasks. No activity record found for this borrower. Please close the tasks manually.");
                        return;
                    }
                    if (RI.AltMessageCode != "49000")
                        plr.AddNotification("Unable to close ASKEY queue task.  " + RI.AltMessage, NotificationType.ErrorReport, NotificationSeverityType.Warning);
                    else
                        successfullyLeftLp50A = true;

                    RI.Hit(ReflectionInterface.Key.F8);
                }
            }
            if (!successfullyLeftLp50A)
                RI.AddCommentInLP50(borrowerSsn, activityType, activityContact, activityActionCode, comments, ScriptId);
        }

        private bool ShouldHitF8OnLp9a(string borrowerSsn, string[] noMoreRecordsCodes)
        {
            bool correctBorrower = RI.CheckForText(17, 70, borrowerSsn);
            bool notAnErrorCode = !RI.AltMessageCode.IsIn(noMoreRecordsCodes);
            return notAnErrorCode && !correctBorrower;
        }

        public void CloseAkQueues(string ssn)
        {
            string queue = "AK";
            string subQueue = "01";
            string status = "C";
            string actionResponse = "COMPL";
            RI.FastPath($"TX3Z/ITX6X{queue};{subQueue};{ssn}");
            if (RI.MessageCode == "01020")
                return;
            if (RI.MessageCode == "80014")
            {
                messages.Add("Could not close AK Queues: " + RI.Message);
                return;
            }

            List<string> ssnsToUnassign = new List<string>();
            PageHelper.IterationSettings settings = PageHelper.IterationSettings.Default();
            settings.RowIncrementValue = 3;
            PageHelper.Iterate(RI, row =>
            {
                if (RI.CheckForText(row, 75, "U"))
                {
                    RI.PutText(21, 18, RI.GetText(row, 3, 2), ReflectionInterface.Key.Enter, true);
                    if (!RI.CheckForText(1, 76, "TSX25"))
                    {
                        RI.FastPath("TX3Z/CTX6J;;;;;;;;;;;;");
                        RI.PutText(7, 42, queue, true);
                        RI.PutText(8, 42, subQueue, true);
                        RI.PutText(12, 42, "W", true);
                        RI.PutText(13, 42, UserId, true);
                        RI.Hit(ReflectionInterface.Key.Enter);
                        RI.PutText(8, 15, "", true);
                        RI.Hit(ReflectionInterface.Key.Enter);
                    }
                    settings.ContinueIterating = false;
                    ssnsToUnassign.Clear();
                    CloseAkQueues(ssn);
                    return;
                }
                string foundSsn = RI.GetText(row, 6, 9);
                if (foundSsn == ssn)
                {
                    RI.PutText(21, 18, RI.GetText(row, 3, 2), ReflectionInterface.Key.F2, true);
                    RI.PutText(8, 19, status);
                    RI.PutText(9, 19, actionResponse, ReflectionInterface.Key.Enter);
                    RI.Hit(ReflectionInterface.Key.F12);
                    return;
                }
                ssnsToUnassign.Add(foundSsn);
            }, settings);
            foreach (string foundSsn in ssnsToUnassign)
            {
                RI.FastPath("TX3Z/CTX6J;;;;;;;;;;;;");
                RI.PutText(7, 42, queue, true);
                RI.PutText(8, 42, subQueue, true);
                RI.PutText(9, 42, foundSsn, true);
                RI.PutText(9, 76, "D", true);
                RI.Hit(ReflectionInterface.Key.Enter);
                RI.PutText(8, 15, "", true);
                RI.Hit(ReflectionInterface.Key.Enter);
            }
        }

        public bool CreateSfollowTask(string ssn, string comment)
        {
            string workGroup = "SFOLLOW";
            RI.FastPath("LP9OA" + ssn + ";;" + workGroup);
            //See if we got in.
            if (!RI.CheckForText(1, 61, "OPEN ACTIVITY DETAIL"))
                return false;

            RI.PutText(16, 12, comment);
            RI.PutText(13, 25, "", true); //blank out user id field
            RI.Hit(ReflectionInterface.Key.F6);
            return RI.WaitForText(22, 3, "48003", 2);
        }

        private SystemBorrowerDemographics GetCompassDemos(string ssn)
        {
            try
            {
                return RI.GetDemographicsFromTx1j(ssn);
            }
            catch (DemographicException)
            {
                return null;
            }
        }

        private SystemBorrowerDemographics GetOneLinkDemos(string ssn)
        {
            try
            {
                return RI.GetDemographicsFromLP22(ssn);
            }
            catch (DemographicException)
            {
                return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;

namespace ACURINTC
{
    class CompassTaskHelper : TaskHelper
    {
        public CompassTaskHelper(General g, bool skipTaskClose) : base(g, skipTaskClose) { }

        public override bool CreateTask(PendingDemos task, string newQueue, string commentIntro)
        {
            string demographicsSource = G.DSH.GetDemographicsSourceName(task.DemographicsSourceId);
            string comment = string.Format("{0}: {1}", demographicsSource, task.HasAddress ? task.GenerateBracketedAddressString() : task.PrimaryPhone);
            if (commentIntro.IsPopulated())
                comment = string.Format("{0}: {1}", commentIntro, comment);
            ArcData ad = new ArcData(DataAccessHelper.CurrentRegion)
            {
                Arc = newQueue,
                Comment = comment,
                ScriptId = G.ScriptId,
                ArcTypeSelected = ArcData.ArcType.Atd22ByBalance,
                AccountNumber = task.AccountNumber
            };
            if (!ad.AddArc().ArcAdded)
                if (!RI.Atd37FirstLoan(task.Ssn, newQueue, comment, G.ScriptId, G.UserId, null))
                    return false;

            task.StatusInfo.ArcsCreated.Add(newQueue);
            return true;
        }

        public override bool BorrowerHasForeignNumber(string ssnOrAccountNumber)
        {
            //Hit up LP22 to get the borrower's legal address.
            if (ssnOrAccountNumber.Length == 9)
            {
                RI.FastPath("TX3Z/ITX1JB" + ssnOrAccountNumber);
            }
            else
            {
                RI.FastPath("TX3Z/ITX1JB*");
                RI.PutText(6, 61, ssnOrAccountNumber);
            }
            //See if the COUNTRY field is populated.
            return !(RI.GetText(18, 15, 3).Replace("__", "").Length > 0);
        }

        public override bool BorrowerHasForeignAddress(string ssnOrAccountNumber)
        {
            //Hit up LP22 to get the borrower's legal address.
            if (ssnOrAccountNumber.Length == 9)
            {
                RI.FastPath("TX3Z/ITX1JB" + ssnOrAccountNumber);
            }
            else
            {
                RI.FastPath("TX3Z/ITX1JB*");
                RI.PutText(6, 61, ssnOrAccountNumber);
            }
            return (RI.GetText(12, 77, 2).Replace("__", "").Length > 0);
        }

        public override bool AddressMatches(PendingDemos task)
        {
            //Hit up TX1J to get the borrower's address.
            RI.FastPath(string.Format("TX3Z/CTX1JB;{0}", task.Ssn));
            if (RI.CheckForText(23, 2, "01080"))
            {
                RI.FastPath(string.Format("TX3Z/CTX1JS;{0}", task.Ssn));
                if (RI.CheckForText(23, 2, "01019", "01222"))
                {
                    RI.FastPath(string.Format("TX3Z/CTX1JE;{0}", task.Ssn));
                }
            }
            if (!RI.CheckForText(1, 71, "TXX"))
            {
                throw new Exception("Could not access borrower " + task.AccountNumber + " on TX1J: " + RI.GetText(23, 2, 77));
            }

            //The column for address type is different depending on the person type.
            int addressTypeColumn = (RI.CheckForText(1, 71, "TXX1R-02", "TXX1R-04") ? 13 : 14);

            //Go to the legal address if it's not already displayed.
            RI.Hit(Key.F6);
            RI.Hit(Key.F6);
            if (!RI.CheckForText(10, addressTypeColumn, "L"))
            {
                RI.PutText(10, addressTypeColumn, "L", Key.Enter);
            }

            //Only valid addresses can be considered a match.
            if (RI.CheckForText(11, 55, "N"))
                return false;

            //Compare the state, zip, and street address.
            if (!RI.CheckForText(14, 32, task.State))
                return false;
            if (!RI.CheckForText(14, 40, task.ZipCode.SafeSubString(0, 5)))
                return false;
            string existingAddr1 = RI.GetText(11, 10, 35);
            string existingAddr2 = RI.GetText(12, 10, 35);
            if (existingAddr1 != task.Address1 || existingAddr2 != task.Address2)
                return false;

            return true;
        }

        public override bool AddComment(QueueInfo data, PendingDemos task, SystemCode code, string arc, string comment)
        {
            //compass
            string demographicsSource = G.DSH.GetDemographicsSourceName(task.DemographicsSourceId);
            ArcData ad = new ArcData(DataAccessHelper.CurrentRegion)
            {
                Arc = arc,
                Comment = comment,
                ScriptId = G.ScriptId,
                ArcTypeSelected = ArcData.ArcType.Atd22ByBalance,
                AccountNumber = task.AccountNumber
            };
            if (!ad.AddArc().ArcAdded)
            {
                if (!RI.Atd37FirstLoan(task.Ssn, arc, comment, G.ScriptId, G.UserId, null))
                {
                    //Anything else is an error.
                    if (!CreateTask(task, "", "Unable to add COMPASS comment"))
                        G.LogRun.AddNotification(string.Format("AccountNumber: {0} QueueName: {1} CapturedDemographics: {2} ErrorReason: {3}", task.AccountNumber, data.DemographicsReviewQueue, task.OriginalAddressText, comment), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    return false;
                }
            }

            return true;
        }
    }
}

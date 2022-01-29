using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;
using Uheaa.Common.ProcessLogger;


namespace ACURINTC
{
    class OneLinkTaskHelper : TaskHelper
    {
        public OneLinkTaskHelper(General g, bool skipTaskClose) : base(g, skipTaskClose) { }
        public override bool CreateTask(PendingDemos task, string newQueue, string commentIntro)
        {
            string demographicsSource = G.DSH.GetDemographicsSourceName(task.DemographicsSourceId);
            string comment = string.Format("{0} {1}", demographicsSource, task.OriginalAddressText);
            if (commentIntro.IsPopulated())
                comment = string.Format("{0}: {1}", commentIntro, comment);
            RI.FastPath(string.Format("LP9OA{0};;{1}", task.Ssn, newQueue));
            RI.PutText(11, 25, DateTime.Now.ToString("MMddyyyy"));
            RI.PutText(16, 12, comment);
            RI.Hit(Key.F6);
            return RI.CheckForText(22, 3, "48003");
        }

        public override bool BorrowerHasForeignNumber(string ssnOrAccountNumber)
        {
            //Hit up LP22 to get the borrower's legal address.
            if (ssnOrAccountNumber.Length == 9)
                RI.FastPath(string.Format("LP22I{0};;;;L", ssnOrAccountNumber));
            else
                RI.FastPath(string.Format("LP22I;;;L;;;{0}", ssnOrAccountNumber));
            //See if the COUNTRY field is populated.
            return (RI.GetText(16, 14, 17).Length > 0);
        }

        public override bool BorrowerHasForeignAddress(string ssnOrAccountNumber)
        {
            //Hit up LP22 to get the borrower's legal address.
            if (ssnOrAccountNumber.Length == 9)
                RI.FastPath(string.Format("LP22I{0};;;;L", ssnOrAccountNumber));
            else
                RI.FastPath(string.Format("LP22I;;;L;;;{0}", ssnOrAccountNumber));
            //See if the COUNTRY field is populated.
            return (RI.GetText(11, 55, 25).Length > 0);
        }

        public override bool AddressMatches(PendingDemos task)
        {
            //Make sure we at least got an SSN or account number.
            if (task.AccountNumber.IsNullOrEmpty() && (task.Ssn.IsNullOrEmpty()))
            {
                throw new Exception("SSN and account number are both missing.");
            }

            //Hit up LP22 to get the borrower's legal address.
            if (task.Ssn.IsPopulated() && task.Ssn.Length == 9)
                RI.FastPath(string.Format("LP22I{0};;;;L", task.Ssn));
            else
                RI.FastPath(string.Format("LP22I;;;L;;;{0}", task.AccountNumber));

            //Compare the address in question to the borrower's legal address.
            if (!RI.CheckForText(12, 60, task.ZipCode.SafeSubString(0, 5)))
                return false;
            if (!RI.CheckForText(12, 52, task.State))
                return false;
            string existingAddr1 = RI.GetText(10, 9, 35);
            string existingAddr2 = RI.GetText(11, 9, 35);
            if (existingAddr1 != task.Address1 || existingAddr2 != task.Address2)
                return false;
            if (!RI.CheckForText(12, 9, task.City))
                return false;

            return true;
        }

        public override bool AddComment(QueueInfo data, PendingDemos task, SystemCode code, string arc, string comment)
        {
            //onelink
            RI.AddCommentInLP50(task.Ssn, code.ActivityType, code.ContactType, arc, comment, G.ScriptId);
            if (!RI.CheckForText(22, 3, "48003", "48081"))
                return false;
            return true;
        }

        public bool AddressIsValid(PendingDemos task)
        {
            //Make sure we at least got an SSN or account number.
            if (task.AccountNumber.IsNullOrEmpty() && (task.Ssn.IsNullOrEmpty()))
            {
                throw new Exception("SSN and account number are both missing.");
            }

            //Hit up LP22 to get the borrower's legal address.
            if (task.Ssn.IsPopulated() && task.Ssn.Length == 9)
                RI.FastPath(string.Format("LP22I{0};;;;L", task.Ssn));
            else
                RI.FastPath(string.Format("LP22I;;;L;;;{0}", task.AccountNumber));

            return RI.CheckForText(10, 57, "Y");
        }
    }
}

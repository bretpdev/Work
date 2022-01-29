using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace ACURINTC
{
    class AddressHelper
    {
        private ReflectionInterface RI;
        private DataAccess DA;
        private PendingDemos task;
        public AddressHelper(ReflectionInterface ri, PendingDemos task, DataAccess da)
        {
            this.RI = ri;
            this.task = task;
            this.DA = da;
        }
        public bool CompassCompareAddressHistory()
        {
            var previousAddresses = DA.GetBorrowerRecentAddressHistory(task.Ssn);
            var matchingAddresses = previousAddresses.Where(a => AddressMatchesHistory(a));
            var addr1Matches = previousAddresses.Where(a => a.Address1 == task.Address1);

            var maxMatchingAddress = matchingAddresses.OrderByDescending(o => o.VerificationDate).FirstOrDefault();
            if (maxMatchingAddress == null)
                return false;
            if (maxMatchingAddress.AddressIsValid)
                return false;
            return true;
        }

        private bool AddressMatchesHistory(AddressHistory ah)
        {
            return Match(ah.Address1, task.Address1)
                && Match(ah.Address2, task.Address2)
                && Match(ah.City, task.City)
                && Match(ah.StateCode, task.State)
                && Match(ah.Zip, task.ZipCode)
                && Match(ah.Country, task.Country)
                && Match(ah.ForeignState, task.ForeignState);
        }

        private bool Match(string first, string second)
        {
            return (first ?? "").Trim() == (second ?? "").Trim();
        }

        public bool AddressIsInvalidInOneLinkWithinPastYear()
        {
            //Make sure we at least got an SSN or account number.
            if (task.AccountNumber.IsNullOrEmpty() && task.Ssn.IsNullOrEmpty())
            {
                throw new Exception("SSN and account number are both missing.");
            }

            //Get the borrower's SSN if needed.
            if (task.Ssn.IsNullOrEmpty() || task.Ssn.Length < 9)
            {
                RI.FastPath(string.Format("LP22I;;;L;;;{0}", task.AccountNumber));
                task.Ssn = RI.GetText(3, 23, 9);
            }

            //Hit up LP2J to get the borrower's address history.
            RI.FastPath(string.Format("LP2JI{0};X;;;Y", task.Ssn));
            if (RI.CheckForText(22, 3, "47004"))
                return false;

            //Check whether we got the selection screen or the target screen.
            if (RI.CheckForText(1, 75, "SELECT"))
            {
                //Selection screen. Select all.
                RI.PutText(3, 13, "X", Key.Enter);

                //Look through the historical addresses for a match.
                for (int dateRow = 4; dateRow <= 22; dateRow += 6)
                {
                    //Move on to the next page if we're done with the current one.
                    if (!RI.CheckForText(dateRow, 63, "DATE"))
                    {
                        RI.Hit(Key.F8);
                        dateRow = 4;
                    }
                    //Stop looking when we run out of address history.
                    if (RI.CheckForText(22, 3, "46004"))
                        break;

                    //Stop looking once we get a date that's older than a year.
                    string date = RI.GetText(dateRow, 68, 2) + RI.GetText(dateRow, 70, 2) + RI.GetText(dateRow, 72, 4);
                    if (date == "MMDDCCYY")
                        break;

                    int addressMonth = int.Parse(RI.GetText(dateRow, 68, 2));
                    int addressDay = int.Parse(RI.GetText(dateRow, 70, 2));
                    int addressYear = int.Parse(RI.GetText(dateRow, 72, 4));
                    DateTime addressDate = new DateTime(addressYear, addressMonth, addressDay);
                    if (DateTime.Now.AddYears(-1) > addressDate)
                        break;

                    //Check the address validity indicator.
                    if (RI.CheckForText(dateRow + 1, 80, "Y"))
                        continue;

                    //Check for an exact match on the state.
                    if (!RI.CheckForText(dateRow + 3, 45, task.State))
                        continue;

                    //Check for a match on the first five digits of the zip code.
                    if (!RI.CheckForText(dateRow + 3, 52, task.ZipCode.SafeSubString(0, 5)))
                        continue;

                    string existingAddr1 = RI.GetText(dateRow + 1, 9, 35);
                    string existingAddr2 = RI.GetText(dateRow + 2, 9, 35);

                    if (existingAddr1 != task.Address1 || existingAddr2 != task.Address2)
                        continue;

                    //If none of the above checks fail and we get to this point, we have a match.
                    return true;
                }//for
            }
            else
            {
                //Target screen. Check the one record.
                string date = RI.GetText(4, 68, 2) + RI.GetText(4, 70, 2) + RI.GetText(4, 72, 4);
                if (date == "MMDDCCYY")
                    return false;

                int addressMonth = int.Parse(RI.GetText(4, 68, 2));
                int addressDay = int.Parse(RI.GetText(4, 70, 2));
                int addressYear = int.Parse(RI.GetText(4, 72, 4));
                DateTime addressDate = new DateTime(addressYear, addressMonth, addressDay);
                if (DateTime.Now.AddYears(-1) > addressDate)
                    return false;
                if (RI.CheckForText(5, 80, "Y"))
                    return false;
                if (!RI.CheckForText(7, 45, task.State))
                    return false;
                if (!RI.CheckForText(7, 52, task.ZipCode.SafeSubString(0, 5)))
                    return false;
                string existingAddr1 = RI.GetText(5, 9, 35);
                string existingAddr2 = RI.GetText(6, 9, 35);
                if (existingAddr1 != task.Address1 || existingAddr2 != task.Address2)
                    return false;
                return true;
            }

            //If we didn't return true before now, then no matches were found.
            return false;
        }

    }
}

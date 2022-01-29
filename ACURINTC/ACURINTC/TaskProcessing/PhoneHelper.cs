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
    class PhoneHelper
    {
        private IReflectionInterface RI;
        private DataAccess DA;
        private PendingDemos task;
        private string phoneNumber;
        private PhoneType phoneType;
        public PhoneHelper(IReflectionInterface ri, PendingDemos task, DataAccess da, string phoneNumber, PhoneType phoneType)
        {
            this.RI = ri;
            this.task = task;
            this.DA = da;
            this.phoneNumber = phoneNumber;
            this.phoneType = phoneType;
        }

        /// <summary>
        /// Check if we have had this number marked bad in the last year
        /// </summary>
        public bool PhoneNumberIsInvalidInOneLinkWithinPastYear()
        {
            //Hit up LP2J to get the borrower's phone history.
            RI.FastPath(string.Format("LP2JI{0};X;;Y", task.Ssn));
            if (RI.CheckForText(22, 3, "47004"))
                return false;

            //Check whether we got the selection screen or the target screen.
            if (RI.CheckForText(1, 75, "SELECT"))
            {
                //Selection screen. Select all.
                RI.PutText(3, 13, "X", Key.Enter);

                //Look through the historical phone numbers for a match.
                for (int dateRow = 4; dateRow <= 18; dateRow += 5)
                {
                    //Move on to the next page if we're done with the current one.
                    if (!RI.CheckForText(dateRow, 19, "DATE"))
                    {
                        RI.Hit(Key.F8);
                        dateRow = 4;
                    }
                    //Stop looking when we run out of address history.
                    if (RI.CheckForText(22, 3, "46004"))
                        break;

                    //Stop looking once we get a date that's older than a year.
                    int monthColumn = 24;
                    int dayColumn = 26;
                    int yearColumn = 28;

                    try
                    {
                        string date = RI.GetText(dateRow, monthColumn, 2) + RI.GetText(dateRow, dayColumn, 2) + RI.GetText(dateRow, yearColumn, 4);
                        if (date == "MMDDCCYY")
                            break;

                        int addressMonth = int.Parse(RI.GetText(dateRow, monthColumn, 2));
                        int addressDay = int.Parse(RI.GetText(dateRow, dayColumn, 2));
                        int addressYear = int.Parse(RI.GetText(dateRow, yearColumn, 4));
                        DateTime addressDate = new DateTime(addressYear, addressMonth, addressDay);

                        if (DateTime.Now.AddYears(-1) > addressDate)
                            break;

                        //Check for a matching phone number (either HOME or OTHER) with a validity indicator of N.
                        int phoneColumn = 12;
                        int validityColumn = 35;

                        bool invalidatedPrimary = RI.CheckForText(dateRow + 1, phoneColumn, phoneNumber) && RI.CheckForText(dateRow + 1, validityColumn, "N");
                        bool invalidatedAlternate = RI.CheckForText(dateRow + 2, phoneColumn, phoneNumber) && RI.CheckForText(dateRow + 2, validityColumn, "N");
                        bool invalidatedOther = RI.CheckForText(dateRow + 3, phoneColumn, phoneNumber) && RI.CheckForText(dateRow + 3, validityColumn, "N");
                        if (invalidatedPrimary || invalidatedAlternate || invalidatedOther)
                            return true;
                    }
                    catch (FormatException)
                    {
                        continue;
                    }
                }
            }
            else
            {
                //Target screen. Check the one record.
                //AES didn't implement the changes to LP2J when the other OneLINK changes happened,
                //so check which version we're working with. Once they put that in place, we can just go with the new position ("else" block).
                if (RI.CheckForText(8, 2, "HOME"))
                {
                    string date = RI.GetText(4, 24, 2) + RI.GetText(4, 26, 2) + RI.GetText(4, 28, 4);
                    if (date == "MMDDCCYY")
                        return false;

                    int addressMonth = int.Parse(RI.GetText(4, 24, 2));
                    int addressDay = int.Parse(RI.GetText(4, 26, 2));
                    int addressYear = int.Parse(RI.GetText(4, 28, 4));
                    DateTime addressDate = new DateTime(addressYear, addressMonth, addressDay);
                    if (DateTime.Now.AddYears(-1) > addressDate)
                        return false;
                    if ((RI.CheckForText(8, 7, phoneNumber) && RI.CheckForText(8, 38, "N")) || (RI.CheckForText(8, 49, phoneNumber) && RI.CheckForText(8, 80, "N")))
                        return true;
                }
                else
                {
                    string date = RI.GetText(4, 24, 2) + RI.GetText(4, 26, 2) + RI.GetText(4, 28, 4);
                    if (date == "MMDDCCYY")
                        return false;

                    int addressMonth = int.Parse(RI.GetText(4, 24, 2));
                    int addressDay = int.Parse(RI.GetText(4, 26, 2));
                    int addressYear = int.Parse(RI.GetText(4, 28, 4));
                    DateTime addressDate = new DateTime(addressYear, addressMonth, addressDay);
                    if (DateTime.Now.AddYears(-1) > addressDate)
                        return false;
                    if ((RI.CheckForText(6, 10, phoneNumber) && RI.CheckForText(6, 32, "N")) || (RI.CheckForText(6, 52, phoneNumber) && RI.CheckForText(6, 74, "N")))
                        return true;
                }
            }

            //If we didn't return True within the loop, then no matches were found.
            return false;
        }

        /// <summary>
        /// Check our phone history for the new number
        /// </summary>
        public bool CompassCompareHistoryPhone()
        {
            var previousPhones = DA.GetBorrowerRecentPhoneHistory(task.Ssn);
            var matchingPhones = previousPhones.Where(p => p.PhoneNumber == phoneNumber);

            var maxMatchingPhone = matchingPhones.OrderByDescending(o => o.VerificationDate).FirstOrDefault();
            if (maxMatchingPhone == null)
                return false;
            if (maxMatchingPhone.PhoneIsValid)
                return false;
            return true;
        }

        /// <summary>
        /// Update the phone information.
        /// </summary>
        public bool UpdatePhoneNumber(SystemCode systemCode)
        {
            if (phoneType == PhoneType.Home)
                RI.PutText(16, 14, "H", ReflectionInterface.Key.Enter);
            else if (phoneType == PhoneType.Alternate)
                RI.PutText(16, 14, "A", ReflectionInterface.Key.Enter);
            else if (phoneType == PhoneType.Work)
                RI.PutText(16, 14, "W", Key.Enter);

            //remove existing foreign fields
            if (!string.IsNullOrWhiteSpace(RI.GetText(18, 15, 3).Trim('_', ' ')))
                RI.PutText(18, 15, "", true);
            if (!string.IsNullOrWhiteSpace(RI.GetText(18, 24, 5).Trim('_', ' ')))
                RI.PutText(18, 24, "", true);
            if (!string.IsNullOrWhiteSpace(RI.GetText(18, 36, 10).Trim('_', ' ')))
                RI.PutText(18, 36, "", true);

            RI.PutText(16, 20, "U");
            RI.PutText(16, 30, "N");
            RI.PutText(16, 45, task.PendingVerificationDate.Value.ToString("MMddyy"));
            RI.PutText(17, 14, phoneNumber);
            RI.PutText(17, 54, "Y");
            RI.PutText(19, 14, systemCode.CompassSourceCode);
            if (!RI.CheckForText(17, 67, "_") && !RI.CheckForText(17, 67, " "))
                RI.PutText(17, 67, "", true);
            RI.Hit(Key.Enter);
            return RI.MessageCode.IsIn("01097", "01100");
        }
    }
}

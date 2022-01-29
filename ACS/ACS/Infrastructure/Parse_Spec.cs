using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Uheaa.Common;

namespace ACS
{
    // For reference, here's the ACSIN file structure: 
    //
    //      17 = Person type (Person, Reference, or Borrower) 
    //   18-26 = SSN (cyphered using either MYLAUGHTER or Letter Writer encryption) 
    //      31 = Address type (Legal or Alternate) 
    //   44-63 = Last name 
    //   64-90 = First name 
    // 120-129 = Old house number 
    // 130-131 = Old house cardinal direction (N, E, S, W, etc.) 
    // 132-159 = Old street name 
    // 160-163 = Old street suffix (St, Rd, Dr, Ave, etc.) 
    // 164-165 = Old street cardinal direction 
    // 166-169 = Old unit type (Apt, eg.) 
    // 170-179 = Old unit number 
    // 180-207 = Old city 
    // 208-209 = Old state 
    // 210-214 = Old zip code 
    // 244-253 = New house number 
    // 254-255 = New house cardinal direction (N, E, S, W, etc.) 
    // 256-283 = New street name 
    // 284-287 = New street suffix (St, Rd, Dr, Ave, etc.) 
    // 288-289 = New street cardinal direction 
    // 290-293 = New unit type (Apt, eg.) 
    // 294-303 = New unit number 
    // 304-331 = New city 
    // 332-333 = New state 
    // 334-338 = New zip code 

    public class Parse_Spec
    {
        const int ADDRESS_LINE_MAX_LENGTH = 35;
        public AcsOlFileRecord ParseData(string lineData)
        {
            AcsOlFileRecord data = new AcsOlFileRecord();
            data.Valid = true; 

            //if (lineData.SafeSubString(0, 1) != "2" ||
            //   (
            //   lineData.SafeSubString(16, 1) != "P" &&
            //   lineData.SafeSubString(16, 1) != "B" &&
            //   lineData.SafeSubString(16, 1) != "R"
            //   ) ||
            //   lineData.SafeSubString(26, 4).IsNumeric() == false)
            //    return data;

            data.PersonType = lineData.SafeSubString(16, 1)[0];
            data.POAddressDate = lineData.SafeSubString(26, 4);
            if (!data.POAddressDate.IsNumeric() && data.POAddressDate.ToUpper().Contains("O"))
                data.POAddressDate = data.POAddressDate.ToUpper().Replace("O", "0"); // Sometimes the file has an "O" instead of a 0. This fixes that edge case.

            data.SSN = lineData.SafeSubString(17, 9);
            data.AddressType = lineData.SafeSubString(31, 1)[0];
            data.ConcatenatedData.FullName = string.Format("{0}, {1}", lineData.SafeSubString(43, 20).Trim(), lineData.SafeSubString(63, 27).Trim());
            while (data.ConcatenatedData.FullName.Contains("  "))
                data.ConcatenatedData.FullName = data.ConcatenatedData.FullName.Replace("  ", " ");
            data.FirstName = lineData.SafeSubString(63, 27).Trim();
            data.LastName = lineData.SafeSubString(43, 20).Trim();
            //old address 
            GatherAndCalculateOldAddress(lineData, data);
            //new address 
            GatherAndCalculateNewAddress(lineData, data);

            if(data.FirstName.Length == 0 || data.LastName.Length == 0)
                data.Valid = false;

            return data;
        }


        internal static void GatherAndCalculateOldAddress(string lineData, AcsOlFileRecord returnData)
        {
            string oldHouseNum = lineData.SafeSubString(119, 10).Trim();
            string oldHouseDir = lineData.SafeSubString(129, 2).Trim();
            string oldSt = lineData.SafeSubString(131, 28).Trim();
            string oldStSfx = lineData.SafeSubString(159, 4).Trim();
            string oldStDir = lineData.SafeSubString(163, 2).Trim();
            string oldUnit = lineData.SafeSubString(165, 4).Trim();
            string oldUnitNum = lineData.SafeSubString(169, 10).Trim();
            string oldCity = lineData.SafeSubString(179, 28).Trim();
            string oldState = lineData.SafeSubString(207, 2);
            string oldZip = lineData.SafeSubString(209, 5);
            //start address calculation 
            if (oldUnit == "#")
            {
                oldUnit = string.Format("APT {0}", oldUnitNum);
            }
            else if (oldUnit != string.Empty)
            {
                oldUnit = string.Format("{0} {1}", oldUnit, oldUnitNum);
            }
            else
            {
                oldUnit = string.Empty;
            }


            //format 
            if (oldSt == string.Empty)
            {
                oldSt = "PO BOX";
            }
            if (oldSt == "PO BOX" || oldSt == "RR")
            {
                returnData.ConcatenatedData.OldAddress = string.Format("{0} {1} {2} {3} {4} {5}", oldSt, oldHouseNum, oldUnit, oldCity, oldState, oldZip);
            }
            else if (oldHouseDir == string.Empty && oldStSfx != string.Empty && oldStDir == string.Empty)
            {
                returnData.ConcatenatedData.OldAddress = string.Format("{0} {1} {2} {3} {4} {5} {6}", oldHouseNum, oldSt, oldStSfx, oldUnit, oldCity, oldState, oldZip);
            }
            else if (oldHouseDir == string.Empty && oldStDir != string.Empty && oldStSfx == string.Empty)
            {
                returnData.ConcatenatedData.OldAddress = string.Format("{0} {1} {2} {3} {4} {5}", oldHouseNum, oldSt, oldStDir, oldCity, oldState, oldZip);
            }
            else if (oldHouseDir != string.Empty && oldStSfx == string.Empty && oldStDir != string.Empty)
            {
                returnData.ConcatenatedData.OldAddress = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", oldHouseNum, oldHouseDir, oldSt, oldStDir, oldUnit, oldCity, oldState, oldZip);
            }
            else if (oldHouseDir != string.Empty && oldStSfx != string.Empty)
            {
                returnData.ConcatenatedData.OldAddress = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", oldHouseNum, oldHouseDir, oldSt, oldStSfx, oldUnit, oldCity, oldState, oldZip);
            }
            else
            {
                returnData.ConcatenatedData.OldAddress = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8}", oldHouseNum, oldHouseDir, oldSt, oldStSfx, oldStDir, oldUnit, oldCity, oldState, oldZip);
            }
        }



        internal static void GatherAndCalculateNewAddress(string lineData, AcsOlFileRecord returnData)
        {
            string newHouseNum = lineData.SafeSubString(243, 10).Trim();
            string newHouseDir = lineData.SafeSubString(253, 2).Trim();
            string newSt = lineData.SafeSubString(255, 28).Trim();
            string newStSfx = lineData.SafeSubString(283, 4).Trim();
            string newStDir = lineData.SafeSubString(287, 2).Trim();
            string newUnit = lineData.SafeSubString(289, 4).Trim();
            string newUnitNum = lineData.SafeSubString(293, 10).Trim();
            returnData.NewAddress.City = lineData.SafeSubString(303, 28).Trim();
            returnData.NewAddress.State = lineData.SafeSubString(331, 2);
            returnData.NewAddress.Zip = string.Format("{0}{1}", lineData.SafeSubString(333, 5), lineData.SafeSubString(339, 4).Trim());
            //start address calculation 
            if (newUnit == "#")
            {
                newUnit = string.Format("APT {0}", newUnitNum);
            }
            returnData.NewAddress.Addr2 = string.Format("{0} {1}", newUnit, newUnitNum).Trim();
            //format 
            if (newSt == string.Empty)
            {
                newSt = "PO BOX";
            }
            if (newSt == "PO BOX" || newSt == "RR")
            {
                returnData.NewAddress.Addr1 = string.Format("{0} {1}", newSt, newHouseNum);
            }
            else if (newHouseDir == string.Empty && newStSfx != string.Empty && newStDir == string.Empty)
            {
                returnData.NewAddress.Addr1 = string.Format("{0} {1} {2}", newHouseNum, newSt, newStSfx);
            }
            else if (newHouseDir == string.Empty && newStDir != string.Empty && newStSfx == string.Empty)
            {
                returnData.NewAddress.Addr1 = string.Format("{0} {1} {2}", newHouseNum, newSt, newStDir);
            }
            else if (newHouseDir != string.Empty && newStSfx == string.Empty && newStDir != string.Empty)
            {
                returnData.NewAddress.Addr1 = string.Format("{0} {1} {2} {3}", newHouseNum, newHouseDir, newSt, newStDir);
            }
            else if (newHouseDir != string.Empty && newStSfx != string.Empty)
            {
                returnData.NewAddress.Addr1 = string.Format("{0} {1} {2} {3}", newHouseNum, newHouseDir, newSt, newStSfx);
            }
            else
            {
                returnData.NewAddress.Addr1 = string.Format("{0} {1} {2} {3} {4}", newHouseNum, newHouseDir, newSt, newStSfx, newStDir);
            }
            //concatenate addr1 and addr2 if < 35 
            if ((returnData.NewAddress.Addr1.Length + returnData.NewAddress.Addr2.Length) < ADDRESS_LINE_MAX_LENGTH && returnData.NewAddress.Addr2 != string.Empty)
            {
                returnData.NewAddress.Addr1 = string.Format("{0} {1}", returnData.NewAddress.Addr1.Trim(), returnData.NewAddress.Addr2);
                returnData.NewAddress.Addr2 = string.Empty;
            }
            returnData.ConcatenatedData.NewAddress = string.Format("{0}, {1} {2} {3}", string.Format("{0} {1}", returnData.NewAddress.Addr1.Trim(), returnData.NewAddress.Addr2).Trim(), returnData.NewAddress.City, returnData.NewAddress.State, returnData.NewAddress.Zip);
        }

        public double ThrowsExceptionIfZeroDenominator(double a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException();
            return a / b;
        }

    }
}

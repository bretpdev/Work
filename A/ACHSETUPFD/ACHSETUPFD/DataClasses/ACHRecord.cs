using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common;

namespace ACHSETUPFD
{
    public class ACHRecord
    {

        public enum BankAccountType
        {
            Checking,
            Savings
        }

		public enum EFTSource
		{
			Paper
		}

        public enum EndorserStatus
        {
            Yes,
            No,
            NA
        }

        public string SSN { get; set; }
        public EndorserStatus IsEndorser { get; set; }
        public string ABARoutingNumber { get; set; }
        public string BankAccountNumber { get; set; }
        public BankAccountType AccountType { get; set; }
        public string AdditionalWithdrawalAmount { get; set; }
        public bool FormSigned { get; set; }
        public List<int> Loans { get; set; }
		public EFTSource EFT { get; set; }

        public ACHRecord()
        {
            SSN = string.Empty;
            ABARoutingNumber = string.Empty;
            BankAccountNumber = string.Empty;
            AdditionalWithdrawalAmount = string.Empty;
            Loans = new List<int>();
        }

        /// <summary>
        /// Parses user provided loan list data into individual list items.  Returns true if successful otherwise false.
        /// </summary>
        public bool ParseLoanSequences(string userProvidedListData)
        {
            Loans = new List<int>(); //blank list

            List<string> tempLoans;

            //remove spaces 
            userProvidedListData = userProvidedListData.Replace(" ",string.Empty);
            //split out at comma delimiter
            tempLoans = new List<string>(userProvidedListData.Split(','));
            //work through list and add to the Loans list in the object
            foreach (string entry in tempLoans)
            {
                //check if entry has a "-" if it does then handle the number run else just move it to the Loans list
                if (entry.Contains("-"))
                {
                    List<string> tempLoansRun = new List<string>(entry.Split('-'));
                    //if there is more or less than two entries then the user entry wasn't in a valid format.
                    if (tempLoansRun.Count != 2)
                    {
                        return false;
                    }
                    else
                    {
                        //check if both entries are numeric
                        if (!tempLoansRun[0].IsNumeric() || !tempLoansRun[1].IsNumeric())
                        {
                            return false;
                        }
                        else
                        {
                            //if both entries are numeric calculate all the numbers between the two numbers
                            int beginning = int.Parse(tempLoansRun[0]);
                            int end = int.Parse(tempLoansRun[1]);
                            int counter = beginning;
                            Loans.Add(beginning);
                            while (counter != end)
                            {
                                counter++;
                                Loans.Add(counter);
                            }
                        }
                    }
                }
                else
                {
                    if (entry.IsNumeric())
                    {
                        Loans.Add(int.Parse(entry));
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Gets the account type string code
        /// </summary>
        public string GetAccountTypeAsString()
        {
            if (AccountType == BankAccountType.Checking)
            {
                return "C";
            }
            else
            {
                return "S";
            }
        }

        /// <summary>
        /// Returns the Paper EFT type
        /// </summary>
		public string GetEftAsString()
		{
            return "PPD";
		}

    }
}

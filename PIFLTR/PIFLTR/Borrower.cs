using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.DocumentProcessing;

namespace PIFLTR
{
    public class Borrower
    {
        public string AccountNumber { get; set; }
        public string BorrowerSsn { get; set; }
        public string BorrowerFirstName { get; set; }
        public string BorrowerLastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string DomesticState { get; set; }
        public string ZIPCode { get; set; }
        public string ForeignCountry { get; set; }
        public string ForeignState { get; set; }
        public string CostCenter { get; set; } //TODO: Find out why borrower costcenter wrong in DB but not coborrower cost center
        public StringBuilder LetterData { get; set; }
        public DateTime PifEffectiveDate { get; set; }
        public StringBuilder ConsolPifLetterData { get; set; }
        public DateTime ConsolPifEffectiveDate { get; set; }
        public List<TaskData> Tasks { get; set; }

        public Borrower()
        {
            Tasks = new List<TaskData>();
        }

        /// <summary>
        /// Retrieves consol tasks that aren't cancelled and haven't already been processed.
        /// </summary>
        /// <returns></returns>
        public List<TaskData> GetConsolLoans()
        {
            List<TaskData> result = new List<TaskData>();
            foreach (TaskData task in Tasks)
            {
                if (task.IsConsolPif)
                {
                    result.Add(task);
                    CostCenter = task.CostCode; //TODO: Figure out why CostCenter is being cut off.
                }
            }
            return result;
        }

        /// <summary>
        /// Retrieves non-consol tasks that aren't cancelled and haven't already been processed.
        /// </summary>
        /// <returns>result - a List of PIF tasks</returns>
        public List<TaskData> GetPifLoans()
        {
            List<TaskData> result = new List<TaskData>();
            foreach (TaskData task in Tasks)
            {
                if (!task.IsConsolPif)
                {
                    result.Add(task);
                    CostCenter = task.CostCode;
                }
            }
            return result;
        }

        /// <summary>
        /// Sets ConsolPiffectiveDate and/or PifEffectiveDate for a task.
        /// </summary>
        /// <returns>boolean indicating whether a date was set</returns>
        public bool SetEffectiveDate()
        {
            List<TaskData> borrowerConsolTasks = GetConsolLoans();
            List<TaskData> borrowerPifTasks = GetPifLoans();
            DateTime effectiveDate = new DateTime();

            // If task is for a consol PIF, set ConsolPifEffectiveDate
            if (borrowerConsolTasks.Count > 0)
            {
                foreach (TaskData task in borrowerConsolTasks)
                {
                    DateTime tempEffectiveDate;
                    DateTime.TryParse(task.EffectiveDate, out tempEffectiveDate);
                    if (tempEffectiveDate > effectiveDate)
                        effectiveDate = tempEffectiveDate;
                }
                ConsolPifEffectiveDate = effectiveDate;
            }

            // If task is for a non-consolidation PIF, set PifEffectiveDate
            else if (borrowerPifTasks.Count > 0)
            {
                effectiveDate = new DateTime();
                foreach (TaskData task in borrowerPifTasks)
                {
                    DateTime tempEffectiveDate;
                    DateTime.TryParse(task.EffectiveDate, out tempEffectiveDate);
                    if (tempEffectiveDate > effectiveDate)
                        effectiveDate = tempEffectiveDate;
                }
                PifEffectiveDate = effectiveDate;
            }
            if (ConsolPifEffectiveDate != null || PifEffectiveDate != null) // Return true if date was set properly
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets and sets LetterData that will later be inserted into the PrintProcessing table.
        /// </summary>
        /// <returns></returns>
        public void GetLetterData()
        {
            List<TaskData> borrowerConsolTasks = GetConsolLoans();
            List<TaskData> borrowerPifTasks = GetPifLoans();
            string[] letterDataArray = new string[115];
            SetEffectiveDate();
            string costCode = "";
            string forInd; //foreign (address) indicator
            if (ForeignCountry == "")
                forInd = DomesticState;
            else
                forInd = "FC";
            string keyLine = DocumentProcessing.ACSKeyLine(BorrowerSsn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);

            letterDataArray[0] = BorrowerSsn.Substring(0, 3) + "-" + BorrowerSsn.Substring(3, 2) + '-' + BorrowerSsn.Substring(5, 4);
            letterDataArray[1] = BorrowerLastName;
            letterDataArray[2] = BorrowerFirstName;
            letterDataArray[3] = Address1;
            letterDataArray[4] = Address2;
            letterDataArray[5] = City;
            letterDataArray[6] = DomesticState + ForeignState;
            letterDataArray[7] = ZIPCode;
            letterDataArray[8] = ForeignCountry;
            letterDataArray[10] = AccountNumber;
            letterDataArray[111] = DateTime.Today.ToString("MM/dd/yyyy");
            letterDataArray[112] = keyLine;
            letterDataArray[113] = forInd;
            int index = 11;

            if (borrowerConsolTasks.Count > 0)
                AddConsolLoanData(borrowerConsolTasks, letterDataArray, ref costCode, ref index);

            if (borrowerPifTasks.Count > 0)
                AddNonConsolLoanData(borrowerPifTasks, letterDataArray, ref costCode, ref index);
        }

        /// <summary>
        /// Adds all the loan level data for non cosolidation loans to the letter data array.
        /// </summary>
        private void AddNonConsolLoanData(List<TaskData> borrowerPifTasks, string[] letterDataArray, ref string costCode, ref int index)
        {
            for (int i = 11; i <= 110; i++)
                letterDataArray[i] = "";

            letterDataArray[9] = PifEffectiveDate.ToString("MMMM dd, yyyy");
            int maxIndexForLoanData = 110;

            foreach (TaskData task in borrowerPifTasks.OrderBy(p => p.LoanSeq))
            {
                costCode = task.CostCode;
                DateTime tempFirstDisbursementDate = Convert.ToDateTime(task.FirstDisbursementDate);
                if (index >= maxIndexForLoanData)
                    break;
                letterDataArray[index++] = null;
                letterDataArray[index++] = task.LoanProgram;
                letterDataArray[index++] = tempFirstDisbursementDate.ToString("MM/dd/yyyy");
                letterDataArray[index++] = task.OriginalBalance;
                letterDataArray[index++] = task.LoanSeq.ToString();
            }
            letterDataArray[114] = costCode;
            LetterData = CreateLetterData(letterDataArray);
        }

        /// <summary>
        /// Adds all the loan level data for cosolidation loans to the letter data array.
        /// </summary>
        private void AddConsolLoanData(List<TaskData> borrowerConsolTasks, string[] letterDataArray, ref string costCode, ref int index)
        {
            letterDataArray[9] = ConsolPifEffectiveDate.ToString("MMMM dd, yyyy");
            int maxIndexForLoanData = 110;

            // Loop through each consol task for borrower, setting loan sequence-specific fields for LetterData
            foreach (TaskData task in borrowerConsolTasks.OrderBy(p => p.LoanSeq))
            {
                costCode = task.CostCode;
                DateTime tempFirstDisbursementDate = Convert.ToDateTime(task.FirstDisbursementDate);
                if (index >= maxIndexForLoanData)
                    break;
                letterDataArray[index++] = null; //Leaving CLID null, as it is unneedeed
                letterDataArray[index++] = task.LoanProgram;
                letterDataArray[index++] = tempFirstDisbursementDate.ToString("MM/dd/yyyy");
                letterDataArray[index++] = task.OriginalBalance;
                letterDataArray[index++] = task.LoanSeq.ToString(); //110 is max index for this field
            }
            letterDataArray[114] = costCode;
            ConsolPifLetterData = CreateLetterData(letterDataArray);
        }

        /// <summary>
        /// Builds StringBuilder from letterDataArray, wrapping each string with quotation marks.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>letterData - a StringBuilder that will be inserted into PrintProcessing</returns>
        public StringBuilder CreateLetterData(string[] data)
        {
            StringBuilder letterData = new StringBuilder();
            foreach (string str in data)
            {
                letterData.Append("\"" + str + "\",");
            }
            return letterData.Remove(letterData.Length - 1, 1);
        }
    }
}


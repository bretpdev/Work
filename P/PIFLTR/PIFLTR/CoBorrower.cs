using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.DocumentProcessing;

namespace PIFLTR
{
    public class CoBorrower
    {
        public string BorrowerAccountNumber { get; set; }
        public string BorrowerSsn { get; set; }
        public string CoBorrowerSsn { get; set; }
        public string CoBorrowerAccount { get; set; }
        public string CoBorrowerFirstName { get; set; }
        public string CoBorrowerLastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string DomesticState { get; set; }
        public string ZIPCode { get; set; }
        public string ForeignCountry { get; set; }
        public string ForeignState { get; set; }
        public string CostCenter { get; set; }
        public StringBuilder PifLetterData { get; set; }
        public StringBuilder ConsolLetterData { get; set; }
        public List<TaskData> Tasks { get; set; }

        public CoBorrower()
        {
            Tasks = new List<TaskData>();
        }

        /// <summary>
        /// Gathers all of the non-consol PIF tasks/loan sequences that aren't canceled and aren't processed.
        /// </summary>
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
        /// Gathers all of the consol PIF tasks/loan sequences that aren't canceled and aren't processed.
        /// </summary>
        public List<TaskData> GetConsolLoans()
        {
            List<TaskData> result = new List<TaskData>();
            foreach (TaskData task in Tasks)
            {
                if (task.IsConsolPif)
                {
                    result.Add(task);
                    CostCenter = task.CostCode;
                }
            }
            return result;
        }

        /// <summary>
        /// Set the effectiveDate to the max effectiveDate.
        /// </summary>
        public DateTime GetEffectiveDate(List<TaskData> tasks)
        {
            DateTime effectiveDate = new DateTime();

            if (tasks.Count > 0)
            {
                foreach (TaskData task in tasks)
                {
                    DateTime tempEffectiveDate;
                    DateTime.TryParse(task.EffectiveDate, out tempEffectiveDate);
                    if (tempEffectiveDate > effectiveDate)
                        effectiveDate = tempEffectiveDate;
                }
                return effectiveDate;
            }
            return effectiveDate;
        }

        /// <summary>
        /// Get and set letterData to pass to PrintProcessing.
        /// </summary>
        /// <returns></returns>
        public void GetLetterData()
        {
            List<TaskData> coBorrowerConsolTasks = GetConsolLoans();
            List<TaskData> coBorrowerPifTasks = GetPifLoans();
            DateTime ConsolEffectiveDate = GetEffectiveDate(coBorrowerConsolTasks);
            DateTime PifEffectiveDate = GetEffectiveDate(coBorrowerPifTasks);

            string[] letterDataArray = new string[115];
            string costCode = "";
            string forInd;
            if (ForeignCountry == "")
                forInd = DomesticState;
            else
                forInd = "FC";
            string keyLine = DocumentProcessing.ACSKeyLine(CoBorrowerSsn, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);

            letterDataArray[0] = CoBorrowerSsn.Substring(0, 3) + "-" + CoBorrowerSsn.Substring(3, 2) + '-' + CoBorrowerSsn.Substring(5, 4);
            letterDataArray[1] = CoBorrowerLastName;
            letterDataArray[2] = CoBorrowerFirstName;
            letterDataArray[3] = Address1;
            letterDataArray[4] = Address2;
            letterDataArray[5] = City;
            letterDataArray[6] = DomesticState + ForeignState;
            letterDataArray[7] = ZIPCode;
            letterDataArray[8] = ForeignCountry;
            letterDataArray[10] = BorrowerAccountNumber;
            letterDataArray[111] = DateTime.Today.ToString("MM/dd/yyyy");
            letterDataArray[112] = keyLine;
            letterDataArray[113] = forInd;
            int index = 11;

            if (coBorrowerConsolTasks.Count > 0)
            {
                AddConsolLoanData(coBorrowerConsolTasks, ConsolEffectiveDate, letterDataArray, ref costCode, ref index);
            }
            else if (coBorrowerPifTasks.Count > 0)
            {
                AddNonConsolLoanData(coBorrowerPifTasks, PifEffectiveDate, letterDataArray, ref costCode, ref index);
            }
        }

        /// <summary>
        /// Adds all the loan level data for the non-consolidation loans to the letter data array.
        /// </summary>
        private void AddNonConsolLoanData(List<TaskData> borrowerPifTasks, DateTime PifEffectiveDate, string[] letterDataArray, ref string costCode, ref int index)
        {
            for (int j = 11; j <= 110; j++)
                letterDataArray[j] = "";
            letterDataArray[9] = PifEffectiveDate.ToString("MMMM dd, yyyy");
            int maxIndexForLoanData = 110;

            foreach (TaskData task in borrowerPifTasks.OrderBy(p => p.LoanSeq))
            {
                costCode = task.CostCode;
                DateTime tempGuranteeDate = Convert.ToDateTime(task.FirstDisbursementDate);
                if (index >= maxIndexForLoanData)
                    break;
                letterDataArray[index++] = null;
                letterDataArray[index++] = task.LoanProgram;
                letterDataArray[index++] = tempGuranteeDate.ToString("MM/dd/yyyy"); //CONVERT DATE
                letterDataArray[index++] = task.OriginalBalance;
                letterDataArray[index++] = task.LoanSeq.ToString();
            }
            letterDataArray[114] = costCode;
            PifLetterData = CreateLetterData(letterDataArray);
        }

        /// <summary>
        /// Adds all the loan level data for the consolidation loans to the letter data array.
        /// </summary>
        private void AddConsolLoanData(List<TaskData> borrowerConsolTasks, DateTime ConsolEffectiveDate, string[] letterDataArray, ref string costCode, ref int index)
        {
            for (int j = 11; j <= 110; j++)
                letterDataArray[j] = "";
            letterDataArray[9] = ConsolEffectiveDate.ToString("MMMM dd, yyyy");
            int maxIndexForLoanData = 110;

            foreach (TaskData task in borrowerConsolTasks.OrderBy(p => p.LoanSeq))
            {
                costCode = task.CostCode;
                DateTime tempGuranteeDate = Convert.ToDateTime(task.FirstDisbursementDate);
                if (index >= maxIndexForLoanData)
                    break;
                letterDataArray[index++] = null;
                letterDataArray[index++] = task.LoanProgram;
                letterDataArray[index++] = tempGuranteeDate.ToString("MM/dd/yyyy");
                letterDataArray[index++] = task.OriginalBalance;
                letterDataArray[index++] = task.LoanSeq.ToString();
            }
            letterDataArray[114] = costCode;
            ConsolLetterData = CreateLetterData(letterDataArray);
        }

        /// <summary>
        /// Method constructs letter data through a string builder and wraps strings with quotation marks.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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




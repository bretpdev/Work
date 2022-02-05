using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;

namespace UHECORPRT
{
    partial class BatchPrinting
    {
        /// <summary>
        /// Loads a dictionary into memory from [print].HeaderReplacementCoBorrower to do leterdata column replacement
        /// </summary>
        public Dictionary<string, string> GetInternalHeaderNamesHelper()
        {
            Dictionary<string, string> internalNames = new Dictionary<string, string>();
            System.Data.DataTable nameMap = DA.GetInternalNames();
            foreach (DataRow r in nameMap.Rows)
            {
                string headerName = null;
                string internalName = null;
                foreach (DataColumn c in nameMap.Columns)
                {
                    if (c.ColumnName == "FileHeader" && c.DataType.Equals(typeof(string)))
                    {
                        headerName = (string)r[c];
                    }
                    else if (c.ColumnName == "InternalName" && c.DataType.Equals(typeof(string)))
                    {
                        internalName = (string)r[c];
                    }
                }
                if (headerName != null && internalName != null)
                {
                    internalNames.Add(headerName, internalName);
                }
                else
                {
                    Program.PL.AddNotification("Error when setting up internal name mapping from [print].HeaderReplacementCoBorrower. Missing column.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                    throw new MissingMemberException("Error when setting up internal name mapping from [print].HeaderReplacementCoBorrower. Missing column.");
                }
            }

            return internalNames;
        }

        public string DoLetterDataReplace(string letterData, CoBorrowerInformation cb, ScriptData file, Dictionary<string, string> internalNames)
        {
            List<string> header = file.FileHeaderConst.SplitAndRemoveQuotes(",");
            List<string> data = letterData.SplitAndPreserveQuotes(",");
            if (header.Count != data.Count)
            {
                Program.PL.AddNotification("The column count of the header and data lines did not match. File Header: " + file.FileHeader + "Data: " + letterData, NotificationType.FileFormatProblem, NotificationSeverityType.Critical);
                throw new FormatException("The column count of the header and data lines did not match. File Header: " + file.FileHeader + "Data: " + letterData);
            }
            for (int i = 0; i < header.Count; i++)
            {
                if (internalNames.ContainsKey(header[i]))
                {
                    //Uses the HeaderReplacementCoBorrower table to determine fields that need to be replaced
                    switch (internalNames[header[i]])
                    {
                        case "ACCOUNTNUMBER":
                            //We want to keep the borrower's account number
                            //data[i] = cb.AccountNumber.ToString();
                            break;
                        case "FIRSTNAME":
                            data[i] = cb.FirstName.TrimRight(" ");
                            break;
                        case "MIDDLEINITIAL":
                            if (cb.MiddleName.Length > 0)
                            {
                                data[i] = cb.MiddleName[0].ToString();
                            }
                            else
                            {
                                data[i] = "";
                            }
                            break;
                        case "LASTNAME":
                            data[i] = cb.LastName.TrimRight(" ");
                            break;
                        case "FIRSTLASTNAME":
                            data[i] = cb.FirstName.TrimRight(" ") + " " + cb.LastName.TrimRight(" ");
                            break;
                        case "SSN":
                            data[i] = DocumentProcessing.ACSKeyLine(cb.CoBorrowerSSN, DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.ACSKeyLineAddressType.Legal);
                            break;
                        case "ADDRESS1":
                            data[i] = cb.Address1.TrimRight(" ");
                            break;
                        case "ADDRESS2":
                            data[i] = cb.Address2.TrimRight(" ");
                            break;
                        case "ADDRESS3":
                            data[i] = cb.Address3.TrimRight(" ");
                            break;
                        case "CITY":
                            data[i] = cb.City.TrimRight(" ");
                            break;
                        case "STATE":
                            data[i] = cb.State.TrimRight(" ");
                            break;
                        case "ZIP":
                            data[i] = cb.Zip.TrimRight(" ");
                            break;
                        case "FOREIGNCOUNTRY":
                            data[i] = cb.ForeignCountry.TrimRight(" ");
                            break;
                        case "FOREIGNSTATE":
                            data[i] = cb.ForeignState.TrimRight(" ");
                            break;
                        default:
                            Program.PL.AddNotification("Internal mapping from [print].HeaderReplacementCoBorrower contained key without an enumerated value.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
                            throw new FormatException("Internal mapping from [print].HeaderReplacementCoBorrower contained key without an enumerated value.");
                    }
                }
            }
            StringBuilder ret = new StringBuilder();
            foreach (string str in data)
            {
                ret.Append(str + ",");
            }
            ret.Remove(ret.Length - 1, 1);
            return ret.ToString();

        }

        private void InsertCoBorrowerInformation(List<CoBorrowerInformation> cbi, ScriptData file, PrintProcessingData data, Dictionary<string, string> internalNames)
        {
            foreach (CoBorrowerInformation cb in cbi)
            {
                string letterData = DoLetterDataReplace(data.LetterDataConst, cb, file, internalNames);
                //The SQL eliminates any CoBorrowers without a valid address and not on Ecorr
                DA.InsertPrintProcessingRecordCoBwr(file.ScriptID, file.Letter, letterData, cb.AccountNumber, data.CostCenter, data.BF_SSN);
            }
        }

        private void LoadAdditionalCoBorrowerFileData(ScriptData file)
        {
            Console.WriteLine("Getting Additional CoBorrower Data Needed.");
            file.LetterDataForCoBorrowers = DA.PopulatePrintProcessingCoBorrowerData(file.ScriptDataId);
        }
    }
}

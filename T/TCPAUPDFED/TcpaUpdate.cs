using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace TCPAUPDFED
{
    public class TcpaUpdate : BatchScript
    {
        public const string EOJ_Total = "Total number of records in the file";
        public const string EOJ_TotalProcess = "Total number of records processed";
        public const string ERR_ErrorProcessing = "There was an error processing a record";
        public const string ERR_BorrowerNotFound = "Borrower not found on sysytem";
        public static List<string> EOJ_Fields = new List<string>() { EOJ_Total, EOJ_TotalProcess, ERR_ErrorProcessing, ERR_BorrowerNotFound };

        public TcpaUpdate(ReflectionInterface ri)
            : base(ri, "TCPAUPDFED", "ERR_BU35", "EOJ_BU35", EOJ_Fields, Uheaa.Common.DataAccess.DataAccessHelper.Region.CornerStone)
        {
        }

        public override void Main()
        {
            OpenFileDialog dialog = CheckOpenDialog.ShowDialog();
            if (dialog == null)
                EndDllScript();

            string fileName = dialog.FileName;

            List<BorrowerData> data = LoadData(fileName);

            ProcessFile(data);

            File.Delete(fileName);

            if (Err.HasErrors)
                ProcessingComplete("Processing Complete with errors. Please check the error record");
            else
                ProcessingComplete();
        }

        /// <summary>
        /// Reads in a file and loads a new BorrowerData object
        /// </summary>
        /// <param name="fileName">The file to read in</param>
        /// <returns>List of BorrowerData objects</returns>
        private List<BorrowerData> LoadData(string fileName)
        {
            List<BorrowerData> data = new List<BorrowerData>();

            using (StreamReader sr = new StreamReader(fileName))
            {
                while (!sr.EndOfStream)
                {
                    BorrowerData bData = new BorrowerData();
                    string line = sr.ReadLine();
                    if (!line.IsNullOrEmpty())
                    {
                        string[] splitLine = line.SplitAndRemoveQuotes(",").ToArray();
                        bData.AccountNumber = splitLine[0];
                        bData.PhoneNumber = splitLine[1];
                        data.Add(bData);
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// Accesses CTX1J and changes the phone consent back to Y where the phone number is found
        /// </summary>
        /// <param name="data">List of BorrowerData objects</param>
        private void ProcessFile(List<BorrowerData> data)
        {
            int recStep = 0;
            int processed = 0;
            string recPhoneType = "";
            if (!Recovery.RecoveryValue.IsNullOrEmpty())
            {
                List<string> rec = Recovery.RecoveryValue.SplitAndRemoveQuotes(",");
                recStep = int.Parse(rec[0]);
                recPhoneType = rec.Count() > 1 ? rec[1] : "";
            }

            string[] phoneType = { "H", "A", "W", "M" };

            foreach (BorrowerData bor in data.Skip(recStep))
            {
                Eoj.Counts[EOJ_Total].Increment();

                FastPath("TX3ZCTX1J;" + bor.AccountNumber);
                if (!CheckForText(1, 71, "TXX1R"))
                {
                    Err.AddRecord(ERR_BorrowerNotFound, bor);
                    Eoj.Counts[ERR_BorrowerNotFound].Increment();
                    Recovery.RecoveryValue = (processed++).ToString();
                    continue;
                }

                //Hit F6 three times to get to the phone indicator
                Hit(ReflectionInterface.Key.F6);
                Hit(ReflectionInterface.Key.F6);
                Hit(ReflectionInterface.Key.F6);

                bool didFind = false;
                for (int i = 0; i < phoneType.Count(); i++)
                {
                    if (recPhoneType != "" && recPhoneType == phoneType[i])
                        continue;

                    PutText(16, 14, phoneType[i], ReflectionInterface.Key.Enter);
                    string phone = GetText(17, 14, 3) + GetText(17, 23, 3) + GetText(17, 31, 4);
                    if (phone != "" && phone == bor.PhoneNumber)
                    {
                        //Update the phone information
                        PutText(16, 20, "M");
                        PutText(16, 30, "Y");
                        PutText(16, 45, DateTime.Now.ToString("MMddyy"));
                        PutText(17, 54, "Y");
                        PutText(19, 14, "59", ReflectionInterface.Key.Enter);

                        if (RI.MessageCode != "01097")
                        {
                            bor.PhoneType = phoneType[i];
                            Err.AddRecord(ERR_ErrorProcessing, bor);
                            Eoj.Counts[ERR_ErrorProcessing].Increment();
                            Recovery.RecoveryValue = (processed++).ToString();
                            continue;
                        }

                        didFind = true;
                        Recovery.RecoveryValue = processed.ToString() + "," + phoneType[i];
                    }
                }

                if (!didFind)
                {
                    bor.PhoneType = string.Join(",", phoneType);
                    Err.AddRecord(ERR_ErrorProcessing, bor);
                    Eoj.Counts[ERR_ErrorProcessing].Increment();
                }
                else
                    Eoj.Counts[EOJ_TotalProcess].Increment();
                

                Recovery.RecoveryValue = (processed++).ToString();
            }
        }
    }
}

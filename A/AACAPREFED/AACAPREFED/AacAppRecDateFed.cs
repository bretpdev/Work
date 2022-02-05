using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AacBase;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace AACAPREFED
{
    public class AacAppRecDateFed : FedAac
    {
        private static readonly string[] EojFields = { EojTotalsFromSas, EojProcessed, EojError };

        public AacAppRecDateFed(ReflectionInterface ri)
            : base(ri, "AACAPREFED", "ERR_BU01", "EOJ_BU01", EojFields)
        {

        }

        public override void Main()
        {
            string fileToProcess = ChooseTheFileToProcess();
            List<SasFileData> fileData = LoadTheFile(fileToProcess);
            int recoveryIndex = Recovery.RecoveryValue.IsNullOrEmpty() ? 0 : int.Parse(Recovery.RecoveryValue);

            string lastMajorbatch = string.Empty;
            string lastMinorbatch = string.Empty;
            bool firstRun = true;
            bool newBatch = true;
            foreach (SasFileData fileLine in fileData.Skip(recoveryIndex))
            {
                if (!lastMajorbatch.Contains(fileLine.MajorBatch) && !firstRun)
                {
                    Unassign();
                    newBatch = true;
                }
                else if (!lastMinorbatch.Contains(fileLine.MinorBatch) && !firstRun)
                {
                    Unassign();
                    newBatch = true;
                }

                firstRun = false;

                if (ProcessTa3x(fileLine, newBatch))
                {
                    Eoj.Counts[EojProcessed].Increment();
                    newBatch = false;
                }
                else
                {
                    Eoj.Counts[EojError].Increment();
                    newBatch = true;
                }

                recoveryIndex++;
                Recovery.RecoveryValue = recoveryIndex.ToString();
                lastMajorbatch = fileLine.MajorBatch;
                lastMinorbatch = fileLine.MinorBatch;
            }

            File.Delete(fileToProcess);
            ProcessingComplete();
        }

        protected override void EnterData(AacBase.SasFileData data)
        {
            PutText(9, 16, data.AppRecDate, ReflectionInterface.Key.Enter);
        }

        private List<SasFileData> LoadTheFile(string fileName)
        {
            List<SasFileData> fileData = new List<SasFileData>();
            using (StreamReader sr = new StreamReader(fileName))
            {
                sr.ReadLine();//Read in the header.

                while (!sr.EndOfStream)
                {
                    List<string> temp = sr.ReadLine().SplitAndRemoveQuotes(",");
                    fileData.Add(new SasFileData()
                    {
                        AccountNumber = GetDemographicsFromTx1j(temp[0]).AccountNumber,
                        Ssn = temp[0],
                        LoanSeq = temp[4],
                        MajorBatch = temp[26],
                        MinorBatch = temp[27],
                        AppRecDate = DateTime.Parse(temp[29].IsNullOrEmpty() ? "01/01/1990" : temp[29]).ToString("MMddyy")
                    });
                    Eoj.Counts[EojTotalsFromSas].Increment();
                }
            }

            return fileData;
        }
    }
}

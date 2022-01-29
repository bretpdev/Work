using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AacBase;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace AAC2898FED
{
    public class AutomationErrorFed : FedAac
    {
        private static readonly string[] EojFields = { EojTotalsFromSas, EojProcessed, EojError };

        public AutomationErrorFed(ReflectionInterface ri)
            : base(ri, "AAC2898FED", "ERR_BU35", "EOJ_BU35", EojFields)
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

                if (ProcessTa07(fileLine, newBatch))
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

            ProcessingComplete();
        }

        protected override void EnterData(SasFileData data)
        {
            List<string> types = new List<string>() { "CP", "CQ", "C1", "C2", "C3", "IB", "IL" };

            if (types.Contains(data.RepaymentScheduleType))
                PutText(19, 60, data.FirstTermGrad, ReflectionInterface.Key.Enter, true);
            else
                PutText(19, 19, data.FirstTermGrad, ReflectionInterface.Key.Enter, true);


            Eoj.Counts[EojTotalsFromSas].Increment();
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
                        RepaymentScheduleType = temp[15],
                        MajorBatch = temp[26],
                        MinorBatch = temp[27],
                        FirstTermGrad = temp[32]
                    });
                }
            }

            return fileData;
        }
    }
}

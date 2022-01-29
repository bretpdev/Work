using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AacBase;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace AACATO4012
{
    public class AacAutomationError40122 : FedAac
    {
        private static readonly string[] EojFields = { EojTotalsFromSas, EojProcessed, EojError };
        public AacAutomationError40122(ReflectionInterface ri)
            : base(ri, "AACATO4012", "ERR_BU01", "EOJ_BU01", EojFields)
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
            Hit(ReflectionInterface.Key.F8);
            Hit(ReflectionInterface.Key.F8);

            PutText(12, 10, data.InterestRate,true);
            PutText(12, 19, data.InterestType, ReflectionInterface.Key.Enter, true);
            Hit(ReflectionInterface.Key.F10);
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
                        LoanSeq = temp[1],
                        MajorBatch = temp[2],
                        MinorBatch = temp[3],
                        InterestRate = temp[4],
                        InterestType = temp[5]
                    });

                    if (Recovery.RecoveryValue.IsNullOrEmpty())
                        Eoj.Counts[EojTotalsFromSas].Increment();
                }
            }

            return fileData;
        }

    }
}

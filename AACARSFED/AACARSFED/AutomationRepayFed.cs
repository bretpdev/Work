using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AacBase;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace AACARSFED
{
    public class AutomationRepayFed : FedAac
    {
        private static readonly string[] EojFields = { EojTotalsFromSas, EojProcessed, EojError };

        public AutomationRepayFed(ReflectionInterface ri)
            : base(ri, "AACARSFED", "ERR_BU35", "EOJ_BU35", EojFields)
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
                    Eoj.Counts[EojTotalsFromSas].Increment();
                }
                else
                {
                    Eoj.Counts[EojError].Increment();
                    newBatch = true;
                    Eoj.Counts[EojTotalsFromSas].Increment();
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
            PutText(10, 13, data.DateSent, true);
            PutText(10, 71, data.DateBegin, true);
            PutText(19, 19, data.FirstTermGrad, ReflectionInterface.Key.Enter, true);

            //TODO: Uncomment this and comment out line 74 if the repayment term needs 2nd term
            //List<string> types = new List<string>() { "CP", "CQ", "C1", "C2", "C3", "IB", "IL" };

            //if (types.Contains(data.RepaymentScheduleType))
            //    PutText(19, 60, data.FirstTermGrad, ReflectionInterface.Key.Enter, true);
            //else
            //    PutText(19, 19, data.FirstTermGrad, ReflectionInterface.Key.Enter, true);
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
                        //TODO: RepaymentScheduleType = temp[15],
                        MajorBatch = temp[26],
                        MinorBatch = temp[27],
                        DateSent = DateTime.Parse(temp[16]).ToString("MMddyy"),
                        DateBegin = DateTime.Parse(temp[17]).ToString("MMddyy"),
                        FirstTermGrad = temp[34]
                    });
                }
            }

            return fileData;
        }
    }
}

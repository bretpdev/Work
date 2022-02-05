using System;
using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Key = Uheaa.Common.Scripts.ReflectionInterface.Key;

namespace REFCALL
{
    public class CcpHelper
    {
        public ReflectionInterface RI { get; set; }
        public DataAccess DA { get; set; }
        public ProcessLogRun LogRun { get; set; }

        public CcpHelper(ReflectionInterface ri, DataAccess da, ProcessLogRun logRun)
        {
            RI = ri;
            DA = da;
            LogRun = logRun;
        }

        public CostCenterCode GetCostCenterCode(string ssn)
        {
            //Determine Cost Center Code
            Dictionary<int, CostCenterCode> sequences = new Dictionary<int, CostCenterCode>
            {
                {1, new CostCenterCode("MA2324", "BWNHDGF")}, 
                {2, new CostCenterCode("MA2329", "BWNHDFH")}, 
                {3, new CostCenterCode("MA2328", "BWNHDFH")}, 
                {4, new CostCenterCode("MA2327", "BWNHDGF")}
            };
            int sequence = 4; //default 4
            int? tempSequence = 0;
            int count = 1;
            RI.FastPath("LG10I" + ssn);
            if (RI.CheckForText(1, 53, "LOAN BWR STATUS RECAP SELECT"))
            {
                //LG10 Selection screen found.
                RI.PutText(19, 15, count.ToString(), Key.Enter);
                while (!RI.CheckForText(20, 3, "47001"))
                {
                    tempSequence = GetSequence();
                    if (!tempSequence.HasValue)
                        return CostCenterCode.ERROR;
                    sequence = Math.Min(sequence, tempSequence.Value);
                    RI.Hit(Key.F12);
                    count++;
                    RI.PutText(19, 15, count.ToString(), Key.Enter);
                }
            }
            else //LG10 target screen found
            {
                var temp = GetSequence();
                if (!temp.HasValue)
                    return CostCenterCode.ERROR;
                sequence = temp.Value;
            }
            //set cost center code and ACS key
            return sequences[sequence];
        }

        private int? GetSequence()
        {
            //get CCC per loan
            int seq = 4;
            if (RI.CheckForText(1, 52, "LOAN BWR STATUS RECAP DISPLAY"))
            {
                while (RI.AltMessageCode != "46004")
                {
                    for (int row = 11; row <= 20; row++)
                    {
                        if (!RI.CheckForText(row, 2, "_"))
                            break;
                        var code = RI.GetText(row, 59, 2);
                        var code2 = RI.GetText(row, 64, 2);
                        var isAffiliatedLenderCode = DA.IsAffiliatedLenderCode(RI.GetText(5, 18, 6));
                        var greaterThanZero = (RI.GetText(row, 48, 8).ToDouble()) > 0;
                        if (code != "CR" && code != "CP" && greaterThanZero && isAffiliatedLenderCode)
                            seq = 1;
                        else if ((code == "CP" && greaterThanZero) || (code == "CR" && !code2.IsIn("DB", "DF", "DQ")))
                            seq = Math.Min(seq, 2);
                        else if (code == "CR" && code2.IsIn("DB", "DF", "DQ") && greaterThanZero && !isAffiliatedLenderCode)
                            seq = Math.Min(seq, 3);
                    }
                    if (seq == 1)
                        break;
                    else
                        RI.Hit(Key.F8);
                }
            }
            else
            {
                LogRun.AddNotification("LG10 Taget screen not found. Contact System Support.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                return null;
            }
            return seq;
        }
    }
}
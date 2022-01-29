using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace LG20LNNXFR
{
    public class LG20Update : BatchScript
    {
        private const string EojProcessed = "Total number of records processed";
        private const string EojFileTotal = "Total number of records in the file";
        private const string EojErrors = "Error updating lender_servicer";
        private static readonly string[] EOJ_FIELDS = { EojProcessed, EojFileTotal, EojErrors };

        public LG20Update(ReflectionInterface ri)
            : base(ri, "LG20LNNXFR", "TEMP", "TEMP", EOJ_FIELDS, DataAccessHelper.Region.Uheaa)
        {
        }

        public override void Main()
        {
            StartupMessage("This script updates the lender/servicer on a loan.");
            bool isInRecovery = !string.IsNullOrEmpty(Recovery.RecoveryValue);

            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = EnterpriseFileSystem.TempFolder;
            open.ShowDialog();
            
            List<string> uniqueIDs = File.ReadAllLines(open.FileName).ToList();

            foreach (string item in uniqueIDs)
            {
                if (isInRecovery && item != Recovery.RecoveryValue)
                    continue;
                else if (isInRecovery && item == Recovery.RecoveryValue)
                {
                    isInRecovery = false;
                    continue;
                }
                else
                    ChangeLender(item);
            }

            ProcessingComplete();
        }

        private void ChangeLender(string item)
        {
            FastPath("LG20C*");
            PutText(8, 46, "", true);
            PutText(9, 46, item, ReflectionInterface.Key.Enter, true);
            if (CheckForText(22, 3, "47004"))
            {
                Eoj.Counts[EojErrors].Increment();
                Err.AddRecord(EojErrors, new { error = item });
                return;
            }
            PutText(6, 14, "K");
            PutText(9, 31, "828476");
            PutText(10, 31, "05152013");
            PutText(11, 31, "05152013");
            PutText(12, 31, "700126", ReflectionInterface.Key.Enter);

            //Increment the total count
            Eoj.Counts[EojFileTotal].Increment();

            if (CheckForText(22, 3, "49000"))
                Eoj.Counts[EojProcessed].Increment();
            else
            {
                Eoj.Counts[EojErrors].Increment();
                Err.AddRecord(EojErrors, new { error = item });
            }

            //Set the recovery value
            Recovery.RecoveryValue = item;
        }

    }//class
}//namespace

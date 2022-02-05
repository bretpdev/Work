using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.Scripts;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;

namespace REMPENDEL
{
    public class Rempendel : FedBatchScript<int?>
    {
        const string InputFileName = "NH 2909_SUB_POP.CSV";
        const string Eoj_Total = "Total Rows Processed";
        const string Eoj_Successful = "Rows Successfully Processed";
        const string Eoj_Error = "Unprocessed Rows";
        public Rempendel(ReflectionInterface ri)
            : base(ri, "REMPENDEL", "ERR_BU01", "ERR_BU01", new string[] { Eoj_Total, Eoj_Successful, Eoj_Error })
        {}
        public override void Main()
        {
            string InputFolder = EnterpriseFileSystem.FtpFolder;
            string[] files = Directory.GetFiles(InputFolder, InputFileName + "*");
            if (files.Length > 1)
                NotifyAndEnd(string.Format("Found multiple {0} files at {1}.  Please remove any duplicates before re-running this script, which will now close.", InputFileName, InputFolder));
            else if (files.Length == 0)
                NotifyAndEnd(string.Format("Couldn't find any files like {0} in {1}.  Script will now close.", InputFileName, InputFolder));

            CsvLineParser parser = new CsvLineParser(files.Single(), ",", CsvLineParser.EmptyValueValidator, line =>
            {
                Recovery.RecoveryValue = line.LineNumber;

                string accountNumber = line.Content[0];
                string loanSequence = line.Content[1].PadLeft(3, '0');

                //this clears the fastpath. A more elegant solution would be desirable.
                RI.GoToScreen("CTD49***************");

                RI.PutText(8, 42, accountNumber);
                RI.PutAndEnter(9, 42, loanSequence);

                if (RI.MessageCode == "01019") //key not found AKA borrower exists but no loan sequence.
                {
                    AddError("Borrower's Loan Sequence not found.", line);
                    return;
                }
                if (RI.MessageCode == "04719") //borrower doesn't exist
                {
                    AddError("Borrower Not Found", line);
                    return;
                }

                if (RI.GetText(12, 16, 2) != "D5") //DMCS Status Field
                {
                    AddError("DMCS Status Field was not D5", line);
                    return;
                }

                RI.PutText(15, 15, "R");
                RI.Hit(ReflectionInterface.Key.Enter);
                if (RI.MessageCode == "01005") //success
                    Eoj[Eoj_Successful]++;
                else
                {
                    AddError(RI.MessageCode, line);
                    return;
                }
            });
            var result = parser.Parse(Recovery.RecoveryValue);
            Eoj[Eoj_Total] = result.TotalLines;
            Eoj.Publish();
            Err.Publish();
            Recovery.RecoveryValue = null;
            EndDllScript();
        }

        private void AddError(string message, Line<string[]> line)
        {
            Err.AddRecord("DMCS Status Field was not D5", line);
            Eoj[Eoj_Error]++;
        }
    }
}

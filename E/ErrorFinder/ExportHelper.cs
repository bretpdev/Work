using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ErrorFinder
{
    public static class ExportHelper
    {
        private class Amalgam
        {
            public string Label { get; set; }
            public string Path { get; set; }
            public IEnumerable<BorrowerLine> Lines { get; set; }
        }
        public static void ExportAll(IEnumerable<BorrowerLine> lines, string exportPath, Action<string> beginProcessingFile, Action success, Action failure)
        {
            string timeStamp = Util.TimeStamp();
            string errorPath = Path.Combine(exportPath, "By Error Code");
            string batchPath = Path.Combine(exportPath, "By Minor Bath No");

            List<Amalgam> amalgamations = new List<Amalgam>();
            amalgamations.Add(new Amalgam() { Label = "all_borrowers", Lines = lines, Path = exportPath });
            amalgamations.AddRange(lines.GroupBy(o => o.ErrorCode).Select(
                o => new Amalgam() { Label = "borrowers_with_error_" + o.Key, Lines = o, Path = errorPath }));
            amalgamations.AddRange(lines.GroupBy(o => o.MinorBatchNo).Select(
                o => new Amalgam() { Label = "borrowers_with_batch_" + o.Key, Lines = o, Path = batchPath }));

            foreach (var a in amalgamations)
            {
                string fileName = string.Format("{0}_{1}.csv", a.Label, timeStamp);
                if (beginProcessingFile != null)
                    beginProcessingFile(fileName);
                bool exportSuccessful = Export(a.Lines, Path.Combine(a.Path, fileName));
                if (!exportSuccessful && failure != null)
                {
                    failure();
                    return;
                }
            }
            success();
        }
        public static bool Export(IEnumerable<BorrowerLine> lines, string fileName, bool autoOpen = false)
        {
            string dir = Path.GetDirectoryName(fileName);
            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Couldn't access path " + dir + ".  Ensure you have permissions or change the export location.");
                return false;
            }

            List<string> exportLines = new List<string>();
            exportLines.Add("SSN,LoanSeq,ErrorCode,MajorBatch,MinorBatch");
            exportLines.AddRange(lines.Select(bor => "=\"" + bor.SSN + "\"," + bor.SeqNo + "," + bor.ErrorCode + "," +bor.MajorBatch + "," + bor.MinorBatchNo));
            File.WriteAllLines(fileName, exportLines.ToArray());

            if (autoOpen)
                Process.Start(fileName);
            return true;
        }
    }
}

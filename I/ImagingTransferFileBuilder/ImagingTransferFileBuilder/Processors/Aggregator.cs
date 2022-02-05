using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ionic.Zip;
using System.IO;
using ImagingTransferFileBuilder.Properties;

namespace ImagingTransferFileBuilder
{
    public class ZipAlreadyExistsException : Exception { }
    public static class Aggregator
    {
        public static void Aggregate(string resultsLocation, string dealID, string saleDate, string loanProgramType)
        {
            Results.Clear();
            Progress.Start("Aggregator");
            string code = Util.Code(dealID, DateTime.Parse(saleDate));
            string zipLocation = Path.Combine(resultsLocation, code + ".ZIP");
            List<string> indexLines = new List<string>();
            if (File.Exists(zipLocation))
                throw new ZipAlreadyExistsException();

            using (ZipFile zf = new ZipFile())
            {
                Progress.Increments = Directory.GetDirectories(resultsLocation).Length + 2;
                foreach (string dir in Directory.GetDirectories(resultsLocation).Union(new string[] { resultsLocation }))
                {
                    indexLines.AddRange(ProcessDirectory(dir, zf, code));
                    Progress.Increment();
                }
                for (int i = 0; i < indexLines.Count; i++)
                {
                    string line = indexLines[i];
                    string[] lineParts = line.Split('|');
                    lineParts[6] = loanProgramType;
                    lineParts[9] = dealID;
                    lineParts[8] = saleDate;
                    indexLines[i] = string.Join("|", lineParts);
                }
                zf.AddEntry(Path.Combine(code, code + ".IDX"), string.Join(Environment.NewLine, indexLines.OrderBy(i => i.Substring(0, 9)).ToArray()));//order by ssn
                zf.SaveProgress += new EventHandler<SaveProgressEventArgs>((o, e) => { Application.DoEvents(); });
                zf.Save(zipLocation);
                Settings.Default.ZipLocation = zipLocation;
            }
            Progress.Increment();
            Progress.Finish();
        }

        private static List<string> ProcessDirectory(string directory, ZipFile zip, string code)
        {
            List<string> indexLines = new List<string>();
            foreach (string file in Directory.GetFiles(directory))
            {
                if (file.ToLower().EndsWith("thumbs.db") || file.ToLower().EndsWith(".zip"))
                    continue;
                if (file.ToLower().EndsWith(".idx"))
                {
                    indexLines = new List<string>(File.ReadAllLines(file).Where(s => !string.IsNullOrEmpty(s)));
                }
                else
                {
                    try
                    {
                        zip.AddFile(file, code);
                    }
                    catch (ArgumentException)
                    {
                        Results.LogError("Tried to add {0} but an item with that name was already found elsewhere.", file);
                    }
                }
            }
            return indexLines;
        }
    }

}

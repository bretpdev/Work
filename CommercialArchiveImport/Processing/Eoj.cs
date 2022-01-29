using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;
using Uheaa.Common;

namespace CommercialArchiveImport
{
    public class Eoj
    {
        int goodLines;
        List<InvalidLine> invalidLines = new List<InvalidLine>();
        ZipFile source;
        ZipFile remainingFile;
        List<Document> docs;
        public Eoj(ZipFile source, List<Document> docs)
        {
            this.docs = docs;
            this.source = source;
            this.remainingFile = new ZipFile();

            foreach (Document d in docs)
            {
                if (d.Invalid)
                    AddInvalidLine(d.OriginalLine, d.LineNumber, d.Error.ErrorMessage);
                else
                    AddProcessedLine();
            }
        }

        public static string EojFolder = @"Q:\Document Services\Commercial Archive Import\End of Job Reports";
        public EojResults Publish(string resultsFolder, string ea80location)
        {
            ProgressHelper.Next("Compiling");
            EojResults results = new EojResults();
            results.ResultsFolder = resultsFolder;
            results.EojFolder = EojFolder;
            results.EA80Location = ea80location;
            int count = 0;
            do
            {
                results.EojFile = string.Format("Commercial Archive Import - EOJ Report {0:MM-dd-yyyy HH.mm}{1}.html",
                    DateTime.Now, count == 0 ? "" : " " + count.ToString());
                results.EojFile = Path.Combine(results.EojFolder, results.EojFile);
                count++;

            } while (File.Exists(results.EojFile));

            GatherOrphanedImages();
            if (CreateIndexFile(results))
            {
                remainingFile.Save(results.RemainingZip);
            }

            if (!WriteHtml(results))
                return null;
            ProgressHelper.Finish();
            return results;
        }

        public void AddProcessedLine()
        {
            goodLines++;
        }

        public void AddInvalidLine(string line, int lineNumber, string error)
        {
            invalidLines.Add(new InvalidLine() { Line = line, LineNumber = lineNumber, Error = error });
        }

        public void GatherOrphanedImages()
        {
            ProgressHelper.Increments = source.Count();
            foreach (ZipEntry ze in source)
            {
                if (!ze.FileName.ToLower().EndsWith(".idx") && !ze.IsDirectory)
                {
                    var line = new InvalidLine() { Line = "(none)", Error = "Unreferenced image found in zip file: " + Path.GetFileName(ze.FileName) };
                    invalidLines.Add(line);
                    ResultsHelper.LogError(line.Error);
                    remainingFile.AddEntry(Path.GetFileName(ze.FileName), ze.GetBytes());
                }
                ProgressHelper.Increment();
            }
        }

        private bool WriteHtml(EojResults results)
        {
            if (!Directory.Exists(results.EojFolder))
                Directory.CreateDirectory(results.EojFolder);
            try
            {
                using (StreamWriter sw = new StreamWriter(results.EojFile))
                {
                    //Start the HTML file and write out the report headers.
                    sw.WriteLine("<html>");
                    sw.WriteLine("<head>");
                    sw.WriteLine("<style type='text/css'>");
                    sw.WriteLine("body{font-family:Arial,Helvetica,sans-serif;}table{border-collapse:collapse;}td,th{padding:2px 10px;}tr.oddrow{background-color:#EEE;}h1{font-size:20px;}h2{font-size:16px;}");
                    sw.WriteLine("</style>");
                    sw.WriteLine("</head>");
                    sw.WriteLine("<body>");
                    sw.WriteLine("<h1>Commercial Archive Import</h1>");
                    sw.WriteLine("<h2>EOJ Report (" + results.EA80Location +  ")</h2>");
                    sw.WriteLine("<h2>" + results.EojFile + "</h2>");

                    if (results.HasRemainingZip)
                        sw.WriteLine("<a href='file://{0}'>Remaining Results Zip file</a>", results.RemainingZip);

                    List<string> synopsis = new List<string>();
                    synopsis.Add("Result, Count");
                    synopsis.Add("Processed Lines, " + goodLines.ToString());
                    synopsis.Add("Invalid Lines, " + invalidLines.Count(o => o.LineNumber != null).ToString());
                    synopsis.Add("Unprocessed Images, " + remainingFile.Where(o => !o.FileName.ToLower().EndsWith(".idx")).Count()); //don't include index file
                    foreach (string line in synopsis.ToHtmlLines("    "))
                        sw.WriteLine(line);
                    sw.WriteLine("<br />");
                    if (invalidLines.Count > 0)
                    {
                        List<string> invalids = new List<string>();
                        invalids.Add("#, Line, Error");
                        invalids.AddRange(invalidLines.Select(iv => iv.LineNumber + ", " + iv.Line + ", " + iv.Error));
                        foreach (string line in invalids.ToHtmlLines("    "))
                            sw.WriteLine(line);
                    }

                    //close out the HTML.
                    sw.WriteLine("</body>");
                    sw.WriteLine("</html>");
                    return true;
                }
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        private bool CreateIndexFile(EojResults results)
        {
            if (remainingFile.Count == 0)
                return false;
            string indexName = Path.GetFileName(source.Name.Substring(0, source.Name.Length - 4)); //remove .zip
            indexName += " - " + DateTime.Now.ToString("MM-dd-yyyy HH.mm");
            int count = 0;
            while (File.Exists(indexName + (count == 0 ? "" : " " + count) + ".zip"))
                count++;
            indexName += (count == 0 ? "" : " " + count);
            var lines = invalidLines.Where(il => il.LineNumber.HasValue).Select(il => il.Line).ToArray();
            byte[] contents = Encoding.ASCII.GetBytes(string.Join(Environment.NewLine, lines));
            remainingFile.AddEntry(indexName + ".idx", contents);
            results.RemainingZip = Path.Combine(results.EojFolder, indexName + ".zip");
            return true;
        }

        private class InvalidLine
        {
            public string Line { get; set; }
            public int? LineNumber { get; set; }
            public string Error { get; set; }
        }
    }

    public class EojResults
    {
        public string ResultsFolder { get; set; }
        public string EojFile { get; set; }
        public string EojFolder { get; set; }
        public string LoadFolder { get; set; }
        public string RemainingZip { get; set; }
        public string EA80Location { get; set; }
        public bool HasRemainingZip { get { return !string.IsNullOrEmpty(RemainingZip); } }

        public EojResults()
        {
            LoadFolder = @"\\imgprodkofax\ascent$\UTCROther_imp";
        }
    }
}

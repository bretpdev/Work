using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ionic.Zip;

namespace AutomatedImageImporter
{
    public static class Processor
    {
        public class NoIndexFileException : Exception { }
        public class MultipleIndexFilesException : Exception { }
        public static void Process(string sourceZip, string monitorFolder, int indexCount, DataGridView grid, TreeView tree)
        {
            using (ZipFile zf = new ZipFile(sourceZip))
            {
                var indices = zf.Where(z => z.FileName.ToLower().EndsWith(".idx"));
                if (indices.Count() > 1)
                    throw new MultipleIndexFilesException();
                if (indices.Count() == 0)
                    throw new NoIndexFileException();

                var index = indices.Single();

                string[] lines = index.GetStringContents().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                int chunks = (lines.Length / indexCount) + 1;
                ProgressHelper progress = new ProgressHelper(chunks, grid, tree);
                int num = 0;
                while (num < lines.Length - 1)
                {
                    int endLine = num + indexCount;
                    if (lines.Length <= endLine)
                    {
                        endLine = lines.Length - 1;
                    }
                    else
                        while (IsTiffPage(lines[endLine]))
                            endLine--;

                    List<string> chunkLines = new List<string>(lines.Skip(num).Take(endLine - num + 1));
                    progress.NextChunk(chunkLines.Count);

                    ProcessChunk(zf, monitorFolder, num, chunkLines, progress);

                    num = endLine;
                }
            }
        }

        private static void ProcessChunk(ZipFile zf, string monitorFolder, int firstLine, List<string> lines, ProgressHelper progress)
        {
            int lineNumber = firstLine - 1;
            List<Line> goodLines = new List<Line>();
            foreach (string line in lines)
            {
                lineNumber++;
                string[] parts = line.Split('|');
                if (parts.Length != 11)
                {
                    progress.InvalidLine(lineNumber, line);
                    continue;
                }
                string fileName = parts.Last();
                ZipEntry source = zf.Find(fileName);
                string monitorPath = Path.Combine(monitorFolder, fileName);
                if (source == null || File.Exists(monitorPath))
                {
                    progress.InvalidLine(lineNumber, line);
                    continue;
                }
                File.WriteAllBytes(monitorPath, source.GetBytes());
                goodLines.Add(new Line(line, lineNumber));
            }
            string index = Guid.NewGuid().ToString() + ".idx";
            //write index file
            File.WriteAllLines(Path.Combine(monitorFolder, index), goodLines.Select(o => o.Text).ToArray());
            string error = Path.Combine(monitorFolder, "Error");

            for (int i = 0; i < goodLines.Count; i++)
            {
                progress.NextFile(goodLines[i].LineNumber, goodLines[i].Text);
                string file = Path.Combine(monitorFolder, goodLines[i].Text.Split('|').Last());
                while (File.Exists(file))
                {
                    if (Directory.Exists(error) && Directory.GetFiles(error, "*" + index).Any())
                    {
                        progress.FailedChunk(goodLines.Skip(i + 1));
                        return;
                    }
                }
                if (Directory.Exists(error) && Directory.GetFiles(error, "*" + index).Any())
                {
                    progress.FailedChunk(goodLines.Skip(i + 1));
                    return;
                }
            }
            progress.CompletedChunk();
        }

        private static bool IsTiffPage(string line)
        {
            if (line.Length < 4)
                return false;
            var ext = line.Substring(line.Length - 4).ToUpper();
            int outer = 0;
            if (ext.StartsWith("T"))
                if (int.TryParse(ext.Substring(1), out outer))
                    return true;
            return false;
        }
    }
}

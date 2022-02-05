using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomatedImageImporter
{
    public class ProgressHelper
    {
        public ProgressResults Results { get; set; }
        private DataGridView grid;
        private TreeView tree;
        private Node currentNode;
        private Node currentChild;
        //csv log
        private string logPath;

        public ProgressHelper(int totalChunks, DataGridView grid, TreeView tree)
        {
            Results = new ProgressResults();
            Results.TotalChunks = totalChunks;
            this.grid = grid;
            this.tree = tree;
            logPath = string.Format(@"T:\Automated Image Importer - LOG - {0:MM-dd-yyyy hh-mm-ss}.csv", DateTime.Now);
            using (StreamWriter sw = File.CreateText(logPath))
                sw.WriteLine("LineNumber,Status,Line"); //csv header
            Sync();
        }

        public void NextChunk(int totalFilesThisChunk)
        {
            Results.TotalFilesThisChunk = totalFilesThisChunk;
            Results.CurrentFileThisChunk = 0;
            currentNode = Node.ChunkNode(tree, Results.CurrentChunk);
            currentChild = null;
            Sync();
        }

        public void NextFile(int lineNumber, string line)
        {
            Results.CurrentFileThisChunk++;
            if (currentChild != null)
            {
                currentChild.Status = Node.LineStatus.Complete;
                LogLine(currentChild.Line.LineNumber, "Complete", currentChild.Line.Text);
            }
            currentChild = Node.LineNode(currentNode.TreeNode, lineNumber, line);
            Sync();
        }

        public void InvalidLine(int lineNumber, string line)
        {
            Results.CurrentFileThisChunk++;
            Node.LineNode(currentNode.TreeNode, lineNumber, line).Status = Node.LineStatus.Invalid;
            LogLine(lineNumber, "Invalid", line);
            Sync();
        }

        public void FailedChunk(IEnumerable<Line> remainingLines)
        {
            Results.FailedChunks++;

            currentNode.Status = Node.LineStatus.Failed;
            if (currentChild != null)
            {
                currentChild.Status = Node.LineStatus.Failed;
                LogLine(currentChild.Line.LineNumber, "Failed", currentChild.Line.Text);
            }
            foreach (Line l in remainingLines)
            {
                Node.LineNode(currentNode.TreeNode, l.LineNumber, l.Text).Status = Node.LineStatus.Unknown;
                LogLine(l.LineNumber, "Not Processed", l.Text);
            }
            Sync();
        }

        public void CompletedChunk()
        {
            Results.CompletedChunks++;
            currentNode.Status = Node.LineStatus.Complete;
            if (currentChild != null)
            {
                currentChild.Status = Node.LineStatus.Complete;
                LogLine(currentChild.Line.LineNumber, "Complete", currentChild.Line.Text);
            }
            currentChild = null;
            Sync();
        }

        private void Sync()
        {
            if (grid != null)
            {
                Invoke(() =>
                {
                    grid.DataSource = Results.ToTable();
                    grid.ClearSelection();
                    grid.Columns[0].Width = (int)(grid.Width * 0.75);
                    grid.Columns[1].Width = (int)(grid.Width * 0.25);
                    grid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                });
            }
        }

        private void Invoke(Action a)
        {
            Control c = grid != null ? (Control)grid : tree;
            if (c == null)
                return;
            while (c.Parent != null)
                c = c.Parent;
            try
            {
                c.Invoke(a);
            }
            catch (InvalidOperationException)
            {
                return; //handle hasn't been created
            }
        }

        private void LogLine(int lineNumber, string status, string line)
        {
            File.AppendAllLines(logPath, new string[] { lineNumber + "," + status + "," + line });
        }

        public class ProgressResults
        {
            public decimal TotalCompletion
            {
                get
                {
                    if (TotalChunks == 0)
                        return 0;
                    decimal result = (CompletedChunks + FailedChunks + ChunkCompletion) / TotalChunks;
                    return result > 1 ? 1 : result;
                }
            }
            public decimal ChunkCompletion
            {
                get
                {
                    if (TotalFilesThisChunk == 0)
                        return 0;
                    return CurrentFileThisChunk / (decimal)TotalFilesThisChunk;
                }
            }
            public int RemainingChunks
            {
                get
                {
                    return TotalChunks - FailedChunks - CompletedChunks;
                }
            }
            public int CurrentChunk
            {
                get
                {
                    return TotalChunks - RemainingChunks;
                }
            }
            public int RemainingChunkFiles
            {
                get
                {
                    return TotalFilesThisChunk - CurrentFileThisChunk;
                }
            }
            public int CompletedChunks { get; set; }
            public int FailedChunks { get; set; }
            public int TotalChunks { get; set; }
            public int TotalFilesThisChunk { get; set; }
            public int CurrentFileThisChunk { get; set; }

            public DataTable ToTable()
            {
                DataTable dt = new DataTable();
                var metric = dt.Columns.Add("Metric");
                var value = dt.Columns.Add("Value");
                Action<string, object> addRow = new Action<string, object>((s, o) =>
                {
                    var row = dt.NewRow();
                    row[metric] = s;
                    if (o is decimal)
                        row[value] = ((decimal)o).ToString("0%");
                    else
                        row[value] = o;
                    dt.Rows.Add(row);
                });
                addRow("Overall Completion", TotalCompletion);
                addRow("Chunks Remaining", RemainingChunks);
                addRow("Chunks Completed", CompletedChunks);
                addRow("Chunks Failed", FailedChunks);
                addRow("Current Chunk Completion", ChunkCompletion);
                addRow("Files This Chunk", TotalFilesThisChunk);
                addRow("Current Chunk File", CurrentFileThisChunk);
                return dt;
            }
        }

    }
}

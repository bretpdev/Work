using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using Uheaa.Common.DataAccess;

namespace SERF_File_Generator
{
    public class ClassFileWriter
    {
        public bool IsRunning { get; private set; }
        public string FileName { get; private set; }
        object queueLock = new object();
        public void Stop()
        {
            while (PendingLines.Count != 0)
                continue;
            IsRunning = false;
            while (!doneProcessing)
                continue;
        }
        private Queue<Queue<string>> PendingLines;

        public ClassFileWriter(string fileName)
        {
            PendingLines = new Queue<Queue<string>>();
            FileName = fileName;
        }

        public void AddBorrower(Queue<string> records)
        {
            lock (queueLock)
                PendingLines.Enqueue(records);
        }

        private const int MaxLineLength = 3075;
        private bool doneProcessing = false;
        public void Process()
        {
            IsRunning = true;
            using (StreamWriter sw = new StreamWriter(FileName, true, Encoding.ASCII, MaxLineLength))
            {
                while (IsRunning || PendingLines.Count != 0)
                {
                    if (PendingLines.Count != 0)
                    {
                        Queue<string> borrower = PendingLines.Dequeue();
                        while (borrower != null && borrower.Count != 0)
                        {
                            string line = borrower.Dequeue();
                            if (line.Length != MaxLineLength)
                                throw new Exception(string.Format("Line was not {0} characters as expected.  Line data: {1}", MaxLineLength, line));
                            sw.WriteLine(line);
                        }
                    }
                }
            }
            doneProcessing = true;
        }
    }
}



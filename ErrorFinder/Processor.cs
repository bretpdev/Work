using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ErrorFinder
{
    public class Processor
    {
        public Thread Thread { get; private set; }
        public bool NelNet { get; private set; }
        private Processor() { }
        public static Processor Process(string filePath, Form invoker, Action<int, int> progressUpdate, Action<List<BorrowerLine>> results)
        {
            Processor p = new Processor();

            using (StreamReader firstLine = new StreamReader(filePath))
                p.NelNet = (firstLine.ReadLine() ?? "").StartsWith(" TAXFN"); //other format has a number in front of TAXFN
            p.Thread = new Thread(() =>
            {
                StateManager sm = new StateManager();
                sm.Nelnet = p.NelNet;
                int progress = 0;
                int max = (int)new FileInfo(filePath).Length;
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        progress += line.Length;
                        if (sm.ValidateLine(line))
                        {
                            sm.LoadMinorBatch(line);
                            sm.LoadSSN(line);
                            sm.LoadLoanSeq(line);
                            sm.LoadErrorCode(line);
                        }
                        invoker.Invoke(progressUpdate, progress, max);
                    }
                }
                invoker.Invoke(results, sm.Lines);
            });
            p.Thread.Start();
            return p;
        }

        public class StateManager
        {
            public bool Nelnet { get; set; }
            private int ShiftMod { get { return Nelnet ? 0 : 1; } }
            public string CurrentSSN { get; set; }
            public string CurrentSeq { get; set; }
            public string CurrentMajorBatch { get; set; }
            public string CurrentMinorBatch { get; set; }
            public List<BorrowerLine> Lines { get; set; }
            public StateManager()
            {
                Lines = new List<BorrowerLine>();
            }

            public bool ValidateLine(string line)
            {
                return line.Length >= 51 + ShiftMod;
            }

            public void LoadMinorBatch(string line)
            {
                if (line.Contains(" MAJOR BATCH"))
                {
                    CurrentMajorBatch = line.Substring(16, 11).Trim();
                    CurrentMinorBatch = line.Substring(51 + ShiftMod).Trim();
                }
            }

            public void LoadSSN(string line)
            {
                CurrentSSN = GetInt(line, 2 + ShiftMod, 9) ?? CurrentSSN;
            }

            public void LoadLoanSeq(string line)
            {
                CurrentSeq = GetInt(line, 17 + ShiftMod, 3) ?? CurrentSeq;
            }

            public void LoadErrorCode(string line)
            {
                string errorCode = GetInt(line, 54 + ShiftMod, 5);
                if (errorCode != null)
                {
                    Lines.Add(new BorrowerLine()
                    {
                        ErrorCode = errorCode,
                        SeqNo = CurrentSeq,
                        SSN = CurrentSSN,
                        MajorBatch = CurrentMajorBatch,
                        MinorBatchNo = CurrentMinorBatch
                    });
                }
            }

            int parser = 0;
            private string GetInt(string line, int start, int length)
            {
                if ((start + length) > line.Length) return null;
                string temp = line.Substring(start, length).Trim();
                if (temp.Length == length && int.TryParse(temp, out parser))
                    return temp;
                return null;
            }

        }
    }
}

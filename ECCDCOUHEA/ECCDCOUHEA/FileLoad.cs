using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace ECCDCOUHEA
{
    public class FileLoad
    {
        private string filePath;
        private ProcessLogRun logRun;

        public FileLoad(string filePath, ProcessLogRun logRun)
        {
            this.filePath = filePath;
            this.logRun = logRun;
        }

        public Stream PickCsv()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (Directory.Exists(filePath))
            {
                openFile.InitialDirectory = filePath;
            }
            else
            {
                openFile.InitialDirectory = Path.GetPathRoot(Environment.SystemDirectory);
            }
            openFile.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            openFile.FilterIndex = 2;
            openFile.RestoreDirectory = true;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    return openFile.OpenFile();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    return PickCsv();
                }
            }
            else
            {
                logRun.LogEnd();
                throw new FileNotFoundException("Dialog exited without a file being provided");
            }
        }

        public List<ArcData> ParseCsv(Stream csv)
        {
            List<ArcData> arcs = new List<ArcData>();
            if (csv == null)
            {
                logRun.LogEnd();
                throw new FileLoadException("Unable to open selected file");
            }
            int lineNum = 0;
            int headerLines = 3;
            try
            {
                using (csv)
                using (StreamReader csvReader = new StreamReader(csv))
                {
                    string line;
                    while ((line = csvReader.ReadLine()) != null)
                    {
                        List<string> cols = new List<string>(CsvHelper.Parse(line));
                        if (lineNum < headerLines)
                        {
                            //Process header if necessary
                        }
                        else
                        {
                            //Process each ARC
                            ArcData arcData = ParseLine(cols, lineNum);
                            if (arcData != null) //Ignore Bad Rows and Defaulted Accounts
                            {
                                arcs.Add(arcData);
                            }
                        }
                        lineNum++;
                    }
                    //No data found
                    if (lineNum == 0)
                    {
                        MessageBox.Show("No data to process.");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to load file.");
            }
            return arcs;
        }

        public ArcData ParseLine(List<string> cols, int line/*informational*/)
        {
            if (cols.Count < 27) //Number of columns in a document
            {
                logRun.AddNotification(string.Format("Bad column count for ECCDCOUHEA, skipping Line:{0}",
                   line.ToString()), NotificationType.FileFormatProblem, NotificationSeverityType.Warning);
                return null;
            }

            if (cols[22].Length < 17)
            {
                //Pad 0's when the length is not long enough
                while (cols[22].Length != 17)
                {
                    cols[22] = '0' + cols[22];
                }
            }
            else if (cols[22].Length != 17)
            {
                logRun.AddNotification(string.Format("Bad account number for ECCDCOUHEA, skipping Line: {0} AccountNumber:{1}",
                   line.ToString(), cols[22]), NotificationType.FileFormatProblem, NotificationSeverityType.Warning);
                return null;
            }

            ArcData arcData;
            DataAccessHelper.Region reg;
            int loanSeq;
            int.TryParse(cols[22].Substring(12, 5), out loanSeq);
            //Region is Cornerstone
            if (cols[22]/*Account Number*/.Substring(10, 2).ToUpper() == "KU")
            {
                reg = DataAccessHelper.Region.CornerStone;
                arcData = new ArcData(reg)
                {
                    ArcTypeSelected = ArcData.ArcType.Atd22ByLoan, //Requirements specifies 0
                    ResponseCode = null,//cols[7],
                    AccountNumber = cols[22].Substring(0, 10),
                    RecipientId = null,
                    Arc = "RVCRD",
                    ScriptId = Program.scriptId,
                    ProcessOn = DateTime.Now,
                    Comment = null,
                    IsReference = false,
                    IsEndorser = false,
                    ProcessFrom = null,
                    ProcessTo = null,
                    NeedBy = null,
                    RegardsTo = null,
                    RegardsCode = null,
                    LoanSequences = new List<int> { loanSeq }
                };
            }
            //Region is Uheaa
            else if (cols[22]/*Account Number*/.Substring(10, 2).ToUpper() == "UT")
            {
                reg = DataAccessHelper.Region.Uheaa;
                arcData = new ArcData(reg)
                {
                    ArcTypeSelected = ArcData.ArcType.Atd22ByLoan, //Requirements specifies 0
                    ResponseCode = null,//cols[7],
                    AccountNumber = cols[22].Substring(0, 10),
                    RecipientId = null,
                    Arc = "RVCRD",
                    ActivityType = null,
                    ActivityContact = null,
                    ScriptId = Program.scriptId,
                    ProcessOn = DateTime.Now,
                    Comment = null,
                    IsReference = false,
                    IsEndorser = false,
                    ProcessFrom = null,
                    ProcessTo = null,
                    NeedBy = null,
                    RegardsTo = null,
                    RegardsCode = null,
                    LoanSequences = new List<int> { loanSeq }
                };
            }
            //Borrower is defaulted
            else
            {
                //ProcessLogger.AddNotification(logRun.ProcessLogId, string.Format("Bad region in account number for ECCDCOUHEA, skipping Line: {1} AccountNumber:{2}",
                //line.ToString(), cols[22] ), NotificationType.FileFormatProblem, NotificationSeverityType.Warning);
                return null;
            }


            return arcData;
        }
    }
}

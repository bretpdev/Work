using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using WinSCP;
using Excel = Microsoft.Office.Interop.Excel;

namespace FSAMTHCALL
{
    public class SampleFile
    {
        const int InboundLength = 10000;
        const int OutboundLength = 5000;

        private readonly ProcessLogRun PLR;
        private readonly DataAccess DA;
        public SampleFile(ProcessLogRun plr)
        {
            PLR = plr;
            DA = new DataAccess(PLR.ProcessLogId);
        }

        public int CreateExcelFile(bool reconcileOverride)
        {
            if (DataAccessHelper.CurrentMode == DataAccessHelper.Mode.Dev && reconcileOverride)
            {
                //Dont validate of test override
            }
            else
            {
                if (Reconcile(false) != 0)//reconciles 1 last time before running the excel sample file.  Full refresh is false
                    PLR.AddNotification("Failed to complete reconciliation prior to sample file creation.  Excel output will be as current as the most recent successful reconciliation.", NotificationType.ErrorReport, NotificationSeverityType.Warning);
                RevalidateCalls();
            }

            List<NobleData> validCalls = DA.GetValidCalls(); //repull after revalidation

            if (validCalls.Any())
            {
                string[] specialCampaigns = DA.GetSpecialCampaigns().ToArray();

                List<NobleData> inBound = GetInboundCalls(validCalls, specialCampaigns).Where(o => !o.VoxFileNotFound).ToList();
                List<NobleData> outBound = GetOutBoundCalls(validCalls, specialCampaigns).Where(o => !o.VoxFileNotFound).ToList();
                List<NobleData> special = GetSpecialCalls(validCalls, specialCampaigns).Where(o => !o.VoxFileNotFound).ToList();

                WriteToExcel(outBound, inBound, special);
            }
            else //all calls invalid.  Cannot do samplefile
            {
                PLR.AddNotification("No valid calls returned for sample file.", NotificationType.ErrorReport, NotificationSeverityType.Critical);
            }
            return 0;
        }

        private void RevalidateCalls()
        {
            List<NobleData> validCalls = DA.GetValidCalls();
            ThreadCallLookup(validCalls, true);
        }

        public int Reconcile(bool fullRefresh)
        {
            List<NobleData> allCalls = DA.GetUnreconciledCalls(fullRefresh);
            if (allCalls.Count != 0)
                ThreadCallLookup(allCalls, false);
            return 0;
        }

        private void ThreadCallLookup(List<NobleData> allCalls, bool revalidation)
        {
            int take = Math.Ceiling(Math.Max(allCalls.Count.ToString().ToDouble() / 75.00, 75.00)).ToString().ToInt(); //Processing 75 threads per calls (noble server can't handle more concurrent connections)
            List<List<NobleData>> threadedCalls = new List<List<NobleData>>();
            for (int skip = 0; skip < allCalls.Count; skip += take)
                threadedCalls.Add(allCalls.Skip(skip).Take(take).ToList());

            if (threadedCalls.Any())
                Parallel.ForEach(threadedCalls, new ParallelOptions() { MaxDegreeOfParallelism = threadedCalls.Count }, calls =>
                {
                    CompareDatabaseToNobleFileServer(calls, revalidation);
                });
        }


        /// <summary>
        /// Grabs a random sampling of the inbound calls for FSA to review or 10000 if we dont have more than that
        /// </summary>
        private List<NobleData> GetInboundCalls(List<NobleData> allCalls, string[] specialCampaigns)
        {
            List<NobleData> inboundCalls = allCalls.Where(p => p.IsInbound).Where(p => !p.CallCampaign.IsIn(specialCampaigns)).ToList();
            if (inboundCalls.Count < InboundLength)
                return inboundCalls;
            else
                return SampleCalls(inboundCalls, InboundLength);
        }

        /// <summary>
        /// Grabs a random sampling of the outbound calls for FSA to review or 5000 if we dont have more than that
        /// </summary>
        private List<NobleData> GetOutBoundCalls(List<NobleData> allCalls, string[] specialCampaigns)
        {
            List<NobleData> outboundCalls = allCalls.Where(p => !p.IsInbound).Where(p => !p.CallCampaign.IsIn(specialCampaigns)).ToList();
            if (outboundCalls.Count < OutboundLength)
                return outboundCalls;
            else
                return SampleCalls(outboundCalls, OutboundLength);
        }

        /// <summary>
        /// Grabs all specialty calls for AES to review (scra)
        /// </summary>
        private List<NobleData> GetSpecialCalls(List<NobleData> allCalls, string[] specialCampaigns)
        {
            return allCalls.Where(p => p.CallCampaign.IsIn(specialCampaigns)).ToList();
        }

        /// <summary>
        /// Takes a list of calls and grabs a subset of those calls based on quantity
        /// </summary>
        private List<NobleData> SampleCalls(List<NobleData> calls, int quantity)
        {
            List<string> campaigns = DA.GetSpecialCampaigns();
            List<int> randomCalls = GenerateRandomNumber(quantity, calls.Where(p => !p.CallCampaign.IsIn(campaigns.ToArray())).ToList());
            return calls.Where(p => p.CallIdNumber.IsIn(randomCalls.ToArray())).ToList();
        }

        /// <summary>
        /// Reconciles our database records with Nobles file server recording files and process logs the ones it cant find
        /// </summary>
        private void CompareDatabaseToNobleFileServer(List<NobleData> allCalls, bool revalidate = false)
        {

            using (Session session = new Session())
            {
                SessionOptions ss = DA.GetSessionOptions("nscsvc", DA.GetNoblePassword("nscsvc"));
                session.Open(ss);

                Parallel.ForEach(allCalls.Where(q => q.VoxFileLocation.IsNullOrEmpty() || revalidate).ToList(), new ParallelOptions() { MaxDegreeOfParallelism = int.MaxValue }, call =>
                {
                    string format = call.CallDate > new DateTime(2019, 11, 8) ? ".wav" : ".vox";
                    string folderToCheck = call.CallDate.AddDays(1).ToString("yyyyMMdd");
                    int folderNumberToCheck = 2;
                    Console.WriteLine($"Checking Vox file exists for CallId: {call.CallIdNumber}");
                    bool foundCall = false;
                    string location = "";
                    if (revalidate && call.VoxFileLocation.IsPopulated())
                    {
                        location = call.VoxFileLocation;
                    }

                    while (session.FileExists($"/archive{folderNumberToCheck}"))
                    {
                        string fullPath = $"/archive{folderNumberToCheck}/fed/{folderToCheck}/";
                        string defaultPath = $"/archive{folderNumberToCheck}/default/{folderToCheck}/";
                        if (!revalidate) //looking for file for the first time
                        {
                            if (session.FileExists($"{fullPath}{call.VoxFileId}{format}"))
                            {
                                DA.UpdateVoxFileLocation(call.CallIdNumber, fullPath);
                                foundCall = true;
                            }
                            else if (session.FileExists($"{defaultPath}{call.VoxFileId}{format}"))
                            {
                                DA.UpdateVoxFileLocation(call.CallIdNumber, defaultPath);
                                foundCall = true;
                            }
                        }
                        else if (session.FileExists($"{location}{call.VoxFileId}{format}")) //making sure it is still there
                        {
                            foundCall = true;
                        }
                        else
                        {
                            foundCall = false; //File was moved at some point after first reconcile
                        }

                        folderNumberToCheck++;
                    }
                    if (!foundCall)
                    {
                        PLR.AddNotification(string.Format("Missing Vox File for NobleCallHistoryId:{0}", call.CallIdNumber), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        DA.UpdateVoxFileLocation(call.CallIdNumber, ""); //set location to blank if it was found at one point, and no longer exists
                        foreach (var record in allCalls.Where(p => p.CallIdNumber == call.CallIdNumber))
                        {
                            record.VoxFileLocation = null;
                            record.VoxFileNotFound = true;
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Randomizes the sampling of calls for our inbound and outbound call selections
        /// </summary>
        private List<int> GenerateRandomNumber(int num, List<NobleData> calls)
        {
            int beginRange = 0;
            int endRange = calls.Count;
            Random rand = new Random();
            List<int> result = new List<int>();
            HashSet<int> check = new HashSet<int>();
            for (int i = 0; i < num; i++)
            {
                int curValue = rand.Next(beginRange, endRange);

                while (check.Contains(curValue))
                    curValue = rand.Next(beginRange, endRange);

                result.Add(calls[curValue].CallIdNumber);
                check.Add(curValue);
            }

            return result;
        }

        /// <summary>
        /// Exports a list of sample calls for FSA to request recordings from
        /// </summary>
        private void WriteToExcel(List<NobleData> outBound, List<NobleData> inBound, List<NobleData> special)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel._Workbook workBook = (Excel._Workbook)(excelApp.Workbooks.Add(Missing.Value));
            Excel._Worksheet sheet;
            workBook.Worksheets.Add();
            workBook.Worksheets.Add();
            excelApp.Visible = true;
            sheet = (Excel._Worksheet)workBook.Sheets[1];
            WriteCalls(outBound, sheet, "Out-Bound");
            sheet = (Excel._Worksheet)workBook.Sheets[2];
            WriteCalls(inBound, sheet, "In-Bound");
            sheet = (Excel._Worksheet)workBook.Sheets[3];
            WriteCalls(special, sheet, "Specialty");

            string saveAs = EnterpriseFileSystem.GetPath("FSACALLS");
#if DEBUG
            saveAs = EnterpriseFileSystem.TempFolder;  //Developers do not have access to the test folder
#endif
            saveAs = Path.Combine(saveAs, string.Format("CornerStone Calls For {0}-{1}{2}.xlsx", DateTime.Now.AddMonths(-1).Month, DateTime.Now.AddMonths(-1).Year,
                DataAccessHelper.TestMode ? string.Format("_{0}", Guid.NewGuid().ToBase64String()) : ""));
            workBook.SaveAs(saveAs);

            //Clean Up
            workBook.Close(0);
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        }

        private void WriteCalls(List<NobleData> calls, Excel._Worksheet sheet, string tab)
        {
            Excel.Range range;
            sheet.Name = tab;
            if (!calls.Any())
                return;
            int columnCount = 1;
            foreach (PropertyInfo pi in calls[0].GetType().GetProperties().Where(p => p.HasAttribute<ExcelHeader>()).ToList())
                sheet.Cells[1, columnCount++] = pi.GetCustomAttribute<ExcelHeader>().HeaderName;

            columnCount = 1;
            int rowCount = 2;//starting at 2 to account for the header
            sheet.Cells.NumberFormat = "@";
            foreach (NobleData call in calls)
            {
                sheet.Cells[rowCount, columnCount++] = call.CallIdNumber;
                sheet.Cells[rowCount, columnCount++] = call.CallDate.ToShortDateString();
                sheet.Cells[rowCount, columnCount++] = call.CalculateTime();
                sheet.Cells[rowCount, columnCount++] = tab;
                sheet.Cells[rowCount, columnCount++] = call.CSRName_AgentId;
                sheet.Cells[rowCount, columnCount++] = call.AccountNumber;

                columnCount = 1;
                rowCount++;
            }

            ////AutoFit columns A:F.
            range = sheet.get_Range("A1", "F1");
            range.EntireColumn.AutoFit();
        }
    }
}

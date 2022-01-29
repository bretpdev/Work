using Alvas.Audio;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using WinSCP;
using Excel = Microsoft.Office.Interop.Excel;

namespace FSAMTHCALL
{
    class ReturnFile
    {
        private readonly ProcessLogRun PLR;
        private readonly DataAccess DA;
        private readonly Directories D = new Directories();

        public ReturnFile(ProcessLogRun plr)
        {
            PLR = plr;
            DA = new DataAccess(PLR.ProcessLogId);
        }

        public int ProcessReturnFile()
        {
            string fileToProcess = LocateReturnFile();
            if (fileToProcess == null)
                return 1;
            //Read in list of calls we need to produce recordings for
            List<int> inbound = GetCallHistoryIds(fileToProcess, NobleData.CallType.Inbound);
            List<int> outbound = GetCallHistoryIds(fileToProcess, NobleData.CallType.OutBound);
            List<int> special = GetCallHistoryIds(fileToProcess, NobleData.CallType.Special);

            bool missingData = false;
            List<NobleData> callData = new List<NobleData>();
            Action<List<int>, NobleData.CallType> addCallData = (calls, type) =>
            {
                var dataList = GetCallData(calls, type);
                callData.AddRange(dataList);
                if (dataList.Count != calls.Count)
                    missingData = true;
            };
            addCallData(inbound, NobleData.CallType.Inbound);
            addCallData(outbound, NobleData.CallType.OutBound);
            addCallData(special, NobleData.CallType.Special);

            string selectedLocation = Path.Combine(D.SelectedDirectory, Path.GetFileName(fileToProcess));
            if (fileToProcess.ToUpper() != selectedLocation.ToUpper())
                Repeater.TryRepeatedly(() => FS.Copy(fileToProcess, selectedLocation, true));

            if (!SaveVoxFiles(callData.DistinctBy(p => new { p.VoxFileId, p.Type }).ToList()))
                missingData = true;

            string emailMessage = $"Vox files have been moved to {D.CallsDirectory} and are ready to be sent.";

            if (missingData)
                emailMessage += $" Please be advised that the application was unable to locate 1 or more vox file.  Please review Process Logs and reference Process Log Id:{PLR.ProcessLogId}";

            EmailHelper.SendMail(DataAccessHelper.TestMode, "AuditCoordination@utahsbr.edu", "FSAMTHCALL@Utahsbr.edu", "Processing Complete", emailMessage, string.Empty, EmailHelper.EmailImportance.High, true);
            Repeater.TryRepeatedly(() => FS.Delete(fileToProcess));
            return 0;
        }

        private bool SaveVoxFiles(List<NobleData> callData)
        {
            using (Session session = new Session())
            {
                SessionOptions ss = DA.GetSessionOptions("nscsvc", DA.GetNoblePassword("nscsvc"));

                session.Open(ss);
                Parallel.ForEach(callData, new ParallelOptions() { MaxDegreeOfParallelism = 10 }, call =>
                {
                    string saveDir = Path.Combine(D.SupervisorDirectory, string.Format(@"{0} {1}\", DateTime.Now.AddMonths(-1).ToString("MMMM"), DateTime.Now.Year), NobleData.TranslateEnum(call.Type)) + @"\";
                    if (!Directory.Exists(saveDir))
                        Directory.CreateDirectory(saveDir);
                    Console.WriteLine($"Processing CallId: {call.CallIdNumber}");
                    string format = call.CallDate > new DateTime(2019, 11, 8) ? ".wav" : ".vox";
                    TransferOperationResult result = session.GetFiles($"{call.VoxFileLocation}/{call.VoxFileId}{format}", saveDir);

                    if (!result.IsSuccess)
                    {
                        try
                        {
                            result.Check();
                        }
                        catch (Exception ex)
                        {
                            PLR.AddNotification($"Unable to download vox file for NobleCallHistoryId:{call.CallIdNumber}.", NotificationType.ErrorReport, NotificationSeverityType.Critical, ex);
                            call.VoxFileNotFound = true;
                        }
                    }
                    else
                    {
                        string save = Path.Combine(saveDir, string.Format($"{call.VoxFileId}_{call.CallDate:MM.dd.yy}{format}"));
                        string voxFile = Path.Combine(saveDir, (call.VoxFileId + format));
                        if (File.Exists(save))
                            File.Delete(save);

                        File.Move(Path.Combine(saveDir, (call.VoxFileId + format)), save);
                    }
                });

                string saveD = D.SupervisorDirectory;
                string saveDirIn = Path.Combine(saveD, string.Format(@"{0} {1}\", DateTime.Now.AddMonths(-1).ToString("MMMM"), DateTime.Now.Year), NobleData.TranslateEnum(NobleData.CallType.Inbound)) + @"\";
                string saveDirOut = Path.Combine(saveD, string.Format(@"{0} {1}\", DateTime.Now.AddMonths(-1).ToString("MMMM"), DateTime.Now.Year), NobleData.TranslateEnum(NobleData.CallType.OutBound)) + @"\";
                string saveDirSp = Path.Combine(saveD, string.Format(@"{0} {1}\", DateTime.Now.AddMonths(-1).ToString("MMMM"), DateTime.Now.Year), NobleData.TranslateEnum(NobleData.CallType.Special)) + @"\";

                ConvertVoxFiles(saveDirIn);
                ConvertVoxFiles(saveDirOut);
                ConvertVoxFiles(saveDirSp);
                WavToMp3(saveDirIn);
                WavToMp3(saveDirOut);
                WavToMp3(saveDirSp);
            }

            return !callData.Any(o => o.VoxFileNotFound);
        }

        /// <summary>
        /// Takes a directory of .vox files and converts them to .mp3
        /// </summary>
        private void ConvertVoxFiles(string dir)
        {
            List<string> files = Directory.GetFiles(dir).ToList();
            foreach (string voxFile in files)
            {
                if (Path.GetExtension(voxFile) != ".vox")
                    continue;
                Console.WriteLine("Converting vox file {0}", voxFile);
                Alvas.Audio.WaveFormat wf = new Alvas.Audio.WaveFormat()
                {
                    wFormatTag = Alvas.Audio.AudioCompressionManager.MuLawFormatTag,
                    nSamplesPerSec = 8000,
                    nChannels = 1,
                };
                FormatDetails[] formatList = AudioCompressionManager.GetFormatList(wf);
                IntPtr format = formatList[0].FormatHandle;
                var rr = new RawReader(File.OpenRead(voxFile), format);
                var data = rr.ReadData();
                rr.Close();
                new IntPtr(0);
                var formatPcm16bit = new IntPtr(0);
                byte[] nothing = null;
                AudioCompressionManager.ToPcm16Bit(format, data, ref formatPcm16bit, ref nothing);
                var formatMp3 = AudioCompressionManager.GetCompatibleFormat(formatPcm16bit, AudioCompressionManager.MpegLayer3FormatTag);

                var dataMp3 = AudioCompressionManager.Convert(formatPcm16bit, formatMp3, nothing, false);
                var mw = new Mp3Writer(File.Create(string.Format(@"{1}{0}.mp3", Path.GetFileNameWithoutExtension(voxFile), dir)));
                mw.WriteData(dataMp3);
                mw.Close();

                File.Delete(voxFile);
            }
        }

        private void WavToMp3(string dir)
        {
            List<string> files = Directory.GetFiles(dir).ToList();
            foreach (string wavFile in files)
            {
                if (Path.GetExtension(wavFile) != ".wav")
                    continue;

                Console.WriteLine("Converting vox file {0}", wavFile);
                string outFile = Path.Combine(Path.Combine(Path.GetDirectoryName(wavFile), Path.GetFileNameWithoutExtension(wavFile) + ".mp3"));
                string newWavFile = Path.Combine(Path.GetDirectoryName(wavFile), $"NEW_{Path.GetFileName(wavFile)}");


                using (var reader = new WaveFileReader(wavFile))
                using (var converter = WaveFormatConversionStream.CreatePcmStream(reader))
                using (var upsampler = new WaveFormatConversionStream(new NAudio.Wave.WaveFormat(16000, converter.WaveFormat.Channels), converter))
                {
                    WaveFileWriter.CreateWaveFile(newWavFile, upsampler);
                }
                Alvas.Audio.WaveFormat wf = new Alvas.Audio.WaveFormat()
                {
                    wFormatTag = Alvas.Audio.AudioCompressionManager.PcmFormatTag,
                    nSamplesPerSec = 16000,
                    nChannels = 1

                };

                Alvas.Audio.FormatDetails[] formatList = Alvas.Audio.AudioCompressionManager.GetFormatList(wf);
                IntPtr format = formatList[0].FormatHandle;
                using (var rr = new Alvas.Audio.RawReader(File.OpenRead(newWavFile), format))
                {
                    var data = rr.ReadData();

                    byte[] d2 = null;

                    using (var retMs = new MemoryStream())
                    using (var ms = new MemoryStream(data))
                    using (var rdr = new WaveFileReader(ms))
                    using (var wtr = new NAudio.Lame.LameMP3FileWriter(retMs, rdr.WaveFormat, 512))
                    {
                        rdr.CopyTo(wtr);
                        d2 = retMs.ToArray();
                    }

                    var mw = new Alvas.Audio.Mp3Writer(File.Create(string.Format(@"{0}", outFile)));

                    mw.WriteData(d2);
                    mw.Close();
                }
                File.Delete(wavFile);
                File.Delete(newWavFile);
            }


        }



        private List<NobleData> GetCallData(List<int> callIds, NobleData.CallType type)
        {
            List<NobleData> calls = new List<NobleData>();
            foreach (int call in callIds)
            {
                var value = DA.GetCallDataFromId(call);
                if (value != null)
                {
                    value.Type = type;
                    calls.Add(value);
                }
                else
                {
                    PLR.AddNotification(string.Format("Unable to locate NobleCallHistoryId:{0} in NobleCalls.NobleCallHistory on {1}", call, DataAccessHelper.TestMode ? "OPSDEV" : "UHEAASQLDB"), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                }
            }

            return calls;
        }

        private string LocateReturnFile()
        {
            string searchPattern = "Selected Calls CornerStone Calls For*";
            List<string> files = Directory.GetFiles(EnterpriseFileSystem.TempFolder, searchPattern).ToList();

            if (!files.Any())
            {
                PLR.AddNotification(string.Format("Unable to find file {0} in {1}.  Please review.", searchPattern, EnterpriseFileSystem.TempFolder), NotificationType.NoFile, NotificationSeverityType.Critical);
                return null;
            }
            else if (files.Count > 1)
            {
                PLR.AddNotification(string.Format("Multiple {0} files found in {1}.  Please review and delete the incorrect file.", searchPattern, EnterpriseFileSystem.TempFolder), NotificationType.NoFile, NotificationSeverityType.Critical);
                return null;
            }

            return files.Single();
        }

        /// <summary>
        /// Reads in an excel spreadsheet and parses out the callhistoryids to get call recordings for
        /// </summary>
        private List<int> GetCallHistoryIds(string excelFile, NobleData.CallType callType)
        {
            List<int> ids = new List<int>();
            Excel.Application excelApp = new Excel.Application();
            excelApp.Visible = true;
            Excel._Workbook workBook = excelApp.Workbooks.Open(excelFile);

            int sheetNumber = 0;
            bool found = false;

            foreach (Excel.Worksheet worksheet in workBook.Worksheets)
            {
                sheetNumber++;
                if (worksheet.Name.ToLower().Contains(callType.ToString().ToLower()))
                {
                    found = true;
                    break;
                }
            }

            if (!found)
                throw new Exception(string.Format("The input file does not appear to be in the correct format.  It is missing the sheet {0}", callType.ToString()));

            Excel._Worksheet sheet = (Excel._Worksheet)(excelApp.Sheets[sheetNumber]);
            ids.AddRange(GetCallsForSheet(sheet));
            //Clean Up
            workBook.Close(0);
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            return ids.Distinct().ToList();
        }

        private List<int> GetCallsForSheet(Excel._Worksheet sheet)
        {
            List<int> ids = new List<int>();
            for (int row = 2; sheet.Rows.CurrentRegion.EntireRow.Count >= row; row++)
            {
                string value = (sheet.Cells[row, 1] as Excel.Range).Value.ToString();
                if (value.IsPopulated())
                {
                    int? intValue = value.ToIntNullable();
                    if (intValue.HasValue)
                        ids.Add(intValue.Value);
                }
            }

            return ids;
        }
    }
}

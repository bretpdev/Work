using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ionic.Zip;
using System.Collections.Concurrent;

namespace DirectoryCompressor
{
    /// <summary>
    /// Processes and compresses directories.
    /// </summary>
    class Compressor
    {
        Log log;
        string location;
        string destination;
        public bool IsRunning { get; set; }
        public bool FinishedRunning { get; private set; }
        /// <summary>
        /// Processes and compresses the given directory.
        /// </summary>
        /// <param name="location">The directory to be processed.</param>
        /// <param name="archiveName">The name of the resulting directory where compressed files are stored.</param>
        public Compressor(Log log, string location, string destination)
        {
            this.log = log;
            this.location = location;
            this.destination = destination;
        }

        /// <summary>
        /// Runs the Process method in a new thread.  ALso catches and logs any errors.
        /// </summary>
        public void ProcessAsync()
        {
            var thread = new Thread(() =>
            {
                try
                {
                    Process();
                }
                catch (Exception e)
                {
                    log.Write(e.ToString());
                    IsRunning = false;
                    FinishedRunning = true;
                    Program.HadErrors = true;
                }
            });
            thread.Start();
        }

        /// <summary>
        /// Compress all files at the given location that are older than a month and not within the current month.
        /// </summary>
        public void Process()
        {
            if (FinishedRunning)
            {
                log.Write("Cannot use a Compressor twice.");
                return;
            }
            IsRunning = true;
            log.Write("Beginning Directory Compressor");

            DateTime cutoff = DateTime.Now.Date.AddDays(-14);
            log.Write("Looking for files last modified before {0:MM/dd/yyyy} in {1}", cutoff, location);
            List<FileInfo> archivableFiles = new List<FileInfo>();
            Dictionary<string, ConcurrentQueue<FileInfo>> archivableFilesByYearMonth = new Dictionary<string, ConcurrentQueue<FileInfo>>();
            bool findingFiles = true;
            Thread fileFinder = new Thread(() =>
            {
                foreach (string file in Directory.EnumerateFiles(location))
                {
                    var info = new FileInfo(file);
                    if (info.LastWriteTime < cutoff)
                    {
                        string yearMonth = info.LastWriteTime.ToString("yyyyMM");
                        if (!archivableFilesByYearMonth.ContainsKey(yearMonth))
                            archivableFilesByYearMonth[yearMonth] = new ConcurrentQueue<FileInfo>();
                        archivableFilesByYearMonth[yearMonth].Enqueue(info);
                        log.FoundFile();
                    }
                    if (!IsRunning)
                    {
                        findingFiles = false;
                        return;
                    }
                }
                log.DoneFindingFiles();
                findingFiles = false;
            });
            fileFinder.Start();

            int processedFileCount = 0;
            while (findingFiles || archivableFilesByYearMonth.Sum(o => o.Value.Count) > 0)
            {
                foreach (var key in archivableFilesByYearMonth.Keys.ToArray())
                {
                    List<FileInfo> infos = new List<FileInfo>();
                    FileInfo tryInfo;
                    while ((archivableFilesByYearMonth[key].TryDequeue(out tryInfo)))
                        infos.Add(tryInfo);
                    if (infos.Any())
                    {
                        if (!IsRunning)
                        {
                            log.Write("Process aborted.");
                            return;
                        }
                        string zipLocation = null;
                        try
                        {
                            zipLocation = CalculateCompressionLocation(infos.First().LastWriteTime);
                        }
                        catch (Exception ex)
                        {
                            log.Write("Error attempting to create archive folder.  Process aborted.  {0}", ex.ToString());
                            Program.HadErrors = true;
                            return;
                        }
                        try
                        {
                            log.Write("Processing " + zipLocation);
                            using (var zipFile = new ZipFile(zipLocation))
                            {
                                foreach (var file in infos)
                                {
                                    if (zipFile.ContainsEntry(Path.GetFileName(file.FullName)))
                                        zipFile.RemoveEntry(Path.GetFileName(file.FullName));
                                    zipFile.AddFile(file.FullName, "");
                                }
                                zipFile.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                                zipFile.UseZip64WhenSaving = Zip64Option.AsNecessary; //otherwise the zip can't hold more than 65k entries.
                                zipFile.Save();
                            }
                            log.Write("Added {0} files to {1}", infos.Count, zipLocation);
                        }
                        catch (Exception ex)
                        {
                            log.Write("Error attempting to access/create zip file at {0}.  Exception: {1}", zipLocation, ex.ToString());
                            Program.HadErrors = true;
                            continue;
                        }
                        log.Write("Cleaning up {0}", zipLocation);
                        foreach (var file in infos)
                            try
                            {
                                File.Delete(file.FullName);
                                processedFileCount++;
                            }
                            catch (Exception ex)
                            {
                                log.Write("Error attempting to delete processed file {0}. Exception: {1}", file.FullName, ex.ToString());
                                Program.HadErrors = true;
                                continue;
                            }
                        log.Write("Cleanup finished for {0}.", zipLocation);
                    }
                }
            }
            log.Write("Finished");
            IsRunning = false;
            FinishedRunning = true;
        }

        private string CalculateCompressionLocation(DateTime date)
        {
            string yearFolder = Path.Combine(destination, date.Year.ToString());
            if (!Directory.Exists(yearFolder))
                Directory.CreateDirectory(yearFolder);
            string monthFile = Path.Combine(yearFolder, date.Month + "_" + date.ToString("MMMM", CultureInfo.InvariantCulture) + "_" + date.Year + ".zip"); //August_2013.zip, etc.
            return monthFile;
        }
    }
}

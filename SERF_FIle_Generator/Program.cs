using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;
using System.IO;
using System.Threading;
using Uheaa.Common.DataAccess;
using System.Threading.Tasks;
using System.Dynamic;
using Uheaa.Common;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SERF_File_Generator
{
    class Program
    {
        public const string SerfFileFormat = @"C:\SERF_File\{2}SERF_AlignToUheaa_{0}_{1:MMddyyyyhhmmss}.txt";
        public static string GenerateSerfFileName(string owner)
        {
            string serfFile = string.Format(Program.SerfFileFormat, owner.Trim(), DateTime.Now, DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live ? "TEST_" : "");
            return serfFile;
        }
        private const bool DistributedMode = false;
        private static List<string> FilterSsns(IEnumerable<string> ssns)
        {
            
            return ssns.Where(p => p == "491042783" || p == "486707225").ToList();
        }
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            if (!DataAccessHelper.StandardArgsCheck(args))
                return;

            if (DistributedMode)
            {
                using (var setup = new DistributedJobSetup())
                {
                    if (setup.ShowDialog() == DialogResult.OK)
                    {
                        Form master = null;
                        Form child = null;
                        if (setup.IsNewJob)
                        {
                            List<string> achBorrowers = DataAccessHelper.ExecuteList<string>("GetAllBorrowersByLenderCode", DataAccessHelper.Database.AlignImport, "826717".ToSqlParameter("LenderCode")).Where(p => p != "382703671").ToList();
                            List<string> nonAchBorrowers = DataAccessHelper.ExecuteList<string>("GetAllBorrowersByLenderCode", DataAccessHelper.Database.AlignImport, "830248".ToSqlParameter("LenderCode")).Where(p => p != "382703671").ToList();
                            int fileCount = 0;
                            fileCount += DistributedJobHelper.PopulateJobScaffold(setup.JobInfo, "826717  ", achBorrowers);
                            fileCount += DistributedJobHelper.PopulateJobScaffold(setup.JobInfo, "830248  ", nonAchBorrowers);
                            master = new DistributedServer(setup.JobInfo, DateTime.Now, fileCount);
                        }
                        if (master == null)
                            master = new DistributedClient(setup.JobInfo);
                        else
                        {
                            child = new DistributedClient(setup.JobInfo);
                            child.Show();
                        }
                        Application.Run(master);
                    }
                }
                return;
            }
            else
            {
                //PrepareFile();

                Stopwatch s = new Stopwatch();
                s.Start();

                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

                List<string> achBorrowers = DataAccessHelper.ExecuteList<string>("GetAllBorrowersByLenderCode", DataAccessHelper.Database.AlignImport, "826717".ToSqlParameter("LenderCode"));
                achBorrowers = FilterSsns(achBorrowers);
                List<string> nonAchBorrowers = DataAccessHelper.ExecuteList<string>("GetAllBorrowersByLenderCode", DataAccessHelper.Database.AlignImport, "830248".ToSqlParameter("LenderCode"));
                nonAchBorrowers = FilterSsns(nonAchBorrowers);
                Console.WriteLine("Processing...");
                var t1 = Task.Factory.StartNew(() => StartProcessing(achBorrowers, "826717  "), TaskCreationOptions.LongRunning);
                var t2 = Task.Factory.StartNew(() => StartProcessing(nonAchBorrowers, "830248  "), TaskCreationOptions.LongRunning);

                Task.WhenAll(t1).Wait();
                Task.WhenAll(t2).Wait();
                //Console.WriteLine(@"Saving files to X:\Archive\ALIGN\Test SERF Files...");
                //string[] files = Directory.GetFiles(@"C:\SERF_File\");
                //foreach (string file in files)
                //{
                //    File.Copy(file, Path.Combine(@"X:\Archive\ALIGN\Test SERF Files", Path.GetFileName(file)));
                //}
                s.Stop();
                Console.WriteLine("Total execution time:  {0} minutes", s.ElapsedMilliseconds / 60000.00);
                Console.Read();
            }
        }

        //private static void PrepareFile()
        //{
        //    if (Directory.Exists(Path.GetDirectoryName(SerfFile)))
        //        Directory.Delete(Path.GetDirectoryName(SerfFile), true);

        //    Directory.CreateDirectory(Path.GetDirectoryName(SerfFile));
        //}

        private static void StartProcessing(List<string> borrowers, string owner)
        {
            Stopwatch stopwatch = new Stopwatch();
            int borrowerCount = borrowers.Count();
            int counter = 0;
            double averageProcessingTime = 0;

            stopwatch.Start();
            ClassFileWriter fileWriter = StartFileWriterProcess(owner);

            Parallel.ForEach(borrowers, new ParallelOptions { MaxDegreeOfParallelism = int.MaxValue }, borrower =>
            {
                counter++;

                RecordGenerator.CreateFiles(borrower, owner, fileWriter);

                averageProcessingTime = stopwatch.ElapsedMilliseconds / counter / 60000.00;
                Console.Clear();
                Console.WriteLine("{1} of {2} borrowers processed.\nEstimated run time: {0} minutes.", Math.Round(averageProcessingTime * borrowerCount, 2), counter, borrowerCount);

            });

            Thread.Sleep(7000);
            fileWriter.Stop();
        }

        private static ClassFileWriter StartFileWriterProcess(string owner)
        {
            ClassFileWriter file = new ClassFileWriter(GenerateSerfFileName(owner));
            Thread backgroundThread = new Thread(new ThreadStart(file.Process));
            backgroundThread.Name = "File Writer Thread";
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
            return file;
        }
    }
}

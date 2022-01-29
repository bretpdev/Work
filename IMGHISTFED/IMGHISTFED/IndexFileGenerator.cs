using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Ionic.Zip;

namespace IMGHISTFED
{
    public class IndexFileGenerator
    {
        public string MainDirectory { get; set; }
        public List<string> FileNames { get; set; }
        public List<string> Directories { get; set; }
        public string TimeStamp { get; set; } = DateTime.Now.ToString("yyyyMMddHHmmss");


        const string SendingServicerCode = "700502";
        const string SendingLenderCode = "898502";
        public List<DealIds> DealIds { get; set; }
        public IndexFileGenerator(string directory)
        {
            MainDirectory = directory;
            DealIds = GetDealIds();
            CreateOrRecoverFiles();
            CreateOrRecoverDirectories();
        }

        public void CreateOrRecoverFiles()
        {
            DirectoryInfo info = new DirectoryInfo(MainDirectory);
            var files = info.GetFiles("*.idx");

            if (files.Length > 0)
            {
                FileNames = files.Select(f => f.Name).ToList();
            }
            else
            {
                FileNames = new List<string>();
                foreach(var deal in DealIds)
                {
                    string fileName = $"COLL_{SendingServicerCode}_{SendingLenderCode}_{deal.DealId}_{deal.SaleDate.ToDate().ToString("yyyyMMdd")}_{TimeStamp}.idx";
                    FileNames.Add(fileName);
                    FS.Create($"{MainDirectory}{fileName}");
                }
            }
        }

        public void CreateOrRecoverDirectories()
        {
            DirectoryInfo info = new DirectoryInfo(MainDirectory);
            var directories = info.GetDirectories($"COLL_{SendingServicerCode}{SendingLenderCode}*");

            if (directories.Length > 0)
            {
                Directories = directories.Select(d => d.Name).ToList();
            }
            else
            {
                Directories = new List<string>();
                foreach (var deal in DealIds)
                {
                    string directoryName = $"COLL_{SendingServicerCode}_{SendingLenderCode}_{deal.DealId}_{deal.SaleDate.ToDate().ToString("yyyyMMdd")}_{TimeStamp}";
                    Directories.Add(directoryName);
                    FS.CreateDirectory($"{MainDirectory}{directoryName}");
                }
            }
        }

        public bool WriteIndexLineForBorrower(BorrowerIndexRecord b, string filename)
        {
            if(b != null)
            {
                WriteLine($"{b.Ssn}|{b.LastName}|{b.FirstName}|{b.DocType}|{b.LoanId}|{b.DocDate}|{b.LoanProgramType}|{b.GuarantyDate.ToDate().ToString("MM/dd/yyyy")}|{b.SaleDate}|{b.DealId}|{filename}",new DealIds() { DealId = b.DealId, SaleDate = b.SaleDate});
                return true;
            }
            return false;
        }

        private void WriteLine(string line, DealIds deal)
        {
            var filePath = $"{MainDirectory}{FileNames.Where(f => f.Contains(deal.DealId) && f.Contains(deal.SaleDate.ToDate().ToString("yyyyMMdd"))).First()}";

            using (var sw = new StreamW(filePath, true))
            {
                sw.WriteLine(line);
            }
        }

        public void MoveFile(BorrowerIndexRecord b, string file)
        {
            if (b != null)
            {
                string directory = Directories.Where(d => d.Contains(b.DealId) && d.Contains(b.SaleDate.ToDate().ToString("yyyyMMdd"))).FirstOrDefault();
                if (!directory.IsNullOrEmpty())
                {
                    FS.Move(file, $"{MainDirectory}{directory}\\{Path.GetFileName(file)}");
                }
            }
        }

        public BorrowerIndexRecord GetIndexDataForBorrower(string ssn)
        {
            var borrowerIndexRecord = DataAccessHelper.ExecuteList<BorrowerIndexRecord>("[dbo].GetBorrowerIndexRecord", DataAccessHelper.Database.Cls, DataAccessHelper.CurrentMode, SqlParams.Single("Ssn", ssn)).FirstOrDefault();
            return borrowerIndexRecord;
        }

        public BorrowerIndexRecord GetIndexDataForEndorser(string ssn, string endorserSsn)
        {
            var borrowerIndexRecord = DataAccessHelper.ExecuteList<BorrowerIndexRecord>("[dbo].GetEndorserIndexRecord", DataAccessHelper.Database.Cls, DataAccessHelper.CurrentMode, SqlParams.Single("Ssn", ssn), SqlParams.Single("EndorserSsn", endorserSsn)).FirstOrDefault();
            return borrowerIndexRecord;
        }

        public List<DealIds> GetDealIds()
        {
            return DataAccessHelper.ExecuteList<DealIds>("[imghistfed].GetDealIds", DataAccessHelper.Database.Cls, DataAccessHelper.CurrentMode);
        }

        public void ZipFiles()
        {
            foreach(string filename in FileNames)
            {
                var result = Directories.Where(d => filename.StartsWith(d)).FirstOrDefault();
                if(result != null)
                {
                    FS.Move($"{MainDirectory}{filename}", $"{MainDirectory}{result}\\{filename}");
                }
            }

            foreach(string str in Directories)
            {
                using (var zip = new Ionic.Zip.ZipFile())
                {
                    zip.AddDirectory($"{MainDirectory}{str}", str);
                    zip.Save($"{MainDirectory}{str}.zip");
                }
            }
        }

        public void DeleteDirectories()
        {
            foreach (string str in Directories)
            {
                if(Directory.Exists($"{MainDirectory}{str}"))
                {
                    Directory.Delete($"{MainDirectory}{str}", true);
                }
            }
        }

    }
}

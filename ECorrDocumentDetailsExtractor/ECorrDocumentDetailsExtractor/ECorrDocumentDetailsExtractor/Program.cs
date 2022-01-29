using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common;

namespace ECorrDocumentDetailsExtractor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int skipDays = args.FirstOrDefault().ToIntNullable() ?? 7;
            if (!DataAccessHelper.ModeSet)//Need this to allow for existing runs
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
            if (!DataAccessHelper.RegionSet)//Needed for a stand alone run (it will only run the fed side)
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;

            Console.WriteLine("Ignoring all records within the last {0} days.", skipDays);

            Console.WriteLine("Processing {0}...", DataAccessHelper.CurrentRegion);
            Process(DataAccessHelper.CurrentRegion, EnterpriseFileSystem.GetPath("EcorrArchive"), EnterpriseFileSystem.GetPath("ECORRLocation"), skipDays);
        }

        static void Process(DataAccessHelper.Region region, string zipLocation, string extractionLocation, int skipDays)
        {
            var data = new DataAccess(region);
            var results = data.GetPaths(skipDays);
            Console.WriteLine("Found {0} records to process.", results.Count);
            Console.WriteLine("Looking for zip files in {0} and extracting results to {1}.", zipLocation, extractionLocation);

            var helper = new ZipHelper(zipLocation, extractionLocation, data);
            int successCount = 0;
            int errorCount = 0;
            int existingCount = 0;
            int notFoundCount = 0;
            foreach (var result in helper.Process(results))
            {
                if (result == ZipHelper.ExtractResult.AlreadyExists)
                    existingCount++;
                else if (result == ZipHelper.ExtractResult.Success)
                    successCount++;
                else if (result == ZipHelper.ExtractResult.Error)
                    errorCount++;
                else if (result == ZipHelper.ExtractResult.NotFound)
                    notFoundCount++;
                decimal totalCount = errorCount + successCount + existingCount + notFoundCount;
                if (totalCount % 100 == 0 || totalCount == results.Count)
                    Console.WriteLine("{0}/{1} ({2:#.##}%): {3} Successful, {6} Not Found, {4} Errors, {5} Existing.", totalCount, results.Count, (totalCount / results.Count) * 100, successCount, errorCount, existingCount, notFoundCount);
            }
        }
    }
}


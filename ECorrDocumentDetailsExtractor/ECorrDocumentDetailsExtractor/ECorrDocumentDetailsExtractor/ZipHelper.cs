using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECorrDocumentDetailsExtractor
{
    public class ZipHelper
    {
        string zipLocation;
        string destination;
        DataAccess data;
        public ZipHelper(string zipLocation, string destination, DataAccess data)
        {
            this.zipLocation = zipLocation;
            this.destination = destination;
            this.data = data;
        }

        public IEnumerable<ExtractResult> Process(List<DocumentDetails> resultsToProcess)
        {
            var lookup = resultsToProcess.ToDictionary(o => Path.GetFileName(o.Path), o => o);

            foreach (var zipFile in GetZipPaths())
                using (var archive = ZipFile.Read(zipFile))
                    foreach (var entry in archive.Entries)
                    {
                        if (lookup.ContainsKey(entry.FileName))
                        {
                            var id = lookup[entry.FileName].DocumentDetailsId;
                            lookup.Remove(entry.FileName);
                            yield return Extract(entry, Path.Combine(destination, entry.FileName), id);
                        }
                    }
            foreach (var remaining in lookup.Keys)
            {
                var dest = Path.Combine(destination, remaining);
                if (File.Exists(dest))
                {
                    data.NullPrinted(lookup[remaining].DocumentDetailsId);
                    yield return ExtractResult.AlreadyExists;
                }
                else
                {
                    Console.WriteLine("Could not find {0}", dest);
                    yield return ExtractResult.NotFound;
                }
            }
        }

        public enum ExtractResult
        {
            NotFound,
            Success,
            Error,
            AlreadyExists
        }

        public ExtractResult Extract(ZipEntry entry, string destination, int documentDetailsId)
        {
            try
            {
                if (File.Exists(destination))
                {
                    data.NullPrinted(documentDetailsId);
                    return ExtractResult.AlreadyExists;
                }

                using (var writer = File.OpenWrite(destination))
                    entry.Extract(writer);
                data.NullPrinted(documentDetailsId);

            }
            catch (Exception)
            {
                return ExtractResult.Error;
            }
            return ExtractResult.Success;
        }

        private IEnumerable<string> GetZipPaths()
        {
            int year = DateTime.Now.Year;
            string path = "";
            while (Directory.Exists(path = Path.Combine(zipLocation, year.ToString())))
            {
                foreach (var file in ZipMonths(path))
                    yield return file;
                year--;
            }
        }


        IEnumerable<string> ZipMonths(string path)
        {
            foreach (var file in System.IO.Directory.GetFiles(path, "*.zip").OrderByDescending(o => o))
            {
                yield return file;
            }
        }
    }
}

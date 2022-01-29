using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace UsBankImport
{
    public class ZipProcessor
    {
        private readonly string zipLocation;
        private readonly Locations locations;
        private readonly Log log;
        public ZipProcessor(string zipLocation, Locations locations, Log log)
        {
            this.zipLocation = zipLocation;
            this.locations = locations;
            this.log = log;
        }

        const string csvName = "DEN-561480.csv";
        /// <summary>
        /// Process the given zip file.
        /// </summary>
        public bool Process()
        {
            using (var zip = new ZipFile(zipLocation))
            {
                var csvFile = zip.SingleOrDefault(z => z.FileName.EndsWith(csvName));
                if (csvFile == null)
                {
                    log.ZipHasNoCsvFile(zipLocation);
                    return false;
                }

                var contents = csvFile.ReadAllLines();
                if (contents.Count <= finalHeaderLoc) //header row, data for header row, blank line, data header row
                {
                    log.ZipHasEmptyCsv(zipLocation);
                    return false;
                }
                DateTime docDate = GetOutputDate(contents);
                var batchItems = ParseBatchItems(contents);
                var batchGroups = batchItems.GroupBy(o => o.Group);
                foreach (var batchGroup in batchGroups)
                {
                    List<string> newNames = new List<string>();
                    List<string> imageNames = new List<string>();
                    foreach (var batchItem in batchGroup)
                    {
                        //These need to be put together in a very specific order, first field followed by second field per row.
                        imageNames.Add(batchItem.CheckImage);
                        imageNames.Add(batchItem.InvoiceImage);
                    }
                    imageNames = imageNames.Where(o => !string.IsNullOrEmpty(o)).Distinct().ToList();
                    foreach (string imageName in imageNames.Where(o => !string.IsNullOrEmpty(o)))
                    {
                        var entry = zip.SingleOrDefault(o => Path.GetFileNameWithoutExtension(o.FileName) == imageName);
                        if (entry == null)
                            log.ImageNotFound(zipLocation, imageName);
                        else
                        {
                            string extension = Path.GetExtension(entry.FileName);
                            entry.FileName = imageName + "_" + Guid.NewGuid().ToString() + extension;
                            entry.Extract(locations.ImageLocation);
                            string newPath = Path.Combine(locations.ImageLocation, entry.FileName);
                            log.ExtractedImage(imageName, newPath);
                            newNames.Add(newPath);
                        }
                    }
                    WriteControlFile(batchGroup.Key, docDate, newNames);
                }
            }
            string destination = Path.Combine(locations.ArchiveLocation, Path.GetFileName(zipLocation));
            if (File.Exists(destination))
                destination = Path.Combine(locations.ArchiveLocation, Guid.NewGuid() + "_" + Path.GetFileName(zipLocation));
            File.Move(zipLocation, destination);
            log.EndZip(zipLocation, destination);
            return true;
        }
        int finalHeaderLoc = 4;

        /// <summary>
        /// Parse the given CSV lines into BatchItem objects.
        /// </summary>
        private List<BatchItem> ParseBatchItems(List<string> csvFileContents)
        {
            var header = csvFileContents[finalHeaderLoc - 1].Split(',').Select((o, i) => new { Index = i, Value = o }).ToDictionary(o => o.Value, o => o.Index);
            List<BatchItem> items = new List<BatchItem>();
            foreach (string line in csvFileContents.Skip(finalHeaderLoc))
            {
                string[] parts = CsvHelper.Parse(line);
                Func<string, string> get = (s) => parts[header[s]];
                BatchItem item = new BatchItem()
                {
                    BatchItemNumber = get("Batch Item"),
                    CheckImage = get("Check Image"),
                    Group = get("Group"),
                    InvoiceImage = get("Invoice Image"),
                    GroupName = get("Group Name")
                };
                if (item.ValidGroup())
                    items.Add(item);
            }
            return items;
        }

        /// <summary>
        /// Generate and output the control file for a given group and list of files.
        /// </summary>
        private void WriteControlFile(string groupId, DateTime docDate, List<string> newNames)
        {
            string docDateString = docDate.ToString("MM/dd/yyyy");
            string docId = "LPDEP";
            string batchNum = "G" + groupId.PadLeft(4, '0');
            string vendorNum = "LOCKBOX";
            List<string> lines = new List<string>();
            lines.Add("~^Folder~Commercial Packet^Type~C_TYPE^Attribute~ACCOUNT_NUMBER~STR~^Attribute~BATCH_NUM~STR~" + batchNum + 
                "^Attribute~DOC_DATE~STR~" + docDateString + 
                "^Attribute~DOC_ID~STR~" + docId + 
                "^Attribute~DOC_TIME~STR~^Attribute~LENDER_CODE~STR~^Attribute~MSG~STR~^Attribute~SCAN_DATE~STR~^Attribute~SSN~STR~^Attribute~VENDOR_NUM~STR~" + vendorNum + 
                "^Attribute~DESCRIPTION~STR~Commercial Packet");
            lines.Add("ImageDoc~Scanned Form(s)" +
                "^Type~C_TYPE^Attribute~MSG~STR~^Attribute~SCAN_DATE~STR~^Attribute~SSN~STR~^Attribute~VENDOR_NUM~STR~" + vendorNum + 
                "^Attribute~ACCOUNT_NUMBER~STR~^Attribute~BATCH_NUM~STR~" + batchNum + 
                "^Attribute~DOC_DATE~STR~" + docDateString + 
                "^Attribute~DOC_ID~STR~" + docId + "^Attribute~DOC_TIME~STR~^Attribute~LENDER_CODE~STR~");
            foreach (string newName in newNames)
                lines.Add("ImageFile~" + newName);

            string fileName = Path.Combine(locations.ImageLocation, groupId + "_" + Guid.NewGuid().ToString() + ".ctl");
            File.WriteAllLines(fileName, lines.ToArray());
            log.WroteControlFile(fileName);
        }

        /// <summary>
        /// Determine the output date from the second line of the csv file.
        /// </summary>
        private DateTime GetOutputDate(List<string> csvContents)
        {
            string[] headerParts = csvContents[0].Split(',');
            int loc = headerParts.Select((o, i) => new { Index = i, Header = o }).Single(o => o.Header == "Output Date").Index;
            string date = csvContents[1].Split(',')[loc];
            return new DateTime(int.Parse(date.Substring(0, 4)), int.Parse(date.Substring(4, 2)), int.Parse(date.Substring(6, 2)));
        }
    }
}

using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Uheaa.Common;

namespace IMGCLIMPRT
{
    public class ZipProcessor
    {
        private readonly string zipLocation;
        private readonly Locations locations;
        private readonly Log log;
        private readonly string scriptId;
        public ZipProcessor(string zipLocation, Locations locations, Log log, string scriptId)
        {
            this.zipLocation = zipLocation;
            this.locations = locations;
            this.log = log;
            this.scriptId = scriptId;
        }

        /// <summary>
        /// Process the given zip file.
        /// </summary>
        public bool Process()
        {
            using (var zip = new ZipFile(zipLocation))
            {
                var xmlFile = zip.SingleOrDefault(z => z.FileName.EndsWith(".xml"));
                if (xmlFile == null)
                {
                    log.ZipHasNoXmlFile(zipLocation);
                    return false;
                }

                List<BatchItem> batchItems = new List<BatchItem>();
                using (var stream = xmlFile.OpenReader())
                    batchItems = ParseBatchItems(stream);
                var types = string.Join(Environment.NewLine, batchItems.Select(o => o.DocumentType).Distinct());
                foreach (var batchItem in batchItems)
                {
                    if (string.IsNullOrEmpty(batchItem.DocId))
                    {
                        log.DocumentTypeNotFound(zipLocation, batchItem.DocumentType);
                        continue;
                    }
                    string imageName = batchItem.DocumentFileName;
                    var entry = zip.SingleOrDefault(o => Path.GetFileName(o.FileName) == imageName);
                    if (entry == null)
                        log.ImageNotFound(zipLocation, imageName);
                    else
                    {
                        string extension = Path.GetExtension(entry.FileName);
                        var tifHelper = new TifHelper();
                        if (!tifHelper.IsMultiPageTif(entry))
                        {
                            entry.FileName = Path.GetFileNameWithoutExtension(imageName) + "_" + Guid.NewGuid().ToString() + extension;
                            entry.Extract(locations.ImageLocation);
                            string newPath = Path.Combine(locations.ImageLocation, entry.FileName);
                            log.ExtractedImage(imageName, newPath);
                            WriteControlFile(newPath, batchItem);
                        }
                        else
                        {
                            List<string> tifFiles = new List<string>();
                            foreach (var file in tifHelper.ExplodeTifs(entry, locations.ImageLocation))
                            {
                                log.ExtractedImage(imageName, file);
                                tifFiles.Add(Path.Combine(locations.ImageLocation, file));
                            }
                            WriteControlFile(tifFiles.ToArray(), batchItem);

                        }
                    }
                }
            }
            string destination = Path.Combine(locations.ArchiveLocation, Path.GetFileName(zipLocation));
            if (File.Exists(destination))
                destination = Path.Combine(locations.ArchiveLocation, Guid.NewGuid() + "_" + Path.GetFileName(zipLocation));
            File.Move(zipLocation, destination);
            log.EndZip(zipLocation, destination);
            return true;
        }

        /// <summary>
        /// Parse the given xml file into BatchItem objects.
        /// </summary>
        private List<BatchItem> ParseBatchItems(Stream xmlStream)
        {
            List<BatchItem> items = new List<BatchItem>();
            BatchItem current = null;
            using (var reader = new XmlTextReader(xmlStream))
            {
                reader.Read(); //XML Declaration
                reader.Read(); //<CampusDoor> root element
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.EndElement)
                        continue;
                    if (reader.Name == "Loan")
                    {
                        current = new BatchItem() { LoanId = reader.GetAttribute("ID") };
                        items.Add(current);
                    }
                    else if (reader.Name.IsPopulated())
                    {
                        var propName = reader.Name;
                        var property = typeof(BatchItem).GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        if (property != null)
                        {
                            var setter = property.GetSetMethod();
                            reader.Read(); //navigate to value
                            setter.Invoke(current, new object[] { reader.Value });
                        }
                    }
                }
            }
            return items;
        }


        /// <summary>
        /// Generate and output the control file for a given group and list of files.
        /// </summary>
        private void WriteControlFile(string itemLocation, BatchItem item)
        {
            WriteControlFile(new string[] { itemLocation }, item);
        }

        /// <summary>
        /// Generate and output the control file for a given group and list of files.
        /// </summary>
        private void WriteControlFile(IEnumerable<string> itemLocations, BatchItem item)
        {
            List<string> lines = new List<string>();
            string fileName = Path.Combine(locations.ImageLocation, scriptId + "_" + Guid.NewGuid().ToString() + ".ctl");
            File.WriteAllText(fileName, GetFileLines(itemLocations, item));
            log.WroteControlFile(fileName);
        }

        private string GetFileLines(IEnumerable<string> itemLocations, BatchItem item)
        {
            string controlTemplate = null;
            string prepend = null;
            if (new string[] { "pdf", "doc", "docx", "txt", "htm", "html" }.Contains(
                item.DocumentFileName.Split('.').Last().ToLower()))
            {
                controlTemplate =
@"~^Folder~{1}, Doc 1^Type~UTCR_TYPE^Attribute~SSN~STR~{3}^Attribute~LENDER_CODE~STR~^Attribute~ACCOUNT_NUM~STR~^Attribute~DOC_ID~STR~{4}^Attribute~DOC_DATE~STR~{2}^Attribute~BATCH_NUM~STR~COMPLT^Attribute~VENDOR_NUM~STR~^Attribute~SCAN_DATE~STR~^Attribute~SCAN_TIME~STR~^Attribute~DESCRIPTION~STR~{1}
{0}~{1}, Doc 1^Type~UTCR_TYPE^Attribute~SSN~STR~{3}^Attribute~LENDER_CODE~STR~^Attribute~ACCOUNT_NUM~STR~^Attribute~DOC_ID~STR~{4}^Attribute~DOC_DATE~STR~{2}^Attribute~BATCH_NUM~STR~COMPLT^Attribute~VENDOR_NUM~STR~^Attribute~SCAN_DATE~STR~^Attribute~SCAN_TIME~STR~";
                prepend = "DesktopDoc~";
            }
            else
            {
                controlTemplate =
@"~^Folder~Commercial Packet^Type~C_TYPE^Attribute~ACCOUNT_NUMBER~STR~^Attribute~BATCH_NUM~STR~COMPLT^Attribute~DOC_DATE~STR~{2}^Attribute~DOC_ID~STR~{4}^Attribute~DOC_TIME~STR~^Attribute~LENDER_CODE~STR~^Attribute~MSG~STR~^Attribute~SCAN_DATE~STR~^Attribute~SSN~STR~{3}^Attribute~VENDOR_NUM~STR~^Attribute~DESCRIPTION~STR~Commercial Packet
ImageDoc~Scanned Form(s)
{0}~Scanned Form(s)^Type~C_TYPE^Attribute~MSG~STR~^Attribute~SCAN_DATE~STR~^Attribute~SSN~STR~{3}^Attribute~VENDOR_NUM~STR~^Attribute~ACCOUNT_NUMBER~STR~^Attribute~BATCH_NUM~STR~COMPLT^Attribute~DOC_DATE~STR~{2}^Attribute~DOC_ID~STR~{4}^Attribute~DOC_TIME~STR~^Attribute~LENDER_CODE~STR~";
            prepend = "ImageFile~";
            }
            var parsedDate = item.DocumentDate.ToDate();
            string date = parsedDate.ToString("MM/dd/yyyy");
            string dateTime = date + " " + parsedDate.ToString("hh:mm:ss tt");
            string fileLines = string.Join(Environment.NewLine, itemLocations.Select(o => prepend + o));// string.Join(prepend + Environment.NewLine, itemLocations);
            controlTemplate = string.Format(controlTemplate, fileLines, dateTime, date, item.Ssn, item.DocId);
            return controlTemplate;
        }
    }
}

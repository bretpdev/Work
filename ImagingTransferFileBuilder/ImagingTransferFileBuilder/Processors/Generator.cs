using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using Ionic.Zip;

namespace ImagingTransferFileBuilder
{
    public static class Generator
    {
        public static void Generate(string resultLocation, string excelLocation, List<string> loadLocations, string sheetNameText, bool clearFiles)
        {
            Results.Clear();
            Progress.Start("Generator");
            Dictionary<string, Borrower> borrowers = ExcelHelper.GetBorrowers(excelLocation, sheetNameText, false);
            if (borrowers == null)
                return;  //couldn't find all the column
            if (clearFiles)
            {
                foreach (string file in Directory.GetFiles(resultLocation))
                    File.Delete(file);
                foreach (string directory in Directory.GetDirectories(resultLocation))
                    Directory.Delete(directory, true);
            }
            Dictionary<string, List<string>> indexFiles = AllIndexFiles(loadLocations);
            List<FileToCopy> filesToCopy = new List<FileToCopy>();
            Progress.Increments = borrowers.Count + indexFiles.Keys.Count;
            foreach (Borrower b in borrowers.Values)
            {
                filesToCopy.AddRange(ProcessBorrower(b, resultLocation, indexFiles));
                Progress.Increment();
            }
            foreach (string load in indexFiles.Keys)
            {
                CopyFiles(load, filesToCopy);
                Progress.Increment();
            }
            Progress.Finish();
        }

        private struct FileToCopy
        {
            public string ZipPath { get; set; }
            public string ZipInnerPath { get; set; }
            public string Destination { get; set; }
        }
        private static Dictionary<string, List<string>> AllIndexFiles(List<string> loadLocations)
        {
            Dictionary<string, List<string>> results = new Dictionary<string, List<string>>();
            foreach (string load in loadLocations)
            {
                foreach (string file in Directory.GetFiles(load, "*.ZIP", SearchOption.TopDirectoryOnly))
                {
                    using (ZipFile zip = new ZipFile(file))
                    {
                        var indexes = zip.Where(z => z.FileName.ToLower().EndsWith(".idx"));
                        if (indexes.Count() > 1)
                        {
                            Results.LogError("Multiple index files in load file {0}", file);
                            continue;
                        }
                        if (indexes.Count() == 0)
                        {
                            Results.LogError("No index files in load file {0}", file);
                            continue;
                        }
                        ZipEntry index = indexes.Single();
                        MemoryStream stream = new MemoryStream();
                        index.Extract(stream);
                        stream.Seek(0, SeekOrigin.Begin);
                        StreamReader sr = new StreamReader(stream);
                        string[] lines = sr.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        stream.Dispose();
                        sr.Dispose();
                        results[file] = new List<string>(lines);
                    }
                }
            }
            return results;
        }
        private static List<FileToCopy> ProcessBorrower(Borrower b, string resultLocation, Dictionary<string, List<string>> indexFiles)
        {
            List<FileToCopy> files = new List<FileToCopy>();
            string borrowerDirectory = Path.Combine(resultLocation, b.FirstName + " " + b.LastName);
            List<string> indexLines = new List<string>();
            Directory.CreateDirectory(borrowerDirectory);
            foreach (string load in indexFiles.Keys)
            {
                foreach (string line in indexFiles[load])
                {
                    if (line.StartsWith(b.SSN))
                    {

                        string[] lineParts = line.Split('|');
                        lineParts[4] = b.LoanID;
                        lineParts[7] = b.GuarantyDate;
                        string fileName = lineParts[lineParts.Length - 1];
                        if (indexLines.Where(idx => idx.ToLower().EndsWith(fileName.ToLower())).Count() > 0)
                        {
                            Results.LogError("Index file in load {0} references file {1}.  This file was already found a previous load.", load, fileName);
                        }
                        else
                        {
                            indexLines.Add(string.Join("|", lineParts));
                            FileToCopy file = new FileToCopy();
                            file.ZipPath = load;
                            file.ZipInnerPath = fileName;
                            file.Destination = borrowerDirectory;
                            files.Add(file);
                        }
                    }
                }
            }
            if (indexLines.Count == 0)
                Results.LogError(string.Format("No Index files were found for borrower {0} {1} ssn {2}", b.FirstName, b.LastName, b.SSN));
            string indexFile = Path.Combine(borrowerDirectory, Util.Code() + ".IDX");
            File.WriteAllLines(indexFile, indexLines.Distinct().ToArray());
            return files;
        }
        private static void CopyFiles(string load, List<FileToCopy> files)
        {
            using (ZipFile zip = new ZipFile(load))
            {
                foreach (FileToCopy file in files.Where(f => f.ZipPath == load))
                {
                    ZipEntry image = zip.Where(z => z.FileName.ToLower().EndsWith(file.ZipInnerPath.ToLower())).SingleOrDefault();
                    if (image != null)
                    {
                        string imageFilePath = Path.Combine(file.Destination, Path.GetFileName(file.ZipInnerPath));
                        if (!File.Exists(imageFilePath))
                        {
                            using (FileStream fs = File.Create(imageFilePath))
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    image.Extract(ms);
                                    ms.Seek(0, SeekOrigin.Begin);
                                    byte[] bytes = new byte[ms.Length];
                                    ms.Read(bytes, 0, (int)ms.Length);
                                    fs.Write(bytes, 0, (int)ms.Length);
                                }
                            }
                        }
                        else
                        {
                            Results.LogError(string.Format("Tried to copy file {0} from zip file {1} but {2} already exists.", file.ZipInnerPath, load, imageFilePath));
                        }
                    }
                    else
                    {
                        Results.LogError(string.Format("Image file not found: {0}.  Was searching in zip file: {1}.", file.ZipInnerPath, load));
                    }
                }
            }

        }

        ///// <summary>
        ///// Cycles through zip, pulls out related index file rows and copies images files to appropriate directory
        ///// </summary>
        //private static List<string> ProcessZip(Borrower b, string zipFile, string borrowerDirectory)
        //{
        //    List<string> indexRows = new List<string>();
        //    using (ZipFile zip = new ZipFile(zipFile))
        //    {
        //        ZipEntry index = zip.Where(z => z.FileName.ToLower().EndsWith(".idx")).Single();
        //        MemoryStream stream = new MemoryStream();
        //        index.Extract(stream);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        StreamReader sr = new StreamReader(stream);
        //        string[] lines = sr.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        //        stream.Dispose();
        //        sr.Dispose();

        //        foreach (string line in lines)
        //        {

        //        }
        //    }
        //    return indexRows;
        //}
    }

}

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;

namespace BILLING
{
    class PrintAndImage
    {
        private const string PrintingOnlyFile = "Print";
        private const string ImageOnlyFile = "Image";
        private List<string> LetterIds { get; set; }

        public static string ImagingFolder { get; set; }
        private DataAccess DA { get; set; }

        /// <summary>
        /// Gets all the letter id's and files needed for printing.
        /// </summary>
        public PrintAndImage(DataAccess da)
        {
            DA = da;
            LetterIds = DA.GetAllLetterIds().Result;
            foreach (string docid in LetterIds)
            {
                List<string> files = Directory.GetFiles(EnterpriseFileSystem.TempFolder).Where(p => p.Contains(docid)).ToList();
                foreach (string file in files)
                    Repeater.TryRepeatedly(() => File.Delete(file));//There is no recovery for printing so delete anything that is there
            }
        }

        /// <summary>
        /// Print the repayment disclosures
        /// </summary>
        /// <param name="borrowers"></param>
        public void PrintAndImageBwrs(List<Borrower> borrowers)
        {
            List<string> filesToProcess = CreateDataFiles(borrowers);
            Print(filesToProcess.Where(p => !p.Contains(ImageOnlyFile)).ToList());
            DataTable ids = Borrower.GetPrintProcessingIds(borrowers);
            DA.SetPrintComplete(ids);//Since printing is an all or nothing we can mass update the print processing table
            DA.SetDocumentCreated(ids);
            Image(filesToProcess.Where(p => !p.Contains(PrintingOnlyFile)).ToList());
            DA.SetImageComplete(ids);//Since Imaging is an all or nothing we can mass update the print processing table

            foreach (string file in filesToProcess)//Clean up the files
                Repeater.TryRepeatedly(() => File.Delete(file));
            if (borrowers.Count > 0)
                Console.WriteLine("Printing Complete");
        }

        /// <summary>
        /// Creates an image for each borrower in each of the files to process
        /// </summary>'
        /// <param name="filesToProcess">all the files that have borrower information</param>
        private void Image(List<string> filesToProcess)
        {
            //UNDONE I am not sure if we will image if we do then this can be uncommented. Some additional work will be needed.
            /*foreach (string file in filesToProcess)
            {
                int index = (file.IndexOf('.') + 1);//Getting the DocId from the file
                int length = file.LastIndexOf('.') - index;
                DocumentProcessing.ImageDocs(ScriptId, "DF_SPE_ACC_ID", file.Substring(index, length), Path.GetExtension(file).Remove(0, 1), file, DocumentProcessing.LetterRecipient.Borrower, new ImagingGenerator(ScriptId, "UT00801"));
            }*/
        }

        /// <summary>
        /// Sends the files to process to CostCenterPrinting to be printed
        /// </summary>
        /// <param name="filesToProcess">files containing borrower information to be merged into the word doc and printed</param>
        private void Print(List<string> filesToProcess)
        {
            foreach (string file in filesToProcess)
            {
                Console.WriteLine(string.Format("Now printing file: {0}", file));
                DocumentProcessing.CostCenterPrinting(Path.GetExtension(file).Remove(0, 1), file, "STATE", Program.ScriptId, "DF_SPE_ACC_ID", DocumentProcessing.LetterRecipient.Borrower, DocumentProcessing.CostCenterOptions.AddBarcode, "COST_CENTER_CODE");

                if (file.Contains(PrintingOnlyFile))//Deletes the print only file Imaging is already done
                    Repeater.TryRepeatedly(() => File.Delete(file));
            }

        }

        /// <summary>
        /// Creates a datafile to print the cover sheet
        /// </summary>
        private List<string> CreateDataFiles(List<Borrower> borrowers)
        {
            List<string> filesToProcesses = new List<string>();
            foreach (Borrower bor in borrowers)
            {
                if ((bor.DocumentCreatedAt == null || bor.PrintedAt == null) && bor.ImagedAt == null)
                    filesToProcesses.Add(AddToDataFile(bor));
                else if ((bor.DocumentCreatedAt == null || bor.PrintedAt == null) && bor.ImagedAt != null)
                    filesToProcesses.Add(AddToDataFile(bor, PrintingOnlyFile));
                else if ((bor.DocumentCreatedAt != null || bor.PrintedAt != null) && bor.ImagedAt == null)
                    filesToProcesses.Add(AddToDataFile(bor, ImageOnlyFile));
                else
                    continue;//The arc needs to be done but will be done in another thread
            }

            return filesToProcesses = filesToProcesses.Distinct().ToList();
        }

        /// <summary>
        /// Adds Creates the data files that will contain all the borrower information for the specific bill type
        /// </summary>
        /// <param name="bor">The borrower being added to the data file</param>
        /// <param name="step">The step specifying whether or not the document will be printed, imaged or all</param>
        /// <returns>string: the name of the file</returns>
        private string AddToDataFile(Borrower bor, string step = "All")
        {
            string file = string.Format("{0}\\{1}.{2}", Program.BillingFolder, step, bor.LetterId);
            if (!File.Exists(file))
                PrepMergeFile(file, bor.FileHeader);

            using (StreamWriter sw = new StreamWriter(file, true))
                sw.WriteLine(bor.LineData);

            return file;
        }

        /// <summary>
        /// Create the file and add the header row
        /// </summary>
        /// <param name="file">The name of the file to create</param>
        /// <param name="header">The header row to add to the file</param>
        private void PrepMergeFile(string file, string header)
        {
            using (StreamWriter sw = new StreamWriter(file))
                sw.WriteLine(header);
        }
    }
}
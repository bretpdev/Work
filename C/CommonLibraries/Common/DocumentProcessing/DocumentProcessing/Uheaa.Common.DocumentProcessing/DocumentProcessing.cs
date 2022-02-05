using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Word = Microsoft.Office.Interop.Word;
//using IDAutomation.Windows.Forms.DataMatrixBarcode;
using DMATRIXLib;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Microsoft.Office.Interop.Word;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Uheaa.Common.DocumentProcessing
{
    public static class DocumentProcessing
    {
        public enum LetterRecipient
        {
            Borrower = 0,
            Reference = 1,
            Other = 2
        }

        public enum CentralizedPrintingDeploymentMethod
        {
            UserPrompt = 0,
            Fax = 1,
            Email = 2,
            Letter = 3
        }

        public enum CostCenterOptions
        {
            None = 0,
            AddBarcode = 1
        }

        public enum ACSKeyLineAddressType
        {
            Legal,
            Alternate,
            Temporary
        }

        public static string ACSKeyLine(string ssn, LetterRecipient personType, ACSKeyLineAddressType addressType)
        {
            string person = string.Empty;
            if (personType == LetterRecipient.Borrower) { person = "P"; }
            else { person = "R"; }

            string address = string.Empty;
            if (addressType == ACSKeyLineAddressType.Legal) { address = "L"; }
            else if (addressType == ACSKeyLineAddressType.Alternate) { address = "A"; }
            else { address = "T"; }

            string encodedSsn = string.Empty;

            if (personType == LetterRecipient.Borrower)
            {
                string nextLetter = string.Empty;

                for (int i = 0; i <= 8; i++)
                {
                    switch (int.Parse(ssn.Substring(i, 1)))
                    {
                        case 1:
                            nextLetter = "R";
                            break;
                        case 2:
                            nextLetter = "E";
                            break;
                        case 3:
                            nextLetter = "T";
                            break;
                        case 4:
                            nextLetter = "H";
                            break;
                        case 5:
                            nextLetter = "G";
                            break;
                        case 6:
                            nextLetter = "U";
                            break;
                        case 7:
                            nextLetter = "A";
                            break;
                        case 8:
                            nextLetter = "L";
                            break;
                        case 9:
                            nextLetter = "Y";
                            break;
                        case 0:
                            nextLetter = "M";
                            break;
                    }
                    encodedSsn += nextLetter;
                }
            }
            else
            {
                encodedSsn = ssn.Substring(0, 2) + "/" + ssn.Substring(3, 6);
            }

            string workingKeyline = person + encodedSsn + DateTime.Now.ToString("MMdd") + address;

            int checkDigit = 0;
            for (int i = 0; i <= workingKeyline.Length - 1; i++)
            {
                int keylineBitValue = 0;
                switch (workingKeyline.Substring(i, 1))
                {
                    case "A":
                        keylineBitValue = 1;
                        break;
                    case "B":
                        keylineBitValue = 2;
                        break;
                    case "C":
                        keylineBitValue = 3;
                        break;
                    case "D":
                        keylineBitValue = 4;
                        break;
                    case "E":
                        keylineBitValue = 5;
                        break;
                    case "F":
                        keylineBitValue = 6;
                        break;
                    case "G":
                        keylineBitValue = 7;
                        break;
                    case "H":
                        keylineBitValue = 8;
                        break;
                    case "I":
                        keylineBitValue = 9;
                        break;
                    case "J":
                        keylineBitValue = 10;
                        break;
                    case "K":
                        keylineBitValue = 11;
                        break;
                    case "L":
                        keylineBitValue = 12;
                        break;
                    case "M":
                        keylineBitValue = 13;
                        break;
                    case "N":
                        keylineBitValue = 14;
                        break;
                    case "O":
                        keylineBitValue = 15;
                        break;
                    case "P":
                        keylineBitValue = 0;
                        break;
                    case "Q":
                        keylineBitValue = 1;
                        break;
                    case "R":
                        keylineBitValue = 2;
                        break;
                    case "S":
                        keylineBitValue = 3;
                        break;
                    case "T":
                        keylineBitValue = 4;
                        break;
                    case "U":
                        keylineBitValue = 5;
                        break;
                    case "V":
                        keylineBitValue = 6;
                        break;
                    case "W":
                        keylineBitValue = 7;
                        break;
                    case "X":
                        keylineBitValue = 8;
                        break;
                    case "Y":
                        keylineBitValue = 9;
                        break;
                    case "Z":
                        keylineBitValue = 10;
                        break;
                    case "/":
                        keylineBitValue = 15;
                        break;
                    default:
                        keylineBitValue = int.Parse(workingKeyline.Substring(i, 1));
                        break;
                }

                if ((i % 2) == 0) { keylineBitValue *= 2; }

                while (keylineBitValue.ToString().Length > 1)
                {
                    string keylineBit = keylineBitValue.ToString();
                    keylineBitValue = 0;
                    for (int x = 0; x <= keylineBit.Length - 1; x++)
                    {
                        keylineBitValue = int.Parse(keylineBit.Substring(x, 1));
                    }
                }

                checkDigit += keylineBitValue;

                checkDigit = checkDigit == 10 ? 0 : checkDigit;

            }//end for

            checkDigit = 10 - (checkDigit % 10);

            return string.Format("#{0}{1}#", workingKeyline, checkDigit.ToString());
        }

        /// <summary>
        /// Adds 2D barcode fields and the static date field to the given data file.
        /// </summary>
        /// <param name="testMode">Test mode indicator</param>
        /// <param name="dataFile">Data file to add fields too</param>
        /// <param name="acctFieldName">The name of the account number field (from header row).</param>
        /// <param name="letterId">The letter id of the document</param>
        /// <param name="createNewFile">bool to create a new file or append to data file</param>
        /// <param name="letterRecip">The letter recipient</param>
        /// <returns>file path for text file with the barcodes added</returns>
        public static string AddBarcodesForBatchProcessing(string dataFile, string acctFieldName, string letterId, bool createNewFile, LetterRecipient letterRecip)
        {
            string barcodeFile = string.Format("{0}Add Return Mail Barcode Temp {1}_{2}.txt", EnterpriseFileSystem.TempFolder, letterId, Guid.NewGuid().ToString());
            string newFileName = string.Empty;
            BarcodeQueryResults queryResults = DataAccess.GetPageCount(letterId);

            if (queryResults == null)
            {
                throw new BarcodeException("The letter ID that the script is using doesn't appear to exist.  Please contact a member of Systems Support");
            }

            int pageCount = FigureSheetCount(queryResults);
            if (pageCount < 1 || pageCount > 8) { throw new BarcodeException("The paper sheet count for the letter ID that the script is using isn't populated.  Please contact a member of Systems Support"); }

            using (StreamReader sr = new StreamR(dataFile))
            {
                List<string> originalFields = sr.ReadLine().SplitAndRemoveQuotes(",");

                int acctNumIndex = 0;
                if (letterRecip != LetterRecipient.Other) { acctNumIndex = originalFields.IndexOf(acctFieldName); }

                List<string> barcodedFields = new List<string>();
                if (letterRecip != LetterRecipient.Other) { barcodedFields.AddRange(new List<string>() { "BC1", "BC2", "BC3", "BC4", "BC5", "BC6" }); }
                barcodedFields.AddRange(new List<string>() { "SMBC1", "SMBC2", "SMBC3", "SMBC4" });

                if (pageCount > 1) { barcodedFields.AddRange(new List<string>() { "SMBC_Pg2_Ln1", "SMBC_Pg2_Ln2", "SMBC_Pg2_Ln3", "SMBC_Pg2_Ln4" }); }
                if (pageCount > 2) { barcodedFields.AddRange(new List<string>() { "SMBC_Pg3_Ln1", "SMBC_Pg3_Ln2", "SMBC_Pg3_Ln3", "SMBC_Pg3_Ln4" }); }
                if (pageCount > 3) { barcodedFields.AddRange(new List<string>() { "SMBC_Pg4_Ln1", "SMBC_Pg4_Ln2", "SMBC_Pg4_Ln3", "SMBC_Pg4_Ln4" }); }
                if (pageCount > 4) { barcodedFields.AddRange(new List<string>() { "SMBC_Pg5_Ln1", "SMBC_Pg5_Ln2", "SMBC_Pg5_Ln3", "SMBC_Pg5_Ln4" }); }
                if (pageCount > 5) { barcodedFields.AddRange(new List<string>() { "SMBC_Pg6_Ln1", "SMBC_Pg6_Ln2", "SMBC_Pg6_Ln3", "SMBC_Pg6_Ln4" }); }
                if (pageCount > 6) { barcodedFields.AddRange(new List<string>() { "SMBC_Pg7_Ln1", "SMBC_Pg7_Ln2", "SMBC_Pg7_Ln3", "SMBC_Pg7_Ln4" }); }
                if (pageCount > 7) { barcodedFields.AddRange(new List<string>() { "SMBC_Pg8_Ln1", "SMBC_Pg8_Ln2", "SMBC_Pg8_Ln3", "SMBC_Pg8_Ln4" }); }
                barcodedFields.Add("StaticCurrentDate");
                barcodedFields.AddRange(originalFields);

                using (StreamWriter sw = new StreamW(barcodeFile, false) { AutoFlush = true })
                {
                    sw.WriteCommaDelimitedLine(true, barcodedFields.ToArray());
                    char[] barcodeDelimiter = { '\r', '\n' };
                    long dataFileLineNum = 0;
                    while (!sr.EndOfStream)
                    {
                        originalFields = sr.ReadLine().SplitAndRemoveQuotes(",");
                        string acctNumber = originalFields[acctNumIndex].Replace(" ", "");
                        if (letterRecip == LetterRecipient.Reference) { acctNumber += " "; }
                        barcodedFields = new List<string>();
                        if (letterRecip != LetterRecipient.Other)
                        {

                            //foreach (string returnMailBarcode in Encode(string.Format("{0}{1}{2:MMddyyyy}", acctNumber, letterId.PadLeft(10), DateTime.Now), false, EncodingModes.Ascii, PreferredFormats.Auto).Split(barcodeDelimiter).Where(p => !string.IsNullOrEmpty(p)))
                            foreach (string returnMailBarcode in Encode(string.Format("{0}{1}{2:MMddyyyy}", acctNumber, letterId.PadLeft(10), DateTime.Now), 0, 0, 0).Split(barcodeDelimiter).Where(p => !string.IsNullOrEmpty(p)))
                            {
                                barcodedFields.Add(returnMailBarcode);
                            }
                        }

                        for (int pageNum = 1; pageNum <= pageCount; pageNum++)
                        {
                            int leadingDigit = pageNum == 1 ? 1 : 0;
                            //foreach (string stateMailBarcode in Encode(string.Format("{0}{1}0{2}{3:00000#}", leadingDigit, pageCount, pageNum, dataFileLineNum), false, EncodingModes.Ascii, PreferredFormats.Auto).Split(barcodeDelimiter).Where(p => !string.IsNullOrEmpty(p)))
                            foreach (string stateMailBarcode in Encode(string.Format("{0}{1}0{2}{3:00000#}", leadingDigit, pageCount, pageNum, dataFileLineNum), 0, 0, 0).Split(barcodeDelimiter).Where(p => !string.IsNullOrEmpty(p)))
                            {
                                barcodedFields.Add(stateMailBarcode);
                            }
                        }

                        barcodedFields.Add(DateTime.Now.ToString("MM/dd/yyyy"));
                        barcodedFields.AddRange(originalFields);
                        sw.WriteCommaDelimitedLine(barcodedFields.ToArray());
                    }
                }
            }

            if (createNewFile)
            {
                newFileName = barcodeFile;
            }
            else
            {
                FS.Delete(dataFile);
                FS.Copy(barcodeFile, dataFile);
                newFileName = dataFile;
                FS.Delete(barcodeFile);

            }

            return newFileName;
        }

        public static List<string> GetReturnMailBarcodes(string accountNumber, string letterId, LetterRecipient letterRecip)
        {
            List<string> barcodedFields = new List<string>();

            char[] barcodeDelimiter = { '\r', '\n' };

            //foreach (string returnMailBarcode in Encode(string.Format("{0}{1}{2:MMddyyyy}", accountNumber, letterId.PadLeft(10), DateTime.Now), false, EncodingModes.Ascii, PreferredFormats.Auto).Split(barcodeDelimiter).Where(p => !string.IsNullOrEmpty(p)))
            foreach (string returnMailBarcode in Encode(string.Format("{0}{1}{2:MMddyyyy}", accountNumber, letterId.PadLeft(10), DateTime.Now), 0, 0, 0).Split(barcodeDelimiter).Where(p => !string.IsNullOrEmpty(p)))
                barcodedFields.Add(returnMailBarcode);


            return barcodedFields;
        }

        public static List<string> GetStateMailBarcodesforPdf(string accountNumber, int numberOfPages, LetterRecipient letterRecip)
        {
            List<string> barcodedFields = new List<string>();

            char[] barcodeDelimiter = { '\r', '\n' };
            long dataFileLineNum = 0;

            for (int pageNum = 1; pageNum <= numberOfPages; pageNum++)
            {
                int leadingDigit = pageNum == 1 ? 1 : 0;
                //foreach (string stateMailBarcode in Encode(string.Format("{0}{1}0{2}{3:00000#}", leadingDigit, numberOfPages, pageNum, dataFileLineNum), false, EncodingModes.Ascii, PreferredFormats.Auto).Split(barcodeDelimiter).Where(p => !string.IsNullOrEmpty(p)))
                foreach (string stateMailBarcode in Encode(string.Format("{0}{1}0{2}{3:00000#}", leadingDigit, numberOfPages, pageNum, dataFileLineNum), 0, 0, 0).Split(barcodeDelimiter).Where(p => !string.IsNullOrEmpty(p)))
                    barcodedFields.Add(stateMailBarcode);
            }

            return barcodedFields;
        }

        public static List<string> GetBarcodeList(string data, string acctNumber, string letterId, LetterRecipient letterRecip)
        {
            string barcodeFile = string.Format("{0}Add Return Mail Barcode Temp {1}_{2}.txt", EnterpriseFileSystem.TempFolder, letterId, Guid.NewGuid().ToString());
            string newFileName = string.Empty;
            BarcodeQueryResults queryResults = DataAccess.GetPageCount(letterId);

            //TODO figure out a better way
            if (queryResults == null)
                throw new BarcodeException("The letter ID that the script is using doesn't appear to exist.  Please contact a member of Systems Support");

            int pageCount = FigureSheetCount(queryResults);

            //TODO figure out a better way
            if (pageCount < 1 || pageCount > 7)
                throw new BarcodeException("The paper sheet count for the letter ID that the script is using isn't populated.  Please contact a member of Systems Support");
            List<string> barcodedFields = new List<string>();

            char[] barcodeDelimiter = { '\r', '\n' };
            long dataFileLineNum = 0;

            //foreach (string returnMailBarcode in Encode(string.Format("{0}{1}{2:MMddyyyy}", acctNumber, letterId.PadLeft(10), DateTime.Now), false, EncodingModes.Ascii, PreferredFormats.Auto).Split(barcodeDelimiter).Where(p => !string.IsNullOrEmpty(p)))
            foreach (string returnMailBarcode in Encode(string.Format("{0}{1}{2:MMddyyyy}", acctNumber, letterId.PadLeft(10), DateTime.Now), 0, 0, 0).Split(barcodeDelimiter).Where(p => !string.IsNullOrEmpty(p)))
                barcodedFields.Add(returnMailBarcode);

            for (int pageNum = 1; pageNum <= pageCount; pageNum++)
            {
                int leadingDigit = pageNum == 1 ? 1 : 0;
                //foreach (string stateMailBarcode in Encode(string.Format("{0}{1}0{2}{3:00000#}", leadingDigit, pageCount, pageNum, dataFileLineNum), false, EncodingModes.Ascii, PreferredFormats.Auto).Split(barcodeDelimiter).Where(p => !string.IsNullOrEmpty(p)))
                foreach (string stateMailBarcode in Encode(string.Format("{0}{1}0{2}{3:00000#}", leadingDigit, pageCount, pageNum, dataFileLineNum), 0, 0, 0).Split(barcodeDelimiter).Where(p => !string.IsNullOrEmpty(p)))
                    barcodedFields.Add(stateMailBarcode);
            }

            barcodedFields.Add(DateTime.Now.ToShortDateString());

            return barcodedFields;
        }

        /// <summary>
        /// Checks the Sprocs marked with the UsesSproc flag.
        /// </summary>
        /// <returns>A list of UserSprocAttributes for any sproc that the user does not have access to.</returns>
        public static List<SprocValidationResult> CheckSprocAccess()
        {
            return DatabaseAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Generates a message with all the missing sprocs
        /// </summary>
        /// <returns></returns>
        public static string GenerateSprocAccessAlert()
        {
            return DatabaseAccessHelper.GenerateSprocAccessAlert(Assembly.GetExecutingAssembly());
        }

        public static string GenerateCentralizedPrintingDocument(string letterId, string dataFile, string accountNumberField, string stateCodeField, SystemBorrowerDemographics borrowerDemos, DataAccessHelper.Region? region = null)
        {
            CentralizedPrintingAnd2DBarcodeInfo procDat = DataAccess.GetUserInfoForCentralizedPrintAnd2DObject();

            DocumentPathAndName pathAndName = GetDocumentPathAndFileName(letterId, region);

            //Check to see if the user has the letter on their T drive if they do delete it.
            if (File.Exists(string.Format("{0}{1}", EnterpriseFileSystem.TempFolder, pathAndName.CalculatedFileName)))
                FS.Delete(string.Format("{0}{1}", EnterpriseFileSystem.TempFolder, pathAndName.CalculatedFileName));

            //We want to copy the word doc to the T drive to do the mail merge so that multiple users can process this code with the same letter at the same time.
            FS.Copy(string.Format("{0}{1}", pathAndName.CalculatedPath, pathAndName.CalculatedFileName), string.Format("{0}{1}", EnterpriseFileSystem.TempFolder, pathAndName.CalculatedFileName), true);
            pathAndName.CalculatedPath = EnterpriseFileSystem.TempFolder;

            LetterRecordCreationResults results = DataAccess.CreateLetterRecordForCentralizedPrinting(letterId, borrowerDemos.AccountNumber, procDat.UsersBusinessUnit, GetStateCode(dataFile, stateCodeField), region);
            AddBarcodesForBatchProcessing(dataFile, accountNumberField, letterId, false, LetterRecipient.Borrower);

            //NOTE currently we only send letters to be printed there is no functionality to send as an email or fax on Federal
            string saveAs = string.Format("{0}{1}_{2}.doc", EnterpriseFileSystem.GetPath("Central Print", region), letterId, results.NewRecordIdentity.ToString());
            SaveDocs(letterId, dataFile, saveAs, true);

            return saveAs;
        }

        public static string GeneratePdfDocument(string letterId, string dataFile, string saveAs, bool generateBarcodes = false, string acctNumField = "")
        {
            if (generateBarcodes)
                dataFile = DocumentProcessing.AddBarcodesForBatchProcessing(dataFile, acctNumField, letterId, false, LetterRecipient.Borrower);

            DocumentPathAndName letterInfo = GetDocumentPathAndFileName(letterId);

            if (!letterInfo.CalculatedFileName.EndsWith(".doc") && !letterInfo.CalculatedFileName.EndsWith(".docx"))
                throw new Exception(string.Format("Invalid file extension for letter {0}", letterId));

            string local = EnterpriseFileSystem.TempFolder;
            string dir = Path.Combine(local, Guid.NewGuid().ToString());
            FS.CreateDirectory(dir);

            string fileAndName = string.Format("{0}{1}", letterInfo.CalculatedPath, letterInfo.CalculatedFileName.Replace(" ", ""));

            FS.Copy(fileAndName, Path.Combine(dir, letterInfo.CalculatedFileName.Replace(" ", "")));

            fileAndName = Path.Combine(dir, letterInfo.CalculatedFileName.Replace(" ", ""));

            string mergedSaveAs = EnterpriseFileSystem.TempFolder + letterId + @"\" + Path.GetFileNameWithoutExtension(saveAs) + string.Format("_{0}.pdf", Guid.NewGuid().ToBase64String());

            object fileName = fileAndName;
            object missing = System.Type.Missing;
            object refFalse = false;
            object refTrue = true;
            object mergeType = Word.WdMergeSubType.wdMergeSubTypeOther;
            object pause = false;
            object saveAsPath = mergedSaveAs;
            object format = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;

            Word.Application wordApp = new Word.Application();
            wordApp.DisplayAlerts = WdAlertLevel.wdAlertsNone;

            DoMailMerge(wordApp, dataFile, true, pause, saveAsPath, fileName, missing, refFalse, refTrue, mergeType, format);

            object saveChanges = Word.WdSaveOptions.wdDoNotSaveChanges;
            ((_Application)wordApp.Application).Quit(ref saveChanges, ref missing, ref missing);

            Forms form = HasForm(letterId);
            if (form != null)
            {
                List<string> file = new List<string>() { mergedSaveAs };
                file.Add(form.PathAndForm);
                PdfHelper.MergePdfs(file, saveAs);
            }

            Repeater.TryRepeatedly(() => FS.Delete(dir, true));

            return Path.GetFileName(saveAs);
        }

        public static Forms HasForm(string letterId)
        {
            try
            {
                return DataAccessHelper.ExecuteSingle<Forms>("GetFormAndPathForLetterId", DataAccessHelper.Database.Csys, letterId.ToSqlParameter("LetterId"));
            }
            catch (InvalidOperationException ex)
            {
                //This exception is thrown if execute single do not return a result.
                return null;
            }

        }

        /// <summary>
        /// Will decrypt the ACSKeyline and return the SSN
        /// </summary>
        /// <param name="keyline">Keyline to decrypt</param>
        /// <returns>Borrowers SSN.  If cannot decrypt Keyline will return null.</returns>
        public static string GetSsnFromKeyline(string keyline)
        {
            string ssn = string.Empty;

            for (int characterPosition = 0; characterPosition <= 8; characterPosition++)
            {
                switch (keyline.Substring(characterPosition, 1).ToUpper())
                {
                    case "M":
                        ssn += "0";
                        break;
                    case "Y":
                        ssn += "9";
                        break;
                    case "L":
                        ssn += "8";
                        break;
                    case "A":
                        ssn += "7";
                        break;
                    case "U":
                        ssn += "6";
                        break;
                    case "G":
                        ssn += "5";
                        break;
                    case "H":
                        ssn += "4";
                        break;
                    case "T":
                        ssn += "3";
                        break;
                    case "E":
                        ssn += "2";
                        break;
                    case "R":
                        ssn += "1";
                        break;
                    default:
                        return null;
                }
            }

            return ssn;
        }

        #region Cost Center Printing

        internal static void GenerateLettersWithForms(string letterId, string dataFile, string acctNumFieldName)
        {
            string dir = EnterpriseFileSystem.TempFolder + letterId + @"\";
            using (StreamReader sr = new StreamR(dataFile))
            {
                string header = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string tempDataFile = Path.Combine(dir, string.Format("{0}_{1:MMddyyyyhhmmss}.txt", letterId, DateTime.Now));
                    using (StreamWriter sw = new StreamW(tempDataFile))
                    {
                        sw.WriteLine(header);
                        sw.WriteLine(sr.ReadLine());
                    }

                    string pdfFile = Path.Combine(dir, string.Format("{0}_{1:MMddyyyyhhmmss}.pdf", letterId, DateTime.Now));
                    GeneratePdfDocument(letterId, tempDataFile, pdfFile, false, acctNumFieldName);

                    new Thread(() => PrintPdfWithExitCode(pdfFile)).Start();
                    Thread.Sleep(15000);//Wait for the PDF to print before deleting the file.

                    DeleteFile(tempDataFile);
                }
            }

            DeleteFile(dataFile);
        }

        internal static void DeleteFile(string file)
        {
            int count = 0;
            while (File.Exists(file))
            {
                try
                {
                    FS.Delete(file);
                }
                catch (IOException)
                {
                    continue;
                }
                catch (UnauthorizedAccessException)
                {
                    if (count == 1)
                    {
                        throw new UnauthorizedAccessException();
                    }
                    Thread.Sleep(5000);
                    count++;
                }
            }
        }

        public static int PrintPdfWithExitCode(string file)
        {
            var startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Verb = "print";
            startInfo.RedirectStandardOutput = true;

            //Define location of adobe reader/command line
            //switches to launch adobe in "print" mode
            startInfo.FileName = EnterpriseFileSystem.GetPath("AcrobatReader");
            if (!File.Exists(startInfo.FileName))
            {
                double version = startInfo.FileName.SplitAndRemoveQuotes("\\")[3].Replace("Acrobat ", "").ToDouble();
                startInfo.FileName = startInfo.FileName.Replace(version.ToString(), (++version).ToString());
            }
            startInfo.Arguments = String.Format(@"/T /N /H /P {0}", file);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            var proc = Process.Start(startInfo);

            int id = proc.Id;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.EnableRaisingEvents = true;

            if (!proc.HasExited)
                proc.CloseMainWindow();

            Thread.Sleep(15000);
            proc.WaitForExit(10000);

            int exitCode = proc.ExitCode;
            //Repeater.TryRepeatedly(() => KillProcessById(id));
            return exitCode;
        }

        public static void PrintPdf(string file)
        {
            var startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Verb = "print";
            startInfo.RedirectStandardOutput = true;

            //Define location of adobe reader/command line
            //switches to launch adobe in "print" mode
            startInfo.FileName = EnterpriseFileSystem.GetPath("AcrobatReader");
            if (!File.Exists(startInfo.FileName))
            {
                double version = startInfo.FileName.SplitAndRemoveQuotes("\\")[3].Replace("Acrobat ", "").ToDouble();
                startInfo.FileName = startInfo.FileName.Replace(version.ToString(), (++version).ToString());
            }
            startInfo.Arguments = String.Format(@"/T /N /H /P {0}", file);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            var proc = Process.Start(startInfo);

            int id = proc.Id;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.EnableRaisingEvents = true;
            Thread.Sleep(15000);
            proc.WaitForExit(10000);

            proc.Close();

            //Repeater.TryRepeatedly(() => KillProcessById(id));
        }

        private static bool KillProcessById(int id)
        {
            foreach (Process clsProcess in Process.GetProcesses().Where(clsProcess => clsProcess.Id == id))
            {
                clsProcess.Kill();
                return true;
            }
            return false;
        }

        internal static bool IsFileEmpty(string file, bool hasHeader)
        {
            using (StreamReader sr = new StreamR(file))
            {
                if (!hasHeader)
                {
                    sr.ReadLine();
                    return sr.EndOfStream;
                }
                else
                {
                    sr.ReadLine();
                    return sr.EndOfStream;
                }
            }
        }

        public static void CostCenterPrintingFormMerge(string letterId, string dataFile, string stateCodeFieldName, string scriptId, string acctNumFieldName, LetterRecipient letterRecipient, CostCenterOptions options, bool copyLetterLocal, string costCenterFieldName = "")
        {
            CostCenterPrintingFormMerge(letterId, dataFile, stateCodeFieldName, scriptId, acctNumFieldName, letterRecipient, options, true, copyLetterLocal, costCenterFieldName);
        }

        public static void CostCenterPrintingFormMerge(string letterId, string dataFile, string stateCodeFieldName, string scriptId, string acctNumFieldName, LetterRecipient letterRecipient, CostCenterOptions options, bool doPrintCoverSheet, bool copyLetterLocal, string costCenterFieldName = "")
        {
            PrintingDialog d = new PrintingDialog(letterId);
            d.StartDisplay();

            string barcodedData = AddBarcodes(dataFile, acctNumFieldName, letterId, letterRecipient, options);

            //UNDONE we current do not have any UHEAA letters that will have forms merge someone will need to implement this when the time comes. For now it will just use the regular process.
            UheaaCostCenterPrinting(scriptId, letterId, barcodedData, costCenterFieldName, stateCodeFieldName, doPrintCoverSheet);

            d.EndDisplay();
        }

        internal static string AddBarcodes(string dataFile, string acctNumFieldName, string letterId, LetterRecipient letterRecipient, CostCenterOptions options)
        {
            //Keeping the variables that are used in both regions outside the switch
            string barcodedData = dataFile;
            if (options == CostCenterOptions.AddBarcode)
                barcodedData = AddBarcodesForBatchProcessing(dataFile, acctNumFieldName, letterId, true, letterRecipient);

            return barcodedData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="letterId"></param>
        /// <param name="dataFile"></param>
        /// <param name="stateCodeFieldName"></param>
        /// <param name="scriptId"></param>
        /// <param name="acctNumFieldName"></param>
        /// <param name="letterRecipient"></param>
        /// <param name="options"></param>
        public static void CostCenterPrinting(string letterId, string dataFile, string stateCodeFieldName, string scriptId, string acctNumFieldName, LetterRecipient letterRecipient, CostCenterOptions options, string costCenterFieldName = "")
        {
            CostCenterPrinting(letterId, dataFile, stateCodeFieldName, scriptId, acctNumFieldName, letterRecipient, options, true, costCenterFieldName);
        }

        public static void CostCenterPrinting(string letterId, string dataFile, string stateCodeFieldName, string scriptId, string acctNumFieldName, LetterRecipient letterRecipient, CostCenterOptions options, bool doPrintCoverSheet, string costCenterFieldName = "")
        {
            CostCenterPrintingFormMerge(letterId, dataFile, stateCodeFieldName, scriptId, acctNumFieldName, letterRecipient, options, doPrintCoverSheet, false, costCenterFieldName);
        }
        public static void CostCenterPrinting(string letterId, string dataFile, string stateCodeFieldName, string scriptId, string acctNumFieldName, LetterRecipient letterRecipient, CostCenterOptions options, bool doPrintCoverSheet, bool copyLetterLocal, string costCenterFieldName = "")
        {
            CostCenterPrintingFormMerge(letterId, dataFile, stateCodeFieldName, scriptId, acctNumFieldName, letterRecipient, options, doPrintCoverSheet, copyLetterLocal, costCenterFieldName);
        }

        internal static void UheaaCostCenterPrinting(string scriptId, string letterId, string barcodedData, string costCenterFieldName, string stateCodeFieldName, bool copyLetterLocal)
        {
            UheaaCostCenterPrinting(scriptId, letterId, barcodedData, costCenterFieldName, stateCodeFieldName, true, copyLetterLocal);
        }

        public static void UheaaCostCenterPrinting(string scriptId, string letterId, string barcodedData, string costCenterFieldName, string stateCodeFieldName, bool doPrintCoverSheet, bool copyLetterLocal)
        {

            //Check for recovery
            string recoveryLog = string.Format("{0}CCP_{1}.txt", EnterpriseFileSystem.LogsFolder, scriptId);
            List<string> alreadyPrintedCostCenters = new List<string>();
            if (File.Exists(EnterpriseFileSystem.LogsFolder) && File.GetCreationTime(EnterpriseFileSystem.LogsFolder).Date == DateTime.Now.Date)
            {
                //In recovery, get a list of cost centers that have already been printed
                foreach (string recoveryLine in FS.ReadAllLines(recoveryLog))
                {
                    List<string> recoveryFields = recoveryLine.SplitAndRemoveQuotes(",");
                    if (recoveryFields[0] == letterId) alreadyPrintedCostCenters.Add(recoveryFields[1]);
                }
            }
            else
            {
                //Not in recovery, initialize the recovery log
                using (StreamWriter sw = new StreamW(recoveryLog, false))
                    sw.WriteCommaDelimitedLine("LetterID", "CostCenterCode", "DomesticCount", "ForeignCount", "TimeStamp");
            }

            //Split the data file into separate files for each cost center
            List<CostCenterFileData> ccDataFiles = CreateCostCenterDataFiles(barcodedData, costCenterFieldName, stateCodeFieldName);

            //Print the letters for each cost center file that hasn't been printed yet
            foreach (CostCenterFileData costCenterData in ccDataFiles.Where(p => !alreadyPrintedCostCenters.Contains(p.CostCenterCode)))
            {
                //Print the cover sheet
                if (doPrintCoverSheet)
                {
                    string coverSheetDataFile = CreateCoverSheetDataFile(letterId, costCenterData);
                    PrintDocs(EnterpriseFileSystem.GetPath("CoverSheet"), "Scripted State Mail Cover Sheet", coverSheetDataFile);
                    while (File.Exists(coverSheetDataFile))
                    {
                        try
                        {
                            FS.Delete(coverSheetDataFile);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }

                //Print the letter
                DocumentPathAndName docPath = GetDocumentPathAndFileName(letterId);
                if (copyLetterLocal)
                    docPath.CalculatedPath.Replace(Path.GetDirectoryName(docPath.CalculatedPath), EnterpriseFileSystem.TempFolder);
                PrintDocs(docPath.CalculatedPath, docPath.CalculatedFileName, costCenterData.DataFileName);
                while (File.Exists(costCenterData.DataFileName))
                {
                    try
                    {
                        FS.Delete(costCenterData.DataFileName);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                UpdateLogFiles(recoveryLog, EnterpriseFileSystem.GetPath("CoverSheet"), letterId, costCenterData.CostCenterCode, costCenterData.DataFileName, costCenterData.DomesticAddressCount, costCenterData.ForeignAddressCount);
            }

            FS.Delete(barcodedData);
        }

        /// <summary>
        /// Splits a data file into separate files based on cost cneter.
        /// </summary>
        /// <param name="dataFile"></param>
        /// <param name="costCenterFieldName"></param>
        /// <param name="stateCodeFieldName"></param>
        /// <returns>An Array of objects that have a cost center code, data file name, foreign address
        /// count and domestic address count for each cost center found in the data file.</returns>
        private static List<CostCenterFileData> CreateCostCenterDataFiles(string dataFile, string costCenterFieldName, string stateCodeFieldName)
        {
            //Make sure the data file exits
            if (!File.Exists(dataFile))
                throw new Exception(string.Format("File {0} is missing. Contact Systems Support", dataFile));

            //Initialize the list that will make up the return value
            List<CostCenterFileData> costCenterFileList = new List<CostCenterFileData>();

            //Open the data file
            using (StreamReader sr = new StreamR(dataFile))
            {
                //Get the indices of the cost center and state code fields from the header row
                List<string> headerFields = sr.ReadLine().SplitAndRemoveQuotes(",");
                int costCenterIndex = headerFields.IndexOf(costCenterFieldName);
                if (costCenterIndex < 0)
                    throw new Exception(string.Format("The cost center code field name ({0}) was not found in the header row.", costCenterFieldName));
                int stateCodeIndex = headerFields.IndexOf(stateCodeFieldName);
                if (stateCodeIndex < 0)
                    throw new Exception(string.Format("The state code field name ({0}) was not found in the header row.", stateCodeFieldName));

                //Create a dictionary of StreamWriters, indexed by cost center.
                //This will make it easy to copy a record from the main data file into the correct
                //cost center data file, and close all the cost center data files when done.
                Dictionary<string, StreamWriter> costCenterWriters = new Dictionary<string, StreamWriter>();

                //Read each line and see what cost center it belongs to
                while (!sr.EndOfStream)
                {
                    List<string> dataFields = sr.ReadLine().SplitAndRemoveQuotes(",");
                    string costCenter = dataFields[costCenterIndex];
                    if (!costCenterWriters.ContainsKey(costCenter))
                    {
                        //Create a new StreamW for this cost center and add it to the dictionary.
                        string costCenterDataFile = string.Format("{0}{1}{2}.txt", EnterpriseFileSystem.TempFolder, dataFile.Substring(dataFile.LastIndexOf('\\') + 1), costCenter);
                        StreamWriter costCenterWriter = new StreamW(costCenterDataFile, false);
                        costCenterWriters.Add(costCenter, costCenterWriter);
                        //Write the header row and the data line.
                        costCenterWriter.WriteCommaDelimitedLine(true, headerFields.ToArray());
                        costCenterWriter.WriteCommaDelimitedLine(dataFields.ToArray());

                        //Create an entry in the return list
                        CostCenterFileData costCenterData = new CostCenterFileData() { CostCenterCode = costCenter, DataFileName = costCenterDataFile, DomesticAddressCount = 0, ForeignAddressCount = 0 };
                        string stateCode = dataFields[stateCodeIndex];
                        if (stateCode == "FC" || stateCode.Trim().Length == 0 || stateCode.Trim().Length > 2)
                            costCenterData.ForeignAddressCount = 1;
                        else
                            costCenterData.DomesticAddressCount = 1;
                        costCenterFileList.Add(costCenterData);
                    }
                    else
                    {
                        //Write the data line to the appropriate StreamWriter
                        costCenterWriters[costCenter].WriteCommaDelimitedLine(dataFields.ToArray());

                        //Increment the foreign or domestic count in the appropriate entry in the return list
                        CostCenterFileData costCenterData = costCenterFileList.Where(p => p.CostCenterCode == costCenter).Single();
                        string stateCode = dataFields[stateCodeIndex];
                        if (stateCode == "FC" || stateCode.Trim().Length == 0)
                            costCenterData.ForeignAddressCount += 1;
                        else
                            costCenterData.DomesticAddressCount += 1;
                    }
                }

                //Close and dispose all the StreamWriters
                foreach (StreamWriter writers in costCenterWriters.Values)
                {
                    writers.Close();
                    writers.Dispose();
                }
            }
            return costCenterFileList;
        }

        /// <summary>
        /// Retrieves data from the database to create a data file that will be merged with the state mail cover sheet
        /// </summary>
        /// <param name="letterId"></param>
        /// <param name="costCenterData"></param>
        /// <returns>The location of the data file created</returns>
        private static string CreateCoverSheetDataFile(string letterId, CostCenterFileData costCenterData)
        {
            string dataFileBaseName = costCenterData.DataFileName.Split('\\').Last();
            string dataFileNoExtension = dataFileBaseName.Substring(0, dataFileBaseName.LastIndexOf("."));
            string coverSheetDataFile = string.Format("{0}{1}_Cover.txt", EnterpriseFileSystem.TempFolder, dataFileNoExtension);

            //Write the header and data line to the cover sheet data file
            using (StreamWriter sw = new StreamW(coverSheetDataFile))
            {
                string exec = string.Format("EXEC spGetCostCenterBU {0}", costCenterData.CostCenterCode);
                string businessUnit = DataAccessHelper.ExecuteList<string>("spGetCostCenterBU", DataAccessHelper.Database.Bsys, SqlParams.Single("CostCenterCode", costCenterData.CostCenterCode)).SingleOrDefault();
                string letterName;
                string pageCount;
                string coverSheetComments;
                try
                {
                    letterName = DataAccess.GetLetterName(letterId);
                }
                catch (Exception)
                {
                    throw new Exception(string.Format("Could not find the letter ID {0} in Letter Tracking.", letterId));
                }
                try
                {
                    BarcodeQueryResults result = DataAccess.GetPageCount(letterId);
                    pageCount = result.Duplex ? Math.Round(result.Pages / 2).ToString() : result.Pages.ToString();
                }
                catch (Exception)
                {
                    throw new BarcodeException("The paper sheet count for the letter id that the script is using isn't populated. Please contact Systems Support.");
                }
                try
                {
                    coverSheetComments = DataAccess.GetCostCenterInstructions(letterId);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Could not find the letter ID {0} in Letter Tracking", letterId), ex);
                }
                sw.WriteCommaDelimitedLine("BU", "Description", "NumPages", "Cost", "Standard", "Foreign", "CoverComment");
                sw.WriteCommaDelimitedLine(businessUnit, letterName, pageCount, costCenterData.CostCenterCode, costCenterData.DomesticAddressCount.ToString(), costCenterData.ForeignAddressCount.ToString(), coverSheetComments);
            }
            return coverSheetDataFile;
        }

        /// <summary>
        /// Appends the new data to the Master Cost Center file
        /// </summary>
        /// <param name="recoveryLog"></param>
        /// <param name="docFolder"></param>
        /// <param name="letterId"></param>
        /// <param name="costCenterCode"></param>
        /// <param name="dataFile"></param>
        /// <param name="domesticCount"></param>
        /// <param name="foreignCount"></param>
        private static void UpdateLogFiles(string recoveryLog, string docFolder, string letterId, string costCenterCode, string dataFile, int domesticCount, int foreignCount)
        {
            //Make sure the Mast Cost Center file exists. Create it, with a header row, if needed.
            string masterFile = string.Format("{0}MasterCostCenterFile.txt", docFolder);
            if (!File.Exists(masterFile))
            {
                using (StreamWriter sw = new StreamW(masterFile))
                {
                    sw.WriteCommaDelimitedLine("Date", "LetterID", "Foreign", "Count", "CostCenterCode", "File", "TimeStamp");
                }
            }

            //Append to the Master Cost Center file
            while (true)
            {
                try
                {
                    using (StreamWriter sw = new StreamW(masterFile, true))
                    {
                        if (domesticCount > 0)
                            sw.WriteCommaDelimitedLine(DateTime.Now.ToString("MM/dd/yyyy"), letterId, "", domesticCount.ToString(), costCenterCode, dataFile, DateTime.Now.ToString());
                        if (foreignCount > 0)
                            sw.WriteCommaDelimitedLine(DateTime.Now.ToString("MM/dd/yyyy"), letterId, "F", foreignCount.ToString(), costCenterCode, dataFile, DateTime.Now.ToString());
                    }
                    break;
                }
                catch (IOException)
                {
                    //The file is busy, sleep for a second and then try again.
                    Thread.Sleep(1000);
                }
            }

            //Append to the recovery log
            using (StreamWriter sw = new StreamW(recoveryLog, true))
            {
                sw.WriteCommaDelimitedLine(letterId, costCenterCode, domesticCount.ToString(), foreignCount.ToString(), DateTime.Now.ToString());
            }
        }

        #endregion

        /// <summary>
        /// Adds copies of documents to the imaging system
        /// </summary>
        /// <param name="testMode">Test mode indicator</param>
        /// <param name="Efs">Enterprise File System object</param>
        /// <param name="scriptId">Script Id</param>
        /// <param name="acctNumFieldName">Account number field name</param>
        /// <param name="docId">Doc Id to use in imaging</param>
        /// <param name="letter">The file name for the document</param>
        /// <param name="dataFile">Data file for a mail merge</param>
        /// <param name="region">region to run in UHEAA or CornerStone</param>
        /// <param name="letterRecip">Letter recipient</param>
        public static void ImageDocs(string scriptId, string acctNumFieldName, string docId, string letter, string dataFile, LetterRecipient letterRecip)
        {
            ImageDocs(scriptId, acctNumFieldName, docId, letter, dataFile, letterRecip, null);
        }

        public static void ImageDocs(string scriptId, string acctNumFieldName, string docId, string letter, string dataFile, LetterRecipient letterRecip, ImagingGenerator img)
        {
            PrintingDialog d = new PrintingDialog(scriptId);
            d.StartDisplay();

            string folder = string.Format("{0}{1}\\", EnterpriseFileSystem.TempFolder, scriptId);
            if (!Directory.Exists(folder))
                FS.CreateDirectory(folder);

            DocumentPathAndName docInfo = GetDocumentPathAndFileName(letter);
            int counter = 0;
            int recoveryCounter = 0;
            string recoveryLog = string.Format(@"{0}\{1}\imagingLog_{1}", EnterpriseFileSystem.TempFolder, scriptId);

            if (File.Exists(recoveryLog))
                recoveryCounter = int.Parse(FS.ReadAllText(recoveryLog));

            string barcodeDataFile = AddBarcodesForBatchProcessing(dataFile, acctNumFieldName, letter, false, letterRecip);

            using (StreamReader sr = new StreamR(barcodeDataFile))
            {
                string header = sr.ReadLine();
                int accountNumIndex = header.SplitAndRemoveQuotes(",").IndexOf(acctNumFieldName);
                while (counter < recoveryCounter)
                {
                    sr.ReadLine();
                    counter += 1;
                }

                while (!sr.EndOfStream)
                {
                    string fileLine = sr.ReadLine();
                    string borrowerData = string.Format(@"{0}\{1}\imagingData_{1}{2}{3}.txt", EnterpriseFileSystem.TempFolder, scriptId, counter, Guid.NewGuid().ToBase64String());

                    using (StreamWriter sw = new StreamW(borrowerData))
                    {
                        sw.WriteLine(header);
                        sw.WriteLine(fileLine);
                    }
                    Console.WriteLine("Imaging Document {0}", counter);
                    Guid guid = Guid.NewGuid();
                    string borrowerDoc = string.Format(@"{0}\{1}\imagingDoc_{1}{2}.doc", EnterpriseFileSystem.TempFolder, scriptId, guid);
                    string imageLocation = string.Format(@"{0}imagingDoc_{1}{2}.doc", EnterpriseFileSystem.GetPath("Imaging"), scriptId, guid);
                    SaveDocs(letter, borrowerData, borrowerDoc);
                    string acctNum = fileLine.SplitAndRemoveQuotes(",")[accountNumIndex];

                    string ssn = DataAccess.GetSsnFromFromAcctNo(acctNum);
                    if (img == null)
                    {
                        ImageFile(borrowerDoc, docId, ssn);
                        DeleteFile(borrowerDoc);
                    }
                    else
                        img.AddFile(imageLocation, docId, ssn); //The document is saved to T:\ but the control file needs to know where the image is moved to.
                    DeleteFile(borrowerData);
                    counter += 1;

                    FS.WriteAllText(recoveryLog, counter.ToString());
                    var result = Repeater.TryRepeatedly(CheckWinwordProcesses, 10, 3000, true);
                    if (!result.Successful)
                    {
                        throw result.CaughtExceptions.First();
                    }
                }
                FS.Delete(recoveryLog);
            }
            if (img != null)
                img.PublishControlFile();
            FS.Delete(folder, true);


            d.EndDisplay();
        }

        private static void CheckWinwordProcesses()
        {
            if (Process.GetProcessesByName("WINWORD").Count() > 20)
            {
                throw new Exception("Imaging has too many winwords open");
            }
        }

        /// <summary>
        /// Auto-archives the given file in the imaging system.
        /// </summary>
        /// <param name="efs">An EnterpriseFileSystem object initialized with the appropriate region and mode (test/live).</param>
        /// <param name="filePathAndName">The full path and name of the file to archive.</param>
        /// <param name="docId">The document ID to log in under when placing in the imaging system.</param>
        /// <param name="ssn">The SSN of the borrower to whom this document pertains.</param>
        public static void ImageFile(string filePathAndName, string docId, string ssn)
        {
            string fileExtension = filePathAndName.Substring(filePathAndName.LastIndexOf("."));
            string uniqueId = Guid.NewGuid().ToString();
            string imagingFolder = EnterpriseFileSystem.GetPath("IMAGING");
            string destination = string.Format("{0}{1}_{2}{3}", imagingFolder, ssn, uniqueId, fileExtension);
            //File.Copy(filePathAndName, destination);
            FS.Copy(filePathAndName, destination);
            string controlFile = string.Format("{0}{1}_{2}.ctl", imagingFolder, ssn, uniqueId);
            using (StreamWriter sw = new StreamW(controlFile))
            {
                sw.WriteLine(string.Format("~^Folder~{0:MM/dd/yyyy} {1}, Doc 1^Type~UTCR_TYPE^Attribute~SSN~STR~{2}^Attribute~LENDER_CODE~STR~^Attribute~ACCOUNT_NUM~STR~^Attribute~DOC_ID~STR~{3}^Attribute~DOC_DATE~STR~{0:MM/dd/yyyy}^Attribute~BATCH_NUM~STR~^Attribute~VENDOR_NUM~STR~^Attribute~SCAN_DATE~STR~{0:MM/dd/yyyy}^Attribute~SCAN_TIME~STR~{0:HH:mm:ss}^Attribute~DESCRIPTION~STR~{0:MM/dd/yyyy} {1}", DateTime.Now, DateTime.Now.TimeOfDay, ssn, docId));
                sw.WriteLine(String.Format("DesktopDoc~{0}{1}_{2}{3}~{4:MM/dd/yyyy} {5}, Doc 1^Type~UTCR_TYPE^Attribute~SSN~STR~{1}^Attribute~LENDER_CODE~STR~^Attribute~ACCOUNT_NUM~STR~^Attribute~DOC_ID~STR~{6}^Attribute~DOC_DATE~STR~{4:MM/dd/yyyy}^Attribute~BATCH_NUM~STR~^Attribute~VENDOR_NUM~STR~^Attribute~SCAN_DATE~STR~{4:MM/dd/yyyy}^Attribute~SCAN_TIME~STR~{4:HH:mm:ss}", imagingFolder, ssn, uniqueId, fileExtension, DateTime.Now, DateTime.Now.TimeOfDay, docId));
            }
        }

        /// <summary>
        /// Merges the merge data source text file with the specified document and saves it to specifed path and file then prints the document
        /// </summary>
        /// <param name="docPath">The directory path for the letter</param>
        /// <param name="doc">Name of the letter.  [does not have to include .doc but it can]</param>
        /// <param name="dataFile">Data file for the mail merge</param>
        /// <param name="saveAs">Path and name the document should be saved to</param>
        public static void PrintAndSaveDocs(bool testMode, string docPath, string doc, string dataFile, string saveAs)
        {
            SaveDocs(doc, dataFile, saveAs);
            PrintDocs(docPath, doc, dataFile);
        }

        /// <summary>
        /// Merges the merge data source text file with the specified document and prints it.
        /// </summary>
        /// <param name="docPath">The directory path for the letter</param>
        /// <param name="doc">Name of the letter.  [does not have to include .doc but it can]</param>
        /// <param name="dataFile">Data file for the mail merge</param>
        public static void PrintDocs(string docPath, string doc, string dataFile)
        {
            string filename = Path.Combine(docPath, doc);
            if (!File.Exists(filename))
                filename = Path.Combine(docPath, doc + ".docx");
            if (!File.Exists(filename))
                filename = Path.Combine(docPath, doc + ".doc");

            object missing = System.Type.Missing;
            object refFalse = false;
            object refTrue = true;
            object mergeType = Word.WdMergeSubType.wdMergeSubTypeOther;
            object pause = false;

            Word.Application wordApp = new Word.Application();
            wordApp.Visible = false;

            DoMailMerge(wordApp, dataFile, false, pause, null, filename, missing, refFalse, refTrue, mergeType);

            //object saveChanges = Word.WdSaveOptions.wdDoNotSaveChanges; Other version just use false
            wordApp.Application.Quit(false);
            wordApp.Quit(false);
            Marshal.FinalReleaseComObject(wordApp);
            Thread.Sleep(1000);
            GC.Collect();
        }

        /// <summary>
        /// Merges the merge data source text file with the specified document and saves it to specifed path and file
        /// </summary>
        /// <param name="doc">Name of the letter.  [does not have to include .doc but it can]</param>
        /// <param name="dataFile">Data file for the mail merge</param>
        /// <param name="saveAs">Path and name the document should be saved to</param>
        public static void SaveDocs(string letterId, string dataFile, string saveAs, bool processOffT = false, DataAccessHelper.Region? region = null)
        {
            DocumentPathAndName letterInfo = GetDocumentPathAndFileName(letterId, region);

            if (processOffT)
            {


                letterInfo.CalculatedPath = EnterpriseFileSystem.TempFolder;
            }

            if (!letterInfo.CalculatedFileName.EndsWith(".doc") && !letterInfo.CalculatedFileName.EndsWith(".docx")) { letterInfo.CalculatedFileName += ".doc"; }
            string networkFile = string.Format("{0}{1}", letterInfo.CalculatedPath, letterInfo.CalculatedFileName.Replace(" ", ""));
            string tempFile = string.Format(@"{0}\{1}_{2}.doc", EnterpriseFileSystem.TempFolder, letterId, Guid.NewGuid().ToBase64String());
            Repeater.TryRepeatedly(() => FS.Copy(networkFile, tempFile));
            object fileName = tempFile;
            object missing = System.Type.Missing;
            object refFalse = false;
            object refTrue = true;
            object mergeType = Word.WdMergeSubType.wdMergeSubTypeOther;
            object pause = false;
            object saveAsPath = saveAs;

            Word.Application wordApp = new Word.Application();
            wordApp.Visible = false;

            DoMailMerge(wordApp, dataFile, true, pause, saveAsPath, fileName, missing, refFalse, refTrue, mergeType);

            wordApp.ActiveDocument.Close(ref refTrue);
            object saveChanges = Word.WdSaveOptions.wdDoNotSaveChanges;
            // ((_Application)wordApp.Application).Quit(ref saveChanges);
            wordApp.Quit(ref saveChanges);
            Marshal.FinalReleaseComObject(wordApp);

            Repeater.TryRepeatedly(() => FS.Delete(tempFile));
        }

        private static void DoMailMerge(Word.Application wordApp, string dataFile, bool save, object pause, object saveAs, object fileName, object missing, object refFalse, object refTrue, object mergeType)
        {
            DoMailMerge(wordApp, dataFile, save, pause, saveAs, fileName, missing, refFalse, refTrue, mergeType, missing);
        }

        private static void DoMailMerge(Word.Application wordApp, string dataFile, bool save, object pause, object saveAs, object fileName, object missing, object refFalse, object refTrue, object mergeType, object format)
        {

            wordApp.Visible = false;

            var doc = wordApp.Application.Documents.Open(ref fileName, ref missing, ref refFalse, ref refFalse, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);


            var temp = wordApp.ActiveDocument.MailMerge;

            bool t = File.Exists(dataFile);

            wordApp.ActiveDocument.MailMerge.OpenDataSource(dataFile, ref missing, ref missing, ref refTrue, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref mergeType);

            wordApp.ActiveDocument.MailMerge.SuppressBlankLines = true;

            if (save)
            {
                wordApp.ActiveDocument.MailMerge.Destination = Microsoft.Office.Interop.Word.WdMailMergeDestination.wdSendToNewDocument;
                wordApp.ActiveDocument.MailMerge.Execute(ref pause);
                while (true)
                {
                    try
                    {
                        SaveAsWord(wordApp, ref saveAs, ref missing, ref format);
                        break;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            else
            {
                wordApp.ActiveDocument.MailMerge.Destination = Microsoft.Office.Interop.Word.WdMailMergeDestination.wdSendToPrinter;
                wordApp.ActiveDocument.MailMerge.Execute(ref pause);
            }
            object noSave = Word.WdSaveOptions.wdDoNotSaveChanges;
            doc.Close(ref noSave);
            Marshal.FinalReleaseComObject(doc);

        }

        private static void SaveAsWord(Word.Application wordApp, ref object saveAs, ref object missing, ref object format)
        {
            wordApp.ActiveDocument.SaveAs(ref saveAs, ref format, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
            ref missing, ref missing, ref missing, ref missing, ref missing);
        }

        internal static DocumentPathAndName GetDocumentPathAndFileName(string letterId, DataAccessHelper.Region? region = null)
        {
            return DataAccess.GetDocumentPathAndName(letterId, region);
        }

        //private static string Encode(string dataToEncode, bool processTilde, EncodingModes em, PreferredFormats pm)
        //{
        //    DataMatrixBarcode DMFontEncoder = new DataMatrixBarcode();
        //    return DMFontEncoder.FontEncode(dataToEncode, processTilde, em, pm);
        //}

        private static string Encode(string dataToEncode, int procTilde, int encMode, int prefFormat)
        {
            string encodedFont = "";
            Datamatrix DMFontEncoder = new Datamatrix();
            DMFontEncoder.FontEncode(dataToEncode, procTilde, encMode, prefFormat, out encodedFont);
            return encodedFont;
        }

        private static int FigureSheetCount(BarcodeQueryResults letterData)
        {
            int pageCount = 0;

            if (letterData.Pages == 0)
            {
                throw new BarcodeException("The paper sheet count for the letter id that the script is using isn't populated.  Please contact a member of Systems Support");
            }
            else
            {
                if (letterData.Pages == 1) { pageCount = 1; }
                else if (!letterData.Duplex) { pageCount = int.Parse(letterData.Pages.ToString()); }
                else//if the letter is duplex we want to divide the page count by 2
                {
                    pageCount = int.Parse(letterData.Pages.ToString()) / 2;
                    if (letterData.Pages % 2 > 0)
                    {
                        pageCount = pageCount + 1;
                    }
                }
            }

            return pageCount;
        }

        private static string GetStateCode(string dataFile, string stateCodeFieldName)
        {
            string stateCode = string.Empty;
            using (StreamReader sr = new StreamR(dataFile))
            {
                List<string> rec = sr.ReadLine().SplitAndRemoveQuotes(",");
                bool foundIndex = false;
                int index = 0;
                foreach (string header in rec)
                {
                    if (header.Contains(stateCodeFieldName))
                    {
                        foundIndex = true;
                        break;
                    }

                    index++;
                }

                if (!foundIndex)
                {
                    throw new Exception("Unable to find the State Code.  Please contact Systems Support for assistance.");
                }

                stateCode = sr.ReadLine().SplitAndRemoveQuotes(",")[index];
                return stateCode;
            }
        }
    }
}
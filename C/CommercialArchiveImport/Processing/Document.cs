using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommercialArchiveImport
{
    public class Document
    {
        public string SSN { get; set; }
        public string DocID { get; set; }
        public string DocDate { get; set; }
        public string ImagePath { get; set; }
        public int LineNumber { get; set; }
        public string OriginalLine { get; set; }
        private DocumentError error = null;
        public DocumentError Error
        {
            get
            {
                return error;
            }
            set
            {
                error = value;
                ResultsHelper.LogError("Line " + LineNumber + ". " + error.ErrorMessage);
            }
        }
        public void SetError(string message, DocumentErrorType type = DocumentErrorType.Unspecified)
        {
            Error = new DocumentError(message, type);
        }
        public bool Invalid { get { return error != null; } }
        public string Extension
        {
            get
            {
                int start = ImagePath.LastIndexOf('.') + 1;
                return ImagePath.Substring(start, ImagePath.Length - start).ToLower();
            }
        }
        static Dictionary<string, string> DocIds = new Dictionary<string, string>();
        private Document() { }
        public static Document Parse(string line, int lineNumber)
        {
            if (string.IsNullOrEmpty(line))
                return null;
            Document doc = new Document();
            doc.OriginalLine = line;
            doc.LineNumber = lineNumber;
            string[] lineParts = line.Split('|');
            if (lineParts.Length > 11)
                doc.SetError("Line has more than 11 fields.  Line was skipped.", DocumentErrorType.MoreThan11);
            else if (lineParts.Length < 11)
                doc.SetError("Line has less than 11 fields.  Line was skipped.", DocumentErrorType.LessThan11);
            else
            {
                doc.SSN = lineParts[0];
                string docType = lineParts[3].ToLower();
                if (!DocIds.Keys.Contains(docType))
                    doc.SetError(string.Format("Line has an unknown Doc Type: {0}.  Line was skipped.", docType), DocumentErrorType.UnknownDocType);
                else
                {
                    doc.DocID = DocIds[docType];
                    doc.DocDate = lineParts[5];
                    doc.ImagePath = lineParts[10];
                    doc.LineNumber = lineNumber;
                }
            }
            return doc;
        }
        static Document()
        {
            //normal docids
            DocIds.Add("bcor", "LSCOR");
            DocIds.Add("corr", "LSCOR");
            DocIds.Add("ddbd", "LSCOR");
            DocIds.Add("defr", "LSDEF");
            DocIds.Add("disc", "LSRSH");
            DocIds.Add("dodf", "LSFRG");
            DocIds.Add("enda", "UGALA");
            DocIds.Add("forb", "LSFOR");
            DocIds.Add("ibrf", "LSIBR");
            DocIds.Add("note", "UGALA");
            DocIds.Add("payh", "CSPSH");
            DocIds.Add("phst", "CSCSH");
            DocIds.Add("scor", "LSCOR");
            DocIds.Add("srvh", "CSCSH");
            DocIds.Add("tlfo", "LSFRG");
            DocIds.Add("tpdd", "LSCOR");
            //proprietary nelnet docids
            DocIds.Add("02", "LSARC");
            DocIds.Add("08", "SKSCH");
            DocIds.Add("09", "LSRSH");
            DocIds.Add("10", "LSRSH");
            DocIds.Add("11", "CSCSH");
            DocIds.Add("12", "UGAML");
            DocIds.Add("13", "LSFRG");
            DocIds.Add("14", "LSCOR");
            DocIds.Add("16", "CRADD");
            DocIds.Add("17", "LSCOR");
            DocIds.Add("18", "DACOR");
            DocIds.Add("19", "UGALA");
            DocIds.Add("20", "UGALA");
            DocIds.Add("21", "UGALA");
            DocIds.Add("21a", "LSCOR");
            DocIds.Add("22", "UGALA");
            DocIds.Add("23", "UGNOG");
            DocIds.Add("24", "UGALA");
            DocIds.Add("25", "LSCOR");
            DocIds.Add("26", "LSCOR");
            DocIds.Add("27", "LSDEF");
            DocIds.Add("28", "LSFOR");
            DocIds.Add("29", "LSCOR");
            DocIds.Add("30", "LSCOR");
            DocIds.Add("31", "LSCOR");
            DocIds.Add("32", "LSACH");
            DocIds.Add("33", "LSCOR");
            DocIds.Add("34", "LSIBR");
            DocIds.Add("35", "LSCOR");
            DocIds.Add("36", "LSCOR");
            DocIds.Add("37", "LSARC");
            DocIds.Add("38", "LSARC");
            DocIds.Add("41", "LSCOR");
            DocIds.Add("42", "LSARC");
            DocIds.Add("43", "LSARC");
            DocIds.Add("44", "LSARC");
            DocIds.Add("46", "ASKEY");
            DocIds.Add("47", "LSARC");
            DocIds.Add("48", "LSARC");
            DocIds.Add("49", "LSARC");
            DocIds.Add("50", "CSCSH");
            DocIds.Add("53", "LSARC");
            DocIds.Add("54", "CSCSH");
            DocIds.Add("55", "LSARC");
            DocIds.Add("56", "UGEVR");
            DocIds.Add("57", "CSCSH");
            DocIds.Add("58", "LSARC");
            DocIds.Add("61", "LSARC");
            DocIds.Add("62", "LSARC");
            DocIds.Add("63", "LSFRG");
            DocIds.Add("64", "LSLVC");
            DocIds.Add("65", "UGAML");
            DocIds.Add("66", "LSARC");
            DocIds.Add("67", "ASBKP");
            DocIds.Add("68", "LSLVC");
            DocIds.Add("69", "LSARC");
            DocIds.Add("70", "LSARC");
            DocIds.Add("71", "LSCUR");
            DocIds.Add("72", "LSARC");
            DocIds.Add("73", "LSARC");
            DocIds.Add("74", "LSARC");
            DocIds.Add("75", "LSARC");
            DocIds.Add("76", "LSARC");
            DocIds.Add("77", "LSARC");
            DocIds.Add("78", "LSARC");
            DocIds.Add("79", "LSARC");
            DocIds.Add("80", "LSARC");
            DocIds.Add("81", "LSARC");
            DocIds.Add("82", "LSARC");
            DocIds.Add("83", "LSARC");
            DocIds.Add("84", "LSARC");
            DocIds.Add("90", "LSARC");
            DocIds.Add("95", "LSCOR");
            DocIds.Add("96", "LSARC");
            DocIds.Add("97", "LSARC");
            DocIds.Add("98", "LSARC");
            DocIds.Add("99", "LSCOR");
        }

        public static List<Document> ParseFile(string indexFileContents)
        {
            ProgressHelper.Start("Validating");
            string[] lines = indexFileContents.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            ProgressHelper.Increments = lines.Length;
            List<Document> docs = new List<Document>();
            for (int i = 0; i < lines.Length; i++)
            {
                Document doc = Document.Parse(lines[i], i + 1);
                if (doc != null) //completely skip blank lines
                {
                    if (!string.IsNullOrEmpty(doc.ImagePath))
                    {
                        var duplicates = docs.Where(d => (d.ImagePath ?? "").ToLower() == doc.ImagePath.ToLower());
                        foreach (var duplicate in duplicates)
                        {
                            duplicate.SetError("Referenced Image is the same as Line #" + doc.LineNumber + ". Line was skipped.", DocumentErrorType.Duplicate);
                            doc.SetError("Referenced Image is the same as Line #" + duplicate.LineNumber + ". Line was skipped.", DocumentErrorType.Duplicate);
                        }
                    }
                    docs.Add(doc);
                }
                ProgressHelper.Increment();
            }
            return docs;
        }
    }
}

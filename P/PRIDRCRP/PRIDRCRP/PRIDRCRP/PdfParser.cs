using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common;

namespace PRIDRCRP
{
    public class PdfParser
    {
        public List<string> ReadPdfFilePages(string fileName)
        {
            List<string> pages = new List<string>();

            if (File.Exists(fileName))
            {
                PdfReader pdfReader = new PdfReader(fileName);

                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                    pages.Add(currentText);
                }
                pdfReader.Close();
            }
            return pages;
        }

        public decimal ConvertPostNegativeToDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0.00M;
            }
            else
            {
                if (value.Length > 2 && value[value.Length - 1] == '-')
                {
                    return -Convert.ToDecimal(value.Substring(0, value.Length - 1));
                }
                else if (value.Length > 3 && value[value.Length - 2] == 'C' && value[value.Length - 1] == 'R')
                {
                    return -Convert.ToDecimal(value.Substring(0, value.Length - 2));
                }
                else
                {
                    return Convert.ToDecimal(value);
                }
            }
        }

        /// <summary>
        /// Finds a field based off of a header and an offset from the end of that header
        /// data must be on the same line
        /// </summary>
        protected string ParseField(string line, string header, int fieldLength, List<string> filterStrings = null)
        {
            string lineUpper = line.ToUpper();
            if (lineUpper.Contains(header))
            {
                int headerLen = header.Length;
                int headerInd = lineUpper.IndexOf(header);
                string fieldData = StringParsingHelper.SafeSubStringTrimmed(line, headerInd + headerLen, fieldLength);
                if (filterStrings != null)
                {
                    foreach (string str in filterStrings)
                    {
                        fieldData = fieldData.Replace(str, "");
                    }
                }
                return fieldData;
            }
            else
            {
                return null;
            }
        }
    }
}

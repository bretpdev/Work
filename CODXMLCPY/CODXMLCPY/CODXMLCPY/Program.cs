using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CODXMLCPY
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            MessageBox.Show("This app is used to read in the COD applications listed in an Excel file and then aggregate the COD XML data. Please copy the BF_SSN and WX_MSG_1_TSK fields and paste them into a text file. Each record should be delimited by a new line.");
            string txtFileName = GetTextFile();
            List<CodApp> codApps = ReadCodAppTextFile(txtFileName);
            string sourceFile = GetCodFileWithData();
            string destinationFile = GetFileForImport();
            CopyCodXmlData(sourceFile, destinationFile, codApps);
            MessageBox.Show($"Process Complete. XML Data imported from {sourceFile} to {destinationFile}");
        }

        

        /// <summary>
        /// Reads the lines from the user-made text file that is made by copying and pasting
        /// the BF_SSN and the WX_MSG_1_TSK fields from the provided list of desired apps.
        /// Uses the data from the read file to construct a list of cod applications the 
        /// script should target.
        /// 
        /// Throws a FormatException() if the line is not in the right format.
        /// </summary>
        private static List<CodApp> ReadCodAppTextFile(string fileName)
        {
            string[] lines = System.IO.File.ReadAllLines(fileName);
            List<CodApp> codApps = new List<CodApp>();
            bool arcFieldIncluded = lines.Any(p => p.Contains("CODCA") || p.Contains("CODPA"));
            foreach (string line in lines)
            {
                string currentLine = line.Replace("APPL ID:", "");
                int dateIndex = currentLine.IndexOf("DATE:");
                currentLine = currentLine.Remove(dateIndex);
                int applicationIndex = arcFieldIncluded ? 2 : 1;
                string[] fields = currentLine.Split('\t');
                if (int.TryParse(fields[0].Trim(), out int ssn) && int.TryParse(fields[applicationIndex].Trim(), out int appId)) 
                {
                    CodApp ca = new CodApp(fields[0].Trim(), appId.ToString()); // Not using "ssn" value because we don't want leading zeros to be cut
                    codApps.Add(ca);
                }
                else
                {
                    throw new FormatException($"The following line was not in the right format: {line}");
                }
            }
            return codApps;
        }

        /// <summary>
        /// Prompts user to select the text file. This should be the file composed of
        /// BF_SSN and WX_MSG_1_TSK for the target apps that we need to pull XML data
        /// for.
        /// </summary>
        /// <returns></returns>
        private static string GetTextFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"T:\",
                Title = "Choose the text file with BF_SSN and WX_MSG_1_TSK",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "txt",
                Filter = "|*.txt",
                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            else
                return null;
        }

        /// <summary>
        /// Prompts user to select the source file.
        /// </summary>
        /// <returns></returns>
        private static string GetCodFileWithData()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"T:\",
                Title = "Choose the file that has the COD XML apps you want to copy",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "xml",
                Filter = "|*.xml",
                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            else
                return null;
        }

        /// <summary>
        /// Prompts user to select the destination file.
        /// </summary>
        /// <returns></returns>
        private static string GetFileForImport()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"T:\",
                Title = "Choose the file that you want to import the COD XML data into",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "xml",
                Filter = "|*.xml",
                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            else
                return null;
        }

        /// <summary>
        /// Copies the XML data in the Borrower nodes that match the desired 
        /// SSN/ApplicationId combo present in the passed-in list of cod applications.
        /// </summary>
        private static void CopyCodXmlData(string sourceFile, string destinationFile, List<CodApp> codApps)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(sourceFile);

            XmlDocument xmlDocNew = new XmlDocument();
            xmlDocNew.Load(destinationFile);
            XmlNode root = xmlDoc.DocumentElement;

            foreach (CodApp ca in codApps)
            {
                string xpath = $"descendant::Borrower[SSN='{ca.Ssn}'][RepaymentApplication/ApplicationID='{ca.ApplicationId}']";
                XmlNode node = root.SelectSingleNode(xpath);
                XmlNode newNode = xmlDocNew.ImportNode(node, true);
                xmlDocNew.DocumentElement.AppendChild(newNode);
            }
            xmlDocNew.Save(destinationFile);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.Scripts;

namespace IPACPMTFED
{
    public partial class Processing : Form
    {
        public List<FileData> Data { get; set; }
        private string FileToProcess;
        public Processing(string fileToProcess)
        {
            InitializeComponent();
            int count = File.ReadAllLines(fileToProcess).Count();
            progress.Maximum = count;
            Data = new List<FileData>();
            FileToProcess = fileToProcess;
            
        }

        private void CheckFileFormat(List<string> fileLine)
        {
            bool hasErrors = false;
            int ssn = 0;
            DateTime effDate = new DateTime();
            decimal paymentAmount = 0;
            if (!int.TryParse(fileLine[0], out ssn))
                hasErrors = true;
            else if (!DateTime.TryParse(fileLine[2], out effDate))
                hasErrors = true;
            else if (!decimal.TryParse(fileLine[3], out paymentAmount))
                hasErrors = true;

            if (hasErrors)
            {
                MessageBox.Show("The file is not in the correct format.  Please review and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new EndDLLException();
            }

        }

        public void Process()
        {
            this.Show();
            Application.DoEvents();
            progress.Refresh();
            using (StreamReader sr = new StreamReader(FileToProcess))
            {
                sr.ReadLine();//Read out the header row.
                while (!sr.EndOfStream)
                {
                    Application.DoEvents();
                    progress.Refresh();
                    List<string> fileLine = sr.ReadLine().SplitAndRemoveQuotes(",");
                    //While testing the script i found that the files that were given to me by operational account had some blank fields.
                    //These files come from an outside source so we do not have control over this and so we are coding around it rather than having OPA fix the files.
                    if (!fileLine[0].IsNullOrEmpty())
                    {
                        fileLine[0] = fileLine[0].Replace("-", "").PadLeft(9, '0');
                        CheckFileFormat(fileLine);

                        Data.Add(new FileData()
                        {
                            Ssn = fileLine[0],
                            BorrowersLastName = DataAccess.GetBorrowersLastName(fileLine[0]),
                            PaymentEffectiveDate = DateTime.Parse(fileLine[2]).ToString("MMddyy"),
                            PaymentAmount = decimal.Parse(fileLine[3])
                        });
                    }
                    progress.Increment(1);
                }

            }
            this.Hide();
        }
    }
}

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

namespace COMPLPPPPS
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
                    List<string> temp = sr.ReadLine().SplitAndRemoveQuotes(",");
                    //While testing the script i found that the files that were given to me by operational account had some blank fields.
                    //These files come from an outside source so we do not have control over this and so we are coding around it rather than having OPA fix the files.
                    if (!temp[0].IsNullOrEmpty())
                    {
                        temp[0] = temp[0].Replace("-", "");
                        switch (temp[0].Length)
                        {
                            case 7:
                                temp[0] = temp[0].Insert(0, "00");
                                break;
                            case 8:
                                temp[0] = temp[0].Insert(0, "0");
                                break;
                        }

                        Data.Add(new FileData()
                        {
                            Ssn = temp[0],
                            BorrowersLastName = DataAccess.GetBorrowersLastName(temp[0]),
                            PaymentEffectiveDate = DateTime.Parse(temp[2]).ToString("MMddyy"),
                            PaymentAmount = decimal.Parse(temp[3])
                        });
                    }
                    progress.Increment(1);
                }

            }
            this.Hide();
        }
    }
}

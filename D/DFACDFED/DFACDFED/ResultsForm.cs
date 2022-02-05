using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace DFACDFED
{
    public partial class ResultsForm : Form
    {
        private Processor processor;
        public ResultsForm(Processor processor, DateTime today)
        {
            InitializeComponent();
            if (DataAccessHelper.TestMode)
                this.Text = "[TEST] " + this.Text;
            this.processor = processor;
            TodayDate.Value = today;
            processor.PrintedLetterProcessed += info =>
            {
                Invoke(() =>
                {
                    PrintedLettersBox.Items.Add(BorrowerString(info));
                    PrintedLettersLabel.Text = "Printed Letters Generated (" + processor.PrintedLetterBorrowers.Count + ")";
                });

            };
            processor.EcorrLetterProcessed += info =>
            {
                Invoke(() =>
                {
                    EcorrLettersBox.Items.Add(BorrowerString(info));
                    PrintedLettersLabel.Text = "Ecorr Letters Generated (" + processor.EcorrLetterBorrowers.Count + ")";
                });
            };
        }

        private void ProcessingButton_Click(object sender, EventArgs e)
        {
            ProcessingButton.Enabled = false;
            ProcessingButton.Text = "Processing...";
            EcorrLettersLabel.Text = "Ecorr Letters Generated (0)";
            PrintedLettersLabel.Text = "Printed Letters Generated (0)";
            TodayDate.Enabled = false;
            Thread t = new Thread(() =>
            {
                processor.Process(TodayDate.Value);
                Invoke(() => ProcessingButton.Text = "Processing Complete");
                Invoke(() => ProcessingCompleteLabel.Visible = true);
            });
            t.Start();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            CancelButton.Text = "Cancelling...";
            processor.CancelPending = true;
            while (processor.Running)
                Thread.Sleep(100);
            this.Close();
        }

        private string BorrowerString(BorrowerInfo bi)
        {
            return string.Format("{0} ({1})", bi.Letter.AccountNumber, bi.Letter.LetterId);
        }

        private void Invoke(Action a)
        {
            base.Invoke(a);
        }
    }
}

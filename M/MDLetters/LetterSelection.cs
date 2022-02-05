using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.WinForms;

namespace MDLetters
{
    public partial class LetterSelection : Form
    {
        private List<LettersSentFromMD> LettersFromMd { get; set; }
        private List<PreviousLetterData> PreviousLetters { get; set; }
        private List<LetterData> AllLetters { get; set; }
        private List<Formats> AltFormats { get; set; }
        private int GloablFormat { get; set; }

        public LetterSelection(string accountNumber)
        {
            InitializeComponent();

            if (accountNumber.IsNullOrEmpty())
                accountNumber = GetAccountNumber();

            if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.Uheaa)
            {
                FuteCorr.Enabled = false;
                PrevLetters.Enabled = false;
                LettersFromMd = LettersSentFromMD.Populate("UHEAA Customer Services");
                CorrespondenceFormat.Enabled = false;
            }
            else
            {
                LettersFromMd = LettersSentFromMD.Populate("CornerStone Customer Services");
                PreviousLetters = PreviousLetterData.PopulatePreviousLetters(accountNumber);
                AllLetters = LetterData.Populate();
                AltFormats = Formats.Populate();
                CorrespondenceFormat.DataSource = AltFormats.Select(p => p.CorrespondenceFormat).ToList();
                CorrespondenceFormat.SelectedIndex = GloablFormat = DataAccessHelper.ExecuteSingle<int>("GetBorrowersAltFormat", DataAccessHelper.Database.ECorrFed, SqlParams.Single("AccountNumber", accountNumber)) - 1;
            }

            Acct.Text = accountNumber;
        }

        private static string GetAccountNumber()
        {
            string accountNumber = string.Empty;
            string message = "Unable to grab account number from MauiDude.  Please enter the account number.";
            using (var input = new InputBox<AccountNumberTextBox>(message, "Enter Account Number"))
            {
                if (input.ShowDialog() == DialogResult.OK)
                    accountNumber = input.InputControl.Text;
                else
                    Environment.Exit(1);
            }
            return accountNumber;
        }

        private void FuteCorr_CheckedChanged(object sender, EventArgs e)
        {
            if (FuteCorr.Checked)
            {
                Letters.DataSource = AllLetters;
            }
        }

        private void PrevLetters_CheckedChanged(object sender, EventArgs e)
        {
            if (PrevLetters.Checked)
            {
                Letters.DataSource = PreviousLetters;
            }
        }

        private void MdLetters_CheckedChanged(object sender, EventArgs e)
        {
            if (MdLetters.Checked)
            {
                Letters.DataSource = LettersFromMd;
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            bool changeGlobal = CorrespondenceFormat.SelectedIndex != GloablFormat;
            Formats selectedFormat = null;
            if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone)
                selectedFormat = AltFormats.Where(p => p.CorrespondenceFormat == CorrespondenceFormat.Text).Single();
            foreach (DataGridViewRow row in Letters.Rows)
            {
                if (MdLetters.Checked)
                {
                    List<LettersSentFromMD> lettersToSend = new List<LettersSentFromMD>();

                    if ((bool)((DataGridViewCheckBoxCell)row.Cells["Select"]).Value)
                    {
                        string arc = ((DataGridViewTextBoxCell)row.Cells["Arc"]).Value.ToString();
                        AddArc(arc, "");
                        if (changeGlobal && DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone)
                        {
                            List<string> letterIds = DataAccessHelper.ExecuteList<string>("GetLetterIdFromArc", DataAccessHelper.Database.Bsys, SqlParams.Single("Arc", arc));
                            if (letterIds.Count == 0)
                                MessageBox.Show(string.Format("Unable to find a letter for this arc:{0}, please contact Systems Support",arc));
                            foreach (string letterId in letterIds)
                            {
                                AddArc("P203A", string.Format("Letter: {0}, will be generated in {1} format.", letterId, selectedFormat.CorrespondenceFormat));//TODO fill out when known
                            }
                        }

                        if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone && selectedFormat.CorrespondenceFormat != "Standard")
                        {
                            List<string> letterIds = DataAccessHelper.ExecuteList<string>("GetLetterIdFromArc", DataAccessHelper.Database.Bsys, SqlParams.Single("Arc", arc));
                            foreach (string letterId in letterIds)
                            {
                                AddFutureLetter(Acct.Text, letterId, selectedFormat.CorrespondenceFormatId);
                            }
                        }
                    }
                }
                else if (PrevLetters.Checked)
                {
                    if ((bool)((DataGridViewCheckBoxCell)row.Cells["Select"]).Value)
                    {
                        int id = ((DataGridViewTextBoxCell)row.Cells["DocumentDetailsId"]).Value.ToString().ToInt();
                        string letterId = ((DataGridViewTextBoxCell)row.Cells["LetterId"]).Value.ToString();
                        string sentAt = ((DataGridViewTextBoxCell)row.Cells["SentAt"]).Value.ToString().ToDate().ToShortDateString();
                        DataAccessHelper.Execute("RegeneratePreviousLetterAsAltFormat", DataAccessHelper.Database.ECorrFed, SqlParams.Single("DocumentDetailsId", id), SqlParams.Single("CorrespondenceFormatId", selectedFormat.CorrespondenceFormatId));
                        AddArc("P203A", string.Format("Letter: {0} sent: {2} will be re-generated in {1} format", letterId, selectedFormat.CorrespondenceFormat, sentAt));//TODO fill out when known
                    }
                }
                else if (FuteCorr.Checked)
                {
                    if ((bool)((DataGridViewCheckBoxCell)row.Cells["Select"]).Value)
                    {
                        string letterId = ((DataGridViewTextBoxCell)row.Cells["LetterId"]).Value.ToString();
                        AddFutureLetter(Acct.Text, letterId, selectedFormat.CorrespondenceFormatId);
                        AddArc("P203A", string.Format("Letter: {0}, will be generated in {1} format the next time it is requested.", letterId, selectedFormat.CorrespondenceFormat));//TODO fill out when known
                    }
                }
            }

            MessageBox.Show("Letters have been sent.");
            DialogResult = DialogResult.OK;
        }

        private void AddArc(string arc, string comment)
        {
            ArcData arcD = new ArcData(DataAccessHelper.CurrentRegion)
            {
                AccountNumber = Acct.Text,
                Arc = arc,
                ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                Comment = comment,
                ScriptId = Program.ScriptId
            };

            ArcAddResults result = arcD.AddArc();
            if (!result.ArcAdded)
            {
                ProcessLogger.AddNotification(Program.LogData.ProcessLogId, string.Join("\r\n", result.Errors), NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Dialog.Error.Ok(string.Join("\r\n", result.Errors));
            }
        }

        private void AddFutureLetter(string accountNumber, string letterId, int formatId)
        {
            DataAccessHelper.Execute("InsertFutureAltFormatLetter", DataAccessHelper.Database.ECorrFed, SqlParams.Single("AccountNumber", accountNumber),
                SqlParams.Single("LetterId", letterId), SqlParams.Single("AddedBy", Environment.UserName), SqlParams.Single("CorrespondenceFormatId", formatId));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (MdLetters.Checked)
            {
                Letters.DataSource = LettersFromMd.Where(p => p.Description.ToUpper().Contains(Search.Text)).ToList();
            }
            else if (PrevLetters.Checked)
            {
                Letters.DataSource = PreviousLetters.Where(p => p.Description.ToUpper().Contains(Search.Text)).ToList();
            }
            else if (FuteCorr.Checked)
            {
                Letters.DataSource = AllLetters.Where(p => (p.Description ?? "").ToUpper().Contains(Search.Text)).ToList();
            }
        }
    }
}

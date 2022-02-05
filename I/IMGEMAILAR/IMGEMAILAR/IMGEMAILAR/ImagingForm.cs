using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.WinForms;
using Dialog = Uheaa.Common.Dialog;
namespace IMGEMAILAR
{
    public partial class ImagingForm : Form
    {
        readonly ProcessLogRun PLR;
        readonly DataAccess DA;
        readonly string ScriptId;
        readonly string TempFolder;
        public ImagingForm(ProcessLogRun plr, string scriptId)
        {
            InitializeComponent();
            ScriptId = scriptId;
            TempFolder = Path.Combine(EnterpriseFileSystem.TempFolder, ScriptId);
            if (!Directory.Exists(TempFolder))
                Directory.CreateDirectory(TempFolder);
            ImageButton.Tag = ImageButton.Text;
            PLR = plr;
            BorrowerSearch.LDA = PLR.LDA;
            DA = new DataAccess(plr);
            BorrowerSearch.OnelinkEnabled = DA.HasOnelinkAccess;
            BorrowerSearch.UheaaEnabled = DA.HasUheaaAccess;

            SetButtonText();
        }

        public QuickBorrower SelectedBorrower { get; private set; }
        public string SelectedLetterId { get; private set; }
        private void SetButtonText()
        {
            ImageButton.Text = "Select a Borrower and Letter";
            if (SelectedBorrower != null)
            {
                if (SelectedLetterId == null)
                    ImageButton.Text = "Select a Letter for Borrower " + SelectedBorrower.FullName;
                else if (currentTempPdf == null && !Attachments.Any())
                    ImageButton.Text = "Paste contents to proceed.";
                else if (currentTempPdf == null && Attachments.Any())
                    ImageButton.Text = $"Image these attachments for Borrower {SelectedBorrower.FullName}, Letter {SelectedLetterId}";
                else
                    ImageButton.Text = $"Image this content for Borrower {SelectedBorrower.FullName}, Letter {SelectedLetterId}";
            }

            if (currentTempPdf == null)
                CopyButton.Text = "Paste Copied Content";
            else
                CopyButton.Text = "Clear Copied Content";


        }

        private void BorrowerSearch_OnSearchResultsRetrieved(DynamicBorrowerSearchControl sender, List<QuickBorrower> results)
        {
            BorrowerResults.SetResults(results);
        }

        private void BorrowerResults_OnBorrowerChosen(object sender, QuickBorrower selected)
        {
            SelectedBorrower = selected;
            List<Document> descriptions = null;
            List<Letter> availables = null;
            if (selected.RegionEnum == RegionSelectionEnum.OneLINK)
            {
                availables = DA.OnelinkAvailableDocuments;
                descriptions = DA.UheaaDocuments;
            }
            else if (selected.RegionEnum == RegionSelectionEnum.Uheaa)
            {
                availables = DA.UheaaAvailableDocuments;
                descriptions = DA.UheaaDocuments;
            }
            else
                throw new Exception("Invalid borrower region " + selected.RegionEnum);
            availables = availables.ToList(); //make a copy
            foreach (var available in availables)
            {
                var match = descriptions.SingleOrDefault(o => o.DocId == available.LetterId);
                if (match != null)
                    available.OverrideDescription = available.OverrideDescription ?? match.DocType;
            }

            availables.Insert(0, new Letter() { });

            LetterBox.DisplayMember = "Description";
            LetterBox.ValueMember = "LetterId";
            LetterBox.DataSource = availables;
            LetterBox.SelectedIndex = -1;
            LetterBox.Enabled = true;
            SetButtonText();
        }

        private void LetterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectedLetterId = (LetterBox.SelectedItem as Letter)?.LetterId;
            SetButtonText();
        }

        private void ImageButton_Click(object sender, EventArgs e)
        {
            if (SelectedBorrower != null && SelectedLetterId != null && (currentTempPdf != null || Attachments.Any()))
            {
                var gen = new ImagingGenerator(ScriptId, Environment.UserName);
                if (currentTempPdf != null)
                {
                    var filename = Path.Combine(gen.ImagingPath, Guid() + ".pdf");
                    PreviewBrowser.Dispose();
                    File.Copy(currentTempPdf, Path.Combine(gen.Folder, Path.GetFileName(filename)));
                    TempFilesToDelete.Push(currentTempPdf);
                    currentTempPdf = null;
                    gen.AddFile(filename, SelectedLetterId, SelectedBorrower.SSN);
                }

                foreach (var attachment in Attachments)
                {
                    string attachmentDestination = Path.Combine(gen.ImagingPath, Guid() + Path.GetExtension(attachment));
                    string tempDestination = Path.Combine(gen.Folder, Path.GetFileName(attachmentDestination));
                    File.Copy(attachment, tempDestination);
                    gen.AddFile(attachmentDestination, SelectedLetterId, SelectedBorrower.SSN);
                }
                gen.PublishControlFile();

                List<string> messages = new List<string>();
                messages.Add($"Email and/or Attachments from Borrower {SelectedBorrower.FullName} successfully imaged with Letter {SelectedLetterId}");
                string comments = ActivityCommentBox.Text;
                string utId = DA.GetCsysAesId();
                if (utId == "UT")
                    utId = null;
                if (string.IsNullOrEmpty(utId))
                    utId = UtIdHelper.CachedUtIds.FirstOrDefault();
                if (utId != null)
                    comments += " " + utId;

                if (SelectedBorrower.RegionEnum != RegionSelectionEnum.OneLINK)
                {
                    var arc = new ArcData(DataAccessHelper.CurrentRegion)
                    {
                        AccountNumber = SelectedBorrower.SSN,
                        Arc = "P203A",
                        ArcTypeSelected = ArcData.ArcType.Atd22AllLoans,
                        Comment = comments,
                        ScriptId = ScriptId,
                    };
                    var results = arc.AddArc();
                    if (!results.ArcAdded)
                    {
                        messages.Add($"Unable to add arc {arc.Arc}.  Please leave an Activity Comment manually.");
                        messages.AddRange(results.Errors);
                    }
                }
                else
                {
                    var arc = new ArcData(DataAccessHelper.CurrentRegion)
                    {
                        AccountNumber = SelectedBorrower.SSN,
                        Arc = "DGNRL",
                        ActivityType = "MS",
                        ActivityContact = "96",
                        ArcTypeSelected = ArcData.ArcType.OneLINK,
                        Comment = comments,
                        ScriptId = ScriptId,
                    };
                    var results = arc.AddArc();
                    if (!results.ArcAdded)
                    {
                        messages.Add($"Unable to add arc {arc.Arc}.  Please leave an Activity Comment manually.");
                        messages.AddRange(results.Errors);
                    }
                }

                Dialog.Def.Ok(string.Join(Environment.NewLine, messages));
                CleanTempFiles();
                ResetButton.PerformClick();
                BorrowerSearch.Focus();
            }
            else
                Uheaa.Common.Dialog.Def.Ok(ImageButton.Text);
        }

        private string Guid()
        {
            return System.Guid.NewGuid().ToString().Replace("-", "");
        }

        string currentTempPdf = null;
        private void CopyButton_Click(object sender, EventArgs e)
        {
            if (currentTempPdf != null)
            {
                ResetBrowser();
                TempFilesToDelete.Push(currentTempPdf);
                currentTempPdf = null;
                SetButtonText();
                return;
            }
            currentTempPdf = Path.Combine(TempFolder, Guid() + ".pdf");
            var app = new Microsoft.Office.Interop.Word.Application();

            var doc = app.Documents.Add();
            try
            {
                doc.Paragraphs.Add().Range.Paste();
                doc.ExportAsFixedFormat(currentTempPdf, WdExportFormat.wdExportFormatPDF);
                PreviewBrowser.Navigate(currentTempPdf);
                PreviewBrowser.Visible = true;
            }
            catch (COMException)
            {
                currentTempPdf = null;
                Dialog.Def.Ok("Couldn't find any usable data in the clipboard.");
                return;
            }
            finally
            {
                doc.Close(false); //don't save changes, we already exported them
                app.Quit();
            }
            SetButtonText();
        }

        private void ResetBrowser()
        {
            var size = PreviewBrowser.Size;
            var location = PreviewBrowser.Location;
            var anchor = PreviewBrowser.Anchor;
            PreviewBrowser.Dispose();
            currentTempPdf = null;
            EmailPasteGroup.Controls.Remove(PreviewBrowser);
            PreviewBrowser = new WebBrowser();
            EmailPasteGroup.Controls.Add(PreviewBrowser);
            PreviewBrowser.Location = location;
            PreviewBrowser.Size = size;
            PreviewBrowser.Anchor = anchor;
            PreviewBrowser.Visible = false;
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            Attachments.Clear();
            AttachmentsButton.Text = "Add Attachments";
            AccountIdentifierBox.Clear();
            BorrowerSearch.Reset();
            LetterBox.DataSource = null;
            LetterBox.Enabled = false;
            LetterBox.SelectedIndex = -1;
            ActivityCommentBox.Text = "";
            ResetBrowser();
            BorrowerResults.SetResults(new List<QuickBorrower>());
            SelectedBorrower = null;
            SetButtonText();
        }

        private void TempTimer_Tick(object sender, EventArgs e)
        {
            CleanTempFiles();
        }

        private ConcurrentStack<string> TempFilesToDelete = new ConcurrentStack<string>();
        private void CleanTempFiles()
        {
            lock (TempFilesToDelete)
            {
                foreach (var file in TempFilesToDelete)
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                        //eat the exception, the browser probably still has a lock on the file
                    }
            }
        }

        BindingList<string> Attachments = new BindingList<string>();

        private void AttachmentsButton_Click(object sender, EventArgs e)
        {
            new AttachmentsForm(Attachments).ShowDialog();
            if (!Attachments.Any())
                AttachmentsButton.Text = "Add Attachments";
            else
                AttachmentsButton.Text = $"Attachments ({Attachments.Count})";

            SetButtonText();
        }

        private void AccountIdentifierButton_Click(object sender, EventArgs e)
        {
            if (AccountIdentifierBox.TextLength < 9)
            {
                Dialog.Def.Ok("Please enter an SSN or Account Number.");
                return;
            }
            var template = new SearchBorrower() { AccountIdentifier = AccountIdentifierBox.Text };
            var results = QuickBorrower.Search(template, RegionSelectionEnum.All, PLR.LDA);
            if (!DA.HasOnelinkAccess)
                results = results.Where(o => o.RegionEnum != RegionSelectionEnum.OneLINK).ToList();
            if (!DA.HasUheaaAccess)
                results = results.Where(o => o.RegionEnum != RegionSelectionEnum.Uheaa).ToList();
            if (results.Any())
            {
                BorrowerSearch.Reset();
                BorrowerResults.SetResults(results);
            }
            else
                Dialog.Def.Ok("Borrower not found.");
        }

        private void AccountIdentifierBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                AccountIdentifierButton.PerformClick();
        }
    }
}

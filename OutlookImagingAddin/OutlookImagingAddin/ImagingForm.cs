using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.WinForms;

namespace OutlookImagingAddin
{
    public partial class ImagingForm : Form
    {
        readonly DataAccess DA;
        readonly MailItem email;
        public ImagingForm(ProcessLogRun plr, MailItem email)
        {
            InitializeComponent();
            LetterBox.DisplayMember = "Description";
            LetterBox.ValueMember = "DocId";
            ImageButton.Tag = ImageButton.Text;
            BorrowerSearch.LDA = plr.LDA;
            DA = new DataAccess(plr);
            this.email = email;
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
                else
                    ImageButton.Text = $"Image this email for Borrower {SelectedBorrower.FullName}, Letter {SelectedLetterId}";
            }
        }

        private void BorrowerSearch_OnSearchResultsRetrieved(SimpleBorrowerSearchControl sender, List<QuickBorrower> results)
        {
            BorrowerResults.SetResults(results);
        }

        private void BorrowerResults_OnBorrowerChosen(object sender, QuickBorrower selected)
        {
            SelectedBorrower = selected;
            List<Document> dataSource;
            if (selected.RegionEnum == RegionSelectionEnum.CornerStone)
                dataSource = DA.CornerstoneDocuments;
            else
                dataSource = DA.UheaaDocuments;
            dataSource.Insert(0, new Document() { });
            LetterBox.DataSource = dataSource;
            LetterBox.Enabled = true;
            SetButtonText();
        }

        private void LetterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectedLetterId = (LetterBox.SelectedItem as Document)?.DocId;
            SetButtonText();
        }

        private void ImageButton_Click(object sender, EventArgs e)
        {
            if (SelectedBorrower != null && SelectedLetterId != null)
            {
                var gen = new ImagingGenerator("OutlookImagingAddin", Environment.UserName);
                var pdf = new PdfHelper();
                var filename = Guid() + ".pdf";
                gen.AddFile(Path.Combine(gen.ImagingPath, filename), SelectedLetterId, SelectedBorrower.SSN);
                pdf.ConvertMsg(email, Path.Combine(gen.Folder, filename));
                foreach (Attachment attach in email.Attachments)
                {
                    string attachName = Guid() + attach.FileName;
                    attach.SaveAsFile(Path.Combine(gen.Folder, attachName));
                    gen.AddFile(Path.Combine(gen.ImagingPath, attachName), SelectedLetterId, SelectedBorrower.SSN);
                }
                gen.PublishControlFile();
                Dialog.Def.Ok("This email was successfully imaged at " + gen.ImagingPath);
                this.DialogResult = DialogResult.OK;
            }
            else
                Dialog.Def.Ok(ImageButton.Text);
        }

        private string Guid()
        {
            return System.Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}

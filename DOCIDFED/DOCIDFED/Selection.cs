using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;
using Uheaa.Common.WinForms;

namespace DOCIDFED
{
    public partial class Selection : Form
    {
        public Borrower Borrower { get; set; }
        public List<Borrower> Bors { get; set; }
        public List<Borrower> BankruptcyBors { get; set; }
        private DataAccess DA { get; set; }
        private QuickBorrower QBorrower { get; set; }
        public List<Doc> DocIds { get; set; }
        public List<Doc> BankruptyIds { get; set; }
        public Doc SelectedDocId { get; set; }
        private int TotalProcessed { get; set; }
        public ProcessLogRun LogRun { get; set; }
        public Func<ProcessedDocuments, object> Predicate { get; set; }
        public bool IsDescending { get; set; }
        public int BorrowerCount { get; set; }
        public bool ProcessingBankruptcy { get; set; }
        public int? IdIndex { get; set; }
        public int? TypeIndex { get; set; }
        public bool IsCorrFax { get; set; }
        public string DocSource { get; set; }
        public bool AddTd22 { get; set; }
        public bool FoundBorrower { get; set; }

        public Selection(ProcessLogRun logRun, DataAccess da)
        {
            InitializeComponent();
            Predicate = p => p.AddedAt;
            DA = da;
            Borrower = new Borrower();
            LoadProcessedRecords();
            LogRun = logRun;
            Bors = new List<Borrower>();
            BankruptcyBors = new List<Borrower>();
            Version v = Assembly.GetEntryAssembly().GetName().Version;
            this.Text = string.Format("{0} :: {1}.{2}.{3}.{4}", this.Text, v.Major, v.Minor, v.Build, v.Revision);
        }

        /// <summary>
        /// Uses the SearchForm form to search all regions
        /// </summary>
        private void LookupButton_Click(object sender, EventArgs e)
        {
            SearchForm search = new SearchForm();
            search.ShowDialog();
            QBorrower = search.Borrower;
            AcctId.Text = QBorrower != null ? QBorrower.SSN : AcctId.Text;
            search.Dispose();
            AcctId.Focus();
        }

        /// <summary>
        /// Verifies the account identifier is great than 9 digits before calling the search feature
        /// </summary>
        private void AcctId_TextChanged(object sender, EventArgs e)
        {
            if (AcctId.Text.StartsWith(" "))
                AcctId.Text = AcctId.Text.Trim();
            Error.Text = "";
            if (AcctId.Text.ToUpper().StartsWith("P"))
            {
                BorrowerName.Text = "Borrower Not Found";
                return;
            }
            if (AcctId.Text.Length >= 7)
            {
                if (!FoundBorrower)
                {
                    Borrower = new Borrower();
                    Bors = new List<Borrower>();
                    FindBorrower();
                    LoadBorrowerInfo();
                    if (BankruptcyBors.Count > 0)
                        LoadBankruptcy();
                    if (Borrower != null && Borrower.Ssn.IsPopulated())
                        AcctId.Text = Borrower.Ssn;
                    FoundBorrower = false; //Reset so you can do another search
                }
            }
            else
            {
                UheaaRegion.Text = "UHEAA";
                ClearForm();
                LoadBorrowerInfo();
                DisableFields();
                if (!ProcessingBankruptcy)
                    BankruptcyBors = new List<Borrower>();
                AcctId.Enabled = true;
                LookupButton.Enabled = true;
                FoundBorrower = false;
            }
        }

        /// <summary>
        /// Sets the DataAccessHelper.Region to cornerstone and searches for the borrower information
        /// </summary>
        private void CornerstoneRegion_CheckedChanged(object sender, EventArgs e)
        {
            if (CornerstoneRegion.Checked && AcctId.Text.Length >= 9)
            {
                Borrower = Bors.Where(p => p.IsFederal).FirstOrDefault();
                if (Borrower.AccountIdentifier.IsPopulated())
                {
                    LoadFields();
                    LoadBorrowerInfo();
                }
            }
        }

        /// <summary>
        /// Sets the DataAccessHelper.Region to uheaa and searches for the borrower information
        /// </summary>
        private void UheaaRegion_CheckedChanged(object sender, EventArgs e)
        {
            if (UheaaRegion.Checked && AcctId.Text.Length >= 9)
            {
                Borrower = Bors.Where(p => !p.IsFederal).FirstOrDefault();
                if (Bors.Any(p => p.IsOneLink))
                {
                    Borrower = Bors.Where(p => p.IsOneLink).FirstOrDefault();
                    UheaaRegion.Text = "OneLink";
                }
                if (Borrower.AccountIdentifier.IsPopulated())
                {
                    LoadFields();
                    LoadBorrowerInfo();
                }
            }
        }

        /// <summary>
        /// Validates the document type/id is selected then adds the ARC to ArcAddProcessing
        /// </summary>
        private void ProcessButton_Click(object sender, EventArgs e)
        {
            if (DocumentIdCombo.Text.IsNullOrEmpty() && DocumentType.Text.IsNullOrEmpty())
            {
                ProcessButton.Enabled = false;
                MessageBox.Show("Please choose a document type or ID to continue", "No document selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Only set to true if the borrower is in OneLink, if they are in Compass, it will already do the TD22
            if (((Doc)DocIds.Where(p => p.DocId == SelectedDocId.DocId).FirstOrDefault()).AddTd22 && UheaaRegion.Text == "OneLink")
                AddTd22 = true;
            else
                AddTd22 = false;

            if (ProcessArc(true))
            {
                if (AddTd22 && !Borrower.AccountIdentifier.StartsWith("RF@"))
                    ProcessTd22Arc();
                if (ProcessingBankruptcy) //Gets the next borrower in the bankruptcy
                    SetupNextBorrower();

                if (BankruptcyBors != null && BankruptcyBors.Count == BorrowerCount)
                {
                    BankruptcyBors = new List<Borrower>();
                    Bors = new List<Borrower>();
                    ProcessingBankruptcy = false; //Set back to false so the next button becomes invisible
                }
            }
            if (BankruptcyBors.Count > 0)
                ProcessButton.Focus();
            else
                AcctId.Focus();
            FoundBorrower = false;
        }

        private void ProcessTd22Arc()
        {
            Borrower.CompassOnly = true;
            CorrFax.Checked = IsCorrFax;
            int id = IdIndex.HasValue ? IdIndex.Value : 0;
            int type = TypeIndex.HasValue ? TypeIndex.Value : 0;
            SelectedDocId = DocIds.Where(p => p.MappingId == (IdIndex > 0 ? IdIndex : TypeIndex)).FirstOrDefault();
            SelectedDocId.DocSource = (Doc.Source)Enum.Parse(typeof(Doc.Source), DocSource);
            UpdateLabels(SelectedDocId);
            SelectedDocId.Arc = SelectedDocId.OriginalARC;
            AddTd22 = false;
            ProcessArc(false);
            IdIndex = id;
            TypeIndex = type;
        }

        /// <summary>
        /// Gets ARC data and adds ARC to ArcAddProcessing tables
        /// </summary>
        private bool ProcessArc(bool count)
        {
            Borrower tempBor = Borrower;
            ProcessedDocuments record = LoadRecordData();
            if (SelectedDocId.CreateQueue && !SelectedDocId.QueueCreated && Borrower.IsOneLink)
                if (!AddQueue())
                    return false;
            int arcId = AddArc();
            if (arcId > 0)
            {
                if (!Borrower.RecordAdded)
                {
                    DA.InsertProcessedRecord(record, arcId);
                    Borrower.RecordAdded = true;
                }
                IdIndex = DocIds.Where(p => p.DocId == DocumentIdCombo.Text).FirstOrDefault()?.MappingId;
                TypeIndex = DocIds.Where(p => p.DocType == DocumentType.Text).FirstOrDefault()?.MappingId;
                IsCorrFax = CorrFax.Checked;
                DocSource = SelectedDocId.DocSource.ToString();
                ClearForm();
                if (count)
                    ++TotalProcessed;
                LoadProcessedRecords();
                AcctId.Text = "";
                if (AddTd22)
                    Borrower = tempBor;
                return true;
            }
            return false;
        }

        private void Skip_Click(object sender, EventArgs e)
        {
            IdIndex = DocumentIdCombo.Text.IsPopulated() ? DocIds.Where(p => p.DocId == DocumentIdCombo.Text).FirstOrDefault().MappingId : 0;
            TypeIndex = DocumentType.Text.IsPopulated() ? DocIds.Where(p => p.DocType == DocumentType.Text).FirstOrDefault().MappingId : 0;
            IsCorrFax = CorrFax.Checked;
            SetupNextBorrower();
            AcctId.Focus();
            if (BorrowerCount == BankruptcyBors.Count - 1)
                Skip.Visible = false;
        }

        private void SetupNextBorrower()
        {
            ++BorrowerCount;
            if (BorrowerCount < BankruptcyBors.Count)
            {
                AcctId.Text = BankruptcyBors[BorrowerCount].AccountIdentifier;
                DocumentIdCombo.Text = BankruptyIds.Where(p => p.MappingId == IdIndex).FirstOrDefault() != null ? BankruptyIds.Where(p => p.MappingId == IdIndex).FirstOrDefault().DocId : "";
                DocumentType.Text = BankruptyIds.Where(p => p.MappingId == TypeIndex).FirstOrDefault() != null ? BankruptyIds.Where(p => p.MappingId == TypeIndex).FirstOrDefault().DocType : "";
                CorrFax.Checked = IsCorrFax;
                if (BorrowerCount == BankruptcyBors.Count - 1)
                    Skip.Visible = false;
            }
            else if (BorrowerCount == BankruptcyBors.Count)
            {
                Skip.Visible = false;
                AcctId.Text = "";
                BankruptcyBors = new List<Borrower>();
                Bors = new List<Borrower>();
            }
        }

        /// <summary>
        /// Chooses the Document Type and ARC to be added to the borrower account
        /// </summary>
        private void DocumentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DocumentIdCombo.SelectedIndex == 0 && DocumentType.SelectedIndex == 0)
                ProcessButton.Enabled = false;
            UpdateLabels(null);
            if (DocumentType.SelectedIndex > 0)
            {
                DocumentIdCombo.SelectedIndex = 0;
                UpdateLabels(null);
            }
        }

        /// <summary>
        /// Chooses the Document ID and ARC to be added to the borrower account
        /// </summary>
        private void DocumentIdCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DocumentIdCombo.SelectedIndex == 0 && DocumentType.SelectedIndex == 0)
                ProcessButton.Enabled = false;
            UpdateLabels(null);
            if (DocumentIdCombo.SelectedIndex > 0)
            {
                DocumentType.SelectedIndex = 0;
                UpdateLabels(null);
            }
        }

        /// <summary>
        /// Refreshes the grid view with the newest data
        /// </summary>
        private void Refresh_Click(object sender, EventArgs e)
        {
            LoadProcessedRecords();
        }

        /// <summary>
        /// calls the refresh for the grid view with the newly selected date
        /// </summary>
        private void DateSelection_ValueChanged(object sender, EventArgs e)
        {
            LoadProcessedRecords();
        }

        /// <summary>
        /// Resizes all the columns in the processed grid view. This has to be done after the 
        /// </summary>
        private void Selection_Validated(object sender, EventArgs e)
        {
            //Need to invoke the Auto Resize for the columns on a new thread
            if (this.IsHandleCreated)
                this.BeginInvoke(new Action(() =>
                {
                    DailyProcessed.AutoResizeColumns();
                }));
        }

        /// <summary>
        /// Chooses which column will be sorted
        /// </summary>
        private void DailyProcessed_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 0:
                    Predicate = p => p.AccountIdentifier;
                    break;
                case 1:
                    Predicate = p => p.Document;
                    break;
                case 2:
                    Predicate = p => p.IsFederal;
                    break;
                case 3:
                    Predicate = p => p.Source;
                    break;
                case 4:
                    Predicate = p => p.ProcessedAt;
                    break;
                case 5:
                    Predicate = p => p.Comment;
                    break;
                case 6:
                    Predicate = p => p.AddedBy;
                    break;
                case 7:
                    Predicate = p => p.AddedAt;
                    break;
                case 8:
                    Predicate = p => p.ArcAddProcessingId;
                    break;
            }
            LoadProcessedRecords();
            IsDescending = !IsDescending;
        }

        /// <summary>
        /// Gets all the records for the selected date and updates the header in the grid view to more descriptive names
        /// </summary>
        private void LoadProcessedRecords()
        {
            SetDateSelection();
            List<ProcessedDocuments> docs = DA.GetProcessedRecords(DateSelection.Value).OrderByDescending(p => p.AddedAt).ToList();
            docs = SetOrder(docs);
            DailyProcessed.DataSource = docs;
            DailyProcessed.Columns[0].HeaderText = "SSN/Acct";
            DailyProcessed.Columns[1].HeaderText = "Doc ID";
            DailyProcessed.Columns[2].HeaderText = "Federal";
            DailyProcessed.Columns[4].HeaderText = "Processed";
            DailyProcessed.Columns[8].HeaderText = "ArcAddId";
            if (docs.Count > 0)
                ProcessedCount.Text = string.Format("Daily Total: {0}     Session Total: {1}", docs.Count, TotalProcessed);
            else
                ProcessedCount.Text = "";
            //Need to invoke the Auto Resize for the columns on a new thread
            if (this.IsHandleCreated)
                this.BeginInvoke(new Action(() =>
                {
                    DailyProcessed.AutoResizeColumns();
                }));
        }

        /// <summary>
        /// Sets the sort order for the data in the grid view.
        /// </summary>
        /// <param name="docs"></param>
        /// <returns></returns>
        private List<ProcessedDocuments> SetOrder(List<ProcessedDocuments> docs)
        {
            if (IsDescending)
                return docs.OrderBy(Predicate).ToList();
            else
                return docs.OrderByDescending(Predicate).ToList();
        }

        /// <summary>
        /// Gets the document id and document type data from the database and loads the combo boxes
        /// </summary>
        private void LoadFields()
        {
            DocIds = DA.GetDocIds(Borrower.IsFederal);
            UpdateDocType(DocIds);
            CheckBankruptyBorrower();

            List<Doc> id = new List<Doc>(DocIds.Where(p => (p.BU && !Borrower.IsFederal) || Borrower.IsFederal).OrderBy(p => p.DocId));
            List<Doc> type = new List<Doc>(DocIds.Where(p => (p.PO && !Borrower.IsFederal || Borrower.IsFederal)).OrderBy(p => p.DocType));
            id = NarrowIds(id);
            type = NarrowType(type);

            DocumentIdCombo.DataSource = id;
            DocumentIdCombo.DisplayMember = "DocId";
            DocumentIdCombo.ValueMember = "MappingId";

            DocumentType.DataSource = type;
            DocumentType.DisplayMember = "DocType";
            DocumentType.ValueMember = "MappingId";
        }

        private void CheckBankruptyBorrower()
        {
            //Check to make sure that the borrower is in the list of bankruptcy borrowers. If the user was searching for bankruptcy then changes the borrower, it needs to clear out the bankruptcy list.
            if (!BankruptcyBors.Any(p => p.AccountIdentifier.Contains(AcctId.Text)))
                BankruptcyBors = new List<Borrower>();
        }

        /// <summary>
        /// Narrow down the doc id's availabe in the drop down according to the type of borrower
        /// </summary>
        public List<Doc> NarrowIds(List<Doc> id)
        {
            List<Doc> ids = id;
            if (AcctId.Text.ToUpper().StartsWith("RF@") || AcctId.Text.ToUpper().StartsWith("P"))
                ids = id.Where(p => p.DocId.ToUpper().StartsWith("SKREF")).ToList();//OneLink reference can only use the SKREF Doc ID

            if (Borrower.CompassOnly)
                ids = id.Where(p => p.Arc.IsPopulated()).ToList();//Only allow the Compass document

            if (BankruptcyBors.Count > 0)
            {
                ids = id.Where(p => p.DocType.ToUpper().StartsWith("BANKRUPTCY") && (Borrower.CompassOnly ? p.Arc.IsPopulated() : p.DocId.IsPopulated())).ToList();//Only allow bankruptcy documents
                BankruptyIds = ids;
            }

            ids.Insert(0, Doc.Empty);
            return ids;
        }

        /// <summary>
        /// Narrow down the doc type's availabe in the drop down according to the type of borrower
        /// </summary>
        public List<Doc> NarrowType(List<Doc> type)
        {
            List<Doc> types = type;
            if (AcctId.Text.ToUpper().StartsWith("RF@") || AcctId.Text.ToUpper().StartsWith("P"))
                types = type.Where(p => p.DocType.ToUpper().StartsWith("REFERENCE SKIP LETTER")).ToList();//OneLink reference can only use the SKREF Doc ID

            if (Borrower.CompassOnly)
                types = type.Where(p => p.Arc.IsPopulated()).ToList();//Only allow the Compass document

            if (BankruptcyBors.Count > 0)
                types = type.Where(p => p.DocType.ToUpper().StartsWith("BANKRUPTCY") && (Borrower.CompassOnly ? p.Arc.IsPopulated() : p.DocType.IsPopulated())).ToList();//Only allow bankruptcy documents

            types.Insert(0, Doc.Empty);
            return types;
        }

        /// <summary>
        /// Adding the DocType and DocId to the same string to show both in the drop down
        /// </summary>
        /// <param name="docIds"></param>
        private void UpdateDocType(List<Doc> docIds)
        {
            foreach (Doc type in docIds)
            {
                if (type.DocId.Trim().IsPopulated() || type.DocType.Trim().IsPopulated())
                {
                    type.DocType = $"{type.DocType} - {type.DocId}";
                    type.DocId = $"{type.DocId} - {type.DocType}";
                    if (UheaaRegion.Checked && !Borrower.CompassOnly)
                    {
                        type.OriginalARC = type.Arc;
                        type.Arc = "MDCID"; //If OneLink, always use ARC of MDCID
                    }
                    else if (UheaaRegion.Checked && (AcctId.Text.ToUpper().StartsWith("RF@") || AcctId.Text.ToUpper().StartsWith("P")) && Borrower.CompassOnly)
                        type.Arc = "SKREF"; //If reference and compass only, use the SKREF ARC
                }
            }
        }

        /// <summary>
        /// Adds the borrowers demographics information to the string to display the selected borrower
        /// </summary>
        private void LoadBorrowerInfo()
        {
            if (Borrower != null && Borrower.AccountIdentifier.IsPopulated() && Borrower.Name.IsPopulated())
                BorrowerName.Text = $"{Borrower.Name} \r\n{Borrower.Address1} {((Borrower.Address2.IsPopulated() ? "\r\n" : "") + Borrower.Address2)}\r\n{Borrower.City}{(Borrower.City.IsPopulated() ? "," : "")} {Borrower.State} {Borrower.Zip}\r\n{Borrower.Country}";
            else if (AcctId.Text.Length >= 9 && (Borrower == null || Borrower.AccountIdentifier.IsNullOrEmpty()))
                BorrowerName.Text = "Borrower Not Found";
            else
                BorrowerName.Text = "";
        }

        private void LoadBankruptcy()
        {
            ProcessingBankruptcy = true;
            AcctId.Enabled = false;
            LookupButton.Enabled = false;
            if (BankruptcyBors.Count > 1)
                Skip.Visible = true;
        }

        /// <summary>
        /// Searched the warehouses to find the borrowers demographics
        /// </summary>
        private void FindBorrower()
        {
            Uncheck();
            DisableFields();
            if (AcctId.Text.Length > 8)
                Bors = DA.GetBorrowerDemographics(AcctId.Text);
            if (Bors.Count == 0 || (Bors.Count == 1 && Bors[0].IsOneLink))
                CheckIfEndorser();
            if (BankruptcyBors.Count == 0)
                CheckBankruptcy();
            if (Bors.Count > 0)
            {
                CheckIfCompassOnly(Bors);
                Borrower = CheckBorrowerRegion(Bors);
                if (Borrower != null)
                {
                    Borrower.AccountIdentifier = AcctId.Text;
                    FoundBorrower = true;
                    EnableFields();
                }
            }
        }

        private void CheckBankruptcy()
        {
            BankruptcyBors = new List<Borrower>();
            List<Borrower> bankBors = new List<Borrower>();
            if (BankruptcyBors.Count == 0)
                bankBors = DA.CaseSearch(AcctId.Text);
            if (bankBors != null && bankBors.Count > 0)
            {
                BorrowerCount = 0;
                BankruptcyBors = bankBors;
                AcctId.Text = BankruptcyBors[BorrowerCount].AccountIdentifier;
            }
        }

        /// <summary>
        /// Checks to see if the borrower has loans in OneLink or just compass
        /// </summary>
        /// <param name="bors"></param>
        private void CheckIfCompassOnly(List<Borrower> bors)
        {
            if (!bors.Any(p => p.IsOneLink))
                foreach (Borrower b in bors)
                    b.CompassOnly = true;
        }

        /// <summary>
        /// Looks up the account identifier to see if they are an 
        /// </summary>
        private void CheckIfEndorser()
        {
            if (Borrower != null)
            {
                List<Borrower> endorsers = DA.GetBorrowersForEndorser(AcctId.Text);
                if (Bors.Count == 1 && Bors[0].IsOneLink && endorsers.Count > 0)
                {
                    if (!Dialog.Def.YesNo("This account was found in OneLink but they are also listed as an endorser to another borrower in Compass. Do you want to search for borrowers they are tied to as an endorser?", "Search for Borrowers?"))
                        return;
                }
                if (endorsers.Count > 0)
                {
                    if (Borrower.AccountIdentifier != null && endorsers.Any(p => p.IsFederal == Borrower.IsFederal && p.IsOneLink == Borrower.IsOneLink))
                        return;
                    EndorserSearch search = new EndorserSearch(DA, endorsers);
                    search.ShowDialog();
                    string acctId = search.SelectedBorrower != null ? search.SelectedBorrower.AccountIdentifier : "";
                    search.Dispose();
                    AcctId.Text = acctId;
                    if (AcctId.Text.IsNullOrEmpty())
                        Bors = new List<Borrower>();
                }
            }
        }

        /// <summary>
        /// Looks at all the borrower demographics found and determines if the borrower is in multiple regions. If they are, it asks which region to choose. If they are in OneLink and Compass but not federal, it defaults to OneLink
        /// </summary>
        /// <param name="bors"></param>
        /// <returns></returns>
        private Borrower CheckBorrowerRegion(List<Borrower> bors)
        {
            Borrower bor = new Borrower();
            if (bors.Count > 1 && !(UheaaRegion.Checked || CornerstoneRegion.Checked))
            {
                if (bors.Any(p => p.IsFederal) && bors.Any(p => !p.IsFederal))
                {
                    DialogResult result = MessageBox.Show("The borrower you selected is in CornerStone and UHEAA. Do you want to process this borrower in CornerStone?", "No for UHEAA", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                        bor = bors.Where(p => p.IsFederal).FirstOrDefault();
                    else
                        bor = bors.Any(p => p.IsOneLink) ? bors.Where(p => p.IsOneLink).FirstOrDefault() : bors.Where(p => p.CompassOnly && !p.IsFederal).FirstOrDefault();
                }
                else
                    bor = bors.FirstOrDefault();
            }
            else if (bors != null && !(UheaaRegion.Checked || CornerstoneRegion.Checked))
                bor = bors.FirstOrDefault();
            if ((Borrower != null && Borrower.AccountIdentifier.IsPopulated()) || (bor != null && bor.AccountIdentifier.IsPopulated()))
            {
                if (Borrower.IsFederal || bor.IsFederal)
                    CornerstoneRegion.Checked = true;
                else
                    UheaaRegion.Checked = true;
                if ((BankruptcyBors.Count > 0 && BankruptcyBors.Any(p => p.IsOneLink)) || (!bor.IsFederal && bors.Any(p => p.IsOneLink)))
                    UheaaRegion.Text = "OneLink";
                else
                    UheaaRegion.Text = "UHEAA";
            }
            else
                Uncheck(); //Borrower was not found so the region and corr fax radio buttons are unselected
            return bor.AccountIdentifier.IsPopulated() ? bor : Borrower;
        }

        /// <summary>
        /// Borrower was not found so the region and corr fax radio buttons are unselected
        /// </summary>
        private void Uncheck()
        {
            CornerstoneRegion.Checked = false;
            UheaaRegion.Checked = false;
            CorrFax.Checked = false;
        }

        private void EnableFields()
        {
            if (Bors.Any(p => !p.IsFederal))
                UheaaRegion.Enabled = true;
            if (Bors.Any(p => p.IsFederal))
                CornerstoneRegion.Enabled = true;
            DocumentIdCombo.Enabled = true;
            DocumentType.Enabled = true;
            CorrFax.Enabled = true;
        }

        private void DisableFields()
        {
            UheaaRegion.Enabled = false;
            CornerstoneRegion.Enabled = false;
            DocumentIdCombo.Enabled = false;
            DocumentType.Enabled = false;
            CorrFax.Enabled = false;
        }

        /// <summary>
        /// Adds the selected document data to the new ProcessedDocuments object that will be added to the DocumentsProcessed table
        /// </summary>
        private ProcessedDocuments LoadRecordData()
        {
            SetSource();
            ProcessedDocuments record = new ProcessedDocuments();
            record.AccountIdentifier = AcctId.Text.IsPopulated() ? AcctId.Text : Borrower.AccountIdentifier;
            record.Document = SelectedId.Text;
            record.IsFederal = CornerstoneRegion.Checked;
            record.Source = SelectedDocId.DocSource.ToString();
            return record;
        }

        /// <summary>
        /// Clears out the fields on the form
        /// </summary>
        private void ClearForm()
        {
            Borrower = null;
            QBorrower = null;
            DocumentType.DataSource = null;
            DocumentIdCombo.DataSource = null;
            ProcessButton.Enabled = false;
            Uncheck();
            ProcessButton.Enabled = false;
            SelectedId.Text = "";
            SelectedType.Text = "";
            SelectedArc.Text = "";
        }

        /// <summary>
        /// Adds text to fields on the form to display the Document Type, Document ID and ARC selected before processing
        /// </summary>
        private void UpdateLabels(Doc docId)
        {
            SelectedArc.Text = "";
            SelectedId.Text = "";
            SelectedType.Text = "";
            if ((DocumentIdCombo.Text.Trim().IsPopulated() || DocumentType.Text.Trim().IsPopulated()) || docId != null)
            {
                GetSelectedDocId();
                if (SelectedDocId != null)
                {
                    SelectedArc.Text = SelectedDocId.Arc;
                    SelectedId.Text = SelectedDocId.DocId.SplitAndRemoveQuotes("-").FirstOrDefault().Trim();
                    SelectedType.Text = SelectedDocId.DocType.Remove(SelectedDocId.DocType.Length - 8);
                    ProcessButton.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Sets the Document ID selected by the user
        /// </summary>
        private void GetSelectedDocId()
        {
            if (DocumentIdCombo.Text.IsPopulated())
                SelectedDocId = DocIds.Where(p => p.DocId == DocumentIdCombo.Text).SingleOrDefault();
            else if (DocumentType.Text.IsPopulated())
                SelectedDocId = DocIds.Where(p => p.DocType == DocumentType.Text).SingleOrDefault();
        }

        /// <summary>
        /// Adds the ARC to the ArcAddProcessing table
        /// </summary>
        /// <returns>ArcAddProcessingID</returns>
        private int AddArc()
        {
            ArcData arc = new ArcData(Borrower.IsFederal ? DataAccessHelper.Region.CornerStone : DataAccessHelper.Region.Uheaa)
            {
                AccountNumber = Borrower.AccountIdentifier,
                Arc = SelectedDocId.Arc,
                Comment = string.Format("{0} received, Doc ID {1}. FROM: {2}", SelectedType.Text, SelectedId.Text, SelectedDocId.DocSource.ToString()),
                ArcTypeSelected = (Borrower.CompassOnly || Borrower.IsFederal) ? ArcData.ArcType.Atd22AllLoans : ArcData.ArcType.OneLINK,
                IsEndorser = false,
                IsReference = Borrower.RecipientId.IsPopulated(),
                ProcessOn = DateTime.Now,
                ScriptId = Borrower.IsFederal ? "DOCIDFED" : "DOCID",
                ActivityType = (Borrower.CompassOnly || Borrower.IsFederal) ? null : "AM",
                ActivityContact = (Borrower.CompassOnly || Borrower.IsFederal) ? null : "10",
                RecipientId = Borrower.IsFederal ? Borrower.Ssn : Borrower.RecipientId
            };
            ArcAddResults result = arc.AddArc();
            if (result.Errors.Count > 0 || !result.ArcAdded)
            {
                LogRun.AddNotification(string.Join(",", result.Errors), NotificationType.ErrorReport, NotificationSeverityType.Warning, result.Ex);
                Error.Text = "The ARC was canceled due to an unforseen error";
            }
            return result.ArcAddProcessingId;
        }

        /// <summary>
        /// Sets the source of where the document came from
        /// </summary>
        private void SetSource()
        {
            if (CorrFax.Checked)
                SelectedDocId.DocSource = DOCIDFED.Doc.Source.CF;
            else if (DocumentType.Text.IsPopulated())
                SelectedDocId.DocSource = DOCIDFED.Doc.Source.PO;
            else if (DocumentIdCombo.Text.IsPopulated())
                SelectedDocId.DocSource = DOCIDFED.Doc.Source.BU;
        }

        /// <summary>
        /// Set the earliest date to the earliest in the database. Don't allow future dates.
        /// </summary>
        private void SetDateSelection()
        {
            DateSelection.MinDate = DA.GetEarliestDate();
            DateSelection.MaxDate = DateTime.Now.Date;
        }

        private bool AddQueue()
        {
            try
            {
                DataAccessHelper.CurrentRegion = Borrower.IsFederal ? DataAccessHelper.Region.CornerStone : DataAccessHelper.Region.Uheaa;
                ReflectionInterface ri = new ReflectionInterface();
                Thread.Sleep(2000);
                int count = 0;
                while (!ri.IsLoggedIn && count < 5)
                {
                    Dialog.Info.Ok("Please log into the session and hit INSERT to add the queue");
                    ri.PauseForInsert();
                    count++;
                    if (count == 5)
                    {
                        string message = string.Format("Error logging into session for user: {0}", Environment.UserName);
                        LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                        SelectedDocId.QueueCreated = false;
                        return false;
                    }
                }
                AccessLP9O(ri);
                ri.CloseSession();
                SelectedDocId.QueueCreated = true;
            }
            catch (Exception ex)
            {
                string message = $"There was an error either opening a session or creating a queue in LP90A for borrower: {Borrower.Ssn}. Error: {ex}";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Warning, ex);
                Dialog.Warning.Ok(message);
            }

            return true;
        }

        private bool AccessLP9O(ReflectionInterface ri)
        {
            bool error = false;
            ri.FastPath("LP9OA" + Borrower.Ssn + ";;" + SelectedId.Text);
            if (!ri.CheckForText(22, 3, "44000"))
                error = true;
            else
            {
                ri.PutText(12, 25, DateTime.Now.AddHours(2).ToString("HHmm"));
                ri.PutText(16, 12, "work load queue task");
                ri.Hit(ReflectionInterface.Key.F6);
            }
            if (!ri.CheckForText(22, 3, "48003"))
                error = true;
            if (error)
            {
                string message = $"Error adding Queue: {SelectedId.Text} for Borrower: {AcctId.Text} in LP9OA; Error Message: {ri.AltMessage}. Please contact System Support and reference Process Log Id: {LogRun.ProcessLogId}";
                LogRun.AddNotification(message, NotificationType.ErrorReport, NotificationSeverityType.Critical);
                Dialog.Warning.Ok(message);
            }
            return error;
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            AcctId.Text = "";
            AcctId.Focus();
            BankruptcyBors = new List<Borrower>();
            Skip.Visible = false;
            Bors = new List<Borrower>();
            FoundBorrower = false;
        }

        private void Close_Leave(object sender, EventArgs e)
        {
            if (BankruptcyBors.Count == 0)
                AcctId.Focus();
            else if (CornerstoneRegion.Checked)
                CornerstoneRegion.Focus();
            else if (UheaaRegion.Checked)
                UheaaRegion.Focus();

        }

        private void DocumentType_TextChanged(object sender, EventArgs e)
        {
            if (DocumentType.Text.IsPopulated())
                DocumentIdCombo.Text = "";
            if (DocumentType.Text.IsPopulated() && !DocIds.Any(p => p.DocType == DocumentType.Text))
                ProcessButton.Enabled = false;
            if (DocumentType.Text.IsNullOrEmpty())
            {
                ProcessButton.Enabled = false;
                UpdateLabels(null);
            }
        }

        private void DocumentIdCombo_TextChanged(object sender, EventArgs e)
        {
            if (DocumentIdCombo.Text.IsPopulated())
                DocumentType.Text = "";
            if (DocumentIdCombo.Text.IsPopulated() && !DocIds.Any(p => p.DocId == DocumentIdCombo.Text))
                ProcessButton.Enabled = false;
            if (DocumentIdCombo.Text.IsNullOrEmpty())
            {
                ProcessButton.Enabled = false;
                UpdateLabels(null);
            }
        }
    }
}
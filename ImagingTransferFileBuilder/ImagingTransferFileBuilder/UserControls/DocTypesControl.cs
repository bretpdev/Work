using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common.DataAccess;
using Uheaa.Common;

namespace ImagingTransferFileBuilder.UserControls
{
    public partial class DocTypesControl : UserControl
    {
        public DocTypesControl()
        {
            InitializeComponent();
            if ((LicenseManager.UsageMode != LicenseUsageMode.Designtime)) //don't do this in design mode
            {
                SyncData();
                DocTypeMode = UIMode.NothingSelected;
            }
        }

        private List<DocType> cachedDocTypes;
        public void SyncData()
        {
            DocTypesBox.DataSource = cachedDocTypes = DocType.GetAll();
            DocTypesBox.DisplayMember = "DocTypeValue";
            DocTypesBox.SelectedIndex = -1;
            ImageScraper.RebuildCache();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            DocTypeMode = UIMode.AddingNewDocType;
            DocTypeBox.Focus();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (DocTypeMode == UIMode.AddingNewDocType || DocTypeMode == UIMode.DocTypeSelected)
            {
                DocTypesBox.SelectedIndex = -1;
                DocTypeMode = UIMode.NothingSelected;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (DocTypeMode == UIMode.AddingNewDocType)
            {
                DocType dt = new DocType() { DocTypeValue = DocTypeBox.Text };
                DocType.Insert(dt);
                AddDocIds(dt);
                SyncData();
                SelectedDocType = dt;
            }
            else if (DocTypeMode == UIMode.DocTypeSelected)
            {
                int index = DocTypesBox.SelectedIndex;
                DocType dt = SelectedDocType;
                if (dt.DocTypeValue != DocTypeBox.Text)
                {
                    //"delete" the doc type to maintain a history of changes
                    DocType.Delete(SelectedDocType);
                    dt = new DocType();
                    dt.DocTypeValue = DocTypeBox.Text;
                    DocType.Insert(dt);
                }
                AddDocIds(dt);
                SyncData();
                DocTypesBox.SelectedIndex = index;
            }
        }

        private void AddDocIds(DocType dt)
        {
            List<DocId> docIds = DocId.GetByDocType(dt);
            foreach (string line in DocIdsBox.Lines.Distinct())
            {
                if (line.IsNullOrEmpty()) continue;
                IEnumerable<DocId> oldDocIds = docIds.Where(o => o.DocIdValue == line).ToArray();
                if (oldDocIds.Count() == 0)
                {
                    DocId di = new DocId()
                    {
                        DocIdValue = line,
                        DocTypeId = dt.DocTypeId
                    };
                    DocId.Insert(di);
                }
                else
                    foreach (DocId old in oldDocIds) //no changes, remove from list
                        docIds.Remove(old);
            }
            //remove any doc ids no longer present
            foreach (DocId old in docIds)
                DocId.Delete(old);
        }

        #region Quick Settings
        Dictionary<Control, bool> pendingEnables = new Dictionary<Control, bool>();
        HashSet<ListBox> pendingIndices = new HashSet<ListBox>();
        private void DisableSaveButton() { EnableSaveButton(false); }
        private void EnableSaveButton(bool enabled = true)
        {
            pendingEnables[SaveButton] = enabled;
        }
        private void DisableConfirmationButtons() { EnableConfirmationButtons(false); }
        private void EnableConfirmationButtons(bool enabled = true)
        {
            pendingEnables[CancelButton] = enabled;
            EnableSaveButton(enabled);
        }
        private void DisableAddButton() { EnableAddButton(false); }
        private void EnableAddButton(bool enabled = true)
        {
            pendingEnables[AddButton] = enabled;
        }
        private void DisableDeleteButton() { EnableDeleteButton(false); }
        private void EnableDeleteButton(bool enabled = true)
        {
            pendingEnables[DeleteButton] = enabled;
        }
        private void DisableInputBoxes() { EnableInputBoxes(false); }
        private void EnableInputBoxes(bool enabled = true)
        {
            pendingEnables[DocTypeBox] =
            pendingEnables[DocIdsBox] = enabled;
        }
        private void DisableEditor() { EnableEditor(false); }
        private void EnableEditor(bool enabled = true)
        {
            EnableInputBoxes(enabled);
            EnableConfirmationButtons(enabled);
        }
        private void DisableDocTypeList() { EnableDocTypeList(false); }
        private void EnableDocTypeList(bool enabled = true)
        {
            pendingEnables[DocTypesBox] = enabled;
            if (!enabled)
                pendingIndices.Add(DocTypesBox);
            else
                pendingIndices.Remove(DocTypesBox);
        }
        private void DisableEverything()
        {
            DisableEditor();
            DisableDocTypeList();
            DisableAddButton();
            DisableDeleteButton();
            DisableConfirmationButtons();
        }
        private void FinalizeOperations()
        {
            foreach (Control key in pendingEnables.Keys)
                if (key.Enabled != pendingEnables[key])
                    key.Enabled = pendingEnables[key];
            pendingEnables.Clear();
            foreach (ListBox key in pendingIndices)
                key.SelectedIndex = -1;
            pendingIndices.Clear();
        }
        #endregion

        private bool SufficientAddData
        {
            get
            {
                bool notEmpty = !DocTypeBox.Text.IsNullOrEmpty() && !DocIdsBox.Text.IsNullOrEmpty();
                bool docType4Chars = DocTypeBox.Text.Length == 4;
                bool docIds5CharsOrEmpty = !DocIdsBox.Lines.Any(o => o.Length != 5 && !o.IsNullOrEmpty());
                return notEmpty && docType4Chars && docIds5CharsOrEmpty;
            }
        }

        private bool NoChanges
        {
            get
            {
                return DocIdsBox.Text == docIdsCache && DocTypeBox.Text == docTypeCache;
            }
        }

        private void SyncUI()
        {
            DisableEverything();
            switch (DocTypeMode)
            {
                case UIMode.NothingSelected:
                    EnableAddButton();
                    EnableDocTypeList();
                    break;
                case UIMode.AddingNewDocType:
                    EnableEditor();
                    if (!SufficientAddData)
                        DisableSaveButton();
                    break;
                case UIMode.DocTypeSelected:
                    EnableEditor();
                    EnableAddButton();
                    EnableDeleteButton();
                    EnableDocTypeList();
                    if (!SufficientAddData || NoChanges)
                        DisableSaveButton();
                    break;
            }
            FinalizeOperations();
        }
        private UIMode docTypeMode;
        private UIMode DocTypeMode
        {
            get { return docTypeMode; }
            set
            {
                docTypeMode = value;
                SyncUI();
            }
        }
        private DocType SelectedDocType
        {
            get
            {
                if (DocTypesBox.SelectedIndex == -1) return null;
                return cachedDocTypes[DocTypesBox.SelectedIndex];
            }
            set
            {
                if (value != null)
                {
                    DocTypesBox.SelectedIndex = cachedDocTypes.Select((d, i) => d.DocTypeId == value.DocTypeId ? i : (int?)null).Single(i => i.HasValue).Value;
                    DocTypeMode = UIMode.DocTypeSelected;
                }
                else
                {
                    DocTypesBox.SelectedIndex = -1;
                    docTypeMode = UIMode.NothingSelected;
                }
            }
        }
        private enum UIMode
        {
            NothingSelected,
            DocTypeSelected,
            AddingNewDocType
        }

        bool codeTriggered = false;
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (!codeTriggered)
                SyncUI();
        }

        string docTypeCache = null;
        string docIdsCache = null;
        private void DocTypesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            codeTriggered = true;
            if (DocTypesBox.SelectedIndex != -1)
            {
                DocTypeBox.Text = docTypeCache = SelectedDocType.DocTypeValue;
                DocIdsBox.Text = docIdsCache = string.Join(Environment.NewLine, DocId.GetByDocType(SelectedDocType).Select(o => o.DocIdValue).ToArray());
                DocTypeMode = UIMode.DocTypeSelected;
            }
            else
            {
                DocTypeBox.Text = DocIdsBox.Text = null;
            }
            codeTriggered = false;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (SelectedDocType != null)
            {
                if (Dialog.Warning.YesNo("Are you sure you want to delete Doc Type " + docTypeCache + " and all associated Doc IDs?"))
                {
                    DocType.Delete(SelectedDocType);
                    SyncData();
                    DocTypeMode = UIMode.NothingSelected;
                }
            }
        }
    }
}

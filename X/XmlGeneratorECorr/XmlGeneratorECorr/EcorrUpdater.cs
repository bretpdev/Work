using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WinForms;

namespace XmlGeneratorECorr
{
    public partial class EcorrUpdater : Form
    {
        private List<DocumentDetails> DbDocDetails { get; set; }
        private List<Letters> DbLetters { get; set; }
        private List<LetterTypes> DbLetterTypes { get; set; }
        private List<TagAttributeValueMapping> DbTagAttributeValueMapping { get; set; }
        private List<TagAttributeValues> DbTagAttributeValues { get; set; }
        private List<string> Tags { get; set; }

        public EcorrUpdater()
        {
            InitializeComponent();
            LoadLists();
            SetDataSources();
        }

        private void SetDataSources()
        {
            //UNDONE: You could set the datasource with the database call 
            //UNDONE: ex: DocDetailsDataGrid.DataSource = DataAccessHelper.ExecuteList<DocumentDetails>("GetAllDocumentDetailData", DataAccessHelper.Database.ECorrFed);
            DocDetailsDataGrid.DataSource = DbDocDetails;
            LettersDataGrid.DataSource = DbLetters;
            LetterTypesDataGrid.DataSource = DbLetterTypes;
            AttributesDataGrid.DataSource = DbTagAttributeValues;
            HideColumns();
        }

        private void HideColumns()
        {
            DocDetailsDataGrid.Columns[0].Visible = false;
            LettersDataGrid.Columns[0].Visible = false;
            LetterTypesDataGrid.Columns[0].Visible = false;
            AttributesDataGrid.Columns[0].Visible = false;
            LettersDataGrid.Columns["ViewableBool"].Visible = false;
            LettersDataGrid.Columns["WorkFlowBool"].Visible = false;
            LettersDataGrid.Columns["DocDeleteBool"].Visible = false;
            LettersDataGrid.Columns["ViewedBool"].Visible = false;
        }
        [UsesSproc(DataAccessHelper.Database.ECorrFed, "GetAllDocumentDetailData")]
        [UsesSproc(DataAccessHelper.Database.ECorrFed, "GetAllLetterData")]
        [UsesSproc(DataAccessHelper.Database.ECorrFed, "GetAllLetterTypeData")]
        [UsesSproc(DataAccessHelper.Database.ECorrFed, "GetAllMappingValues")]
        [UsesSproc(DataAccessHelper.Database.ECorrFed, "GetAttributeValues")]
        private void LoadLists()
        {
            DbDocDetails = DataAccessHelper.ExecuteList<DocumentDetails>("GetAllDocumentDetailData", DataAccessHelper.Database.ECorrFed);
            DbLetters = DataAccessHelper.ExecuteList<Letters>("GetAllLetterData", DataAccessHelper.Database.ECorrFed);
            DbLetterTypes = DataAccessHelper.ExecuteList<LetterTypes>("GetAllLetterTypeData", DataAccessHelper.Database.ECorrFed);
            DbTagAttributeValueMapping = DataAccessHelper.ExecuteList<TagAttributeValueMapping>("GetAllMappingValues", DataAccessHelper.Database.ECorrFed);
            DbTagAttributeValues = DataAccessHelper.ExecuteList<TagAttributeValues>("GetAttributeValues", DataAccessHelper.Database.ECorrFed);
            Tags = DbTagAttributeValueMapping.Select(p => p.Tag).Distinct().ToList();
        }

        private void LettersDataGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Letters lettersData = DbLetters[LettersDataGrid.CurrentRow.Index];
            FormBuilder form = GenerateLetterForm(lettersData);
            if (FormBuilder.Generate(lettersData, form).ShowPopoverDialog(this) == true)
            {
                Letters.UpdateDb(lettersData);
                LoadLists();
                SetDataSources();
            }
        }

        private FormBuilder GenerateLetterForm(Letters LettersData)
        {
            FormBuilder form = new FormBuilder("Letters");
            form.InputWidth = 300;
            var box = form.AddField<ComboBox>("Letter Type", p =>
            {
                p.DataSource = DbLetterTypes;
                p.DisplayMember = "LetterType";
                p.SetInitialIndex(LettersData.LetterTypeId - 1);
            });

            form.IncludeAlternateButton = true;

            form.FormAccepted += fb =>
            {
                var letterBox = fb.GetInput(box);
                LettersData.LetterTypeId = DbLetterTypes[letterBox.SelectedIndex].LetterTypeId;
                LoadLists();
                SetDataSources();
                return true;
            };

            form.FormAlternate += path =>
            {
                Letters.InactiveRecord(LettersData.LetterId);
                LoadLists();
                SetDataSources();
                return true;
            };

            return form;
        }

        private FormBuilder GenerateDocumentDetails(DocumentDetails selectedTab)
        {
            FormBuilder form = new FormBuilder(Tabs.SelectedTab.Name);
            form.AlternateButtonText = "Path";
            form.IncludeAlternateButton = true;
            form.InputWidth = 400;
            form.FormAlternate += path =>
            {
                using (OpenFileDialog open = new OpenFileDialog())
                {
                    if (open.ShowDialog() == DialogResult.OK)
                    {
                        (selectedTab).Path = Path.Combine(EnterpriseFileSystem.GetPath("ECORRDocuments"), open.SafeFileName);
                        var field = form.Fields.Where(p => p.Label == "Path").Single();
                        var text = form.GetControl(field);
                        text.Text = ((DocumentDetails)selectedTab).Path;
                        text.Enabled = false;
                    }
                    return false;
                }
            };

            var box = form.AddField<ComboBox>("Letter", p =>
            {
                p.DataSource = DbLetters;
                p.DisplayMember = "Letter";
                p.SetInitialIndex((selectedTab).LetterId - 1);
            }, 1);

            form.FormAccepted += fb =>
            {
                var letterBox = fb.GetInput(box);
                var field = form.Fields.Where(p => p.Label == "Path").Single();
                fb.GetInput(field);
                (selectedTab.Path) = fb.GetInput(field).Text;
                (selectedTab).LetterId = DbLetters[letterBox.SelectedIndex].LetterId;
                return true;
            };

            form.AddField<RequiredTextBox>("Path", p =>
            {

            }, 16);

            return form;
        }

        private void Add()
        {
            var selectedTab = new object();
            FormBuilder form = new FormBuilder(Tabs.SelectedTab.Name);
            form.InputWidth = 300;
            switch (Tabs.SelectedTab.Text)
            {
                case "Document Details":
                    selectedTab = new DocumentDetails(DateTime.Now);
                    form = GenerateDocumentDetails((DocumentDetails)selectedTab);
                    break;
                case "Letters":
                    selectedTab = new Letters();

                    var ltrType = form.AddField<ComboBox>("Letter Type", p =>
                    {
                        p.DataSource = DbLetterTypes;
                        p.DisplayMember = "LetterType";
                        p.SetInitialIndex(((Letters)selectedTab).LetterTypeId - 1);
                    }, 2);

                    form.FormAccepted += fb =>
                    {
                        var letterBox = fb.GetInput(ltrType);
                        ((Letters)selectedTab).LetterTypeId = DbLetterTypes[letterBox.SelectedIndex].LetterTypeId;
                        return true;
                    };

                    break;
                case "Letter Types":
                    selectedTab = new LetterTypes();
                    break;
                case "Attributes":
                    selectedTab = new TagAttributeValues();
                    break;
            }


            if (FormBuilder.Generate(selectedTab, form).ShowPopoverDialog(this) == true)
            {
                if (selectedTab.GetType() == typeof(DocumentDetails))
                    DocumentDetails.AddRecordDb((DocumentDetails)selectedTab);
                else if (selectedTab.GetType() == typeof(Letters))
                    Letters.AddRecordDb((Letters)selectedTab);
                else if (selectedTab.GetType() == typeof(LetterTypes))
                    LetterTypes.AddRecordDb((LetterTypes)selectedTab);
                else if (selectedTab.GetType() == typeof(TagAttributeValues))
                    TagAttributeValues.AddRecordDb((TagAttributeValues)selectedTab);

                LoadLists();
                SetDataSources();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var selectedTab = new object();
            FormBuilder form = new FormBuilder(Tabs.SelectedTab.Name);
            form.InputWidth = 300;
            switch (Tabs.SelectedTab.Text)
            {
                case "Document Details":
                    selectedTab = new DocumentDetails(DateTime.Now);
                    form = GenerateDocumentDetails((DocumentDetails)selectedTab);
                    break;
                case "Letters":
                    selectedTab = new Letters();

                    var ltrType = form.AddField<ComboBox>("Letter Type", p =>
                    {
                        p.DataSource = DbLetterTypes;
                        p.DisplayMember = "LetterType";
                        p.SetInitialIndex(((Letters)selectedTab).LetterTypeId - 1);
                    }, 2);

                    form.FormAccepted += fb =>
                    {
                        var letterBox = fb.GetInput(ltrType);
                        ((Letters)selectedTab).LetterTypeId = DbLetterTypes[letterBox.SelectedIndex].LetterTypeId;
                        return true;
                    };

                    break;
                case "Letter Types":
                    selectedTab = new LetterTypes();
                    break;
                case "Attributes":
                    selectedTab = new TagAttributeValues();
                    break;
            }


            if (FormBuilder.Generate(selectedTab, form).ShowPopoverDialog(this) == true)
            {
                if (selectedTab.GetType() == typeof(DocumentDetails))
                    DocumentDetails.AddRecordDb((DocumentDetails)selectedTab);
                else if (selectedTab.GetType() == typeof(Letters))
                    Letters.AddRecordDb((Letters)selectedTab);
                else if (selectedTab.GetType() == typeof(LetterTypes))
                    LetterTypes.AddRecordDb((LetterTypes)selectedTab);
                else if (selectedTab.GetType() == typeof(TagAttributeValues))
                    TagAttributeValues.AddRecordDb((TagAttributeValues)selectedTab);

                LoadLists();
                SetDataSources();
            }
        }

        private void DocDetailsDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DocumentDetails docDetailsData = DbDocDetails[DocDetailsDataGrid.CurrentRow.Index];
            FormBuilder form = new FormBuilder("Document Details");
            form.InputWidth = 550;

            var box = form.AddField<ComboBox>("Letter", p =>
            {
                p.DataSource = DbLetters;
                p.DisplayMember = "Letter";
                p.SetInitialIndex(DbLetters.IndexOf(DbLetters.Where(q => q.LetterId == docDetailsData.LetterId).Single()));
            }, 1);

            form.IncludeAlternateButton = true;
            form.AlternateButtonText = "Delete";

            form.FormAccepted += fb =>
            {
                var letterBox = fb.GetInput(box);
                docDetailsData.LetterId = DbLetters[letterBox.SelectedIndex].LetterId;
                return true;
            };

            var pathButton = form.AddField<RequiredTextBox>("Path", p =>
            {
                p.Text = docDetailsData.Path;
            }, 1);

            form.FormAlternate += path =>
            {
                DocumentDetails.InactiveRecord(docDetailsData.DocumentDetailsId);
                LoadLists();
                SetDataSources();
                return true;
            };

            if (FormBuilder.Generate(docDetailsData, form).ShowPopoverDialog(this) == true)
            {
                DocumentDetails.UpdateDb(docDetailsData);
                LoadLists();
                SetDataSources();
            }
        }

        private void LetterTypesDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            LetterTypes letterTypesData = DbLetterTypes[LetterTypesDataGrid.CurrentRow.Index];
            FormBuilder form = new FormBuilder("Letter Type");

            if (FormBuilder.Generate(letterTypesData, form).ShowPopoverDialog(this) == true)
            {
                LetterTypes.UpdateDb(letterTypesData);
                LoadLists();
                SetDataSources();
            }
        }

        private void AttributesDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            TagAttributeValues attributeData = DbTagAttributeValues[AttributesDataGrid.CurrentRow.Index];
            FormBuilder form = new FormBuilder("Attributes");
            form.InputWidth = 300;
            if (FormBuilder.Generate(attributeData, form).ShowPopoverDialog(this) == true)
            {
                TagAttributeValues.UpdateDb(attributeData);
                LoadLists();
                SetDataSources();
            }
        }

        private void AddB_Click(object sender, EventArgs e)
        {
            Add();
        }
    }
}

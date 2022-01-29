using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using static Uheaa.Common.Dialog;

namespace TRDPRTYRES
{
    public partial class ReferenceInfoFrm : Form
    {
        private BorReferenceInfo Bdata { get; set; }
        private List<string> States { get; set; }
        private List<Relationships> Relationship { get; set; }
        private List<Sources> Source { get; set; }

        public ReferenceInfoFrm(BorReferenceInfo bData, bool isOneLink, DataAccess DA)
        {
            InitializeComponent();
            Relationship = null;
            Bdata = bData;

            LoadDropDowns(isOneLink, DA);
            borInfoBindingSource.DataSource = Bdata;
            Text = string.Empty;
            Street1.MaxLength = Street2.MaxLength = isOneLink ? 35 : 30;
            CheckForExistingRef(Bdata);
        }

        private void LoadDropDowns(bool oneLink, DataAccess DA)
        {
            States = DA.GetStateCodes();
            States.Insert(0, "");
            State.DataSource = States;

            Source = DA.GetSources(oneLink);
            Source.Insert(0, new Sources() { Source = "", SourceCode = "" });
            CboSource.DataSource = Source;
            CboSource.DisplayMember = "Source";
            CboSource.ValueMember = "SourceCode";
            if (Bdata.SourceCode.IsPopulated())
                CboSource.SelectedItem = Source.Where(p => p.SourceCode == Bdata.SourceCode).SingleOrDefault();

            Relationship = DA.GetRelationships(oneLink);
            Relationship.Insert(0, new Relationships() { Relationship = "", RelationshipCode = "" });
            CboRelationship.DataSource = Relationship;
            CboRelationship.DisplayMember = "Relationship";
            CboRelationship.ValueMember = "RelationshipCode";
            if (Bdata.RelationshipCode.IsPopulated())
                CboRelationship.SelectedItem = Relationship.Where(p => p.RelationshipCode == Bdata.RelationshipCode).SingleOrDefault();
        }

        private void CheckForExistingRef(BorReferenceInfo bdata)
        {
            if (bdata.ReferenceId.IsNullOrEmpty())
            {
                RefFirstName.ReadOnly = false;
                RefMidInt.ReadOnly = false;
                RefLastName.ReadOnly = false;
            }
        }

        private void OK_Click(object sender, System.EventArgs e)
        {
            bool hasError = false;
            string message = "The following fields are required but were not filled in. Please fill in all the required fields.\r\n\r\n";
            if (CboSource.Text.IsNullOrEmpty() || !Source.Any(p => p.Source == CboSource.Text))
            {
                hasError = true;
                message += "Source\r\n";
            }
            if (CboRelationship.Text.IsNullOrEmpty() || !Relationship.Any(p => p.Relationship == CboRelationship.Text))
            {
                hasError = true;
                message += "Relationship\r\n";
            }
            if (Street1.Text.IsNullOrEmpty())
            {
                hasError = true;
                message += "Street Address 1\r\n";
            }
            if (City.Text.IsNullOrEmpty())
            {
                hasError = true;
                message += "City\r\n";
            }
            if (State.Text.IsNullOrEmpty())
            {
                hasError = true;
                message += "States\r\n";
            }
            if (Zip.Text.IsNullOrEmpty())
            {
                hasError = true;
                message += "Zip";
            }

            if (hasError)
            {
                Error.Ok(message);
                DialogResult = DialogResult.None;
            }
            else
            {
                Bdata.SourceCode = Source.Where(p => p.Source == CboSource.Text).Select(q => q.SourceCode).Single();
                Bdata.RelationshipCode = Relationship.Where(p => p.Relationship == CboRelationship.Text).Select(q => q.RelationshipCode).Single();

                DialogResult = DialogResult.OK;
            }
        }
    }
}
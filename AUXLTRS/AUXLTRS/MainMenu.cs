using System.Diagnostics;
using System.Windows.Forms;
using Q;

namespace AUXLTRS
{
    public partial class MainMenu : FormBase
    {
        public enum Selection
        {
            CustomLetter,
            ThirdPartyAuthorizationForm,
            NameConflictLetter,
            DobConflictLetter,
            SsnConflictLetter,
            PropertyOwnerInformationRequest,
            PostOfficeLetter,
            RequestForHearing,
            NoticeOfSatisfaction,
            NsldsSsn1,
            NsldsSsn2,
            NsldsSsn3
        }

        private SelectionContainer _selection;

        public MainMenu(SelectionContainer selection)
        {
            InitializeComponent();

            _selection = selection;
        }

        public new DialogResult ShowDialog()
        {
            //Reset all the form fields before showing the form again.
            chkCustomLetter.Checked = false;
            chkDobConflict.Checked = false;
            chkNameConflict.Checked = false;
            chkNoticeOfSatisfaction.Checked = false;
            chkPostOffice.Checked = false;
            chkPropertyOwner.Checked = false;
            chkRequestForHearing.Checked = false;
            chkSsnConflict.Checked = false;
            chkThirdPartyAuth.Checked = false;
            chkNsldsSsn1.Checked = false;
            chkNsldsSsn2.Checked = false;
            chkNsldsSsn3.Checked = false;

            DialogResult result = base.ShowDialog();
            return result;
        }

        private void SetSelectionAndReturn(Selection selection)
        {
            _selection.Value = selection;
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        #region Event Handlers
        private void chkCustomLetter_Click(object sender, System.EventArgs e)
        {
            SetSelectionAndReturn(Selection.CustomLetter);
        }

        private void chkDobConflict_Click(object sender, System.EventArgs e)
        {
            SetSelectionAndReturn(Selection.DobConflictLetter);
        }

        private void chkNameConflict_Click(object sender, System.EventArgs e)
        {
            SetSelectionAndReturn(Selection.NameConflictLetter);
        }

        private void chkNoticeOfSatisfaction_Click(object sender, System.EventArgs e)
        {
            SetSelectionAndReturn(Selection.NoticeOfSatisfaction);
        }

        private void chkPostOffice_Click(object sender, System.EventArgs e)
        {
            SetSelectionAndReturn(Selection.PostOfficeLetter);
        }

        private void chkPropertyOwner_Click(object sender, System.EventArgs e)
        {
            SetSelectionAndReturn(Selection.PropertyOwnerInformationRequest);
        }

        private void chkRequestForHearing_Click(object sender, System.EventArgs e)
        {
            SetSelectionAndReturn(Selection.RequestForHearing);
        }

        private void chkSsnConflict_Click(object sender, System.EventArgs e)
        {
            SetSelectionAndReturn(Selection.SsnConflictLetter);
        }

        private void chkThirdPartyAuth_Click(object sender, System.EventArgs e)
        {
            SetSelectionAndReturn(Selection.ThirdPartyAuthorizationForm);
        }

        private void chkNsldsSsn1_Click(object sender, System.EventArgs e)
        {
            SetSelectionAndReturn(Selection.NsldsSsn1);
        }

        private void chkNsldsSsn2_Click(object sender, System.EventArgs e)
        {
            SetSelectionAndReturn(Selection.NsldsSsn2);
        }

        private void chkNsldsSsn3_Click(object sender, System.EventArgs e)
        {
            SetSelectionAndReturn(Selection.NsldsSsn3);
        }
        #endregion Event Handlers

        //The selection that gets passed to the constructor needs to be a reference type,
        //which enums are not, so this class wraps a Selection enum in a reference type.
        public class SelectionContainer
        {
            public Selection Value { get; set; }
        }
    }//class
}//namespace

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
using Uheaa.Common.WinForms;

namespace TPERM
{
    public partial class BorrowerEntry : Form
    {
        public ApplicationData AppData { get; set; }
        private static Dictionary<string, string> RelationshipCodes = new Dictionary<string, string>()
        {
            {"UNKNOWN", "01"},
            {"PARENT", "02"},
            {"RELATIVE", "03"},
            {"NON-RELATIVE", "04"},
            {"EMPLOYER", "05"},
            {"SPOUSE", "06"},
            {"SIBLING", "07"},
            {"ROOMMATE", "08"},
            {"NEIGHBOR", "09"},
            {"CHILD", "10"},
            {"FRIEND", "11"},
            {"GUARDIAN REF", "12"},
            {"PHYSICIAN", "13"},
            {"SURVIVOR", "14"},
            {"ATTORNEY REF", "15"},
            {"POA", "16"},
            {"SPLIT AUTHZN", "17"},
            {"LIMITED POA", "18"},
            {"ENDRSR POA", "21"},
            {"GUARDIAN", "22"},
            {"EXECUTOR", "23"},
            {"IFM-PARENT", "24"},
            {"IFM-SPOUSE", "25"},
            {"IFM-CHILD", "26"},
            {"JOINT POA", "27"},
            {"STUDENT POA", "28"},
        };


        public BorrowerEntry(ApplicationData bData)
        {
            InitializeComponent();
            AppData = bData;
            LoadBorrowerInfo();
        }

        private void LoadBorrowerInfo()
        {
            AccountNumber.Text = AppData.BorrowerInfo.AccountNumber;
            SSN.Text = AppData.BorrowerInfo.Ssn;
            FName.Text = AppData.BorrowerInfo.FirstName;
            LName.Text = AppData.BorrowerInfo.LastName;
            Relationship.DataSource = RelationshipCodes.Select(p => p.Key).OrderBy(p => p).ToList();
            Relationship.SelectedIndex = -1;
            Corr.Text = AppData.TaskInfo.CorrDocNum;
            CoMakerInfoBtn.Enabled = (AppData.CoMaker.Any());
        }

        private void Continue_Click(object sender, EventArgs e)
        {
            if (!CheckUserInput())
                return;

            SetResult();
            if (!SaveData())
                return;

            DialogResult = DialogResult.OK;
        }

        private bool SaveData()
        {
            string bwrSignAs = string.Empty;
            if (AppData.Result == ApplicationData.ApplicationResults.WrongSign)
            {
                bwrSignAs = GetBwrSigned();
                if (bwrSignAs.IsNullOrEmpty())
                    return false;
            }

            AppData.AddMultiple = Multi.Checked;

            AppData.ReferenceInfo = new ReferenceData()
            {
                FirstName = RefFName.Text,
                LastName = RefLName.Text,
                MiddleName = RefMName.Text,
                Relationship = Relationship.Text,
                RelationshipCode = Relationship.Text.IsNullOrEmpty() ? "" : RelationshipCodes[Relationship.Text],
                BorrowerSignatureName = bwrSignAs
            };

            return true;
        }

        private static string GetBwrSigned()
        {
            string bwrSignAs;
            using (InputBox<TextBox> bwrSig = new InputBox<TextBox>("Please enter the signature on the application.", "Enter Signature"))
            {
                bwrSig.InputControl.CharacterCasing = CharacterCasing.Upper;
                bwrSig.ShowDialog();

                bwrSignAs = bwrSig.InputControl.Text;
            }

            return bwrSignAs;
        }

        private bool CheckUserInput()
        {
            bool complete = true;
            if (!SignedBwr.Checked && !SignedCo.Checked && !SignedPOA.Checked && !SignedNone.Checked)
            {
                Signedlbl.BackColor = Color.LightPink;
                complete = false;
            }
            else
                Signedlbl.BackColor = SystemColors.Control;
            if (!Illegible.Checked)
            {
                if (RefFName.Text.IsNullOrEmpty())
                {
                    RefFName.BackColor = Color.LightPink;
                    complete = false;
                }
                else
                    RefFName.BackColor = SystemColors.Control;

                if (RefLName.Text.IsNullOrEmpty())
                {
                    RefLName.BackColor = Color.LightPink;
                    complete = false;
                }
                else
                    RefLName.BackColor = SystemColors.Control;

                if (Relationship.SelectedIndex == -1)
                {
                    Relationship.BackColor = Color.LightPink;
                    complete = false;
                }
                else
                    Relationship.BackColor = SystemColors.Control;
            }

            return complete;
        }

        private void SetResult()
        {
            if (SignedNone.Checked)
                AppData.Result = ApplicationData.ApplicationResults.NoSign;
            else if (Illegible.Checked)
                AppData.Result = ApplicationData.ApplicationResults.Illegible;
            else if (NameNoMatch.Checked)
                AppData.Result = ApplicationData.ApplicationResults.NoMatch;
            else if (DiffForm.Checked)
                AppData.Result = ApplicationData.ApplicationResults.WrongForm;
            else
                AppData.Result = ApplicationData.ApplicationResults.InProcess;

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            if (Dialog.Error.YesNo("Are you sure you want to cancel processing this task?"))
                DialogResult = DialogResult.Cancel;
        }

        private void CoMakerInfo_Click(object sender, EventArgs e)
        {
            using (CoMakerInfo cmi = new CoMakerInfo(AppData.CoMaker))
            {
                cmi.ShowDialog();
                if (cmi.SelectedCoMaker != null)
                {
                    AppData.SelectedCoMaker = cmi.SelectedCoMaker;
                    SignedCo.Checked = SignedBwr.Enabled = SignedNone.Enabled = SignedPOA.Enabled = false;
                    SignedCo.Enabled = SignedCo.Checked = true;
                }
                else
                {
                    AppData.SelectedCoMaker = cmi.SelectedCoMaker;
                    SignedBwr.Enabled = SignedNone.Enabled = SignedPOA.Enabled = true;
                    SignedCo.Checked = SignedCo.Enabled = false;
                }
            }
        }

        private void SignedNone_CheckedChanged(object sender, EventArgs e)
        {
            if (SignedNone.Checked)
                DiffForm.Enabled = DiffForm.Checked = Illegible.Enabled = NameNoMatch.Enabled = Illegible.Checked = NameNoMatch.Checked = false;
            else
                DiffForm.Enabled =  Illegible.Enabled = NameNoMatch.Enabled = true;
        }

        private void Illegible_CheckedChanged(object sender, EventArgs e)
        {
            if (Illegible.Checked)
                DiffForm.Enabled = DiffForm.Checked =  NameNoMatch.Checked = NameNoMatch.Enabled = false;
            else
                DiffForm.Enabled =  NameNoMatch.Enabled = true;
        }

        private void NameNoMatch_CheckedChanged(object sender, EventArgs e)
        {
            if (NameNoMatch.Checked)
                DiffForm.Enabled = DiffForm.Checked =  Illegible.Checked = Illegible.Enabled = false;
            else
                DiffForm.Enabled = Illegible.Enabled = true;
        }

        private void Corr_MouseClick(object sender, MouseEventArgs e)
        {
            Clipboard.SetText(Corr.Text);
        }

        private void DiffForm_CheckedChanged(object sender, EventArgs e)
        {
            if (DiffForm.Checked)
                NameNoMatch.Enabled= NameNoMatch.Checked=  Illegible.Checked = Illegible.Enabled = false;
            else
                NameNoMatch.Enabled = DiffForm.Enabled= Illegible.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Corr.Text);
        }
    }
}

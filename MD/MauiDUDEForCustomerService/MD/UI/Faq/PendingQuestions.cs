using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;

namespace MD
{
    public partial class PendingQuestions : UserControl
    {
        public PendingQuestions()
        {
            InitializeComponent();
        }

        public enum PendingQuestionsMode
        {
            Representative,
            Admin
        }
        private PendingQuestionsMode mode;
        public void Init(PendingQuestionsMode mode)
        {
            this.mode = mode;
            if (mode == PendingQuestionsMode.Representative)
            {
                GridLabel.Text = "My Questions";
            }
            else if (mode == PendingQuestionsMode.Admin)
            {
                GridLabel.Text = "Pending Questions";
                var col = MyQuestionsGrid.Columns[MyQuestionsGrid.Columns.Add("SubmittedByColumn", "Submitter")];
                col.DataPropertyName = "SubmittedByString";
                SubmitButton.Visible = false;
                ApproveButton.Visible = true;
                RejectButton.Visible = true;
            }
            DisabledBoxes();
            SyncData();
            LoadSelected();
        }

        bool submitMode = false;
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (!submitMode)
            {
                submitMode = true;
                NormalBoxes();
                MyQuestionsGrid.Enabled = false;
                MyQuestionsGrid.ClearSelection();
                CancelButton.Visible = true;
                WithdrawButton.Visible = false;
                CancelButton.Text = "Cancel";
                SubmitButton.Text = "Submit Question";
                QuestionBox.Text = NotesBox.Text = "";
                QuestionBox.Focus();
            }
            else if (Dialog.Info.YesNo("Are you sure you want to submit this question?"))
            {
                FaqPendingQuestion question = new FaqPendingQuestion() { Question = QuestionBox.Text, Notes = NotesBox.Text };
                FaqPendingQuestion.Submit(question);
                submitMode = false;
                CancelButton.Visible = false;
                DisabledBoxes();
                SubmitButton.Text = "Ask New Question";
                MyQuestionsGrid.Enabled = true;
                SyncData();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (submitMode)
            {
                submitMode = false;
                DisabledBoxes();
                MyQuestionsGrid.Enabled = true;
                SubmitButton.Text = "Ask New Question";
                CancelButton.Visible = false;
                LoadSelected();
            }
        }

        private List<FaqPendingQuestion> questions;
        public delegate void OnDataSynced(PendingQuestions control, List<FaqPendingQuestion> questions);
        public event OnDataSynced DataSynced;
        public void SyncData()
        {
            MyQuestionsGrid.AutoGenerateColumns = false;
            MyQuestionsGrid.DataSource = null;
            if (mode == PendingQuestionsMode.Representative)
                MyQuestionsGrid.DataSource = questions = FaqPendingQuestion.GetByCurrentUser();
            else
                MyQuestionsGrid.DataSource = questions = FaqPendingQuestion.GetAllPending();
            if (DataSynced != null)
                DataSynced(this, questions);
        }

        private void MyQuestionsGrid_SelectionChanged(object sender, EventArgs e)
        {
            LoadSelected();
        }

        private void LoadSelected()
        {
            if (MyQuestionsGrid.Enabled && MyQuestionsGrid.SelectedRows.Count == 1)
            {
                var question = questions[MyQuestionsGrid.SelectedRows[0].Index];
                if (mode == PendingQuestionsMode.Admin)
                    QuestionLabel.Text = "Question submitted by " + UsernameHelper.GetDisplayName(question.SubmittedBy);
                QuestionBox.Text = question.Question;
                NotesBox.Text = question.Notes;
                ReadOnlyBoxes();

                if (question.ProcessedOn == null)
                {
                    if (mode == PendingQuestionsMode.Representative)
                    {
                        CancelButton.Visible = false;
                        WithdrawButton.Visible = true;
                        WithdrawButton.Tag = question;
                    }
                    else if (mode == PendingQuestionsMode.Admin)
                    {
                        ApproveButton.Enabled = true;
                        RejectButton.Enabled = true;
                        ApproveButton.Tag = RejectButton.Tag = question;
                    }
                }
                else
                    WithdrawButton.Visible = false;
            }
            else
            {
                QuestionBox.Text = NotesBox.Text = "";
                ApproveButton.Enabled = false;
                RejectButton.Enabled = false;
            }
        }

        private void DisabledBoxes()
        {
            QuestionBox.Enabled = NotesBox.Enabled = false;
            QuestionBox.BackColor = NotesBox.BackColor = SystemColors.Control;
        }
        private void ReadOnlyBoxes()
        {
            QuestionBox.ReadOnly = NotesBox.ReadOnly = true;
            QuestionBox.BackColor = NotesBox.BackColor = Color.White;
            QuestionBox.ForeColor = NotesBox.ForeColor = Color.Black;
        }
        private void NormalBoxes()
        {
            QuestionBox.ReadOnly = NotesBox.ReadOnly = false;
            QuestionBox.Enabled = NotesBox.Enabled = true;
            QuestionBox.BackColor = NotesBox.BackColor = Color.White;
        }

        private void RejectButton_Click(object sender, EventArgs e)
        {
            if (Dialog.Warning.YesNo("Are you sure you want to reject this question?"))
            {
                var question = (FaqPendingQuestion)RejectButton.Tag;
                FaqPendingQuestion.Reject(question);
                RejectButton.Tag = null;
                SyncData();
            }
        }

        private void WithdrawButton_Click(object sender, EventArgs e)
        {
            if (Dialog.Warning.YesNo("Are you sure you want to withdraw this question?"))
            {
                var question = (FaqPendingQuestion)WithdrawButton.Tag;
                FaqPendingQuestion.Withdraw(question);
                WithdrawButton.Tag = null;
                SyncData();
            }
        }

        public delegate void OnQuestionSelectedForApproval(PendingQuestions control, FaqPendingQuestion question);
        public event OnQuestionSelectedForApproval QuestionSelectedForApproval;
        private void ApproveButton_Click(object sender, EventArgs e)
        {
            QuestionSelectedForApproval?.Invoke(this, (FaqPendingQuestion)ApproveButton.Tag);
            ApproveButton.Tag = null;
            LoadSelected();
        }
    }
}

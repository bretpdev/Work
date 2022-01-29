using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.WinForms;

namespace MD
{
    public partial class ExistingQuestionsTab : UserControl
    {
        public ExistingQuestionsTab()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void Setup()
        {
            AnswerBox.DocumentRtfTextChanged += (o, ea) => { if (!codeInitiated) DetectChanges(); };
            SyncData(false);
        }
        public delegate void OnDataSynced();
        public event OnDataSynced DataSynced;
        public void SyncData()
        {
            SyncData(false);
        }
        private List<FaqQuestionGroup> groups = new List<FaqQuestionGroup>();
        private List<FaqPortfolio> portfolios = new List<FaqPortfolio>();
        private void SyncData(bool triggerEvent)
        {
            groups = FaqQuestionGroup.GetAll();
            AssignedGroupsList.DisplayMember = "GroupName";
            AssignedGroupsList.BeginUpdate();
            AssignedGroupsList.DataSource = groups;
            AssignedGroupsList.EndUpdate();

            portfolios = FaqPortfolio.GetAll();
            AssignedPortfoliosList.Items.Clear();
            AssignedPortfoliosList.DisplayMember = "PortfolioName";
            foreach (FaqPortfolio portfolio in portfolios)
                AssignedPortfoliosList.Items.Add(portfolio, true);

            QuestionFilterBox.SyncData();

            if (triggerEvent)
                if (DataSynced != null)
                    DataSynced();
        }

        private void QuestionFilterBox_QuestionSelected(QuestionFilter sender, FaqQuestion selected)
        {
            LoadQuestion(selected, SelectedQuestionModeEnum.Normal);
        }

        private enum SelectedQuestionModeEnum
        {
            Normal,
            NewQuestion,
            ExternallyLoadedQuestion
        }
        private SelectedQuestionModeEnum SelectedQuestionMode { get; set; }
        private FaqQuestion selectedQuestion;
        private FaqQuestion SelectedQuestion
        {
            get { return selectedQuestion; }
            set
            {
                selectedQuestion = value;
                SelectedQuestionPortfolios = null;
                if (selectedQuestion != null)
                {
                    if (SelectedQuestionMode == SelectedQuestionModeEnum.NewQuestion)
                        SelectedQuestionPortfolios = new List<FaqPortfolio>();
                    else
                        SelectedQuestionPortfolios = FaqPortfolio.GetByQuestion(selectedQuestion);
                }
            }
        }
        private List<FaqPortfolio> SelectedQuestionPortfolios { get; set; }

        bool codeInitiated = false;
        public void LoadQuestion(FaqQuestion question)
        {
            LoadQuestion(question, SelectedQuestionModeEnum.ExternallyLoadedQuestion);
        }
        private void LoadQuestion(FaqQuestion question, SelectedQuestionModeEnum mode)
        {
            if (!IsHandleCreated)
            {
                //TODO:  RichTextBox is the only type of control that has problems like this.
                //If the control is accessed before the handle is created, the form attempts to dispose itself
                this.Load += (o, ea) => LoadQuestion(question, mode);
                return;
            }
            codeInitiated = true;
            SelectedQuestionMode = mode;
            SelectedQuestion = question;
            for (int i = 0; i < AssignedPortfoliosList.Items.Count; i++)
            {
                AssignedPortfoliosList.SetItemChecked(i, false);
            }
            if (question != null)
            {
                if (QuestionBox.Text != question.Question)
                    QuestionBox.Text = question.Question;
                try
                {
                    AnswerBox.DocumentRtf = null;
                    AnswerBox.DocumentRtf = question.Answer;
                }
                catch (Exception)
                {
                    AnswerBox.DocumentText = question.Answer;
                }
                AssignedGroupsList.SelectedIndex = groups.Select((o, i) => new { Index = i, QuestionGroupId = o.QuestionGroupId })
                                                    .Single(o => o.QuestionGroupId == question.QuestionGroupId).Index;
                for (int i = 0; i < AssignedPortfoliosList.Items.Count; i++)
                {
                    int portfolioId = (AssignedPortfoliosList.Items[i] as FaqPortfolio).PortfolioId;
                    if (SelectedQuestionPortfolios.Select(o => o.PortfolioId).Contains(portfolioId))
                        AssignedPortfoliosList.SetItemChecked(i, true);
                }
                LastEditedLabel.Text = string.Format("Last edited on {0:MM/dd/yyyy} by {1}", question.LastUpdatedOn, question.LastUpdatedBy);
            }
            else
            {
                QuestionBox.Text = "";
                AnswerBox.DocumentRtf = "";
                AssignedGroupsList.SelectedIndex = -1;
            }
            DetectChanges();
            codeInitiated = false;
        }

        private void QuestionBox_TextChanged(object sender, EventArgs e)
        {
            if (!codeInitiated)
                DetectChanges();
        }

        private void AssignedGroupsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!codeInitiated)
                DetectChanges();
        }

        private void AssignedPortfoliosList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!codeInitiated)
                DetectChanges();
        }

        private void DetectChanges()
        {
            FaqQuestion question = SelectedQuestion;
            bool hasChanges = false;
            bool newQuestion = false;
            if (question != null)
            {
                newQuestion = question.QuestionId == -1;
                bool questionChanged = question.Question != QuestionBox.Text;
                QuestionLabel.MakeFontBold(questionChanged);

                bool answerChanged = AnswerBox.DocumentRtfHasChanged;
                AnswerLabel.MakeFontBold(answerChanged);

                bool groupChanged = question.QuestionGroupId != groups[AssignedGroupsList.SelectedIndex].QuestionGroupId;
                AssignedGroupLabel.MakeFontBold(groupChanged);

                var selected = AssignedPortfoliosList.CheckedItems.Cast<FaqPortfolio>().Select(o => o.PortfolioId).ToArray();
                var orig = SelectedQuestionPortfolios.Select(o => o.PortfolioId).ToArray();
                bool portfoliosChanged = !Enumerable.SequenceEqual(selected, orig);
                AssignedPortfoliosLabel.MakeFontBold(portfoliosChanged);

                hasChanges = questionChanged || answerChanged || groupChanged || portfoliosChanged;
            }
            bool external = SelectedQuestionMode == SelectedQuestionModeEnum.ExternallyLoadedQuestion;
            bool noQuestion = question == null;
            QuestionBox.Enabled = AnswerBox.Enabled = AssignedGroupsList.Enabled = AssignedPortfoliosList.Enabled = !noQuestion;
            QuestionFilterBox.Enabled = !hasChanges && !external && !newQuestion;
            CancelButton.Enabled = hasChanges || external || newQuestion;
            NewButton.Enabled = !hasChanges && !external;
            SaveButton.Enabled = hasChanges && !noQuestion;
            DeleteButton.Enabled = !newQuestion && !noQuestion;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (SelectedQuestion != null)
            {
                if (Dialog.Warning.YesNo("Are you sure you want to delete question \"" + SelectedQuestion.Question + "\"?"))
                {
                    FaqQuestion.Delete(SelectedQuestion);
                    SyncData(true);
                    LoadQuestion(QuestionFilterBox.SelectedQuestion, SelectedQuestionModeEnum.Normal);
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            LoadQuestion(QuestionFilterBox.SelectedQuestion, SelectedQuestionModeEnum.Normal);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (SelectedQuestion != null)
            {
                FaqQuestion updated = SelectedQuestion;
                bool newQuestion = updated.QuestionId == -1;
                updated.Question = QuestionBox.Text;
                updated.Answer = AnswerBox.DocumentRtf;
                updated.QuestionGroupId = groups[AssignedGroupsList.SelectedIndex].QuestionGroupId;
                var portfolios = AssignedPortfoliosList.CheckedItems.Cast<FaqPortfolio>();
                if (!newQuestion)
                    FaqQuestion.Save(updated, portfolios);
                else
                    FaqQuestion.Add(updated, portfolios);
                if (pendingQuestion != null)
                {
                    FaqPendingQuestion.Approve(pendingQuestion);
                    pendingQuestion = null;
                }
                SyncData(true);
                if (!newQuestion)
                    LoadQuestion(updated, SelectedQuestionModeEnum.Normal);
                else
                {
                    QuestionFilterBox.ClearFields();
                    QuestionFilterBox.Search();
                    QuestionFilterBox.SetQuestion(updated);
                }
                DetectChanges();
            }
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            QuestionFilterBox.Enabled = false;
            var newQuestion = new FaqQuestion() { QuestionId = -1, QuestionGroupId = groups.First().QuestionGroupId };
            LoadQuestion(newQuestion, SelectedQuestionModeEnum.NewQuestion);
        }

        FaqPendingQuestion pendingQuestion;
        public void LoadNewPendingQuestion(FaqPendingQuestion question)
        {
            pendingQuestion = question;
            QuestionFilterBox.Enabled = false;
            FaqQuestion fq = new FaqQuestion() { QuestionId = -1, Question = question.Question, Answer = question.Notes, QuestionGroupId = groups[0].QuestionGroupId };
            LoadQuestion(fq, SelectedQuestionModeEnum.NewQuestion);
        }
    }
}

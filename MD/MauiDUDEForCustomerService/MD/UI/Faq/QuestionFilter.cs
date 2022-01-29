using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MD
{
    public partial class QuestionFilter : UserControl
    {
        public QuestionFilter()
        {
            InitializeComponent();
        }

        public delegate void OnQuestionSelected(QuestionFilter sender, FaqQuestion selected);
        public event OnQuestionSelected QuestionSelected;

        bool codeInitiated = false;
        private List<FaqQuestionGroup> groups;
        private List<FaqPortfolio> portfolios;
        public void SyncData()
        {
            SyncData(true);
        }
        private void SyncData(bool alsoSearch)
        {
            codeInitiated = true;
            groups = FaqQuestionGroup.GetAll();
            List<FaqQuestionGroup> filterGroups = new List<FaqQuestionGroup>(groups);
            SearchGroupsList.DisplayMember = "GroupName";
            FaqQuestionGroup selectedGroup = null;
            if (SearchGroupsList.SelectedIndex > 0)
                selectedGroup = groups[SearchGroupsList.SelectedIndex - 1];
            filterGroups.Insert(0, new FaqQuestionGroup() { QuestionGroupId = -1, GroupName = "Any Group" });
            SearchGroupsList.BeginUpdate();
            SearchGroupsList.DataSource = filterGroups;
            if (selectedGroup != null)
                SearchGroupsList.SelectedIndex = groups.Select((o, i) => new { Index = i + 1, Match = o.QuestionGroupId == selectedGroup.QuestionGroupId })
                                                       .Single(o => o.Match).Index;
            SearchGroupsList.EndUpdate();


            bool firstSearch = portfolios == null;
            List<FaqPortfolio> selectedPortfolios = new List<FaqPortfolio>();
            bool includeAll = false;
            if (!firstSearch)
                foreach (int index in SearchPortfoliosList.CheckedIndices)
                    if (index == 0)
                        includeAll = true;
                    else
                        selectedPortfolios.Add(portfolios[index - 1]);
            if (firstSearch) includeAll = true;
            portfolios = FaqPortfolio.GetAll();
            SearchPortfoliosList.BeginUpdate();
            SearchPortfoliosList.Items.Clear();
            SearchPortfoliosList.DisplayMember = "PortfolioName";
            SearchPortfoliosList.Items.Add("(unassigned)", includeAll);
            foreach (FaqPortfolio portfolio in portfolios)
            {
                bool include = selectedPortfolios.Any(o => o.PortfolioId == portfolio.PortfolioId);
                if (firstSearch) include = true;
                SearchPortfoliosList.Items.Add(portfolio, include);
            }
            SearchPortfoliosList.EndUpdate();

            codeInitiated = false;
            if (alsoSearch)
                Search();
        }
        private List<FaqQuestion> questions;

        private HashSet<string> loggedSearches = new HashSet<string>();
        public void Search()
        {
            if (codeInitiated) return;
            codeInitiated = true;
            FaqQuestion selected = null;
            if (QuestionsList.SelectedIndex != -1)
                selected = questions[QuestionsList.SelectedIndex];
            string query = SearchBox.Text;
            FaqQuestionGroup group = null;
            if (SearchGroupsList.SelectedIndex > 0) //0 is the default element
                group = groups[SearchGroupsList.SelectedIndex - 1];
            List<FaqPortfolio> checkedPortfolios = new List<FaqPortfolio>();
            foreach (var item in SearchPortfoliosList.CheckedItems)
                if (item is FaqPortfolio)
                    checkedPortfolios.Add((FaqPortfolio)item);
            bool includeQuestionsWithoutPortfolios = SearchPortfoliosList.CheckedIndices.Contains(0); //first item is option to include questions with no portfolios
            QuestionsList.DisplayMember = "Question";
            QuestionsList.BeginUpdate(); //reduces flicker, follow with EndUpdate

            QuestionsList.DataSource = questions = FaqQuestion.Search(query, group, checkedPortfolios, includeQuestionsWithoutPortfolios);
            if (QuestionsList.Items.Count == 1)
                FaqLog.InsertLog(questions[0]);
            if (selected != null)
                if (!SetQuestion(selected))
                    selected = null;

            QuestionsList.EndUpdate();
            if (QuestionSelected != null)
                if (selected == null)
                    QuestionSelected(this, questions.FirstOrDefault());

            if (!loggedSearches.Contains(SearchBox.Text) && !string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                FaqQuestion.RecordQuestion(SearchBox.Text);
                loggedSearches.Add(SearchBox.Text);
            }

            codeInitiated = false;
        }

        //private void SearchBox_TextChanged(object sender, EventArgs e)
        //{
        //    Search();
        //}

        private void SearchGroupsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            QuestionsList.SelectedIndex = -1;
            Search();
        }

        private void SearchPortfoliosList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //The ItemCheck event is triggered BEFORE the control has been updated.
            //To counter this, we use BeginInvoke, which only executes code until all events
            //have resolved and the UI thread is idle.  This ensures that the control has
            //actually been updated when this code is executed.
            if (this.IsHandleCreated && !codeInitiated)
                this.BeginInvoke((Action)Search);
        }

        private void QuestionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (codeInitiated) return;
            if (QuestionsList.SelectedIndex != -1)
            {
                if (QuestionSelected != null)
                    QuestionSelected(this, questions[QuestionsList.SelectedIndex]);
            }
            else
                QuestionSelected(this, null);
        }

        private void QuestionsList_Click(object sender, EventArgs e)
        {
            if (QuestionsList.SelectedIndex > -1)
                FaqLog.InsertLog(questions[QuestionsList.SelectedIndex]);
        }

        public void ClearFields()
        {
            SearchBox.Text = "";
            SetGroup((int?)null);
            SetPortfolio(true, portfolios.ToArray());
        }

        public void SetGroup(FaqQuestionGroup group)
        {
            SetGroup(group.QuestionGroupId);
        }
        public void SetGroup(int? questionGroupId)
        {
            var selection = groups.Select((o, i) => new { Index = i + 1, Selection = o.QuestionGroupId == questionGroupId }).SingleOrDefault(o => o.Selection);
            if (selection == null)
                SearchGroupsList.SelectedIndex = 0;
            else
                SearchGroupsList.SelectedIndex = selection.Index;
        }

        public void SetPortfolio(bool includeAny, params FaqPortfolio[] portfolios)
        {
            SearchPortfoliosList.SetItemChecked(0, includeAny);
            for (int i = 1; i < SearchPortfoliosList.Items.Count; i++)
            {
                var portfolio = this.portfolios[i - 1];
                bool match = portfolios.Any(p => p.PortfolioId == portfolio.PortfolioId);
                SearchPortfoliosList.SetItemChecked(i, match);
            }
        }

        public bool SetQuestion(FaqQuestion question)
        {
            var obj = questions.Select((o, i) => new { Index = i, Match = o.QuestionId == question.QuestionId }).SingleOrDefault(o => o.Match);
            if (obj != null)
            {
                QuestionsList.SelectedIndex = obj.Index;
                return true;
            }
            return false;
        }

        public FaqQuestion SelectedQuestion
        {
            get
            {
                if (QuestionsList.SelectedIndex == -1)
                    return null;
                else return questions[QuestionsList.SelectedIndex];
            }
        }

        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search();
            }
        }
    }
}

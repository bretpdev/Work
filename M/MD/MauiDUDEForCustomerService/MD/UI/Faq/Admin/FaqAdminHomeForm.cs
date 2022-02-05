using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using System.Xml;
using System.Threading;

namespace MD
{
    public partial class FaqAdminHomeForm : BaseForm
    {
        #region Singleton
        public static FaqAdminHomeForm Instance
        {
            get
            {
                var instance = (FaqAdminHomeForm)Hlpr.UI.Forms.SingleOrDefault(f => f is FaqAdminHomeForm);
                if (instance == null)
                    instance = Hlpr.UI.CreateAndShow<FaqAdminHomeForm>();
                return instance;
            }
        }
        public static void ShowAdmin()
        {
            Instance.Show();
        }
        #endregion
        public FaqAdminHomeForm()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                ExistingQuestionsTabControl.Setup();
                ExistingQuestionsTabControl.DataSynced += () =>
                {
                    PendingQuestionsControl.SyncData();
                    GroupsControl.SyncData();
                    PortfoliosControl.SyncData();
                };
                InitGroupPortfolioManagement();
                PendingQuestionsControl.Init(PendingQuestions.PendingQuestionsMode.Admin);
            }
        }

        SingleValueManager<FaqQuestionGroup> GroupsControl;
        SingleValueManager<FaqPortfolio> PortfoliosControl;
        private void InitGroupPortfolioManagement()
        {
            GroupsControl = new SingleValueManager<FaqQuestionGroup>("Group", "GroupName",
               () => FaqQuestionGroup.GetAll(),
               (g) => FaqQuestion.Search(null, g, null, true),
               (g) => FaqQuestionGroup.Delete(g),
               (s) => FaqQuestionGroup.Add(new FaqQuestionGroup() { GroupName = s }),
               (g, s) =>
               {
                   g.GroupName = s;
                   FaqQuestionGroup.Save(g);
               });
            GroupsControl.QuestionSelected += Either_QuestionSelected;
            GroupsControl.DataSynced += () => ExistingQuestionsTabControl.SyncData();
            GroupsControl.Dock = DockStyle.Fill;
            GroupPortfolioTable.Controls.Add(GroupsControl, 0, 0);

            PortfoliosControl = new SingleValueManager<FaqPortfolio>("Portfolio", "PortfolioName",
                () => FaqPortfolio.GetAll(),
                (p) => FaqQuestion.Search(null, null, new FaqPortfolio[] { p }.ToList(), false),
                (p) => FaqPortfolio.Delete(p),
                (s) => FaqPortfolio.Add(new FaqPortfolio() { PortfolioName = s }),
                (p, s) =>
                {
                    p.PortfolioName = s;
                    FaqPortfolio.Save(p);
                });
            PortfoliosControl.QuestionSelected += Either_QuestionSelected;
            PortfoliosControl.DataSynced += () => ExistingQuestionsTabControl.SyncData();
            PortfoliosControl.Dock = DockStyle.Fill;
            GroupPortfolioTable.Controls.Add(PortfoliosControl, 0, 1);
        }

        private void Either_QuestionSelected(FaqQuestion question)
        {
            ExistingQuestionsTabControl.LoadQuestion(question);
            MainTabControl.SelectedTab = ExistingQuestionsTab;
        }

        private void PendingQuestionsControl_QuestionSelectedForApproval(PendingQuestions control, FaqPendingQuestion question)
        {
            ExistingQuestionsTabControl.LoadNewPendingQuestion(question);
            MainTabControl.SelectedTab = ExistingQuestionsTab;
        }

        private void PendingQuestionsControl_DataSynced(PendingQuestions control, List<FaqPendingQuestion> questions)
        {
            PendingQuestionsTab.Text = "Pending Questions (" + questions.Count + ")";
        }
    }
}

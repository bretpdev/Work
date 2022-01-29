using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Uheaa.Common;

namespace MD
{
    public partial class FaqViewerForm : BaseForm
    {
        #region Singleton
        public static FaqViewerForm Instance
        {
            get
            {
                var instance = (FaqViewerForm)Hlpr.UI.Forms.SingleOrDefault(f => f is FaqViewerForm);
                if (instance == null)
                    instance = Hlpr.UI.CreateAndShow<FaqViewerForm>();
                return instance;
            }
        }
        public static void ShowViewer()
        {
            Instance.Invoke(Instance.Activate);
        }
        #endregion
        public FaqViewerForm()
        {
            InitializeComponent();
            PendingQuestionsControl.Init(PendingQuestions.PendingQuestionsMode.Representative);
            SearchQuestionsBox.SyncData();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void SearchQuestionsBox_QuestionSelected(QuestionFilter sender, FaqQuestion selected)
        {
            if (!IsHandleCreated)
            {
                this.Load += (o, ea) => SearchQuestionsBox_QuestionSelected(sender, selected);
                return;
            }

            if (selected == null)
            {
                QuestionLabel.Text = "";
                AnswerBox.Rtf = "";
                RelatedPanel.Visible = false;
                LastEditedLabel.Text = "";
            }
            else
            {
                QuestionLabel.Text = selected.Question;
                var size = QuestionLabel.GetPreferredSize(new Size(QuestionAnswerPanel.Width, int.MaxValue));
                QuestionAnswerPanel.RowStyles[0] = new RowStyle(SizeType.Absolute, size.Height);
                string userName = selected.LastUpdatedBy;
                userName = UsernameHelper.GetDisplayName(userName);
                LastEditedLabel.Text = string.Format("Last edited on {0:MM/dd/yyyy} by {1}", selected.LastUpdatedOn, userName);
                AnswerBox.Rtf = "";
                try
                {
                    AnswerBox.Rtf = selected.Answer;
                }
                catch (ArgumentException)
                {
                    AnswerBox.Text = selected.Answer;
                }
                RelatedPanel.Visible = true;
                RelatedPanel.Controls.Clear();
                RelatedPanel.Controls.Add(new Label() { Text = "Related:", AutoSize = true });
                var groupLabel = new LinkLabel() { Text = selected.QuestionGroupName, AutoSize = true };
                groupLabel.Click += (o, ea) =>
                {
                    SearchQuestionsBox.ClearFields();
                    SearchQuestionsBox.SetGroup(selected.QuestionGroupId);
                };
                RelatedPanel.Controls.Add(groupLabel);
                foreach (var portfolio in FaqPortfolio.GetByQuestion(selected))
                {
                    var portfolioLabel = new LinkLabel() { Text = portfolio.PortfolioName, AutoSize = true };
                    portfolioLabel.Click += (o, ea) =>
                    {
                        SearchQuestionsBox.ClearFields();
                        SearchQuestionsBox.SetPortfolio(false, portfolio);
                    };
                    RelatedPanel.Controls.Add(portfolioLabel);
                }
            }
        }
    }
}

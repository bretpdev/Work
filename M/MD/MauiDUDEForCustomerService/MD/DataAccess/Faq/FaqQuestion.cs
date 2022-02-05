using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;
using Uheaa.Common;
using System.Data;

namespace MD
{
    public class FaqQuestion
    {
        [PrimaryKey]
        public int QuestionId { get; set; }
        public int QuestionGroupId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        [DbReadOnly]
        public DateTime LastUpdatedOn { get; set; }
        [DbReadOnly]
        public string LastUpdatedBy { get; set; }
        [DbReadOnly]
        public string QuestionGroupName { get; set; }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.QuestionsSearch")]
        public static List<FaqQuestion> Search(string query, FaqQuestionGroup group, List<FaqPortfolio> portfolios, bool includeQuestionsWithoutPortfolios)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            if (query.IsPopulated())
                parms.Add(new SqlParameter("SearchTerm", query));
            if (group != null)
                parms.Add(new SqlParameter("QuestionGroupId", group.QuestionGroupId));
            parms.Add(ToParameter(portfolios ?? new List<FaqPortfolio>()));
            parms.Add(new SqlParameter("IncludeQuestionsWithoutPortfolios", includeQuestionsWithoutPortfolios));
            return DataAccessHelper.ExecuteList<FaqQuestion>("faq.QuestionsSearch", DataAccessHelper.Database.MauiDude,
                parms.ToArray());
        }

        private static SqlParameter ToParameter(IEnumerable<FaqPortfolio> portfolios)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PortfolioId", typeof(int));
            foreach (FaqPortfolio portfolio in portfolios)
                dt.Rows.Add(portfolio.PortfolioId);

            SqlParameter udt = new SqlParameter("PortfolioIds", dt);
            udt.TypeName = "faq.PortfolioIds";
            return udt;
        }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.QuestionDelete")]
        public static void Delete(FaqQuestion SelectedQuestion)
        {
            DataAccessHelper.Execute("faq.QuestionDelete", DataAccessHelper.Database.MauiDude, SqlParams.Delete(SelectedQuestion));
        }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.QuestionUpdate")]
        public static void Save(FaqQuestion question, IEnumerable<FaqPortfolio> portfolios)
        {
            List<SqlParameter> parms = SqlParams.Specifics<FaqQuestion>(question, o => o.QuestionId, o => o.QuestionGroupId, o => o.Question, o => o.Answer).ToList();
            parms.Add(ToParameter(portfolios));
            DataAccessHelper.Execute("faq.QuestionUpdate", DataAccessHelper.Database.MauiDude, parms.ToArray());
        }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.QuestionInsert")]
        public static void Add(FaqQuestion question, IEnumerable<FaqPortfolio> portfolios)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.AddRange(SqlParams.Specifics<FaqQuestion>(question, o => o.QuestionGroupId, o => o.Question, o => o.Answer).ToList());
            parms.Add(ToParameter(portfolios));
            question.QuestionId = DataAccessHelper.ExecuteSingle<int>("faq.QuestionInsert", DataAccessHelper.Database.MauiDude, parms.ToArray());
        }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.RecordQuestion")]
        public static void RecordQuestion(string question)
        {
            DataAccessHelper.Execute("faq.RecordQuestion", DataAccessHelper.Database.MauiDude, SqlParams.Single("Question", question));
        }
    }
}

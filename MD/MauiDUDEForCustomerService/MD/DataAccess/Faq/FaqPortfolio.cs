using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;

namespace MD
{
    public class FaqPortfolio
    {
        [PrimaryKey]
        public int PortfolioId { get; set; }
        public string PortfolioName { get; set; }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.PortfoliosSelectAll")]
        public static List<FaqPortfolio> GetAll()
        {
            return DataAccessHelper.ExecuteList<FaqPortfolio>("faq.PortfoliosSelectAll", DataAccessHelper.Database.MauiDude);
        }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.PortfoliosSelectByQuestion")]
        public static List<FaqPortfolio> GetByQuestion(FaqQuestion question)
        {
            return DataAccessHelper.ExecuteList<FaqPortfolio>("faq.PortfoliosSelectByQuestion", DataAccessHelper.Database.MauiDude, new SqlParameter("QuestionId", question.QuestionId));
        }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.PortfolioUpdate")]
        public static void Save(FaqPortfolio portfolio)
        {
            DataAccessHelper.Execute("faq.PortfolioUpdate", DataAccessHelper.Database.MauiDude, SqlParams.Update(portfolio));
        }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.PortfolioInsert")]
        public static void Add(FaqPortfolio portfolio)
        {
            DataAccessHelper.Execute("faq.PortfolioInsert", DataAccessHelper.Database.MauiDude, SqlParams.Insert(portfolio));
        }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.PortfolioDelete")]
        public static void Delete(FaqPortfolio portfolio)
        {
            DataAccessHelper.Execute("faq.PortfolioDelete", DataAccessHelper.Database.MauiDude, SqlParams.Delete(portfolio));
        }
    }
}

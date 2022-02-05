using Uheaa.Common.DataAccess;

namespace MD
{
    public class FaqLog
    {
        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.InsertLog")]
        public static void InsertLog(FaqQuestion question)
        {
            DataAccessHelper.Execute("faq.InsertLog", DataAccessHelper.Database.MauiDude,
                SqlParams.Single("QuestionGroupId", question.QuestionGroupId),
                SqlParams.Single("QuestionId", question.QuestionId));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;
using Uheaa.Common;

namespace MD
{
    public class FaqQuestionGroup
    {
        [PrimaryKey]
        public int QuestionGroupId { get; set; }
        public string GroupName { get; set; }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.QuestionGroupsSelectAll")]
        public static List<FaqQuestionGroup> GetAll()
        {
            return DataAccessHelper.ExecuteList<FaqQuestionGroup>("faq.QuestionGroupsSelectAll", DataAccessHelper.Database.MauiDude);
        }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.QuestionGroupUpdate")]
        public static void Save(FaqQuestionGroup group)
        {
            DataAccessHelper.Execute("faq.QuestionGroupUpdate", DataAccessHelper.Database.MauiDude, SqlParams.Update(group));
        }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.QuestionGroupInsert")]
        public static void Add(FaqQuestionGroup group)
        {
            DataAccessHelper.Execute("faq.QuestionGroupInsert", DataAccessHelper.Database.MauiDude, SqlParams.Insert(group));
        }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.QuestionGroupDelete")]
        public static void Delete(FaqQuestionGroup group)
        {
            DataAccessHelper.Execute("faq.QuestionGroupDelete", DataAccessHelper.Database.MauiDude, SqlParams.Delete(group));
        }
    }
}

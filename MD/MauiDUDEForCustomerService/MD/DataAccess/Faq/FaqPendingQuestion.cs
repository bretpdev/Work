using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;
using Uheaa.Common;

namespace MD
{
    public class FaqPendingQuestion
    {
        [PrimaryKey]
        public int PendingQuestionId { get; set; }
        public string Question { get; set; }
        public string Notes { get; set; }
        [DbReadOnly]
        public DateTime SubmittedOn { get; set; }
        [DbReadOnly]
        public string SubmittedBy { get; set; }
        [DbReadOnly]
        public DateTime? ProcessedOn { get; set; }
        [DbReadOnly]
        public string ApprovedBy { get; set; }
        [DbReadOnly]
        public string RejectedBy { get; set; }
        [DbReadOnly]
        public string WithdrawnBy { get; set; }
        [DbIgnore]
        public string Status
        {
            get
            {
                if (!ApprovedBy.IsNullOrEmpty())
                    return "Approved";
                if (!RejectedBy.IsNullOrEmpty())
                    return "Rejected";
                if (!WithdrawnBy.IsNullOrEmpty())
                    return "Withdrawn";
                return "Pending";
            }
        }
        [DbIgnore]
        public string SubmittedOnString { get { return SubmittedOn.ToString("MM/dd/yyyy"); } }
        [DbIgnore]
        public string SubmittedByString { get { return SubmittedBy.Replace("UHEAA\\", ""); } }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.PendingQuestionsSelectByUser")]
        public static List<FaqPendingQuestion> GetByCurrentUser()
        {
            return DataAccessHelper.ExecuteList<FaqPendingQuestion>("faq.PendingQuestionsSelectByUser", DataAccessHelper.Database.MauiDude, 
                SqlParams.Single("UserName", Environment.UserDomainName + "\\" + Environment.UserName)).ToList();
        }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.PendingQuestionsSelectPending")]
        public static List<FaqPendingQuestion> GetAllPending()
        {
            return DataAccessHelper.ExecuteList<FaqPendingQuestion>("faq.PendingQuestionsSelectPending", DataAccessHelper.Database.MauiDude);
        }

        public static void Withdraw(FaqPendingQuestion pending)
        {
            PendingQuestionChange("Withdraw", pending);
        }

        public static void Approve(FaqPendingQuestion pending)
        {
            PendingQuestionChange("Approve", pending);
        }

        public static void Reject(FaqPendingQuestion pending)
        {
            PendingQuestionChange("Reject", pending);
        }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.PendingQuestionApprove")]
        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.PendingQuestionReject")]
        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.PendingQuestionWithdraw")]
        private static void PendingQuestionChange(string modifier, FaqPendingQuestion question)
        {
            DataAccessHelper.Execute("faq.PendingQuestion" + modifier, DataAccessHelper.Database.MauiDude, SqlParams.Single("PendingQuestionId", question.PendingQuestionId));
        }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "faq.PendingQuestionInsert")]
        public static void Submit(FaqPendingQuestion question)
        {
            DataAccessHelper.Execute("faq.PendingQuestionInsert", DataAccessHelper.Database.MauiDude, SqlParams.Insert(question));
        }
    }
}

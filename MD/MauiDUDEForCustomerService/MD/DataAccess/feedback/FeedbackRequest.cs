using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;

namespace MD
{
    public enum FeedbackTypeEnum
    {
        Bug,
        Feature
    }
    public class FeedbackSubmission
    {
        [PrimaryKey]
        public int FeedbackSubmissionId { get; set; }
        public string FeedbackType { get; set; }
        public string FeedbackDetails { get; set; }
        public string AppVersion { get; set; }
        public byte[] Screenshot { get; set; }
        public byte[] ScreenshotOverlay { get; set; }
        public byte[] ReflectionScreenshot { get; set; }
        public byte[] ReflectionScreenshotOverlay { get; set; }
        [DbReadOnly]
        public DateTime RequestedOn { get; set; }
        [DbIgnore]
        public string RequestedOnString { get { return RequestedOn.ToString("MM/dd/yyyy"); } }
        [DbReadOnly]
        public string RequestedBy { get; set; }
        [DbReadOnly]
        public string RequestedByString { get { return RequestedBy.Replace("UHEAA\\", ""); } }
        [DbReadOnly]
        public DateTime ProcessedOn { get; set; }
        [DbReadOnly]
        public string ProcessedBy { get; set; }
        [DbIgnore]
        public FeedbackTypeEnum FeedbackTypeValue { get { return FeedbackType == "bug" ? FeedbackTypeEnum.Bug : FeedbackTypeEnum.Feature; } }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "feedback.FeedbackSubmissionsSelectPending")]
        public static List<FeedbackSubmission> GetPending()
        {
            return DataAccessHelper.ExecuteList<FeedbackSubmission>("feedback.FeedbackSubmissionsSelectPending", DataAccessHelper.Database.MauiDude)
                .OrderByDescending(o => o.RequestedOn).ToList();
        }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "feedback.FeedbackSubmissionInsert")]
        public static void SubmitRequest(FeedbackSubmission request)
        {
            DataAccessHelper.Execute("feedback.FeedbackSubmissionInsert", DataAccessHelper.Database.MauiDude, SqlParams.Insert(request));
        }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "feedback.FeedbackSubmissionProcess")]
        public static void ProcessFeedback(FeedbackSubmission request)
        {
            DataAccessHelper.Execute("feedback.FeedbackSubmissionProcess", DataAccessHelper.Database.MauiDude, SqlParams.PK(request));
        }
    }
}

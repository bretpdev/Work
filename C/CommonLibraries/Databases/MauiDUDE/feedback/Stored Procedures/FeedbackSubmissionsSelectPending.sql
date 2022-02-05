CREATE PROCEDURE [feedback].[FeedbackSubmissionsSelectPending]
AS
	select FeedbackSubmissionId, FeedbackType, FeedbackDetails, Screenshot, ScreenshotOverlay, ReflectionScreenshot, ReflectionScreenshotOverlay, AppVersion, RequestedOn, RequestedBy
	  from feedback.FeedbackSubmissions
	 where ProcessedOn is null
RETURN 0
grant execute on [feedback].[FeedbackSubmissionsSelectPending] to [db_executor]
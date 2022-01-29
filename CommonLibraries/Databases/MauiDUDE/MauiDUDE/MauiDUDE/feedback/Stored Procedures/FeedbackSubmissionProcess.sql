CREATE PROCEDURE [feedback].[FeedbackSubmissionProcess]
	@FeedbackSubmissionId int
AS
	update feedback.FeedbackSubmissions set ProcessedOn = getdate(), ProcessedBy=SYSTEM_USER
	 where FeedbackSubmissionId = @FeedbackSubmissionId
RETURN 0
grant execute on [feedback].[FeedbackSubmissionProcess] to [db_executor]

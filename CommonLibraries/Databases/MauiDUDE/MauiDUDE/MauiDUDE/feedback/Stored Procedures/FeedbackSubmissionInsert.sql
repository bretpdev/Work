CREATE PROCEDURE [feedback].[FeedbackSubmissionInsert]
	@FeedbackType varchar(7),
	@FeedbackDetails varchar(4000),
	@Screenshot image = null,
	@ScreenshotOverlay image = null,
	@ReflectionScreenshot image = null,
	@ReflectionScreenshotOverlay image = null,
	@AppVersion varchar(20)
AS
	insert into feedback.FeedbackSubmissions(FeedbackType, FeedbackDetails, Screenshot, ScreenshotOverlay, ReflectionScreenshot, ReflectionScreenshotOverlay, AppVersion)
	values (@FeedbackType, @FeedbackDetails, @Screenshot, @ScreenshotOverlay, @ReflectionScreenshot, @ReflectionScreenshotOverlay, @AppVersion)
RETURN 0

grant execute on [feedback].[FeedbackSubmissionInsert] to [db_executor]

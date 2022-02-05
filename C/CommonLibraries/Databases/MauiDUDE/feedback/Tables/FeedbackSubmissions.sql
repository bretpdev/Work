CREATE TABLE [feedback].[FeedbackSubmissions]
(
	[FeedbackSubmissionId] INT NOT NULL PRIMARY KEY IDENTITY,
	[FeedbackType] varchar(7) not null, 
    [FeedbackDetails] VARCHAR(4000) NOT NULL, 
    [Screenshot] IMAGE NULL, 
	[ScreenshotOverlay] IMAGE NULL,
	[ReflectionScreenshot] IMAGE NULL,
	[ReflectionScreenshotOverlay] IMAGE NULL,
	[AppVersion] VARCHAR(20) NULL,
    [RequestedBy] VARCHAR(50) NOT NULL DEFAULT SYSTEM_USER, 
    [RequestedOn] SMALLDATETIME NOT NULL DEFAULT getdate(), 
    [ProcessedOn] SMALLDATETIME NULL, 
    [ProcessedBy] VARCHAR(50) NULL, 
    CONSTRAINT [CK_FeedbackSubmissions_FeedbackType] CHECK (FeedbackType in ('feature', 'bug'))
)

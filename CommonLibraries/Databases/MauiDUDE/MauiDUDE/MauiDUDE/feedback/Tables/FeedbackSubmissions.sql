CREATE TABLE [feedback].[FeedbackSubmissions] (
    [FeedbackSubmissionId]        INT            IDENTITY (1, 1) NOT NULL,
    [FeedbackType]                VARCHAR (7)    NOT NULL,
    [FeedbackDetails]             VARCHAR (4000) NOT NULL,
    [Screenshot]                  IMAGE          NULL,
    [ScreenshotOverlay]           IMAGE          NULL,
    [ReflectionScreenshot]        IMAGE          NULL,
    [ReflectionScreenshotOverlay] IMAGE          NULL,
    [AppVersion]                  VARCHAR (20)   NULL,
    [RequestedBy]                 VARCHAR (50)   DEFAULT (suser_sname()) NOT NULL,
    [RequestedOn]                 SMALLDATETIME  DEFAULT (getdate()) NOT NULL,
    [ProcessedOn]                 SMALLDATETIME  NULL,
    [ProcessedBy]                 VARCHAR (50)   NULL,
    PRIMARY KEY CLUSTERED ([FeedbackSubmissionId] ASC),
    CONSTRAINT [CK_FeedbackSubmissions_FeedbackType] CHECK ([FeedbackType]='bug' OR [FeedbackType]='feature')
);


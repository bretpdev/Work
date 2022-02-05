CREATE TABLE [faq].[PendingQuestions]
(
	[PendingQuestionId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Question] VARCHAR(4000) NOT NULL, 
    [Notes] VARCHAR(4000) NOT NULL, 
    [SubmittedOn] SMALLDATETIME NOT NULL DEFAULT getdate(), 
    [SubmittedBy] VARCHAR(50) NOT NULL DEFAULT SYSTEM_USER, 
    [ProcessedOn] SMALLDATETIME NULL, 
    [ApprovedBy] VARCHAR(50) NULL, 
    [RejectedBy] VARCHAR(50) NULL, 
    [WithdrawnBy] VARCHAR(50) NULL
)

GO

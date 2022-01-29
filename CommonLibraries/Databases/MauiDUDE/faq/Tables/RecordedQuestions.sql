CREATE TABLE [faq].[RecordedQuestions]
(
	[RecordedQuestionId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Question] NVARCHAR(MAX) NOT NULL, 
    [DateAsked] DATETIME NOT NULL DEFAULT getdate(), 
    [AskedBy] NVARCHAR(50) NOT NULL DEFAULT SYSTEM_USER
)

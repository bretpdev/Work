CREATE TABLE [faq].[Logs]
(
	[LogId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [QuestionGroupsId] INT NOT NULL, 
    [QuestionsId] INT NOT NULL, 
    [User] VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(), 
    [SearchedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT [FK_Logs_QuestionGroups] FOREIGN KEY ([QuestionGroupsId]) REFERENCES [faq].[QuestionGroups]([QuestionGroupId]), 
    CONSTRAINT [FK_Logs_Questions] FOREIGN KEY ([QuestionsId]) REFERENCES [faq].[Questions]([QuestionId])
)

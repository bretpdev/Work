CREATE TABLE [faq].[RecordedQuestions] (
    [RecordedQuestionId] INT            IDENTITY (1, 1) NOT NULL,
    [Question]           NVARCHAR (MAX) NOT NULL,
    [DateAsked]          DATETIME       DEFAULT (getdate()) NOT NULL,
    [AskedBy]            NVARCHAR (50)  DEFAULT (suser_sname()) NOT NULL,
    PRIMARY KEY CLUSTERED ([RecordedQuestionId] ASC)
);


CREATE TABLE [faq].[PendingQuestions] (
    [PendingQuestionId] INT            IDENTITY (1, 1) NOT NULL,
    [Question]          VARCHAR (4000) NOT NULL,
    [Notes]             VARCHAR (4000) NOT NULL,
    [SubmittedOn]       SMALLDATETIME  DEFAULT (getdate()) NOT NULL,
    [SubmittedBy]       VARCHAR (50)   DEFAULT (suser_sname()) NOT NULL,
    [ProcessedOn]       SMALLDATETIME  NULL,
    [ApprovedBy]        VARCHAR (50)   NULL,
    [RejectedBy]        VARCHAR (50)   NULL,
    [WithdrawnBy]       VARCHAR (50)   NULL,
    PRIMARY KEY CLUSTERED ([PendingQuestionId] ASC)
);


CREATE TABLE [faq].[QuestionGroups] (
    [QuestionGroupId] INT          IDENTITY (1, 1) NOT NULL,
    [GroupName]       VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([QuestionGroupId] ASC)
);


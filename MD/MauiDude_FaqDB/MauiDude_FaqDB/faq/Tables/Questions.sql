CREATE TABLE [faq].[Questions] (
    [QuestionId]      INT            IDENTITY (1, 1) NOT NULL,
    [QuestionGroupId] INT            NOT NULL,
    [Question]        VARCHAR (4000) NOT NULL,
    [Answer]          VARCHAR (4000) NOT NULL,
    [LastUpdatedOn]   SMALLDATETIME  NULL,
    [LastUpdatedBy]   VARCHAR (50)   NULL,
    PRIMARY KEY CLUSTERED ([QuestionId] ASC),
    CONSTRAINT [FK_Questions_QuestionGroups] FOREIGN KEY ([QuestionGroupId]) REFERENCES [faq].[QuestionGroups] ([QuestionGroupId])
);


GO

CREATE TRIGGER [faq].[Trigger_Questions_UpdateInsert]
    ON [faq].[Questions]
    FOR INSERT, UPDATE
    AS
    BEGIN
        update q
		   set LastUpdatedOn = getdate(), LastUpdatedBy = SYSTEM_USER
		  from faq.Questions q
		  join inserted i on q.QuestionId = i.QuestionId
    END
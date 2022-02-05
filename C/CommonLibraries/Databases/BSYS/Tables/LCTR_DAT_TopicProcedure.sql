CREATE TABLE [dbo].[LCTR_DAT_TopicProcedure] (
    [TopicID]     BIGINT NOT NULL,
    [ProcedureID] BIGINT NOT NULL,
    CONSTRAINT [PK_LCTR_DAT_TopicProcedure] PRIMARY KEY CLUSTERED ([TopicID] ASC, [ProcedureID] ASC),
    CONSTRAINT [FK_LCTR_DAT_TopicProcedure_LCTR_DAT_Procedures] FOREIGN KEY ([ProcedureID]) REFERENCES [dbo].[LCTR_DAT_Procedures] ([ID]),
    CONSTRAINT [FK_LCTR_DAT_TopicProcedure_LCTR_DAT_Topic] FOREIGN KEY ([TopicID]) REFERENCES [dbo].[LCTR_DAT_Topic] ([ID])
);


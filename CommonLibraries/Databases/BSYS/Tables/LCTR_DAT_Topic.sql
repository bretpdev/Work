CREATE TABLE [dbo].[LCTR_DAT_Topic] (
    [ID]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (50)  NOT NULL,
    [Narrative] TEXT          NULL,
    [SearchKey] VARCHAR (500) NULL,
    CONSTRAINT [PK_LCTR_DAT_Topic] PRIMARY KEY CLUSTERED ([ID] ASC)
);


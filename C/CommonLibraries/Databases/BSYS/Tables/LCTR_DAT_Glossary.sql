CREATE TABLE [dbo].[LCTR_DAT_Glossary] (
    [ID]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [Term]       VARCHAR (100)  NOT NULL,
    [Definition] VARCHAR (8000) NULL,
    CONSTRAINT [PK_LCTR_DAT_Glossary] PRIMARY KEY CLUSTERED ([ID] ASC)
);


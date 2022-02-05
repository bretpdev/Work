CREATE TABLE [dbo].[LCTR_DAT_Procedures] (
    [ID]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (50)  NOT NULL,
    [SearchKey] VARCHAR (500) NULL,
    CONSTRAINT [PK_LCTR_DAT_Procedures] PRIMARY KEY CLUSTERED ([ID] ASC)
);


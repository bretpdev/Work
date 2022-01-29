CREATE TABLE [dbo].[LCTR_DAT_Docs] (
    [ID]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [Type]      CHAR (10)     NULL,
    [Name]      VARCHAR (50)  NULL,
    [Path]      VARCHAR (200) NULL,
    [SearchKey] VARCHAR (500) NULL,
    CONSTRAINT [PK_LCTR_DAT_Letters] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_LCTR_DAT_Docs_LCTR_LST_DocType] FOREIGN KEY ([Type]) REFERENCES [dbo].[LCTR_LST_DocType] ([Type])
);


CREATE TABLE [dbo].[LetterTypes] (
    [LetterTypeId] INT          IDENTITY (1, 1) NOT NULL,
    [LetterType]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_LetterTypes] PRIMARY KEY CLUSTERED ([LetterTypeId] ASC) WITH (FILLFACTOR = 95)
);


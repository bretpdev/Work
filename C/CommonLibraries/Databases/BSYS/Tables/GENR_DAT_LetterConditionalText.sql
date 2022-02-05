CREATE TABLE [dbo].[GENR_DAT_LetterConditionalText] (
    [LetterID]       VARCHAR (10)   NOT NULL,
    [MergeFieldName] VARCHAR (50)   NOT NULL,
    [Condition]      VARCHAR (50)   NOT NULL,
    [Text]           VARCHAR (2048) NULL,
    CONSTRAINT [PK_GENR_DAT_LetterConditionalText] PRIMARY KEY CLUSTERED ([LetterID] ASC, [MergeFieldName] ASC, [Condition] ASC)
);


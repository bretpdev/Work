CREATE TABLE [dbo].[Aging360SentToDMCS] (
    [Aging360SentToDMCSId] INT      IDENTITY (1, 1) NOT NULL,
    [BF_SSN]               CHAR (9) NOT NULL,
    [LN_SEQ]               SMALLINT NOT NULL,
    [AGING_DATE]           DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([Aging360SentToDMCSId] ASC)
);


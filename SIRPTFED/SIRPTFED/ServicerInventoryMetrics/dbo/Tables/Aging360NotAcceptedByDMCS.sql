CREATE TABLE [dbo].[Aging360NotAcceptedByDMCS] (
    [Aging360NotAcceptedByDMCSId] INT      IDENTITY (1, 1) NOT NULL,
    [BF_SSN]                      CHAR (9) NOT NULL,
    [LN_SEQ]                      SMALLINT NOT NULL,
    [AGING_DATE]                  DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([Aging360NotAcceptedByDMCSId] ASC)
);


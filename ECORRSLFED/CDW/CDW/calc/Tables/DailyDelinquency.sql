CREATE TABLE [calc].[DailyDelinquency] (
    [BF_SSN]     CHAR (9) NOT NULL,
    [LN_SEQ]     SMALLINT NOT NULL,
    [LD_DLQ_OCC] DATE     NOT NULL,
    [LN_DLQ_MAX] INT      NULL,
    [AddedAt]    DATE     NOT NULL,
    CONSTRAINT [PK_DailyDelinquency] PRIMARY KEY CLUSTERED ([BF_SSN] ASC, [LN_SEQ] ASC, [LD_DLQ_OCC] ASC, [AddedAt] ASC)
);


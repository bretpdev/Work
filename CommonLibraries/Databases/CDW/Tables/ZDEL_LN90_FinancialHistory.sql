CREATE TABLE [dbo].[ZDEL_LN90_FinancialHistory] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL,
    [LN_SEQ]        INT          NOT NULL,
    [LN_FAT_SEQ]    INT          NOT NULL,
    CONSTRAINT [PK_ZDEL_LN90_FinancialHistory] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC, [LN_FAT_SEQ] ASC)
);


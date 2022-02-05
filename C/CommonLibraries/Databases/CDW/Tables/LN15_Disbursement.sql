CREATE TABLE [dbo].[LN15_Disbursement] (
    [DF_SPE_ACC_ID] VARCHAR (10)   NOT NULL,
    [LN_BR_DSB_SEQ] INT            NOT NULL,
    [LA_DSB]        NUMERIC (9, 2) NULL,
    [LD_DSB]        VARCHAR (10)   NULL,
    [LC_DSB_TYP]    VARCHAR (1)    NULL,
    [LC_STA_LON15]  VARCHAR (1)    NULL,
    [LN_SEQ]        INT            NULL,
    [LA_DL_REBATE]  NUMERIC (9, 2) NULL,
    CONSTRAINT [PK_LN15_Disbursement] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_BR_DSB_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN15_Disbursement', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


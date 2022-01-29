CREATE TABLE [dbo].[ZDEL_LN90_FINANCIALTRAN] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL,
    [LN_SEQ]        INT          NOT NULL,
    [LN_FAT_SEQ]    INT          NOT NULL,
    CONSTRAINT [PK_FINANCIALTRAN_DELETE] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC, [LN_FAT_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_LN90_FINANCIALTRAN', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_LN90_FINANCIALTRAN', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transaction sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_LN90_FINANCIALTRAN', @level2type = N'COLUMN', @level2name = N'LN_FAT_SEQ';


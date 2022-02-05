CREATE TABLE [dbo].[ZDEL_BL10_BILL] (
    [DF_SPE_ACC_ID]     VARCHAR (10) NOT NULL,
    [LN_SEQ]            INT          NOT NULL,
    [LD_BIL_CRT]        VARCHAR (10) NOT NULL,
    [LN_SEQ_BIL_WI_DTE] INT          NOT NULL,
    CONSTRAINT [PK_BILL_DELETE] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC, [LD_BIL_CRT] ASC, [LN_SEQ_BIL_WI_DTE] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence bill within date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_BL10_BILL', @level2type = N'COLUMN', @level2name = N'LN_SEQ_BIL_WI_DTE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bill create date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_BL10_BILL', @level2type = N'COLUMN', @level2name = N'LD_BIL_CRT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_BL10_BILL', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_BL10_BILL', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


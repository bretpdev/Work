CREATE TABLE [dbo].[ZDEL_FB10_FORBEARANCE] (
    [DF_SPE_ACC_ID]  VARCHAR (10) NOT NULL,
    [LN_SEQ]         INT          NOT NULL,
    [LF_FOR_CTL_NUM] VARCHAR (3)  NOT NULL,
    [LN_FOR_OCC_SEQ] INT          NOT NULL,
    CONSTRAINT [PK_FORBEARANCE_DELETE] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC, [LF_FOR_CTL_NUM] ASC, [LN_FOR_OCC_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Numerical sequence of forbearances for the loan', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_FB10_FORBEARANCE', @level2type = N'COLUMN', @level2name = N'LN_FOR_OCC_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Forbearance control number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_FB10_FORBEARANCE', @level2type = N'COLUMN', @level2name = N'LF_FOR_CTL_NUM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_FB10_FORBEARANCE', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_FB10_FORBEARANCE', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


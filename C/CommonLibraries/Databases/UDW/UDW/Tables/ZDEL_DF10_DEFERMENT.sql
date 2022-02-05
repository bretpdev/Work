CREATE TABLE [dbo].[ZDEL_DF10_DEFERMENT] (
    [DF_SPE_ACC_ID]  VARCHAR (10) NOT NULL,
    [LN_SEQ]         INT          NOT NULL,
    [LF_DFR_CTL_NUM] VARCHAR (3)  NOT NULL,
    [LN_DFR_OCC_SEQ] INT          NOT NULL,
    CONSTRAINT [PK_DEFERMENT_DELETE] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC, [LF_DFR_CTL_NUM] ASC, [LN_DFR_OCC_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_DF10_DEFERMENT', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_DF10_DEFERMENT', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Deferment control number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_DF10_DEFERMENT', @level2type = N'COLUMN', @level2name = N'LF_DFR_CTL_NUM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Numerical sequence for deferments for this loan', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_DF10_DEFERMENT', @level2type = N'COLUMN', @level2name = N'LN_DFR_OCC_SEQ';


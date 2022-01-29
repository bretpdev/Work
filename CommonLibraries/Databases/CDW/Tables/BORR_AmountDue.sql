CREATE TABLE [dbo].[BORR_AmountDue] (
    [DF_SPE_ACC_ID] VARCHAR (10)   NULL,
    [CUR_DUE]       DECIMAL (9, 2) NULL,
    [PAST_DUE]      DECIMAL (9, 2) NULL,
    [TOT_DUE]       DECIMAL (9, 2) NULL,
    [TOT_DUE_FEE]   DECIMAL (9, 2) NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [borr_bill]
    ON [dbo].[BORR_AmountDue]([DF_SPE_ACC_ID] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BORR_AmountDue', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Current amount due', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BORR_AmountDue', @level2type = N'COLUMN', @level2name = N'CUR_DUE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Amount past due', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BORR_AmountDue', @level2type = N'COLUMN', @level2name = N'PAST_DUE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Total amount due', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BORR_AmountDue', @level2type = N'COLUMN', @level2name = N'TOT_DUE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Total amount due + late fees', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BORR_AmountDue', @level2type = N'COLUMN', @level2name = N'TOT_DUE_FEE';


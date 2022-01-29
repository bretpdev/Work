CREATE TABLE [dbo].[BORR_Repayment] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL,
    [LD_CRT_LON65]  VARCHAR (10) CONSTRAINT [DF_BORR_Repayment_LD_CRT_LON65] DEFAULT (' ') NULL,
    [DUE_DAY]       VARCHAR (8)  NULL,
    [MONTH_AMT]     VARCHAR (20) NULL,
    [MULT_DUE_DT]   VARCHAR (1)  NULL,
    CONSTRAINT [PK_BORR_Repayment] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BORR_Repayment', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Repayment schedule create date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BORR_Repayment', @level2type = N'COLUMN', @level2name = N'LD_CRT_LON65';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Day of month that payments are due', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BORR_Repayment', @level2type = N'COLUMN', @level2name = N'DUE_DAY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Installment amount, grouped by due day', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BORR_Repayment', @level2type = N'COLUMN', @level2name = N'MONTH_AMT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indicator that the borrower has multiple due dates per month', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BORR_Repayment', @level2type = N'COLUMN', @level2name = N'MULT_DUE_DT';


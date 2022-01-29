CREATE TABLE [dbo].[RM31_Suspense] (
    [DF_SPE_ACC_ID] VARCHAR (10)   NOT NULL,
    [LA_BR_RMT_PST] DECIMAL (8, 2) NULL,
    CONSTRAINT [PK_Suspense] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RM31_Suspense', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sum of payments in suspense', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RM31_Suspense', @level2type = N'COLUMN', @level2name = N'LA_BR_RMT_PST';


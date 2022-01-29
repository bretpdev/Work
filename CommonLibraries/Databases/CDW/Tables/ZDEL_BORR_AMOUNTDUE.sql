CREATE TABLE [dbo].[ZDEL_BORR_AMOUNTDUE] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_BORR_AMOUNTDUE', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


CREATE TABLE [dbo].[ZDEL_RM31_SUSPENSE] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_SUSPENSE_DELETE] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower Account Number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_RM31_SUSPENSE', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


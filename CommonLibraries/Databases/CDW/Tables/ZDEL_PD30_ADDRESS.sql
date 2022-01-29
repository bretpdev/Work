CREATE TABLE [dbo].[ZDEL_PD30_ADDRESS] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_ADDRESS_DELETE] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower Account Number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_PD30_ADDRESS', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


CREATE TABLE [dbo].[BORR_Delinquency] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL,
    [LD_DLQ_OCC]    VARCHAR (10) CONSTRAINT [DF_BORR_Delinquency_LD_DLQ_OCC] DEFAULT (' ') NULL,
    [CUR_DLQ]       INT          NULL,
    CONSTRAINT [PK_BORR_Delinquency] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BORR_Delinquency', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Delinquency occurred date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BORR_Delinquency', @level2type = N'COLUMN', @level2name = N'LD_DLQ_OCC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrowers maximum active delinquency', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BORR_Delinquency', @level2type = N'COLUMN', @level2name = N'CUR_DLQ';


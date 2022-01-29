CREATE TABLE [dbo].[LN16_Delinquency] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL,
    [LN_SEQ]        INT          NOT NULL,
    [LD_DLQ_OCC]    VARCHAR (10) CONSTRAINT [DF_Delinquency_LD_DLQ_OCC] DEFAULT (' ') NULL,
    [LN_DLQ_MAX]    INT          CONSTRAINT [DF_Delinquency_LN_DLQ_MAX] DEFAULT ((0)) NULL,
    [LD_DLQ_MAX]    VARCHAR (10) NULL,
    CONSTRAINT [PK_Delinquency] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date of maximum delinquency', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN16_Delinquency', @level2type = N'COLUMN', @level2name = N'LD_DLQ_MAX';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Days delinquent', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN16_Delinquency', @level2type = N'COLUMN', @level2name = N'LN_DLQ_MAX';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Delinquency occurred date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN16_Delinquency', @level2type = N'COLUMN', @level2name = N'LD_DLQ_OCC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN16_Delinquency', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN16_Delinquency', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


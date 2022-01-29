CREATE TABLE [dbo].[ZDEL_LN56_CREDITREPORT] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL,
    [LN_SEQ]        INT          NOT NULL,
    [LD_RPT_CRB]    VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_CREDIT_REPORTING_DELETE] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC, [LD_RPT_CRB] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_LN56_CREDITREPORT', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_LN56_CREDITREPORT', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date borrower reported to credit bureau', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_LN56_CREDITREPORT', @level2type = N'COLUMN', @level2name = N'LD_RPT_CRB';


CREATE TABLE [dbo].[LN56_CreditReport] (
    [DF_SPE_ACC_ID]  VARCHAR (10) NOT NULL,
    [LN_SEQ]         INT          NOT NULL,
    [LD_RPT_CRB]     VARCHAR (10) NOT NULL,
    [LC_RPT_STA_CRB] VARCHAR (2)  NULL,
    [DT_ADJ]         VARCHAR (10) NULL,
    [RPT_STA_CRB]    VARCHAR (30) NULL,
    CONSTRAINT [PK_Credit_Reporting] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC, [LD_RPT_CRB] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN56_CreditReport', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN56_CreditReport', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date borrower reported to credit bureau', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN56_CreditReport', @level2type = N'COLUMN', @level2name = N'LD_RPT_CRB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Status code reported to the Credit Bureau', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN56_CreditReport', @level2type = N'COLUMN', @level2name = N'LC_RPT_STA_CRB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Credit reporting date adjusted', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN56_CreditReport', @level2type = N'COLUMN', @level2name = N'DT_ADJ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Status reported to the Credit Bureau', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN56_CreditReport', @level2type = N'COLUMN', @level2name = N'RPT_STA_CRB';


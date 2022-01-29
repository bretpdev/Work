CREATE TABLE [dbo].[DW01_Loan] (
    [DF_SPE_ACC_ID]  VARCHAR (10)    NOT NULL,
    [LN_SEQ]         INT             NOT NULL,
    [WC_DW_LON_STA]  VARCHAR (2)     CONSTRAINT [DF_Loan2_WC_DW_LON_STA] DEFAULT (' ') NULL,
    [DW_LON_STA]     VARCHAR (20)    CONSTRAINT [DF_Loan2_DW_LON_STA] DEFAULT (' ') NULL,
    [WD_LON_RPD_SR]  VARCHAR (10)    NULL,
    [WA_TOT_BRI_OTS] NUMERIC (13, 2) CONSTRAINT [DF_Loan2_WA_TOT_BRI_OTS] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Loan2] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DW01_Loan', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DW01_Loan', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan status code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DW01_Loan', @level2type = N'COLUMN', @level2name = N'WC_DW_LON_STA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan Status description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DW01_Loan', @level2type = N'COLUMN', @level2name = N'DW_LON_STA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Repayment start date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DW01_Loan', @level2type = N'COLUMN', @level2name = N'WD_LON_RPD_SR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Outstanding interest amount', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DW01_Loan', @level2type = N'COLUMN', @level2name = N'WA_TOT_BRI_OTS';


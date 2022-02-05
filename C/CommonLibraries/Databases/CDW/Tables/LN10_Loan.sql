CREATE TABLE [dbo].[LN10_Loan] (
    [DF_SPE_ACC_ID]  VARCHAR (10)   NOT NULL,
    [LN_SEQ]         INT            NOT NULL,
    [LA_CUR_PRI]     NUMERIC (9, 2) CONSTRAINT [DF_Loan_LA_CUR_PRI] DEFAULT ((0)) NULL,
    [LA_LON_AMT_GTR] NUMERIC (9, 2) CONSTRAINT [DF_Loan_LA_LON_AMT_GTR] DEFAULT ((0)) NULL,
    [LD_END_GRC_PRD] VARCHAR (10)   CONSTRAINT [DF_Loan_LD_END_GRC_PRD] DEFAULT (' ') NULL,
    [IC_LON_PGM]     VARCHAR (6)    CONSTRAINT [DF_Loan_IC_LON_PGM] DEFAULT (' ') NULL,
    [LD_LON_1_DSB]   VARCHAR (10)   CONSTRAINT [DF_Loan_LD_LON_1_DSB] DEFAULT (' ') NULL,
    [LD_PIF_RPT]     VARCHAR (10)   CONSTRAINT [DF_Loan_LD_PIF_RPT] DEFAULT (' ') NULL,
    [LC_SST_LON10]   VARCHAR (1)    NULL DEFAULT (' '),
    [FORWARDING]     VARCHAR (2)    NULL DEFAULT (' '),
    [LC_STA_LON10]   VARCHAR (1)    NULL DEFAULT (' '),
    [HEP]            VARCHAR (1)    CONSTRAINT [DF_LN10_Loan_HEP] DEFAULT (' ') NOT NULL,
    [LF_LON_CUR_OWN] VARCHAR(8) NULL DEFAULT (' '), 
    [LF_DOE_SCL_ORG] VARCHAR(8) NULL DEFAULT (' '), 
    CONSTRAINT [PK_Loan] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN10_Loan', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN10_Loan', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Current principal', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN10_Loan', @level2type = N'COLUMN', @level2name = N'LA_CUR_PRI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Amount guaranteed', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN10_Loan', @level2type = N'COLUMN', @level2name = N'LA_LON_AMT_GTR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Grace period end date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN10_Loan', @level2type = N'COLUMN', @level2name = N'LD_END_GRC_PRD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Current loan program', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN10_Loan', @level2type = N'COLUMN', @level2name = N'IC_LON_PGM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date of first disbursement for the loan', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN10_Loan', @level2type = N'COLUMN', @level2name = N'LD_LON_1_DSB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Paid in full report date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN10_Loan', @level2type = N'COLUMN', @level2name = N'LD_PIF_RPT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sub-status', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN10_Loan', @level2type = N'COLUMN', @level2name = N'LC_SST_LON10';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Forwarding code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN10_Loan', @level2type = N'COLUMN', @level2name = N'FORWARDING';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Status code for this record', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN10_Loan', @level2type = N'COLUMN', @level2name = N'LC_STA_LON10';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Original School Code',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LN10_Loan',
    @level2type = N'COLUMN',
    @level2name = N'LF_DOE_SCL_ORG'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Current Owner',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LN10_Loan',
    @level2type = N'COLUMN',
    @level2name = N'LF_LON_CUR_OWN'
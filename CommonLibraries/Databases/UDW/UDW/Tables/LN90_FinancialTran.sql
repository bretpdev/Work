CREATE TABLE [dbo].[LN90_FinancialTran] (
    [DF_SPE_ACC_ID]  VARCHAR (10)   NOT NULL,
    [LN_SEQ]         INT            NOT NULL,
    [LN_FAT_SEQ]     INT            NOT NULL,
    [LD_FAT_PST]     VARCHAR (10)   CONSTRAINT [DF_FinancialTran_LD_FAT_PST] DEFAULT (' ') NULL,
    [LD_FAT_EFF]     VARCHAR (10)   CONSTRAINT [DF_FinancialTran_LD_FAT_EFF] DEFAULT (' ') NULL,
    [LC_STA_LON90]   VARCHAR (1)    CONSTRAINT [DF_FinancialTran_LC_STA_LON90] DEFAULT (' ') NULL,
    [LA_FAT_LTE_FEE] NUMERIC (8, 2) CONSTRAINT [DF_FinancialTran_LA_FAT_LTE_FEE] DEFAULT ((0)) NULL,
    [LA_FAT_CUR_PRI] NUMERIC (9, 2) CONSTRAINT [DF_FinancialTran_LA_FAT_CUR_PRI] DEFAULT ((0)) NULL,
    [PC_FAT_TYP]     VARCHAR (2)    CONSTRAINT [DF_FinancialTran_PC_FAT_TYP] DEFAULT (' ') NULL,
    [PC_FAT_SUB_TYP] VARCHAR (2)    CONSTRAINT [DF_FinancialTran_PC_FAT_SUB_TYP] DEFAULT (' ') NULL,
    [LC_FAT_REV_REA] VARCHAR (1)    CONSTRAINT [DF_FinancialTran_LC_FAT_REV_REA] DEFAULT (' ') NULL,
    [LA_FAT_NSI]     NUMERIC (7, 2) CONSTRAINT [DF_FinancialTran_LA_FAT_NSI_ACR] DEFAULT ((0)) NULL,
    [FAT_REV_REA]    VARCHAR (37)   CONSTRAINT [DF_FinancialTran_FAT_REV_REA] DEFAULT (' ') NULL,
    [TRAN_AMT]       AS             (([LA_FAT_CUR_PRI]+[LA_FAT_NSI])+[LA_FAT_LTE_FEE]),
    CONSTRAINT [PK_FinancialTran] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC, [LN_FAT_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN90_FinancialTran', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN90_FinancialTran', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transaction sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN90_FinancialTran', @level2type = N'COLUMN', @level2name = N'LN_FAT_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transaction posted date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN90_FinancialTran', @level2type = N'COLUMN', @level2name = N'LD_FAT_PST';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transaction effective date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN90_FinancialTran', @level2type = N'COLUMN', @level2name = N'LD_FAT_EFF';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transaction status code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN90_FinancialTran', @level2type = N'COLUMN', @level2name = N'LC_STA_LON90';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Amount applied to late fees', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN90_FinancialTran', @level2type = N'COLUMN', @level2name = N'LA_FAT_LTE_FEE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Amount applied to principal', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN90_FinancialTran', @level2type = N'COLUMN', @level2name = N'LA_FAT_CUR_PRI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transaction type code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN90_FinancialTran', @level2type = N'COLUMN', @level2name = N'PC_FAT_TYP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transaction sub type code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN90_FinancialTran', @level2type = N'COLUMN', @level2name = N'PC_FAT_SUB_TYP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reversal reason code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN90_FinancialTran', @level2type = N'COLUMN', @level2name = N'LC_FAT_REV_REA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Non-subsidized interest accrual amount', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN90_FinancialTran', @level2type = N'COLUMN', @level2name = N'LA_FAT_NSI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reversal reason', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN90_FinancialTran', @level2type = N'COLUMN', @level2name = N'FAT_REV_REA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Total Transaction Amount', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN90_FinancialTran', @level2type = N'COLUMN', @level2name = N'TRAN_AMT';


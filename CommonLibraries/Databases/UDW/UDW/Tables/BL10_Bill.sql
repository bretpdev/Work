CREATE TABLE [dbo].[BL10_Bill] (
    [DF_SPE_ACC_ID]      VARCHAR (10)   NOT NULL,
    [LN_SEQ]             INT            NOT NULL,
    [LD_BIL_CRT]         VARCHAR (10)   NOT NULL,
    [LN_SEQ_BIL_WI_DTE]  INT            NOT NULL,
    [LD_BIL_DU_LON]      VARCHAR (10)   CONSTRAINT [DF_Bill_LD_BIL_DU_LON] DEFAULT (' ') NULL,
    [BIL_MTD]            VARCHAR (12)   CONSTRAINT [DF_Bill_BIL_MTD] DEFAULT (' ') NULL,
    [LC_BIL_MTD]         VARCHAR (1)    CONSTRAINT [DF_Bill_LC_BIL_MTD] DEFAULT (' ') NULL,
    [LC_IND_BIL_SNT]     VARCHAR (1)    CONSTRAINT [DF_Bill_LC_IND_BIL_SNT] DEFAULT (' ') NULL,
    [LC_STA_LON80]       VARCHAR (1)    CONSTRAINT [DF_Bill_LC_STA_LON80] DEFAULT (' ') NULL,
    [LC_STA_BIL10]       VARCHAR (1)    CONSTRAINT [DF_Bill_LC_STA_BIL10] DEFAULT (' ') NULL,
    [LD_BIL_STS_RIR_TOL] VARCHAR (10)   CONSTRAINT [DF_Bill_LD_BIL_STS_RIR_TOL] DEFAULT (' ') NULL,
    [BIL_SAT]            VARCHAR (1)    CONSTRAINT [DF_Bill_BIL_SAT] DEFAULT (' ') NULL,
    [LA_BIL_CUR_DU]      NUMERIC (8, 2) CONSTRAINT [DF_Bill_LA_BIL_CUR_DU] DEFAULT ((0)) NULL,
    [LA_BIL_PAS_DU]      NUMERIC (8, 2) CONSTRAINT [DF_Bill_LA_BIL_PAS_DU] DEFAULT ((0)) NULL,
    [LA_TOT_BIL_STS]     NUMERIC (8, 2) CONSTRAINT [DF_Bill_LA_TOT_BIL_STS] DEFAULT ((0)) NULL,
    [PAID_AHEAD]         VARCHAR (1)    CONSTRAINT [DF_Bill_PAID_AHEAD] DEFAULT (' ') NULL,
    CONSTRAINT [PK_Bill] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC, [LD_BIL_CRT] ASC, [LN_SEQ_BIL_WI_DTE] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BL10_Bill', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BL10_Bill', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bill create date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BL10_Bill', @level2type = N'COLUMN', @level2name = N'LD_BIL_CRT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence bill within date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BL10_Bill', @level2type = N'COLUMN', @level2name = N'LN_SEQ_BIL_WI_DTE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date bill due', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BL10_Bill', @level2type = N'COLUMN', @level2name = N'LD_BIL_DU_LON';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bill method', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BL10_Bill', @level2type = N'COLUMN', @level2name = N'BIL_MTD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bill method code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BL10_Bill', @level2type = N'COLUMN', @level2name = N'LC_BIL_MTD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bill sent indicator code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BL10_Bill', @level2type = N'COLUMN', @level2name = N'LC_IND_BIL_SNT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Status code of loan bill record', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BL10_Bill', @level2type = N'COLUMN', @level2name = N'LC_STA_LON80';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bill status code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BL10_Bill', @level2type = N'COLUMN', @level2name = N'LC_STA_BIL10';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bill satisfied date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BL10_Bill', @level2type = N'COLUMN', @level2name = N'LD_BIL_STS_RIR_TOL';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bill satisfied indicator', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BL10_Bill', @level2type = N'COLUMN', @level2name = N'BIL_SAT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Amount billed currently due', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BL10_Bill', @level2type = N'COLUMN', @level2name = N'LA_BIL_CUR_DU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Amount billed past due', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BL10_Bill', @level2type = N'COLUMN', @level2name = N'LA_BIL_PAS_DU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Amount of total bill satisified', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BL10_Bill', @level2type = N'COLUMN', @level2name = N'LA_TOT_BIL_STS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Paid ahead indicator', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BL10_Bill', @level2type = N'COLUMN', @level2name = N'PAID_AHEAD';


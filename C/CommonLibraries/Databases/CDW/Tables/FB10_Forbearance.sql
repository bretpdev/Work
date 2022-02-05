CREATE TABLE [dbo].[FB10_Forbearance] (
    [DF_SPE_ACC_ID]      VARCHAR (10)   NOT NULL,
    [LN_SEQ]             INT            NOT NULL,
    [LF_FOR_CTL_NUM]     VARCHAR (3)    NOT NULL,
    [LN_FOR_OCC_SEQ]     INT            NOT NULL,
    [LC_FOR_TYP]         VARCHAR (2)    CONSTRAINT [DF_Forbearance_LC_FOR_TYP] DEFAULT (' ') NULL,
    [FOR_TYP]            VARCHAR (36)   CONSTRAINT [DF_Forbearance_FOR_TYP] DEFAULT (' ') NULL,
    [LD_FOR_INF_CER]     VARCHAR (10)   CONSTRAINT [DF_Forbearance_LD_FOR_INF_CER] DEFAULT (' ') NULL,
    [LD_FOR_BEG]         VARCHAR (10)   CONSTRAINT [DF_Forbearance_LD_FOR_BEG] DEFAULT (' ') NULL,
    [LD_FOR_END]         VARCHAR (10)   CONSTRAINT [DF_Forbearance_LD_FOR_END] DEFAULT (' ') NULL,
    [LI_CAP_FOR_INT_REQ] VARCHAR (1)    CONSTRAINT [DF_Forbearance_LI_CAP_FOR_INT_REQ] DEFAULT (' ') NULL,
    [LC_STA_LON60]       VARCHAR (1)    CONSTRAINT [DF_Forbearance_LC_STA_LON60] DEFAULT (' ') NULL,
    [LC_FOR_STA]         VARCHAR (1)    CONSTRAINT [DF_Forbearance_LC_FOR_STA] DEFAULT (' ') NULL,
    [LC_STA_FOR10]       VARCHAR (1)    CONSTRAINT [DF_Forbearance_LC_STA_FOR10] DEFAULT (' ') NULL,
    [MONTHS_USED]        NUMERIC (4, 1) CONSTRAINT [DF_Forbearance_MONTHS_USED] DEFAULT ((0)) NULL,
    [LA_REQ_RDC_PAY]     NUMERIC (9, 2) CONSTRAINT [DF_Forbearance_LA_REQ_RDC_PAY] DEFAULT ((0)) NULL,
    [LI_FOR_VRB_DFL_RUL] CHAR NULL DEFAULT (' '), 
    CONSTRAINT [PK_Forbearance] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC, [LF_FOR_CTL_NUM] ASC, [LN_FOR_OCC_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FB10_Forbearance', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FB10_Forbearance', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Forbearance control number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FB10_Forbearance', @level2type = N'COLUMN', @level2name = N'LF_FOR_CTL_NUM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Numerical sequence of forbearances for the loan', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FB10_Forbearance', @level2type = N'COLUMN', @level2name = N'LN_FOR_OCC_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Forbearance type code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FB10_Forbearance', @level2type = N'COLUMN', @level2name = N'LC_FOR_TYP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Forbearance type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FB10_Forbearance', @level2type = N'COLUMN', @level2name = N'FOR_TYP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date forbearance information was certified', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FB10_Forbearance', @level2type = N'COLUMN', @level2name = N'LD_FOR_INF_CER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Forbearance begin date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FB10_Forbearance', @level2type = N'COLUMN', @level2name = N'LD_FOR_BEG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Forbearance end date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FB10_Forbearance', @level2type = N'COLUMN', @level2name = N'LD_FOR_END';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Request indicator for forbearance interest to be capped', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FB10_Forbearance', @level2type = N'COLUMN', @level2name = N'LI_CAP_FOR_INT_REQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Forbearance approval status code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FB10_Forbearance', @level2type = N'COLUMN', @level2name = N'LC_STA_LON60';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Forbearance request status code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FB10_Forbearance', @level2type = N'COLUMN', @level2name = N'LC_FOR_STA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Code forbearance request status', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FB10_Forbearance', @level2type = N'COLUMN', @level2name = N'LC_STA_FOR10';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Forbearance duration in months', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FB10_Forbearance', @level2type = N'COLUMN', @level2name = N'MONTHS_USED';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Requested amount in payment reduction', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FB10_Forbearance', @level2type = N'COLUMN', @level2name = N'LA_REQ_RDC_PAY';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Indicates whether the forbearance was approved under the Verbal/Default rules',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FB10_Forbearance',
    @level2type = N'COLUMN',
    @level2name = N'LI_FOR_VRB_DFL_RUL'
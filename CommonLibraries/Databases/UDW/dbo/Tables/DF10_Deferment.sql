CREATE TABLE [dbo].[DF10_Deferment] (
    [DF_SPE_ACC_ID]      VARCHAR (10)   NOT NULL,
    [LN_SEQ]             INT            NOT NULL,
    [LF_DFR_CTL_NUM]     VARCHAR (3)    NOT NULL,
    [LN_DFR_OCC_SEQ]     INT            NOT NULL,
    [LC_DFR_TYP]         VARCHAR (2)    CONSTRAINT [DF_Deferment_LC_DFR_TYP] DEFAULT (' ') NULL,
    [DFR_TYP]            VARCHAR (39)   CONSTRAINT [DF_Deferment_DFR_TYP] DEFAULT (' ') NULL,
    [LD_DFR_INF_CER]     VARCHAR (10)   CONSTRAINT [DF_Deferment_LD_DFR_INF_CER] DEFAULT (' ') NULL,
    [LD_DFR_BEG]         VARCHAR (10)   CONSTRAINT [DF_Deferment_LD_DFR_BEG] DEFAULT (' ') NULL,
    [LD_DFR_END]         VARCHAR (10)   CONSTRAINT [DF_Deferment_LD_DFR_END] DEFAULT (' ') NULL,
    [LC_LON_LEV_DFR_CAP] VARCHAR (1)    CONSTRAINT [DF_Table_1_LI_CAP_DFR_INT_REQ] DEFAULT (' ') NULL,
    [LC_STA_LON50]       VARCHAR (1)    CONSTRAINT [DF_Deferment_LC_STA_LON50] DEFAULT (' ') NULL,
    [LC_DFR_STA]         VARCHAR (1)    CONSTRAINT [DF_Deferment_LC_DFR_STA] DEFAULT (' ') NULL,
    [LC_STA_DFR10]       VARCHAR (1)    CONSTRAINT [DF_Deferment_LC_STA_DFR10] DEFAULT (' ') NULL,
    [MONTHS_USED]        NUMERIC (4, 1) CONSTRAINT [DF_Deferment_MONTHS_USED] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Deferment] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC, [LF_DFR_CTL_NUM] ASC, [LN_DFR_OCC_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Deferment duration in months rounded to .1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DF10_Deferment', @level2type = N'COLUMN', @level2name = N'MONTHS_USED';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Code deferment request status', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DF10_Deferment', @level2type = N'COLUMN', @level2name = N'LC_STA_DFR10';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Deferment request status code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DF10_Deferment', @level2type = N'COLUMN', @level2name = N'LC_DFR_STA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Deferment approval status code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DF10_Deferment', @level2type = N'COLUMN', @level2name = N'LC_STA_LON50';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan level deferment cap code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DF10_Deferment', @level2type = N'COLUMN', @level2name = N'LC_LON_LEV_DFR_CAP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Deferment end date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DF10_Deferment', @level2type = N'COLUMN', @level2name = N'LD_DFR_END';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Deferment begin date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DF10_Deferment', @level2type = N'COLUMN', @level2name = N'LD_DFR_BEG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date the deferment information is certified', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DF10_Deferment', @level2type = N'COLUMN', @level2name = N'LD_DFR_INF_CER';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Deferment type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DF10_Deferment', @level2type = N'COLUMN', @level2name = N'DFR_TYP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Deferment type code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DF10_Deferment', @level2type = N'COLUMN', @level2name = N'LC_DFR_TYP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Numerical sequence for deferments for this loan', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DF10_Deferment', @level2type = N'COLUMN', @level2name = N'LN_DFR_OCC_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Deferment control number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DF10_Deferment', @level2type = N'COLUMN', @level2name = N'LF_DFR_CTL_NUM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DF10_Deferment', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DF10_Deferment', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


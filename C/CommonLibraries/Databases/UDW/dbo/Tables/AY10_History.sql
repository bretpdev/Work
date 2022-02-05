CREATE TABLE [dbo].[AY10_History] (
    [DF_SPE_ACC_ID]  VARCHAR (10)  NOT NULL,
    [LN_ATY_SEQ]     INT           NOT NULL,
    [LC_STA_ACTY10]  VARCHAR (1)   NULL,
    [PF_REQ_ACT]     VARCHAR (5)   NULL,
    [PF_RSP_ACT]     VARCHAR (5)   NULL,
    [PX_ACT_DSC_REQ] VARCHAR (10)  NULL,
    [LD_ATY_REQ_RCV] VARCHAR (10)  NULL,
    [LD_ATY_RSP]     VARCHAR (10)  NULL,
    [LF_USR_REQ_ATY] VARCHAR (8)   NULL,
    [LT_ATY_RSP]     VARCHAR (8)   NULL,
    [LX_ATY]         VARCHAR (360) NULL,
    CONSTRAINT [PK_Activity_History] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_ATY_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Activity text', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_History', @level2type = N'COLUMN', @level2name = N'LX_ATY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Time activity response was performed', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_History', @level2type = N'COLUMN', @level2name = N'LT_ATY_RSP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'User who requested activity', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_History', @level2type = N'COLUMN', @level2name = N'LF_USR_REQ_ATY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Activity response date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_History', @level2type = N'COLUMN', @level2name = N'LD_ATY_RSP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Activity request received date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_History', @level2type = N'COLUMN', @level2name = N'LD_ATY_REQ_RCV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Action code description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_History', @level2type = N'COLUMN', @level2name = N'PX_ACT_DSC_REQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Action response code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_History', @level2type = N'COLUMN', @level2name = N'PF_RSP_ACT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Action request code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_History', @level2type = N'COLUMN', @level2name = N'PF_REQ_ACT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Activity status code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_History', @level2type = N'COLUMN', @level2name = N'LC_STA_ACTY10';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Activity sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_History', @level2type = N'COLUMN', @level2name = N'LN_ATY_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_History', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


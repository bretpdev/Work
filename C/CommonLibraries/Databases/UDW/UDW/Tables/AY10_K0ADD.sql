CREATE TABLE [dbo].[AY10_K0ADD] (
    [DF_SPE_ACC_ID] VARCHAR (10)  NOT NULL,
    [LN_ATY_SEQ]    INT           NOT NULL,
    [LC_STA_ACTY10] VARCHAR (1)   NULL,
    [DX_STR_ADR_1]  VARCHAR (30)  NULL,
    [DX_STR_ADR_2]  VARCHAR (30)  NULL,
    [DM_CT]         VARCHAR (20)  NULL,
    [DC_DOM_ST]     VARCHAR (2)   NULL,
    [DF_ZIP_CDE]    VARCHAR (17)  NULL,
    [DM_FGN_CNY]    VARCHAR (25)  NULL,
    [COMMENTS]      VARCHAR (300) NULL,
    CONSTRAINT [PK_AY10_K0ADD] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_ATY_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_K0ADD', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Activity sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_K0ADD', @level2type = N'COLUMN', @level2name = N'LN_ATY_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Activity status code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_K0ADD', @level2type = N'COLUMN', @level2name = N'LC_STA_ACTY10';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Street address line 1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_K0ADD', @level2type = N'COLUMN', @level2name = N'DX_STR_ADR_1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Street address line 2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_K0ADD', @level2type = N'COLUMN', @level2name = N'DX_STR_ADR_2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'City', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_K0ADD', @level2type = N'COLUMN', @level2name = N'DM_CT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'State code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_K0ADD', @level2type = N'COLUMN', @level2name = N'DC_DOM_ST';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Zip code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_K0ADD', @level2type = N'COLUMN', @level2name = N'DF_ZIP_CDE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Foreign country address', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_K0ADD', @level2type = N'COLUMN', @level2name = N'DM_FGN_CNY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Comments', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_K0ADD', @level2type = N'COLUMN', @level2name = N'COMMENTS';


CREATE TABLE [dbo].[PD30_Address] (
    [DF_SPE_ACC_ID]  VARCHAR (10) NOT NULL,
    [DX_STR_ADR_1]   VARCHAR (30) CONSTRAINT [DF_PD30_Address_DX_STR_ADR_1] DEFAULT (' ') NULL,
    [DX_STR_ADR_2]   VARCHAR (30) CONSTRAINT [DF_PD30_Address_DX_STR_ADR_2] DEFAULT (' ') NULL,
    [DM_CT]          VARCHAR (20) CONSTRAINT [DF_PD30_Address_DM_CT] DEFAULT (' ') NULL,
    [DC_DOM_ST]      VARCHAR (2)  CONSTRAINT [DF_PD30_Address_DC_DOM_ST] DEFAULT (' ') NULL,
    [DF_ZIP_CDE]     VARCHAR (17) CONSTRAINT [DF_PD30_Address_DF_ZIP_CDE] DEFAULT (' ') NULL,
    [DM_FGN_ST]      VARCHAR (15) CONSTRAINT [DF_PD30_Address_DM_FGN_ST] DEFAULT (' ') NULL,
    [DM_FGN_CNY]     VARCHAR (25) CONSTRAINT [DF_PD30_Address_DM_FGN_CNY] DEFAULT (' ') NULL,
    [DD_VER_ADR]     VARCHAR (10) CONSTRAINT [DF_PD30_Address_DD_VER_ADR] DEFAULT (' ') NULL,
    [DI_VLD_ADR]     VARCHAR (1)  CONSTRAINT [DF_PD30_Address_DI_VLD_ADR] DEFAULT (' ') NULL,
    [DF_ZIP_CDE_IVR] AS           (substring([DF_ZIP_CDE],(1),(5))),
    CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD30_Address', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Street address line 1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD30_Address', @level2type = N'COLUMN', @level2name = N'DX_STR_ADR_1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Street address line 2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD30_Address', @level2type = N'COLUMN', @level2name = N'DX_STR_ADR_2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'City', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD30_Address', @level2type = N'COLUMN', @level2name = N'DM_CT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'State code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD30_Address', @level2type = N'COLUMN', @level2name = N'DC_DOM_ST';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Zip code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD30_Address', @level2type = N'COLUMN', @level2name = N'DF_ZIP_CDE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Foreign country address', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD30_Address', @level2type = N'COLUMN', @level2name = N'DM_FGN_CNY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date address verified', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD30_Address', @level2type = N'COLUMN', @level2name = N'DD_VER_ADR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Valid address indicator', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD30_Address', @level2type = N'COLUMN', @level2name = N'DI_VLD_ADR';


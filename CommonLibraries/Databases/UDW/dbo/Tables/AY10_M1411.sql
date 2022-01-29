CREATE TABLE [dbo].[AY10_M1411] (
    [DF_SPE_ACC_ID] VARCHAR (10)  NOT NULL,
    [LN_ATY_SEQ]    INT           NOT NULL,
    [LC_STA_ACTY10] VARCHAR (1)   NULL,
    [LX_ATY]        VARCHAR (360) NULL,
    CONSTRAINT [PK_AY01_M1411_1] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_ATY_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Activity text', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_M1411', @level2type = N'COLUMN', @level2name = N'LX_ATY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Activity status code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_M1411', @level2type = N'COLUMN', @level2name = N'LC_STA_ACTY10';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Activity sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_M1411', @level2type = N'COLUMN', @level2name = N'LN_ATY_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_M1411', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


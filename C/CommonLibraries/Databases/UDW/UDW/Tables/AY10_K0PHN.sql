CREATE TABLE [dbo].[AY10_K0PHN] (
    [DF_SPE_ACC_ID] VARCHAR (10)  NOT NULL,
    [LN_ATY_SEQ]    INT           NOT NULL,
    [LC_STA_ACTY10] VARCHAR (1)   NULL,
    [PHN1]          VARCHAR (20)  NULL,
    [PHN2]          VARCHAR (20)  NULL,
    [PHN3]          VARCHAR (20)  NULL,
    [COMMENTS]      VARCHAR (300) NULL,
    CONSTRAINT [PK_AY10_K0PHN] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_ATY_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_K0PHN', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Activity sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_K0PHN', @level2type = N'COLUMN', @level2name = N'LN_ATY_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Activity status code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_K0PHN', @level2type = N'COLUMN', @level2name = N'LC_STA_ACTY10';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Phone 1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_K0PHN', @level2type = N'COLUMN', @level2name = N'PHN1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Phone 2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_K0PHN', @level2type = N'COLUMN', @level2name = N'PHN2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Phone 3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_K0PHN', @level2type = N'COLUMN', @level2name = N'PHN3';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Comments', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_K0PHN', @level2type = N'COLUMN', @level2name = N'COMMENTS';


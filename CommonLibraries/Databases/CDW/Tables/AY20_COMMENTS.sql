CREATE TABLE [dbo].[AY20_COMMENTS] (
    [DF_SPE_ACC_ID]  VARCHAR (10) NOT NULL,
    [LN_ATY_SEQ]     INT          NOT NULL,
    [LN_ATY_CMT_SEQ] INT          NOT NULL,
    [LN_ATY_TXT_SEQ] INT          NOT NULL,
    [LX_ATY]         NCHAR (10)   NULL,
    CONSTRAINT [PK_AY20_COMMENTS] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_ATY_SEQ] ASC, [LN_ATY_CMT_SEQ] ASC, [LN_ATY_TXT_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY20_COMMENTS', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Activity sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY20_COMMENTS', @level2type = N'COLUMN', @level2name = N'LN_ATY_SEQ';


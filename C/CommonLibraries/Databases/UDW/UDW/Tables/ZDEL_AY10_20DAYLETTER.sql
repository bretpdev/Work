CREATE TABLE [dbo].[ZDEL_AY10_20DAYLETTER] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL,
    [LN_ATY_SEQ]    INT          NOT NULL,
    CONSTRAINT [PK_DL200_LETTERS_DELETE] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_ATY_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower Account Number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_AY10_20DAYLETTER', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Activity Sequence Number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_AY10_20DAYLETTER', @level2type = N'COLUMN', @level2name = N'LN_ATY_SEQ';


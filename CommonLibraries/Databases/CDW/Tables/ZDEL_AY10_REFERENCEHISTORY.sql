CREATE TABLE [dbo].[ZDEL_AY10_REFERENCEHISTORY] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL,
    [LF_ATY_RCP]    VARCHAR (9)  NOT NULL,
    [LN_ATY_SEQ]    INT          NOT NULL,
    CONSTRAINT [PK_ZDEL_AY10_REFERENCEHISTORY] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LF_ATY_RCP] ASC, [LN_ATY_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_AY10_REFERENCEHISTORY', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Activity recipient', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_AY10_REFERENCEHISTORY', @level2type = N'COLUMN', @level2name = N'LF_ATY_RCP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Activity sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_AY10_REFERENCEHISTORY', @level2type = N'COLUMN', @level2name = N'LN_ATY_SEQ';


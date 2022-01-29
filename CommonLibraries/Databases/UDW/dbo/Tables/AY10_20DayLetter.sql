CREATE TABLE [dbo].[AY10_20DayLetter] (
    [DF_SPE_ACC_ID]  VARCHAR (10) NOT NULL,
    [LN_ATY_SEQ]     INT          NOT NULL,
    [LC_STA_ACTY10]  VARCHAR (1)  NULL,
    [LD_ATY_REQ_RCV] VARCHAR (10) NULL,
    CONSTRAINT [PK_DL200_Letters] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_ATY_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Activity Request Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_20DayLetter', @level2type = N'COLUMN', @level2name = N'LD_ATY_REQ_RCV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Activity Status Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_20DayLetter', @level2type = N'COLUMN', @level2name = N'LC_STA_ACTY10';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Activity Sequence Number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_20DayLetter', @level2type = N'COLUMN', @level2name = N'LN_ATY_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower Account Number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AY10_20DayLetter', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


CREATE TABLE [dbo].[ZDEL_GA10_CALLFORWARDING] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL,
    [CLUID]         VARCHAR (19) CONSTRAINT [DF_CALL_FORWARDING_DELETE_CLUID] DEFAULT (' ') NULL,
    [LN_SEQ]        INT          CONSTRAINT [DF_CALL_FORWARDING_DELETE_LN_SEQ] DEFAULT ((0)) NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_GA10_CALLFORWARDING', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique identification of the loan', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_GA10_CALLFORWARDING', @level2type = N'COLUMN', @level2name = N'CLUID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ZDEL_GA10_CALLFORWARDING', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


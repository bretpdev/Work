CREATE TABLE [dbo].[GA10_CallForwarding] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL,
    [BF_SSN]        VARCHAR (9)  NOT NULL,
    [CLUID]         VARCHAR (19) CONSTRAINT [DF_Call_forwarding_CLUID] DEFAULT (' ') NULL,
    [LN_SEQ]        INT          CONSTRAINT [DF_Call_forwarding_LN_SEQ] DEFAULT ((0)) NULL,
    [FORWARDING]    VARCHAR (2)  CONSTRAINT [DF_Call_forwarding_FORWARDING] DEFAULT (' ') NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [FORWARDING]
    ON [dbo].[GA10_CallForwarding]([DF_SPE_ACC_ID] ASC, [CLUID] ASC, [LN_SEQ] ASC);


GO
CREATE NONCLUSTERED INDEX [SSN]
    ON [dbo].[GA10_CallForwarding]([BF_SSN] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GA10_CallForwarding', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower SSN', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GA10_CallForwarding', @level2type = N'COLUMN', @level2name = N'BF_SSN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique identification of the loan', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GA10_CallForwarding', @level2type = N'COLUMN', @level2name = N'CLUID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GA10_CallForwarding', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Call forwarding code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GA10_CallForwarding', @level2type = N'COLUMN', @level2name = N'FORWARDING';


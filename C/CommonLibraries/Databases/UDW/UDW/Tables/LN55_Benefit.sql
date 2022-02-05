CREATE TABLE [dbo].[LN55_Benefit] (
    [DF_SPE_ACC_ID]  VARCHAR (10) NOT NULL,
    [LN_SEQ]         INT          NOT NULL,
    [LN_LON_BBT_PAY] INT          NULL,
    [PN_BBT_PAY_ICV] INT          NULL,
    [RIR_CT]         VARCHAR (8)  NULL,
    CONSTRAINT [PK_Borrower_Benefit] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN55_Benefit', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN55_Benefit', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Number of payments toward borrower benefit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN55_Benefit', @level2type = N'COLUMN', @level2name = N'LN_LON_BBT_PAY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower benefit program tier number of payments', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN55_Benefit', @level2type = N'COLUMN', @level2name = N'PN_BBT_PAY_ICV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Payments made out of payments required for borrower benefit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN55_Benefit', @level2type = N'COLUMN', @level2name = N'RIR_CT';


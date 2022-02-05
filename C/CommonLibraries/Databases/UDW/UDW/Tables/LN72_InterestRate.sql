CREATE TABLE [dbo].[LN72_InterestRate] (
    [DF_SPE_ACC_ID]      VARCHAR (10)   NOT NULL,
    [LN_SEQ]             INT            NOT NULL,
    [LR_ITR]             NUMERIC (6, 3) CONSTRAINT [DF_Int_Rate_LR_ITR] DEFAULT ((0)) NULL,
    [LR_INT_RDC_PGM_ORG] NUMERIC (6, 3) CONSTRAINT [DF_Int_Rate_LR_INT_RDC_PGM_ORG] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Int_Rate] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN72_InterestRate', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN72_InterestRate', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Current interest rate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN72_InterestRate', @level2type = N'COLUMN', @level2name = N'LR_ITR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Statutory rate of the loan', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN72_InterestRate', @level2type = N'COLUMN', @level2name = N'LR_INT_RDC_PGM_ORG';


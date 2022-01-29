CREATE TABLE [dbo].[LN09_Rehabilitation] (
    [DF_SPE_ACC_ID]  VARCHAR (10) NOT NULL,
    [LN_SEQ]         INT          NOT NULL,
    [LD_LON_RHB_PCV] VARCHAR (10) NULL,
    CONSTRAINT [PK_Rehabilitation] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN09_Rehabilitation', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN09_Rehabilitation', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Rehabilitation date prior to conversion', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN09_Rehabilitation', @level2type = N'COLUMN', @level2name = N'LD_LON_RHB_PCV';


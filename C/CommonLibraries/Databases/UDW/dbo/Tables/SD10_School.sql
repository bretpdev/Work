CREATE TABLE [dbo].[SD10_School] (
    [DF_SPE_ACC_ID]   VARCHAR (10) NOT NULL,
    [LN_SEQ]          INT          NOT NULL,
    [LD_SCL_SPR]      VARCHAR (10) CONSTRAINT [DF_School_LD_SCL_SPR] DEFAULT (' ') NULL,
    [DOE_SCL_ENR_CUR] VARCHAR (40) CONSTRAINT [DF_School_DOE_SCL_ENR_CUR] DEFAULT (' ') NULL,
    CONSTRAINT [PK_School] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Current school name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SD10_School', @level2type = N'COLUMN', @level2name = N'DOE_SCL_ENR_CUR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'School separation date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SD10_School', @level2type = N'COLUMN', @level2name = N'LD_SCL_SPR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SD10_School', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SD10_School', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


CREATE TABLE [dbo].[FS14_LoanSubsidy]
(
	[DF_SPE_ACC_ID] CHAR(10) NOT NULL , 
    [LN_SEQ] INT NOT NULL, 
    [LN_INC_SUB_EVT_SEQ] INT NOT NULL, 
    [LD_INC_SUB_EFF_BEG] DATETIME NULL, 
    [LD_INC_SUB_EFF_END] DATETIME NULL, 
    [LC_INC_SUB_STA] CHAR NULL, 
    [LR_SUB_RMN] MONEY NULL, 
    [LF_LST_USR_FS14] VARCHAR(12) NULL, 
    [LF_LST_DTS_FS14] DATETIME NULL, 
    [LF_CRT_USR_FS14] VARCHAR(12) NULL, 
    [LD_CRT_FS14] DATETIME NULL, 
    [LC_STA_FS14] CHAR NULL, 
    [LD_STA_FS14] DATETIME NULL, 
    PRIMARY KEY ([DF_SPE_ACC_ID], [LN_SEQ], [LN_INC_SUB_EVT_SEQ])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Account Number ',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FS14_LoanSubsidy',
    @level2type = N'COLUMN',
    @level2name = N'DF_SPE_ACC_ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Loan Sequence',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FS14_LoanSubsidy',
    @level2type = N'COLUMN',
    @level2name = N'LN_SEQ'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Incoming Subsidy Event Sequence',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FS14_LoanSubsidy',
    @level2type = N'COLUMN',
    @level2name = N'LN_INC_SUB_EVT_SEQ'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Incoming Subsidy Effective Begin Date',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FS14_LoanSubsidy',
    @level2type = N'COLUMN',
    @level2name = N'LD_INC_SUB_EFF_BEG'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Incoming Subsidy Effective End Date',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FS14_LoanSubsidy',
    @level2type = N'COLUMN',
    @level2name = N'LD_INC_SUB_EFF_END'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Incoming Subsidy Status',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FS14_LoanSubsidy',
    @level2type = N'COLUMN',
    @level2name = N'LC_INC_SUB_STA'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Subsidy Remaining Rate',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FS14_LoanSubsidy',
    @level2type = N'COLUMN',
    @level2name = N'LR_SUB_RMN'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Last Update User ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FS14_LoanSubsidy',
    @level2type = N'COLUMN',
    @level2name = N'LF_LST_USR_FS14'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Last Updated TimeStamp',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FS14_LoanSubsidy',
    @level2type = N'COLUMN',
    @level2name = N'LF_LST_DTS_FS14'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Create user for this row',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FS14_LoanSubsidy',
    @level2type = N'COLUMN',
    @level2name = N'LF_CRT_USR_FS14'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Date the FS14 record was created',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FS14_LoanSubsidy',
    @level2type = N'COLUMN',
    @level2name = N'LD_CRT_FS14'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'FS14 record status code',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FS14_LoanSubsidy',
    @level2type = N'COLUMN',
    @level2name = N'LC_STA_FS14'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'FS14 record status date',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FS14_LoanSubsidy',
    @level2type = N'COLUMN',
    @level2name = N'LD_STA_FS14'
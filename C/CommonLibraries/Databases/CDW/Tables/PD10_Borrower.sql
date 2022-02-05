CREATE TABLE [dbo].[PD10_Borrower] (
    [DF_SPE_ACC_ID]     VARCHAR (50) NOT NULL,
    [BF_SSN]            VARCHAR (9)  NOT NULL,
    [DM_PRS_1]          VARCHAR (13) NULL,
    [DM_PRS_LST]        VARCHAR (23) NULL,
    [DM_PRS_MID]        VARCHAR (13) NULL,
    [DD_BRT]            VARCHAR (10) NULL,
    [DD_BRT_IVR]        AS           ((substring([DD_BRT],(7),(4))+substring([DD_BRT],(1),(2)))+substring([DD_BRT],(4),(2))),
    [AccountHolderName] AS           ((rtrim(([dm_prs_1]+' ')+[dm_prs_mid])+' ')+[dm_prs_lst]),
    CONSTRAINT [PK_Borrower] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower Account Number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD10_Borrower', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower SSN', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD10_Borrower', @level2type = N'COLUMN', @level2name = N'BF_SSN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'First name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD10_Borrower', @level2type = N'COLUMN', @level2name = N'DM_PRS_1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Last name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD10_Borrower', @level2type = N'COLUMN', @level2name = N'DM_PRS_LST';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Middle Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD10_Borrower', @level2type = N'COLUMN', @level2name = N'DM_PRS_MID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BirthDay(MM/DD/YYYY)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PD10_Borrower', @level2type = N'COLUMN', @level2name = N'DD_BRT';


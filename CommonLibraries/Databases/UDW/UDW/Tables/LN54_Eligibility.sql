﻿CREATE TABLE [dbo].[LN54_Eligibility] (
    [DF_SPE_ACC_ID] VARCHAR (10) NOT NULL,
    [LN_SEQ]        INT          NOT NULL,
    [LD_BBS_DSQ]    VARCHAR (10) NULL,
    [LC_BBS_ELG]    VARCHAR (1)  NULL,
    CONSTRAINT [PK_BB_Eligibility] PRIMARY KEY CLUSTERED ([DF_SPE_ACC_ID] ASC, [LN_SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower account number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN54_Eligibility', @level2type = N'COLUMN', @level2name = N'DF_SPE_ACC_ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loan sequence number', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN54_Eligibility', @level2type = N'COLUMN', @level2name = N'LN_SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Date loan disqualified from the borrower benefit program', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN54_Eligibility', @level2type = N'COLUMN', @level2name = N'LD_BBS_DSQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Borrower benefit system program eligibility code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LN54_Eligibility', @level2type = N'COLUMN', @level2name = N'LC_BBS_ELG';


CREATE TABLE [dbo].[Prereleased_Accounts] (
    [prereleased_account_id] INT          IDENTITY (1, 1) NOT NULL,
    [DF_SPE_ACC_ID]          VARCHAR (20) NOT NULL,
    [BF_SSN]                 CHAR (9)     NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_Account]
    ON [dbo].[Prereleased_Accounts]([DF_SPE_ACC_ID] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Account_SSN]
    ON [dbo].[Prereleased_Accounts]([DF_SPE_ACC_ID] ASC, [BF_SSN] ASC);


GO
CREATE CLUSTERED INDEX [CX_SSN]
    ON [dbo].[Prereleased_Accounts]([BF_SSN] ASC);


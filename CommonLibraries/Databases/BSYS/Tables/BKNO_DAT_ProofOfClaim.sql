CREATE TABLE [dbo].[BKNO_DAT_ProofOfClaim] (
    [Index]      INT          IDENTITY (1, 1) NOT NULL,
    [Debtor]     VARCHAR (50) NULL,
    [Debtor2]   VARCHAR(50)    NULL,
    [Court]      VARCHAR (100) NULL,
    [State]     VARCHAR (50) NULL,
    [CaseNumber] VARCHAR(50)     NULL,
	[SendTo] VARCHAR(100) null,
    [AccountNumber] CHAR (10)    NULL,
    [ClaimAmount]  VARCHAR (10) NULL,
    CONSTRAINT [PK_BKNO_DAT_ProofOfClaim] PRIMARY KEY CLUSTERED ([Index] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_BKNO_DAT_ProofOfClaim]
    ON [dbo].[BKNO_DAT_ProofOfClaim]([Index] ASC);


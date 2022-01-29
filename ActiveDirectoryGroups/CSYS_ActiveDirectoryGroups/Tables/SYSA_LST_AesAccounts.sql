CREATE TABLE [dbo].[SYSA_LST_AesAccounts]
(
	[AesAccountId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SqlUserId] INT NOT NULL, 
    [AesAccount] CHAR(10) NOT NULL, 
    [AddedAt] DATETIME NOT NULL, 
    [AddedBy] VARCHAR(25) NOT NULL, 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(25) NULL, 
    CONSTRAINT [FK_SYSA_LST_AesAccounts_SYSA_DAT_Users] FOREIGN KEY ([SqlUserId]) REFERENCES [SYSA_DAT_Users]([SqlUserId])
)

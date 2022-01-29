CREATE TABLE [payhistlpp].[Accounts]
(
    [AccountsId] INT NOT NULL PRIMARY KEY IDENTITY,
	[Ssn] VARCHAR(9) NOT NULL,
    [Lender] VARCHAR(6) NOT NULL,
    [UserAccessId] INT NOT NULL,
    [RunId] INT NOT NULL,
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT USER_NAME(), 
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [ProcessedAt] DATETIME NULL, 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(50) NULL, 
    CONSTRAINT [FK_Accounts_UserAccess] FOREIGN KEY ([UserAccessId]) REFERENCES payhistlpp.UserAccess([UserAccessId]), 
    CONSTRAINT [FK_Accounts_Run] FOREIGN KEY ([RunId]) REFERENCES payhistlpp.Run([RunId])
)

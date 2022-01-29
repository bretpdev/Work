CREATE TABLE [tilpcrost].[Accounts]
(
	[AccountsId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AccountNumber] VARCHAR(10) NOT NULL, 
    [Ssn] VARCHAR(9) NOT NULL, 
    [TransactionType] VARCHAR(10) NOT NULL, 
    [LoanSequence] INT NOT NULL, 
    [PrincipalAmount] VARCHAR(10) NOT NULL, 
    [TransactionDate] DATETIME NOT NULL, 
    [LastName] VARCHAR(50) NOT NULL, 
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(), 
    [ProcessedAt] DATETIME NULL , 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] VARCHAR(50) NULL
)

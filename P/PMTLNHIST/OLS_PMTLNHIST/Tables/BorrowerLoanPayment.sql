CREATE TABLE [pmtlnhist].[BorrowerLoanPayment]
(
	[BorrowerLoanPaymentId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AccountNumber] VARCHAR(10) NOT NULL, 
    [Name] VARCHAR(50) NOT NULL,
    [Principal] FLOAT NOT NULL DEFAULT 0, 
    [Interest] FLOAT NOT NULL DEFAULT 0, 
    [LegalCosts] FLOAT NOT NULL DEFAULT 0, 
    [OtherCosts] FLOAT NOT NULL DEFAULT 0, 
    [CollectionCosts] FLOAT NOT NULL DEFAULT 0, 
    [ProjectedCollectionCosts] FLOAT NOT NULL DEFAULT 0, 
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT USER_NAME(), 
    [DeletedAt] DATETIME NULL, 
    [DeletedBy] NCHAR(10) NULL
)
